/****************************************************************************
Copyright (c) 2010-2012 cocos2d-x.org
Copyright (C) 2008      Apple Inc. All Rights Reserved.
Copyright (c) 2011      Zynga Inc.
Copyright (c) 2011-2012 openxlive.com

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
****************************************************************************/

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#if OPENGL
#if MONOMAC
using MonoMac.OpenGL;
#elif WINDOWS || LINUX
using OpenTK.Graphics.OpenGL;
#elif GLES
using OpenTK.Graphics.ES20;
using TextureTarget = OpenTK.Graphics.ES20.All;
using TextureMinFilter = OpenTK.Graphics.ES20.All;
#endif
#endif

namespace Cocos2D
{
    /// <summary>
    /// Possible texture pixel formats
    /// </summary>
    public enum CCTexture2DPixelFormat
    {
        kCCTexture2DPixelFormat_Automatic = 0,
        //! 32-bit texture: RGBA8888
        kCCTexture2DPixelFormat_RGBA8888,
        //! 24-bit texture: RGBA888
        kCCTexture2DPixelFormat_RGB888,
        //! 16-bit texture without Alpha channel
        kCCTexture2DPixelFormat_RGB565,
        //! 8-bit textures used as masks
        kCCTexture2DPixelFormat_A8,
        //! 8-bit intensity texture
        kCCTexture2DPixelFormat_I8,
        //! 16-bit textures used as masks
        kCCTexture2DPixelFormat_AI88,
        //! 16-bit textures: RGBA4444
        kCCTexture2DPixelFormat_RGBA4444,
        //! 16-bit textures: RGB5A1
        kCCTexture2DPixelFormat_RGB5A1,
        //! 4-bit PVRTC-compressed texture: PVRTC4
        kCCTexture2DPixelFormat_PVRTC4,
        //! 2-bit PVRTC-compressed texture: PVRTC2
        kCCTexture2DPixelFormat_PVRTC2,

        //! Default texture format: RGBA8888
        kCCTexture2DPixelFormat_Default = kCCTexture2DPixelFormat_RGBA8888,

        // backward compatibility stuff
        kTexture2DPixelFormat_Automatic = kCCTexture2DPixelFormat_Automatic,
        kTexture2DPixelFormat_RGBA8888 = kCCTexture2DPixelFormat_RGBA8888,
        kTexture2DPixelFormat_RGB888 = kCCTexture2DPixelFormat_RGB888,
        kTexture2DPixelFormat_RGB565 = kCCTexture2DPixelFormat_RGB565,
        kTexture2DPixelFormat_A8 = kCCTexture2DPixelFormat_A8,
        kTexture2DPixelFormat_RGBA4444 = kCCTexture2DPixelFormat_RGBA4444,
        kTexture2DPixelFormat_RGB5A1 = kCCTexture2DPixelFormat_RGB5A1,
        kTexture2DPixelFormat_Default = kCCTexture2DPixelFormat_Default
    };

    /// <summary>
    /// Extension to set the Min / Mag filter
    /// </summary>
    public struct CCTexParams
    {
        public uint MagFilter;
        public uint MinFilter;
        public uint WrapS;
        public uint WrapT;
    }

    /// <summary>
    /// This class allows to easily create OpenGL 2D textures from images, text or raw data.
    /// The created CCTexture2D object will always have power-of-two dimensions. 
    /// Depending on how you create the CCTexture2D object, the actual image area of the texture might be smaller than the texture dimensions i.e. "contentSize" != (pixelsWide, pixelsHigh) and (maxS, maxT) != (1.0, 1.0).
    /// Be aware that the content of the generated textures will be upside-down!
    /// </summary>
    public class CCTexture2D : IDisposable
    {
        // If the image has alpha, you can create RGBA8 (32-bit) or RGBA4 (16-bit) or RGB5A1 (16-bit)
        // Default is: RGBA8888 (32-bit textures)
        public static CCTexture2DPixelFormat g_defaultAlphaPixelFormat = CCTexture2DPixelFormat.kCCTexture2DPixelFormat_Default;
        private bool m_bPVRHaveAlphaPremultiplied;

        #region Property

