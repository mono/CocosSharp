using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

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
			// Register Touch Event
			var touchListener = new CCEventListenerTouchAllAtOnce();

			touchListener.OnTouchesBegan = onTouchesBegan;
			touchListener.OnTouchesMoved = onTouchesMoved;
			touchListener.OnTouchesEnded = onTouchesEnded;
			touchListener.OnTouchesCancelled = onTouchesCancelled;

			AddEventListener(touchListener);
        }

        public override string title()
        {
            return "Standard touches";
        }

		void onTouchesBegan(List<CCTouch> touches, CCEvent touchEvent)
        {
            numberOfTouchesB += touches.Count;
        }

		void onTouchesMoved(List<CCTouch> touches, CCEvent touchEvent)
        {
            numberOfTouchesM += touches.Count;
        }

		void onTouchesEnded(List<CCTouch> touches, CCEvent touchEvent)
        {
            numberOfTouchesE += touches.Count;
        }

		void onTouchesCancelled(List<CCTouch> touches, CCEvent touchEvent)
        {
			numberOfTouchesC += touches.Count;
        }
    }
}
