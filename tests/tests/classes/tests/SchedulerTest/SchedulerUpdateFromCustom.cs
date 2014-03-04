using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
    public class SchedulerUpdateFromCustom : SchedulerTestLayer
    {
        public override void OnEnter()
        {
            base.OnEnter();
            Schedule(schedUpdate, 2.0f);
        }

        public override string title()
        {
            return "Schedule Update in 2 sec";
        }

        public override string subtitle()
        {
            return "Update schedules in 2 secs. Stops 2 sec later. See console";
        }

        public void update(float dt)
        {
            CCLog.Log("update called:{0}", dt);
        }

        public void schedUpdate(float dt)
        {
            Unschedule(schedUpdate);
            base.Schedule ();
            Schedule(stopUpdate, 2.0f);
        }

        public void stopUpdate(float dt)
        {
            Unschedule ();
            Unschedule(stopUpdate);
        }
    }
}
