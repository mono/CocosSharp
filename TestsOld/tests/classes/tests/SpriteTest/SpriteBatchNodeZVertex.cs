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
        CCNode node;
        CCLayer contentLayer;

        #region Properties

        public override string Title
        {
            get { return "SpriteBatchNode: Z vertex"; }
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



        }

        #endregion Constructors


        #region Setup content

        protected override void AddedToScene()
        {
            base.AddedToScene();

            contentLayer = new CCLayer();
            GameView.DepthTesting = true;
            node = new CCNode(Layer.VisibleBoundsWorldspace.Size);
            node.AnchorPoint = CCPoint.AnchorMiddle;
            node.IgnoreAnchorPointForPosition = true;

            AddChild(contentLayer);
            contentLayer.AddChild(node);

            CCSize layerSize = contentLayer.VisibleBoundsWorldspace.Size;

            float step = layerSize.Width / 12;

            // small capacity. Testing resizing.
            // Don't use capacity=1 in your real game. It is expensive to resize the capacity
            batch = new CCSpriteBatchNode("Images/grossini_dance_atlas", 1);
            node.AddChild(batch, 0, (int)kTags.kTagSpriteBatchNode);

            sprites = new CCSprite[numOfSprites];

            for (int i = 0; i < 5; i++)
            {
                sprites[i] = new CCSprite(batch.Texture, new CCRect(85 * 0, 121 * 1, 85, 121));
                sprites[i].Position = (new CCPoint((i + 1) * step, layerSize.Height / 2));
                sprites[i].VertexZ = (10 + i * 20);
                batch.AddChild(sprites[i], 0);
            }

            for (int i = 5; i < 11; i++)
            {
                sprites[i] = new CCSprite(batch.Texture, new CCRect(85 * 1, 121 * 0, 85, 121));
                sprites[i].Position = (new CCPoint((i + 1) * step, layerSize.Height / 2));
                sprites[i].VertexZ = 10 + (10 - i) * 20;
                batch.AddChild(sprites[i], 0);
            }

            // camera uses the center of the image as the pivoting point
            node.RunAction(new CCOrbitCamera(10, 1, 0, 0, 360, 0, 0));
        }

        #endregion Setup contetn


    }
}
