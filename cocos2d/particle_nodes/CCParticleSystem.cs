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

        protected struct CCParticle
        {
            internal int AtlasIndex { get; set; }

            internal float TimeToLive { get; set; }
            internal float Rotation { get; set; }
            internal float Size { get; set; }
            internal float DeltaRotation { get; set; }
            internal float DeltaSize { get; set; }

            internal CCPoint Position { get; set; }
            internal CCPoint StartPosition { get; set; }

            internal CCColor4F Color { get; set; }
            internal CCColor4F DeltaColor { get; set; }

            internal GravityParticleMoveMode GravityParticleMode { get; set; }
            internal RadialParticleMoveMode RadialParticleMode { get; set; }

            internal struct GravityParticleMoveMode
            {
                internal CCPoint Direction { get; set; }
                internal float RadialAccel { get; set; }
                internal float TangentialAccel { get; set; }
            }

            internal struct RadialParticleMoveMode
            {
                internal float Angle { get; set; }
                internal float Radius { get; set; }
                internal float DegreesPerSecond { get; set; }
                internal float DeltaRadius { get; set; }
            }
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

        public CCEmitterMode EmitterMode { get; set; }

        protected CCParticle[] Particles { get; set; }

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
                        // each particle needs a unique index
                        for (int i = 0; i < totalParticles; i++)
                        {
                            Particles[i].AtlasIndex = i;
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

        public CCParticleSystem(string plistFile) 
            : this(CCContentManager.SharedContentManager.Load<PlistDocument>(plistFile).Root.AsDictionary)
        {
        }

        protected CCParticleSystem(int numberOfParticles)
        {
            TotalParticles = numberOfParticles;
            AllocatedParticles = numberOfParticles;

            BlendFunc = CCBlendFunc.AlphaBlend;
            PositionType = CCPositionType.Free;
            EmitterMode = CCEmitterMode.Gravity;

            IsActive = true;
            AutoRemoveOnFinish = false;

            Particles = new CCParticle[TotalParticles];
        }

        CCParticleSystem(PlistDictionary dictionary) : this(dictionary["maxParticles"].AsInt)
        {
            Duration = dictionary["duration"].AsFloat;
            Life = dictionary["particleLifespan"].AsFloat;
            LifeVar = dictionary["particleLifespanVariance"].AsFloat;
            EmissionRate = TotalParticles / Life;

            Angle = dictionary["angle"].AsFloat;
            AngleVar = dictionary["angleVariance"].AsFloat;

            CCBlendFunc blendFunc;
            blendFunc.Source = dictionary["blendFuncSource"].AsInt;
            blendFunc.Destination = dictionary["blendFuncDestination"].AsInt;
            BlendFunc = blendFunc;

            CCColor4F startColor = new CCColor4F();
            startColor.R = dictionary["startColorRed"].AsFloat;
            startColor.G = dictionary["startColorGreen"].AsFloat;
            startColor.B = dictionary["startColorBlue"].AsFloat;
            startColor.A = dictionary["startColorAlpha"].AsFloat;
            StartColor = startColor;

            CCColor4F startColorVar = new CCColor4F();
            startColorVar.R = dictionary["startColorVarianceRed"].AsFloat;
            startColorVar.G = dictionary["startColorVarianceGreen"].AsFloat;
            startColorVar.B = dictionary["startColorVarianceBlue"].AsFloat;
            startColorVar.A = dictionary["startColorVarianceAlpha"].AsFloat;
            StartColorVar = startColorVar;

            CCColor4F endColor = new CCColor4F();
            endColor.R = dictionary["finishColorRed"].AsFloat;
            endColor.G = dictionary["finishColorGreen"].AsFloat;
            endColor.B = dictionary["finishColorBlue"].AsFloat;
            endColor.A = dictionary["finishColorAlpha"].AsFloat;
            EndColor = endColor;

            CCColor4F endColorVar = new CCColor4F();
            endColorVar.R = dictionary["finishColorVarianceRed"].AsFloat;
            endColorVar.G = dictionary["finishColorVarianceGreen"].AsFloat;
            endColorVar.B = dictionary["finishColorVarianceBlue"].AsFloat;
            endColorVar.A = dictionary["finishColorVarianceAlpha"].AsFloat;
            EndColorVar = endColorVar;

            StartSize = dictionary["startParticleSize"].AsFloat;
            StartSizeVar = dictionary["startParticleSizeVariance"].AsFloat;
            EndSize = dictionary["finishParticleSize"].AsFloat;
            EndSizeVar = dictionary["finishParticleSizeVariance"].AsFloat;

            CCPoint position;
            position.X = dictionary["sourcePositionx"].AsFloat;
            position.Y = dictionary["sourcePositiony"].AsFloat;
            Position = position;

            CCPoint positionVar;
            positionVar.X = dictionary["sourcePositionVariancex"].AsFloat;
            positionVar.Y = dictionary["sourcePositionVariancey"].AsFloat;
            PositionVar = positionVar;

            StartSpin = dictionary["rotationStart"].AsFloat;
            StartSpinVar = dictionary["rotationStartVariance"].AsFloat;
            EndSpin = dictionary["rotationEnd"].AsFloat;
            EndSpinVar = dictionary["rotationEndVariance"].AsFloat;

            EmitterMode = (CCEmitterMode)dictionary["emitterType"].AsInt;

            if (EmitterMode == CCEmitterMode.Gravity)
            {
                GravityMoveMode newGravityMode = new GravityMoveMode();

                CCPoint gravity;
                gravity.X = dictionary["gravityx"].AsFloat;
                gravity.Y = dictionary["gravityy"].AsFloat;
                newGravityMode.Gravity = gravity;

                newGravityMode.Speed = dictionary["speed"].AsFloat;
                newGravityMode.SpeedVar = dictionary["speedVariance"].AsFloat;
                newGravityMode.RadialAccel = dictionary["radialAcceleration"].AsFloat;
                newGravityMode.RadialAccelVar = dictionary["radialAccelVariance"].AsFloat;
                newGravityMode.TangentialAccel = dictionary["tangentialAcceleration"].AsFloat;
                newGravityMode.TangentialAccelVar = dictionary["tangentialAccelVariance"].AsFloat;
                newGravityMode.RotationIsDir = dictionary["rotationIsDir"].AsBool;

                GravityMode = newGravityMode;
            }

            else if (EmitterMode == CCEmitterMode.Radius)
            {
                RadialMoveMode newRadialMode = new RadialMoveMode();

                newRadialMode.StartRadius = dictionary["maxRadius"].AsFloat;
                newRadialMode.StartRadiusVar = dictionary["maxRadiusVariance"].AsFloat;
                newRadialMode.EndRadius = dictionary["minRadius"].AsFloat;
                newRadialMode.EndRadiusVar = 0.0f;
                newRadialMode.RotatePerSecond = dictionary["rotatePerSecond"].AsFloat;
                newRadialMode.RotatePerSecondVar = dictionary["rotatePerSecondVariance"].AsFloat;

                RadialMode = newRadialMode;
            }
            else
            {
                Debug.Assert(false, "Invalid emitterType in config file");
                return;
            }

            LoadParticleTexture(dictionary);
        }

        void LoadParticleTexture(PlistDictionary dictionary)
        {
            // Don't get the internal texture if a batchNode is used
            if (BatchNode == null)
            {
                OpacityModifyRGB = false;

                string textureName = dictionary["textureFileName"].AsString;

                CCTexture2D tex = null;

                if (!string.IsNullOrEmpty(textureName))
                {
                    bool bNotify = CCFileUtils.IsPopupNotify;
                    CCFileUtils.IsPopupNotify = false;
                    try
                    {
                        tex = CCTextureCache.SharedTextureCache.AddImage(textureName);
                    }
                    catch (Exception)
                    {
                        tex = null;
                    }

                    CCFileUtils.IsPopupNotify = bNotify;
                }

                if (tex != null)
                {
                    Texture = tex;
                }
                else
                {
                    string textureData = dictionary["textureImageData"].AsString;
                    Debug.Assert(!string.IsNullOrEmpty(textureData), 
                        string.Format("CCParticleSystem: textureData does not exist : {0}",textureName));

                    int dataLen = textureData.Length;
                    if (dataLen != 0)
                    {

                        var dataBytes = Convert.FromBase64String(textureData);
                        Debug.Assert(dataBytes != null, 
                            string.Format("CCParticleSystem: error decoding textureImageData : {0}",textureName));

                        var imageBytes = Inflate(dataBytes);
                        Debug.Assert(imageBytes != null, 
                            string.Format("CCParticleSystem: error init image with Data for texture : {0}",textureName));

                        try
                        {
                            Texture = CCTextureCache.SharedTextureCache.AddImage(imageBytes, textureName, SurfaceFormat.Color);
                        }
                        catch (Exception ex)
                        {
                            CCLog.Log(ex.ToString());
                            Texture = CCParticleExample.DefaultTexture;
                        }
                    }
                }

                Debug.Assert(Texture != null, 
                    string.Format("CCParticleSystem: error loading the texture : {0}", textureName));
            }
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
            for (int i = 0; i < ParticleCount; ++i)
            {
                Particles[i].TimeToLive = 0;
            }
        }

        bool AddParticle()
        {
            if (IsFull)
            {
                return false;
            }

            InitParticle(ref Particles[ParticleCount]);
            ++ParticleCount;

            return true;
        }

        void InitParticle(ref CCParticle particle)
        {
            float timeToLive = Math.Max(0, Life + LifeVar * CCRandom.Float_Minus1_1());
            particle.TimeToLive = timeToLive;

            CCPoint position;
            position.X = SourcePosition.X + PositionVar.X * CCRandom.Float_Minus1_1();
            position.Y = SourcePosition.Y + PositionVar.Y * CCRandom.Float_Minus1_1();
            particle.Position = position;

            CCColor4F start = new CCColor4F();
            start.R = MathHelper.Clamp(StartColor.R + StartColorVar.R * CCRandom.Float_Minus1_1(), 0, 1);
            start.G = MathHelper.Clamp(StartColor.G + StartColorVar.G * CCRandom.Float_Minus1_1(), 0, 1);
            start.B = MathHelper.Clamp(StartColor.B + StartColorVar.B * CCRandom.Float_Minus1_1(), 0, 1);
            start.A = MathHelper.Clamp(StartColor.A + StartColorVar.A * CCRandom.Float_Minus1_1(), 0, 1);
            particle.Color = start;

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
            particle.DeltaColor = deltaColor;

            float startSize = Math.Max(0, StartSize + StartSizeVar * CCRandom.Float_Minus1_1());
            particle.Size = startSize;

            if (EndSize == ParticleStartSizeEqualToEndSize)
            {
                particle.DeltaSize = 0;
            }
            else
            {
                float endS = EndSize + EndSizeVar * CCRandom.Float_Minus1_1();
                endS = Math.Max(0, endS);
                particle.DeltaSize = (endS - startSize) / timeToLive;
            }

            float startSpin = StartSpin + StartSpinVar * CCRandom.Float_Minus1_1();
            float endSpin = EndSpin + EndSpinVar * CCRandom.Float_Minus1_1();
            particle.Rotation = startSpin;
            particle.DeltaRotation = (endSpin - startSpin) / timeToLive;

            if (PositionType == CCPositionType.Free)
            {
                particle.StartPosition = ConvertToWorldSpace(CCPoint.Zero);
            }
            else if (PositionType == CCPositionType.Relative)
            {
                particle.StartPosition = m_obPosition;
            }

            // direction
            float a = MathHelper.ToRadians(Angle + AngleVar * CCRandom.Float_Minus1_1());

            if(EmitterMode == CCEmitterMode.Gravity)
            {
                CCPoint v = new CCPoint(CCMathHelper.Cos(a), CCMathHelper.Sin(a));
                float s = GravityMode.Speed + GravityMode.SpeedVar * CCRandom.Float_Minus1_1();

                CCParticle.GravityParticleMoveMode gravParticleMode = new CCParticle.GravityParticleMoveMode();
                gravParticleMode.Direction = v * s;
                gravParticleMode.RadialAccel = GravityMode.RadialAccel + GravityMode.RadialAccelVar * CCRandom.Float_Minus1_1();
                gravParticleMode.TangentialAccel = GravityMode.TangentialAccel + GravityMode.TangentialAccelVar * CCRandom.Float_Minus1_1();
                particle.GravityParticleMode = gravParticleMode;

                if (GravityMode.RotationIsDir)
                {
                    particle.Rotation = -MathHelper.ToDegrees(CCPoint.ToAngle(particle.GravityParticleMode.Direction));
                }
            }

            else
            {
                // Set the default diameter of the particle from the source position
                float startRadius = RadialMode.StartRadius + RadialMode.StartRadiusVar * CCRandom.Float_Minus1_1();
                float endRadius = RadialMode.EndRadius + RadialMode.EndRadiusVar * CCRandom.Float_Minus1_1();

                CCParticle.RadialParticleMoveMode radialParticleMode = new CCParticle.RadialParticleMoveMode();
                radialParticleMode.Radius = startRadius;

                radialParticleMode.DeltaRadius = (RadialMode.EndRadius == ParticleStartRadiusEqualToEndRadius) ? 
                    0 : (endRadius - startRadius) / timeToLive;
                    
                radialParticleMode.Angle = a;
                radialParticleMode.DegreesPerSecond =
                    MathHelper.ToRadians(RadialMode.RotatePerSecond + RadialMode.RotatePerSecondVar * CCRandom.Float_Minus1_1());

                particle.RadialParticleMode = radialParticleMode;
            }
        }

        bool UpdateParticle(ref CCParticle p, float dt)
        {
            p.TimeToLive -= dt;

            if (p.TimeToLive > 0)
            {
                if (EmitterMode == CCEmitterMode.Gravity)
                {
                    float radial_x = 0;
                    float radial_y = 0;

                    float tmp_x, tmp_y;
                    float tangential_x, tangential_y;

                    CCPoint pos = p.Position;
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

                    CCParticle.GravityParticleMoveMode gravityMode = p.GravityParticleMode;

                    float radialAccel = gravityMode.RadialAccel;
                    radial_x *= radialAccel;
                    radial_y *= radialAccel;

                    float tangentAccel = gravityMode.TangentialAccel;
                    float newy = tangential_x;
                    tangential_x = -tangential_y;
                    tangential_y = newy;
                    tangential_x *= tangentAccel;
                    tangential_y *= tangentAccel;

                    CCPoint gravity = GravityMode.Gravity;
                    tmp_x = (radial_x + tangential_x + gravity.X) * dt;
                    tmp_y = (radial_y + tangential_y + gravity.Y) * dt;

                    CCPoint direction = gravityMode.Direction;
                    direction.X += tmp_x;
                    direction.Y += tmp_y;
                    gravityMode.Direction = direction;
                    p.GravityParticleMode = gravityMode;

                    CCPoint position = p.Position;
                    position.X += direction.X * dt;
                    position.Y += direction.Y * dt;
                    p.Position = position;
                }

                else
                {
                    CCParticle.RadialParticleMoveMode radialMode = p.RadialParticleMode;

                    radialMode.Angle += radialMode.DegreesPerSecond * dt;
                    radialMode.Radius += radialMode.DeltaRadius * dt;

                    CCPoint position = p.Position;
                    position.X = -CCMathHelper.Cos(radialMode.Angle) * radialMode.Radius;
                    position.Y = -CCMathHelper.Sin(radialMode.Angle) * radialMode.Radius;
                    p.Position = position;

                    p.RadialParticleMode = radialMode;
                }

                CCColor4F color = p.Color;
                color.R += (p.DeltaColor.R * dt);
                color.G += (p.DeltaColor.G * dt);
                color.B += (p.DeltaColor.B * dt);
                color.A += (p.DeltaColor.A * dt);
                p.Color = color;

                p.Size += (p.DeltaSize * dt);
                if (p.Size < 0)
                {
                    p.Size = 0;
                }

                p.Rotation += (p.DeltaRotation * dt);

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

        // ParticleSystem - MainLoop
        public override void Update(float dt)
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
                    InitParticle(ref Particles[ParticleCount]);
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
                    if (UpdateParticle(ref Particles[index], dt))
                    {
                        // update particle counter
                        ++index;
                    }
                    else
                    {
                        // life < 0
                        int currentIndex = Particles[index].AtlasIndex;
                        if (index != ParticleCount - 1)
                        {
                            Particles[index] = Particles[ParticleCount - 1];
                        }

                        if (BatchNode != null)
                        {
                            //disable the switched particle
                            BatchNode.DisableParticle(AtlasIndex + currentIndex);

                            //switch indexes
                            Particles[ParticleCount - 1].AtlasIndex = currentIndex;
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

            UpdateQuads();

            if (BatchNode == null)
            {
                PostStep();
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

        /// <summary>
        /// Decompresses the given data stream from its source ZIP or GZIP format.
        /// </summary>
        /// <param name="dataBytes"></param>
        /// <returns></returns>
        private static byte[] Inflate(byte[] dataBytes)
        {

            byte[] outputBytes = null;
            var zipInputStream = new ZipInputStream(new MemoryStream(dataBytes));

            if (zipInputStream.CanDecompressEntry) 
            {
                MemoryStream zipoutStream = new MemoryStream();
#if XBOX
                byte[] buf = new byte[4096];
                int amt = -1;
                while (true)
                {
                    amt = zipInputStream.Read(buf, 0, buf.Length);
                    if (amt == -1)
                    {
                        break;
                    }
                    zipoutStream.Write(buf, 0, amt);
                }
#else
                zipInputStream.CopyTo(zipoutStream);
#endif
                outputBytes = zipoutStream.ToArray();
            }
            else 
            {
                try 
                {
                    var gzipInputStream = new GZipInputStream(new MemoryStream(dataBytes));

                    MemoryStream zipoutStream = new MemoryStream();

#if XBOX
                    byte[] buf = new byte[4096];
                    int amt = -1;
                    while (true)
                    {
                        amt = gzipInputStream.Read(buf, 0, buf.Length);
                        if (amt == -1)
                        {
                            break;
                        }
                        zipoutStream.Write(buf, 0, amt);
                    }
#else
                    gzipInputStream.CopyTo(zipoutStream);
#endif
                    outputBytes = zipoutStream.ToArray();
                }
                    
                catch (Exception exc)
                {
                    CCLog.Log("Error decompressing image data: " + exc.Message);
                }
            }

            return outputBytes;
        }
    }
}