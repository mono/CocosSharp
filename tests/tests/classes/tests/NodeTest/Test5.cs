using CocosSharp;

namespace tests
{
    public class Test5 : TestCocosNodeDemo
    {
        private CCSprite sp2;
        private CCAction forever2;

        public Test5()
        {
            CCSprite sp1 = new CCSprite(TestResource.s_pPathSister1);
            sp2 = new CCSprite(TestResource.s_pPathSister2);

            sp1.Position = (new CCPoint(100, 160));
            sp2.Position = (new CCPoint(380, 160));

			var rot = new CCRotateBy (2, 360);
            var rot_back = rot.Reverse();

			var forever = new CCRepeatForever (rot, rot_back) { Tag = 101 };

			// Since Actions are immutable to set the tag differently we need to 
			// create a new action.  Notice that the same actions can be used in
			// this case instead of copying them as well.
			forever2 = new CCRepeatForever (rot, rot_back) { Tag = 102 };
            
            AddChild(sp1, 0, CocosNodeTestStaticLibrary.kTagSprite1);
            AddChild(sp2, 0, CocosNodeTestStaticLibrary.kTagSprite2);

            RemoveChild(sp2, true);
            AddChild(sp2, 0, CocosNodeTestStaticLibrary.kTagSprite2);

            // Sprite 1 should run and run
            // Sprite 2 should stop
            sp1.RunAction(forever);
            sp2.RunAction(forever2);

            // Experiment with removing sp2 and re-adding it after cleanup to reproduce an error in child management
			//ScheduleOnce(Stage2OfTest, 2.0f);
			Schedule(addAndRemove, 2.0f);
        }

        public void Stage2OfTest(float dt)
        {
            //initialization Cleanup() errors
            CCLog.Log("Node test #5, stage 2, remove right side sprite and re-add it");
            RemoveChild(sp2, true);
            AddChild(sp2, 0, CocosNodeTestStaticLibrary.kTagSprite2);
            sp2.RunAction(forever2);
            Schedule(addAndRemove, 2.0f);
        }

        public void addAndRemove(float dt)
        {
            CCNode sp1 = GetChildByTag(CocosNodeTestStaticLibrary.kTagSprite1);
            CCNode sp2 = GetChildByTag(CocosNodeTestStaticLibrary.kTagSprite2);

            RemoveChild(sp1, false);
            RemoveChild(sp2, true);

            AddChild(sp1, 0, CocosNodeTestStaticLibrary.kTagSprite1);
            AddChild(sp2, 0, CocosNodeTestStaticLibrary.kTagSprite2);
        }

        public override string subtitle()
        {
            return ("test #5");
        }

        public override string title()
        {
            return "remove and cleanup";
        }
    }
}