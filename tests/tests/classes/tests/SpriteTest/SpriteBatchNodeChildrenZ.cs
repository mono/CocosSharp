using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
    public class SpriteBatchNodeChildrenZ : SpriteTestDemo
    {
        public SpriteBatchNodeChildrenZ()
        {
            CCSize s = CCDirector.SharedDirector.WinSize;

            // parents
            CCSpriteBatchNode batch;
            CCSprite sprite1, sprite2, sprite3;


            CCSpriteFrameCache.Instance.AddSpriteFrames("animations/grossini.plist");

            // test 1
            batch = new CCSpriteBatchNode("animations/grossini", 50);
            AddChild(batch, 0, (int)kTags.kTagSpriteBatchNode);

            sprite1 = new CCSprite("grossini_dance_01.png");
            sprite1.Position = (new CCPoint(s.Width / 3, s.Height / 2));

            sprite2 = new CCSprite("grossini_dance_02.png");
            sprite2.Position = (new CCPoint(20, 30));

            sprite3 = new CCSprite("grossini_dance_03.png");
            sprite3.Position = (new CCPoint(-20, 30));

            batch.AddChild(sprite1);
            sprite1.AddChild(sprite2, 2);
            sprite1.AddChild(sprite3, -2);

            // test 2
            batch = new CCSpriteBatchNode("animations/grossini", 50);
            AddChild(batch, 0, (int)kTags.kTagSpriteBatchNode);

            sprite1 = new CCSprite("grossini_dance_01.png");
            sprite1.Position = (new CCPoint(2 * s.Width / 3, s.Height / 2));

            sprite2 = new CCSprite("grossini_dance_02.png");
            sprite2.Position = (new CCPoint(20, 30));

            sprite3 = new CCSprite("grossini_dance_03.png");
            sprite3.Position = (new CCPoint(-20, 30));

            batch.AddChild(sprite1);
            sprite1.AddChild(sprite2, -2);
            sprite1.AddChild(sprite3, 2);

            // test 3
            batch = new CCSpriteBatchNode("animations/grossini", 50);
            AddChild(batch, 0, (int)kTags.kTagSpriteBatchNode);

            sprite1 = new CCSprite("grossini_dance_01.png");
            sprite1.Position = (new CCPoint(s.Width / 2 - 90, s.Height / 4));

            sprite2 = new CCSprite("grossini_dance_02.png");
            sprite2.Position = (new CCPoint(s.Width / 2 - 60, s.Height / 4));

            sprite3 = new CCSprite("grossini_dance_03.png");
            sprite3.Position = (new CCPoint(s.Width / 2 - 30, s.Height / 4));

            batch.AddChild(sprite1, 10);
            batch.AddChild(sprite2, -10);
            batch.AddChild(sprite3, -5);

            // test 4
            batch = new CCSpriteBatchNode("animations/grossini", 50);
            AddChild(batch, 0, (int)kTags.kTagSpriteBatchNode);

            sprite1 = new CCSprite("grossini_dance_01.png");
            sprite1.Position = (new CCPoint(s.Width / 2 + 30, s.Height / 4));

            sprite2 = new CCSprite("grossini_dance_02.png");
            sprite2.Position = (new CCPoint(s.Width / 2 + 60, s.Height / 4));

            sprite3 = new CCSprite("grossini_dance_03.png");
            sprite3.Position = (new CCPoint(s.Width / 2 + 90, s.Height / 4));

            batch.AddChild(sprite1, -10);
            batch.AddChild(sprite2, -5);
            batch.AddChild(sprite3, -2);
        }

        public override void OnExit()
        {
            base.OnExit();
            CCSpriteFrameCache.Instance.RemoveUnusedSpriteFrames();
        }

        public override string title()
        {
            return "Sprite/BatchNode + child + scale + rot";
        }
    }
}
