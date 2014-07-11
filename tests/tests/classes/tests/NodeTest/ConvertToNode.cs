using System.Collections.Generic;
using CocosSharp;

namespace tests
{
    public class ConvertToNode : TestCocosNodeDemo
    {
        public ConvertToNode()
        {

        }

        protected virtual void AddedToNewScene()
        {
            base.AddedToNewScene();

			var listener = new CCEventListenerTouchAllAtOnce();
			listener.OnTouchesEnded = onTouchesEnded;

			AddEventListener(listener);    

            CCSize s = Scene.VisibleBoundsWorldspace.Size;

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
						sprite.AnchorPoint = CCPoint.AnchorLowerLeft;
						break;
					case 1:
						sprite.AnchorPoint = CCPoint.AnchorMiddle;
						break;
					case 2:
						sprite.AnchorPoint = CCPoint.AnchorUpperRight;
						break;
				}

				point.Position = sprite.Position;

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
				var location = touch.LocationOnScreen;

				for( int i = 0; i < 3; i++)
				{
					var node = this[100+i];
					CCPoint p1, p2;

                    p1 = node.Scene.ScreenToWorldspace(location);
					p2 = node.Scene.ScreenToWorldspace(location);

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