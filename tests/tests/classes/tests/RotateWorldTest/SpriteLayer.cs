using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
    public class SpriteLayer : CCLayer
    {
        string s_pPathGrossini = "Images/grossini";
        string s_pPathSister1 = "Images/grossinis_sister1";
        string s_pPathSister2 = "Images/grossinis_sister2";

        public override void OnEnter()
        {
            base.OnEnter();

            float x, y;

            CCSize size = CCApplication.SharedApplication.MainWindowDirector.WindowSizeInPoints;
            x = size.Width;
            y = size.Height;

            CCSprite sprite = new CCSprite(s_pPathGrossini);
            CCSprite spriteSister1 = new CCSprite(s_pPathSister1);
            CCSprite spriteSister2 = new CCSprite(s_pPathSister2);

            sprite.Scale = (1.5f);
            spriteSister1.Scale = (1.5f);
            spriteSister2.Scale = (1.5f);

            sprite.Position = (new CCPoint(x / 2, y / 2));
            spriteSister1.Position = (new CCPoint(40, y / 2));
            spriteSister2.Position = (new CCPoint(x - 40, y / 2));

            CCAction rot = new CCRotateBy (16, -3600);

            AddChild(sprite);
            AddChild(spriteSister1);
            AddChild(spriteSister2);

            sprite.RunAction(rot);

			var jump1 = new CCJumpBy (4, new CCPoint(-400, 0), 100, 4);
			var jump2 = jump1.Reverse();

			var rot1 = new CCRotateBy (4, 360 * 2);
			var rot2 = rot1.Reverse();

			spriteSister1.Repeat (5, jump2, jump1);
			spriteSister2.Repeat (5, jump1, jump2);

			spriteSister1.Repeat (5, rot1, rot2);
			spriteSister2.Repeat (5, rot2, rot1);
        }

        public static new SpriteLayer node()
        {
            SpriteLayer node = new SpriteLayer();
            return node;
        }
    }
}
