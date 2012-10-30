using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace cocos2d
{
    public class CCMotionStreak : CCNode, ICCTextureProtocol, ICCRGBAProtocol
    {
        protected bool m_bFastMode;
        protected bool m_bStartingPositionInitialized;
        private float m_fFadeDelta;
        private float m_fMinSeg;
        private float m_fStroke;
        private float[] m_pPointState;
        private CCPoint[] m_pPointVertexes;
        /** texture used for the motion streak */
        private CCTexture2D m_pTexture;
        private ccV3F_C4B_T2F[] m_pVertices;
        private ccBlendFunc m_tBlendFunc;
        private ccColor3B m_tColor;
        private CCPoint m_tPositionR;

        private int m_uMaxPoints;
        private int m_uNuPoints;
        private int m_uPreviousNuPoints;

        /** Pointers */

        public CCMotionStreak()
        {
            m_tBlendFunc = new ccBlendFunc(OGLES.GL_SRC_ALPHA, OGLES.GL_ONE_MINUS_SRC_ALPHA);
        }

        public override CCPoint Position
        {
            set
            {
                m_bStartingPositionInitialized = true;
                m_tPositionR = value;
            }
        }

        #region ICCRGBAProtocol Members

        public ccColor3B Color
        {
            set { m_tColor = value; }
            get { return m_tColor; }
        }

        public byte Opacity
        {
            get { return 0; }
            set { }
        }

        public bool IsOpacityModifyRGB
        {
            get { return false; }
            set { }
        }

        #endregion

        #region ICCTextureProtocol Members

        public CCTexture2D Texture
        {
            get { return m_pTexture; }
            set { m_pTexture = value; }
        }

        public ccBlendFunc BlendFunc
        {
            set { m_tBlendFunc = value; }
            get { return (m_tBlendFunc); }
        }

        #endregion

        public bool FastMode
        {
            get { return m_bFastMode; }
            set { m_bFastMode = value; }
        }

        public bool StartingPositionInitialized
        {
            get { return m_bStartingPositionInitialized; }
            set { m_bStartingPositionInitialized = value; }
        }

        public static CCMotionStreak Create(float fade, float minSeg, float stroke, ccColor3B color, string path)
        {
            var pRet = new CCMotionStreak();
            pRet.InitWithFade(fade, minSeg, stroke, color, path);
            return pRet;
        }

        public static CCMotionStreak Create(float fade, float minSeg, float stroke, ccColor3B color, CCTexture2D texture)
        {
            var pRet = new CCMotionStreak();
            pRet.InitWithFade(fade, minSeg, stroke, color, texture);
            return pRet;
        }

        public bool InitWithFade(float fade, float minSeg, float stroke, ccColor3B color, string path)
        {
            Debug.Assert(!String.IsNullOrEmpty(path), "Invalid filename");

            CCTexture2D texture = CCTextureCache.SharedTextureCache.AddImage(path);
            return InitWithFade(fade, minSeg, stroke, color, texture);
        }

        public bool InitWithFade(float fade, float minSeg, float stroke, ccColor3B color, CCTexture2D texture)
        {
            Position = CCPoint.Zero;
            AnchorPoint = CCPoint.Zero;
            IgnoreAnchorPointForPosition = true;
            m_bStartingPositionInitialized = false;

            m_tPositionR = CCPoint.Zero;
            m_bFastMode = true;
            m_fMinSeg = (minSeg == -1.0f) ? stroke / 5.0f : minSeg;
            m_fMinSeg *= m_fMinSeg;

            m_fStroke = stroke;
            m_fFadeDelta = 1.0f / fade;

            m_uMaxPoints = (int) (fade * 60.0f) + 2;
            m_uNuPoints = 0;
            m_pPointState = new float[m_uMaxPoints];
            m_pPointVertexes = new CCPoint[m_uMaxPoints];

            m_pVertices = new ccV3F_C4B_T2F[(m_uMaxPoints + 1) * 2];

            // Set blend mode
            m_tBlendFunc.src = OGLES.GL_SRC_ALPHA;
            m_tBlendFunc.dst = OGLES.GL_ONE_MINUS_SRC_ALPHA;

            // shader program
            // setShaderProgram(CCShaderCache.sharedShaderCache().programForKey(kCCShader_PositionTextureColor));

            Texture = texture;
            Color = color;
            ScheduleUpdate();

            return true;
        }

        public void TintWithColor(ccColor3B colors)
        {
            Color = colors;

            for (int i = 0; i < m_uNuPoints * 2; i++)
            {
                m_pVertices[i].colors = new ccColor4B(colors.r, colors.g, colors.b, 255);
            }
        }

        public override void Update(float delta)
        {
            if (!m_bStartingPositionInitialized)
            {
                return;
            }

            delta *= m_fFadeDelta;

            int newIdx, newIdx2, i, i2;
            int mov = 0;

            // Update current points
            for (i = 0; i < m_uNuPoints; i++)
            {
                m_pPointState[i] -= delta;

                if (m_pPointState[i] <= 0)
                {
                    mov++;
                }
                else
                {
                    newIdx = i - mov;

                    if (mov > 0)
                    {
                        // Move data
                        m_pPointState[newIdx] = m_pPointState[i];

                        // Move point
                        m_pPointVertexes[newIdx] = m_pPointVertexes[i];

                        // Move vertices
                        i2 = i * 2;
                        newIdx2 = newIdx * 2;
                        m_pVertices[newIdx2].vertices = m_pVertices[i2].vertices;
                        m_pVertices[newIdx2 + 1].vertices = m_pVertices[i2 + 1].vertices;

                        // Move color
                        m_pVertices[newIdx2].colors = m_pVertices[i2].colors;
                        m_pVertices[newIdx2 + 1].colors = m_pVertices[i2 + 1].colors;
                    }
                    else
                    {
                        newIdx2 = newIdx * 2;
                    }

                    m_pVertices[newIdx2].colors.a = m_pVertices[newIdx2 + 1].colors.a = (byte) (m_pPointState[newIdx] * 255.0f);
                }
            }
            m_uNuPoints -= mov;

            // Append new point
            bool appendNewPoint = true;
            if (m_uNuPoints >= m_uMaxPoints)
            {
                appendNewPoint = false;
            }

            else if (m_uNuPoints > 0)
            {
                bool a1 = m_pPointVertexes[m_uNuPoints - 1].DistanceSQ(ref m_tPositionR) < m_fMinSeg;
                bool a2 = (m_uNuPoints != 1) && (m_pPointVertexes[m_uNuPoints - 2].DistanceSQ(ref m_tPositionR) < (m_fMinSeg * 2.0f));

                if (a1 || a2)
                {
                    appendNewPoint = false;
                }
            }

            if (appendNewPoint)
            {
                m_pPointVertexes[m_uNuPoints] = m_tPositionR;
                m_pPointState[m_uNuPoints] = 1.0f;

                // Color asignation
                int offset = m_uNuPoints * 2;
                m_pVertices[offset].colors = m_pVertices[offset + 1].colors = new ccColor4B(m_tColor.r, m_tColor.g, m_tColor.b, 255);

                // Generate polygon
                if (m_uNuPoints > 0 && m_bFastMode)
                {
                    if (m_uNuPoints > 1)
                    {
                        VertexLineToPolygon(m_pPointVertexes, m_fStroke, m_pVertices, m_uNuPoints, 1);
                    }
                    else
                    {
                        VertexLineToPolygon(m_pPointVertexes, m_fStroke, m_pVertices, 0, 2);
                    }
                }

                m_uNuPoints++;
            }

            if (!m_bFastMode)
            {
                VertexLineToPolygon(m_pPointVertexes, m_fStroke, m_pVertices, 0, m_uNuPoints);
            }

            // Updated Tex Coords only if they are different than previous step
            if (m_uNuPoints > 0 && m_uPreviousNuPoints != m_uNuPoints)
            {
                float texDelta = 1.0f / m_uNuPoints;
                for (i = 0; i < m_uNuPoints; i++)
                {
                    m_pVertices[i * 2].texCoords = new ccTex2F(0, texDelta * i);
                    m_pVertices[i * 2 + 1].texCoords = new ccTex2F(1, texDelta * i);
                }

                m_uPreviousNuPoints = m_uNuPoints;
            }
        }


        private void VertexLineToPolygon(CCPoint[] points, float stroke, ccV3F_C4B_T2F[] vertices, int offset, int nuPoints)
        {
            nuPoints += offset;
            if (nuPoints <= 1) return;

            stroke *= 0.5f;

            int idx;
            int nuPointsMinus = nuPoints - 1;

            float rad70 = MathHelper.ToRadians(70);
            float rad170 = MathHelper.ToRadians(170);

            for (int i = offset; i < nuPoints; i++)
            {
                idx = i * 2;
                CCPoint p1 = points[i];
                CCPoint perpVector;

                if (i == 0)
                {
                    perpVector = CCPoint.Perp(CCPoint.Normalize(p1 - points[i + 1]));
                }
                else if (i == nuPointsMinus)
                {
                    perpVector = CCPoint.Perp(CCPoint.Normalize(points[i - 1] - p1));
                }
                else
                {
                    CCPoint p2 = points[i + 1];
                    CCPoint p0 = points[i - 1];

                    CCPoint p2p1 = CCPoint.Normalize(p2 - p1);
                    CCPoint p0p1 = CCPoint.Normalize(p0 - p1);

                    // Calculate angle between vectors
                    var angle = (float) Math.Acos(CCPoint.Dot(p2p1, p0p1));

                    if (angle < rad70)
                    {
                        perpVector = CCPoint.Perp(CCPoint.Normalize(CCPoint.Midpoint(p2p1, p0p1)));
                    }
                    else if (angle < rad170)
                    {
                        perpVector = CCPoint.Normalize(CCPoint.Midpoint(p2p1, p0p1));
                    }
                    else
                    {
                        perpVector = CCPoint.Perp(CCPoint.Normalize(p2 - p0));
                    }
                }

                perpVector = perpVector * stroke;

                vertices[idx].vertices = new ccVertex3F(p1.x + perpVector.x, p1.y + perpVector.y, 0);
                vertices[idx + 1].vertices = new ccVertex3F(p1.x - perpVector.x, p1.y - perpVector.y, 0);
            }

            // Validate vertexes
            offset = (offset == 0) ? 0 : offset - 1;
            for (int i = offset; i < nuPointsMinus; i++)
            {
                idx = i * 2;
                int idx1 = idx + 2;

                ccVertex3F p1 = vertices[idx].vertices;
                ccVertex3F p2 = vertices[idx + 1].vertices;
                ccVertex3F p3 = vertices[idx1].vertices;
                ccVertex3F p4 = vertices[idx1 + 1].vertices;

                float s;
                bool fixVertex = !ccVertexLineIntersect(p1.x, p1.y, p4.x, p4.y, p2.x, p2.y, p3.x, p3.y, out s);
                if (!fixVertex)
                {
                    if (s < 0.0f || s > 1.0f)
                    {
                        fixVertex = true;
                    }
                }

                if (fixVertex)
                {
                    vertices[idx1].vertices = p4;
                    vertices[idx1 + 1].vertices = p3;
                }
            }
        }

        private bool ccVertexLineIntersect(float Ax, float Ay, float Bx, float By, float Cx, float Cy, float Dx, float Dy, out float T)
        {
            float distAB, theCos, theSin, newX;

            T = 0;

            // FAIL: Line undefined
            if ((Ax == Bx && Ay == By) || (Cx == Dx && Cy == Dy)) return false;

            //  Translate system to make A the origin
            Bx -= Ax;
            By -= Ay;
            Cx -= Ax;
            Cy -= Ay;
            Dx -= Ax;
            Dy -= Ay;

            // Length of segment AB
            distAB = (float) Math.Sqrt(Bx * Bx + By * By);

            // Rotate the system so that point B is on the positive X axis.
            theCos = Bx / distAB;
            theSin = By / distAB;
            newX = Cx * theCos + Cy * theSin;
            Cy = Cy * theCos - Cx * theSin;
            Cx = newX;
            newX = Dx * theCos + Dy * theSin;
            Dy = Dy * theCos - Dx * theSin;
            Dx = newX;

            // FAIL: Lines are parallel.
            if (Cy == Dy) return false;

            // Discover the relative position of the intersection in the line AB
            T = (Dx + (Cx - Dx) * Dy / (Dy - Cy)) / distAB;

            // Success.
            return true;
        }

        private void Reset()
        {
            m_uNuPoints = 0;
        }

        public override void Draw()
        {
            DrawManager.BlendFunc(m_tBlendFunc);
            DrawManager.BindTexture(m_pTexture);

            DrawManager.DrawPrimitives(PrimitiveType.TriangleStrip, m_pVertices, 0, m_uNuPoints * 2 - 2);
        }
    }
}