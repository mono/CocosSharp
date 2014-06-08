using System;
using System.Diagnostics;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CocosSharp
{

    public enum CCParticleSystemType
    {
        Internal,
        Cocos2D,
        Custom,
    }


    public class CCParticleSystemConfig : IDisposable
    {

        public CCParticleSystemType ParticleSystemType { get; internal set; }
        public string Name { get; set; }
        public string DirectoryName { get; private set; }

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
        public CCColor4F StartColor { get; set; }
        public CCColor4F StartColorVar { get; set; }
        public CCColor4F EndColor { get; set; }
        public CCColor4F EndColorVar { get; set; }
        public CCPoint PositionVar { get; set; }
        public CCPositionType PositionType { get; set; }
        public CCBlendFunc BlendFunc { get; set; }
        public CCPoint Position { get; set; }

        // Gravity properities
        public CCPoint Gravity { get; set; }
        public float GravityRadialAccel { get; set; }
        public float GravityRadialAccelVar { get; set; }
        public float GravitySpeed { get; set; }
        public float GravitySpeedVar { get; set; }
        public float GravityTangentialAccel { get; set; }
        public float GravityTangentialAccelVar { get; set; }
        public bool GravityRotationIsDir { get; set; }

        // Radial properties
        public float RadialEndRadius { get; set; }
        public float RadialEndRadiusVar { get; set; }
        public float RadialRotatePerSecond { get; set; }
        public float RadialRotatePerSecondVar { get; set; }
        public float RadialStartRadius { get; set; }
        public float RadialStartRadiusVar { get; set; }

        public int MaxParticles { get; set; }

        public CCEmitterMode EmitterMode { get; set; }

        public string TextureData { get; set; }
        public string TextureName { get; set; }
        public CCTexture2D Texture { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CocosSharp.CCParticleSystemConfig"/> class.
        /// </summary>
        public CCParticleSystemConfig()
        { 
            ParticleSystemType = CCParticleSystemType.Custom;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CocosSharp.CCParticleSystemConfig"/> class.
        /// </summary>
        /// <param name="plistFile">Plist file.</param>
        /// <param name="directoryName">Directory name.</param>
        public CCParticleSystemConfig (string plistFile, string directoryName = null)
            : this(plistFile, directoryName, false)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="CocosSharp.CCParticleSystemConfig"/> class.
        /// </summary>
        /// <param name="plistFile">Plist file.</param>
        /// <param name="directoryName">Directory name.</param>
        /// <param name="loadingAsync">If set to <c>true</c> loading async.</param>
        internal CCParticleSystemConfig (string plistFile, string directoryName = null, bool loadingAsync = false)
            : this(CCContentManager.SharedContentManager.Load<PlistDocument>(plistFile).Root.AsDictionary, directoryName, loadingAsync)
        {
            Name = plistFile;
        }


        CCParticleSystemConfig(PlistDictionary dictionary, string directoryName, bool loadingAsync) 
        {
            ParticleSystemType = CCParticleSystemType.Cocos2D;

            MaxParticles = dictionary ["maxParticles"].AsInt;
            Duration = dictionary["duration"].AsFloat;
            Life = dictionary["particleLifespan"].AsFloat;
            LifeVar = dictionary["particleLifespanVariance"].AsFloat;

            Angle = dictionary["angle"].AsFloat;
            AngleVar = dictionary["angleVariance"].AsFloat;

            CCBlendFunc blendFunc = new CCBlendFunc();
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

                CCPoint gravity;
                gravity.X = dictionary["gravityx"].AsFloat;
                gravity.Y = dictionary["gravityy"].AsFloat;
                Gravity = gravity;

                GravitySpeed = dictionary["speed"].AsFloat;
                GravitySpeedVar = dictionary["speedVariance"].AsFloat;
                GravityRadialAccel = dictionary["radialAcceleration"].AsFloat;
                GravityRadialAccelVar = dictionary["radialAccelVariance"].AsFloat;
                GravityTangentialAccel = dictionary["tangentialAcceleration"].AsFloat;
                GravityTangentialAccelVar = dictionary["tangentialAccelVariance"].AsFloat;
                GravityRotationIsDir = dictionary["rotationIsDir"].AsBool;

            }

            else if (EmitterMode == CCEmitterMode.Radius)
            {
                RadialStartRadius = dictionary["maxRadius"].AsFloat;
                RadialStartRadiusVar = dictionary["maxRadiusVariance"].AsFloat;
                RadialEndRadius = dictionary["minRadius"].AsFloat;
                RadialEndRadiusVar = 0.0f;
                RadialRotatePerSecond = dictionary["rotatePerSecond"].AsFloat;
                RadialRotatePerSecondVar = dictionary["rotatePerSecondVariance"].AsFloat;
            }
            else
            {
                Debug.Assert(false, "Invalid emitterType in config file");
                return;
            }

            TextureName = dictionary["textureFileName"].AsString;
            TextureData = dictionary["textureImageData"].AsString;

            if (!loadingAsync)
                LoadParticleTexture();

        }

        internal void LoadParticleTexture()
        {

            string textureName = TextureName;

            CCTexture2D tex = null;

            string textureData = TextureData;

            // We will try loading the texture data first if it exists.
            if (!string.IsNullOrEmpty(textureData))
            {
                //Debug.Assert(!string.IsNullOrEmpty(textureData),
                //    string.Format("CCParticleSystem: textureData does not exist : {0}", textureName));

                int dataLen = textureData.Length;
                if (dataLen != 0)
                {

                    var dataBytes = Convert.FromBase64String(textureData);
                    Debug.Assert(dataBytes != null,
                        string.Format("CCParticleSystem: error decoding textureImageData : {0}", textureName));

                    var imageBytes = ZipUtils.Inflate(dataBytes);
                    Debug.Assert(imageBytes != null,
                        string.Format("CCParticleSystem: error init image with Data for texture : {0}", textureName));

                    try
                    {
                        tex = CCApplication.SharedApplication.TextureCache.AddImage(imageBytes, textureName, CCSurfaceFormat.Color);
                    }
                    catch (Exception ex)
                    {
                        CCLog.Log(ex.ToString());
                        //Texture = CCParticleExample.DefaultTexture;

                    }

                    if (tex == null)
                    {
                        if (!string.IsNullOrEmpty(textureName))
                        {
                            bool bNotify = CCFileUtils.IsPopupNotify;
                            CCFileUtils.IsPopupNotify = false;
                            try
                            {
                                tex = CCApplication.SharedApplication.TextureCache.AddImage(textureName);
                            }
                            catch (Exception)
                            {
                                tex = null;
                                Texture = CCParticleExample.DefaultTexture;

                            }

                            CCFileUtils.IsPopupNotify = bNotify;
                            if (tex == null)
                                Texture = CCParticleExample.DefaultTexture;
                        }

                    }
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(textureName))
                {
                    bool bNotify = CCFileUtils.IsPopupNotify;
                    CCFileUtils.IsPopupNotify = false;
                    try
                    {
                        tex = CCApplication.SharedApplication.TextureCache.AddImage(textureName);
                    }
                    catch (Exception)
                    {
                        tex = null;
                    }

                    CCFileUtils.IsPopupNotify = bNotify;
                }
            }
            if (tex != null)
            {
                Texture = tex;
            }

            Debug.Assert(Texture != null, 
                string.Format("CCParticleSystem: error loading the texture : {0}", textureName));
        }

        #region Cleaning up

        // No unmanaged resources, so no need for finalizer

        public void Dispose()
        {
            this.Dispose(true);

            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
            }
        }

        #endregion Cleaning up
    }
}

