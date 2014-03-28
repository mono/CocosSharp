using System;
using System.Diagnostics;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GZipInputStream=WP7Contrib.Communications.Compression.GZipStream; // Found in Support/Compression/GZipStream
using ICSharpCode.SharpZipLib.Zip;

namespace CocosSharp
{
    // ideas taken from:
    //     . The ocean spray in your face [Jeff Lander]
    //        http://www.double.co.nz/dust/col0798.pdf
    //     . Building an Advanced Particle System [John van der Burg]
    //        http://www.gamasutra.com/features/20000623/vanderburg_01.htm
    //   . LOVE game engine
    //        http://love2d.org/
    //
    //
    // Radius mode support, from 71 squared
    //        http://particledesigner.71squared.com/
    //
    // IMPORTANT: Particle Designer is supported by cocos2d, but
    // 'Radius Mode' in Particle Designer uses a fixed emit rate of 30 hz. Since that can't be guarateed in cocos2d,
    //  cocos2d uses a another approach, but the results are almost identical. 
    //

    public enum CCEmitterMode
    {
        Gravity,    
        Radius,
    }

    public enum CCPositionType
    {

        Free,       // Living particles are attached to the world and are unaffected by emitter repositioning.
        Relative,   // Living particles are attached to the world but will follow the emitter repositioning.
                    // Use case: Attach an emitter to an sprite, and you want that the emitter follows the sprite.
        Grouped,    // Living particles are attached to the emitter and are translated along with it.
    }


    public class CCParticleSystem : CCNode, ICCTexture
    {
        public const int ParticleDurationInfinity = -1;           
        public const int ParticleStartSizeEqualToEndSize = -1;   
        public const int ParticleStartRadiusEqualToEndRadius = -1;

        // ivars
        int totalParticles;
        CCParticleBatchNode batchNode;
        CCTexture2D texture;
        CCBlendFunc blendFunc = CCBlendFunc.AlphaBlend;
        GravityMoveMode gravityMode;
        RadialMoveMode radialMode;


        #region Structures

        protected struct GravityMoveMode
        {
            internal CCPoint Gravity { get; set; }
            internal float RadialAccel { get; set; }
            internal float RadialAccelVar { get; set; }
            internal float Speed { get; set; }
            internal float SpeedVar { get; set; }
            internal float TangentialAccel { get; set; }
            internal float TangentialAccelVar { get; set; }
            internal bool RotationIsDir { get; set; }
        }

        protected struct RadialMoveMode
        {
            internal float EndRadius { get; set; }
            internal float EndRadiusVar { get; set; }
            internal float RotatePerSecond { get; set; }
            internal float RotatePerSecondVar { get; set; }
            internal float StartRadius { get; set; }
            internal float StartRadiusVar { get; set; }
        }

        // Particle structures

        protected struct CCParticleBase
        {
            internal float TimeToLive { get; set; }
            internal float Rotation { get; set; }
            internal float Size { get; set; }
            internal float DeltaRotation { get; set; }
            internal float DeltaSize { get; set; }

            internal CCPoint Position { get; set; }
            internal CCPoint StartPosition { get; set; }

            internal CCColor4F Color { get; set; }
            internal CCColor4F DeltaColor { get; set; }
        }

        protected struct CCParticleGravity
        {
            internal int AtlasIndex { get; set; }
            internal CCParticleBase ParticleBase;

            internal CCPoint Direction { get; set; }
            internal float RadialAccel { get; set; }
            internal float TangentialAccel { get; set; }
        }

        protected struct CCParticleRadial
        {
            internal int AtlasIndex { get; set; }
            internal CCParticleBase ParticleBase;

            internal float Angle { get; set; }
            internal float Radius { get; set; }
            internal float DegreesPerSecond { get; set; }
            internal float DeltaRadius { get; set; }
        }

        #endregion Structures


        #region Properties

        public bool IsActive { get; private set; }
        public bool AutoRemoveOnFinish { get; set; }
        public bool OpacityModifyRGB { get; set; }

        protected int AllocatedParticles { get; set; }
        public int ParticleCount { get; private set; }
        public int AtlasIndex { get; set; }

