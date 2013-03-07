using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace cocos2d
{
    public class CCParticleSystemQuad : CCParticleSystem
    {
        private RawList<CCV3F_C4B_T2F_Quad> m_pQuads; // quads to be rendered

        //implementation CCParticleSystemQuad
        // overriding the init method
        public override bool InitWithTotalParticles(int numberOfParticles)
        {
            // base initialization
            base.InitWithTotalParticles(numberOfParticles);

            // allocating data space
            AllocMemory();

#if CC_TEXTURE_ATLAS_USE_VAO
            setupVBOandVAO();
#else
            setupVBO();
#endif

            //setShaderProgram(CCShaderCache::sharedShaderCache().programForKey(kCCShader_PositionTextureColor));


            // Need to listen the event only when not use batchnode, because it will use VBO
            //CCNotificationCenter::sharedNotificationCenter().addObserver(this,
            //                                                          callfuncO_selector(listenBackToForeground),
            //                                                          EVNET_COME_TO_FOREGROUND,
            //                                                          null);

            return true;
        }

        public new static CCParticleSystemQuad Create(string plistFile)
        {
            var pRet = new CCParticleSystemQuad();
            pRet.InitWithFile(plistFile);
            return pRet;
        }

        public static CCParticleSystemQuad Create(int numberOfParticles) 
        {
            var pRet = new CCParticleSystemQuad();
            pRet.InitWithTotalParticles(numberOfParticles);
            return pRet;
        }

        // pointRect should be in Texture coordinates, not pixel coordinates
        private void InitTexCoordsWithRect(CCRect pointRect)
        {
            // convert to Tex coords

            var rect = new CCRect(
                pointRect.origin.x * ccMacros.CC_CONTENT_SCALE_FACTOR(),
                pointRect.origin.y * ccMacros.CC_CONTENT_SCALE_FACTOR(),
                pointRect.size.Width * ccMacros.CC_CONTENT_SCALE_FACTOR(),
                pointRect.size.Height * ccMacros.CC_CONTENT_SCALE_FACTOR());

            float wide = pointRect.size.Width;
            float high = pointRect.size.Height;

            if (m_pTexture != null)
            {
                wide = m_pTexture.PixelsWide;
                high = m_pTexture.PixelsHigh;
            }

#if CC_FIX_ARTIFACTS_BY_STRECHING_TEXEL
                                    float left = (rect.origin.x*2+1) / (wide*2);
    float bottom = (rect.origin.y*2+1) / (high*2);
    float right = left + (rect.size.width*2-2) / (wide*2);
    float top = bottom + (rect.size.height*2-2) / (high*2);
        #else
            float left = rect.origin.x / wide;
            float bottom = rect.origin.y / high;
            float right = left + rect.size.Width / wide;
            float top = bottom + rect.size.Height / high;
#endif
            // ! CC_FIX_ARTIFACTS_BY_STRECHING_TEXEL

            // Important. Texture in cocos2d are inverted, so the Y component should be inverted
            float tmp = top;
            top = bottom;
            bottom = tmp;

            CCV3F_C4B_T2F_Quad[] quads;
            int start, end;
            if (m_pBatchNode != null)
            {
                quads = m_pBatchNode.TextureAtlas.m_pQuads.Elements;
                m_pBatchNode.TextureAtlas.Dirty = true;
                start = m_uAtlasIndex;
                end = m_uAtlasIndex + m_uTotalParticles;
            }
            else
            {
                quads = m_pQuads.Elements;
                start = 0;
                end = m_uTotalParticles;
            }

            for (int i = start; i < end; i++)
            {
                // bottom-left vertex:
                quads[i].BottomLeft.TexCoords.U = left;
                quads[i].BottomLeft.TexCoords.V = bottom;
                // bottom-right vertex:
                quads[i].BottomRight.TexCoords.U = right;
                quads[i].BottomRight.TexCoords.V = bottom;
                // top-left vertex:
                quads[i].TopLeft.TexCoords.U = left;
                quads[i].TopLeft.TexCoords.V = top;
                // top-right vertex:
                quads[i].TopRight.TexCoords.U = right;
                quads[i].TopRight.TexCoords.V = top;
            }
        }

        public void SetTextureWithRect(CCTexture2D texture, CCRect rect)
        {
            // Only update the texture if is different from the current one
            if (m_pTexture == null || texture.Name != m_pTexture.Name)
            {
                base.Texture = texture;
            }

            InitTexCoordsWithRect(rect);
        }

        public override CCTexture2D Texture
        {
            set
            {
                CCSize s = value.ContentSize;
                SetTextureWithRect(value, new CCRect(0, 0, s.Width, s.Height));
            }
        }

        public void SetDisplayFrame(CCSpriteFrame spriteFrame)
        {
            Debug.Assert(spriteFrame.OffsetInPixels.Equals(CCPoint.Zero),
                         "QuadParticle only supports SpriteFrames with no offsets");

            // update texture before updating texture rect
            if (m_pTexture != null || spriteFrame.Texture.Name != m_pTexture.Name)
            {
                Texture = spriteFrame.Texture;
            }
        }

        private static CCPoint s_currentPosition;

        private void UpdateQuad(ref CCV3F_C4B_T2F_Quad quad, ref CCParticle particle)
        {
            CCPoint newPosition;

            if (m_ePositionType == CCPositionType.kCCPositionTypeFree || m_ePositionType == CCPositionType.kCCPositionTypeRelative)
            {
                newPosition.x = particle.pos.x - (s_currentPosition.x - particle.startPos.x);
                newPosition.y = particle.pos.y - (s_currentPosition.y - particle.startPos.y);
            }
            else
            {
                newPosition = particle.pos;
            }

            // translate newPos to correct position, since matrix transform isn't performed in batchnode
            // don't update the particle with the new position information, it will interfere with the radius and tangential calculations
            if (m_pBatchNode != null)
            {
                newPosition.x += m_tPosition.x;
                newPosition.y += m_tPosition.y;
            }

            CCColor4B color;
            
            if  (m_bOpacityModifyRGB)
            {
                color.R = (byte) (particle.color.R * particle.color.A * 255);
                color.G = (byte) (particle.color.G * particle.color.A * 255);
                color.B = (byte) (particle.color.B * particle.color.A * 255);
                color.A = (byte)(particle.color.A * 255);
            }
            else
            {
                color.R = (byte)(particle.color.R * 255);
                color.G = (byte)(particle.color.G * 255);
                color.B = (byte)(particle.color.B * 255);
                color.A = (byte)(particle.color.A * 255);
            }

            quad.BottomLeft.Colors = color;
            quad.BottomRight.Colors = color;
            quad.TopLeft.Colors = color;
            quad.TopRight.Colors = color;

            // vertices
            float size_2 = particle.size / 2;
            if (particle.rotation != 0.0)
            {
                float x1 = -size_2;
                float y1 = -size_2;

                float x2 = size_2;
                float y2 = size_2;
                float x = newPosition.x;
                float y = newPosition.y;

                float r = -MathHelper.ToRadians(particle.rotation);
                float cr = MHelper.Cos(r);
                float sr = MHelper.Sin(r);
                float ax = x1 * cr - y1 * sr + x;
                float ay = x1 * sr + y1 * cr + y;
                float bx = x2 * cr - y1 * sr + x;
                float by = x2 * sr + y1 * cr + y;
                float cx = x2 * cr - y2 * sr + x;
                float cy = x2 * sr + y2 * cr + y;
                float dx = x1 * cr - y2 * sr + x;
                float dy = x1 * sr + y2 * cr + y;

                // bottom-left
                quad.BottomLeft.Vertices.X = ax;
                quad.BottomLeft.Vertices.Y = ay;

                // bottom-right vertex:
                quad.BottomRight.Vertices.X = bx;
                quad.BottomRight.Vertices.Y = by;

                // top-left vertex:
                quad.TopLeft.Vertices.X = dx;
                quad.TopLeft.Vertices.Y = dy;

                // top-right vertex:
                quad.TopRight.Vertices.X = cx;
                quad.TopRight.Vertices.Y = cy;
            }
            else
            {
                // bottom-left vertex:
                quad.BottomLeft.Vertices.X = newPosition.x - size_2;
                quad.BottomLeft.Vertices.Y = newPosition.y - size_2;

                // bottom-right vertex:
                quad.BottomRight.Vertices.X = newPosition.x + size_2;
                quad.BottomRight.Vertices.Y = newPosition.y - size_2;

                // top-left vertex:
                quad.TopLeft.Vertices.X = newPosition.x - size_2;
                quad.TopLeft.Vertices.Y = newPosition.y + size_2;

                // top-right vertex:
                quad.TopRight.Vertices.X = newPosition.x + size_2;
                quad.TopRight.Vertices.Y = newPosition.y + size_2;
            }
        }


        public override void UpdateQuadsWithParticles()
        {
            if (!m_bIsVisible)
            {
                return;
            }

            s_currentPosition = CCPoint.Zero;
            
            if (m_ePositionType == CCPositionType.kCCPositionTypeFree)
            {
                s_currentPosition = ConvertToWorldSpace(CCPoint.Zero);
            }
            else if (m_ePositionType == CCPositionType.kCCPositionTypeRelative)
            {
                s_currentPosition = m_tPosition;
            }

            CCV3F_C4B_T2F_Quad[] quads;
            if (m_pBatchNode != null)
            {
                quads = m_pBatchNode.TextureAtlas.m_pQuads.Elements;
                m_pBatchNode.TextureAtlas.Dirty = true;
            }
            else
            {
                quads = m_pQuads.Elements;
            }

            var particles = m_pParticles;
            var count = m_uParticleCount;
            if (m_pBatchNode != null)
            {
                for (int i = 0; i < count; i++)
                {
                    UpdateQuad(ref quads[m_uAtlasIndex + particles[i].atlasIndex], ref particles[i]);
                }
            }
            else
            {
                for (int i = 0; i < count; i++)
                {
                    UpdateQuad(ref quads[i], ref particles[i]);
                }
            }
        }

        protected override void PostStep()
        {
            //glBindBuffer(GL_ARRAY_BUFFER, m_pBuffersVBO[0] );
            //glBufferSubData(GL_ARRAY_BUFFER, 0, sizeof(m_pQuads[0]) * m_uParticleCount, m_pQuads);
            //glBindBuffer(GL_ARRAY_BUFFER, 0);
            //CHECK_GL_ERROR_DEBUG();
        }

        // overriding draw method
        public override void Draw()
        {
            Debug.Assert(m_pBatchNode == null, "draw should not be called when added to a particleBatchNode");
            //Debug.Assert(m_uParticleIdx == m_uParticleCount, "Abnormal error in particle quad");

            //updateQuadsWithParticles();

            DrawManager.BindTexture(m_pTexture);
            DrawManager.BlendFunc(m_tBlendFunc);
            DrawManager.DrawQuads(m_pQuads, 0, m_uParticleCount);

            /*
            CC_NODE_DRAW_SETUP();

            ccGLBindTexture2D(m_pTexture.Name);
            ccGLBlendFunc(m_tBlendFunc.src, m_tBlendFunc.dst);


#if CC_TEXTURE_ATLAS_USE_VAO
                                                                                                    //
    // Using VBO and VAO
    //
    glBindVertexArray( m_uVAOname );

#if CC_REBIND_INDICES_BUFFER
    glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, m_pBuffersVBO[1]);
#endif

    glDrawElements(GL_TRIANGLES, (GLsizei) m_uParticleIdx*6, GL_UNSIGNED_SHORT, 0);

#if CC_REBIND_INDICES_BUFFER
    glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, 0);
#endif

    glBindVertexArray( 0 );

#else
                //
                // Using VBO without VAO
                //

#define kQuadSize sizeof(m_pQuads[0].bl)

            ccGLEnableVertexAttribs(kCCVertexAttribFlag_PosColorTex);

            glBindBuffer(GL_ARRAY_BUFFER, m_pBuffersVBO[0]);
            // vertices
            glVertexAttribPointer(kCCVertexAttrib_Position, 3, GL_FLOAT, GL_FALSE, kQuadSize,
                                  (GLvoid*) offsetof(ccV3F_C4B_T2F, vertices));
            // colors
            glVertexAttribPointer(kCCVertexAttrib_Color, 4, GL_UNSIGNED_BYTE, GL_TRUE, kQuadSize,
                                  (GLvoid*) offsetof(ccV3F_C4B_T2F, colors));
            // tex coords
            glVertexAttribPointer(kCCVertexAttrib_TexCoords, 2, GL_FLOAT, GL_FALSE, kQuadSize,
                                  (GLvoid*) offsetof(ccV3F_C4B_T2F, texCoords));

            glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, m_pBuffersVBO[1]);

            glDrawElements(GL_TRIANGLES, (GLsizei) m_uParticleIdx * 6, GL_UNSIGNED_SHORT, 0);

            glBindBuffer(GL_ARRAY_BUFFER, 0);
            glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, 0);

        #endif
            CC_INCREMENT_GL_DRAWS(1);
            CHECK_GL_ERROR_DEBUG();
*/
        }

        public override int TotalParticles
        {
            set
            {
                // If we are setting the total numer of particles to a number higher
                // than what is allocated, we need to allocate new arrays
                if (value > m_uAllocatedParticles)
                {
                    m_pParticles = new CCParticle[value];
                    m_pQuads.Capacity = value;

                    m_uTotalParticles = value;

                    // Init particles
                    if (m_pBatchNode != null)
                    {
                        for (int i = 0; i < m_uTotalParticles; i++)
                        {
                            m_pParticles[i].atlasIndex = i;
                        }
                    }

#if CC_TEXTURE_ATLAS_USE_VAO
                    setupVBOandVAO();
#else
                    setupVBO();
#endif
                }
                else
                {
                    m_uTotalParticles = value;
                }
            }
        }

#if CC_TEXTURE_ATLAS_USE_VAO
void setupVBOandVAO()
{
    glGenVertexArrays(1, &m_uVAOname);
    glBindVertexArray(m_uVAOname);

//#define kQuadSize sizeof(m_pQuads[0].bl)

    glGenBuffers(2, &m_pBuffersVBO[0]);

    glBindBuffer(GL_ARRAY_BUFFER, m_pBuffersVBO[0]);
    glBufferData(GL_ARRAY_BUFFER, sizeof(m_pQuads[0]) * m_uTotalParticles, m_pQuads, GL_DYNAMIC_DRAW);

    // vertices
    glEnableVertexAttribArray(kCCVertexAttrib_Position);
    glVertexAttribPointer(kCCVertexAttrib_Position, 2, GL_FLOAT, GL_FALSE, kQuadSize, (GLvoid*) offsetof( ccV3F_C4B_T2F, vertices));

    // colors
    glEnableVertexAttribArray(kCCVertexAttrib_Color);
    glVertexAttribPointer(kCCVertexAttrib_Color, 4, GL_UNSIGNED_BYTE, GL_TRUE, kQuadSize, (GLvoid*) offsetof( ccV3F_C4B_T2F, colors));

    // tex coords
    glEnableVertexAttribArray(kCCVertexAttrib_TexCoords);
    glVertexAttribPointer(kCCVertexAttrib_TexCoords, 2, GL_FLOAT, GL_FALSE, kQuadSize, (GLvoid*) offsetof( ccV3F_C4B_T2F, texCoords));

    glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, m_pBuffersVBO[1]);
    glBufferData(GL_ELEMENT_ARRAY_BUFFER, sizeof(m_pIndices[0]) * m_uTotalParticles * 6, m_pIndices, GL_STATIC_DRAW);

    glBindVertexArray(0);
    glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, 0);
    glBindBuffer(GL_ARRAY_BUFFER, 0);

    CHECK_GL_ERROR_DEBUG();
}
#else

        private void setupVBO()
        {
/*
    glGenBuffers(2, &m_pBuffersVBO[0]);

    glBindBuffer(GL_ARRAY_BUFFER, m_pBuffersVBO[0]);
    glBufferData(GL_ARRAY_BUFFER, sizeof(m_pQuads[0]) * m_uTotalParticles, m_pQuads, GL_DYNAMIC_DRAW);
    glBindBuffer(GL_ARRAY_BUFFER, 0);

    glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, m_pBuffersVBO[1]);
    glBufferData(GL_ELEMENT_ARRAY_BUFFER, sizeof(m_pIndices[0]) * m_uTotalParticles * 6, m_pIndices, GL_STATIC_DRAW);
    glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, 0);

    CHECK_GL_ERROR_DEBUG();
*/
        }

