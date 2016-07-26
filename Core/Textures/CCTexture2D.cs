using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#if (WINDOWS && !NETFX_CORE)
using BitMiracle.LibTiff.Classic;
#endif

namespace CocosSharp
{
    #region Enums and structs

    public enum CCImageFormat
    {
        Jpg = 0,
        Png,
        Tiff,
        Webp,
        Gif,
        RawData,
        UnKnown
    }

    public enum CCSurfaceFormat
    {
        Color = 0,
        Bgr565 = 1,
        Bgra5551 = 2,
        Bgra4444 = 3,
        Dxt1 = 4,
        Dxt3 = 5,
        Dxt5 = 6,
        NormalizedByte2 = 7,
        NormalizedByte4 = 8,
        Rgba1010102 = 9,
        Rg32 = 10,
        Rgba64 = 11,
        Alpha8 = 12,
        Single = 13,
        CCVector2 = 14,
        Vector4 = 15,
        HalfSingle = 16,
        HalfCCVector2 = 17,
        HalfVector4 = 18,
        HdrBlendable = 19,

        // BGRA formats are required for compatibility with WPF D3DImage.
        Bgr32 = 20,     // B8G8R8X8
        Bgra32 = 21,    // B8G8R8A8

        // Good explanation of compressed formats for mobile devices (aimed at Android, but describes PVRTC)
        // http://developer.motorola.com/docstools/library/understanding-texture-compression/

        // PowerVR texture compression (iOS and Android)
        RgbPvrtc2Bpp = 50,
        RgbPvrtc4Bpp = 51,
        RgbaPvrtc2Bpp = 52,
        RgbaPvrtc4Bpp = 53,

        // Ericcson Texture Compression (Android)
        RgbEtc1 = 60,

        // DXT1 also has a 1-bit alpha form
        Dxt1a = 70,
    }

    internal enum CCTextureCacheType
    {
        None,
        AssetFile,
        Data,
        RawData,
        String
    }

    internal struct CCStringCache
    {
        public String Text;
        public CCSize Dimensions;
        public CCTextAlignment HAlignment;
        public CCVerticalTextAlignment VAlignment;
        public String FontName;
        public float FontSize;
    }

    internal struct CCTextureCacheInfo
    {
        public CCTextureCacheType CacheType;
        public Object Data;
    }

    #endregion Enums and structs


    public class CCTexture2D : CCGraphicsResource
    {
        public static CCSurfaceFormat DefaultAlphaPixelFormat = CCSurfaceFormat.Color;
        public static bool OptimizeForPremultipliedAlpha = true;
        public static bool DefaultIsAntialiased = true;

        bool hasMipmaps;
        bool managed;
        bool antialiased;

        CCTextureCacheInfo cacheInfo;
        Texture2D texture2D;
        private readonly int textureId = Interlocked.Increment(ref lastTextureId);
        private static int lastTextureId;

        #region Properties

        /// <summary>
        /// Gets a unique identifier of this texture for batch rendering purposes.
        /// </summary>
        /// <remarks>
        /// <para>For example, this value is used by <see cref="CCRenderer"/> when determining batch rendering.</para>
        /// <para>The value is an implementation detail and may change between application launches or CocosSharp versions.
        /// It is only guaranteed to stay consistent during application lifetime.</para>
        /// </remarks>
        internal int TextureId { get { return textureId; } }

        public bool HasPremultipliedAlpha { get; private set; }
        public int PixelsWide { get; private set; }
        public int PixelsHigh { get; private set; }
        public CCSize ContentSizeInPixels { get; private set; }
        public CCSurfaceFormat PixelFormat { get; set; }
        public SamplerState SamplerState { get; set; }

        public bool IsTextureDefined
        {
            get { return (texture2D != null && !texture2D.IsDisposed); }
        }

        public bool IsAntialiased
        {
            get { return antialiased; }

            set
            {
                if (antialiased != value)
                {
                    antialiased = value;

                    RefreshAntialiasSetting();
                }
            }
        }

