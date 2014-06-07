using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
    public class SpriteBatchNodeAliased : SpriteTestDemo
    {
        public SpriteBatchNodeAliased()
        {
			var batch = new CCSpriteBatchNode("Images/grossini_dance_atlas", 10);
            AddChild(batch, 0, (int)kTags.kTagSpriteBatchNode);

			var s = CCApplication.SharedApplication.MainWindowDirector.WinSize;

			var sprite1 = new CCSprite(batch.Texture, new CCRect(85 * 1, 121 * 1, 85, 121));
            sprite1.Position = new CCPoint(s.Width / 2 - 100, s.Height / 2);
            batch.AddChild(sprite1, 0, (int)kTagSprite.kTagSprite1);

			var sprite2 = new CCSprite(batch.Texture, new CCRect(85 * 1, 121 * 1, 85, 121));
            sprite2.Position = (new CCPoint(s.Width / 2 + 100, s.Height / 2));
            batch.AddChild(sprite2, 0, (int)kTagSprite.kTagSprite2);

			var scale = new CCScaleBy(2, 5);
			var scale_back = (CCActionInterval)scale.Reverse();
			var repeat = new CCRepeatForever (scale, scale_back);

            sprite1.RunAction(repeat);
			sprite2.RunAction(repeat);
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
