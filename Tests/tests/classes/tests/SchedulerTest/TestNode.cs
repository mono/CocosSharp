using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
    public class TestNode : CCNode
    {
		private string printString;

		public TestNode(string printString, int priority)
		{
			this.printString = printString;
			Schedule (priority);
		}

		public override void Update (float dt)
		{
			CCLog.Log (printString);
		}
    }
}
