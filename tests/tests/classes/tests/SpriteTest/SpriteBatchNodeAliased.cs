using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class SpriteBatchNodeAliased : SpriteTestDemo
    {
        public SpriteBatchNodeAliased()
        {
            CCSpriteBatchNode batch = CCSpriteBatchNode.Create("Images/grossini_dance_atlas", 10);
            AddChild(batch, 0, (int)kTags.kTagSpriteBatchNode);

            CCSize s = CCDirector.SharedDirector.WinSize;

            CCSprite sprite1 = CCSprite.Create(batch.Texture, new CCRect(85 * 1, 121 * 1, 85, 121));
            sprite1.Position = (new CCPoint(s.width / 2 - 100, s.height / 2));
            batch.AddChild(sprite1, 0, (int)kTagSprite.kTagSprite1);

            CCSprite sprite2 = CCSprite.Create(batch.Texture, new CCRect(85 * 1, 121 * 1, 85, 121));
            sprite2.Position = (new CCPoint(s.width / 2 + 100, s.height / 2));
            batch.AddChild(sprite2, 0, (int)kTagSprite.kTagSprite2);

            CCActionInterval scale = CCScaleBy.Create(2, 5);
            CCActionInterval scale_back = (CCActionInterval)scale.Reverse();
            CCActionInterval seq = (CCActionInterval)(CCSequence.Create(scale, scale_back));
            CCAction repeat = CCRepeatForever.Create(seq);

            CCAction repeat2 = (CCAction)(repeat.Copy());

            sprite1.RunAction(repeat);
            sprite2.RunAction(repeat2);
        }

        public override void OnEnter()
        {
            base.OnEnter();
            CCSpriteBatchNode batch = (CCSpriteBatchNode)GetChildByTag((int)kTags.kTagSpriteBatchNode);
            batch.Texture.SetAliasTexParameters();
        }

        public override void OnExit()
        {
            // restore the tex parameter to AntiAliased.
            CCSpriteBatchNode batch = (CCSpriteBatchNode)GetChildByTag((int)kTags.kTagSpriteBatchNode);
            batch.Texture.SetAntiAliasTexParameters();
            base.OnExit();
        }

        public override string title()
        {
            return "SpriteBatchNode Aliased";
        }
    }
}