        private bool m_bHasPremultipliedAlpha;
        private CCTexture2DPixelFormat m_ePixelFormat;
        private float m_fMaxS;
        private float m_fMaxT;
        private CCSize m_tContentSize;
        private int m_uPixelsHigh;

        private int m_uPixelsWide;
        private Texture2D m_texture2D;
        private bool m_bIsManaged = false;
        private string m_ContentFile = null;

        internal SamplerState m_samplerState;

        public string ContentFile
        {
            get
            {
                return (m_ContentFile);
            }
            set
            {
                m_ContentFile = value;
            }
        }

        public bool IsManaged
        {
            get
            {
                return (m_bIsManaged);
            }
            set
            {
                m_bIsManaged = value;
            }
        }

        public bool IsTextureDefined
        {
            get
            {
                return (m_texture2D != null && !m_texture2D.IsDisposed);
            }
        }

        internal Texture2D Texture
        {
            get
            {
                if (m_texture2D != null && m_texture2D.IsDisposed)
                {
                    // Need to get it from the cache?
                    if (IsManaged)
                    {
                        CCTextureCache.SharedTextureCache.ReloadMyTexture(this);
                    }
                    else if (m_spriteFont != null && m_CallParams != null)
                    {
                        // THis is a label, so restore it using hte call parameters.
                        InitWithString((string)m_CallParams[0], (CCSize)m_CallParams[1], (CCTextAlignment)m_CallParams[2], (CCVerticalTextAlignment)m_CallParams[3], (string)m_CallParams[4], (float)m_CallParams[5]);
                    }
                }
                else if(m_texture2D == null)
                {
                    if (m_ContentFile != null)
                    {
                        CCLog.Log("CCTexture2D: Creating myself as a new texture from {0}", m_ContentFile);

                        CCTexture2D texture = CCTextureCache.SharedTextureCache.AddImage(m_ContentFile);
                        m_texture2D = texture.m_texture2D;
                    }
                    else if (m_spriteFont != null && m_CallParams != null)
                    {
                        CCLog.Log("CCTexture2D: creating the sprite font label from call parameters.");
                        // THis is a label, so restore it using hte call parameters.
                        InitWithString((string)m_CallParams[0], (CCSize)m_CallParams[1], (CCTextAlignment)m_CallParams[2], (CCVerticalTextAlignment)m_CallParams[3], (string)m_CallParams[4], (float)m_CallParams[5]);
                    }
                    else if (m_spriteFont != null)
                    {
                        CCLog.Log("Oops - need to recreate the texture for the spritefont!");
                    }
                    else
                    {
                        // CCLog.Log("CCTexture2D: null Texture2D object, refresh the content manager!");
                    }
                }
                return (m_texture2D);
            }
            set
            {
                m_texture2D = value;
            }
        }

        /// <summary>
        /// Contains the full pixmap of the sprite - very expensive
        /// </summary>
        private Color[] _MyTextureData;

        /// <summary>
        /// Returns the pixmap of the sprite - this requries a tremendous amount of memory
        /// </summary>
        public Color[] TextureData
        {
            get
            {
                if (_MyTextureData == null)
                {
                    Color[] bitsA = new Color[Texture2D.Width * Texture2D.Height];
                    Texture2D.GetData(bitsA);
                    _MyTextureData = bitsA;
                }
                return (_MyTextureData);
            }
        }

        /// <summary>
        /// pixel format of the texture
        /// </summary>
        public CCTexture2DPixelFormat PixelFormat
        {
            get { return m_ePixelFormat; }
            set { m_ePixelFormat = value; }
        }

        /// <summary>
        /// width in pixels
        /// </summary>
        public int PixelsWide
        {
            get { return m_uPixelsWide; }
            set { m_uPixelsWide = value; }
        }

        /// <summary>
        /// hight in pixels
        /// </summary>
        public int PixelsHigh
        {
            get { return m_uPixelsHigh; }
            set { m_uPixelsHigh = value; }
        }

        /// <summary>
        /// texture name
        /// </summary>
        public uint Name { get; set; }

        /// <summary>
        /// content size
        /// </summary>
        public CCSize ContentSizeInPixels
        {
            get { return m_tContentSize; }
            set { m_tContentSize = value; }
        }

