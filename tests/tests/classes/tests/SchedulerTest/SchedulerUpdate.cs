using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
    public class SchedulerUpdate : SchedulerTestLayer
    {
		public override void OnEnter ()
		{
            base.OnEnter();

			TestNode d = new TestNode("---", 50);
            AddChild(d);

			TestNode b = new TestNode("3rd", 0);
            AddChild(b);

			TestNode a = new TestNode("1st", -10);
            AddChild(a);

			TestNode c = new TestNode("4th", 10);
            AddChild(c);

			TestNode e = new TestNode("5th", 20);
            AddChild(e);

			TestNode f = new TestNode("2nd", -5);
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
                item.UnscheduleAll();
            }
        }
    }
}
