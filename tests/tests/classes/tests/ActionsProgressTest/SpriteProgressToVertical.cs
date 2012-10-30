using cocos2d;

namespace tests
{
    public class SpriteProgressToVertical : SpriteDemo
    {
        private string s_pPathSister1 = "Images/grossinis_sister1";
        private string s_pPathSister2 = "Images/grossinis_sister2";

        public override void OnEnter()
        {
            base.OnEnter();

            CCSize s = CCDirector.SharedDirector.WinSize;

            CCProgressTo to1 = CCProgressTo.Create(2, 100);
            CCProgressTo to2 = CCProgressTo.Create(2, 100);

            CCProgressTimer left = CCProgressTimer.Create(s_pPathSister1);
            left.Type = CCProgressTimerType.Bar;
            //    Setup for a bar starting from the left since the midpoint is 0 for the y
            left.Midpoint = new CCPoint(0, 0);
            //    Setup for a horizontal bar since the bar change rate is 0 for y meaning no horizontaly change
            left.BarChangeRate = new CCPoint(0, 1);
            AddChild(left);
            left.Position = new CCPoint(100, s.height / 2);
            left.RunAction(CCRepeatForever.Create(to1));

            CCProgressTimer right = CCProgressTimer.Create(s_pPathSister2);
            right.Type = CCProgressTimerType.Bar;
            //    Setup for a bar starting from the left since the midpoint is 0 for the y
            right.Midpoint = new CCPoint(0, 1);
            //    Setup for a horizontal bar since the bar change rate is 0 for y meaning no horizontaly change
            right.BarChangeRate = new CCPoint(0, 1);
            AddChild(right);
            right.Position = new CCPoint(s.width - 100, s.height / 2);
            right.RunAction(CCRepeatForever.Create(to2));
        }

        public override string subtitle()
        {
            return "ProgressTo Vertical";
        }
    }
}