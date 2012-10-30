using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace cocos2d
{
    public class CCGrabber : CCObject
    {
        private RenderTarget2D m_pOldRenderTarget;

        public void Grab(CCTexture2D pTexture)
        {
            DrawManager.CreateRenderTarget(pTexture, RenderTargetUsage.DiscardContents);
        }

        public void BeforeRender(CCTexture2D pTexture)
        {
            m_pOldRenderTarget = DrawManager.GetRenderTarget();
            DrawManager.SetRenderTarget(pTexture);
            DrawManager.Clear(Color.Transparent);
        }

        public void AfterRender(CCTexture2D pTexture)
        {
            DrawManager.SetRenderTarget(m_pOldRenderTarget);
        }
    }
}