        /// <summary>
        /// texture max S
        /// </summary>
        public float MaxS
        {
            get { return m_fMaxS; }
            set { m_fMaxS = value; }
        }

        /// <summary>
        /// texture max T
        /// </summary>
        public float MaxT
        {
            get { return m_fMaxT; }
            set { m_fMaxT = value; }
        }

        /// <summary>
        /// whether or not the texture has their Alpha premultiplied
        /// </summary>
        public bool HasPremultipliedAlpha
        {
            get { return m_bHasPremultipliedAlpha; }
            set { m_bHasPremultipliedAlpha = value; }
        }

        public Texture2D Texture2D
        {
            get { return Texture; }
        }

        /// <summary>
        /// returns the content size of the texture in points
        /// </summary>
        public CCSize ContentSize
        {
            get
            {
                var ret = new CCSize();
                ret.Width = m_tContentSize.Width / CCMacros.CCContentScaleFactor();
                ret.Height = m_tContentSize.Height / CCMacros.CCContentScaleFactor();

                return ret;
            }
        }

        #endregion

        public CCTexture2D()
        {
            m_uPixelsWide = 0;
            m_uPixelsHigh = 0;
            m_fMaxS = 0.0f;
            m_fMaxT = 0.0f;
            m_bHasPremultipliedAlpha = false;
            m_bPVRHaveAlphaPremultiplied = true;
            m_tContentSize = new CCSize();

            // We will default to LinearClamp which is the default for SpriteBatch.Begin()
            m_samplerState = SamplerState.LinearClamp;
        }

        public override string ToString()
        {
            string ret = "<CCTexture2D | Dimensions = " + m_uPixelsWide + " x " + m_uPixelsHigh + " | Coordinates = (" + m_fMaxS + ", " + m_fMaxT +
                         ")>";
            return ret;
        }

        public void SaveAsJpeg(Stream stream, int width, int height)
        {
            if (Texture != null)
            {
                Texture.SaveAsJpeg(stream, width, height);
            }
        }

        public void SaveAsPng(Stream stream, int width, int height)
        {
            if (Texture != null)
            {
                Texture.SaveAsPng(stream, width, height);
            }
        }

        /** sets the min filter, mag filter, wrap s and wrap t texture parameters.
        If the texture size is NPOT (non power of 2), then in can only use GL_CLAMP_TO_EDGE in GL_TEXTURE_WRAP_{S,T}.
        @since v0.8
        */
        public CCTexParams TexParameters
        {
            set 
            {
#if OPENGL
                m_samplerState = new SamplerState();
                m_samplerState.Filter = GetFilter(value.MagFilter, value.MinFilter);
                m_samplerState.AddressU = GetWrapMode(value.WrapS);
                m_samplerState.AddressV = GetWrapMode(value.WrapT);
#else
                // We will just return the default for now for non OpenGL
                // Non OpenGL systems should use the SamplerState property
                m_samplerState = SamplerState.LinearClamp;
#endif

            }
        }

        /**
         * Sets the XNA sampler state for the Texture in those cases where OpenGL is not the platform
         * 
         */
        public SamplerState SamplerState
        {
            set
            {
                m_samplerState = value;
            }

        }

        private TextureAddressMode GetWrapMode(uint param)
        {
#if OPENGL
            switch ((All)param)
            {
            case All.Repeat :
                return TextureAddressMode.Wrap;
            case All.ClampToEdge:
                return TextureAddressMode.Clamp;
            case All.MirroredRepeat:
                return TextureAddressMode.Mirror;
            default:
                return TextureAddressMode.Clamp;
            }
#else
            return TextureAddressMode.Clamp;

#endif
        }

        /**
         * Gets the Filter of the SamplerState based on parameters magFilter and minFilter
         * 
         * Right now we are only setting based on magFilter to control the TextureFilter
         * We will probably want to modify this in the future and base it off of a 
         * combination of both the magFilter and minFilter
         * 
         * TODO: Look deeper at the combinations of the magFilter and minFilter parameters to 
         * determine the correct SamplterState TextureFilter to be used.
         */
        private TextureFilter GetFilter (uint magFilter, uint minFilter)
        {
#if OPENGL
            switch ((All)magFilter)
            {
            case All.Linear:
                return TextureFilter.Linear;
            case All.Nearest:
                return TextureFilter.Point;
            default:
                return TextureFilter.Linear;
            }
#else
            return TextureFilter.Linear;
#endif
        }

