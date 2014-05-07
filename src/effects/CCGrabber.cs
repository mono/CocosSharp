using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CocosSharp
{
    public class CCGrabber
    {
		RenderTarget2D oldRenderTarget;

        public void Grab(CCTexture2D pTexture)
        {
			CCDrawManager.CreateRenderTarget(pTexture, CCRenderTargetUsage.DiscardContents);
        }

        public void BeforeRender(CCTexture2D pTexture)
        {
			oldRenderTarget = CCDrawManager.GetRenderTarget();
            CCDrawManager.SetRenderTarget(pTexture);
            CCDrawManager.Clear(CCColor4B.Transparent);
        }

        public void AfterRender(CCTexture2D pTexture)
        {
			CCDrawManager.SetRenderTarget(oldRenderTarget);
        }
    }
}