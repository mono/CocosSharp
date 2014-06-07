using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

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
            CCSize size = CCApplication.SharedApplication.MainWindowDirector.WindowSizeInPoints;

            streak = new CCMotionStreak(2.0f, 1.0f, 50.0f, new CCColor3B(255, 255, 0), "Images/Icon");
            AddChild(streak);

            m_center = new CCPoint(size.Width / 2, size.Height / 2);
            m_fRadius = size.Width / 3f;
            m_fAngle = 0.0f;

            Schedule(onUpdate, 0);
        }

        public void onUpdate(float delta)
        {
            m_fAngle += 1.0f;
            streak.Position = new CCPoint(m_center.X + (float) Math.Cos(m_fAngle / 180f * Math.PI) * m_fRadius,
                                          m_center.Y + (float) Math.Sin(m_fAngle / 180f * Math.PI) * m_fRadius);
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