        /** sets antialias texture parameters:
        - GL_TEXTURE_MIN_FILTER = GL_LINEAR
        - GL_TEXTURE_MAG_FILTER = GL_LINEAR

        @since v0.8
        */

        public void SetAntiAliasTexParameters()
        {
            var saveState = m_samplerState;
            m_samplerState = new SamplerState();
            m_samplerState.Filter = TextureFilter.Point;
            m_samplerState.AddressU = saveState.AddressU;
            m_samplerState.AddressV = saveState.AddressV;
            m_samplerState.AddressW = saveState.AddressW;
        }

        /** sets alias texture parameters:
        - GL_TEXTURE_MIN_FILTER = GL_NEAREST
        - GL_TEXTURE_MAG_FILTER = GL_NEAREST

        @since v0.8
        */

        public void SetAliasTexParameters()
        {
            var saveState = m_samplerState;
            m_samplerState = new SamplerState();
            m_samplerState.Filter = TextureFilter.Linear;
            m_samplerState.AddressU = saveState.AddressU;
            m_samplerState.AddressV = saveState.AddressV;
            m_samplerState.AddressW = saveState.AddressW;
        }


        /** Generates mipmap images for the texture.
        It only works if the texture size is POT (power of 2).
        @since v0.99.0
        */

        public void GenerateMipmap()
        {
            //throw new NotImplementedException();
        }

        /** returns the bits-per-pixel of the in-memory OpenGL texture
        @since v1.0
        */

        public uint BitsPerPixelForFormat()
        {
            throw new NotImplementedException();
        }

        public void SetPvrImagesHavePremultipliedAlpha(bool haveAlphaPremultiplied)
        {
            m_bPVRHaveAlphaPremultiplied = haveAlphaPremultiplied;
        }

        /** sets the default pixel format for UIImages that contains alpha channel.
        If the UIImage contains alpha channel, then the options are:
        - generate 32-bit textures: kCCTexture2DPixelFormat_RGBA8888 (default one)
        - generate 24-bit textures: kCCTexture2DPixelFormat_RGB888
        - generate 16-bit textures: kCCTexture2DPixelFormat_RGBA4444
        - generate 16-bit textures: kCCTexture2DPixelFormat_RGB5A1
        - generate 16-bit textures: kCCTexture2DPixelFormat_RGB565
        - generate 8-bit textures: kCCTexture2DPixelFormat_A8 (only use it if you use just 1 color)

        How does it work ?
        - If the image is an RGBA (with Alpha) then the default pixel format will be used (it can be a 8-bit, 16-bit or 32-bit texture)
        - If the image is an RGB (without Alpha) then an RGB565 or RGB888 texture will be used (16-bit texture)

        @since v0.8
        */

        /** returns the alpha pixel format
        @since v0.8
        */

        public static CCTexture2DPixelFormat DefaultAlphaPixelFormat
        {
            set { }
            get { return CCTexture2DPixelFormat.kCCTexture2DPixelFormat_RGBA8888; }
        }

        #region raw data

        /// <summary>
        /// These functions are needed to create mutable textures
        /// </summary>
        public void ReleaseData(object data)
        {
            throw new NotImplementedException();
        }

        public object KeepData(object data, uint length)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Intializes with a texture2d with data
        /// </summary>
        public bool InitWithData(object data, CCTexture2DPixelFormat pixelFormat, uint pixelsWide, uint pixelsHigh, CCSize contentSize)
        {
            CCApplication app = CCApplication.SharedApplication;

            Texture = new Texture2D(app.GraphicsDevice, (int) contentSize.Width, (int) contentSize.Height);

            m_tContentSize = contentSize;
            m_uPixelsWide = (int) pixelsWide;
            m_uPixelsHigh = (int) pixelsHigh;
            m_ePixelFormat = pixelFormat;
            m_fMaxS = contentSize.Width / (pixelsWide);
            m_fMaxT = contentSize.Height / (pixelsHigh);

            m_bHasPremultipliedAlpha = false;
            //m_bHasMipmaps = false;

            return true;
        }

