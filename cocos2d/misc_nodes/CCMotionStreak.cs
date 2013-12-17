using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CocosSharp
{
    public class CCMotionStreak : CCNodeRGBA, ICCTexture
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
        private CCV3F_C4B_T2F[] m_pVertices;
        private CCBlendFunc m_tBlendFunc;
        private CCPoint m_tPositionR;

        private int m_uMaxPoints;
        private int m_uNuPoints;
        private int m_uPreviousNuPoints;

        /** Pointers */

        public CCMotionStreak()
        {
            m_tBlendFunc = CCBlendFunc.NonPremultiplied;
        }

        public override CCPoint Position
        {
            set
            {
                m_bStartingPositionInitialized = true;
                m_tPositionR = value;
            }
        }

        #region RGBA Protocol

        public override byte Opacity
        {
            get { return 0; }
            set { }
        }

        public override bool IsOpacityModifyRGB
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

        public CCBlendFunc BlendFunc
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

        public static CCMotionStreak Create(float fade, float minSeg, float stroke, CCColor3B color, string path)
        {
            var pRet = new CCMotionStreak();
            pRet.InitWithFade(fade, minSeg, stroke, color, path);
            return pRet;
        }

        public static CCMotionStreak Create(float fade, float minSeg, float stroke, CCColor3B color, CCTexture2D texture)
        {
            var pRet = new CCMotionStreak();
            pRet.InitWithFade(fade, minSeg, stroke, color, texture);
            return pRet;
        }

        public bool InitWithFade(float fade, float minSeg, float stroke, CCColor3B color, string path)
        {
            Debug.Assert(!String.IsNullOrEmpty(path), "Invalid filename");

            CCTexture2D texture = CCTextureCache.SharedTextureCache.AddImage(path);
            return InitWithFade(fade, minSeg, stroke, color, texture);
        }

        public bool InitWithFade(float fade, float minSeg, float stroke, CCColor3B color, CCTexture2D texture)
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

            m_pVertices = new CCV3F_C4B_T2F[(m_uMaxPoints + 1) * 2];

            // Set blend mode
            m_tBlendFunc = CCBlendFunc.NonPremultiplied;

            Texture = texture;
            Color = color;
            ScheduleUpdate();

            return true;
        }

        public void TintWithColor(CCColor3B colors)
        {
            Color = colors;

            for (int i = 0; i < m_uNuPoints * 2; i++)
            {
                m_pVertices[i].Colors = new CCColor4B(colors.R, colors.G, colors.B, 255);
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
                        m_pVertices[newIdx2].Vertices = m_pVertices[i2].Vertices;
                        m_pVertices[newIdx2 + 1].Vertices = m_pVertices[i2 + 1].Vertices;

                        // Move color
                        m_pVertices[newIdx2].Colors = m_pVertices[i2].Colors;
                        m_pVertices[newIdx2 + 1].Colors = m_pVertices[i2 + 1].Colors;
                    }
                    else
                    {
                        newIdx2 = newIdx * 2;
                    }

                    m_pVertices[newIdx2].Colors.A = m_pVertices[newIdx2 + 1].Colors.A = (byte) (m_pPointState[newIdx] * 255.0f);
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
                m_pVertices[offset].Colors = m_pVertices[offset + 1].Colors = new CCColor4B(_displayedColor.R, _displayedColor.G, _displayedColor.B, 255);

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
                    m_pVertices[i * 2].TexCoords = new CCTex2F(0, texDelta * i);
                    m_pVertices[i * 2 + 1].TexCoords = new CCTex2F(1, texDelta * i);
                }

                m_uPreviousNuPoints = m_uNuPoints;
            }
        }


        private void VertexLineToPolygon(CCPoint[] points, float stroke, CCV3F_C4B_T2F[] vertices, int offset, int nuPoints)
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

                vertices[idx].Vertices = new CCVertex3F(p1.X + perpVector.X, p1.Y + perpVector.Y, 0);
                vertices[idx + 1].Vertices = new CCVertex3F(p1.X - perpVector.X, p1.Y - perpVector.Y, 0);
            }

            // Validate vertexes
            offset = (offset == 0) ? 0 : offset - 1;
            for (int i = offset; i < nuPointsMinus; i++)
            {
                idx = i * 2;
                int idx1 = idx + 2;

                CCVertex3F p1 = vertices[idx].Vertices;
                CCVertex3F p2 = vertices[idx + 1].Vertices;
                CCVertex3F p3 = vertices[idx1].Vertices;
                CCVertex3F p4 = vertices[idx1 + 1].Vertices;

                float s;
                bool fixVertex = !ccVertexLineIntersect(p1.X, p1.Y, p4.X, p4.Y, p2.X, p2.Y, p3.X, p3.Y, out s);
                if (!fixVertex)
                {
                    if (s < 0.0f || s > 1.0f)
                    {
                        fixVertex = true;
                    }
                }

                if (fixVertex)
                {
                    vertices[idx1].Vertices = p4;
                    vertices[idx1 + 1].Vertices = p3;
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
            CCDrawManager.BlendFunc(m_tBlendFunc);
            CCDrawManager.BindTexture(m_pTexture);
            CCDrawManager.VertexColorEnabled = true;
            CCDrawManager.DrawPrimitives(PrimitiveType.TriangleStrip, m_pVertices, 0, m_uNuPoints * 2 - 2);
        }
    }
}