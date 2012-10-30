using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

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


            CCSpriteFrameCache.SharedSpriteFrameCache.AddSpriteFramesWithFile("animations/grossini.plist");

            // test 1
            batch = CCSpriteBatchNode.Create("animations/grossini", 50);
            AddChild(batch, 0, (int)kTags.kTagSpriteBatchNode);

            sprite1 = CCSprite.Create("grossini_dance_01.png");
            sprite1.Position = (new CCPoint(s.width / 3, s.height / 2));

            sprite2 = CCSprite.Create("grossini_dance_02.png");
            sprite2.Position = (new CCPoint(20, 30));

            sprite3 = CCSprite.Create("grossini_dance_03.png");
            sprite3.Position = (new CCPoint(-20, 30));

            batch.AddChild(sprite1);
            sprite1.AddChild(sprite2, 2);
            sprite1.AddChild(sprite3, -2);

            // test 2
            batch = CCSpriteBatchNode.Create("animations/grossini", 50);
            AddChild(batch, 0, (int)kTags.kTagSpriteBatchNode);

            sprite1 = CCSprite.Create("grossini_dance_01.png");
            sprite1.Position = (new CCPoint(2 * s.width / 3, s.height / 2));

            sprite2 = CCSprite.Create("grossini_dance_02.png");
            sprite2.Position = (new CCPoint(20, 30));

            sprite3 = CCSprite.Create("grossini_dance_03.png");
            sprite3.Position = (new CCPoint(-20, 30));

            batch.AddChild(sprite1);
            sprite1.AddChild(sprite2, -2);
            sprite1.AddChild(sprite3, 2);

            // test 3
            batch = CCSpriteBatchNode.Create("animations/grossini", 50);
            AddChild(batch, 0, (int)kTags.kTagSpriteBatchNode);

            sprite1 = CCSprite.Create("grossini_dance_01.png");
            sprite1.Position = (new CCPoint(s.width / 2 - 90, s.height / 4));

            sprite2 = CCSprite.Create("grossini_dance_02.png");
            sprite2.Position = (new CCPoint(s.width / 2 - 60, s.height / 4));

            sprite3 = CCSprite.Create("grossini_dance_03.png");
            sprite3.Position = (new CCPoint(s.width / 2 - 30, s.height / 4));

            batch.AddChild(sprite1, 10);
            batch.AddChild(sprite2, -10);
            batch.AddChild(sprite3, -5);

            // test 4
            batch = CCSpriteBatchNode.Create("animations/grossini", 50);
            AddChild(batch, 0, (int)kTags.kTagSpriteBatchNode);

            sprite1 = CCSprite.Create("grossini_dance_01.png");
            sprite1.Position = (new CCPoint(s.width / 2 + 30, s.height / 4));

            sprite2 = CCSprite.Create("grossini_dance_02.png");
            sprite2.Position = (new CCPoint(s.width / 2 + 60, s.height / 4));

            sprite3 = CCSprite.Create("grossini_dance_03.png");
            sprite3.Position = (new CCPoint(s.width / 2 + 90, s.height / 4));

            batch.AddChild(sprite1, -10);
            batch.AddChild(sprite2, -5);
            batch.AddChild(sprite3, -2);
        }

        public override void OnExit()
        {
            base.OnExit();
            CCSpriteFrameCache.SharedSpriteFrameCache.RemoveUnusedSpriteFrames();
        }

        public override string title()
        {
            return "Sprite/BatchNode + child + scale + rot";
        }
    }
}
