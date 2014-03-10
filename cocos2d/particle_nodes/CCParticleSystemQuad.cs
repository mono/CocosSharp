using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CocosSharp
{
    public class CCParticleSystemQuad : CCParticleSystem
    {
        private CCRawList<CCV3F_C4B_T2F_Quad> m_pQuads; // quads to be rendered


        #region Constructors

        internal CCParticleSystemQuad()
        {  
        }

        public CCParticleSystemQuad(int numberOfParticles) : base(numberOfParticles)
        {
        }

        public CCParticleSystemQuad(string plistFile) : base(plistFile)
        { 
        }

        protected override void InitWithTotalParticles(int numberOfParticles)
        {
            base.InitWithTotalParticles(numberOfParticles);

            AllocMemory();
        }

        #endregion Constructors


        // pointRect should be in Texture coordinates, not pixel coordinates
        private void InitTexCoordsWithRect(CCRect pointRect)
        {
            // convert to Tex coords
            var rect = pointRect.PointsToPixels();

            float wide = pointRect.Size.Width;
            float high = pointRect.Size.Height;

            if (Texture != null)
            {
                wide = Texture.PixelsWide;
                high = Texture.PixelsHigh;
            }

#if CC_FIX_ARTIFACTS_BY_STRECHING_TEXEL
            float left = (rect.Origin.X * 2 + 1) / (wide * 2);
            float bottom = (rect.Origin.Y * 2 + 1) / (high * 2);
            float right = left + (rect.Size.Width * 2 - 2) / (wide * 2);
            float top = bottom + (rect.Size.Height * 2 - 2) / (high * 2);
#else
            float left = rect.Origin.X / wide;
            float bottom = rect.Origin.Y / high;
            float right = left + rect.Size.Width / wide;
            float top = bottom + rect.Size.Height / high;
#endif
            // ! CC_FIX_ARTIFACTS_BY_STRECHING_TEXEL

            // Important. Texture in cocos2d are inverted, so the Y component should be inverted
            float tmp = top;
            top = bottom;
            bottom = tmp;

            CCV3F_C4B_T2F_Quad[] quads;
            int start, end;
            if (BatchNode != null)
            {
                quads = BatchNode.TextureAtlas.m_pQuads.Elements;
                BatchNode.TextureAtlas.Dirty = true;
                start = AtlasIndex;
                end = AtlasIndex + TotalParticles;
            }
            else
            {
                quads = m_pQuads.Elements;
                start = 0;
                end = TotalParticles;
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
            if (Texture == null || texture.Name != Texture.Name)
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
            if (Texture != null || spriteFrame.Texture.Name != Texture.Name)
            {
                Texture = spriteFrame.Texture;
            }
        }

        private static CCPoint s_currentPosition;

        private void UpdateQuad(ref CCV3F_C4B_T2F_Quad quad, ref CCParticle particle)
        {
            CCPoint newPosition;

            if (PositionType == CCPositionType.Free || PositionType == CCPositionType.Relative)
            {
                newPosition.X = particle.Position.X - (s_currentPosition.X - particle.StartPosition.X);
                newPosition.Y = particle.Position.Y - (s_currentPosition.Y - particle.StartPosition.Y);
            }
            else
            {
                newPosition = particle.Position;
            }

            // translate newPos to correct position, since matrix transform isn't performed in batchnode
            // don't update the particle with the new position information, it will interfere with the radius and tangential calculations
            if (BatchNode != null)
            {
                newPosition.X += m_obPosition.X;
                newPosition.Y += m_obPosition.Y;
            }

            CCColor4B color;
            
            if  (OpacityModifyRGB)
            {
                color.R = (byte) (particle.Color.R * particle.Color.A * 255);
                color.G = (byte) (particle.Color.G * particle.Color.A * 255);
                color.B = (byte) (particle.Color.B * particle.Color.A * 255);
                color.A = (byte)(particle.Color.A * 255);
            }
            else
            {
                color.R = (byte)(particle.Color.R * 255);
                color.G = (byte)(particle.Color.G * 255);
                color.B = (byte)(particle.Color.B * 255);
                color.A = (byte)(particle.Color.A * 255);
            }

            quad.BottomLeft.Colors = color;
            quad.BottomRight.Colors = color;
            quad.TopLeft.Colors = color;
            quad.TopRight.Colors = color;

            // vertices
            float size_2 = particle.Size / 2;
            if (particle.Rotation != 0.0)
            {
                float x1 = -size_2;
                float y1 = -size_2;

                float x2 = size_2;
                float y2 = size_2;
                float x = newPosition.X;
                float y = newPosition.Y;

                float r = -CCMathHelper.ToRadians(particle.Rotation);
                float cr = CCMathHelper.Cos(r);
                float sr = CCMathHelper.Sin(r);
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
                quad.BottomLeft.Vertices.X = newPosition.X - size_2;
                quad.BottomLeft.Vertices.Y = newPosition.Y - size_2;

                // bottom-right vertex:
                quad.BottomRight.Vertices.X = newPosition.X + size_2;
                quad.BottomRight.Vertices.Y = newPosition.Y - size_2;

                // top-left vertex:
                quad.TopLeft.Vertices.X = newPosition.X - size_2;
                quad.TopLeft.Vertices.Y = newPosition.Y + size_2;

                // top-right vertex:
                quad.TopRight.Vertices.X = newPosition.X + size_2;
                quad.TopRight.Vertices.Y = newPosition.Y + size_2;
            }
        }


        public override void UpdateQuadsWithParticles()
        {
            if (!m_bVisible)
            {
                return;
            }

            s_currentPosition = CCPoint.Zero;
            
            if (PositionType == CCPositionType.Free)
            {
                s_currentPosition = ConvertToWorldSpace(CCPoint.Zero);
            }
            else if (PositionType == CCPositionType.Relative)
            {
                s_currentPosition = m_obPosition;
            }

            CCV3F_C4B_T2F_Quad[] quads;
            if (BatchNode != null)
            {
                quads = BatchNode.TextureAtlas.m_pQuads.Elements;
                BatchNode.TextureAtlas.Dirty = true;
            }
            else
            {
                quads = m_pQuads.Elements;
            }

            var count = ParticleCount;
            if (BatchNode != null)
            {
                for (int i = 0; i < count; i++)
                {
                    UpdateQuad(ref quads[AtlasIndex + Particles[i].AtlasIndex], ref Particles[i]);
                }
            }
            else
            {
                for (int i = 0; i < count; i++)
                {
                    UpdateQuad(ref quads[i], ref Particles[i]);
                }
            }
        }

        // overriding draw method
        protected override void Draw()
        {
            Debug.Assert(BatchNode == null, "draw should not be called when added to a particleBatchNode");
            //Debug.Assert(m_uParticleIdx == ParticleCount, "Abnormal error in particle quad");

            //updateQuadsWithParticles();

            CCDrawManager.BindTexture(Texture);
            CCDrawManager.BlendFunc(BlendFunc);
            CCDrawManager.DrawQuads(m_pQuads, 0, ParticleCount);
        }

        public override int TotalParticles
        {
            set
            {
                // If we are setting the total numer of particles to a number higher
                // than what is allocated, we need to allocate new arrays
                if (value > AllocatedParticles)
                {
                    Particles = new CCParticle[value];

                    if (m_pQuads == null)
                    {
                        m_pQuads = new CCRawList<CCV3F_C4B_T2F_Quad>(value); 
                    }
                    else
                    {
                        m_pQuads.Capacity = value;
                    }

                    base.TotalParticles = value;

                    // Init particles
                    if (BatchNode != null)
                    {
                        for (int i = 0; i < TotalParticles; i++)
                        {
                            Particles[i].AtlasIndex = i;
                        }
                    }
                }
                else
                {
                    base.TotalParticles = value;
                }
            }
        }

        private bool AllocMemory()
        {
            Debug.Assert(BatchNode == null, "Memory should not be alloced when not using batchNode");
            Debug.Assert((m_pQuads == null), "Memory already alloced");
            m_pQuads = new CCRawList<CCV3F_C4B_T2F_Quad>(TotalParticles);
            return true;
        }

        public override CCParticleBatchNode BatchNode
        {
            set
            {
                if (BatchNode != value)
                {
                    CCParticleBatchNode oldBatch = BatchNode;

                    base.BatchNode = value;

                    // NEW: is self render ?
                    if (value == null)
                    {
                        AllocMemory();
                        Texture = oldBatch.Texture;
                    }
                        // OLD: was it self render ? cleanup
                    else if (oldBatch == null)
                    {
                        // copy current state to batch
                        var batchQuads = BatchNode.TextureAtlas.m_pQuads.Elements;
                        BatchNode.TextureAtlas.Dirty = true;
                        Array.Copy(m_pQuads.Elements, 0, batchQuads, AtlasIndex, TotalParticles);
                        m_pQuads = null;
                    }
                }
            }
        }

        public CCParticleSystemQuad Clone()
        {
            var p = new CCParticleSystemQuad(TotalParticles);

            // angle
            p.Angle = Angle;
            p.AngleVar = AngleVar;

            // duration
            p.Duration = Duration;

            // blend function 
            p.BlendFunc = BlendFunc;

            // color
            p.StartColor = StartColor;
            p.StartColorVar = StartColorVar;
            p.EndColor = EndColor;
            p.EndColorVar = EndColorVar;

            // particle size
            p.StartSize = StartSize;
            p.StartSizeVar = StartSizeVar;
            p.EndSize = EndSize;
            p.EndSizeVar = EndSizeVar;

            // position
            p.Position = Position;
            p.PositionVar = PositionVar;

            // Spinning
            p.StartSpin = StartSpin;
            p.StartSpinVar = StartSpinVar;
            p.EndSpin = EndSpin;
            p.EndSpinVar = EndSpinVar;

            p.EmitterMode = EmitterMode;

            p.GravityMode = GravityMode;
            p.RadialMode = RadialMode;

            // life span
            p.Life = Life;
            p.LifeVar = LifeVar;

            // emission Rate
            p.EmissionRate = EmissionRate;

            p.OpacityModifyRGB = OpacityModifyRGB;
            p.Texture = Texture;

            p.AutoRemoveOnFinish = AutoRemoveOnFinish;

            return p;
        }
    }
}