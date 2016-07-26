using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
    public class SpriteLayer : CCNode
    {

		// Image paths
		string grossini = "Images/grossini";
		string sister1 = "Images/grossinis_sister1";
		string sister2 = "Images/grossinis_sister2";

		// Actions
		static CCJumpBy jump1 = new CCJumpBy (4, new CCPoint(-400, 0), 100, 4);
		static CCFiniteTimeAction jump2 = jump1.Reverse();
		static CCRotateBy rot1 = new CCRotateBy (4, 360 * 2);
		static CCFiniteTimeAction rot2 = rot1.Reverse();
		static CCAction rot = new CCRotateBy (16, -3600);

		public SpriteLayer ()
		{}

        public override void OnEnter()
        {
            base.OnEnter();

            float x, y;

            CCSize size = Layer.VisibleBoundsWorldspace.Size;
            x = size.Width;
            y = size.Height;

            CCSprite sprite = new CCSprite(grossini);
            CCSprite spriteSister1 = new CCSprite(sister1);
            CCSprite spriteSister2 = new CCSprite(sister2);

            sprite.Scale = 1.5f;
            spriteSister1.Scale = 1.5f;
            spriteSister2.Scale = 1.5f;

            sprite.Position = size.Center;
            spriteSister1.Position = (new CCPoint(40, y / 2));
            spriteSister2.Position = (new CCPoint(x - 40, y / 2));

            AddChild(sprite);
            AddChild(spriteSister1);
            AddChild(spriteSister2);

            sprite.RunAction(rot);

			spriteSister1.Repeat (5, jump2, jump1);
			spriteSister2.Repeat (5, jump1, jump2);

			spriteSister1.Repeat (5, rot1, rot2);
			spriteSister2.Repeat (5, rot2, rot1);
        }

    }
}
