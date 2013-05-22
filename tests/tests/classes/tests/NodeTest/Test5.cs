using Cocos2D;

namespace tests
{
    public class Test5 : TestCocosNodeDemo
    {
        public Test5()
        {
            CCSprite sp1 = new CCSprite(TestResource.s_pPathSister1);
            CCSprite sp2 = new CCSprite(TestResource.s_pPathSister2);

            sp1.Position = (new CCPoint(100, 160));
            sp2.Position = (new CCPoint(380, 160));

            CCRotateBy rot = new CCRotateBy (2, 360);
            var rot_back = rot.Reverse() as CCActionInterval;
            CCAction forever = new CCRepeatForever ((CCActionInterval)CCSequence.FromActions(rot, rot_back));
            var forever2 = (CCAction) (forever.Copy());
            forever.Tag = (101);
            forever2.Tag = (102);

            AddChild(sp1, 0, CocosNodeTestStaticLibrary.kTagSprite1);
            AddChild(sp2, 0, CocosNodeTestStaticLibrary.kTagSprite2);

            sp1.RunAction(forever);
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

        public override string title()
        {
            return "remove and cleanup";
        }
    }
}