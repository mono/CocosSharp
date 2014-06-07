using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
    public class Sprite6 : SpriteTestDemo
    {
        int kTagSpriteBatchNode = 1;
        public Sprite6()
        {
            // small capacity. Testing resizing
            // Don't use capacity=1 in your real game. It is expensive to resize the capacity
			var batch = new CCSpriteBatchNode("Images/grossini_dance_atlas", 1);
            AddChild(batch, 0, kTagSpriteBatchNode);
            batch.IgnoreAnchorPointForPosition = true;

			var s = CCApplication.SharedApplication.MainWindowDirector.WinSize;

            batch.AnchorPoint = new CCPoint(0.5f, 0.5f);
            batch.ContentSize = (new CCSize(s.Width, s.Height));

            // SpriteBatchNode actions
			var rotate = new CCRotateBy (5, 360);
			var action = new CCRepeatForever (rotate);

            // SpriteBatchNode actions
			var rotate_back = rotate.Reverse();

			var scale = new CCScaleBy(5, 1.5f);
			var scale_back = scale.Reverse();

            float step = s.Width / 4;

            for (int i = 0; i < 3; i++)
            {
                CCSprite sprite = new CCSprite(batch.Texture, new CCRect(85 * i, 121 * 1, 85, 121));
                sprite.Position = (new CCPoint((i + 1) * step, s.Height / 2));

                sprite.RunAction(action);
                batch.AddChild(sprite, i);
            }

			batch.RepeatForever(scale, scale_back);
			batch.RepeatForever(rotate, rotate_back);
        }

        public override string title()
        {
            return "SpriteBatchNode transformation";
        }
    }
}
