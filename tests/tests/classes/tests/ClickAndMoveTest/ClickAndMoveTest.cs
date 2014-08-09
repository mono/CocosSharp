using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
    public class ClickAndMoveTest : TestScene
    {
        public static int kTagSprite = 1;
        public static string s_pPathGrossini = "Images/grossini";

        public override void runThisTest()
        {
            CCLayer pLayer = new MainLayer();

            AddChild(pLayer);
            Director.ReplaceScene(this);
        }

        protected override void NextTestCase()
        {
        }
        protected override void PreviousTestCase()
        {
        }
        protected override void RestTestCase()
        {
        }
    }

    public class MainLayer : CCLayer
    {
        public MainLayer()
        {
			var listener = new CCEventListenerTouchAllAtOnce();
			listener.OnTouchesEnded = onTouchesEnded;

            AddEventListener(listener);    

            CCSprite sprite = new CCSprite(ClickAndMoveTest.s_pPathGrossini);

            CCLayer layer = new CCLayerColor(new CCColor4B(255, 255, 0, 255));
            AddChild(layer, -1);

            AddChild(sprite, 0, ClickAndMoveTest.kTagSprite);
            sprite.Position = new CCPoint(20, 150);

            sprite.RunAction(new CCJumpTo (4, new CCPoint(300, 48), 100, 4));
			layer.RepeatForever(new CCFadeIn(1), new CCFadeOut(1));
        }

		void onTouchesEnded(List<CCTouch> touches, CCEvent touchEvent)
        {
            //base.ccTouchesEnded(touches, event_);
            object it = touches.First();
            CCTouch touch = (CCTouch)(it);

            var convertedLocation = touch.Location;

            CCNode s = this[ClickAndMoveTest.kTagSprite];
            s.StopAllActions();
            s.RunAction(new CCMoveTo (1, new CCPoint(convertedLocation.X, convertedLocation.Y)));
            float o = convertedLocation.X - s.Position.X;
            float a = convertedLocation.Y - s.Position.Y;
            float at = (float)(Math.Atan(o / a) * 57.29577951f);

            if (a < 0)
            {
                if (o < 0)
                    at = 180 + Math.Abs(at);
                else
                    at = 180 - Math.Abs(at);
            }

            s.RunAction(new CCRotateTo (1, at));
        }
    }
}
