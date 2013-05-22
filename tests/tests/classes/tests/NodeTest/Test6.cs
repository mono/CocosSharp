using Cocos2D;

namespace tests
{
    public class Test6 : TestCocosNodeDemo
    {
        public Test6()
        {
            CCSprite sp1 = new CCSprite(TestResource.s_pPathSister1);
            CCSprite sp11 = new CCSprite(TestResource.s_pPathSister1);

            CCSprite sp2 = new CCSprite(TestResource.s_pPathSister2);
            CCSprite sp21 = new CCSprite(TestResource.s_pPathSister2);

            sp1.Position = (new CCPoint(100, 160));
            sp2.Position = (new CCPoint(380, 160));

            CCActionInterval rot = new CCRotateBy (2, 360);
            var rot_back = rot.Reverse() as CCActionInterval;
            CCAction forever1 = new CCRepeatForever ((CCActionInterval)CCSequence.FromActions(rot, rot_back));
            var forever11 = (CCAction) (forever1.Copy());

            var forever2 = (CCAction) (forever1.Copy());
            var forever21 = (CCAction) (forever1.Copy());

            AddChild(sp1, 0, CocosNodeTestStaticLibrary.kTagSprite1);
            sp1.AddChild(sp11);
            AddChild(sp2, 0, CocosNodeTestStaticLibrary.kTagSprite2);
            sp2.AddChild(sp21);

            sp1.RunAction(forever1);
            sp11.RunAction(forever11);
            sp2.RunAction(forever2);
            sp21.RunAction(forever21);

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
            return "remove/cleanup with children";
        }
    }
}