using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class SpriteBatchNodeReorderIssue744 : SpriteTestDemo
    {
        public SpriteBatchNodeReorderIssue744()
        {
            CCSize s = CCDirector.SharedDirector.WinSize;

            // Testing issue #744
            // http://code.google.com/p/cocos2d-iphone/issues/detail?id=744
            CCSpriteBatchNode batch = new CCSpriteBatchNode("Images/grossini_dance_atlas", 15);
            AddChild(batch, 0, (int)kTags.kTagSpriteBatchNode);

            CCSprite sprite = new CCSprite(batch.Texture, new CCRect(0, 0, 85, 121));
            sprite.Position = (new CCPoint(s.Width / 2, s.Height / 2));
            batch.AddChild(sprite, 3);
            batch.ReorderChild(sprite, 1);
        }

        public override string title()
        {
            return "SpriteBatchNode: reorder issue #744";
        }

        public override string subtitle()
        {
            return "Should not crash";
        }
    }
}
