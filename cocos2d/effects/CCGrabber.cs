using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Cocos2D
{
    public class CCGrabber
    {
        private RenderTarget2D m_pOldRenderTarget;

        public void Grab(CCTexture2D pTexture)
        {
            CCDrawManager.CreateRenderTarget(pTexture, RenderTargetUsage.DiscardContents);
        }

        public void BeforeRender(CCTexture2D pTexture)
        {
            m_pOldRenderTarget = CCDrawManager.GetRenderTarget();
            CCDrawManager.SetRenderTarget(pTexture);
            CCDrawManager.Clear(Color.Transparent);
        }

        public void AfterRender(CCTexture2D pTexture)
        {
            CCDrawManager.SetRenderTarget(m_pOldRenderTarget);
        }
    }
}