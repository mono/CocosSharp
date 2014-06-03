using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
    public class MotionStreakTest2 : MotionStreakTest
    {
        string s_streak = "Images/streak";
        protected CCNode m_root;
        protected CCNode m_target;

        public override void OnEnter()
        {
            base.OnEnter();

			var listener = new CCEventListenerTouchAllAtOnce();
			listener.OnTouchesMoved = onTouchesMoved;
			AddEventListener(listener); 

            CCSize s = CCDirector.SharedDirector.WinSize;

            // create the streak object and add it to the scene
            streak = new CCMotionStreak(3, 3, 64, CCColor3B.White, s_streak);
            AddChild(streak);

            streak.Position = (new CCPoint(s.Width / 2, s.Height / 2));
        }

		void onTouchesMoved(List<CCTouch> touches, CCEvent touchEvent)
        {
            streak.Position = touches[0].Location;
        }

        public override string title()
        {
            return "MotionStreak test";
        }
    }
}
