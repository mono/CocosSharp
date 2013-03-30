using cocos2d;

namespace tests
{
    public class Test4 : TestCocosNodeDemo
    {
        public Test4()
        {
            CCSprite sp1 = CCSprite.Create(TestResource.s_pPathSister1);
            CCSprite sp2 = CCSprite.Create(TestResource.s_pPathSister2);

            sp1.Position = (new CCPoint(100, 160));
            sp2.Position = (new CCPoint(380, 160));

            AddChild(sp1, 0, 2);
            AddChild(sp2, 0, 3);

            Schedule(delay2, 2.0f);
            Schedule(delay4, 4.0f);
        }

        public void delay2(float dt)
        {
            var node = (CCSprite) (GetChildByTag(2));
            CCAction action1 = new CCRotateBy (1, 360);
            node.RunAction(action1);
        }

        public void delay4(float dt)
        {
            Unschedule(delay4);
            RemoveChildByTag(3, false);
        }

        public override string title()
        {
            return "tags";
        }
    }
}