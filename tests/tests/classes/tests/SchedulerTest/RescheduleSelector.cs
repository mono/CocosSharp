using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class RescheduleSelector : SchedulerTestLayer
    {
        public override void OnEnter()
        {
            base.OnEnter();

            m_fInterval = 1.0f;
            m_nTicks = 0;
            Schedule(schedUpdate, m_fInterval);
        }

        public override string title()
        {
            return "Reschedule Selector";
        }

        public override string subtitle()
        {
            return "Interval is 1 second, then 2, then 3...";
        }

        public void schedUpdate(float dt)
        {
            m_nTicks++;
            CCLog.Log("schedUpdate: %.2f", dt);
            if (m_nTicks > 3)
            {
                m_fInterval += 1.0f;
                Schedule(schedUpdate, m_fInterval);
                m_nTicks = 0;
            }
        }
        private float m_fInterval;
        private int m_nTicks;
    }
}
