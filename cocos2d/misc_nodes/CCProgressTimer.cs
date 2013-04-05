using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace cocos2d
{
    public enum CCProgressTimerType
    {
        /// Radial Counter-Clockwise
        Radial,
        /// Bar
        Bar,
    }

/**
 @brief CCProgresstimer is a subclass of CCNode.
 It renders the inner sprite according to the percentage.
 The progress can be Radial, Horizontal or vertical.
 @since v0.99.1
 */

    public class CCProgressTimer : CCNode, ICCRGBAProtocol
    {
        private const int kProgressTextureCoordsCount = 4;
        //kProgressTextureCoords holds points {0,1} {0,0} {1,0} {1,1} we can represent it as bits
        private const uint kCCProgressTextureCoords = 0x4b;

        protected bool m_bReverseDirection;
        protected CCProgressTimerType m_eType;
        protected float m_fPercentage;
        protected int m_nVertexDataCount;
        protected CCSprite m_pSprite;
        protected CCV3F_C4B_T2F[] m_pVertexData;
        protected CCPoint m_tBarChangeRate;
        protected CCPoint m_tMidpoint;

        /**    Change the percentage to change progress. */

        public CCProgressTimerType Type
        {
            get { return m_eType; }
            set
            {
                if (value != m_eType)
                {
                    //    release all previous information
                    if (m_pVertexData != null)
                    {
                        m_pVertexData = null;
                        m_nVertexDataCount = 0;
                    }

                    m_eType = value;
                }
            }
        }

        /** Percentages are from 0 to 100 */

        public float Percentage
        {
            get { return m_fPercentage; }
            set
            {
                if (m_fPercentage != value)
                {
                    m_fPercentage = MathHelper.Clamp(value, 0, 100);
                    UpdateProgress();
                }
            }
        }

        /** The image to show the progress percentage, retain */

        public CCSprite Sprite
        {
            get { return m_pSprite; }
            set
            {
                if (m_pSprite != value)
                {
                    m_pSprite = value;
                    ContentSize = value.ContentSize;

                    //    Everytime we set a new sprite, we free the current vertex data
                    if (m_pVertexData != null)
                    {
                        m_pVertexData = null;
                        m_nVertexDataCount = 0;
                    }
                }
            }
        }

        /** Initializes a progress timer with the sprite as the shape the timer goes through */

        public bool ReverseProgress
        {
            set
            {
                if (m_bReverseDirection != value)
                {
                    m_bReverseDirection = value;

                    //    release all previous information
                    m_pVertexData = null;
                    m_nVertexDataCount = 0;
                }
            }
        }

        public CCPoint Midpoint
        {
            get { return m_tMidpoint; }
            set
            {
                m_tMidpoint.X = MathHelper.Clamp(value.X, 0, 1);
                m_tMidpoint.Y = MathHelper.Clamp(value.Y, 0, 1);
            }
        }

        public CCPoint BarChangeRate
        {
            get { return m_tBarChangeRate; }
            set { m_tBarChangeRate = value; }
        }

        public bool ReverseDirection
        {
            get { return m_bReverseDirection; }
            set { m_bReverseDirection = value; }
        }


        #region ICCRGBAProtocol Members

        public byte Opacity
        {
            get { return m_pSprite.Opacity; }
            set { m_pSprite.Opacity = value; }
        }

        public CCColor3B Color
        {
            get { return m_pSprite.Color; }
            set { m_pSprite.Color = value; }
        }

        public bool IsOpacityModifyRGB
        {
            get { return false; }
            set { }
        }

        #endregion

        public bool InitWithSprite(CCSprite sp)
        {
            Percentage = 0.0f;
            m_pVertexData = null;
            m_nVertexDataCount = 0;

            AnchorPoint = new CCPoint(0.5f, 0.5f);
            m_eType = CCProgressTimerType.Radial;
            m_bReverseDirection = false;
            Midpoint = new CCPoint(0.5f, 0.5f);
            BarChangeRate = new CCPoint(1, 1);
            Sprite = sp;

            // shader program
            //setShaderProgram(CCShaderCache::sharedShaderCache()->programForKey(kCCShader_PositionTextureColor));
            return true;
        }

        private static short[] s_pIndexes;

        public override void Draw()
        {
            if (m_pVertexData == null || m_pSprite == null)
                return;

            DrawManager.BindTexture(Sprite.Texture);
            DrawManager.BlendFunc(m_pSprite.BlendFunc);

            var count = (m_nVertexDataCount - 2);

            if (s_pIndexes == null || s_pIndexes.Length < count * 3)
            {
                s_pIndexes = new short[count * 3];
            }

            if (m_eType == CCProgressTimerType.Radial)
            {
                //FAN
                for (int i = 0; i < count; i++)
                {
                    var i3 = i * 3;
                    s_pIndexes[i3 + 0] = 0;
                    s_pIndexes[i3 + 1] = (short)(i + 1);
                    s_pIndexes[i3 + 2] = (short)(i + 2);
                }

                DrawManager.DrawIndexedPrimitives(PrimitiveType.TriangleList, m_pVertexData, 0, m_nVertexDataCount, s_pIndexes, 0, count);
            }
            else if (m_eType == CCProgressTimerType.Bar)
            {
                //TRIANGLE STRIP
                for (int i = 0; i < count; i++)
                {
                    var i3 = i * 3;
                    s_pIndexes[i3 + 0] = (short)(i + 0);
                    s_pIndexes[i3 + 1] = (short)(i + 1);
                    s_pIndexes[i3 + 2] = (short)(i + 2);
                }

                if (!m_bReverseDirection)
                {
                    DrawManager.DrawIndexedPrimitives(PrimitiveType.TriangleList, m_pVertexData, 0, m_nVertexDataCount, s_pIndexes, 0, count);
                }
                else
                {
                    DrawManager.DrawIndexedPrimitives(PrimitiveType.TriangleList, m_pVertexData, 0, m_nVertexDataCount, s_pIndexes, 0, count);
                }
            }

            /*
            CC_NODE_DRAW_SETUP();

            ccGLBlendFunc( m_pSprite->getBlendFunc().src, m_pSprite->getBlendFunc().dst );

            ccGLEnableVertexAttribs(kCCVertexAttribFlag_PosColorTex );

            ccGLBindTexture2D( m_pSprite->getTexture()->getName() );

            glVertexAttribPointer( kCCVertexAttrib_Position, 2, GL_FLOAT, GL_FALSE, sizeof(m_pVertexData[0]) , &m_pVertexData[0].vertices);
            glVertexAttribPointer( kCCVertexAttrib_TexCoords, 2, GL_FLOAT, GL_FALSE, sizeof(m_pVertexData[0]), &m_pVertexData[0].texCoords);
            glVertexAttribPointer( kCCVertexAttrib_Color, 4, GL_UNSIGNED_BYTE, GL_TRUE, sizeof(m_pVertexData[0]), &m_pVertexData[0].colors);

            if(m_eType == kCCProgressTimerTypeRadial)
            {
                glDrawArrays(GL_TRIANGLE_FAN, 0, m_nVertexDataCount);
            } 
            else if (m_eType == kCCProgressTimerTypeBar)
            {
                if (!m_bReverseDirection) 
                {
                    glDrawArrays(GL_TRIANGLE_STRIP, 0, m_nVertexDataCount);
                } 
                else 
                {
                    glDrawArrays(GL_TRIANGLE_STRIP, 0, m_nVertexDataCount/2);
                    glDrawArrays(GL_TRIANGLE_STRIP, 4, m_nVertexDataCount/2);
                    // 2 draw calls
                    CC_INCREMENT_GL_DRAWS(1);
                }
            }
            CC_INCREMENT_GL_DRAWS(1);
            */
        }


        public static CCProgressTimer Create(string fileName)
        {
            return Create(new CCSprite(fileName));
        }

        /** Creates a progress timer with the sprite as the shape the timer goes through */

        public static CCProgressTimer Create(CCSprite sp)
        {
            var pProgressTimer = new CCProgressTimer();
            pProgressTimer.InitWithSprite(sp);
            return pProgressTimer;
        }

        protected CCTex2F TextureCoordFromAlphaPoint(CCPoint alpha)
        {
            var ret = new CCTex2F(0.0f, 0.0f);
            if (m_pSprite == null)
            {
                return ret;
            }

            CCV3F_C4B_T2F_Quad quad = m_pSprite.Quad;

            var min = new CCPoint(quad.BottomLeft.TexCoords.U, quad.BottomLeft.TexCoords.V);
            var max = new CCPoint(quad.TopRight.TexCoords.U, quad.TopRight.TexCoords.V);

            //  Fix bug #1303 so that progress timer handles sprite frame texture rotation
            if (m_pSprite.IsTextureRectRotated)
            {
                float tmp = alpha.X;
                alpha.X = alpha.Y;
                alpha.Y = tmp;
            }
            return new CCTex2F(min.X * (1f - alpha.X) + max.X * alpha.X, min.Y * (1f - alpha.Y) + max.Y * alpha.Y);
        }

        protected CCVertex3F VertexFromAlphaPoint(CCPoint alpha)
        {
            var ret = new CCVertex3F(0.0f, 0.0f, 0.0f);

            if (m_pSprite == null)
            {
                return ret;
            }

            CCV3F_C4B_T2F_Quad quad = m_pSprite.Quad;

            var min = new CCPoint(quad.BottomLeft.Vertices.X, quad.BottomLeft.Vertices.Y);
            var max = new CCPoint(quad.TopRight.Vertices.X, quad.TopRight.Vertices.Y);

            ret.X = min.X * (1f - alpha.X) + max.X * alpha.X;
            ret.Y = min.Y * (1f - alpha.Y) + max.Y * alpha.Y;

            return ret;
        }

        protected void UpdateProgress()
        {
            switch (m_eType)
            {
                case CCProgressTimerType.Radial:
                    UpdateRadial();
                    break;
                case CCProgressTimerType.Bar:
                    UpdateBar();
                    break;
                default:
                    break;
            }
        }

        protected void UpdateBar()
        {
            if (m_pSprite == null)
            {
                return;
            }

            float alpha = m_fPercentage / 100.0f;
            CCPoint alphaOffset =
                CCPointExtension.Multiply(
                    new CCPoint(1.0f * (1.0f - m_tBarChangeRate.X) + alpha * m_tBarChangeRate.X,
                                1.0f * (1.0f - m_tBarChangeRate.Y) + alpha * m_tBarChangeRate.Y), 0.5f);
            CCPoint min = CCPointExtension.Subtract(m_tMidpoint, alphaOffset);
            CCPoint max = CCPointExtension.Add(m_tMidpoint, alphaOffset);

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


            if (!m_bReverseDirection)
            {
                if (m_pVertexData == null)
                {
                    m_nVertexDataCount = 4;
                    m_pVertexData = new CCV3F_C4B_T2F[m_nVertexDataCount];
                }
                //    TOPLEFT
                m_pVertexData[0].TexCoords = TextureCoordFromAlphaPoint(new CCPoint(min.X, max.Y));
                m_pVertexData[0].Vertices = VertexFromAlphaPoint(new CCPoint(min.X, max.Y));

                //    BOTLEFT
                m_pVertexData[1].TexCoords = TextureCoordFromAlphaPoint(new CCPoint(min.X, min.Y));
                m_pVertexData[1].Vertices = VertexFromAlphaPoint(new CCPoint(min.X, min.Y));

                //    TOPRIGHT
                m_pVertexData[2].TexCoords = TextureCoordFromAlphaPoint(new CCPoint(max.X, max.Y));
                m_pVertexData[2].Vertices = VertexFromAlphaPoint(new CCPoint(max.X, max.Y));

                //    BOTRIGHT
                m_pVertexData[3].TexCoords = TextureCoordFromAlphaPoint(new CCPoint(max.X, min.Y));
                m_pVertexData[3].Vertices = VertexFromAlphaPoint(new CCPoint(max.X, min.Y));
            }
            else
            {
                if (m_pVertexData == null)
                {
                    m_nVertexDataCount = 8;
                    m_pVertexData = new CCV3F_C4B_T2F[m_nVertexDataCount];

                    //    TOPLEFT 1
                    m_pVertexData[0].TexCoords = TextureCoordFromAlphaPoint(new CCPoint(0, 1));
                    m_pVertexData[0].Vertices = VertexFromAlphaPoint(new CCPoint(0, 1));

                    //    BOTLEFT 1
                    m_pVertexData[1].TexCoords = TextureCoordFromAlphaPoint(new CCPoint(0, 0));
                    m_pVertexData[1].Vertices = VertexFromAlphaPoint(new CCPoint(0, 0));

                    //    TOPRIGHT 2
                    m_pVertexData[6].TexCoords = TextureCoordFromAlphaPoint(new CCPoint(1, 1));
                    m_pVertexData[6].Vertices = VertexFromAlphaPoint(new CCPoint(1, 1));

                    //    BOTRIGHT 2
                    m_pVertexData[7].TexCoords = TextureCoordFromAlphaPoint(new CCPoint(1, 0));
                    m_pVertexData[7].Vertices = VertexFromAlphaPoint(new CCPoint(1, 0));
                }

                //    TOPRIGHT 1
                m_pVertexData[2].TexCoords = TextureCoordFromAlphaPoint(new CCPoint(min.X, max.Y));
                m_pVertexData[2].Vertices = VertexFromAlphaPoint(new CCPoint(min.X, max.Y));

                //    BOTRIGHT 1
                m_pVertexData[3].TexCoords = TextureCoordFromAlphaPoint(new CCPoint(min.X, min.Y));
                m_pVertexData[3].Vertices = VertexFromAlphaPoint(new CCPoint(min.X, min.Y));

                //    TOPLEFT 2
                m_pVertexData[4].TexCoords = TextureCoordFromAlphaPoint(new CCPoint(max.X, max.Y));
                m_pVertexData[4].Vertices = VertexFromAlphaPoint(new CCPoint(max.X, max.Y));

                //    BOTLEFT 2
                m_pVertexData[5].TexCoords = TextureCoordFromAlphaPoint(new CCPoint(max.X, min.Y));
                m_pVertexData[5].Vertices = VertexFromAlphaPoint(new CCPoint(max.X, min.Y));
            }
            UpdateColor();
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
        protected void UpdateRadial()
        {
            if (m_pSprite == null)
            {
                return;
            }
            float alpha = m_fPercentage / 100f;

            float angle = 2f * (MathHelper.Pi) * (m_bReverseDirection ? alpha : 1.0f - alpha);

            //    We find the vector to do a hit detection based on the percentage
            //    We know the first vector is the one @ 12 o'clock (top,mid) so we rotate
            //    from that by the progress angle around the m_tMidpoint pivot
            var topMid = new CCPoint(m_tMidpoint.X, 1f);
            CCPoint percentagePt = CCPointExtension.RotateByAngle(topMid, m_tMidpoint, angle);


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

                for (int i = 0; i <= kProgressTextureCoordsCount; ++i)
                {
                    int pIndex = (i + (kProgressTextureCoordsCount - 1)) % kProgressTextureCoordsCount;

                    CCPoint edgePtA = BoundaryTexCoord(i % kProgressTextureCoordsCount);
                    CCPoint edgePtB = BoundaryTexCoord(pIndex);

                    //    Remember that the top edge is split in half for the 12 o'clock position
                    //    Let's deal with that here by finding the correct endpoints
                    if (i == 0)
                    {
                        edgePtB = CCPointExtension.Lerp(edgePtA, edgePtB, 1 - m_tMidpoint.X);
                    }
                    else if (i == 4)
                    {
                        edgePtA = CCPointExtension.Lerp(edgePtA, edgePtB, 1 - m_tMidpoint.X);
                    }

                    //    s and t are returned by ccpLineIntersect
                    float s = 0, t = 0;
                    if (CCPointExtension.LineIntersect(edgePtA, edgePtB, m_tMidpoint, percentagePt, ref s, ref t))
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
                hit = CCPointExtension.Add(m_tMidpoint, CCPointExtension.Multiply(CCPointExtension.Subtract(percentagePt, m_tMidpoint), min_t));
            }


            //    The size of the vertex data is the index from the hitpoint
            //    the 3 is for the m_tMidpoint, 12 o'clock point and hitpoint position.

            bool sameIndexCount = true;
            if (m_nVertexDataCount != index + 3)
            {
                sameIndexCount = false;
                m_pVertexData = null;
            }


            if (m_pVertexData == null)
            {
                m_nVertexDataCount = index + 3;
                m_pVertexData = new CCV3F_C4B_T2F[m_nVertexDataCount];
            }

            UpdateColor();

            if (!sameIndexCount)
            {
                //    First we populate the array with the m_tMidpoint, then all
                //    vertices/texcoords/colors of the 12 'o clock start and edges and the hitpoint
                m_pVertexData[0].TexCoords = TextureCoordFromAlphaPoint(m_tMidpoint);
                m_pVertexData[0].Vertices = VertexFromAlphaPoint(m_tMidpoint);

                m_pVertexData[1].TexCoords = TextureCoordFromAlphaPoint(topMid);
                m_pVertexData[1].Vertices = VertexFromAlphaPoint(topMid);

                for (int i = 0; i < index; ++i)
                {
                    CCPoint alphaPoint = BoundaryTexCoord(i);
                    m_pVertexData[i + 2].TexCoords = TextureCoordFromAlphaPoint(alphaPoint);
                    m_pVertexData[i + 2].Vertices = VertexFromAlphaPoint(alphaPoint);
                }
            }

            //    hitpoint will go last
            m_pVertexData[m_nVertexDataCount - 1].TexCoords = TextureCoordFromAlphaPoint(hit);
            m_pVertexData[m_nVertexDataCount - 1].Vertices = VertexFromAlphaPoint(hit);
        }

        protected void UpdateColor()
        {
            if (m_pSprite == null)
            {
                return;
            }

            if (m_pVertexData != null)
            {
                CCColor4B sc = m_pSprite.Quad.TopLeft.Colors;
                for (int i = 0; i < m_nVertexDataCount; ++i)
                {
                    m_pVertexData[i].Colors = sc;
                }
            }
        }

        protected CCPoint BoundaryTexCoord(int index)
        {
            if (index < kProgressTextureCoordsCount)
            {
                if (m_bReverseDirection)
                {
                    return new CCPoint((kCCProgressTextureCoords >> (7 - (index << 1))) & 1,
                                       (kCCProgressTextureCoords >> (7 - ((index << 1) + 1))) & 1);
                }
                return new CCPoint((kCCProgressTextureCoords >> ((index << 1) + 1)) & 1, (kCCProgressTextureCoords >> (index << 1)) & 1);
            }
            return CCPoint.Zero;
        }
    }
}