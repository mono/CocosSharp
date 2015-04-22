using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CocosSharp
{
    public class CCParticleSystemQuad : CCParticleSystem
    {
        // ivars
        CCRawList<CCV3F_C4B_T2F_Quad> quads;
        CCPoint currentPosition;


        #region Properties

        public override int TotalParticles
        {
            set
            {
                // If we are setting the total numer of particles to a number higher
                // than what is allocated, we need to allocate new arrays
                if (value > AllocatedParticles)
                {
                    if (EmitterMode == CCEmitterMode.Gravity) 
                    {
                        GravityParticles = new CCParticleGravity[value];

                        if(BatchNode != null)
                        {
                            for (int i = 0; i < value; i++)
                            {
                                GravityParticles[i].AtlasIndex = i;
                            }
                        }
                    } 
                    else 
                    {
                        RadialParticles = new CCParticleRadial[value];

                        if(BatchNode != null)
                        {
                            for (int i = 0; i < value; i++)
                            {
                                RadialParticles[i].AtlasIndex = i;
                            }
                        }
                    }

                    if (Quads == null) 
                    {
                        Quads = new CCRawList<CCV3F_C4B_T2F_Quad> (value); 
                    } 
                    else 
                    {
                        Quads.Capacity = value;
                    }

                    AllocatedParticles = value;
                    base.TotalParticles = value;
                }
                else
                {
                    base.TotalParticles = value;
                }
            }
        }

        public override CCTexture2D Texture
        {
            set
            {
                if (value == null)
                    return;

                // Only update the texture if is different from the current one
                if (Texture == null || value.Name != Texture.Name)
                {
                    base.Texture = value;

                    CCSize s = value.ContentSizeInPixels;
                    TextureRect = new CCRect (0, 0, s.Width, s.Height);
                }
            }
        }

        public CCRect TextureRect
        {
            set { ResetTexCoords(value); }
        }

        public override CCParticleBatchNode BatchNode
        {
            set
            {
                if (BatchNode != value)
                {
                    CCParticleBatchNode oldBatch = BatchNode;

                    base.BatchNode = value;

                    if (value == null)
                    {
                        Debug.Assert (BatchNode == null, "Memory should not be alloced when not using batchNode");
                        Debug.Assert ((quads == null), "Memory already alloced");

                        Quads = new CCRawList<CCV3F_C4B_T2F_Quad> (TotalParticles);
                        Texture = oldBatch.Texture;
                    }

                    else if (oldBatch == null)
                    {
                        var batchQuads = BatchNode.TextureAtlas.Quads.Elements;
                        BatchNode.TextureAtlas.Dirty = true;
                        Array.Copy(quads.Elements, 0, batchQuads, AtlasIndex, TotalParticles);
                        Quads = null;
                    }
                }
            }
        }

        CCRawList<CCV3F_C4B_T2F_Quad> Quads
        {
            get { return quads; }
            set 
            {
                CCRawList<CCV3F_C4B_T2F_Quad> oldQuads = quads;
                quads = value;

                if (Texture!= null && quads != null && quads != oldQuads && Window != null) 
                {
                    CCSize texSize = Texture.ContentSizeInPixels;

                    // Load the quads with tex coords
                    ResetTexCoords(new CCRect(0.0f, 0.0f, texSize.Width, texSize.Height));
                }
            }
        }

        #endregion Properties


        #region Constructors

        internal CCParticleSystemQuad()
        {  
        }

        public CCParticleSystemQuad(int numberOfParticles, CCEmitterMode emitterMode=CCEmitterMode.Gravity) 
            : base(numberOfParticles, emitterMode)
        {
        }

        public CCParticleSystemQuad(CCParticleSystemConfig config) : base(config)
        {}

        public CCParticleSystemQuad(string plistFile, string directoryName = null) : base(plistFile, directoryName)
        {
            int totalPart = TotalParticles;
        }

        #endregion Constructors

        internal override void VisitRenderer()
        {
            // Add command to renderer
            // WARNING: NOT USING GLOBAL Z
            // SHOULD PROBABLY CACHE THE CCQUADCOMMAND
            var quadsCommand = new CCQuadCommand(GlobalZOrder, AffineWorldTransform, Texture, BlendFunc, ParticleCount, quads.Elements);
            Renderer.AddCommand(quadsCommand);

        }

        protected override void Draw()
        {
            Debug.Assert(BatchNode == null, "draw should not be called when added to a particleBatchNode");

            Window.DrawManager.BindTexture(Texture);
            Window.DrawManager.BlendFunc(BlendFunc);
            Window.DrawManager.DrawQuads(quads, 0, ParticleCount);
        }


        #region Updating quads

        // pointRect should be in Texture coordinates, not pixel coordinates
        void ResetTexCoords(CCRect texRectInPixels)
        {

            float wide = texRectInPixels.Size.Width;
            float high = texRectInPixels.Size.Height;

            if (Texture != null)
            {
                wide = Texture.PixelsWide;
                high = Texture.PixelsHigh;
            }

            #if CC_FIX_ARTIFACTS_BY_STRECHING_TEXEL
            float left = (texRectInPixels.Origin.X * 2 + 1) / (wide * 2);
            float bottom = (texRectInPixels.Origin.Y * 2 + 1) / (high * 2);
            float right = left + (texRectInPixels.Size.Width * 2 - 2) / (wide * 2);
            float top = bottom + (texRectInPixels.Size.Height * 2 - 2) / (high * 2);
            #else
            float left = texRectInPixels.Origin.X / wide;
            float bottom = texRectInPixels.Origin.Y / high;
            float right = left + texRectInPixels.Size.Width / wide;
            float top = bottom + texRectInPixels.Size.Height / high;
            #endif
            // ! CC_FIX_ARTIFACTS_BY_STRECHING_TEXEL

            // Important. Textures in CocosSharp are inverted, so the Y component should be inverted
            float tmp = top;
            top = bottom;
            bottom = tmp;

            CCV3F_C4B_T2F_Quad[] rawQuads;
            int start, end;
            if (BatchNode != null)
            {
                rawQuads = BatchNode.TextureAtlas.Quads.Elements;
                BatchNode.TextureAtlas.Dirty = true;
                start = AtlasIndex;
                end = AtlasIndex + TotalParticles;
            }
            else
            {
                rawQuads = Quads.Elements;
                start = 0;
                end = TotalParticles;
            }

            for (int i = start; i < end; i++)
            {
                rawQuads[i].BottomLeft.TexCoords.U = left;
                rawQuads[i].BottomLeft.TexCoords.V = bottom;
                rawQuads[i].BottomRight.TexCoords.U = right;
                rawQuads[i].BottomRight.TexCoords.V = bottom;
                rawQuads[i].TopLeft.TexCoords.U = left;
                rawQuads[i].TopLeft.TexCoords.V = top;
                rawQuads[i].TopRight.TexCoords.U = right;
                rawQuads[i].TopRight.TexCoords.V = top;
            }
        }

        void UpdateQuad(ref CCV3F_C4B_T2F_Quad quad, ref CCParticleBase particle)
        {
            CCPoint newPosition;

            if(PositionType == CCPositionType.Free || PositionType == CCPositionType.Relative)
            {
                newPosition.X = particle.Position.X - (currentPosition.X - particle.StartPosition.X);
                newPosition.Y = particle.Position.Y - (currentPosition.Y - particle.StartPosition.Y);
            }
            else
            {
                newPosition = particle.Position;
            }

            // translate newPos to correct position, since matrix transform isn't performed in batchnode
            // don't update the particle with the new position information, it will interfere with the radius and tangential calculations
            if(BatchNode != null)
            {
                newPosition.X += Position.X;
                newPosition.Y += Position.Y;
            }

            CCColor4B color = new CCColor4B();

            if(OpacityModifyRGB)
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


        public override void UpdateQuads()
        {
            if (!Visible || Layer == null)
            {
                return;
            }

            currentPosition = CCPoint.Zero;

            if (PositionType == CCPositionType.Free)
            {
                currentPosition = Layer.VisibleBoundsWorldspace.Origin;
            }
            else if (PositionType == CCPositionType.Relative)
            {
                currentPosition = Position;
            }

            CCV3F_C4B_T2F_Quad[] rawQuads;
            if (BatchNode != null)
            {
                rawQuads = BatchNode.TextureAtlas.Quads.Elements;
                BatchNode.TextureAtlas.Dirty = true;
            }
            else
            {
                rawQuads = Quads.Elements;
            }

            if (EmitterMode == CCEmitterMode.Gravity) 
            {
                UpdateGravityParticleQuads(rawQuads);
            } 
            else 
            {
                UpdateRadialParticleQuads(rawQuads);
            }
        }

        void UpdateGravityParticleQuads(CCV3F_C4B_T2F_Quad[] rawQuads)
        {
            var count = ParticleCount;
            if (BatchNode != null)
            {
                for (int i = 0; i < count; i++)
                {
                    UpdateQuad(ref rawQuads[AtlasIndex + GravityParticles[i].AtlasIndex], ref GravityParticles[i].ParticleBase);
                }
            }
            else
            {
                for (int i = 0; i < count; i++)
                {
                    UpdateQuad(ref rawQuads[i], ref GravityParticles[i].ParticleBase);
                }
            }
        }

        void UpdateRadialParticleQuads(CCV3F_C4B_T2F_Quad[] rawQuads)
        {
            var count = ParticleCount;
            if (BatchNode != null)
            {
                for (int i = 0; i < count; i++)
                {
                    UpdateQuad(ref rawQuads[AtlasIndex + RadialParticles[i].AtlasIndex], ref RadialParticles[i].ParticleBase);
                }
            }
            else
            {
                for (int i = 0; i < count; i++)
                {
                    UpdateQuad(ref rawQuads[i], ref RadialParticles[i].ParticleBase);
                }
            }
        }

        #endregion Updating quads


        public CCParticleSystemQuad Clone()
        {
            var p = new CCParticleSystemQuad(TotalParticles, EmitterMode);

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