        protected float Elapsed { get; private set; }
        public float Duration { get; set; }
        public float Life { get; set; }
        public float LifeVar { get; set; }
        public float Angle { get; set; }
        public float AngleVar { get; set; }
        public float StartSize { get; set; }
        public float StartSizeVar { get; set; }
        public float EndSize { get; set; }
        public float EndSizeVar { get; set; }
        public float StartSpin { get; set; }
        public float StartSpinVar { get; set; }
        public float EndSpin { get; set; }
        public float EndSpinVar { get; set; }
        public float EmissionRate { get; set; }
        protected float EmitCounter { get; set; }

        public CCPoint SourcePosition { get; set; }
        public CCPoint PositionVar { get; set; }
        public CCPositionType PositionType { get; set; }

        public CCColor4F StartColor { get; set; }
        public CCColor4F StartColorVar { get; set; }
        public CCColor4F EndColor { get; set; }
        public CCColor4F EndColorVar { get; set; }

        // We only want this to be set when system initialised
        public CCEmitterMode EmitterMode { get; private set; }

        protected CCParticleGravity[] GravityParticles { get; set; }
        protected CCParticleRadial[] RadialParticles { get; set; }


        public bool IsFull
        {
            get { return (ParticleCount == totalParticles); }
        }

        public virtual int TotalParticles
        {
            get { return totalParticles; }
            set
            {
                Debug.Assert(value <= AllocatedParticles, "Particle: resizing particle array only supported for quads");
                totalParticles = value;
            }
        }

        public CCBlendFunc BlendFunc
        {
            get { return blendFunc; }
            set
            {
                if (blendFunc.Source != value.Source || blendFunc.Destination != value.Destination)
                {
                    blendFunc = value;
                    UpdateBlendFunc();
                }
            }
        }

        public bool BlendAdditive
        {
            get { return blendFunc == CCBlendFunc.Additive; }
            set
            {
                if (value)
                {
                    blendFunc = CCBlendFunc.Additive;
                }
                else
                {
                    if (Texture != null && !Texture.HasPremultipliedAlpha)
                    {
                        blendFunc = CCBlendFunc.NonPremultiplied;
                    }
                    else
                    {
                        blendFunc = CCBlendFunc.AlphaBlend;
                    }
                }
            }
        }

        public virtual CCParticleBatchNode BatchNode
        {
            get { return batchNode; }
            set
            {
                if (batchNode != value)
                {
                    batchNode = value;

                    if (value != null)
                    {
                        if (EmitterMode == CCEmitterMode.Gravity) 
                        {
                            // each particle needs a unique index
                            for (int i = 0; i < totalParticles; i++) 
                            {
                                GravityParticles[i].AtlasIndex = i;
                            }
                        } 
                        else 
                        {
                            // each particle needs a unique index
                            for (int i = 0; i < totalParticles; i++) 
                            {
                                RadialParticles[i].AtlasIndex = i;
                            }
                        }
                    }
                }
            }
        }

        public virtual CCTexture2D Texture
        {
            get { return texture; }
            set
            {
                if (Texture != value)
                {
                    texture = value;
                    UpdateBlendFunc();
                }
            }
        }

        protected GravityMoveMode GravityMode 
        { 
            get 
            { 
                Debug.Assert(EmitterMode == CCEmitterMode.Gravity, "Particle Mode should be Gravity");
                return gravityMode; 
            }
            set { gravityMode = value; }
        }

        protected RadialMoveMode RadialMode 
        { 
            get 
            { 
                Debug.Assert(EmitterMode == CCEmitterMode.Radius, "Particle Mode should be Radius");
                return radialMode; 
            } 
            set { radialMode = value; }
        }

        // Gravity mode

        public CCPoint Gravity
        {
            get { return GravityMode.Gravity; }
            set { gravityMode.Gravity = value; }
        }

        public float TangentialAccel
        {
            get { return GravityMode.TangentialAccel; }
            set { gravityMode.TangentialAccel = value; }
        }

        public float TangentialAccelVar
        {
            get { return GravityMode.TangentialAccelVar; }
            set { gravityMode.TangentialAccelVar = value; }
        }

        public float RadialAccel
        {
            get { return GravityMode.RadialAccel; }
            set { gravityMode.RadialAccel = value; }
        }