        public uint BitsPerPixelForFormat
        {
            //from MG: Microsoft.Xna.Framework.Graphics.GraphicsExtensions
            get
            {
                switch (PixelFormat)
                {
                case CCSurfaceFormat.Dxt1:
                    #if !WINDOWS && !WINDOWS_PHONE
                case CCSurfaceFormat.Dxt1a:
                case CCSurfaceFormat.RgbPvrtc2Bpp:
                case CCSurfaceFormat.RgbaPvrtc2Bpp:
                case CCSurfaceFormat.RgbEtc1:
                    #endif
                    // One texel in DXT1, PVRTC 2bpp and ETC1 is a minimum 4x4 block, which is 8 bytes
                    return 8;

                case CCSurfaceFormat.Dxt3:
                case CCSurfaceFormat.Dxt5:
                    #if !WINDOWS && !WINDOWS_PHONE
                case CCSurfaceFormat.RgbPvrtc4Bpp:
                case CCSurfaceFormat.RgbaPvrtc4Bpp:
                    #endif
                    // One texel in DXT3, DXT5 and PVRTC 4bpp is a minimum 4x4 block, which is 16 bytes
                    return 16;

                case CCSurfaceFormat.Alpha8:
                    return 1;

                case CCSurfaceFormat.Bgr565:
                case CCSurfaceFormat.Bgra4444:
                case CCSurfaceFormat.Bgra5551:
                case CCSurfaceFormat.HalfSingle:
                case CCSurfaceFormat.NormalizedByte2:
                    return 2;

                case CCSurfaceFormat.Color:
                case CCSurfaceFormat.Single:
                case CCSurfaceFormat.Rg32:
                case CCSurfaceFormat.HalfCCVector2:
                case CCSurfaceFormat.NormalizedByte4:
                case CCSurfaceFormat.Rgba1010102:
                    return 4;

                case CCSurfaceFormat.HalfVector4:
                case CCSurfaceFormat.Rgba64:
                case CCSurfaceFormat.CCVector2:
                    return 8;

                case CCSurfaceFormat.Vector4:
                    return 16;

                default:
                    throw new NotImplementedException();
                }
            }
        }

        public Texture2D Name
        {
            get { return XNATexture; }
        }

        public Texture2D XNATexture
        {
            get
            {
                if (texture2D != null && texture2D.IsDisposed)
                {
                    ReinitResource();
                }
                return texture2D;
            }
        }

        #endregion Properties


        #region Constructors and initialization

        public CCTexture2D()
        {
            SamplerState = SamplerState.LinearClamp;
            IsAntialiased = DefaultIsAntialiased;

            RefreshAntialiasSetting();
        }

        public CCTexture2D (int pixelsWide, int pixelsHigh, CCSurfaceFormat pixelFormat=CCSurfaceFormat.Color, bool premultipliedAlpha=true, bool mipMap=false) 
            : this(new Texture2D(CCDrawManager.SharedDrawManager.XnaGraphicsDevice, pixelsWide, pixelsHigh, mipMap, (SurfaceFormat)pixelFormat), pixelFormat, premultipliedAlpha)
        {
            cacheInfo.CacheType = CCTextureCacheType.None;
            cacheInfo.Data = null;
        }

        public CCTexture2D(byte[] data, CCSurfaceFormat pixelFormat=CCSurfaceFormat.Color, bool mipMap=false)
            : this()
        {
            InitWithData(data, pixelFormat, mipMap);
        }

        public CCTexture2D(Stream stream, CCSurfaceFormat pixelFormat=CCSurfaceFormat.Color) 
            : this()
        {
            InitWithStream(stream, pixelFormat);
        }

        public CCTexture2D(string text, CCSize dimensions, CCTextAlignment hAlignment, 
            CCVerticalTextAlignment vAlignment, string fontName, float fontSize) 
            : this()
        {
            InitWithString(text, dimensions, hAlignment, vAlignment, fontName, fontSize);
        }

        public CCTexture2D(string text, string fontName, float fontSize) 
            : this(text, CCSize.Zero, CCTextAlignment.Center, CCVerticalTextAlignment.Top, fontName, fontSize)
        {
        }

        public CCTexture2D(string file) 
            : this()
        {
            InitWithFile(file);
        }

        public CCTexture2D(Texture2D texture, CCSurfaceFormat format, bool premultipliedAlpha=true, bool managed=false)
            : this()
        {
            InitWithTexture(texture, format, premultipliedAlpha, managed);
        }

        public CCTexture2D(Texture2D texture) 
            : this(texture, (CCSurfaceFormat)texture.Format)
        {
        }

