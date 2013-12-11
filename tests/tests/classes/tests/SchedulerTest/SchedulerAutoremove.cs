using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
    public class SchedulerAutoremove : SchedulerTestLayer
    {
        public virtual void onEnter()
        {
            base.OnEnter();

            Schedule(autoremove, 0.5f);
            Schedule(tick, 0.5f);
            accum = 0;
        }

        public override string title()
        {
            return "Self-remove an scheduler";
        }

        public override string subtitle()
        {
            return "1 scheduler will be autoremoved in 3 seconds. See console";
        }

        public void autoremove(float dt)
        {
            accum += dt;
            CCLog.Log("Time: %f", accum);

            if (accum > 3)
            {
                Unschedule(autoremove);
                CCLog.Log("scheduler removed");
            }
        }

        public void tick(float dt)
        {
            CCLog.Log("This scheduler should not be removed");
        }

        private float accum;
    }
}
