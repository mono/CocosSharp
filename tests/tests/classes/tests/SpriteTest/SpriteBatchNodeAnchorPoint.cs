using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
    public class SpriteBatchNodeAnchorPoint : SpriteTestDemo
    {
        const int numOfSprites = 3;

        CCSprite[] pointSprites;
        CCSprite[] sprites;

        CCRepeatForever action;


        #region Properties

        public override string Title
        {
            get { return "SpriteBatchNode: anchor point"; }
        }

        #endregion Properties


        #region Constructors

        public SpriteBatchNodeAnchorPoint()
        {
            sprites = new CCSprite[numOfSprites];
            pointSprites = new CCSprite[numOfSprites];

            CCRotateBy rotate = new CCRotateBy(10, 360);
            action = new CCRepeatForever(rotate);

            // Small capacity. Testing resizing.
            // Don't use capacity=1 in your real game. It is expensive to resize the capacity
            CCSpriteBatchNode batch = new CCSpriteBatchNode("Images/grossini_dance_atlas", 1);
            AddChild(batch, 0, (int)kTags.kTagSpriteBatchNode);

            for (int i = 0; i < numOfSprites; i++)
            {
                CCSprite sprite = new CCSprite(batch.Texture, new CCRect(85, 121 * 1, 85, 121));
                batch.AddChild(sprite, i);

                CCSprite point = new CCSprite("Images/r1");
                AddChild(point, 10);

                switch(i)
                {
                    case 0:
                        sprite.AnchorPoint = (new CCPoint(0, 0));
                        break;
                    case 1:
                        sprite.AnchorPoint = (new CCPoint(0.5f, 0.5f));
                        break;
                    case 2:
                        sprite.AnchorPoint = (new CCPoint(1, 1));
                        break;
                }

                point.Position = sprite.Position;

                sprites[i] = sprite;
                pointSprites[i] = point;
            }
        }

        #endregion Constructors


        #region Setup content

        public override void OnEnter()
        {
            base.OnEnter(); CCSize windowSize = Scene.VisibleBoundsWorldspace.Size;

            for (int i = 0; i < numOfSprites; i++) 
            {
                sprites[i].Position = (new CCPoint(windowSize.Width / 4 * (i + 1), windowSize.Height / 2));
                pointSprites[i].Scale = 0.25f;
                pointSprites[i].Position = (sprites[i].Position);

                sprites[i].RunAction(action);
            }
        }

        #endregion Setup content
    }
}
