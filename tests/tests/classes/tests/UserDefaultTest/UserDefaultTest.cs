using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;
using System.Diagnostics;

namespace tests
{
    public class UserDefaultTest : CCLayer
    {
        public UserDefaultTest() { }
        private void doTest()
        {
            CCLog.Log("********************** init value ***********************");

            // set default value

            //CCUserDefault.sharedUserDefault().setStringForKey("string", "value1");
            //CCUserDefault.sharedUserDefault().setIntegerForKey("integer", 10);
            //CCUserDefault.sharedUserDefault().setFloatForKey("float", 2.3f);
            //CCUserDefault.sharedUserDefault().setDoubleForKey("double", 2.4);
            //CCUserDefault.sharedUserDefault().setBoolForKey("bool", true);

            // print value

        //    string ret = CCUserDefault.sharedUserDefault()->getStringForKey("string");
        //    CCLog.Log("string is {0}", ret);

        //    double d = CCUserDefault.sharedUserDefault()->getDoubleForKey("double");
        //    CCLog.Log("double is {0}", d);

        //    int i = CCUserDefault.sharedUserDefault()->getIntegerForKey("integer");
        //    CCLog.Log("integer is {0}", i);

        //    float f = CCUserDefault.sharedUserDefault()->getFloatForKey("float");
        //    CCLog.Log("float is {0}", f);

        //    bool b = CCUserDefault.sharedUserDefault()->getBoolForKey("bool");
        //    if (b)
        //    {
        //        CCLog.Log("bool is true");
        //    }
        //    else
        //    {
        //        CCLog.Log("bool is false");
        //    }

        //    CCLog.Log("********************** after change value ***********************");

        //    // change the value

        //    CCUserDefault.sharedUserDefault().setStringForKey("string", "value2");
        //    CCUserDefault.sharedUserDefault().setIntegerForKey("integer", 11);
        //    CCUserDefault.sharedUserDefault().setFloatForKey("float", 2.5f);
        //    CCUserDefault.sharedUserDefault().setDoubleForKey("double", 2.6);
        //    CCUserDefault.sharedUserDefault().setBoolForKey("bool", false);

        //    // print value

        //    ret = CCUserDefault.sharedUserDefault().getStringForKey("string");
        //    CCLog.Log("string is %s", ret);

        //    d = CCUserDefault.sharedUserDefault().getDoubleForKey("double");
        //    CCLog.Log("double is %f", d);

        //    i = CCUserDefault.sharedUserDefault().getIntegerForKey("integer");
        //    CCLog.Log("integer is %d", i);

        //    f = CCUserDefault.sharedUserDefault().getFloatForKey("float");
        //    CCLog.Log("float is %f", f);

        //    b = CCUserDefault.sharedUserDefault().getBoolForKey("bool");
        //    if (b)
        //    {
        //        CCLog.Log("bool is true");
        //    }
        //    else
        //    {
        //        CCLog.Log("bool is false");
        //    }
        }
    }
}