        public float RadialAccelVar
        {
            get { return GravityMode.RadialAccelVar; }
            set { gravityMode.RadialAccelVar = value; }
        }

        public bool RotationIsDir
        {
            get { return GravityMode.RotationIsDir; }
            set { gravityMode.RotationIsDir = value; }
        }

        public float Speed
        {
            get { return GravityMode.Speed; }
            set { gravityMode.Speed = value; }
        }

        public float SpeedVar
        {
            get { return GravityMode.SpeedVar; }
            set { gravityMode.SpeedVar = value; }
        }


        // Radius mode

        public float StartRadius
        {
            get { return RadialMode.StartRadius; }
            set { radialMode.StartRadius = value; }
        }

        public float StartRadiusVar
        {
            get { return RadialMode.StartRadiusVar; }
            set { radialMode.StartRadiusVar = value; }
        }

        public float EndRadius
        {
            get { return RadialMode.EndRadius; }
            set { radialMode.EndRadius = value; }
        }

        public float EndRadiusVar
        {
            get { return RadialMode.EndRadiusVar; }
            set { radialMode.EndRadiusVar = value; }
        }

        public float RotatePerSecond
        {
            get { return RadialMode.RotatePerSecond; }
            set { radialMode.RotatePerSecond = value; }
        }

        public float RotatePerSecondVar
        {
            get { return RadialMode.RotatePerSecondVar; }
            set { radialMode.RotatePerSecondVar = value; }
        }

        #endregion Properties


        #region Constructors

        internal CCParticleSystem()
        {  
        }

		public CCParticleSystem(string plistFile, string directoryName = null) 
			: this(new CCParticleSystemConfig(plistFile, directoryName))
        {  }

        protected CCParticleSystem(int numberOfParticles, CCEmitterMode emitterMode = CCEmitterMode.Gravity) 
            : this(numberOfParticles, true, emitterMode)
        {
        }

        CCParticleSystem(int numberOfParticles, bool shouldAllocParticles, CCEmitterMode emitterMode = CCEmitterMode.Gravity)
        {
            TotalParticles = numberOfParticles;
            AllocatedParticles = numberOfParticles;

            BlendFunc = CCBlendFunc.AlphaBlend;
            PositionType = CCPositionType.Free;
            EmitterMode = emitterMode;

            IsActive = true;
            AutoRemoveOnFinish = false;

            if(shouldAllocParticles) 
            {
                if (emitterMode == CCEmitterMode.Gravity) 
                {
                    GravityParticles = new CCParticleGravity[TotalParticles];
                } 
                else 
                {
                    RadialParticles = new CCParticleRadial[TotalParticles];
                }
            }
        }