        #endregion

        #region create extensions

        private object[] m_CallParams;

        public bool InitWithString(string text, string fontName, float fontSize)
        {
            return InitWithString(text, CCSize.Zero, CCTextAlignment.CCTextAlignmentCenter, CCVerticalTextAlignment.CCVerticalTextAlignmentTop,
                                  fontName, fontSize);
        }

        private SpriteFont m_spriteFont;
        public bool InitWithString(string text, CCSize dimensions, CCTextAlignment hAlignment, CCVerticalTextAlignment vAlignment, string fontName,
                                   float fontSize)
        {
            try
            {
                m_CallParams = new object[] { text, dimensions, hAlignment, vAlignment, fontName, fontSize };

                // CCLog.Log("InitWithString: text={0}", text);

                Debug.Assert(dimensions.Width >= 0 || dimensions.Height >= 0);

                if (string.IsNullOrEmpty(text))
                {
                    return false;
                }

                SpriteFont font = m_spriteFont;

                if (font == null)
                {
                    font = CCSpriteFontCache.SharedInstance.GetFont(fontName, fontSize);
                    if (font == null)
                    {
                        CCLog.Log("Can't find {0}, use system default ({1})", fontName, CCDrawManager.DefaultFont);
                        font = CCSpriteFontCache.SharedInstance.GetFont(CCDrawManager.DefaultFont, fontSize);
                        if (font == null)
                        {
                            CCLog.Log("Failed to load default font. No font supported.");
                        }
                    }
                    // m_spriteFont = font;
                }
                
                if (font == null)
                    return (false);

                // m_spriteFont = font;

                if (dimensions.Equals(CCSize.Zero))
                {
                    Microsoft.Xna.Framework.Vector2 temp = font.MeasureString(text);
                    dimensions.Width = temp.X;
                    dimensions.Height = temp.Y;
                }

                //float scale = 1.0f;//need refer fontSize;

                var textList = new List<String>();
                var nextText = new StringBuilder();
                string[] lineList = text.Split('\n');

                float spaceWidth = font.MeasureString(" ").X;

                for (int j = 0; j < lineList.Length; ++j)
                {
                    string[] wordList = lineList[j].Split(' ');

                    float lineWidth = 0;
                    bool firstWord = true;
                    for (int i = 0; i < wordList.Length; ++i)
                    {
                        lineWidth += font.MeasureString(wordList[i]).X;

                        if (lineWidth > dimensions.Width)
                        {
                            lineWidth = 0;

                            if (nextText.Length > 0)
                            {
                                firstWord = true;
                                textList.Add(nextText.ToString());
#if XBOX || XBOX360
                                nextText.Length = 0;
#else
                        nextText.Clear();
#endif
                            }
                            else
                            {
                                firstWord = false;
                                textList.Add(wordList[i]);
                                continue;
                            }
                        }

                        if (!firstWord)
                        {
                            nextText.Append(' ');
                            lineWidth += spaceWidth;
                        }

                        nextText.Append(wordList[i]);
                        firstWord = false;
                    }

                    textList.Add(nextText.ToString());
#if XBOX || XBOX360
                    nextText.Length = 0;
#else
                        nextText.Clear();
#endif
                }

                if (dimensions.Height == 0)
                {
                    dimensions.Height = textList.Count * font.LineSpacing;
                }

                //*  for render to texture
                RenderTarget2D renderTarget = CCDrawManager.CreateRenderTarget((int)dimensions.Width, (int)dimensions.Height,
                                                                              RenderTargetUsage.PreserveContents);
                CCDrawManager.SetRenderTarget(renderTarget);

                CCDrawManager.Clear(Color.Transparent);

                SpriteBatch spriteBatch = CCDrawManager.spriteBatch;

                spriteBatch.Begin();

                int textHeight = textList.Count * font.LineSpacing;
                float nextY = 0;

                if (vAlignment == CCVerticalTextAlignment.CCVerticalTextAlignmentBottom)
                {
                    nextY = dimensions.Height - textHeight;
                }
                else if (vAlignment == CCVerticalTextAlignment.CCVerticalTextAlignmentCenter)
                {
                    nextY = (dimensions.Height - textHeight) / 2.0f;
                }

                for (int j = 0; j < textList.Count; ++j)
                {
                    string line = textList[j];

					var position = new Microsoft.Xna.Framework.Vector2(0, nextY);

                    if (hAlignment == CCTextAlignment.CCTextAlignmentRight)
                    {
                        position.X = dimensions.Width - font.MeasureString(line).X;
                    }
                    else if (hAlignment == CCTextAlignment.CCTextAlignmentCenter)
                    {
                        position.X = (dimensions.Width - font.MeasureString(line).X) / 2.0f;
                    }

                    spriteBatch.DrawString(font, line, position, Color.White);
                    nextY += font.LineSpacing;
                }
                spriteBatch.End();

                CCDrawManager.graphicsDevice.RasterizerState = RasterizerState.CullNone;
                CCDrawManager.graphicsDevice.DepthStencilState = DepthStencilState.Default;

                CCDrawManager.SetRenderTarget((RenderTarget2D)null);

                // to copy the rendered target data to a plain texture(to the memory)
                //            texture2D = CCDrawManager.CreateTexture2D(renderTarget.Width, renderTarget.Height);
                // This is the old 3.1 way of doing things. 4.0 does not need this and it causes compatibility problems.

                //            var colors1D = new Color[renderTarget.Width * renderTarget.Height];
                //            renderTarget.GetData(colors1D);
                //            texture2D.SetData(colors1D);
                return InitWithTexture(renderTarget);
            }
            catch (Exception ex)
            {
                CCLog.Log(ex.ToString());
            }
            return (false);
        }

