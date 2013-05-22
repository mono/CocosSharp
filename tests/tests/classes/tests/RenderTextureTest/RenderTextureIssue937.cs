using Cocos2D;

namespace tests
{
    public class RenderTextureIssue937 : RenderTextureTestDemo
    {
        public RenderTextureIssue937()
        {
            /*
            *     1    2
            * A: A1   A2
            *
            * B: B1   B2
            *
            *  A1: premulti sprite
            *  A2: premulti render
            *
            *  B1: non-premulti sprite
            *  B2: non-premulti render
            */
            CCLayerColor background = new CCLayerColor(new CCColor4B(200, 200, 200, 255));
            AddChild(background);

            CCSprite spr_premulti = new CCSprite("Images/fire");
            spr_premulti.Position = new CCPoint(16, 48);

            CCSprite spr_nonpremulti = new CCSprite("Images/fire");
            spr_nonpremulti.Position = new CCPoint(16, 16);


            /* A2 & B2 setup */
            CCRenderTexture rend = new CCRenderTexture(32, 64);

            // It's possible to modify the RenderTexture blending function by
            //CCBlendFunc bf = new CCBlendFunc (OGLES.GL_ONE, OGLES.GL_ONE_MINUS_SRC_ALPHA);
            //rend.Sprite.BlendFunc = bf;

            rend.Begin();
            // A2
            spr_premulti.Visit();
            // B2
            spr_nonpremulti.Visit();
            rend.End();

            CCSize s = CCDirector.SharedDirector.WinSize;

            /* A1: setup */
            spr_premulti.Position = new CCPoint(s.Width / 2 - 16, s.Height / 2 + 16);
            /* B1: setup */
            spr_nonpremulti.Position = new CCPoint(s.Width / 2 - 16, s.Height / 2 - 16);

            rend.Position = new CCPoint(s.Width / 2 + 16, s.Height / 2);

            AddChild(spr_nonpremulti);
            AddChild(spr_premulti);
            AddChild(rend);
        }

        public override string title()
        {
            return "Testing issue #937";
        }

        public override string subtitle()
        {
            return "All images should be equal...";
        }
    }
}