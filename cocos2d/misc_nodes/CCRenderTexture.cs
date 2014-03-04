using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CocosSharp
{
    public partial class CCRenderTexture : CCNode
    {
        bool firstUsage = true;
        RenderTarget2D renderTarget2D;


        public CCSprite Sprite { get; set; }
        protected CCTexture2D Texture { get; private set; }
        protected SurfaceFormat PixelFormat { get; private set; }


        #region Constructors

        public CCRenderTexture()
        {
            PixelFormat = SurfaceFormat.Color;
        }

        public CCRenderTexture(int w, int h) 
            : this(w, h, SurfaceFormat.Color, DepthFormat.None, RenderTargetUsage.DiscardContents)
        {
        }

        public CCRenderTexture(int w, int h, SurfaceFormat format) 
            : this(w, h, format, DepthFormat.None, RenderTargetUsage.DiscardContents)
        {
        }

        public CCRenderTexture(int w, int h, SurfaceFormat colorFormat, DepthFormat depthFormat, RenderTargetUsage usage)
        {
            w = (int)Math.Ceiling(w * CCMacros.CCContentScaleFactor());
            h = (int)Math.Ceiling(h * CCMacros.CCContentScaleFactor());

            firstUsage = true;
            renderTarget2D = CCDrawManager.CreateRenderTarget(w, h, colorFormat, depthFormat, usage);

            Texture = new CCTexture2D(renderTarget2D, colorFormat, true, false);
            Texture.IsAntialiased = false;

            Sprite = new CCSprite(Texture);
            Sprite.BlendFunc = CCBlendFunc.AlphaBlend;

            AddChild(Sprite);
        }

        #endregion Constructors


        public virtual void Begin()
        {
            // Save the current matrix
            CCDrawManager.PushMatrix();

            CCSize texSize = Texture.ContentSizeInPixels;

            // Calculate the adjustment ratios based on the old and new projections
            CCDirector director = CCDirector.SharedDirector;
            CCSize size = director.WinSizeInPixels;
            float widthRatio = size.Width / texSize.Width;
            float heightRatio = size.Height / texSize.Height;

            CCDrawManager.SetRenderTarget(Texture);

            CCDrawManager.SetViewPort(0, 0, (int) texSize.Width, (int) texSize.Height);

            Matrix projection = Matrix.CreateOrthographicOffCenter(
                -1.0f / widthRatio, 1.0f / widthRatio,
                -1.0f / heightRatio, 1.0f / heightRatio,
                -1, 1
                );

            CCDrawManager.MultMatrix(ref projection);

            if (firstUsage)
            {
                CCDrawManager.Clear(Color.Transparent);
                firstUsage = false;
            }
        }

        public void BeginWithClear(float r, float g, float b, float a)
        {
            Begin();
            CCDrawManager.Clear(new Color(r, g, b, a));
        }

        public void BeginWithClear(float r, float g, float b, float a, float depthValue)
        {
            Begin();
            CCDrawManager.Clear(new Color(r, g, b, a), depthValue);
        }

        public void BeginWithClear(float r, float g, float b, float a, float depthValue, int stencilValue)
        {
            Begin();
            CCDrawManager.Clear(new Color(r, g, b, a), depthValue, stencilValue);
        }

        public void ClearDepth(float depthValue)
        {
            Begin();
            CCDrawManager.Clear(ClearOptions.DepthBuffer, Color.White, depthValue, 0);
            End();
        }

        public void ClearStencil(int stencilValue)
        {
            Begin();
            CCDrawManager.Clear(ClearOptions.Stencil, Color.White, 0, stencilValue);
            End();
        }

        public virtual void End()
        {
            CCDrawManager.PopMatrix();

            CCDirector director = CCDirector.SharedDirector;

            CCDrawManager.SetRenderTarget((CCTexture2D) null);

            director.Projection = director.Projection;
        }

        public void Clear(float r, float g, float b, float a)
        {
            BeginWithClear(r, g, b, a);
            End();
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