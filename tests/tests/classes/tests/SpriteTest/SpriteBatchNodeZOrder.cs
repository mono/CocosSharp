using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;
using Microsoft.Xna.Framework;

namespace tests
{
    public class SpriteBatchNodeZOrder : SpriteTestDemo
    {
        const int numOfSprites = 10;

        int dir;
        CCSprite sprite1;
        CCSprite[] sprites;


        #region Properties

        public override string Title
        {
            get { return "SpriteBatchNode: Z order"; }
        }

        #endregion Properties


        #region Constructors

        public SpriteBatchNodeZOrder()
        {
            dir = 1;

            // small capacity. Testing resizing.
            // Don't use capacity=1 in your real game. It is expensive to resize the capacity
            CCSpriteBatchNode batch = new CCSpriteBatchNode("Images/grossini_dance_atlas", 1);
            AddChild(batch, 0, (int)kTags.kTagSpriteBatchNode);

            sprites = new CCSprite[numOfSprites];

            for (int i = 0; i < 5; i++)
            {
                sprites[i] = new CCSprite(batch.Texture, new CCRect(85 * 0, 121 * 1, 85, 121));
                batch.AddChild(sprites[i], i);
            }

            for (int i = 5; i < 10; i++)
            {
                sprites[i] = new CCSprite(batch.Texture, new CCRect(85 * 1, 121 * 0, 85, 121));
                batch.AddChild(sprites[i], 14 - i);
            }

            sprite1 = new CCSprite(batch.Texture, new CCRect(85 * 3, 121 * 0, 85, 121));
            batch.AddChild(sprite1, -1, (int)kTagSprite.kTagSprite1);
            sprite1.Scale = 6;
            sprite1.Color = CCColor3B.Red;

        }

        #endregion Constructors


        #region Setup content

        protected override void RunningOnNewWindow(CCSize windowSize)
        {
            base.RunningOnNewWindow (windowSize);

            float step = windowSize.Width / 11;

            for (int i = 0; i < numOfSprites; i++) 
            {
                sprites[i].Position = (new CCPoint((i + 1) * step, windowSize.Height / 2));
            }

            sprite1.Position = (new CCPoint(windowSize.Width / 2, windowSize.Height / 2 - 20));

            Schedule(ReorderSprite, 1);
        }

        #endregion Setup content


        void ReorderSprite(float dt)
        {
            CCSpriteBatchNode batch = (CCSpriteBatchNode)(GetChildByTag((int)kTags.kTagSpriteBatchNode));
            CCSprite sprite = (CCSprite)(batch.GetChildByTag((int)kTagSprite.kTagSprite1));

            int z = sprite.ZOrder;

            if (z < -1)
                dir = 1;
            else if (z > 10)
                dir = -1;

            z += dir * 3;

            batch.ReorderChild(sprite, z);
        }
    }
}
