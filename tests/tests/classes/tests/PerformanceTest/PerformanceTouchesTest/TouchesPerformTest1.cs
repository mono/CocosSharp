using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cocos2D;

namespace tests
{
    public class TouchesPerformTest1 : TouchesMainScene
    {

        public TouchesPerformTest1(bool bControlMenuVisible, int nMaxCases, int nCurCase)
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
            return "Targeted touches";
        }

        public override void RegisterWithTouchDispatcher()
        {
            CCDirector.SharedDirector.TouchDispatcher.AddTargetedDelegate(this, 0, true);
        }

        public override bool TouchBegan(CCTouch touch, CCEvent events)
        {
            numberOfTouchesB++;
            return true;
        }

        public override void TouchMoved(CCTouch touch, CCEvent events)
        {
            numberOfTouchesM++;
        }

        public override void TouchEnded(CCTouch touch, CCEvent events)
        {
            numberOfTouchesE++;
        }

        public override void TouchCancelled(CCTouch touch, CCEvent events)
        {
            numberOfTouchesC++;
        }
    }
}