        internal void InitWithRawData<T>(T[] data, CCSurfaceFormat pixelFormat, int pixelsWide, int pixelsHigh, bool premultipliedAlpha, bool mipMap)
            where T : struct
        {
            InitWithRawData(data, pixelFormat, pixelsWide, pixelsHigh, premultipliedAlpha, mipMap, new CCSize(pixelsWide, pixelsHigh));
        }

        internal void InitWithRawData<T>(T[] data, CCSurfaceFormat pixelFormat, int pixelsWide, int pixelsHigh,
            bool premultipliedAlpha, bool mipMap, CCSize ContentSizeInPixelsIn) where T : struct
        {
            var texture = LoadRawData(data, pixelsWide, pixelsHigh, (SurfaceFormat)pixelFormat, mipMap);
            InitWithTexture(texture, pixelFormat, premultipliedAlpha, false);

            ContentSizeInPixels = ContentSizeInPixelsIn;

            cacheInfo.CacheType = CCTextureCacheType.RawData;
            cacheInfo.Data = data;
        }

        void InitWithData(byte[] data, CCSurfaceFormat pixelFormat, bool mipMap)
        {
            if (data == null)
            {
                return;
            }

            #if WINDOWS_PHONE8
            /*
            byte[] cloneOfData = new byte[data.Length];
            data.CopyTo(cloneOfData, 0);
            data = cloneOfData;
            */
            #endif

            var texture = LoadTexture(new MemoryStream(data, false));

            if (texture != null)
            {
                InitWithTexture(texture, pixelFormat, true, false);
                cacheInfo.CacheType = CCTextureCacheType.Data;
                cacheInfo.Data = data;

                if (mipMap)
                {
                    GenerateMipmap();
                }
            }
        }

        void InitWithStream(Stream stream, CCSurfaceFormat pixelFormat)
        {
            Texture2D texture;
            try
            {
                texture = LoadTexture(stream);

                InitWithTexture(texture, pixelFormat, false, false);

                return;
            }
            catch (Exception)
            {

            }
        }

        void InitWithFile(string file)
        {
            managed = false;

            Texture2D texture = null;

            cacheInfo.CacheType = CCTextureCacheType.AssetFile;
            cacheInfo.Data = file;

            //TODO: may be move this functional to CCContentManager?

            var contentManager = CCContentManager.SharedContentManager;

            var loadedFile = file;

            // first try to download xnb
            if (Path.HasExtension(loadedFile))
            {
                loadedFile = Path.Combine(Path.GetDirectoryName(file), Path.GetFileNameWithoutExtension(file));
            }

            // use WeakReference. Link for regular textures are stored in CCTextureCache
            texture = contentManager.TryLoad<Texture2D>(loadedFile, true);

            if (texture != null)
            {
                // usually xnb texture prepared as PremultipliedAlpha
                InitWithTexture(texture, DefaultAlphaPixelFormat, true, true);
				return;
            }

            // try load raw image
            if (loadedFile != file)
            {
                texture = contentManager.TryLoad<Texture2D>(file, true);

                if (texture != null)
                {
                    // not premultiplied alpha
                    InitWithTexture(texture, DefaultAlphaPixelFormat, false, true);
					return;
                }
            }

            // try load not supported format (for example tga)
            try
            {
                using (var stream = contentManager.GetAssetStream(file))
                {
                    InitWithStream(stream, DefaultAlphaPixelFormat);
                }
            }
            catch (Exception)
            {
            }

            CCLog.Log("Texture {0} was not found.", file);
        }

