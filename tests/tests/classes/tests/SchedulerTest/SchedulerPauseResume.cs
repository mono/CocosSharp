using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using CocosSharp;

namespace tests
{
    public class SchedulerPauseResume : SchedulerTestLayer
    {
        public override void OnEnter()
        {
            base.OnEnter();

            Schedule(tick1, 0.5f);
            Schedule(tick2, 0.5f);
			Schedule(pause, 3.0f);
        }

        public override string title()
        {
            return "Pause / Resume";
        }

        public override string subtitle()
        {
            return "Scheduler should be paused after 3 seconds. See console";
        }

        public void tick1(float dt)
        {
            CCLog.Log("tick1");
        }

        public void tick2(float dt)
        {
            CCLog.Log("tick2");
        }

        public void pause(float dt)
        {
            CCDirector.SharedDirector.Scheduler.PauseTarget(this);
        }
    }

	public class SchedulerPauseResumeAll : SchedulerTestLayer
	{

		private static CCRotateBy rotateBy = new CCRotateBy(3.0f, 360);

		public override void OnEnter()
		{
			base.OnEnter();

			var s = CCDirector.SharedDirector.WinSize;

			var sprite = new CCSprite("Images/grossinis_sister1.png");
			sprite.Position = CCVisibleRect.Center;
			AddChild(sprite);
			sprite.RepeatForever(rotateBy);

			// Add a menu item to resume the Scheduled actions.
			uint fontSize = 32;
			string fontName = "MarkerFelt";

			var menu = new CCMenu(
				new CCMenuItemFont("Resume", fontName, fontSize, resume)
			);

			menu.AlignItemsVertically(4);
			menu.Position = new CCPoint(s.Width / 2, s.Height / 4);
			AddChild(menu);


			Schedule ();
			Schedule (tick1, 0.5f);
			Schedule (tick2, 1.0f);
			Schedule (pause, 3.0f, 0, 0);
		}

		private int times;
		public override void Update (float dt)
		{
			// Create a counter so that we can actually see the console output
			times++;
			if (times < 20)
				// base.Update (dt);
				// Do nothing
				CCLog.Log ("Update {0}", dt);
		}

		public override void OnExit()
		{
			if (pausedTargets != null && pausedTargets.Count > 0)
			{
				CCDirector.SharedDirector.Scheduler.Resume (pausedTargets);
				pausedTargets.Clear ();
			}
			base.OnExit();
		}


		public override string title()
		{
			return "Pause / Resume";
		}

		public override string subtitle()
		{
			return "Everything will pause after 3s, then resume at 5s. See console";
		}

		public void tick1(float dt)
		{
			CCLog.Log("tick1");
		}

		public void tick2(float dt)
		{
			CCLog.Log("tick2");
		}

		List<ICCUpdatable> pausedTargets;

		public void pause(float dt)
		{
			CCLog.Log ("Pausing");
			pausedTargets = CCDirector.SharedDirector.Scheduler.PauseAllTargets();

			// should have only 2 items: ActionManager, self
			Debug.Assert(pausedTargets.Count == 2, "Error: pausedTargets should have only 2 items");

		}

		//		public void resume(float dt)
		public void resume(object pSender)
		{
			times = 0;
			CCLog.Log("Resuming");
			CCDirector.SharedDirector.Scheduler.Resume (pausedTargets);
			pausedTargets.Clear ();
			
		}
	}

	public class SchedulerPauseResumeUser : SchedulerTestLayer
	{

		private static CCRotateBy rotateBy = new CCRotateBy(3.0f, 360);

		public override void OnEnter()
		{
			base.OnEnter();

			var s = CCDirector.SharedDirector.WinSize;

			var sprite = new CCSprite("Images/grossinis_sister1.png");
			sprite.Position = CCVisibleRect.Center;
			AddChild(sprite);
			sprite.RepeatForever(rotateBy);

			// Add a menu item to resume the Scheduled actions.
			uint fontSize = 32;
			string fontName = "MarkerFelt";

			var menu = new CCMenu(
				new CCMenuItemFont("Resume", fontName, fontSize, resume)
			);

			menu.AlignItemsVertically(4);
			menu.Position = new CCPoint(s.Width / 2, s.Height / 4);
			AddChild(menu);


			Schedule ();
			Schedule (tick1, 0.5f);
			Schedule (tick2, 1.0f);
			Schedule (pause, 3.0f, 0, 0);
		}

		private int times;
		public override void Update (float dt)
		{
			// Create a counter so that we can actually see the console output
			times++;
			if (times < 20)
				// base.Update (dt);
				// Do nothing
				CCLog.Log ("Update {0}", dt);
		}

		public override void OnExit()
		{
			if (pausedTargets != null && pausedTargets.Count > 0)
			{
				CCDirector.SharedDirector.Scheduler.Resume (pausedTargets);
				pausedTargets.Clear ();
			}
			base.OnExit();
		}


		public override string title()
		{
			return "Pause / Resume";
		}

		public override string subtitle()
		{
			return "Everything will pause after 3s, then resume at 5s. See console";
		}

		public void tick1(float dt)
		{
			CCLog.Log("tick1");
		}

		public void tick2(float dt)
		{
			CCLog.Log("tick2");
		}

		List<ICCUpdatable> pausedTargets;

		public void pause(float dt)
		{
			CCLog.Log ("Pausing");
			pausedTargets = CCDirector.SharedDirector.Scheduler.PauseAllTargets(CCScheduler.PriorityNonSystemMin);

		}

		//		public void resume(float dt)
		public void resume(object pSender)
		{
			times = 0;
			CCLog.Log("Resuming");
			CCDirector.SharedDirector.Scheduler.Resume (pausedTargets);
			pausedTargets.Clear ();

		}
	}

}
