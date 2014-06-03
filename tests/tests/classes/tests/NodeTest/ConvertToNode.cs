using System.Collections.Generic;
using CocosSharp;

namespace tests
{
    public class ConvertToNode : TestCocosNodeDemo
    {
        public ConvertToNode()
        {
			var listener = new CCEventListenerTouchAllAtOnce();
			listener.OnTouchesEnded = onTouchesEnded;

			AddEventListener(listener);    

            CCSize s = CCDirector.SharedDirector.WinSize;

			var rotate = new CCRotateBy (10, 360);

            for (int i = 0; i < 3; i++)
            {
                CCSprite sprite = new CCSprite("Images/grossini");
                sprite.Position = (new CCPoint(s.Width / 4 * (i + 1), s.Height / 2));

                CCSprite point = new CCSprite("Images/r1");
                point.Scale = 0.25f;
                point.Position = sprite.Position;
                AddChild(point, 10, 100 + i);

                switch (i)
                {
                    case 0:
                        sprite.AnchorPoint = new CCPoint(0, 0);
                        break;
                    case 1:
                        sprite.AnchorPoint = (new CCPoint(0.5f, 0.5f));
                        break;
                    case 2:
                        sprite.AnchorPoint = (new CCPoint(1, 1));
                        break;
                }

                point.Position = (sprite.Position);

				sprite.RepeatForever(rotate);

                AddChild(sprite, i);
            }
        }

        public override string title()
        {
            return "Convert To Node Space";
        }

		void onTouchesEnded(List<CCTouch> touches, CCEvent touchEvent)
        {
			foreach( var touch in touches)
			{
				var location = touch.Location;

				for( int i = 0; i < 3; i++)
				{
					var node = GetChildByTag(100+i);
					CCPoint p1, p2;

					p1 = node.ConvertToNodeSpaceAr(location);
					p2 = node.ConvertToNodeSpace(location);

					CCLog.Log("AR: x={0:f2}, y={1:f2} -- Not AR: x={2:f2}, y={3:f2}", p1.X, p1.Y, p2.X, p2.Y);
				}
			}    

        }

        public override string subtitle()
        {
            return "testing convertToNodeSpace / AR. Touch and see console";
        }
    }
}