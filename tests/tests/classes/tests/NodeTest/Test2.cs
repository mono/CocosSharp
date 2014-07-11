using CocosSharp;

namespace tests
{
    public class Test2 : TestCocosNodeDemo
    {

        public override void OnEnter()
        {
            base.OnEnter();

            CCSize s = Scene.VisibleBoundsWorldspace.Size;

			var sp1 = new CCSprite(TestResource.s_pPathSister1);
			var sp2 = new CCSprite(TestResource.s_pPathSister2);
			var sp3 = new CCSprite(TestResource.s_pPathSister1);
			var sp4 = new CCSprite(TestResource.s_pPathSister2);

			sp1.Position = (new CCPoint(100, s.Height / 2));
			sp2.Position = (new CCPoint(380, s.Height / 2));

            AddChild(sp1);
            AddChild(sp2);

            sp3.Scale = (0.25f);
            sp4.Scale = (0.25f);

            sp1.AddChild(sp3);
            sp2.AddChild(sp4);

			var a1 = new CCRotateBy (2, 360);
			var a2 = new CCScaleBy(2, 2);

			var action1 = new CCRepeatForever (a1, a2, a2.Reverse());

            sp2.AnchorPoint = (new CCPoint(0, 0));

            sp1.RunAction(action1);
			sp2.RunAction(action1);
        }

        public override string title()
        {
            return "anchorPoint and children";
        }
    }
}