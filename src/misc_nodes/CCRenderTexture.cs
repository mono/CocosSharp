using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CocosSharp
{

    public enum CCRenderTargetUsage
    {
        DiscardContents,
        PreserveContents,
        PlatformContents
    }

    public class CCRenderTexture
    {

        [Flags]
        enum ClearFlags
        {
            None = 0x0,
            ColorBuffer = 0x1,
            DepthBuffer = 0x2,
            StencilBuffer = 0x4,
            All = ~0x0
        }


        bool firstUsage = true;
        RenderTarget2D renderTarget2D;


        #region Properties

        public bool AutoDraw { get; set; }
        public CCSprite Sprite { get; set; }
        public CCTexture2D Texture { get; private set; }
        protected CCSurfaceFormat PixelFormat { get; private set; }

        #endregion Properties

        CCColor4B beginClearColor = CCColor4B.Transparent;
        float beginDepthValue = 0;
        int beginStencilValue = 0;

        ClearFlags clearFlags = ClearFlags.None;

        #region Constructors

        public CCRenderTexture()
        {
            PixelFormat = CCSurfaceFormat.Color;
        }

        public CCRenderTexture(CCSize contentSize, CCSize textureSizeInPixels, 
            CCSurfaceFormat colorFormat=CCSurfaceFormat.Color, 
            CCDepthFormat depthFormat=CCDepthFormat.None, 
            CCRenderTargetUsage usage=CCRenderTargetUsage.DiscardContents)
        {
            int textureWidth = (int)textureSizeInPixels.Width;
            int textureHeight = (int)textureSizeInPixels.Height;

            firstUsage = true;
            renderTarget2D = CCDrawManager.SharedDrawManager.CreateRenderTarget(
                textureWidth, textureHeight, colorFormat, depthFormat, usage);

            Texture = new CCTexture2D(renderTarget2D, colorFormat, true, false);
            Texture.IsAntialiased = false;

            Sprite = new CCSprite(Texture);
            Sprite.ContentSize = contentSize;
            Sprite.BlendFunc = CCBlendFunc.AlphaBlend;
        }

        #endregion Constructors


        public void Begin()
        {
            CCDrawManager drawManager = CCDrawManager.SharedDrawManager;

            drawManager.Renderer.PushGroup();

            var beginCommand = new CCCustomCommand(long.MinValue);
            beginCommand.Action = OnBegin;
            drawManager.Renderer.AddCommand(beginCommand);

        }

        void OnBegin()
        {
            CCDrawManager drawManager = CCDrawManager.SharedDrawManager;

            drawManager.SetRenderTarget(Texture);

            if (firstUsage)
            {
                drawManager.Clear(beginClearColor);
                firstUsage = false;
            }
        }

        public void BeginWithClear(byte r, byte g, byte b, byte a, float depthValue = 1.0f, int stencilValue = 0)
        {
            BeginWithClear(new CCColor4B(r, g, b, a), depthValue, stencilValue);
        }

        public void BeginWithClear(CCColor4B beginClearColor, float depthValue = 1.0f, int stencilValue = 0)
		{
            Begin();
            beginDepthValue = depthValue;
            beginStencilValue = stencilValue;
            clearFlags = ClearFlags.All;

            CCDrawManager drawManager = CCDrawManager.SharedDrawManager;

            var beginWithClearCommand = new CCCustomCommand(long.MinValue);
            beginWithClearCommand.Action = () => 
                {
                    drawManager.Clear(beginClearColor, depthValue, stencilValue);
                };
            drawManager.Renderer.AddCommand(beginWithClearCommand);
		}

        public virtual void End()
        {
            var endCommand = new CCCustomCommand(float.MaxValue);
            endCommand.Action = OnEnd;

            CCDrawManager.SharedDrawManager.Renderer.AddCommand(endCommand);
            CCDrawManager.SharedDrawManager.Renderer.PopGroup();
        }

        void OnEnd ()
        {
            CCDrawManager.SharedDrawManager.RestoreRenderTarget();
        }

        public bool SaveToStream(Stream stream, CCImageFormat format)
        {
            if (format == CCImageFormat.Png)
            {
                Texture.SaveAsPng(stream, Texture.PixelsWide, Texture.PixelsHigh);
            }
            else if (format == CCImageFormat.Jpg)
            {
                Texture.SaveAsJpeg(stream, Texture.PixelsWide, Texture.PixelsHigh);
            }
            else
            {
                return false;
            }
            
            return true;
        }
    }
}