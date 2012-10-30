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

            CCProgressTo action = CCProgressTo.Create(2, 100);

            /**
           *  Our image on the left should be a radial progress indicator, clockwise
           */
            CCProgressTimer left = CCProgressTimer.Create(CCSprite.Create(s_pPathBlock));
            left.Type = CCProgressTimerType.Radial;
            AddChild(left);
            left.Midpoint = new CCPoint(0.25f, 0.75f);
            left.Position = new CCPoint(100, s.height / 2);
            left.RunAction(CCRepeatForever.Create((CCActionInterval) action.Copy()));

            /**
           *  Our image on the left should be a radial progress indicator, counter clockwise
           */
            CCProgressTimer right = CCProgressTimer.Create(CCSprite.Create(s_pPathBlock));
            right.Type = CCProgressTimerType.Radial;
            right.Midpoint = new CCPoint(0.75f, 0.25f);

            /**
           *  Note the reverse property (default=NO) is only added to the right image. That's how
           *  we get a counter clockwise progress.
           */
            AddChild(right);
            right.Position = new CCPoint(s.width - 100, s.height / 2);
            right.RunAction(CCRepeatForever.Create((CCActionInterval) action.Copy()));
        }

        public override string subtitle()
        {
            return "Radial w/ Different Midpoints";
        }
    }
}