using Cocos2D;

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

            CCProgressTo to1 = new CCProgressTo(2, 100);
            CCProgressTo to2 = new CCProgressTo(2, 100);

            CCProgressTimer left = new CCProgressTimer(s_pPathSister1);
            left.Type = CCProgressTimerType.Bar;
            //    Setup for a bar starting from the left since the midpoint is 0 for the y
            left.Midpoint = new CCPoint(0, 0);
            //    Setup for a horizontal bar since the bar change rate is 0 for y meaning no horizontaly change
            left.BarChangeRate = new CCPoint(0, 1);
            AddChild(left);
            left.Position = new CCPoint(100, s.Height / 2);
            left.RunAction(new CCRepeatForever (to1));

            CCProgressTimer right = new CCProgressTimer(s_pPathSister2);
            right.Type = CCProgressTimerType.Bar;
            //    Setup for a bar starting from the left since the midpoint is 0 for the y
            right.Midpoint = new CCPoint(0, 1);
            //    Setup for a horizontal bar since the bar change rate is 0 for y meaning no horizontaly change
            right.BarChangeRate = new CCPoint(0, 1);
            AddChild(right);
            right.Position = new CCPoint(s.Width - 100, s.Height / 2);
            right.RunAction(new CCRepeatForever (to2));
        }

        public override string subtitle()
        {
            return "ProgressTo Vertical";
        }
    }
}