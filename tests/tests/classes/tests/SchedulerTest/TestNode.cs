using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class TestNode : CCNode
    {
        public void initWithString(string pStr, int priority)
        {
            m_pstring = pStr;
            ScheduleUpdateWithPriority(priority);
        }

        private string m_pstring;
    }
}
