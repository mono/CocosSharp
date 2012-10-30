using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

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
            this.TouchEnabled = true;

            CCSize s = CCDirector.SharedDirector.WinSize;

            // create the streak object and add it to the scene
            streak = CCMotionStreak.Create(3, 3, 64, ccTypes.ccWHITE, s_streak);
            AddChild(streak);

            streak.Position = (new CCPoint(s.width / 2, s.height / 2));
        }

        public override void TouchesMoved(List<CCTouch> touches, CCEvent event_)
        {
            streak.Position = touches[0].Location;
        }

        public override string title()
        {
            return "MotionStreak test";
        }
    }
}
