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

        bool shouldClear;
        CCColor4B clearColor;
                
        RenderTarget2D renderTarget2D;
        CCDrawManager drawManager;
        CCRenderer renderer;

        Matrix renderViewMatrix;
        Matrix renderProjMatrix;
        Viewport renderViewport;


        #region Properties

        public CCSprite Sprite { get; private set; }
        public CCTexture2D Texture { get; private set; }
        protected CCSurfaceFormat PixelFormat { get; private set; }

        CCColor4B ClearColor
        {
            get { return clearColor; }
            set
            {
                clearColor = value;
                shouldClear = true;
            }
        }

        #endregion Properties



        #region Constructors

        public CCRenderTexture()
        {
            PixelFormat = CCSurfaceFormat.Color;
            drawManager = CCDrawManager.SharedDrawManager;
            renderer = drawManager.Renderer;
        }

        public CCRenderTexture(CCSize contentSize, CCSize textureSizeInPixels, 
            CCSurfaceFormat colorFormat=CCSurfaceFormat.Color, 
            CCDepthFormat depthFormat=CCDepthFormat.None, 
            CCRenderTargetUsage usage=CCRenderTargetUsage.DiscardContents) : this()
        {
            int textureWidth = (int)textureSizeInPixels.Width;
            int textureHeight = (int)textureSizeInPixels.Height;

            renderTarget2D = drawManager.CreateRenderTarget(
                textureWidth, textureHeight, colorFormat, depthFormat, usage);

            Texture = new CCTexture2D(renderTarget2D, colorFormat, true, false);
            Texture.IsAntialiased = false;

            Sprite = new CCSprite(Texture);
            Sprite.ContentSize = contentSize;
            Sprite.BlendFunc = CCBlendFunc.AlphaBlend;

            CCPoint center = contentSize.Center;

            renderViewMatrix = 
                Matrix.CreateLookAt(new CCPoint3(center, 300.0f).XnaVector, new CCPoint3(center, 0.0f).XnaVector, Vector3.Up);
            renderProjMatrix = 
                Matrix.CreateOrthographic(contentSize.Width, contentSize.Height, 1024f, -1024);
            renderViewport = new Viewport(0, 0, textureWidth, textureHeight);


            clearColor = CCColor4B.Transparent;
            drawManager.SetRenderTarget(Texture);
            drawManager.Clear(clearColor);
            drawManager.RestoreRenderTarget();
        }

        #endregion Constructors


        public void Begin()
        {
            renderer.PushGroup();
            renderer.PushViewportGroup(ref renderViewport);
            renderer.PushLayerGroup(ref renderViewMatrix, ref renderProjMatrix);
            var beginCommand = new CCCustomCommand(float.MinValue);
            beginCommand.Action = OnBegin;
            drawManager.Renderer.AddCommand(beginCommand);

        }

        void OnBegin()
        {
            CCDrawManager drawManager = CCDrawManager.SharedDrawManager;

            drawManager.SetRenderTarget(Texture);

            if(shouldClear)
            {
                drawManager.Clear(clearColor);
                shouldClear = false;
            }
        }

        public void BeginWithClear(byte r, byte g, byte b, byte a, float depth = 1.0f, int stencil = 0)
        {
            BeginWithClear(new CCColor4B(r, g, b, a), depth, stencil);
        }

        public void BeginWithClear(CCColor4B clearColor, float depth = 1.0f, int stencil = 0)
		{
            ClearColor = clearColor;
            Begin();
		}

        public void End()
        {
            var endCommand = new CCCustomCommand(float.MaxValue);
            endCommand.Action = OnEnd;

            renderer.AddCommand(endCommand);
            renderer.PopLayerGroup();
            renderer.PopViewportGroup();
            renderer.PopGroup();
        }

        void OnEnd()
        {
            drawManager.RestoreRenderTarget();
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