        public CCParticleSystem(CCParticleSystemConfig particleConfig) : this(particleConfig.MaxParticles, false)
        {
			Duration = particleConfig.Duration;;
			Life = particleConfig.Life;
			LifeVar = particleConfig.LifeVar;
            EmissionRate = TotalParticles / Life;

			Angle = particleConfig.Angle;
			AngleVar = particleConfig.AngleVar;

            CCBlendFunc blendFunc = new CCBlendFunc();
			blendFunc.Source = particleConfig.BlendFunc.Source;
			blendFunc.Destination = particleConfig.BlendFunc.Destination;
            BlendFunc = blendFunc;

            CCColor4F startColor = new CCColor4F();
			startColor.R = particleConfig.StartColor.R;
			startColor.G = particleConfig.StartColor.G;
			startColor.B = particleConfig.StartColor.B;
			startColor.A = particleConfig.StartColor.A;
            StartColor = startColor;

            CCColor4F startColorVar = new CCColor4F();
			startColorVar.R = particleConfig.StartColorVar.R;
			startColorVar.G = particleConfig.StartColorVar.G;
			startColorVar.B = particleConfig.StartColorVar.B;
			startColorVar.A = particleConfig.StartColorVar.A;
            StartColorVar = startColorVar;

            CCColor4F endColor = new CCColor4F();
			endColor.R = particleConfig.EndColor.R;
			endColor.G = particleConfig.EndColor.G;
			endColor.B = particleConfig.EndColor.B;
			endColor.A = particleConfig.EndColor.A;
            EndColor = endColor;

            CCColor4F endColorVar = new CCColor4F();
			endColorVar.R = particleConfig.EndColorVar.R;
			endColorVar.G = particleConfig.EndColorVar.G;
			endColorVar.B = particleConfig.EndColorVar.B;
			endColorVar.A = particleConfig.EndColorVar.A;
            EndColorVar = endColorVar;

			StartSize = particleConfig.StartSize;
			StartSizeVar = particleConfig.StartSizeVar;
			EndSize = particleConfig.EndSize;
			EndSizeVar = particleConfig.EndSizeVar;

            CCPoint position;
			position.X = particleConfig.Position.X;
			position.Y = particleConfig.Position.Y;
            Position = position;

            CCPoint positionVar;
			positionVar.X = particleConfig.PositionVar.X;
			positionVar.Y = particleConfig.PositionVar.X;
            PositionVar = positionVar;

			StartSpin = particleConfig.StartSpin;
			StartSpinVar = particleConfig.StartSpinVar;
			EndSpin = particleConfig.EndSpin;
			EndSpinVar = particleConfig.EndSpinVar;

			EmitterMode = particleConfig.EmitterMode;

            if (EmitterMode == CCEmitterMode.Gravity)
            {
                GravityParticles = new CCParticleGravity[TotalParticles];

                GravityMoveMode newGravityMode = new GravityMoveMode();

                CCPoint gravity;
				gravity.X = particleConfig.Gravity.X;
				gravity.Y = particleConfig.Gravity.Y;
                newGravityMode.Gravity = gravity;

				newGravityMode.Speed = particleConfig.GravitySpeed;
				newGravityMode.SpeedVar = particleConfig.GravitySpeedVar;
				newGravityMode.RadialAccel = particleConfig.GravityRadialAccel;
				newGravityMode.RadialAccelVar = particleConfig.GravityRadialAccelVar;
				newGravityMode.TangentialAccel = particleConfig.GravityTangentialAccel;
				newGravityMode.TangentialAccelVar = particleConfig.GravityTangentialAccelVar;
				newGravityMode.RotationIsDir = particleConfig.GravityRotationIsDir;

                GravityMode = newGravityMode;
            }
            else if (EmitterMode == CCEmitterMode.Radius)
            {
                RadialParticles = new CCParticleRadial[TotalParticles];

                RadialMoveMode newRadialMode = new RadialMoveMode();

				newRadialMode.StartRadius = particleConfig.RadialStartRadius;
				newRadialMode.StartRadiusVar = particleConfig.RadialStartRadiusVar;
				newRadialMode.EndRadius = particleConfig.RadialEndRadius;
				newRadialMode.EndRadiusVar = particleConfig.RadialEndRadiusVar;
				newRadialMode.RotatePerSecond = particleConfig.RadialRotatePerSecond;
				newRadialMode.RotatePerSecondVar = particleConfig.RadialRotatePerSecondVar;

                RadialMode = newRadialMode;
            }
            else
            {
                Debug.Assert(false, "Invalid emitterType in config file");
                return;
            }

			// Don't get the internal texture if a batchNode is used
			if (BatchNode == null)
				Texture = particleConfig.Texture;
        }

        #endregion Constructors


        public override void OnEnter()
        {
            base.OnEnter();

            Schedule(1);
        }

        public override void OnExit()
        {
            Unschedule();

            base.OnExit();
        }


        #region Particle management

        public void StopSystem()
        {
            IsActive = false;
            Elapsed = Duration;
            EmitCounter = 0;
        }

        public void ResetSystem()
        {
            IsActive = true;
            Elapsed = 0;

            if (EmitterMode == CCEmitterMode.Gravity) 
            {
                for (int i = 0; i < ParticleCount; ++i) 
                {
                    GravityParticles[i].ParticleBase.TimeToLive = 0;
                }
            } 
            else 
            {
                for (int i = 0; i < ParticleCount; ++i) 
                {
                    RadialParticles[i].ParticleBase.TimeToLive = 0;
                }
            }
        }

        // Initialise particle

