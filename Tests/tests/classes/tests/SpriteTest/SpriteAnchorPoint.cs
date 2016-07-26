using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
    public class SpriteAnchorPoint : SpriteTestDemo
    {
        const int numOfSprites = 3;

        CCSprite[] pointSprites;
        CCSprite[] sprites;

        CCRepeatForever action;


        #region Properties

        public override string Title
        {
            get { return "Testing Sprite"; }
        }

        public override string Subtitle
        {
            get
            {
                return "Anchor Point";
            }
        }
        #endregion Properties


        #region Constructors

        public SpriteAnchorPoint()
        {
            sprites = new CCSprite[numOfSprites];
            pointSprites = new CCSprite[numOfSprites];

            CCRotateBy rotate = new CCRotateBy(10, 360);
            action = new CCRepeatForever(rotate);


            for (int i = 0; i < numOfSprites; i++)
            {
                CCSprite sprite = new CCSprite("Images/grossini_dance_atlas", new CCRect(85, 121 * 1, 85, 121));
                AddChild(sprite, i);

                CCSprite point = new CCSprite("Images/r1");
                AddChild(point, 10);

                switch (i)
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
            base.OnEnter(); CCSize windowSize = Layer.VisibleBoundsWorldspace.Size;

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
