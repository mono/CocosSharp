using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
    public class SpriteBatchNodeZVertex : SpriteTestDemo
    {
        const int numOfSprites = 11;

        CCSpriteBatchNode batch;
        CCSprite[] sprites;


        #region Properties

        public override string Title
        {
            get { return "SpriteBatchNode: openGL Z vertex"; }
        }

        #endregion Properties


        #region Constructors

        public SpriteBatchNodeZVertex()
        {
            //
            // This test tests z-order
            // If you are going to use it is better to use a 3D projection
            //
            // WARNING:
            // The developer is resposible for ordering it's sprites according to it's Z if the sprite has
            // transparent parts.
            //

            // small capacity. Testing resizing.
            // Don't use capacity=1 in your real game. It is expensive to resize the capacity
            batch = new CCSpriteBatchNode("Images/grossini_dance_atlas", 1);
            AddChild(batch, 0, (int)kTags.kTagSpriteBatchNode);

            sprites = new CCSprite[numOfSprites];

            for (int i = 0; i < 5; i++)
            {
                sprites[i] = new CCSprite(batch.Texture, new CCRect(85 * 0, 121 * 1, 85, 121));
                sprites[i].VertexZ = (10 + i * 40);
                batch.AddChild(sprites[i], 0);
            }

            for (int i = 5; i < 11; i++)
            {
                sprites[i] = new CCSprite(batch.Texture, new CCRect(85 * 1, 121 * 0, 85, 121));
                sprites[i].VertexZ = 10 + (10 - i) * 40;
                batch.AddChild(sprites[i], 0);
            }

        }

        #endregion Constructors


        #region Setup content

        protected override void RunningOnNewWindow(CCSize windowSize)
        {
            base.RunningOnNewWindow (windowSize);

            float step = windowSize.Width / 12;

            // camera uses the center of the image as the pivoting point
            batch.ContentSize = windowSize;
            batch.AnchorPoint = (new CCPoint(0.5f, 0.5f));
            batch.Position = (new CCPoint(windowSize.Width / 2, windowSize.Height / 2));

            for (int i = 0; i < numOfSprites; i++)
            {
                sprites[i].Position = (new CCPoint((i + 1) * step, windowSize.Height / 2));
            }

            batch.RunAction(new CCOrbitCamera(10, 1, 0, 0, 360, 0, 0));
        }

        #endregion Setup contetn


        public override void OnEnter()
        {
            base.OnEnter();

            Director.Projection = CCDirectorProjection.Projection3D;
        }

        public override void OnExit()
        {
            Director.Projection = (CCDirectorProjection.Projection2D);
            base.OnExit();
        }

    }
}
