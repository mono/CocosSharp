using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

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
			// Register Touch Event
			var touchListener = new CCEventListenerTouchOneByOne();
			touchListener.IsSwallowTouches = true;

			touchListener.OnTouchBegan = onTouchBegan;
			touchListener.OnTouchMoved = onTouchMoved;
			touchListener.OnTouchEnded = onTouchEnded;
			touchListener.OnTouchCancelled = onTouchCancelled;

			EventDispatcher.AddEventListener(touchListener, this);
        }

        public override string title()
        {
            return "Targeted touches";
        }

		bool onTouchBegan(CCTouch touch, CCEvent touchEvent)
        {
            numberOfTouchesB++;
            return true;
        }

		void onTouchMoved(CCTouch touch, CCEvent touchEvent)
        {
            numberOfTouchesM++;
        }

		void onTouchEnded(CCTouch touch, CCEvent touchEvent)
        {
            numberOfTouchesE++;
        }

		void onTouchCancelled(CCTouch touch, CCEvent touchEvent)
        {
            numberOfTouchesC++;
        }
    }
}
