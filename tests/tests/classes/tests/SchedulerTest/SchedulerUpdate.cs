using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cocos2D;

namespace tests
{
    public class SchedulerUpdate : SchedulerTestLayer
    {
        public virtual void onEnter()
        {
            base.OnEnter();

            TestNode d = new TestNode();
            string pStr = "---";
            d.initWithString(pStr, 50);
            AddChild(d);

            TestNode b = new TestNode();
            pStr = "3rd";
            b.initWithString(pStr, 0);
            AddChild(b);

            TestNode a = new TestNode();
            pStr = "1st";
            a.initWithString(pStr, -10);
            AddChild(a);

            TestNode c = new TestNode();
            pStr = "4th";
            c.initWithString(pStr, 10);
            AddChild(c);

            TestNode e = new TestNode();
            pStr = "5th";
            e.initWithString(pStr, 20);
            AddChild(e);

            TestNode f = new TestNode();
            pStr = "2nd";
            f.initWithString(pStr, -5);
            AddChild(f);

            Schedule(removeUpdates, 4.0f);
        }

        public override string title()
        {
            return "Schedule update with priority";
        }

        public override string subtitle()
        {
            return "3 scheduled updates. Priority should work. Stops in 4s. See console";
        }

        void removeUpdates(float dt)
        {
            var children = this.Children;

            foreach (var item in children)
            {
                if (item == null)
                {
                    break;
                }
                item.UnscheduleAllSelectors();
            }
        }
    }
}
