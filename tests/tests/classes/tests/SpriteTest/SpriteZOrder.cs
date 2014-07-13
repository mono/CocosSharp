using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;
using Microsoft.Xna.Framework;

namespace tests
{
    public class SpriteZOrder : SpriteTestDemo
    {
        const int numOfSprites = 10;

        int dir;
        CCSprite[] sprites;
        CCSprite sprite1;

        #region Properties

        public override string Title
        {
            get { return "Sprite: Z order"; }
        }

        #endregion Properties


        #region Constructors

        public SpriteZOrder()
        {
            dir = 1;
            sprites = new CCSprite[numOfSprites];

            for (int i = 0; i < 5; i++)
            {
                CCSprite sprite = new CCSprite("Images/grossini_dance_atlas", new CCRect(85 * 0, 121 * 1, 85, 121));
                AddChild(sprite, i);
                sprites[i] = sprite;
            }

            for (int i = 5; i < 10; i++)
            {
                CCSprite sprite = new CCSprite("Images/grossini_dance_atlas", new CCRect(85 * 1, 121 * 0, 85, 121));
                AddChild(sprite, 14 - i);
                sprites[i] = sprite;
            }

            sprite1 = new CCSprite("Images/grossini_dance_atlas", new CCRect(85 * 3, 121 * 0, 85, 121));
            AddChild(sprite1, -1, (int)kTagSprite.kTagSprite1);
        }

        #endregion Constructors


        #region Setup content

        public override void OnEnter()
        {
            base.OnEnter(); CCSize windowSize = Scene.VisibleBoundsWorldspace.Size;

            float step = windowSize.Width / 11;

            for(int i = 0; i < numOfSprites; i++)
            {
                sprites[i].Position = (new CCPoint((i + 1) * step, windowSize.Height / 2));
            }

            sprite1.Position = (new CCPoint(windowSize.Width / 2, windowSize.Height / 2 - 20));
            sprite1.Scale = 6;
            sprite1.Color = CCColor3B.Red;

            Schedule(ReorderSprite, 1);
        }

        #endregion Setup content


        void ReorderSprite(float dt)
        {
            CCSprite sprite = (CCSprite)(GetChildByTag((int)kTagSprite.kTagSprite1));

            int z = sprite.ZOrder;

            if (z < -1)
                dir = 1;
            else if(z > 10)
                dir = -1;

            z += dir * 3;

            ReorderChild(sprite, z);
        }
    }
}
