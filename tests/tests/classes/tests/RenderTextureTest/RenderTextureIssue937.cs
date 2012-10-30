using cocos2d;

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
            CCLayerColor background = CCLayerColor.Create(new ccColor4B(200, 200, 200, 255));
            AddChild(background);

            CCSprite spr_premulti = CCSprite.Create("Images/fire");
            spr_premulti.Position = new CCPoint(16, 48);

            CCSprite spr_nonpremulti = CCSprite.Create("Images/fire");
            spr_nonpremulti.Position = new CCPoint(16, 16);


            /* A2 & B2 setup */
            CCRenderTexture rend = CCRenderTexture.Create(32, 64);

            // It's possible to modify the RenderTexture blending function by
            //		[[rend sprite] setBlendFunc:(ccBlendFunc) {GL_ONE, GL_ONE_MINUS_SRC_ALPHA}];
            rend.Begin();
            spr_premulti.Visit();
            spr_nonpremulti.Visit();
            rend.End();

            CCSize s = CCDirector.SharedDirector.WinSize;

            /* A1: setup */
            spr_premulti.Position = new CCPoint(s.width / 2 - 16, s.height / 2 + 16);
            /* B1: setup */
            spr_nonpremulti.Position = new CCPoint(s.width / 2 - 16, s.height / 2 - 16);

            rend.Position = new CCPoint(s.width / 2 + 16, s.height / 2);

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