        void InitParticle(ref CCParticleGravity particleGrav, ref CCParticleBase particleBase)
        {
            InitParticleBase(ref particleBase);

            // direction
            float a = MathHelper.ToRadians(Angle + AngleVar * CCRandom.Float_Minus1_1());

            if(EmitterMode == CCEmitterMode.Gravity)
            {
                CCPoint v = new CCPoint(CCMathHelper.Cos(a), CCMathHelper.Sin(a));
                float s = GravityMode.Speed + GravityMode.SpeedVar * CCRandom.Float_Minus1_1();

                particleGrav.Direction = v * s;
                particleGrav.RadialAccel = GravityMode.RadialAccel + GravityMode.RadialAccelVar * CCRandom.Float_Minus1_1();
                particleGrav.TangentialAccel = GravityMode.TangentialAccel + GravityMode.TangentialAccelVar * CCRandom.Float_Minus1_1();

                if (GravityMode.RotationIsDir)
                {
                    particleBase.Rotation = -MathHelper.ToDegrees(CCPoint.ToAngle(particleGrav.Direction));
                }
            }
        }

        void InitParticle(ref CCParticleRadial particleRadial, ref CCParticleBase particleBase)
        {
            InitParticleBase(ref particleBase);

            // direction
            float a = MathHelper.ToRadians(Angle + AngleVar * CCRandom.Float_Minus1_1());

            // Set the default diameter of the particle from the source position
            float startRadius = RadialMode.StartRadius + RadialMode.StartRadiusVar * CCRandom.Float_Minus1_1();
            float endRadius = RadialMode.EndRadius + RadialMode.EndRadiusVar * CCRandom.Float_Minus1_1();

            particleRadial.Radius = startRadius;
            particleRadial.DeltaRadius = (RadialMode.EndRadius == ParticleStartRadiusEqualToEndRadius) ? 
                0 : (endRadius - startRadius) / particleBase.TimeToLive;

            particleRadial.Angle = a;
            particleRadial.DegreesPerSecond =
                MathHelper.ToRadians(RadialMode.RotatePerSecond + RadialMode.RotatePerSecondVar * CCRandom.Float_Minus1_1());
        }

        void InitParticleBase(ref CCParticleBase particleBase)
        {
            float timeToLive = Math.Max(0, Life + LifeVar * CCRandom.Float_Minus1_1());
            particleBase.TimeToLive = timeToLive;

            CCPoint position;
            position.X = SourcePosition.X + PositionVar.X * CCRandom.Float_Minus1_1();
            position.Y = SourcePosition.Y + PositionVar.Y * CCRandom.Float_Minus1_1();
            particleBase.Position = position;

            CCColor4F start = new CCColor4F();
            start.R = MathHelper.Clamp(StartColor.R + StartColorVar.R * CCRandom.Float_Minus1_1(), 0, 1);
            start.G = MathHelper.Clamp(StartColor.G + StartColorVar.G * CCRandom.Float_Minus1_1(), 0, 1);
            start.B = MathHelper.Clamp(StartColor.B + StartColorVar.B * CCRandom.Float_Minus1_1(), 0, 1);
            start.A = MathHelper.Clamp(StartColor.A + StartColorVar.A * CCRandom.Float_Minus1_1(), 0, 1);
            particleBase.Color = start;

            CCColor4F end = new CCColor4F();
            end.R = MathHelper.Clamp(EndColor.R + EndColorVar.R * CCRandom.Float_Minus1_1(), 0, 1);
            end.G = MathHelper.Clamp(EndColor.G + EndColorVar.G * CCRandom.Float_Minus1_1(), 0, 1);
            end.B = MathHelper.Clamp(EndColor.B + EndColorVar.B * CCRandom.Float_Minus1_1(), 0, 1);
            end.A = MathHelper.Clamp(EndColor.A + EndColorVar.A * CCRandom.Float_Minus1_1(), 0, 1);

            CCColor4F deltaColor = new CCColor4F();
            deltaColor.R = (end.R - start.R) / timeToLive;
            deltaColor.G = (end.G - start.G) / timeToLive;
            deltaColor.B = (end.B - start.B) / timeToLive;
            deltaColor.A = (end.A - start.A) / timeToLive;
            particleBase.DeltaColor = deltaColor;

            float startSize = Math.Max(0, StartSize + StartSizeVar * CCRandom.Float_Minus1_1());
            particleBase.Size = startSize;

            if (EndSize == ParticleStartSizeEqualToEndSize)
            {
                particleBase.DeltaSize = 0;
            }
            else
            {
                float endS = EndSize + EndSizeVar * CCRandom.Float_Minus1_1();
                endS = Math.Max(0, endS);
                particleBase.DeltaSize = (endS - startSize) / timeToLive;
            }

            float startSpin = StartSpin + StartSpinVar * CCRandom.Float_Minus1_1();
            float endSpin = EndSpin + EndSpinVar * CCRandom.Float_Minus1_1();
            particleBase.Rotation = startSpin;
            particleBase.DeltaRotation = (endSpin - startSpin) / timeToLive;

            if (PositionType == CCPositionType.Free)
            {
                particleBase.StartPosition = ConvertToWorldSpace(CCPoint.Zero);
            }
            else if (PositionType == CCPositionType.Relative)
            {
                particleBase.StartPosition = m_obPosition;
            }
        }

