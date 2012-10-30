using cocos2d;

namespace tests
{
    public class SchedulerTest1 : TestCocosNodeDemo
    {
        public SchedulerTest1()
        {
            CCLayer layer = CCLayer.Create();
            //UXLOG("retain count after init is %d", layer->retainCount());                // 1

            AddChild(layer, 0);
            //UXLOG("retain count after addChild is %d", layer->retainCount());      // 2

            layer.Schedule((doSomething));
            //UXLOG("retain count after schedule is %d", layer->retainCount());      // 3 : (object-c viersion), but win32 version is still 2, because CCTimer class don't save target.

            layer.Unschedule((doSomething));
            //UXLOG("retain count after unschedule is %d", layer->retainCount());		// STILL 3!  (win32 is '2')
        }

        public void doSomething(float dt)
        {
        }

        public override string title()
        {
            return "cocosnode scheduler test #1";
        }
    }
}