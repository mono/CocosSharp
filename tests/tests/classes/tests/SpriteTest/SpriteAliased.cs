using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class SpriteAliased : SpriteTestDemo
    {
        public SpriteAliased()
        {
            CCSize s = CCDirector.SharedDirector.WinSize;

            CCSprite sprite1 = CCSprite.Create("Images/grossini_dance_atlas", new CCRect(85 * 1, 121 * 1, 85, 121));
            sprite1.Position = (new CCPoint(s.Width / 2 - 100, s.Height / 2));
            AddChild(sprite1, 0, (int)kTagSprite.kTagSprite1);

            CCSprite sprite2 = CCSprite.Create("Images/grossini_dance_atlas", new CCRect(85 * 1, 121 * 1, 85, 121));
            sprite2.Position = (new CCPoint(s.Width / 2 + 100, s.Height / 2));
            AddChild(sprite2, 0, (int)kTagSprite.kTagSprite2);

            CCActionInterval scale = CCScaleBy.Create(2, 5);
            CCActionInterval scale_back = (CCActionInterval)scale.Reverse();
            CCActionInterval seq = (CCActionInterval)(CCSequence.FromActions(scale, scale_back));
            CCAction repeat = CCRepeatForever.Create(seq);

            CCAction repeat2 = (CCAction)(repeat.Copy());

            sprite1.RunAction(repeat);
            sprite2.RunAction(repeat2);
        }

        public override void OnEnter()
        {
            base.OnEnter();

            //
            // IMPORTANT:
            // This change will affect every sprite that uses the same texture
            // So sprite1 and sprite2 will be affected by this change
            //
            CCSprite sprite = (CCSprite)GetChildByTag((int)kTagSprite.kTagSprite1);
            sprite.Texture.SetAliasTexParameters();
        }

        public override void OnExit()
        {
            // restore the tex parameter to AntiAliased.
            CCSprite sprite = (CCSprite)GetChildByTag((int)kTagSprite.kTagSprite1);
            sprite.Texture.SetAntiAliasTexParameters();
            base.OnExit();
        }

        public override string title()
        {
            return "Sprite Aliased";
        }
    }
}
