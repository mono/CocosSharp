using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CocosSharp
{
    public enum CCProgressTimerType
    {
        Radial, // Radial Counter-Clockwise
        Bar,    // Bar
    }

/**
 @brief CCProgresstimer is a subclass of CCNode.
 It renders the inner sprite according to the percentage.
 The progress can be Radial, Horizontal or vertical.
 @since v0.99.1
 */

    public class CCProgressTimer : CCNodeRGBA
    {
        const int ProgressTextureCoordsCount = 4;
        const uint ProgressTextureCoords = 0x4b;    // ProgressTextureCoords holds points {0,1} {0,0} {1,0} {1,1} we can represent it as bits

        // ivars
        bool reverseDirection;
        CCProgressTimerType type;
        float percentage;
        CCSprite sprite;
        CCPoint midpoint;

        CCV3F_C4B_T2F[] vertexData;
        short[] vertexIndices;


        #region Properties

        public CCPoint BarChangeRate { get; set; }

        public CCProgressTimerType Type
        {
            get { return type; }
            set
            {
                if (value != type)
                {
                    type = value;

                    vertexData = null;
                    RefreshVertexIndices();
                }
            }
        }

        public float Percentage
        {
            get { return percentage; }
            set
            {
                if (percentage != value)
                {
                    percentage = MathHelper.Clamp(value, 0, 100);
                    UpdateProgress();
                }
            }
        }

        public CCSprite Sprite
        {
            get { return sprite; }
            set
            {
                if (sprite != value)
                {
                    sprite = value;
                    ContentSize = value.ContentSize;

                    vertexData = null;
                }
            }
        }
        
        public bool ReverseDirection
        {
            get { return reverseDirection; }
            set
            {
                if (reverseDirection != value)
                {
                    reverseDirection = value;

                    vertexData = null;
                }
            }
        }

        public CCPoint Midpoint
        {
            get { return midpoint; }
            set
            {
                midpoint.X = MathHelper.Clamp(value.X, 0, 1);
                midpoint.Y = MathHelper.Clamp(value.Y, 0, 1);
            }
        }

        // Overriden properties

        public override CCColor3B Color
        {
            get { return Sprite.Color; }
            set
            {
                Sprite.Color = value;
                UpdateColor();
            }
        }

        public override byte Opacity
        {
            get { return Sprite.Opacity; }
            set
            {
                Sprite.Opacity = value;
                UpdateColor();
            }
        }

        public override bool IsColorModifiedByOpacity
        {
            get { return false; }
            set { }
        }

        #endregion Properties


        #region Constructors

        public CCProgressTimer(string fileName) : this(new CCSprite(fileName))
        {
        }

        public CCProgressTimer(CCSprite sp)
        {
            AnchorPoint = new CCPoint(0.5f, 0.5f);
            Type = CCProgressTimerType.Radial;
            Midpoint = new CCPoint(0.5f, 0.5f);
            BarChangeRate = new CCPoint(1, 1);
            Sprite = sp;
            UpdateProgress();
        }

        #endregion Constructors


        #region Drawing

        protected override void Draw()
        {
            if (vertexData != null && sprite != null) 
            {
                Window.DrawManager.BindTexture(Sprite.Texture);
                Window.DrawManager.BlendFunc(Sprite.BlendFunc);

                Window.DrawManager.DrawIndexedPrimitives(PrimitiveType.TriangleList, vertexData, 0, 
                    vertexData.Length, vertexIndices, 0, vertexData.Length - 2);
            }
        }

        #endregion Drawing


        #region Vertex data

        CCTex2F TextureCoordFromAlphaPoint(CCPoint alpha)
        {
            CCTex2F ret = new CCTex2F(0.0f, 0.0f);

            if (Sprite != null) 
            {
                CCV3F_C4B_T2F_Quad quad = Sprite.Quad;

                CCPoint min = new CCPoint (quad.BottomLeft.TexCoords.U, quad.BottomLeft.TexCoords.V);
                CCPoint max = new CCPoint (quad.TopRight.TexCoords.U, quad.TopRight.TexCoords.V);

                //  Fix bug #1303 so that progress timer handles sprite frame texture rotation
                if (Sprite.IsTextureRectRotated) 
                {
                    float tmp = alpha.X;
                    alpha.X = alpha.Y;
                    alpha.Y = tmp;
                }

                ret = new CCTex2F(min.X * (1f - alpha.X) + max.X * alpha.X, min.Y * (1f - alpha.Y) + max.Y * alpha.Y);
            }

            return ret;
        }

        CCVertex3F VertexFromAlphaPoint(CCPoint alpha)
        {
            CCVertex3F ret = new CCVertex3F(0.0f, 0.0f, 0.0f);

            if (Sprite != null) 
            {
                CCV3F_C4B_T2F_Quad quad = Sprite.Quad;

                CCPoint min = new CCPoint(quad.BottomLeft.Vertices.X, quad.BottomLeft.Vertices.Y);
                CCPoint max = new CCPoint(quad.TopRight.Vertices.X, quad.TopRight.Vertices.Y);

                ret.X = min.X * (1f - alpha.X) + max.X * alpha.X;
                ret.Y = min.Y * (1f - alpha.Y) + max.Y * alpha.Y;
            }

            return ret;
        }

        CCPoint BoundaryTexCoord(int index)
        {
            if (index < ProgressTextureCoordsCount)
            {
                if (ReverseDirection)
                {
                    return new CCPoint((ProgressTextureCoords >> (7 - (index << 1))) & 1,
                        (ProgressTextureCoords >> (7 - ((index << 1) + 1))) & 1);
                }
                return new CCPoint((ProgressTextureCoords >> ((index << 1) + 1)) & 1,
                    (ProgressTextureCoords >> (index << 1)) & 1);
            }
            return CCPoint.Zero;
        }

        void UpdateProgress()
        {
            switch(Type)
            {
                case CCProgressTimerType.Radial:
                    UpdateRadial();
                    RefreshVertexIndices();
                    UpdateColor();
                    break;
                case CCProgressTimerType.Bar:
                    UpdateBar();
                    RefreshVertexIndices();
                    UpdateColor();
                    break;
                default:
                    break;
            }
        }

        void RefreshVertexIndices()
        {
            if (vertexData != null) 
            {
                int count = (vertexData.Length - 2);
                int i3;
                vertexIndices = new short[count * 3];

                if (Type == CCProgressTimerType.Radial) 
                {
                    // Fan
                    for (int i = 0; i < count; i++) 
                    {
                        i3 = i * 3;
                        vertexIndices [i3 + 0] = 0;
                        vertexIndices [i3 + 1] = (short)(i + 1);
                        vertexIndices [i3 + 2] = (short)(i + 2);
                    }
                } 
                else if (Type == CCProgressTimerType.Bar) 
                {
                    // Triangle strip
                    for (int i = 0; i < count; i++) 
                    {
                        i3 = i * 3;
                        vertexIndices [i3 + 0] = (short)(i + 0);
                        vertexIndices [i3 + 1] = (short)(i + 1);
                        vertexIndices [i3 + 2] = (short)(i + 2);
                    }
                }
            }
        }

        void UpdateColor()
        {
            if (Sprite != null && vertexData != null)
            {
                CCColor4B sc = Sprite.Quad.TopLeft.Colors;
                for (int i = 0; i < vertexData.Length; ++i)
                {
                    vertexData[i].Colors = sc;
                }
            }
        }

        void UpdateBar()
        {
            if (Sprite == null)
            {
                return;
            }

            float alpha = Percentage / 100.0f;
            CCPoint alphaOffset = new CCPoint(1.0f * (1.0f - BarChangeRate.X) + alpha * BarChangeRate.X, 
                1.0f * (1.0f - BarChangeRate.Y) + alpha * BarChangeRate.Y) * 0.5f;
            CCPoint min = Midpoint - alphaOffset;
            CCPoint max = Midpoint + alphaOffset;

            if (min.X < 0f)
            {
                max.X += -min.X;
                min.X = 0f;
            }

            if (max.X > 1f)
            {
                min.X -= max.X - 1f;
                max.X = 1f;
            }

            if (min.Y < 0f)
            {
                max.Y += -min.Y;
                min.Y = 0f;
            }

            if (max.Y > 1f)
            {
                min.Y -= max.Y - 1f;
                max.Y = 1f;
            }


            if (!ReverseDirection)
            {
                if (vertexData == null)
                {
                    vertexData = new CCV3F_C4B_T2F[4];
                }
                //    TOPLEFT
                vertexData[0].TexCoords = TextureCoordFromAlphaPoint(new CCPoint(min.X, max.Y));
                vertexData[0].Vertices = VertexFromAlphaPoint(new CCPoint(min.X, max.Y));

                //    BOTLEFT
                vertexData[1].TexCoords = TextureCoordFromAlphaPoint(new CCPoint(min.X, min.Y));
                vertexData[1].Vertices = VertexFromAlphaPoint(new CCPoint(min.X, min.Y));

                //    TOPRIGHT
                vertexData[2].TexCoords = TextureCoordFromAlphaPoint(new CCPoint(max.X, max.Y));
                vertexData[2].Vertices = VertexFromAlphaPoint(new CCPoint(max.X, max.Y));

                //    BOTRIGHT
                vertexData[3].TexCoords = TextureCoordFromAlphaPoint(new CCPoint(max.X, min.Y));
                vertexData[3].Vertices = VertexFromAlphaPoint(new CCPoint(max.X, min.Y));
            }
            else
            {
                if (vertexData == null)
                {
                    vertexData = new CCV3F_C4B_T2F[8];

                    // TOPLEFT 1
                    vertexData[0].TexCoords = TextureCoordFromAlphaPoint(new CCPoint(0, 1));
                    vertexData[0].Vertices = VertexFromAlphaPoint(new CCPoint(0, 1));

                    // BOTLEFT 1
                    vertexData[1].TexCoords = TextureCoordFromAlphaPoint(new CCPoint(0, 0));
                    vertexData[1].Vertices = VertexFromAlphaPoint(new CCPoint(0, 0));

                    // TOPRIGHT 2
                    vertexData[6].TexCoords = TextureCoordFromAlphaPoint(new CCPoint(1, 1));
                    vertexData[6].Vertices = VertexFromAlphaPoint(new CCPoint(1, 1));

                    // BOTRIGHT 2
                    vertexData[7].TexCoords = TextureCoordFromAlphaPoint(new CCPoint(1, 0));
                    vertexData[7].Vertices = VertexFromAlphaPoint(new CCPoint(1, 0));
                }

                // TOPRIGHT 1
                vertexData[2].TexCoords = TextureCoordFromAlphaPoint(new CCPoint(min.X, max.Y));
                vertexData[2].Vertices = VertexFromAlphaPoint(new CCPoint(min.X, max.Y));

                // BOTRIGHT 1
                vertexData[3].TexCoords = TextureCoordFromAlphaPoint(new CCPoint(min.X, min.Y));
                vertexData[3].Vertices = VertexFromAlphaPoint(new CCPoint(min.X, min.Y));

                // TOPLEFT 2
                vertexData[4].TexCoords = TextureCoordFromAlphaPoint(new CCPoint(max.X, max.Y));
                vertexData[4].Vertices = VertexFromAlphaPoint(new CCPoint(max.X, max.Y));

                // BOTLEFT 2
                vertexData[5].TexCoords = TextureCoordFromAlphaPoint(new CCPoint(max.X, min.Y));
                vertexData[5].Vertices = VertexFromAlphaPoint(new CCPoint(max.X, min.Y));
            }
        }

        ///
        //    Update does the work of mapping the texture onto the triangles
        //    It now doesn't occur the cost of free/alloc data every update cycle.
        //    It also only changes the percentage point but no other points if they have not
        //    been modified.
        //    
        //    It now deals with flipped texture. If you run into this problem, just use the
        //    sprite property and enable the methods flipX, flipY.
        ///
        void UpdateRadial()
        {
            if (Sprite == null)
            {
                return;
            }

            float alpha = Percentage / 100f;

            float angle = 2f * (MathHelper.Pi) * (ReverseDirection ? alpha : 1.0f - alpha);

            //    We find the vector to do a hit detection based on the percentage
            //    We know the first vector is the one @ 12 o'clock (top,mid) so we rotate
            //    from that by the progress angle around the m_tMidpoint pivot
            var topMid = new CCPoint(Midpoint.X, 1f);
            CCPoint percentagePt = CCPoint.RotateByAngle(topMid, Midpoint, angle);


            int index = 0;
            CCPoint hit;

            if (alpha == 0f)
            {
                //    More efficient since we don't always need to check intersection
                //    If the alpha is zero then the hit point is top mid and the index is 0.
                hit = topMid;
                index = 0;
            }
            else if (alpha == 1f)
            {
                //    More efficient since we don't always need to check intersection
                //    If the alpha is one then the hit point is top mid and the index is 4.
                hit = topMid;
                index = 4;
            }
            else
            {
                //    We run a for loop checking the edges of the texture to find the
                //    intersection point
                //    We loop through five points since the top is split in half

                float min_t = float.MaxValue;

                for (int i = 0; i <= ProgressTextureCoordsCount; ++i)
                {
                    int pIndex = (i + (ProgressTextureCoordsCount - 1)) % ProgressTextureCoordsCount;

                    CCPoint edgePtA = BoundaryTexCoord(i % ProgressTextureCoordsCount);
                    CCPoint edgePtB = BoundaryTexCoord(pIndex);

                    //    Remember that the top edge is split in half for the 12 o'clock position
                    //    Let's deal with that here by finding the correct endpoints
                    if (i == 0)
                    {
                        edgePtB = CCPoint.Lerp(edgePtA, edgePtB, 1 - Midpoint.X);
                    }
                    else if (i == 4)
                    {
                        edgePtA = CCPoint.Lerp(edgePtA, edgePtB, 1 - Midpoint.X);
                    }

                    //    s and t are returned by ccpLineIntersect
                    float s = 0, t = 0;
                    if (CCPoint.LineIntersect(edgePtA, edgePtB, Midpoint, percentagePt, ref s, ref t))
                    {
                        //    Since our hit test is on rays we have to deal with the top edge
                        //    being in split in half so we have to test as a segment
                        if ((i == 0 || i == 4))
                        {
                            //    s represents the point between edgePtA--edgePtB
                            if (!(0f <= s && s <= 1f))
                            {
                                continue;
                            }
                        }
                        //    As long as our t isn't negative we are at least finding a
                        //    correct hitpoint from m_tMidpoint to percentagePt.
                        if (t >= 0f)
                        {
                            //    Because the percentage line and all the texture edges are
                            //    rays we should only account for the shortest intersection
                            if (t < min_t)
                            {
                                min_t = t;
                                index = i;
                            }
                        }
                    }
                }

                //    Now that we have the minimum magnitude we can use that to find our intersection
                hit = Midpoint + ((percentagePt - Midpoint) * min_t);
            }


            //    The size of the vertex data is the index from the hitpoint
            //    the 3 is for the m_tMidpoint, 12 o'clock point and hitpoint position.

            bool sameIndexCount = true;
            if (vertexData != null && vertexData.Length != index + 3)
            {
                sameIndexCount = false;
                vertexData = null;
            }

            if (vertexData == null)
            {
                vertexData = new CCV3F_C4B_T2F[index + 3];
            }

            if (!sameIndexCount)
            {
                //    First we populate the array with the m_tMidpoint, then all
                //    vertices/texcoords/colors of the 12 'o clock start and edges and the hitpoint
                vertexData[0].TexCoords = TextureCoordFromAlphaPoint(Midpoint);
                vertexData[0].Vertices = VertexFromAlphaPoint(Midpoint);

                vertexData[1].TexCoords = TextureCoordFromAlphaPoint(topMid);
                vertexData[1].Vertices = VertexFromAlphaPoint(topMid);

                for (int i = 0; i < index; ++i)
                {
                    CCPoint alphaPoint = BoundaryTexCoord(i);
                    vertexData[i + 2].TexCoords = TextureCoordFromAlphaPoint(alphaPoint);
                    vertexData[i + 2].Vertices = VertexFromAlphaPoint(alphaPoint);
                }
            }

            // hitpoint will go last
            vertexData[vertexData.Length - 1].TexCoords = TextureCoordFromAlphaPoint(hit);
            vertexData[vertexData.Length - 1].Vertices = VertexFromAlphaPoint(hit);
        }

        #endregion Vertex data
            
    }
}