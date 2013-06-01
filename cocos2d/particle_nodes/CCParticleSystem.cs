using System;
using System.Diagnostics;
using System.IO;
using Microsoft.Xna.Framework;
#if !HAS_NATIVE_ZIPFILE_SUPPORT
//using ICSharpCode.SharpZipLib.Zip;
using GZipInputStream=WP7Contrib.Communications.Compression.GZipStream; // Found in Support/Compression/GZipStream
using ICSharpCode.SharpZipLib.Zip;
#else
using System.IO.Compression.ZipFile;
using System.IO.Compression.GZip;
#endif
using cocos2d;

namespace Cocos2D
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
        /** Gravity mode (A mode) */
        kCCParticleModeGravity,

        /** Radius mode (B mode) */
        kCCParticleModeRadius,
    }

    public enum CCPositionType
    {
        /** Living particles are attached to the world and are unaffected by emitter repositioning. */
        kCCPositionTypeFree,

        /** Living particles are attached to the world but will follow the emitter repositioning.
            Use case: Attach an emitter to an sprite, and you want that the emitter follows the sprite.
        */
        kCCPositionTypeRelative,

        /** Living particles are attached to the emitter and are translated along with it. */
        kCCPositionTypeGrouped,
    }


    public class CCParticleSystem : CCNode, ICCTextureProtocol
    {
        /** The Particle emitter lives forever */
        public const int kCCParticleDurationInfinity = -1;
        /** The starting size of the particle is equal to the ending size */
        public const int kCCParticleStartSizeEqualToEndSize = -1;

        /** The starting radius of the particle is equal to the ending radius */
        public const int kCCParticleStartRadiusEqualToEndRadius = -1;

        // backward compatible
        public const int kParticleStartSizeEqualToEndSize = kCCParticleStartSizeEqualToEndSize;
        public const int kParticleDurationInfinity = kCCParticleDurationInfinity;

        protected bool m_bIsActive;
        protected bool m_bIsAutoRemoveOnFinish;
        protected bool m_bIsBlendAdditive;
        protected bool m_bOpacityModifyRGB;
        protected bool m_bTransformSystemDirty;
        protected CCPositionType m_ePositionType;
        protected float m_fAngle;
        protected float m_fAngleVar;
        protected float m_fDuration;
        protected float m_fElapsed;
        protected float m_fEmissionRate;
        protected float m_fEmitCounter;
        protected float m_fEndSize;
        protected float m_fEndSizeVar;
        protected float m_fEndSpin;
        protected float m_fEndSpinVar;
        protected float m_fLife;
        protected float m_fLifeVar;
        protected float m_fStartSize;
        protected float m_fStartSizeVar;
        protected float m_fStartSpin;
        protected float m_fStartSpinVar;
        protected CCEmitterMode m_nEmitterMode;
        protected CCParticleBatchNode m_pBatchNode;
        protected CCParticle[] m_pParticles;
        protected CCTexture2D m_pTexture;
        protected string m_sPlistFile;
        protected CCBlendFunc m_tBlendFunc;

        protected CCColor4F m_tEndColor;
        protected CCColor4F m_tEndColorVar;
        protected CCPoint m_tPosVar;
        protected CCPoint m_tSourcePosition;
        protected CCColor4F m_tStartColor;
        protected CCColor4F m_tStartColorVar;
        protected int m_uAllocatedParticles;
        protected int m_uAtlasIndex;
        protected int m_uParticleCount;
        protected int m_uTotalParticles;

        protected ModeA modeA;
        protected ModeB modeB;


        // implementation CCParticleSystem

        public bool isFull
        {
            get { return (m_uParticleCount == m_uTotalParticles); }
        }

        public bool IsActive
        {
            get { return m_bIsActive; }
        }

        public int ParticleCount
        {
            get { return m_uParticleCount; }
        }

        public float Duration
        {
            get { return m_fDuration; }
            set { m_fDuration = value; }
        }

        public CCPoint SourcePosition
        {
            get { return m_tSourcePosition; }
            set { m_tSourcePosition = value; }
        }

        public CCPoint PosVar
        {
            get { return m_tPosVar; }
            set { m_tPosVar = value; }
        }

        public float Life
        {
            get { return m_fLife; }
            set { m_fLife = value; }
        }

        public float LifeVar
        {
            get { return m_fLifeVar; }
            set { m_fLifeVar = value; }
        }

        public float Angle
        {
            get { return m_fAngle; }
            set { m_fAngle = value; }
        }

        public float AngleVar
        {
            get { return m_fAngleVar; }
            set { m_fAngleVar = value; }
        }

        public float StartSize
        {
            get { return m_fStartSize; }
            set { m_fStartSize = value; }
        }

        public float StartSizeVar
        {
            get { return m_fStartSizeVar; }
            set { m_fStartSizeVar = value; }
        }

        public float EndSize
        {
            get { return m_fEndSize; }
            set { m_fEndSize = value; }
        }

        public float EndSizeVar
        {
            get { return m_fEndSizeVar; }
            set { m_fEndSizeVar = value; }
        }

        public CCColor4F StartColor
        {
            get { return m_tStartColor; }
            set { m_tStartColor = value; }
        }

        public CCColor4F StartColorVar
        {
            get { return m_tStartColorVar; }
            set { m_tStartColorVar = value; }
        }

        public CCColor4F EndColor
        {
            get { return m_tEndColor; }
            set { m_tEndColor = value; }
        }

        public CCColor4F EndColorVar
        {
            get { return m_tEndColorVar; }
            set { m_tEndColorVar = value; }
        }

        public float StartSpin
        {
            get { return m_fStartSpin; }
            set { m_fStartSpin = value; }
        }

        public float StartSpinVar
        {
            get { return m_fStartSpinVar; }
            set { m_fStartSpinVar = value; }
        }

        public float EndSpin
        {
            get { return m_fEndSpin; }
            set { m_fEndSpin = value; }
        }

        public float EndSpinVar
        {
            get { return m_fEndSpinVar; }
            set { m_fEndSpinVar = value; }
        }

        public float EmissionRate
        {
            get { return m_fEmissionRate; }
            set { m_fEmissionRate = value; }
        }

        public virtual int TotalParticles
        {
            get { return m_uTotalParticles; }
            set
            {
                Debug.Assert(value <= m_uAllocatedParticles, "Particle: resizing particle array only supported for quads");
                m_uTotalParticles = value;
            }
        }

        public bool OpacityModifyRGB
        {
            get { return m_bOpacityModifyRGB; }
            set { m_bOpacityModifyRGB = value; }
        }

        public CCPositionType PositionType
        {
            get { return m_ePositionType; }
            set { m_ePositionType = value; }
        }

        public bool AutoRemoveOnFinish
        {
            get { return m_bIsAutoRemoveOnFinish; }
            set { m_bIsAutoRemoveOnFinish = value; }
        }

        public CCEmitterMode EmitterMode
        {
            get { return m_nEmitterMode; }
            set { m_nEmitterMode = value; }
        }

        public int AtlasIndex
        {
            get { return m_uAtlasIndex; }
            set { m_uAtlasIndex = value; }
        }

        #region ParticleSystem - methods for batchNode rendering

        public virtual CCParticleBatchNode BatchNode
        {
            get { return m_pBatchNode; }
            set
            {
                if (m_pBatchNode != value)
                {
                    m_pBatchNode = value; // weak reference

                    if (value != null)
                    {
                        //each particle needs a unique index
                        for (int i = 0; i < m_uTotalParticles; i++)
                        {
                            m_pParticles[i].atlasIndex = i;
                        }
                    }
                }
            }
        }

        //don't use a transform matrix, this is faster
        public override float Scale
        {
            get { return base.Scale; }
            set
            {
                base.Scale = value;
                m_bTransformSystemDirty = true;
            }
        }

        public override float Rotation
        {
            get { return base.Rotation; }
            set
            {
                base.Rotation = value;
                m_bTransformSystemDirty = true;
            }
        }

        public override float ScaleX
        {
            get { return base.ScaleX; }
            set
            {
                base.ScaleX = value;
                m_bTransformSystemDirty = true;
            }
        }

        public override float ScaleY
        {
            get { return base.ScaleY; }
            set
            {
                base.ScaleY = value;
                m_bTransformSystemDirty = true;
            }
        }

        #endregion

        #region ICCTextureProtocol Members

        public CCBlendFunc BlendFunc
        {
            get { return m_tBlendFunc; }
            set
            {
                if (m_tBlendFunc.Source != value.Source || m_tBlendFunc.Destination != value.Destination)
                {
                    m_tBlendFunc = value;
                    updateBlendFunc();
                }
            }
        }

        #endregion

        protected CCParticleSystem ()
        {
            Init();
        }

        public CCParticleSystem (string plistFile)
        {
            InitWithFile(plistFile);
        }

        public bool Init()
        {
            return InitWithTotalParticles(150);
        }

        public bool InitWithFile(string plistFile)
        {
            bool bRet;
            m_sPlistFile = CCFileUtils.FullPathFromRelativePath(plistFile);

            //var content = CCApplication.SharedApplication.content.Load<CCContent>(m_sPlistFile);
            //PlistDocument dict = PlistDocument.Create(content.Content);

            PlistDocument doc = null;
            try
            {
                doc = CCApplication.SharedApplication.Content.Load<PlistDocument>(m_sPlistFile);
            }
            catch (Exception)
            {
                string xml = Cocos2D.Framework.CCContent.LoadContentFile(m_sPlistFile);
                if (xml != null)
                {
                    doc = new PlistDocument(xml);
                }
            }
            if (doc == null)
            {
                throw (new Microsoft.Xna.Framework.Content.ContentLoadException("Failed to load the particle definition file from " + m_sPlistFile));
            }
            Debug.Assert(doc != null, "Particles: file not found");
            Debug.Assert(doc.Root != null, "Particles: file not found");

            bRet = InitWithDictionary(doc.Root.AsDictionary);

            return bRet;
        }

        public bool InitWithDictionary(PlistDictionary dictionary)
        {
            bool bRet = false;

            do
            {
                int maxParticles = dictionary["maxParticles"].AsInt;
                // self, not super
                if (InitWithTotalParticles(maxParticles))
                {
                    // angle
                    m_fAngle = dictionary["angle"].AsFloat;
                    m_fAngleVar = dictionary["angleVariance"].AsFloat;

                    // duration
                    m_fDuration = dictionary["duration"].AsFloat;

                    // blend function 
                    m_tBlendFunc.Source = dictionary["blendFuncSource"].AsInt;
                    m_tBlendFunc.Destination = dictionary["blendFuncDestination"].AsInt;

                    // color
                    m_tStartColor.R = dictionary["startColorRed"].AsFloat;
                    m_tStartColor.G = dictionary["startColorGreen"].AsFloat;
                    m_tStartColor.B = dictionary["startColorBlue"].AsFloat;
                    m_tStartColor.A = dictionary["startColorAlpha"].AsFloat;

                    m_tStartColorVar.R = dictionary["startColorVarianceRed"].AsFloat;
                    m_tStartColorVar.G = dictionary["startColorVarianceGreen"].AsFloat;
                    m_tStartColorVar.B = dictionary["startColorVarianceBlue"].AsFloat;
                    m_tStartColorVar.A = dictionary["startColorVarianceAlpha"].AsFloat;

                    m_tEndColor.R = dictionary["finishColorRed"].AsFloat;
                    m_tEndColor.G = dictionary["finishColorGreen"].AsFloat;
                    m_tEndColor.B = dictionary["finishColorBlue"].AsFloat;
                    m_tEndColor.A = dictionary["finishColorAlpha"].AsFloat;

                    m_tEndColorVar.R = dictionary["finishColorVarianceRed"].AsFloat;
                    m_tEndColorVar.G = dictionary["finishColorVarianceGreen"].AsFloat;
                    m_tEndColorVar.B = dictionary["finishColorVarianceBlue"].AsFloat;
                    m_tEndColorVar.A = dictionary["finishColorVarianceAlpha"].AsFloat;

                    // particle size
                    m_fStartSize = dictionary["startParticleSize"].AsFloat;
                    m_fStartSizeVar = dictionary["startParticleSizeVariance"].AsFloat;
                    m_fEndSize = dictionary["finishParticleSize"].AsFloat;
                    m_fEndSizeVar = dictionary["finishParticleSizeVariance"].AsFloat;

                    // position
                    float x = dictionary["sourcePositionx"].AsFloat;
                    float y = dictionary["sourcePositiony"].AsFloat;
                    Position = new CCPoint(x, y);
                    m_tPosVar.X = dictionary["sourcePositionVariancex"].AsFloat;
                    m_tPosVar.Y = dictionary["sourcePositionVariancey"].AsFloat;

                    // Spinning
                    m_fStartSpin = dictionary["rotationStart"].AsFloat;
                    m_fStartSpinVar = dictionary["rotationStartVariance"].AsFloat;
                    m_fEndSpin = dictionary["rotationEnd"].AsFloat;
                    m_fEndSpinVar = dictionary["rotationEndVariance"].AsFloat;

                    m_nEmitterMode = (CCEmitterMode) dictionary["emitterType"].AsInt;

                    // Mode A: Gravity + tangential accel + radial accel
                    if (m_nEmitterMode == CCEmitterMode.kCCParticleModeGravity)
                    {
                        // gravity
                        modeA.gravity.X = dictionary["gravityx"].AsFloat;
                        modeA.gravity.Y = dictionary["gravityy"].AsFloat;

                        // speed
                        modeA.speed = dictionary["speed"].AsFloat;
                        modeA.speedVar = dictionary["speedVariance"].AsFloat;

                        // radial acceleration
                        modeA.radialAccel = dictionary["radialAcceleration"].AsFloat;
                        modeA.radialAccelVar = dictionary["radialAccelVariance"].AsFloat;

                        // tangential acceleration
                        modeA.tangentialAccel = dictionary["tangentialAcceleration"].AsFloat;
                        modeA.tangentialAccelVar = dictionary["tangentialAccelVariance"].AsFloat;
                    }

                        // or Mode B: radius movement
                    else if (m_nEmitterMode == CCEmitterMode.kCCParticleModeRadius)
                    {
                        modeB.startRadius = dictionary["maxRadius"].AsFloat;
                        modeB.startRadiusVar = dictionary["maxRadiusVariance"].AsFloat;
                        modeB.endRadius = dictionary["minRadius"].AsFloat;
                        modeB.endRadiusVar = 0.0f;
                        modeB.rotatePerSecond = dictionary["rotatePerSecond"].AsFloat;
                        modeB.rotatePerSecondVar = dictionary["rotatePerSecondVariance"].AsFloat;
                    }
                    else
                    {
                        Debug.Assert(false, "Invalid emitterType in config file");
                        break;
                    }

                    // life span
                    m_fLife = dictionary["particleLifespan"].AsFloat;
                    m_fLifeVar = dictionary["particleLifespanVariance"].AsFloat;

                    // emission Rate
                    m_fEmissionRate = m_uTotalParticles / m_fLife;

                    //don't get the internal texture if a batchNode is used
                    if (m_pBatchNode == null)
                    {
                        // Set a compatible default for the alpha transfer
                        m_bOpacityModifyRGB = false;

                        // texture        
                        // Try to get the texture from the cache
                        string textureName = dictionary["textureFileName"].AsString;
                        string fullpath = CCFileUtils.FullPathFromRelativeFile(textureName, m_sPlistFile);

                        CCTexture2D tex = null;

                        if (!string.IsNullOrEmpty(textureName))
                        {
                            // set not pop-up message box when load image failed
                            bool bNotify = CCFileUtils.IsPopupNotify;
                            CCFileUtils.IsPopupNotify = false;
                            try
                            {
                                tex = CCTextureCache.SharedTextureCache.AddImage(fullpath);
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
                            Debug.Assert(textureData != null && textureData.Length > 0, string.Format("CCParticleSystem: textureData does not exist : {0}",textureName));

                            int dataLen = textureData.Length;
                            if (dataLen != 0)
                            {

                                var dataBytes = Convert.FromBase64String(textureData);
                                Debug.Assert(dataBytes != null, string.Format("CCParticleSystem: error decoding textureImageData : {0}",textureName));

                                var imageBytes = Inflate(dataBytes);
                                Debug.Assert(imageBytes != null, string.Format("CCParticleSystem: error init image with Data for texture : {0}",textureName));

                                using (var imageStream = new MemoryStream(imageBytes))
                                {
                                    try
                                    {
                                        Texture = CCTextureCache.SharedTextureCache.AddImage(imageStream, textureName);
                                    }
                                    catch (Exception ex)
                                    {
                                        CCLog.Log(ex.ToString());
                                        throw (new NotSupportedException("Embedded textureImageData is a format that this platform does not understand. Use PNG, GIF, or JPEG for your particle systems."));
                                    }
                                }
                            }
                        }
                        Debug.Assert(Texture != null, string.Format("CCParticleSystem: error loading the texture : {0}", textureName));
                    }
                    bRet = true;
                }
            } while (false);

            return bRet;
        }

        /// <summary>
        /// Decompresses the given data stream from its source ZIP or GZIP format.
        /// </summary>
        /// <param name="dataBytes"></param>
        /// <returns></returns>
        private static byte[] Inflate(byte[] dataBytes)
        {

            byte[] outputBytes = null;
            var zipInputStream = 
                new ZipInputStream(new MemoryStream(dataBytes));

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

        public virtual bool InitWithTotalParticles(int numberOfParticles)
        {
            m_uTotalParticles = numberOfParticles;

            m_pParticles = new CCParticle[m_uTotalParticles];

            m_uAllocatedParticles = numberOfParticles;

            if (m_pBatchNode != null)
            {
                for (int i = 0; i < m_uTotalParticles; i++)
                {
                    m_pParticles[i].atlasIndex = i;
                }
            }
            // default, active
            m_bIsActive = true;

            // default blend function
            m_tBlendFunc.Source = CCMacros.CCDefaultSourceBlending;
            m_tBlendFunc.Destination = CCMacros.CCDefaultDestinationBlending;

            // default movement type;
            m_ePositionType = CCPositionType.kCCPositionTypeFree;

            // by default be in mode A:
            m_nEmitterMode = CCEmitterMode.kCCParticleModeGravity;

            // default: modulate
            // XXX: not used
            //    colorModulate = YES;

            m_bIsAutoRemoveOnFinish = false;

            // Optimization: compile udpateParticle method
            //updateParticleSel = @selector(updateQuadWithParticle:newPosition:);
            //updateParticleImp = (CC_UPDATE_PARTICLE_IMP) [self methodForSelector:updateParticleSel];
            //for batchNode
            m_bTransformSystemDirty = false;

            return true;
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
            if (isFull)
            {
                return false;
            }

            InitParticle(ref m_pParticles[m_uParticleCount]);
            ++m_uParticleCount;

            return true;
        }

        private void InitParticle(ref CCParticle particle)
        {
            // timeToLive
            // no negative life. prevent division by 0
            particle.timeToLive = Math.Max(0, m_fLife + m_fLifeVar * CCRandom.Float_Minus1_1());

            // position
            particle.pos.X = m_tSourcePosition.X + m_tPosVar.X * CCRandom.Float_Minus1_1();
            particle.pos.Y = m_tSourcePosition.Y + m_tPosVar.Y * CCRandom.Float_Minus1_1();

            // Color
            CCColor4F start;
            start.R = MathHelper.Clamp(m_tStartColor.R + m_tStartColorVar.R * CCRandom.Float_Minus1_1(), 0, 1);
            start.G = MathHelper.Clamp(m_tStartColor.G + m_tStartColorVar.G * CCRandom.Float_Minus1_1(), 0, 1);
            start.B = MathHelper.Clamp(m_tStartColor.B + m_tStartColorVar.B * CCRandom.Float_Minus1_1(), 0, 1);
            start.A = MathHelper.Clamp(m_tStartColor.A + m_tStartColorVar.A * CCRandom.Float_Minus1_1(), 0, 1);

            CCColor4F end;
            end.R = MathHelper.Clamp(m_tEndColor.R + m_tEndColorVar.R * CCRandom.Float_Minus1_1(), 0, 1);
            end.G = MathHelper.Clamp(m_tEndColor.G + m_tEndColorVar.G * CCRandom.Float_Minus1_1(), 0, 1);
            end.B = MathHelper.Clamp(m_tEndColor.B + m_tEndColorVar.B * CCRandom.Float_Minus1_1(), 0, 1);
            end.A = MathHelper.Clamp(m_tEndColor.A + m_tEndColorVar.A * CCRandom.Float_Minus1_1(), 0, 1);

            particle.color = start;
            particle.deltaColor.R = (end.R - start.R) / particle.timeToLive;
            particle.deltaColor.G = (end.G - start.G) / particle.timeToLive;
            particle.deltaColor.B = (end.B - start.B) / particle.timeToLive;
            particle.deltaColor.A = (end.A - start.A) / particle.timeToLive;

            // size
            float startS = m_fStartSize + m_fStartSizeVar * CCRandom.Float_Minus1_1();
            startS = Math.Max(0, startS); // No negative value

            particle.size = startS;

            if (m_fEndSize == kCCParticleStartSizeEqualToEndSize)
            {
                particle.deltaSize = 0;
            }
            else
            {
                float endS = m_fEndSize + m_fEndSizeVar * CCRandom.Float_Minus1_1();
                endS = Math.Max(0, endS); // No negative values
                particle.deltaSize = (endS - startS) / particle.timeToLive;
            }

            // rotation
            float startA = m_fStartSpin + m_fStartSpinVar * CCRandom.Float_Minus1_1();
            float endA = m_fEndSpin + m_fEndSpinVar * CCRandom.Float_Minus1_1();
            particle.rotation = startA;
            particle.deltaRotation = (endA - startA) / particle.timeToLive;

            // position
            if (m_ePositionType == CCPositionType.kCCPositionTypeFree)
            {
                particle.startPos = ConvertToWorldSpace(CCPoint.Zero);
            }
            else if (m_ePositionType == CCPositionType.kCCPositionTypeRelative)
            {
                particle.startPos = m_tPosition;
            }

            // direction
            float a = MathHelper.ToRadians(m_fAngle + m_fAngleVar * CCRandom.Float_Minus1_1());

            // Mode Gravity: A
            if (m_nEmitterMode == CCEmitterMode.kCCParticleModeGravity)
            {
                var v = new CCPoint(CCMathHelper.Cos(a), CCMathHelper.Sin(a));

                float s = modeA.speed + modeA.speedVar * CCRandom.Float_Minus1_1();

                // direction
                particle.modeA.dir = v * s;

                // radial accel
                particle.modeA.radialAccel = modeA.radialAccel + modeA.radialAccelVar * CCRandom.Float_Minus1_1();


                // tangential accel
                particle.modeA.tangentialAccel = modeA.tangentialAccel + modeA.tangentialAccelVar * CCRandom.Float_Minus1_1();
            }

                // Mode Radius: B
            else
            {
                // Set the default diameter of the particle from the source position
                float startRadius = modeB.startRadius + modeB.startRadiusVar * CCRandom.Float_Minus1_1();
                float endRadius = modeB.endRadius + modeB.endRadiusVar * CCRandom.Float_Minus1_1();

                particle.modeB.radius = startRadius;

                if (modeB.endRadius == kCCParticleStartRadiusEqualToEndRadius)
                {
                    particle.modeB.deltaRadius = 0;
                }
                else
                {
                    particle.modeB.deltaRadius = (endRadius - startRadius) / particle.timeToLive;
                }

                particle.modeB.angle = a;
                particle.modeB.degreesPerSecond =
                    MathHelper.ToRadians(modeB.rotatePerSecond + modeB.rotatePerSecondVar * CCRandom.Float_Minus1_1());
            }
        }

        public void StopSystem()
        {
            m_bIsActive = false;
            m_fElapsed = m_fDuration;
            m_fEmitCounter = 0;
        }

        public void ResetSystem()
        {
            m_bIsActive = true;
            m_fElapsed = 0;
            for (int i = 0; i < m_uParticleCount; ++i)
            {
                m_pParticles[i].timeToLive = 0;
            }
        }

        private bool UpdateParticle(ref CCParticle p, float dt)
        {
            // life
            p.timeToLive -= dt;

            if (p.timeToLive > 0)
            {
                // Mode A: gravity, direction, tangential accel & radial accel
                if (m_nEmitterMode == CCEmitterMode.kCCParticleModeGravity)
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

                    //radial = CCPoint.ccpMult(radial, p.modeA.radialAccel);
                    radial_x *= p.modeA.radialAccel;
                    radial_y *= p.modeA.radialAccel;


                    // tangential acceleration
                    float newy = tangential_x;
                    tangential_x = -tangential_y;
                    tangential_y = newy;
                    //tangential = CCPoint.ccpMult(tangential, p.modeA.tangentialAccel);
                    tangential_x *= p.modeA.tangentialAccel;
                    tangential_y *= p.modeA.tangentialAccel;

                    // (gravity + radial + tangential) * dt
                    //tmp = CCPoint.ccpAdd(CCPoint.ccpAdd(radial, tangential), modeA.gravity);
                    //tmp = CCPoint.ccpMult(tmp, dt);
                    //p.modeA.dir = CCPoint.ccpAdd(p.modeA.dir, tmp);
                    //tmp = CCPoint.ccpMult(p.modeA.dir, dt);
                    //p.pos = CCPoint.ccpAdd(p.pos, tmp);

                    tmp_x = (radial_x + tangential_x + modeA.gravity.X) * dt;
                    tmp_y = (radial_y + tangential_y + modeA.gravity.Y) * dt;

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
            //CC_PROFILER_START_CATEGORY(kCCProfilerCategoryParticles , "CCParticleSystem - update");

            CCParticle[] particles = m_pParticles;
            //fixed (CCParticle* particles = m_pParticles)
            {
                if (m_bIsActive && m_fEmissionRate > 0)
                {
                    float rate = 1.0f / m_fEmissionRate;
                    //issue #1201, prevent bursts of particles, due to too high emitCounter
                    if (m_uParticleCount < m_uTotalParticles)
                    {
                        m_fEmitCounter += dt;
                    }

                    while (m_uParticleCount < m_uTotalParticles && m_fEmitCounter > rate)
                    {
                        InitParticle(ref particles[m_uParticleCount]);
                        ++m_uParticleCount;
                        m_fEmitCounter -= rate;
                    }

                    m_fElapsed += dt;

                    if (m_fDuration != -1 && m_fDuration < m_fElapsed)
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


                if (m_bIsVisible)
                {
                    while (index < m_uParticleCount)
                    {
                        if (UpdateParticle(ref particles[index], dt))
                        {
                            // update particle counter
                            ++index;
                        }
                        else
                        {
                            // life < 0
                            int currentIndex = particles[index].atlasIndex;
                            if (index != m_uParticleCount - 1)
                            {
                                particles[index] = particles[m_uParticleCount - 1];
                            }

                            if (m_pBatchNode != null)
                            {
                                //disable the switched particle
                                m_pBatchNode.DisableParticle(m_uAtlasIndex + currentIndex);

                                //switch indexes
                                particles[m_uParticleCount - 1].atlasIndex = currentIndex;
                            }

                            --m_uParticleCount;

                            if (m_uParticleCount == 0 && m_bIsAutoRemoveOnFinish)
                            {
                                UnscheduleUpdate();
                                m_pParent.RemoveChild(this, true);
                                return;
                            }
                        }
                    } //while
                    m_bTransformSystemDirty = false;
                }
            }

            UpdateQuadsWithParticles();

            if (m_pBatchNode == null)
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
            get { return m_pTexture; }
            set
            {
                if (m_pTexture != value)
                {
                    m_pTexture = value;
                    updateBlendFunc();
                }
            }
        }

        private void updateBlendFunc()
        {
            Debug.Assert(m_pBatchNode == null, "Can't change blending functions when the particle is being batched");

            if (m_pTexture != null)
            {
                bool premultiplied = m_pTexture.HasPremultipliedAlpha;

                m_bOpacityModifyRGB = false;

                if (m_tBlendFunc.Source == CCMacros.CCDefaultSourceBlending && m_tBlendFunc.Destination == CCMacros.CCDefaultDestinationBlending)
                {
                    if (premultiplied)
                    {
                        m_bOpacityModifyRGB = true;
                    }
                    else
                    {
                        m_tBlendFunc.Source = CCOGLES.GL_SRC_ALPHA;
                        m_tBlendFunc.Destination = CCOGLES.GL_ONE_MINUS_SRC_ALPHA;
                    }
                }
            }
        }

        #endregion

        #region ParticleSystem - Additive Blending

        public bool BlendAdditive
        {
            get { return (m_tBlendFunc.Source == CCOGLES.GL_SRC_ALPHA && m_tBlendFunc.Destination == CCOGLES.GL_ONE); }
            set
            {
                if (value)
                {
                    m_tBlendFunc.Source = CCOGLES.GL_SRC_ALPHA;
                    m_tBlendFunc.Destination = CCOGLES.GL_ONE;
                }
                else
                {
                    if (m_pTexture != null && !m_pTexture.HasPremultipliedAlpha)
                    {
                        m_tBlendFunc.Source = CCOGLES.GL_SRC_ALPHA;
                        m_tBlendFunc.Destination = CCOGLES.GL_ONE_MINUS_SRC_ALPHA;
                    }
                    else
                    {
                        m_tBlendFunc.Source = CCMacros.CCDefaultSourceBlending;
                        m_tBlendFunc.Destination = CCMacros.CCDefaultDestinationBlending;
                    }
                }
            }
        }

        #endregion

        #region ParticleSystem - Properties of Gravity Mode

        public float TangentialAccel
        {
            get
            {
                Debug.Assert(m_nEmitterMode == CCEmitterMode.kCCParticleModeGravity, "Particle Mode should be Gravity");
                return modeA.tangentialAccel;
            }
            set
            {
                Debug.Assert(m_nEmitterMode == CCEmitterMode.kCCParticleModeGravity, "Particle Mode should be Gravity");
                modeA.tangentialAccel = value;
            }
        }

        public float TangentialAccelVar
        {
            get
            {
                Debug.Assert(m_nEmitterMode == CCEmitterMode.kCCParticleModeGravity, "Particle Mode should be Gravity");
                return modeA.tangentialAccelVar;
            }
            set
            {
                Debug.Assert(m_nEmitterMode == CCEmitterMode.kCCParticleModeGravity, "Particle Mode should be Gravity");
                modeA.tangentialAccelVar = value;
            }
        }

        public float RadialAccel
        {
            get
            {
                Debug.Assert(m_nEmitterMode == CCEmitterMode.kCCParticleModeGravity, "Particle Mode should be Gravity");
                return modeA.radialAccel;
            }
            set
            {
                Debug.Assert(m_nEmitterMode == CCEmitterMode.kCCParticleModeGravity, "Particle Mode should be Gravity");
                modeA.radialAccel = value;
            }
        }

        public float RadialAccelVar
        {
            get
            {
                Debug.Assert(m_nEmitterMode == CCEmitterMode.kCCParticleModeGravity, "Particle Mode should be Gravity");
                return modeA.radialAccelVar;
            }
            set
            {
                Debug.Assert(m_nEmitterMode == CCEmitterMode.kCCParticleModeGravity, "Particle Mode should be Gravity");
                modeA.radialAccelVar = value;
            }
        }

        public CCPoint Gravity
        {
            get
            {
                Debug.Assert(m_nEmitterMode == CCEmitterMode.kCCParticleModeGravity, "Particle Mode should be Gravity");
                return modeA.gravity;
            }
            set
            {
                Debug.Assert(m_nEmitterMode == CCEmitterMode.kCCParticleModeGravity, "Particle Mode should be Gravity");
                modeA.gravity = value;
            }
        }

        public float Speed
        {
            get
            {
                Debug.Assert(m_nEmitterMode == CCEmitterMode.kCCParticleModeGravity, "Particle Mode should be Gravity");
                return modeA.speed;
            }
            set
            {
                Debug.Assert(m_nEmitterMode == CCEmitterMode.kCCParticleModeGravity, "Particle Mode should be Gravity");
                modeA.speed = value;
            }
        }

        public float SpeedVar
        {
            get
            {
                Debug.Assert(m_nEmitterMode == CCEmitterMode.kCCParticleModeGravity, "Particle Mode should be Gravity");
                return modeA.speedVar;
            }
            set
            {
                Debug.Assert(m_nEmitterMode == CCEmitterMode.kCCParticleModeGravity, "Particle Mode should be Gravity");
                modeA.speedVar = value;
            }
        }

        #endregion

        #region ParticleSystem - Properties of Radius Mode

        public float StartRadius
        {
            get
            {
                Debug.Assert(m_nEmitterMode == CCEmitterMode.kCCParticleModeRadius, "Particle Mode should be Radius");
                return modeB.startRadius;
            }
            set
            {
                Debug.Assert(m_nEmitterMode == CCEmitterMode.kCCParticleModeRadius, "Particle Mode should be Radius");
                modeB.startRadius = value;
            }
        }

        public float StartRadiusVar
        {
            get
            {
                Debug.Assert(m_nEmitterMode == CCEmitterMode.kCCParticleModeRadius, "Particle Mode should be Radius");
                return modeB.startRadiusVar;
            }
            set
            {
                Debug.Assert(m_nEmitterMode == CCEmitterMode.kCCParticleModeRadius, "Particle Mode should be Radius");
                modeB.startRadiusVar = value;
            }
        }

        public float EndRadius
        {
            get
            {
                Debug.Assert(m_nEmitterMode == CCEmitterMode.kCCParticleModeRadius, "Particle Mode should be Radius");
                return modeB.endRadius;
            }
            set
            {
                Debug.Assert(m_nEmitterMode == CCEmitterMode.kCCParticleModeRadius, "Particle Mode should be Radius");
                modeB.endRadius = value;
            }
        }

        public float EndRadiusVar
        {
            get
            {
                Debug.Assert(m_nEmitterMode == CCEmitterMode.kCCParticleModeRadius, "Particle Mode should be Radius");
                return modeB.endRadiusVar;
            }
            set
            {
                Debug.Assert(m_nEmitterMode == CCEmitterMode.kCCParticleModeRadius, "Particle Mode should be Radius");
                modeB.endRadiusVar = value;
            }
        }

        public float RotatePerSecond
        {
            get
            {
                Debug.Assert(m_nEmitterMode == CCEmitterMode.kCCParticleModeRadius, "Particle Mode should be Radius");
                return modeB.rotatePerSecond;
            }
            set
            {
                Debug.Assert(m_nEmitterMode == CCEmitterMode.kCCParticleModeRadius, "Particle Mode should be Radius");
                modeB.rotatePerSecond = value;
            }
        }

        public float RotatePerSecondVar
        {
            get
            {
                Debug.Assert(m_nEmitterMode == CCEmitterMode.kCCParticleModeRadius, "Particle Mode should be Radius");
                return modeB.rotatePerSecondVar;
            }
            set
            {
                Debug.Assert(m_nEmitterMode == CCEmitterMode.kCCParticleModeRadius, "Particle Mode should be Radius");
                modeB.rotatePerSecondVar = value;
            }
        }

        #endregion

        #region Nested type: CCParticle

        protected struct CCParticle
        {
            public int atlasIndex;
            public CCColor4F color;
            public CCColor4F deltaColor;

            public float deltaRotation;
            public float deltaSize;

            public ModeA modeA;
            public ModeB modeB;
            public CCPoint pos;
            public float rotation;
            public float size;
            public CCPoint startPos;
            public float timeToLive;

            //! Mode A: gravity, direction, radial accel, tangential accel

            #region Nested type: ModeA

            public struct ModeA
            {
                public CCPoint dir;
                public float radialAccel;
                public float tangentialAccel;
            };

            #endregion

            //! Mode B: radius mode

            #region Nested type: ModeB

            public struct ModeB
            {
                public float angle;
                public float degreesPerSecond;
                public float deltaRadius;
                public float radius;
            }

            #endregion
        }

        #endregion

        // Different modes
        //! Mode A:Gravity + Tangential Accel + Radial Accel

        #region Nested type: ModeA

        protected struct ModeA
        {
            /** Gravity value. Only available in 'Gravity' mode. */
            public CCPoint gravity;
            public float radialAccel;
            /** radial acceleration variance of each particle. Only available in 'Gravity' mode. */
            public float radialAccelVar;
            /** speed of each particle. Only available in 'Gravity' mode.  */
            public float speed;
            /** speed variance of each particle. Only available in 'Gravity' mode. */
            public float speedVar;
            /** tangential acceleration of each particle. Only available in 'Gravity' mode. */
            public float tangentialAccel;
            /** tangential acceleration variance of each particle. Only available in 'Gravity' mode. */
            public float tangentialAccelVar;
            /** radial acceleration of each particle. Only available in 'Gravity' mode. */
        }

        #endregion

        //! Mode B: circular movement (gravity, radial accel and tangential accel don't are not used in this mode)

        #region Nested type: ModeB

        protected struct ModeB
        {
            /** The starting radius of the particles. Only available in 'Radius' mode. */
            /** The ending radius of the particles. Only available in 'Radius' mode. */
            public float endRadius;
            /** The ending radius variance of the particles. Only available in 'Radius' mode. */
            public float endRadiusVar;
            /** Number of degress to rotate a particle around the source pos per second. Only available in 'Radius' mode. */
            public float rotatePerSecond;
            /** Variance in degrees for rotatePerSecond. Only available in 'Radius' mode. */
            public float rotatePerSecondVar;
            public float startRadius;
            /** The starting radius variance of the particles. Only available in 'Radius' mode. */
            public float startRadiusVar;
        }

        #endregion
    }
}