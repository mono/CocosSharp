using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CocosSharp
{
    public class CCMotionStreak : CCNodeRGBA, ICCTexture
    {
        // ivars
        int maxPoints;
        int numOfPoints;
        int previousNumOfPoints;

        float fadeDelta;
        float minSeg;
        float stroke;
        float[] pointState;

        CCPoint positionR;
        CCPoint[] pointVertexes;
        CCV3F_C4B_T2F[] vertices;


        #region Properties

        public CCTexture2D Texture { get; set; }
        public CCBlendFunc BlendFunc { get; set; }
        public bool FastMode { get; set; }
        public bool StartingPositionInitialized { get; private set; }

        public override CCPoint Position
        {
            set
            {
                StartingPositionInitialized = true;
                positionR = value;
            }
        }
            
        public override byte Opacity
        {
            get { return 0; }
            set { }
        }

        public override bool IsColorModifiedByOpacity
        {
            get { return false; }
            set { }
        }

        #endregion Properties


        #region Constructors

        public CCMotionStreak()
        {
            BlendFunc = CCBlendFunc.NonPremultiplied;
        }

        public CCMotionStreak(float fade, float minSeg, float stroke, CCColor3B color, string path) 
            : this(fade, minSeg, stroke, color, CCTextureCache.SharedTextureCache.AddImage(path))
        {
        }

        public CCMotionStreak(float fade, float minSegIn, float strokeIn, CCColor3B color, CCTexture2D texture)
        {
            AnchorPoint = CCPoint.Zero;
            IgnoreAnchorPointForPosition = true;
            StartingPositionInitialized = false;
            FastMode = true;
            Texture = texture;
            Color = color;
            stroke = strokeIn;
            BlendFunc = CCBlendFunc.NonPremultiplied;

            minSeg = (minSegIn == -1.0f) ? stroke / 5.0f : minSegIn;
            minSeg *= minSeg;

            fadeDelta = 1.0f / fade;

            maxPoints = (int) (fade * 60.0f) + 2;

            pointState = new float[maxPoints];
            pointVertexes = new CCPoint[maxPoints];
            vertices = new CCV3F_C4B_T2F[(maxPoints + 1) * 2];

            ScheduleUpdate();
        }

        #endregion Constructors


        #region Drawing

        protected override void Draw()
        {
            CCDrawManager.BlendFunc(BlendFunc);
            CCDrawManager.BindTexture(Texture);
            CCDrawManager.VertexColorEnabled = true;
            CCDrawManager.DrawPrimitives(PrimitiveType.TriangleStrip, vertices, 0, numOfPoints * 2 - 2);
        }

        #endregion Drawing


        #region Updating

        static bool VertexLineIntersect(float Ax, float Ay, float Bx, float By, float Cx, 
            float Cy, float Dx, float Dy, out float T)
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

        static void VertexLineToPolygon(CCPoint[] points, float stroke, CCV3F_C4B_T2F[] vertices, int offset, int nuPoints)
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
                bool fixVertex = !VertexLineIntersect(p1.X, p1.Y, p4.X, p4.Y, p2.X, p2.Y, p3.X, p3.Y, out s);
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

        void Reset()
        {
            numOfPoints = 0;
        }

        public void TintWithColor(CCColor3B colors)
        {
            Color = colors;

            for (int i = 0; i < numOfPoints * 2; i++)
            {
                vertices[i].Colors = new CCColor4B(colors.R, colors.G, colors.B, 255);
            }
        }

        public override void Update(float delta)
        {
            if (!StartingPositionInitialized)
            {
                return;
            }

            delta *= fadeDelta;

            int newIdx, newIdx2, i, i2;
            int mov = 0;

            // Update current points
            for (i = 0; i < numOfPoints; i++)
            {
                pointState[i] -= delta;

                if (pointState[i] <= 0)
                {
                    mov++;
                }
                else
                {
                    newIdx = i - mov;

                    if (mov > 0)
                    {
                        // Move data
                        pointState[newIdx] = pointState[i];

                        // Move point
                        pointVertexes[newIdx] = pointVertexes[i];

                        // Move vertices
                        i2 = i * 2;
                        newIdx2 = newIdx * 2;
                        vertices[newIdx2].Vertices = vertices[i2].Vertices;
                        vertices[newIdx2 + 1].Vertices = vertices[i2 + 1].Vertices;

                        // Move color
                        vertices[newIdx2].Colors = vertices[i2].Colors;
                        vertices[newIdx2 + 1].Colors = vertices[i2 + 1].Colors;
                    }
                    else
                    {
                        newIdx2 = newIdx * 2;
                    }

                    vertices[newIdx2].Colors.A = vertices[newIdx2 + 1].Colors.A = (byte) (pointState[newIdx] * 255.0f);
                }
            }

            numOfPoints -= mov;

            // Append new point
            bool appendNewPoint = true;
            if (numOfPoints >= maxPoints)
            {
                appendNewPoint = false;
            }

            else if (numOfPoints > 0)
            {
                bool a1 = pointVertexes[numOfPoints - 1].DistanceSQ(ref positionR) < minSeg;
                bool a2 = (numOfPoints != 1) && (pointVertexes[numOfPoints - 2].DistanceSQ(ref positionR) < (minSeg * 2.0f));

                if (a1 || a2)
                {
                    appendNewPoint = false;
                }
            }

            if (appendNewPoint)
            {
                pointVertexes[numOfPoints] = positionR;
                pointState[numOfPoints] = 1.0f;

                // Color asignation
                int offset = numOfPoints * 2;
                vertices[offset].Colors = vertices[offset + 1].Colors = new CCColor4B(_displayedColor.R, _displayedColor.G, _displayedColor.B, 255);

                // Generate polygon
                if (numOfPoints > 0 && FastMode)
                {
                    if (numOfPoints > 1)
                    {
                        VertexLineToPolygon(pointVertexes, stroke, vertices, numOfPoints, 1);
                    }
                    else
                    {
                        VertexLineToPolygon(pointVertexes, stroke, vertices, 0, 2);
                    }
                }

                numOfPoints++;
            }

            if (!FastMode)
            {
                VertexLineToPolygon(pointVertexes, stroke, vertices, 0, numOfPoints);
            }

            // Updated Tex Coords only if they are different than previous step
            if (numOfPoints > 0 && previousNumOfPoints != numOfPoints)
            {
                float texDelta = 1.0f / numOfPoints;
                for (i = 0; i < numOfPoints; i++)
                {
                    vertices[i * 2].TexCoords = new CCTex2F(0, texDelta * i);
                    vertices[i * 2 + 1].TexCoords = new CCTex2F(1, texDelta * i);
                }

                previousNumOfPoints = numOfPoints;
            }
        }

        #endregion Updating
    }
}