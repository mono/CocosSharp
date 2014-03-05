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

    /**
    Structure that contains the values of each particle
    */

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
        CircularMoveMode circularMode;


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

        protected struct CircularMoveMode
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
            internal CCColor4F color;
            internal CCColor4F deltaColor;

            internal float deltaRotation;
            internal float deltaSize;

            public ModeA modeA;
            public ModeB modeB;
            public CCPoint pos;
            public float rotation;
            public float size;
            public CCPoint startPos;
            public float timeToLive;

            // Mode A: gravity, direction, radial accel, tangential accel

            public struct ModeA
            {
                public CCPoint dir;
                public float radialAccel;
                public float tangentialAccel;
            };
                
            // Mode B: radius mode

            public struct ModeB
            {
                public float angle;
                public float degreesPerSecond;
                public float deltaRadius;
                public float radius;
            }
        }

        #endregion Structures


        #region Properties

        public bool IsActive { get; private set; }
        public bool AutoRemoveOnFinish { get; set; }
        public bool OpacityModifyRGB { get; set; }

        protected int AllocatedParticles { get; private set; }
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

        public CCBlendFunc BlendFunc
        {
            get { return blendFunc; }
            set
            {
                if (blendFunc.Source != value.Source || blendFunc.Destination != value.Destination)
                {
                    blendFunc = value;
                    updateBlendFunc();
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

        public virtual int TotalParticles
        {
            get { return totalParticles; }
            set
            {
                Debug.Assert(value <= AllocatedParticles, "Particle: resizing particle array only supported for quads");
                totalParticles = value;
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

        // We want to have an explicit ivar for these modes so that
        // we can set individual structure fields without having to copy the entire struct
        protected GravityMoveMode GravityMode 
        { 
            get 
            { 
                Debug.Assert(EmitterMode == CCEmitterMode.Gravity, "Particle Mode should be Gravity");
                return gravityMode; 
            }
            set { gravityMode = value; }
        }

        protected CircularMoveMode CircularMode 
        { 
            get { return circularMode; } 
            set { circularMode = value; }
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
            get
            {
                Debug.Assert(EmitterMode == CCEmitterMode.Radius, "Particle Mode should be Radius");
                return CircularMode.StartRadius;
            }
            set
            {
                Debug.Assert(EmitterMode == CCEmitterMode.Radius, "Particle Mode should be Radius");
                circularMode.StartRadius = value;
            }
        }

        public float StartRadiusVar
        {
            get
            {
                Debug.Assert(EmitterMode == CCEmitterMode.Radius, "Particle Mode should be Radius");
                return CircularMode.StartRadiusVar;
            }
            set
            {
                Debug.Assert(EmitterMode == CCEmitterMode.Radius, "Particle Mode should be Radius");
                circularMode.StartRadiusVar = value;
            }
        }

        public float EndRadius
        {
            get
            {
                Debug.Assert(EmitterMode == CCEmitterMode.Radius, "Particle Mode should be Radius");
                return CircularMode.EndRadius;
            }
            set
            {
                Debug.Assert(EmitterMode == CCEmitterMode.Radius, "Particle Mode should be Radius");
                circularMode.EndRadius = value;
            }
        }

        public float EndRadiusVar
        {
            get
            {
                Debug.Assert(EmitterMode == CCEmitterMode.Radius, "Particle Mode should be Radius");
                return CircularMode.EndRadiusVar;
            }
            set
            {
                Debug.Assert(EmitterMode == CCEmitterMode.Radius, "Particle Mode should be Radius");
                circularMode.EndRadiusVar = value;
            }
        }

        public float RotatePerSecond
        {
            get
            {
                Debug.Assert(EmitterMode == CCEmitterMode.Radius, "Particle Mode should be Radius");
                return CircularMode.RotatePerSecond;
            }
            set
            {
                Debug.Assert(EmitterMode == CCEmitterMode.Radius, "Particle Mode should be Radius");
                circularMode.RotatePerSecond = value;
            }
        }

        public float RotatePerSecondVar
        {
            get
            {
                Debug.Assert(EmitterMode == CCEmitterMode.Radius, "Particle Mode should be Radius");
                return CircularMode.RotatePerSecondVar;
            }
            set
            {
                Debug.Assert(EmitterMode == CCEmitterMode.Radius, "Particle Mode should be Radius");
                circularMode.RotatePerSecondVar = value;
            }
        }

        #endregion Properties


        #region Constructors

        internal CCParticleSystem()
        {  
        }

        public CCParticleSystem(string plistFile)
        {
            PlistDocument doc = CCContentManager.SharedContentManager.Load<PlistDocument>(plistFile);
            InitCCParticleSystem(doc.Root.AsDictionary);
        }

        protected CCParticleSystem(int numberOfParticles)
        {
            InitWithTotalParticles(numberOfParticles);
        }

        private void InitCCParticleSystem(PlistDictionary dictionary)
        {
            int maxParticles = dictionary["maxParticles"].AsInt;

            InitWithTotalParticles(maxParticles);

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

            CCColor4F startColor;
            startColor.R = dictionary["startColorRed"].AsFloat;
            startColor.G = dictionary["startColorGreen"].AsFloat;
            startColor.B = dictionary["startColorBlue"].AsFloat;
            startColor.A = dictionary["startColorAlpha"].AsFloat;
            StartColor = startColor;

            CCColor4F startColorVar;
            startColorVar.R = dictionary["startColorVarianceRed"].AsFloat;
            startColorVar.G = dictionary["startColorVarianceGreen"].AsFloat;
            startColorVar.B = dictionary["startColorVarianceBlue"].AsFloat;
            startColorVar.A = dictionary["startColorVarianceAlpha"].AsFloat;
            StartColorVar = startColorVar;

            CCColor4F endColor;
            endColor.R = dictionary["finishColorRed"].AsFloat;
            endColor.G = dictionary["finishColorGreen"].AsFloat;
            endColor.B = dictionary["finishColorBlue"].AsFloat;
            endColor.A = dictionary["finishColorAlpha"].AsFloat;
            EndColor = endColor;

            CCColor4F endColorVar;
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

            EmitterMode = (CCEmitterMode) dictionary["emitterType"].AsInt;

            // Mode A: Gravity + tangential accel + radial accel
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

                // or Mode B: radius movement
            else if (EmitterMode == CCEmitterMode.Radius)
            {
                CircularMoveMode newCircularMode = new CircularMoveMode();
                newCircularMode.StartRadius = dictionary["maxRadius"].AsFloat;
                newCircularMode.StartRadiusVar = dictionary["maxRadiusVariance"].AsFloat;
                newCircularMode.EndRadius = dictionary["minRadius"].AsFloat;
                newCircularMode.EndRadiusVar = 0.0f;
                newCircularMode.RotatePerSecond = dictionary["rotatePerSecond"].AsFloat;
                newCircularMode.RotatePerSecondVar = dictionary["rotatePerSecondVariance"].AsFloat;
            }
            else
            {
                Debug.Assert(false, "Invalid emitterType in config file");
                return;
            }


            // Don't get the internal texture if a batchNode is used
            if (BatchNode == null)
            {
                // Set a compatible default for the alpha transfer
                OpacityModifyRGB = false;

                // texture        
                // Try to get the texture from the cache
                string textureName = dictionary["textureFileName"].AsString;

                CCTexture2D tex = null;

                if (!string.IsNullOrEmpty(textureName))
                {
                    // set not pop-up message box when load image failed
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

                    // reset the value of UIImage notify
                    CCFileUtils.IsPopupNotify = bNotify;
                }

                if (tex != null)
                {
                    Texture = tex;
                }
                else
                {
                    string textureData = dictionary["textureImageData"].AsString;
                    Debug.Assert(!string.IsNullOrEmpty(textureData), string.Format("CCParticleSystem: textureData does not exist : {0}",textureName));

                    int dataLen = textureData.Length;
                    if (dataLen != 0)
                    {

                        var dataBytes = Convert.FromBase64String(textureData);
                        Debug.Assert(dataBytes != null, string.Format("CCParticleSystem: error decoding textureImageData : {0}",textureName));

                        var imageBytes = Inflate(dataBytes);
                        Debug.Assert(imageBytes != null, string.Format("CCParticleSystem: error init image with Data for texture : {0}",textureName));

                        try
                        {
                            Texture = CCTextureCache.SharedTextureCache.AddImage(imageBytes, textureName, SurfaceFormat.Color);
                        }
                        catch (Exception ex)
                        {
                            CCLog.Log(ex.ToString());
                            Texture = CCParticleExample.DefaultTexture;
                            //throw (new NotSupportedException("Embedded textureImageData is a format that this platform does not understand. Use PNG, GIF, or JPEG for your particle systems."));
                        }
                    }
                }

                Debug.Assert(Texture != null, string.Format("CCParticleSystem: error loading the texture : {0}", textureName));
            }
        }

        protected virtual void InitWithTotalParticles(int numberOfParticles)
        {
            TotalParticles = numberOfParticles;
            AllocatedParticles = numberOfParticles;

            BlendFunc = CCBlendFunc.AlphaBlend;
            PositionType = CCPositionType.Free;
            EmitterMode = CCEmitterMode.Gravity;

            IsActive = true;
            AutoRemoveOnFinish = false;

            Particles = new CCParticle[TotalParticles];

            if (BatchNode != null)
            {
                for (int i = 0; i < TotalParticles; i++)
                {
                    Particles[i].AtlasIndex = i;
                }
            }
        }

        #endregion Constructors


        /// <summary>
        /// Decompresses the given data stream from its source ZIP or GZIP format.
        /// </summary>
        /// <param name="dataBytes"></param>
        /// <returns></returns>
        private static byte[] Inflate(byte[] dataBytes)
        {

            byte[] outputBytes = null;
            var zipInputStream = new ZipInputStream(new MemoryStream(dataBytes));

            if (zipInputStream.CanDecompressEntry) {

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
            else {

                try {
                var gzipInputStream = 
                    new GZipInputStream(new MemoryStream(dataBytes));


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

        public override void OnEnter()
        {
            base.OnEnter();

            // udpate after action in run!
            ScheduleUpdateWithPriority(1);
        }

        public override void OnExit()
        {
            UnscheduleUpdate();

            base.OnExit();
        }

        private bool AddParticle()
        {
            if (IsFull)
            {
                return false;
            }

            InitParticle(ref Particles[ParticleCount]);
            ++ParticleCount;

            return true;
        }

        private void InitParticle(ref CCParticle particle)
        {
            // timeToLive
            // no negative life. prevent division by 0
            particle.timeToLive = Math.Max(0, Life + LifeVar * CCRandom.Float_Minus1_1());

            // position
            particle.pos.X = SourcePosition.X + PositionVar.X * CCRandom.Float_Minus1_1();
            particle.pos.Y = SourcePosition.Y + PositionVar.Y * CCRandom.Float_Minus1_1();

            // Color
            CCColor4F start;
            start.R = MathHelper.Clamp(StartColor.R + StartColorVar.R * CCRandom.Float_Minus1_1(), 0, 1);
            start.G = MathHelper.Clamp(StartColor.G + StartColorVar.G * CCRandom.Float_Minus1_1(), 0, 1);
            start.B = MathHelper.Clamp(StartColor.B + StartColorVar.B * CCRandom.Float_Minus1_1(), 0, 1);
            start.A = MathHelper.Clamp(StartColor.A + StartColorVar.A * CCRandom.Float_Minus1_1(), 0, 1);

            CCColor4F end;
            end.R = MathHelper.Clamp(EndColor.R + EndColorVar.R * CCRandom.Float_Minus1_1(), 0, 1);
            end.G = MathHelper.Clamp(EndColor.G + EndColorVar.G * CCRandom.Float_Minus1_1(), 0, 1);
            end.B = MathHelper.Clamp(EndColor.B + EndColorVar.B * CCRandom.Float_Minus1_1(), 0, 1);
            end.A = MathHelper.Clamp(EndColor.A + EndColorVar.A * CCRandom.Float_Minus1_1(), 0, 1);

            particle.color = start;
            particle.deltaColor.R = (end.R - start.R) / particle.timeToLive;
            particle.deltaColor.G = (end.G - start.G) / particle.timeToLive;
            particle.deltaColor.B = (end.B - start.B) / particle.timeToLive;
            particle.deltaColor.A = (end.A - start.A) / particle.timeToLive;

            float startS = StartSize + StartSizeVar * CCRandom.Float_Minus1_1();
            startS = Math.Max(0, startS); // No negative value

            particle.size = startS;

            if (EndSize == ParticleStartSizeEqualToEndSize)
            {
                particle.deltaSize = 0;
            }
            else
            {
                float endS = EndSize + EndSizeVar * CCRandom.Float_Minus1_1();
                endS = Math.Max(0, endS); // No negative values
                particle.deltaSize = (endS - startS) / particle.timeToLive;
            }

            float startA = StartSpin + StartSpinVar * CCRandom.Float_Minus1_1();
            float endA = EndSpin + EndSpinVar * CCRandom.Float_Minus1_1();
            particle.rotation = startA;
            particle.deltaRotation = (endA - startA) / particle.timeToLive;

            if (PositionType == CCPositionType.Free)
            {
                particle.startPos = ConvertToWorldSpace(CCPoint.Zero);
            }
            else if (PositionType == CCPositionType.Relative)
            {
                particle.startPos = m_obPosition;
            }

            // direction
            float a = MathHelper.ToRadians(Angle + AngleVar * CCRandom.Float_Minus1_1());

            // Mode Gravity: A
            if (EmitterMode == CCEmitterMode.Gravity)
            {
                var v = new CCPoint(CCMathHelper.Cos(a), CCMathHelper.Sin(a));

                float s = GravityMode.Speed + GravityMode.SpeedVar * CCRandom.Float_Minus1_1();

                // direction
                particle.modeA.dir = v * s;

                // radial accel
                particle.modeA.radialAccel = GravityMode.RadialAccel + GravityMode.RadialAccelVar * CCRandom.Float_Minus1_1();


                // tangential accel
                particle.modeA.tangentialAccel = GravityMode.TangentialAccel + GravityMode.TangentialAccelVar * CCRandom.Float_Minus1_1();

                // rotation is dir
                if (GravityMode.RotationIsDir)
                {
                    particle.rotation = -MathHelper.ToDegrees(CCPoint.ToAngle(particle.modeA.dir));
                }
            }

                // Mode Radius: B
            else
            {
                // Set the default diameter of the particle from the source position
                float startRadius = CircularMode.StartRadius + CircularMode.StartRadiusVar * CCRandom.Float_Minus1_1();
                float endRadius = CircularMode.EndRadius + CircularMode.EndRadiusVar * CCRandom.Float_Minus1_1();

                particle.modeB.radius = startRadius;

                if (CircularMode.EndRadius == ParticleStartRadiusEqualToEndRadius)
                {
                    particle.modeB.deltaRadius = 0;
                }
                else
                {
                    particle.modeB.deltaRadius = (endRadius - startRadius) / particle.timeToLive;
                }

                particle.modeB.angle = a;
                particle.modeB.degreesPerSecond =
                    MathHelper.ToRadians(CircularMode.RotatePerSecond + CircularMode.RotatePerSecondVar * CCRandom.Float_Minus1_1());
            }
        }

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
                Particles[i].timeToLive = 0;
            }
        }

        private bool UpdateParticle(ref CCParticle p, float dt)
        {
            // life
            p.timeToLive -= dt;

            if (p.timeToLive > 0)
            {
                // Mode A: gravity, direction, tangential accel & radial accel
                if (EmitterMode == CCEmitterMode.Gravity)
                {
                    float radial_x = 0;
                    float radial_y = 0;

                    float tmp_x, tmp_y;
                    float tangential_x, tangential_y;

                    float x = p.pos.X;
                    float y = p.pos.Y;

                    if (x != 0 || y != 0)
                    {
                        float l = 1.0f / (float) Math.Sqrt(x * x + y * y);

                        radial_x = x * l;
                        radial_y = y * l;
                    }
                    tangential_x = radial_x;
                    tangential_y = radial_y;

                    //radial = CCPoint.ccpMult(radial, p.modeA.RadialAccel);
                    radial_x *= p.modeA.radialAccel;
                    radial_y *= p.modeA.radialAccel;


                    // tangential acceleration
                    float newy = tangential_x;
                    tangential_x = -tangential_y;
                    tangential_y = newy;
                    //tangential = CCPoint.ccpMult(tangential, p.modeA.TangentialAccel);
                    tangential_x *= p.modeA.tangentialAccel;
                    tangential_y *= p.modeA.tangentialAccel;

                    // (gravity + radial + tangential) * dt
                    //tmp = CCPoint.ccpAdd(CCPoint.ccpAdd(radial, tangential), modeA.Gravity);
                    //tmp = CCPoint.ccpMult(tmp, dt);
                    //p.modeA.dir = CCPoint.ccpAdd(p.modeA.dir, tmp);
                    //tmp = CCPoint.ccpMult(p.modeA.dir, dt);
                    //p.pos = CCPoint.ccpAdd(p.pos, tmp);

                    tmp_x = (radial_x + tangential_x + GravityMode.Gravity.X) * dt;
                    tmp_y = (radial_y + tangential_y + GravityMode.Gravity.Y) * dt;

                    p.modeA.dir.X += tmp_x;
                    p.modeA.dir.Y += tmp_y;

                    p.pos.X += p.modeA.dir.X * dt;
                    p.pos.Y += p.modeA.dir.Y * dt;
                }

                    // Mode B: radius movement
                else
                {
                    // Update the angle and radius of the particle.
                    p.modeB.angle += p.modeB.degreesPerSecond * dt;
                    p.modeB.radius += p.modeB.deltaRadius * dt;

                    p.pos.X = -CCMathHelper.Cos(p.modeB.angle) * p.modeB.radius;
                    p.pos.Y = -CCMathHelper.Sin(p.modeB.angle) * p.modeB.radius;
                }

                // color
                p.color.R += (p.deltaColor.R * dt);
                p.color.G += (p.deltaColor.G * dt);
                p.color.B += (p.deltaColor.B * dt);
                p.color.A += (p.deltaColor.A * dt);

                // size
                p.size += (p.deltaSize * dt);
                if (p.size < 0)
                {
                    p.size = 0;
                }

                // angle
                p.rotation += (p.deltaRotation * dt);

                return true;
            }
            return false;
        }


        // ParticleSystem - MainLoop
        public override void Update(float dt)
        {
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

                /*
                CCPoint currentPosition = CCPoint.Zero;
                if (m_ePositionType == CCPositionType.kCCPositionTypeFree)
                {
                    currentPosition = convertToWorldSpace(CCPoint.Zero);
                }
                else if (m_ePositionType == CCPositionType.kCCPositionTypeRelative)
                {
                    currentPosition = m_tPosition;
                }
                */


                if (m_bVisible)
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
                                UnscheduleUpdate();
                                m_pParent.RemoveChild(this, true);
                                return;
                            }
                        }
                    } //while
                }
            }

            UpdateQuadsWithParticles();

            if (BatchNode == null)
            {
                PostStep();
            }

            //CC_PROFILER_STOP_CATEGORY(kCCProfilerCategoryParticles , "CCParticleSystem - update");
        }

        public void UpdateWithNoTime()
        {
            Update(0.0f);
        }

        public virtual void UpdateQuadsWithParticles()
        {
            // should be overriden
        }

        protected virtual void PostStep()
        {
            // should be overriden
        }

        #region ParticleSystem - CCTexture protocol

        public virtual CCTexture2D Texture
        {
            get { return texture; }
            set
            {
                if (Texture != value)
                {
                    texture = value;
                    updateBlendFunc();
                }
            }
        }

        private void updateBlendFunc()
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

        #endregion


    }
}