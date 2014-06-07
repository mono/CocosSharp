using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;
using System.Diagnostics;

namespace tests
{
    public class SpriteBatchNodeFlip : SpriteTestDemo
    {
        public SpriteBatchNodeFlip()
        {
            CCSpriteBatchNode batch = new CCSpriteBatchNode("Images/grossini_dance_atlas", 10);
            AddChild(batch, 0, (int)kTags.kTagSpriteBatchNode);

            CCSize s = CCApplication.SharedApplication.MainWindowDirector.WindowSizeInPoints;

            CCSprite sprite1 = new CCSprite(batch.Texture, new CCRect(85 * 1, 121 * 1, 85, 121));
            sprite1.Position = (new CCPoint(s.Width / 2 - 100, s.Height / 2));
            batch.AddChild(sprite1, 0, (int)kTagSprite.kTagSprite1);

            CCSprite sprite2 = new CCSprite(batch.Texture, new CCRect(85 * 1, 121 * 1, 85, 121));
            sprite2.Position = new CCPoint(s.Width / 2 + 100, s.Height / 2);
            batch.AddChild(sprite2, 0, (int)kTagSprite.kTagSprite2);

            Schedule(flipSprites, 1);
        }

        public void flipSprites(float dt)
        {
            CCSpriteBatchNode batch = (CCSpriteBatchNode)(GetChildByTag((int)kTags.kTagSpriteBatchNode));
            CCSprite sprite1 = (CCSprite)(batch.GetChildByTag((int)kTagSprite.kTagSprite1));
            CCSprite sprite2 = (CCSprite)(batch.GetChildByTag((int)kTagSprite.kTagSprite2));

            bool x = sprite1.FlipX;
            bool y = sprite2.FlipY;

            CCLog.Log("Pre: {0}", sprite1.ContentSize.Height);
            sprite1.FlipX = !x;
            sprite2.FlipY = !y;
            CCLog.Log("Post: {0}", sprite1.ContentSize.Height);
        }

        public override string title()
        {
            return "SpriteBatchNode Flip X & Y";
        }
    }
}
