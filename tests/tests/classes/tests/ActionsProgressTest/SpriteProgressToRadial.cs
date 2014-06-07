using CocosSharp;

namespace tests
{
    public class SpriteProgressToRadial : SpriteDemo
    {
        private string s_pPathBlock = "Images/blocks";
        private string s_pPathSister1 = "Images/grossinis_sister1";

        public override void OnEnter()
        {
            base.OnEnter();

            CCSize s = CCApplication.SharedApplication.MainWindowDirector.WinSize;

			var progressTo = new CCProgressTo(2, 100);

            CCProgressTimer left = new CCProgressTimer(s_pPathSister1);
            left.Type = CCProgressTimerType.Radial;
            AddChild(left);
            left.Position = new CCPoint(100, s.Height / 2);
			left.RepeatForever(progressTo);

            CCProgressTimer right = new CCProgressTimer(s_pPathBlock);
            right.Type = CCProgressTimerType.Radial;
            right.ReverseDirection = true;
            AddChild(right);
            right.Position = new CCPoint(s.Width - 100, s.Height / 2);
			right.RepeatForever(progressTo);
        }

        public override string subtitle()
        {
            return "ProgressTo Radial";
        }
    }
}