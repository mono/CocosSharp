using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cocos2D;

namespace tests
{
    public class SpriteBatchNodeAliased : SpriteTestDemo
    {
        public SpriteBatchNodeAliased()
        {
            CCSpriteBatchNode batch = new CCSpriteBatchNode("Images/grossini_dance_atlas", 10);
            AddChild(batch, 0, (int)kTags.kTagSpriteBatchNode);

            CCSize s = CCDirector.SharedDirector.WinSize;

            CCSprite sprite1 = new CCSprite(batch.Texture, new CCRect(85 * 1, 121 * 1, 85, 121));
            sprite1.Position = (new CCPoint(s.Width / 2 - 100, s.Height / 2));
            batch.AddChild(sprite1, 0, (int)kTagSprite.kTagSprite1);

            CCSprite sprite2 = new CCSprite(batch.Texture, new CCRect(85 * 1, 121 * 1, 85, 121));
            sprite2.Position = (new CCPoint(s.Width / 2 + 100, s.Height / 2));
            batch.AddChild(sprite2, 0, (int)kTagSprite.kTagSprite2);

            CCActionInterval scale = new CCScaleBy(2, 5);
            CCActionInterval scale_back = (CCActionInterval)scale.Reverse();
            CCActionInterval seq = (CCActionInterval)(new CCSequence(scale, scale_back));
            CCAction repeat = new CCRepeatForever (seq);

            CCAction repeat2 = (CCAction)(repeat.Copy());

            sprite1.RunAction(repeat);
            sprite2.RunAction(repeat2);
        }

        public override void OnEnter()
        {
            base.OnEnter();
            CCSpriteBatchNode batch = (CCSpriteBatchNode)GetChildByTag((int)kTags.kTagSpriteBatchNode);
			batch.IsAntialiased = false;
        }

        public override void OnExit()
        {
            // restore the tex parameter to AntiAliased.
            CCSpriteBatchNode batch = (CCSpriteBatchNode)GetChildByTag((int)kTags.kTagSpriteBatchNode);
            batch.IsAntialiased = true;
            base.OnExit();
        }

        public override string title()
        {
            return "SpriteBatchNode Aliased";
        }
    }
}
