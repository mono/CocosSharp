using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
    public class SpriteAliased : SpriteTestDemo
    {
        public SpriteAliased()
        {
			var s = CCDirector.SharedDirector.WinSize;

			var sprite1 = new CCSprite("Images/grossini_dance_atlas", new CCRect(85 * 1, 121 * 1, 85, 121));
            sprite1.Position = new CCPoint(s.Width / 2 - 100, s.Height / 2);
            AddChild(sprite1, 0, (int)kTagSprite.kTagSprite1);

			var sprite2 = new CCSprite("Images/grossini_dance_atlas", new CCRect(85 * 1, 121 * 1, 85, 121));
            sprite2.Position = new CCPoint(s.Width / 2 + 100, s.Height / 2);
            AddChild(sprite2, 0, (int)kTagSprite.kTagSprite2);

			var scale = new CCScaleBy(2, 5);
			var scale_back = scale.Reverse();
			var seq = new CCSequence(scale, scale_back);

			sprite1.RepeatForever(seq);
			sprite2.RepeatForever(seq);
        }

        public override void OnEnter()
        {
            base.OnEnter();

            //
            // IMPORTANT:
            // This change will affect every sprite that uses the same texture
            // So sprite1 and sprite2 will be affected by this change
            //
			var sprite = (CCSprite)GetChildByTag((int)kTagSprite.kTagSprite1);
			sprite.IsAntialiased = false;
        }

        public override void OnExit()
        {
            // restore the tex parameter to AntiAliased.
			var sprite = (CCSprite)GetChildByTag((int)kTagSprite.kTagSprite1);
            sprite.IsAntialiased = true;
            base.OnExit();
        }

        public override string title()
        {
            return "Sprite Aliased";
        }
    }
}
