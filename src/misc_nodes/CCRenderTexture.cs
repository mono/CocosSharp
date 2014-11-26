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

    public partial class CCRenderTexture : CCNode
    {
        bool firstUsage = true;
        RenderTarget2D renderTarget2D;

        #region Properties

        public CCSprite Sprite { get; set; }
        public CCTexture2D Texture { get; private set; }
        protected CCSurfaceFormat PixelFormat { get; private set; }

        #endregion Properties


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
            renderTarget2D = CCDrawManager.SharedDrawManager.CreateRenderTarget(textureWidth, textureHeight, colorFormat, depthFormat, usage);

            Texture = new CCTexture2D(renderTarget2D, colorFormat, true, false);
            Texture.IsAntialiased = false;

            Sprite = new CCSprite(Texture);
            Sprite.ContentSize = contentSize;
            Sprite.BlendFunc = CCBlendFunc.AlphaBlend;

            AddChild(Sprite);
        }

        #endregion Constructors


        public virtual void Begin()
        {
            CCDrawManager drawManager = CCDrawManager.SharedDrawManager;

            // Save the current matrix
            drawManager.PushMatrix();

//            Matrix projection = Matrix.CreateOrthographicOffCenter(
//                -1.0f / widthRatio, 1.0f / widthRatio,
//                -1.0f / heightRatio, 1.0f / heightRatio,
//                -1, 1
//            );
//
//            drawManager.MultMatrix(ref projection);

            drawManager.SetRenderTarget(Texture);

            if (firstUsage)
            {
                drawManager.Clear(CCColor4B.Transparent);
                firstUsage = false;
            }
        }

        public void BeginWithClear(float r, float g, float b, float a)
        {
            Begin();
            CCDrawManager.SharedDrawManager.Clear(new CCColor4B(r, g, b, a));
        }

        public void BeginWithClear(float r, float g, float b, float a, float depthValue)
        {
            Begin();
            CCDrawManager.SharedDrawManager.Clear(new CCColor4B(r, g, b, a), depthValue);
        }

		public void BeginWithClear(float r, float g, float b, float a, float depthValue, int stencilValue)
		{
			Begin();
			CCDrawManager.SharedDrawManager.Clear(new CCColor4B(r, g, b, a), depthValue, stencilValue);
		}

		public void BeginWithClear(CCColor4B col)
		{
			Begin();
			CCDrawManager.SharedDrawManager.Clear(col);
		}

		public void BeginWithClear(CCColor4B col, float depthValue)
		{
			Begin();
			CCDrawManager.SharedDrawManager.Clear(col, depthValue);
		}

		public void BeginWithClear(CCColor4B col, float depthValue, int stencilValue)
		{
			Begin();
			CCDrawManager.SharedDrawManager.Clear(col, depthValue, stencilValue);
		}

        public void ClearDepth(float depthValue)
        {
            Begin();
            CCDrawManager.SharedDrawManager.Clear(ClearOptions.DepthBuffer, Microsoft.Xna.Framework.Color.White, depthValue, 0);
            End();
        }

        public void ClearStencil(int stencilValue)
        {
            Begin();
            CCDrawManager.SharedDrawManager.Clear(ClearOptions.Stencil, Microsoft.Xna.Framework.Color.White, 1, stencilValue);
            End();
        }

        public virtual void End()
        {
            CCDrawManager.SharedDrawManager.PopMatrix();

            CCDrawManager.SharedDrawManager.SetRenderTarget((CCTexture2D) null);
        }

		public void Clear(float r, float g, float b, float a)
		{
			BeginWithClear(r, g, b, a);
			End();
		}

		public void Clear(CCColor4B col)
		{
			BeginWithClear(col);
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