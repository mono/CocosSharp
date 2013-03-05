using cocos2d;

namespace tests
{
    public class Test2 : TestCocosNodeDemo
    {
        public override void OnEnter()
        {
            base.OnEnter();

            CCSize s = CCDirector.SharedDirector.WinSize;

            CCSprite sp1 = CCSprite.Create(TestResource.s_pPathSister1);
            CCSprite sp2 = CCSprite.Create(TestResource.s_pPathSister2);
            CCSprite sp3 = CCSprite.Create(TestResource.s_pPathSister1);
            CCSprite sp4 = CCSprite.Create(TestResource.s_pPathSister2);

            sp1.Position = (new CCPoint(100, s.Height / 2));
            sp2.Position = (new CCPoint(380, s.Height / 2));
            AddChild(sp1);
            AddChild(sp2);

            sp3.Scale = (0.25f);
            sp4.Scale = (0.25f);

            sp1.AddChild(sp3);
            sp2.AddChild(sp4);

            CCActionInterval a1 = CCRotateBy.Create(2, 360);
            CCActionInterval a2 = CCScaleBy.Create(2, 2);

            CCAction action1 = CCRepeatForever.Create((CCActionInterval)CCSequence.Create(a1, a2, a2.Reverse())
                );
            CCAction action2 = CCRepeatForever.Create((CCActionInterval)
                (CCSequence.Create(
                    (CCActionInterval) (a1.Copy()),
                    (CCActionInterval) (a2.Copy()),
                    a2.Reverse()))
                );

            sp2.AnchorPoint = (new CCPoint(0, 0));

            sp1.RunAction(action1);
            sp2.RunAction(action2);
        }

        public override string title()
        {
            return "anchorPoint and children";
        }
    }
}