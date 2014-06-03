using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CocosSharp
{
    public class CCGrabber
    {
        RenderTarget2D oldRenderTarget;

        public void Grab(CCTexture2D texture)
        {
            CCDrawManager.CreateRenderTarget(texture, CCRenderTargetUsage.DiscardContents);
        }

        public void BeforeRender(CCTexture2D texture)
        {
            oldRenderTarget = CCDrawManager.GetRenderTarget();
            CCDrawManager.SetRenderTarget(texture);

            CCDrawManager.Clear(CCColor4B.Transparent);
        }

        public void AfterRender(CCTexture2D texture)
        {
            CCDrawManager.SetRenderTarget(oldRenderTarget);
        }
    }
}