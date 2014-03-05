using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
    public class RescheduleSelector : SchedulerTestLayer
    {
        public override void OnEnter()
        {
            base.OnEnter();

            interval = 1.0f;
            ticks = 0;
            Schedule(schedUpdate, interval);
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
            ticks++;
			CCLog.Log("schedUpdate: {0:F2}", dt);
            if (ticks > 3)
            {
                interval += 1.0f;
                Schedule(schedUpdate, interval);
                ticks = 0;
            }
        }
		private float interval;
		private int ticks;
    }
}