        // Update particle

        bool UpdateParticleBase(ref CCParticleBase particleBase, float dt)
        {
            particleBase.TimeToLive -= dt;

            if(particleBase.TimeToLive > 0)
            {
                CCColor4F color = particleBase.Color;
                color.R += (particleBase.DeltaColor.R * dt);
                color.G += (particleBase.DeltaColor.G * dt);
                color.B += (particleBase.DeltaColor.B * dt);
                color.A += (particleBase.DeltaColor.A * dt);
                particleBase.Color = color;

                particleBase.Size += (particleBase.DeltaSize * dt);
                if(particleBase.Size < 0)
                {
                    particleBase.Size = 0;
                }

                particleBase.Rotation += (particleBase.DeltaRotation * dt);

                return true;
            }

            return false;
        }

        bool UpdateParticle(ref CCParticleRadial particleRadial, float dt)
        {
            if(UpdateParticleBase(ref particleRadial.ParticleBase, dt))
            {
                particleRadial.Angle += particleRadial.DegreesPerSecond * dt;
                particleRadial.Radius += particleRadial.DeltaRadius * dt;

                CCPoint position = particleRadial.ParticleBase.Position;
                position.X = -CCMathHelper.Cos(particleRadial.Angle) * particleRadial.Radius;
                position.Y = -CCMathHelper.Sin(particleRadial.Angle) * particleRadial.Radius;
                particleRadial.ParticleBase.Position = position;

                return true;
            }

            return false;
        }

        bool UpdateParticle(ref CCParticleGravity particleGrav, float dt)
        {
            if(UpdateParticleBase(ref particleGrav.ParticleBase, dt)) 
            {
                float radial_x = 0;
                float radial_y = 0;

                float tmp_x, tmp_y;
                float tangential_x, tangential_y;

                CCPoint pos = particleGrav.ParticleBase.Position;
                float x = pos.X;
                float y = pos.Y;

                if (x != 0 || y != 0)
                {
                    float l = 1.0f / (float) Math.Sqrt(x * x + y * y);

                    radial_x = x * l;
                    radial_y = y * l;
                }

                tangential_x = radial_x;
                tangential_y = radial_y;

                float radialAccel = particleGrav.RadialAccel;
                radial_x *= radialAccel;
                radial_y *= radialAccel;

                float tangentAccel = particleGrav.TangentialAccel;
                float newy = tangential_x;
                tangential_x = -tangential_y;
                tangential_y = newy;
                tangential_x *= tangentAccel;
                tangential_y *= tangentAccel;

                CCPoint gravity = GravityMode.Gravity;
                tmp_x = (radial_x + tangential_x + gravity.X) * dt;
                tmp_y = (radial_y + tangential_y + gravity.Y) * dt;

                CCPoint direction = particleGrav.Direction;
                direction.X += tmp_x;
                direction.Y += tmp_y;
                particleGrav.Direction = direction;

                CCPoint position = particleGrav.ParticleBase.Position;
                position.X += direction.X * dt;
                position.Y += direction.Y * dt;
                particleGrav.ParticleBase.Position = position;

                return true;
            }

            return false;
        }

        #endregion Particle management


        #region Updating system

        public void UpdateWithNoTime()
        {
            Update(0.0f);
        }

        public virtual void UpdateQuads()
        {
            // should be overriden
        }

        protected virtual void PostStep()
        {
            // should be overriden
        }

