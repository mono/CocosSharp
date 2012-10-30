using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class TouchesPerformTest2 : TouchesMainScene
    {
        public TouchesPerformTest2(bool bControlMenuVisible, int nMaxCases, int nCurCase)
            : base(bControlMenuVisible, nMaxCases, nCurCase)
        {
        }

        public override void OnEnter()
        {
            base.OnEnter();
            TouchEnabled = true;
        }

        public override string title()
        {
            return "Standard touches";
        }

        public override void RegisterWithTouchDispatcher()
        {
            CCDirector.SharedDirector.TouchDispatcher.AddStandardDelegate(this, 0);
        }

        public override void TouchesBegan(List<CCTouch> touches, CCEvent events)
        {
            numberOfTouchesB += touches.Count;
        }

        public override void TouchesMoved(List<CCTouch> touches, CCEvent events)
        {
            numberOfTouchesM += touches.Count;
        }

        public override void TouchesEnded(List<CCTouch> touches, CCEvent events)
        {
            numberOfTouchesE += touches.Count;
        }

        public override void TouchesCancelled(List<CCTouch> touches, CCEvent events)
        {
            numberOfTouchesC += touches.Count;
        }
    }
}
