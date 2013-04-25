using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace cocos2d
{
    public enum ImageFormat
    {
        JPG = 0,
        PNG = 1
    }

    public class CCRenderTexture : CCNode
    {
        private bool m_bFirstUsage = true;
        protected SurfaceFormat m_ePixelFormat;
        private RenderTarget2D m_pRenderTarget2D;
        protected CCSprite m_pSprite;
        protected CCTexture2D m_pTexture;

        public CCRenderTexture()
        {
            m_ePixelFormat = SurfaceFormat.Color;
        }

        public CCSprite Sprite
        {
            get { return m_pSprite; }
            set { m_pSprite = value; }
        }

        public static CCRenderTexture Create(int w, int h)
        {
            var pRet = new CCRenderTexture();
            pRet.InitWithWidthAndHeight(w, h, SurfaceFormat.Color, DepthFormat.None, RenderTargetUsage.DiscardContents);
            return pRet;
        }

        public static CCRenderTexture Create(int w, int h, SurfaceFormat format)
        {
            var pRet = new CCRenderTexture();
            pRet.InitWithWidthAndHeight(w, h, format, DepthFormat.None, RenderTargetUsage.DiscardContents);
            return pRet;
        }

        public static CCRenderTexture Create(int w, int h, SurfaceFormat format, DepthFormat depthFormat, RenderTargetUsage usage)
        {
            var pRet = new CCRenderTexture();
            pRet.InitWithWidthAndHeight(w, h, format, depthFormat, usage);
            return pRet;
        }

        public bool InitWithWidthAndHeight(int w, int h, SurfaceFormat colorFormat, DepthFormat depthFormat, RenderTargetUsage usage)
        {
            w = (w * CCMacros.CCContentScaleFactor());
            h = (h * CCMacros.CCContentScaleFactor());

            m_pTexture = new CCTexture2D();
            m_pTexture.SetAliasTexParameters();

            m_pRenderTarget2D = DrawManager.CreateRenderTarget(w, h, colorFormat, depthFormat, usage);
            m_pTexture.InitWithTexture(m_pRenderTarget2D);

            m_bFirstUsage = true;

            m_pSprite = new CCSprite(m_pTexture);
            //m_pSprite.scaleY = -1;
            m_pSprite.BlendFunc = new CCBlendFunc(CCMacros.CCDefaultSourceBlending, CCMacros.CCDefaultDestinationBlending); // OGLES.GL_ONE, OGLES.GL_ONE_MINUS_SRC_ALPHA);

            AddChild(m_pSprite);

            return true;
        }

        public void Begin()
        {
            // Save the current matrix
            DrawManager.PushMatrix();

            CCSize texSize = m_pTexture.ContentSizeInPixels;

            // Calculate the adjustment ratios based on the old and new projections
            CCDirector director = CCDirector.SharedDirector;
            CCSize size = director.WinSize;
            float widthRatio = size.Width / texSize.Width;
            float heightRatio = size.Height / texSize.Height;

            DrawManager.SetRenderTarget(m_pTexture);

            DrawManager.SetViewPort(0, 0, (int) texSize.Width, (int) texSize.Height);

            Matrix projection = Matrix.CreateOrthographicOffCenter(
                -1.0f / widthRatio, 1.0f / widthRatio,
                -1.0f / heightRatio, 1.0f / heightRatio,
                -1, 1
                );

            DrawManager.MultMatrix(ref projection);

            if (m_bFirstUsage)
            {
                DrawManager.Clear(Color.Transparent);
                m_bFirstUsage = false;
            }
        }

        public void BeginWithClear(float r, float g, float b, float a)
        {
            Begin();
            DrawManager.Clear(new Color(r, g, b, a));
        }

        public void BeginWithClear(float r, float g, float b, float a, float depthValue)
        {
            Begin();
            DrawManager.Clear(new Color(r, g, b, a), depthValue);
        }

        public void BeginWithClear(float r, float g, float b, float a, float depthValue, int stencilValue)
        {
            Begin();
            DrawManager.Clear(new Color(r, g, b, a), depthValue, stencilValue);
        }

        public void ClearDepth(float depthValue)
        {
            Begin();
            DrawManager.Clear(ClearOptions.DepthBuffer, Color.White, depthValue, 0);
            End();
        }

        public void ClearStencil(int stencilValue)
        {
            Begin();
            DrawManager.Clear(ClearOptions.Stencil, Color.White, 0, stencilValue);
            End();
        }

        public void End()
        {
            DrawManager.PopMatrix();

            CCDirector director = CCDirector.SharedDirector;

            DrawManager.SetRenderTarget((CCTexture2D) null);

            director.Projection = director.Projection;
        }

        public void Clear(float r, float g, float b, float a)
        {
            BeginWithClear(r, g, b, a);
            End();
        }

        public bool SaveToStream(Stream stream, ImageFormat format)
        {
            if (format == ImageFormat.PNG)
            {
                m_pTexture.SaveAsPng(stream, m_pTexture.PixelsWide, m_pTexture.PixelsHigh);
            }
            else
            {
                m_pTexture.SaveAsJpeg(stream, m_pTexture.PixelsWide, m_pTexture.PixelsHigh);
            }
            return true;
        }
    }
}