#endif

        private void ListenBackToForeground(CCObject obj)
        {
#if CC_TEXTURE_ATLAS_USE_VAO
            setupVBOandVAO();
#else
            setupVBO();
#endif
        }

        private bool AllocMemory()
        {
            Debug.Assert(m_pBatchNode == null, "Memory should not be alloced when not using batchNode");
            Debug.Assert((m_pQuads == null), "Memory already alloced");
            m_pQuads = new RawList<CCV3F_C4B_T2F_Quad>(m_uTotalParticles);
            return true;
        }

        public override CCParticleBatchNode BatchNode
        {
            set
            {
                if (m_pBatchNode != value)
                {
                    CCParticleBatchNode oldBatch = m_pBatchNode;

                    base.BatchNode = value;

                    // NEW: is self render ?
                    if (value == null)
                    {
                        AllocMemory();
                        Texture = oldBatch.Texture;
#if CC_TEXTURE_ATLAS_USE_VAO
                        setupVBOandVAO();
#else
                        setupVBO();
#endif
                    }
                        // OLD: was it self render ? cleanup
                    else if (oldBatch == null)
                    {
                        // copy current state to batch
                        var batchQuads = m_pBatchNode.TextureAtlas.m_pQuads.Elements;
                        m_pBatchNode.TextureAtlas.Dirty = true;
                        Array.Copy(m_pQuads.Elements, 0, batchQuads, m_uAtlasIndex, m_uTotalParticles);
                        m_pQuads = null;

                        //glDeleteBuffers(2, &m_pBuffersVBO[0]);
#if CC_TEXTURE_ATLAS_USE_VAO
                        glDeleteVertexArrays(1, &m_uVAOname);
#endif
                    }
                }
            }
        }

        public new static CCParticleSystemQuad Create()
        {
            var p = new CCParticleSystemQuad();
            p.Init();
            return p;
        }

        public CCParticleSystemQuad Clone()
        {
            var p = new CCParticleSystemQuad();

            p.InitWithTotalParticles(m_uTotalParticles);

            // angle
            p.m_fAngle = m_fAngle;
            p.m_fAngleVar = m_fAngleVar;

            // duration
            p.m_fDuration = m_fDuration;

            // blend function 
            p.m_tBlendFunc = m_tBlendFunc;

            // color
            p.m_tStartColor = m_tStartColor;
            p.m_tStartColorVar = m_tStartColorVar;
            p.m_tEndColor = m_tEndColor;
            p.m_tEndColorVar = m_tEndColorVar;

            // particle size
            p.m_fStartSize = m_fStartSize;
            p.m_fStartSizeVar = m_fStartSizeVar;
            p.m_fEndSize = m_fEndSize;
            p.m_fEndSizeVar = m_fEndSizeVar;

            // position
            p.Position = Position;
            p.m_tPosVar = m_tPosVar;

            // Spinning
            p.m_fStartSpin = m_fStartSpin;
            p.m_fStartSpinVar = m_fStartSpinVar;
            p.m_fEndSpin = m_fEndSpin;
            p.m_fEndSpinVar = m_fEndSpinVar;

            p.m_nEmitterMode = m_nEmitterMode;

            p.modeA = modeA;
            p.modeB = modeB;

            // life span
            p.m_fLife = m_fLife;
            p.m_fLifeVar = m_fLifeVar;

            // emission Rate
            p.m_fEmissionRate = m_fEmissionRate;

            p.m_bOpacityModifyRGB = m_bOpacityModifyRGB;
            p.m_pTexture = m_pTexture;

            p.AutoRemoveOnFinish = AutoRemoveOnFinish;

            return p;
        }
    }
}