using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class Issue1358 : MotionStreakTest
    {
        private float m_fRadius;
        private float m_fAngle;
        private CCPoint m_center;

        public override void OnEnter()
        {
            base.OnEnter();

            // ask director the the window size
            CCSize size = CCDirector.SharedDirector.WinSize;

            streak = CCMotionStreak.Create(2.0f, 1.0f, 50.0f, new ccColor3B(255, 255, 0), "Images/Icon");
            AddChild(streak);

            m_center = new CCPoint(size.width / 2, size.height / 2);
            m_fRadius = size.width / 3f;
            m_fAngle = 0.0f;

            Schedule(onUpdate, 0);
        }

        public void onUpdate(float delta)
        {
            m_fAngle += 1.0f;
            streak.Position = new CCPoint(m_center.x + (float) Math.Cos(m_fAngle / 180f * Math.PI) * m_fRadius,
                                          m_center.y + (float) Math.Sin(m_fAngle / 180f * Math.PI) * m_fRadius);
        }

        public override string title()
        {
            return "Issue 1358";
        }

        public override string subtitle()
        {
            return "The tail should use the texture";
        }
    }
}

