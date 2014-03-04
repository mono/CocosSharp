using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using CocosSharp;

namespace tests
{
    public class SchedulerUnscheduleAllHard : SchedulerTestLayer
    {

		private bool actionManagerActive;

        public override void OnEnter()
        {
            base.OnEnter();

			var s = CCDirector.SharedDirector.WinSize;

			var sprite = new CCSprite("Images/grossinis_sister1.png");
			sprite.Position = new CCPoint(s.Width/2, s.Height/2);
			AddChild(sprite);
			sprite.RepeatForever(new CCRotateBy(3.0f, 360));

			actionManagerActive = true;

            Schedule(tick1, 0.5f);
            Schedule(tick2, 1.0f);
            Schedule(tick3, 1.5f);
            Schedule(tick4, 1.5f);
            Schedule(unscheduleAll, 4);
        }

		public override void OnExit ()
		{
			if(!actionManagerActive) {
				// Restore the director's action manager.
				var director = CCDirector.SharedDirector;
				director.Scheduler.ScheduleUpdateForTarget (CCDirector.SharedDirector.ActionManager, CCScheduler.PrioritySystem, false);
			}

			base.OnExit ();
		}

        public override string title()
        {
            return "Unschedule All selectors #2";
        }

        public override string subtitle()
        {
            return "Unschedules all selectors after 4s. Uses CCScheduler. See console";
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
             CCDirector.SharedDirector.Scheduler.UnscheduleAll();
			actionManagerActive = false;
        }
    }
}
