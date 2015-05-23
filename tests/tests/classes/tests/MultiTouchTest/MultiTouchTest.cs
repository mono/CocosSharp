using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
    public class MultiTouchTestLayer : CCLayer
    {

		CCLabel title;

        public MultiTouchTestLayer()
        {
			var listener = new CCEventListenerTouchAllAtOnce();
			listener.OnTouchesBegan = onTouchesBegan;
			listener.OnTouchesMoved = onTouchesMoved;
			listener.OnTouchesEnded = onTouchesEnded;

			AddEventListener(listener);   

			title = new CCLabel("Please touch the screen!", "arial", 24, CCLabelFormat.SpriteFont);
            title.AnchorPoint = CCPoint.AnchorMiddle;
			AddChild(title);
		}

        protected override void AddedToScene()
        {
            base.AddedToScene();

            title.Position = VisibleBoundsWorldspace.Top() + new CCPoint(0, -40);
		}

        private CCColor3B[] s_TouchColors = new CCColor3B[] 
        {
            CCColor3B.Yellow,
            CCColor3B.Blue,
            CCColor3B.Green,
            CCColor3B.Red,
            CCColor3B.Magenta
        };

        private static Dictionary<int, TouchPoint> s_dic = new Dictionary<int, TouchPoint>();

		void onTouchesBegan(List<CCTouch> touches, CCEvent touchEvent)
        {
            foreach (var item in touches)
            {
                CCTouch touch = (item);
                TouchPoint touchPoint = TouchPoint.TouchPointWithParent(this);
                CCPoint location = touch.Location;
                touchPoint.SetTouchPos(location);
                touchPoint.SetTouchColor(s_TouchColors[touch.Id % 5]);

                AddChild(touchPoint);
                s_dic.Add( touch.Id, touchPoint);
            }
        }

		void onTouchesMoved(List<CCTouch> touches, CCEvent touchEvent)
        {
            foreach(var item in touches)
            {
                CCTouch touch = item;
                TouchPoint pTP = s_dic[touch.Id];
                CCPoint location = touch.LocationOnScreen;
                location = Layer.ScreenToWorldspace(location);
                pTP.SetTouchPos(location);
            }
        }

		void onTouchesEnded(List<CCTouch> touches, CCEvent touchEvent)
        {
            foreach (var item in touches )
            {
                CCTouch touch = item;
                TouchPoint pTP = s_dic[touch.Id];
                RemoveChild(pTP, true);
                s_dic.Remove(touch.Id);
            }
        }

		void onTouchesCancelled(List<CCTouch> touches, CCEvent touchEvent)
        {
			onTouchesEnded(touches, touchEvent);
        }

    }

    public class MultiTouchTestScene : TestScene
    {
        public override void runThisTest()
        {
            MultiTouchTestLayer layer = new MultiTouchTestLayer();

            AddChild(layer, 0);

            Scene.Director.ReplaceScene(this);
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

    public class TouchPoint : CCNode
    {

        CCDrawNode touchPoint = new CCDrawNode();

        public TouchPoint()
        {
            AddChild (touchPoint);
        }

        protected override void VisitRenderer (ref CCAffineTransform worldTransform)
        {
            base.VisitRenderer (ref worldTransform);
            Renderer.AddCommand(new CCCustomCommand(worldTransform.Tz, worldTransform, RenderTouchPoint));
        }

        protected void RenderTouchPoint()
        {
            touchPoint.Clear ();
            touchPoint.Color = new CCColor3B (_touchColor.R, _touchColor.G, _touchColor.B);
            touchPoint.Opacity = 255;
            touchPoint.DrawLine (new CCPoint (0, _touchPoint.Y), new CCPoint (ContentSize.Width, _touchPoint.Y), 5);
            touchPoint.DrawLine (new CCPoint(_touchPoint.X, 0), new CCPoint(_touchPoint.X, ContentSize.Height), 5);
            touchPoint.DrawRect(_touchPoint, 30);
        }

        public void SetTouchPos(CCPoint pt)
        {
            _touchPoint = pt;
        }

        public void SetTouchColor(CCColor3B color)
        {
            _touchColor = color;
        }

        public static TouchPoint TouchPointWithParent(CCNode pParent)
        {
            TouchPoint pRet = new TouchPoint();
            pRet.ContentSize = pParent.ContentSize;
			pRet.AnchorPoint = CCPoint.AnchorLowerLeft;
            return pRet;
        }

        private CCPoint _touchPoint;
        private CCColor3B _touchColor;
    }
}
