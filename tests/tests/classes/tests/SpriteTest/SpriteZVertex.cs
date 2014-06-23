using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
    public class SpriteZVertex : SpriteTestDemo
    {
        const int numOfSprites = 16;

        int dir;
        float time;

        CCNode node;
        CCSprite[] sprites;

        #region Properties

        public override string Title
        {
            get { return "Sprite: openGL Z vertex"; }
        }

        #endregion Properties


        #region Constructors

        public SpriteZVertex()
        {
            //
            // This test tests z-order
            // If you are going to use it is better to use a 3D projection
            //
            // WARNING:
            // The developer is resposible for ordering it's sprites according to it's Z if the sprite has
            // transparent parts.
            //

            dir = 1;
            time = 0;
            sprites = new CCSprite[numOfSprites];

            node = new CCNode();
            AddChild(node, 0);

            for (int i = 0; i < 5; i++)
            {
                CCSprite sprite = new CCSprite("Images/grossini_dance_atlas", new CCRect(85 * 0, 121 * 1, 85, 121));
                node.AddChild(sprite, 0);
                sprites[i] = sprite;
            }

            for (int i = 5; i < 11; i++)
            {
                CCSprite sprite = new CCSprite("Images/grossini_dance_atlas", new CCRect(85 * 1, 121 * 0, 85, 121));
                node.AddChild(sprite, 0);
                sprites[i] = sprite;
            }
        }

        #endregion Constructors


        #region Setup content

        protected override void RunningOnNewWindow(CCSize windowSize)
        {
            base.RunningOnNewWindow (windowSize);

            // camera uses the center of the image as the pivoting point
            node.ContentSize = (new CCSize(windowSize.Width, windowSize.Height));
            node.AnchorPoint = (new CCPoint(0.5f, 0.5f));
            node.Position = (new CCPoint(windowSize.Width / 2, windowSize.Height / 2));

            float step = windowSize.Width / 12;

            for (int i = 0; i < 5; i++)
            {
                sprites[i].Position = (new CCPoint((i + 1) * step, windowSize.Height / 2));
                sprites[i].VertexZ = (10 + i * 40);
            }

            for (int i = 5; i < 11; i++)
            {
                sprites[i].Position = (new CCPoint((i + 1) * step, windowSize.Height / 2));
                sprites[i].VertexZ = 10 + (10 - i) * 40;
            }

            node.RunAction(new CCOrbitCamera(10, 1, 0, 0, 360, 0, 0));
        }

        #endregion Setup content

        public override void OnEnter()
        {
            base.OnEnter();

            // TIP: don't forget to enable Alpha test
            //glEnable(GL_ALPHA_TEST);
            //glAlphaFunc(GL_GREATER, 0.0f);
            Director.Projection = (CCDirectorProjection.Projection3D);
        }
        public override void OnExit()
        {
            Director.Projection = (CCDirectorProjection.Projection2D);
            base.OnExit();
        }

    }
}
