using cocos2d;

namespace tests
{
    public class SpriteProgressToRadialMidpointChanged : SpriteDemo
    {
        private string s_pPathBlock = "Images/blocks";

        public override void OnEnter()
        {
            base.OnEnter();

            CCSize s = CCDirector.SharedDirector.WinSize;

            CCProgressTo action = new CCProgressTo(2, 100);

            /**
           *  Our image on the left should be a radial progress indicator, clockwise
           */
            CCProgressTimer left = CCProgressTimer.Create(new CCSprite(s_pPathBlock));
            left.Type = CCProgressTimerType.Radial;
            AddChild(left);
            left.Midpoint = new CCPoint(0.25f, 0.75f);
            left.Position = new CCPoint(100, s.Height / 2);
            left.RunAction(new CCRepeatForever ((CCActionInterval) action.Copy()));

            /**
           *  Our image on the left should be a radial progress indicator, counter clockwise
           */
            CCProgressTimer right = CCProgressTimer.Create(new CCSprite(s_pPathBlock));
            right.Type = CCProgressTimerType.Radial;
            right.Midpoint = new CCPoint(0.75f, 0.25f);

            /**
           *  Note the reverse property (default=NO) is only added to the right image. That's how
           *  we get a counter clockwise progress.
           */
            AddChild(right);
            right.Position = new CCPoint(s.Width - 100, s.Height / 2);
            right.RunAction(new CCRepeatForever ((CCActionInterval) action.Copy()));
        }

        public override string subtitle()
        {
            return "Radial w/ Different Midpoints";
        }
    }
}