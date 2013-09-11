using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cocos2D;

namespace tests
{
    public class MultiTouchTestLayer : CCLayer
    {
        private CCColor3B[] s_TouchColors = new CCColor3B[] 
        {
            CCColor3B.Yellow,
            CCColor3B.Blue,
            CCColor3B.Green,
            CCColor3B.Red,
            CCColor3B.Magenta
        };

        private static Dictionary<int, TouchPoint> s_dic = new Dictionary<int, TouchPoint>();

        public override bool Init()
        {
            if (base.Init())
            {
                TouchEnabled = true;
                return true;
            }
            return false;
        }

        public override void RegisterWithTouchDispatcher()
        {
            base.RegisterWithTouchDispatcher();
            CCDirector.SharedDirector.TouchDispatcher.AddStandardDelegate(this, 0);
        }

        public override void TouchesBegan(List<CCTouch> touches)
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

        public override void TouchesMoved(List<CCTouch> touches)
        {
            foreach(var item in touches)
            {
                CCTouch touch = item;
                TouchPoint pTP = s_dic[touch.Id];
                CCPoint location = touch.Location;
                pTP.SetTouchPos(location);
            }
        }

        public override void TouchesEnded(List<CCTouch> touches)
        {
            foreach (var item in touches )
            {
                CCTouch touch = item;
                TouchPoint pTP = s_dic[touch.Id];
                RemoveChild(pTP, true);
                s_dic.Remove(touch.Id);
            }
        }

        public override void TouchesCancelled(List<CCTouch> touches)
        {
            TouchesEnded(touches);
        }

    }

    public class MultiTouchTestScene : TestScene
    {
        public override void runThisTest()
        {
            MultiTouchTestLayer layer = new MultiTouchTestLayer();

            AddChild(layer, 0);

            CCDirector.SharedDirector.ReplaceScene(this);
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

        public override void Draw()
        {
            CCDrawingPrimitives.Begin();
            CCDrawingPrimitives.DrawLine(new CCPoint(0, _touchPoint.Y), new CCPoint(ContentSize.Width, _touchPoint.Y),
                                         new CCColor4B(_touchColor.R, _touchColor.G, _touchColor.B, 255));
            CCDrawingPrimitives.DrawLine(new CCPoint(_touchPoint.X, 0), new CCPoint(_touchPoint.X, ContentSize.Height),
                                         new CCColor4B(_touchColor.R, _touchColor.G, _touchColor.B, 255));
            CCDrawingPrimitives.DrawPoint(_touchPoint, 30,
                                          new CCColor4B(_touchColor.R, _touchColor.G, _touchColor.B, 255));
            CCDrawingPrimitives.End();
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
            pRet.AnchorPoint = new CCPoint(0.0f, 0.0f);
            return pRet;
        }

        private CCPoint _touchPoint;
        private CCColor3B _touchColor;
    }
}
