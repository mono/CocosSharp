using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
    public class SchedulerUnscheduleAll : SchedulerTestLayer
    {
        public override void OnEnter()
        {
            base.OnEnter();
            Schedule(tick1, 0.5f);
            Schedule(tick2, 1.0f);
            Schedule(tick3, 1.5f);
            Schedule(tick4, 1.5f);
            Schedule(unscheduleAll, 4);
        }

        public override string title()
        {
            return "Unschedule All selectors";
        }

        public override string subtitle()
        {
            return "All scheduled selectors will be unscheduled in 4 seconds. See console";
        }

        public void tick1(float dt)
        {
            CCLog.Log("tick1");
        }

        public void tick2(float dt)
        {
            CCLog.Log("tick2");
        }

        public void tick3(float dt)
        {
            CCLog.Log("tick3");
        }

        public void tick4(float dt)
        {
            CCLog.Log("tick4");
        }

        public void unscheduleAll(float dt)
        {
            UnscheduleAll();
        }
    }

	public class SchedulerUnscheduleAllUserLevel : SchedulerTestLayer
	{

		private static CCRotateBy rotateBy = new CCRotateBy(3.0f, 360);

		public override void OnEnter()
		{
			base.OnEnter();

			var s = CCApplication.SharedApplication.MainWindowDirector.WinSize;

			var sprite = new CCSprite("Images/grossinis_sister1.png");
			sprite.Position = CCVisibleRect.Center;
			AddChild(sprite);
			sprite.RepeatForever(rotateBy);

			Schedule(tick1, 0.5f);
			Schedule(tick2, 1.0f);
			Schedule(tick3, 1.5f);
			Schedule(tick4, 1.5f);
			Schedule(unscheduleAll, 4);
		}

		public override string title()
		{
			return "Unschedule All user selectors";
		}

		public override string subtitle()
		{
			return "Unschedules all user selectors after 4s. Action should not stop. See console";
		}

		public void tick1(float dt)
		{
			CCLog.Log("tick1");
		}

		public void tick2(float dt)
		{
			CCLog.Log("tick2");
		}

		public void tick3(float dt)
		{
			CCLog.Log("tick3");
		}

		public void tick4(float dt)
		{
			CCLog.Log("tick4");
		}

		public void unscheduleAll(float dt)
		{
			CCApplication.SharedApplication.MainWindowDirector.Scheduler.UnscheduleAll (CCSchedulePriority.User);
		}
	}


}