        /** Initializes a texture from a PVR file */

        public bool InitWithPvrFile(string file)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Initializes a texture from a content file
        /// </summary>
        public bool InitWithTexture(Texture2D texture)
        {
            if (null == texture)
            {
                return false;
            }

            long POTWide = texture.Width; //ccUtils.ccNextPOT(texture.Width);
            long POTHigh = texture.Height; //ccUtils.ccNextPOT(texture.Height);

            int maxTextureSize = CCConfiguration.SharedConfiguration.MaxTextureSize;
            if (POTHigh > maxTextureSize || POTWide > maxTextureSize)
            {
                CCLog.Log(string.Format("cocos2d: WARNING: Image ({0} x {1}) is bigger than the supported {2} x {3}", POTWide, POTHigh, maxTextureSize,
                                        maxTextureSize));
                return false;
            }
#if IPHONE
            m_bHasPremultipliedAlpha = false;
            return InitTextureWithImage(texture, texture.Width, texture.Height);
#else
            return InitPremultipliedATextureWithImage(texture, texture.Width, texture.Height);
#endif
        }


        public bool InitTextureWithImage(Texture2D texture, int POTWide, int POTHigh)
        {
            Texture = texture;
            m_tContentSize.Width = Texture.Width;
            m_tContentSize.Height = Texture.Height;

            m_uPixelsWide = POTWide;
            m_uPixelsHigh = POTHigh;
            //m_ePixelFormat = pixelFormat;
            m_fMaxS = m_tContentSize.Width / (POTWide);
            m_fMaxT = m_tContentSize.Height / (POTHigh);
            return true;
        }

        public bool InitPremultipliedATextureWithImage(Texture2D texture, int POTWide, int POTHigh)
        {
            if (!InitTextureWithImage(texture, POTWide, POTHigh))
            {
                return (false);
            }
            m_bHasPremultipliedAlpha = true;
            return true;
        }

        /** Initializes a texture from a file */

        public bool InitWithFile(string file)
        {
            throw new NotImplementedException();
        }

        #endregion

        //private bool initPremultipliedATextureWithImage(CCImage image, uint pixelsWide, uint pixelsHigh)
        //{
        //    throw new NotImplementedException();
        //}

        // By default PVR images are treated as if they don't have the alpha channel premultiplied

        #region IDisposable Members

        /// <summary>
        /// Dumps the texture2D used by this texture.
        /// </summary>
        public void Dispose()
        {
            if (!m_bIsManaged)
            {
                if (m_texture2D != null && !m_texture2D.IsDisposed)
                {
                    m_texture2D.Dispose();
                    m_texture2D = null;
                }
            }
        }

        #endregion
    }
}