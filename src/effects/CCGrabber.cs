using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CocosSharp
{
    public class CCGrabber
    {
        CCDrawManager drawManager;

        internal CCGrabber(CCDrawManager drawManager)
        {
            this.drawManager = drawManager;
        }

        public void Grab(CCTexture2D texture)
        {
            drawManager.CreateRenderTarget(texture, CCRenderTargetUsage.DiscardContents);
        }

        public void BeforeRender(CCTexture2D texture)
        {
            drawManager.SetRenderTarget(texture);
            drawManager.Clear(CCColor4B.Transparent);
        }

        public void AfterRender(CCTexture2D texture)
        {
            drawManager.RestoreRenderTarget();
        }
    }
}