        void InitWithString(string text, CCSize dimensions, CCTextAlignment hAlignment, CCVerticalTextAlignment vAlignment, string fontName, float fontSize)
        {
            try
            {
                Debug.Assert(dimensions.Width >= 0 || dimensions.Height >= 0);

                if (string.IsNullOrEmpty(text))
                {
                    return;
                }

                float loadedSize = fontSize;

                SpriteFont font = CCSpriteFontCache.SharedSpriteFontCache.TryLoadFont(fontName, fontSize, out loadedSize);

                if (font == null)
                {
                    CCLog.Log("Failed to load default font. No font supported.");
                    return;
                }

                float scale = 1f;

                if (loadedSize != 0)
                {
                    scale = fontSize / loadedSize * CCSpriteFontCache.SharedSpriteFontCache.FontScale;
                }

                if (dimensions.Equals(CCSize.Zero))
                {
                    CCVector2 temp = font.MeasureString(text).ToCCVector2();
                    dimensions.Width = temp.X * scale;
                    dimensions.Height = temp.Y * scale;
                }

                var textList = new List<String>();
                var nextText = new StringBuilder();

                string[] lineList = text.Split('\n');

                float spaceWidth = font.MeasureString(" ").X * scale;

                for (int j = 0; j < lineList.Length; ++j)
                {
                    string[] wordList = lineList[j].Split(' ');

                    float lineWidth = 0;
                    bool firstWord = true;

                    for (int i = 0; i < wordList.Length; ++i)
                    {
                        float wordWidth = font.MeasureString(wordList[i]).X * scale;

                        if ((lineWidth + wordWidth) > dimensions.Width)
                        {
                            lineWidth = wordWidth;

                            if (nextText.Length > 0)
                            {
                                firstWord = true;
                                textList.Add(nextText.ToString());
                                nextText.Length = 0;
                            }
                            else
                            {
                                lineWidth += wordWidth;
                                firstWord = false;
                                textList.Add(wordList[i]);
                                continue;
                            }
                        }
                        else
                        {
                            lineWidth += wordWidth;
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

                    nextText.Clear();
                }

                if (dimensions.Height == 0)
                {
                    dimensions.Height = textList.Count * font.LineSpacing * scale;
                }

                //*  for render to texture
                RenderTarget2D renderTarget = CCDrawManager.SharedDrawManager.CreateRenderTarget(
                    (int)dimensions.Width, (int)dimensions.Height,
                    DefaultAlphaPixelFormat, CCRenderTargetUsage.DiscardContents
                );

                CCDrawManager.SharedDrawManager.CurrentRenderTarget = renderTarget;
                CCDrawManager.SharedDrawManager.Clear(CCColor4B.Transparent);

                SpriteBatch sb = CCDrawManager.SharedDrawManager.SpriteBatch;
                sb.Begin();

                float textHeight = textList.Count * font.LineSpacing * scale;
                float nextY = 0;

                if (vAlignment == CCVerticalTextAlignment.Bottom)
                {
                    nextY = dimensions.Height - textHeight;
                }
                else if (vAlignment == CCVerticalTextAlignment.Center)
                {
                    nextY = (dimensions.Height - textHeight) / 2.0f;
                }

                for (int j = 0; j < textList.Count; ++j)
                {
                    string line = textList[j];

                    var position = new CCVector2(0, nextY);

                    if (hAlignment == CCTextAlignment.Right)
                    {
                        position.X = dimensions.Width - font.MeasureString(line).X * scale;
                    }
                    else if (hAlignment == CCTextAlignment.Center)
                    {
                        position.X = (dimensions.Width - font.MeasureString(line).X * scale) / 2.0f;
                    }

                    sb.DrawString(font, line, position.ToVector2(), Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0);

                    nextY += font.LineSpacing * scale;
                }

                sb.End();

                CCDrawManager.SharedDrawManager.XnaGraphicsDevice.RasterizerState = RasterizerState.CullNone;
                CCDrawManager.SharedDrawManager.DepthStencilState = DepthStencilState.Default;

                CCDrawManager.SharedDrawManager.CurrentRenderTarget = (RenderTarget2D)null;

                InitWithTexture(renderTarget, (CCSurfaceFormat)renderTarget.Format, true, false);
                cacheInfo.CacheType = CCTextureCacheType.String;
                cacheInfo.Data = new CCStringCache()
                {
                    Dimensions = dimensions,
                    Text = text,
                    FontName = fontName,
                    FontSize = fontSize,
                    HAlignment = hAlignment,
                    VAlignment = vAlignment
                };
            }
            catch (Exception ex)
            {
                CCLog.Log(ex.ToString());
            }
        }

        // Method called externally by CCDrawManager
        internal void InitWithTexture(Texture2D texture, CCSurfaceFormat format, bool premultipliedAlpha, bool managedIn)
        {
            managed = managedIn;

            if (null == texture)
            {
                return;
            }

            if (OptimizeForPremultipliedAlpha && !premultipliedAlpha)
            {
                texture2D = ConvertToPremultiplied(texture, (SurfaceFormat)format);

                if (!managed)
                {
                    texture.Dispose();
                    managed = false;
                }
            }
            else
            {
                if (texture.Format != (SurfaceFormat)format)
                {
                    texture2D = ConvertSurfaceFormat(texture, (SurfaceFormat)format);

                    if (!managed)
                    {
                        texture.Dispose();
                        managed = false;
                    }
                }
                else
                {
                    texture2D = texture;
                }
            }

            PixelFormat = (CCSurfaceFormat)texture.Format;
            PixelsWide = texture.Width;
            PixelsHigh = texture.Height;
            ContentSizeInPixels = new CCSize(texture.Width, texture.Height);
            hasMipmaps = texture.LevelCount > 1;
            HasPremultipliedAlpha = premultipliedAlpha;
        }

        public override void ReinitResource()
        {
            //CCLog.Log("reinit called on texture '{0}' {1}x{2}", Name, ContentSizeInPixels.Width, ContentSizeInPixels.Height);

            Texture2D textureToDispose = null;
            if (texture2D != null && !texture2D.IsDisposed && !managed)
            {
                textureToDispose = texture2D;
                // texture2D.Dispose();
            }

            managed = false;
            texture2D = null;

            switch (cacheInfo.CacheType)
            {
            case CCTextureCacheType.None:
                return;

            case CCTextureCacheType.AssetFile:
                InitWithFile((string)cacheInfo.Data);
                break;

            case CCTextureCacheType.Data:
                InitWithData((byte[])cacheInfo.Data, (CCSurfaceFormat)PixelFormat, hasMipmaps);
                break;

            case CCTextureCacheType.RawData:
                #if NETFX_CORE
                var methodInfo = typeof(CCTexture2D).GetType().GetTypeInfo().GetDeclaredMethod("InitWithRawData");
                #else
                var methodInfo = typeof(CCTexture2D).GetMethod("InitWithRawData", BindingFlags.Public | BindingFlags.Instance);
                #endif
                var genericMethod = methodInfo.MakeGenericMethod(cacheInfo.Data.GetType());
                genericMethod.Invoke(this, new object[]
                    {
                        Convert.ChangeType(cacheInfo.Data, cacheInfo.Data.GetType(),System.Globalization.CultureInfo.InvariantCulture),
                        PixelFormat, PixelsWide, PixelsHigh, 
                        HasPremultipliedAlpha, hasMipmaps, ContentSizeInPixels
                    });

                //                    InitWithRawData((byte[])cacheInfo.Data, PixelFormat, PixelsWide, PixelsHigh,
                //                                    HasPremultipliedAlpha, hasMipmaps, ContentSizeInPixels);
                break;

            case CCTextureCacheType.String:
                var si = (CCStringCache)cacheInfo.Data;
                InitWithString(si.Text, si.Dimensions, si.HAlignment, si.VAlignment, si.FontName, si.FontSize);
                if (hasMipmaps)
                {
                    hasMipmaps = false;
                    GenerateMipmap();
                }
                break;

            default:
                throw new ArgumentOutOfRangeException();
            }
            if (textureToDispose != null && !textureToDispose.IsDisposed)
            {
                textureToDispose.Dispose();
            }
        }

        #endregion Constructors and initialization


        public override string ToString()
        {
            return String.Format("<CCTexture2D | Dimensions = {0} x {1})>", PixelsWide, PixelsHigh);
        }

        #region Cleanup

        void RefreshAntialiasSetting()
        {
            var saveState = SamplerState;

            if (antialiased && SamplerState.Filter != TextureFilter.Linear)
            {
                if (SamplerState == SamplerState.PointClamp)
                {
                    SamplerState = SamplerState.LinearClamp;
                    return;
                }

                SamplerState = new SamplerState
                {
                    Filter = TextureFilter.Linear
                };
            }
            else if (!antialiased && SamplerState.Filter != TextureFilter.Point)
            {
                if (SamplerState == SamplerState.LinearClamp)
                {
                    SamplerState = SamplerState.PointClamp;
                    return;
                }

                SamplerState = new SamplerState
                {
                    Filter = TextureFilter.Point
                };
            }
            else
            {
                return;
            }

            SamplerState.AddressU = saveState.AddressU;
            SamplerState.AddressV = saveState.AddressV;
            SamplerState.AddressW = saveState.AddressW;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing && texture2D != null && !texture2D.IsDisposed && !managed) 
            {
                texture2D.Dispose();
            }

            texture2D = null;
        }

        #endregion Cleanup


        #region Saving texture

        public void SaveAsJpeg(Stream stream, int width, int height)
        {
            if (texture2D != null)
            {
                texture2D.SaveAsJpeg(stream, width, height);
            }
        }

        public void SaveAsPng(Stream stream, int width, int height)
        {
            if (texture2D != null)
            {
                texture2D.SaveAsPng(stream, width, height);
            }
        }

        #endregion Saving texture


        #region Conversion

        public void GenerateMipmap()
        {
            if (!hasMipmaps)
            {
                var target = new RenderTarget2D(CCDrawManager.SharedDrawManager.XnaGraphicsDevice, PixelsWide, PixelsHigh, true, (SurfaceFormat)PixelFormat,
                    DepthFormat.None, 0, RenderTargetUsage.DiscardContents);

                CCDrawManager.SharedDrawManager.CurrentRenderTarget = target;

                SpriteBatch sb = CCDrawManager.SharedDrawManager.SpriteBatch;

                sb.Begin();
                sb.Draw(texture2D, Vector2.Zero, Color.White);
                sb.End();

                if (!managed)
                {
                    texture2D.Dispose();
                }

                managed = false;
                texture2D = target;

                hasMipmaps = true;
            }
        }

        Texture2D ConvertSurfaceFormat(Texture2D texture, SurfaceFormat format)
        {
            if (texture.Format == format)
            {
                return texture;
            }

            var renderTarget = new RenderTarget2D(
                CCDrawManager.SharedDrawManager.XnaGraphicsDevice,
                texture.Width, texture.Height, hasMipmaps, format,
                DepthFormat.None, 0, RenderTargetUsage.DiscardContents
            );

            CCDrawManager.SharedDrawManager.CurrentRenderTarget = renderTarget;
            CCDrawManager.SharedDrawManager.SpriteBatch.Begin();
            CCDrawManager.SharedDrawManager.SpriteBatch.Draw(texture, Vector2.Zero, Color.White);
            CCDrawManager.SharedDrawManager.SpriteBatch.End();
            CCDrawManager.SharedDrawManager.SetRenderTarget((CCTexture2D)null);

            return renderTarget;
        }

        Texture2D ConvertToPremultiplied(Texture2D texture, SurfaceFormat format)
        {
            //Jake Poznanski - Speeding up XNA Content Load
            //http://jakepoz.com/jake_poznanski__speeding_up_xna.html

            //Setup a render target to hold our final texture which will have premulitplied alpha values
            var result = new RenderTarget2D(
                CCDrawManager.SharedDrawManager.XnaGraphicsDevice,
                texture.Width, texture.Height, hasMipmaps, format,
                DepthFormat.None, 0, RenderTargetUsage.DiscardContents
            );

            CCDrawManager.SharedDrawManager.CurrentRenderTarget = result;

            CCDrawManager.SharedDrawManager.Clear(CCColor4B.Transparent);

            var spriteBatch = CCDrawManager.SharedDrawManager.SpriteBatch;

            if (format != SurfaceFormat.Alpha8)
            {
                //Multiply each color by the source alpha, and write in just the color values into the final texture
                var blendColor = new BlendState();
                blendColor.ColorWriteChannels = ColorWriteChannels.Red | ColorWriteChannels.Green |
                    ColorWriteChannels.Blue;

                blendColor.AlphaDestinationBlend = Blend.Zero;
                blendColor.ColorDestinationBlend = Blend.Zero;

                blendColor.AlphaSourceBlend = Blend.SourceAlpha;
                blendColor.ColorSourceBlend = Blend.SourceAlpha;

                spriteBatch.Begin(SpriteSortMode.Immediate, blendColor);
                spriteBatch.Draw(texture, texture.Bounds, Color.White);
                spriteBatch.End();
            }

            //Now copy over the alpha values from the PNG source texture to the final one, without multiplying them
            var blendAlpha = new BlendState();

            blendAlpha.ColorWriteChannels = ColorWriteChannels.Alpha;

            blendAlpha.AlphaDestinationBlend = Blend.Zero;
            blendAlpha.ColorDestinationBlend = Blend.Zero;

            blendAlpha.AlphaSourceBlend = Blend.One;
            blendAlpha.ColorSourceBlend = Blend.One;

            spriteBatch.Begin(SpriteSortMode.Immediate, blendAlpha);
            spriteBatch.Draw(texture, texture.Bounds, Color.White);
            spriteBatch.End();

            //Release the GPU back to drawing to the screen
            CCDrawManager.SharedDrawManager.SetRenderTarget((CCTexture2D) null);

            return result;
        }

        #endregion Conversion


        #region Loading Texture

        public static CCImageFormat DetectImageFormat(Stream stream)
        {
            var data = new byte[8];

            var pos = stream.Position;
            var dataLen = stream.Read(data, 0, 8);
            stream.Position = pos;

            if (dataLen >= 8)
            {
                if (data[0] == 0x89 && data[1] == 0x50 && data[2] == 0x4E && data[3] == 0x47
                    && data[4] == 0x0D && data[5] == 0x0A && data[6] == 0x1A && data[7] == 0x0A)
                {
                    return CCImageFormat.Png;
                }
            }

            if (dataLen >= 3)
            {
                if (data[0] == 0x47 && data[1] == 0x49 && data[1] == 0x46)
                {
                    return CCImageFormat.Gif;
                }
            }

            if (dataLen >= 2)
            {
                if ((data[0] == 0x49 && data[1] == 0x49) || (data[0] == 0x4d && data[1] == 0x4d))
                {
                    return CCImageFormat.Tiff;
                }
            }

            if (dataLen >= 2)
            {
                if (data[0] == 0xff && data[1] == 0xd8)
                {
                    return CCImageFormat.Jpg;
                }
            }

            return CCImageFormat.UnKnown;
        }

        Texture2D LoadTexture(Stream stream)
        {
            return LoadTexture(stream, CCImageFormat.UnKnown);
        }

        Texture2D LoadRawData<T>(T[] data, int width, int height, SurfaceFormat pixelFormat, bool mipMap) where T : struct
        {
            var result = new Texture2D(CCDrawManager.SharedDrawManager.XnaGraphicsDevice, width, height, mipMap, pixelFormat);
            result.SetData(data);
            return result;
        }

        Texture2D LoadTexture(Stream stream, CCImageFormat imageFormat)
        {
            Texture2D result = null;

            if (imageFormat == CCImageFormat.UnKnown)
            {
                imageFormat = DetectImageFormat(stream);
            }

            if (imageFormat == CCImageFormat.Tiff)
            {
                result = LoadTextureFromTiff(stream);
            }

            if (imageFormat == CCImageFormat.Jpg || imageFormat == CCImageFormat.Png || imageFormat == CCImageFormat.Gif)
            {
                try
                {
                    result = Texture2D.FromStream(CCDrawManager.SharedDrawManager.XnaGraphicsDevice, stream);
                }
                catch (Exception)
                {
                    // Some platforms do not implement FromStream or do not support the format that may be passed.
                    CCLog.Log("CocosSharp: unable to load texture from stream.");
                }
            }

            return result;
        }

        Texture2D LoadTextureFromTiff(Stream stream)
        {
#if (WINDOWS && !NETFX_CORE)
            using (var tiff = Tiff.ClientOpen("file.tif", "r", stream, new TiffStream()))
            {
                var w = tiff.GetField(TiffTag.IMAGEWIDTH)[0].ToInt();
                var h = tiff.GetField(TiffTag.IMAGELENGTH)[0].ToInt();

                var raster = new int[w * h];

                if (tiff.ReadRGBAImageOriented(w, h, raster, Orientation.LEFTTOP))
                {
                    var result = new Texture2D(CCDrawManager.SharedDrawManager.XnaGraphicsDevice, w, h, false, SurfaceFormat.Color);
                    result.SetData(raster);
                    return result;
                }
                else
                {
                    return null;
                }
            }
#elif MACOS || IOS

            return Texture2D.FromStream(CCDrawManager.SharedDrawManager.XnaGraphicsDevice, stream);
#else
            return null;
#endif
        }

        #endregion Loading Texture
    }
}