        public override void Update(float dt)
        {
            if (EmitterMode == CCEmitterMode.Gravity) 
            {
                UpdateGravityParticles(dt);
            } 
            else 
            {
                UpdateRadialParticles(dt);
            }

            UpdateQuads();

            if (BatchNode == null)
            {
                PostStep();
            }
        }

        void UpdateGravityParticles(float dt)
        {
            if (IsActive && EmissionRate > 0)
            {
                float rate = 1.0f / EmissionRate;

                //issue #1201, prevent bursts of particles, due to too high emitCounter
                if (ParticleCount < TotalParticles)
                {
                    EmitCounter += dt;
                }

                while (ParticleCount < TotalParticles && EmitCounter > rate)
                {
                    InitParticle(ref GravityParticles[ParticleCount], ref GravityParticles[ParticleCount].ParticleBase);
                    ++ParticleCount;
                    EmitCounter -= rate;
                }

                Elapsed += dt;

                if (Duration != -1 && Duration < Elapsed)
                {
                    StopSystem();
                }
            }

            int index = 0;

            if (Visible)
            {
                while (index < ParticleCount)
                {
                    if (UpdateParticle(ref GravityParticles[index], dt))
                    {
                        // update particle counter
                        ++index;
                    }
                    else
                    {
                        // life < 0
                        int currentIndex = GravityParticles[index].AtlasIndex;
                        if (index != ParticleCount - 1)
                        {
                            GravityParticles[index] = GravityParticles[ParticleCount - 1];
                        }

                        if (BatchNode != null)
                        {
                            //disable the switched particle
                            BatchNode.DisableParticle(AtlasIndex + currentIndex);

                            //switch indexes
                            GravityParticles[ParticleCount - 1].AtlasIndex = currentIndex;
                        }

                        --ParticleCount;

                        if (ParticleCount == 0 && AutoRemoveOnFinish)
                        {
                            Unschedule();
                            Parent.RemoveChild(this, true);
                            return;
                        }
                    }
                }
            }
        }

        void UpdateRadialParticles(float dt)
        {
            if (IsActive && EmissionRate > 0)
            {
                float rate = 1.0f / EmissionRate;

                //issue #1201, prevent bursts of particles, due to too high emitCounter
                if (ParticleCount < TotalParticles)
                {
                    EmitCounter += dt;
                }

                while (ParticleCount < TotalParticles && EmitCounter > rate)
                {
                    InitParticle(ref RadialParticles[ParticleCount], ref RadialParticles[ParticleCount].ParticleBase);
                    ++ParticleCount;
                    EmitCounter -= rate;
                }

                Elapsed += dt;

                if (Duration != -1 && Duration < Elapsed)
                {
                    StopSystem();
                }
            }

            int index = 0;

            if (Visible)
            {
                while (index < ParticleCount)
                {
                    if (UpdateParticle(ref RadialParticles[index], dt))
                    {
                        // update particle counter
                        ++index;
                    }
                    else
                    {
                        // life < 0
                        int currentIndex = RadialParticles[index].AtlasIndex;
                        if (index != ParticleCount - 1)
                        {
                            RadialParticles[index] = RadialParticles[ParticleCount - 1];
                        }

                        if (BatchNode != null)
                        {
                            //disable the switched particle
                            BatchNode.DisableParticle(AtlasIndex + currentIndex);

                            //switch indexes
                            RadialParticles[ParticleCount - 1].AtlasIndex = currentIndex;
                        }

                        --ParticleCount;

                        if (ParticleCount == 0 && AutoRemoveOnFinish)
                        {
                            Unschedule();
                            Parent.RemoveChild(this, true);
                            return;
                        }
                    }
                }
            }
        }

        void UpdateBlendFunc()
        {
            Debug.Assert(BatchNode == null, "Can't change blending functions when the particle is being batched");

            if (Texture != null)
            {
                bool premultiplied = Texture.HasPremultipliedAlpha;

                OpacityModifyRGB = false;

                if (blendFunc == CCBlendFunc.AlphaBlend)
                {
                    if (premultiplied)
                    {
                        OpacityModifyRGB = true;
                    }
                    else
                    {
                        blendFunc = CCBlendFunc.NonPremultiplied;
                    }
                }
            }
        }

        #endregion Updating system
    }
}