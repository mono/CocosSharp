using CocosSharp;

namespace tests
{
    public class RenderTextureIssue937 : RenderTextureTestDemo
    {
        CCSprite spritePremulti;
        CCSprite spriteNonpremulti;

        #region Properties

        public override string Title
        {
            get { return "Testing issue #937"; }
        }

        public override string Subtitle
        {
            get { return "All images should be equal..."; }
        }

        #endregion Properties


        #region Constructors

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

            spritePremulti = new CCSprite("Images/fire");
            spriteNonpremulti = new CCSprite("Images/fire");
        }

        #endregion Constructors


        #region Setup content

        protected override void RunningOnNewWindow(CCSize windowSize)
        {
            base.RunningOnNewWindow(windowSize);

            spritePremulti.Position = new CCPoint(16, 48);
            spriteNonpremulti.Position = new CCPoint(16, 16);

            /* A2 & B2 setup */
            CCRenderTexture rend = new CCRenderTexture(32, 64);

            //  It's possible to modify the RenderTexture blending function by
            //  CCBlendFunc bf = new CCBlendFunc (OGLES.GL_ONE, OGLES.GL_ONE_MINUS_SRC_ALPHA);
            //  rend.Sprite.BlendFunc = bf;

            rend.Begin();
            // A2
            spritePremulti.Visit();
            // B2
            spriteNonpremulti.Visit();
            rend.End();

            /* A1: setup */
            spritePremulti.Position = new CCPoint(windowSize.Width / 2 - 16, windowSize.Height / 2 + 16);
            /* B1: setup */
            spriteNonpremulti.Position = new CCPoint(windowSize.Width / 2 - 16, windowSize.Height / 2 - 16);

            rend.Position = new CCPoint(windowSize.Width / 2 + 16, windowSize.Height / 2);

            AddChild(spriteNonpremulti);
            AddChild(spritePremulti);
            AddChild(rend);
        }

        #endregion Setup content
    }
}