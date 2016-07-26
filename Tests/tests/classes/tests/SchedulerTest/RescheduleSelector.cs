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
			CCLog.Log("schedUpdate: {0:F4}", dt);
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

	public class SchedulerDelayAndRepeat : SchedulerTestLayer
	{
		public override void OnEnter()
		{
			base.OnEnter();

			Schedule(update, 0, 4 , 3.0f);
			CCLog.Log("update is scheduled should begin after 3 seconds");

		}

		public override string title()
		{
			return "Schedule with delay of 3 sec, repeat 4 times";
		}

		public override string subtitle()
		{
			return "After 5 x executed, method unscheduled. See console";
		}

		public void update(float dt)
		{
			CCLog.Log("update called: {0}", dt);
		}
	}
}
