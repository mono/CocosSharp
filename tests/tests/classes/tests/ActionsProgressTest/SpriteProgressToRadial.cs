using cocos2d;

namespace tests
{
    public class SpriteProgressToRadial : SpriteDemo
    {
        private string s_pPathBlock = "Images/blocks";
        private string s_pPathSister1 = "Images/grossinis_sister1";

        public override void OnEnter()
        {
            base.OnEnter();

            CCSize s = CCDirector.SharedDirector.WinSize;

            CCProgressTo to1 = CCProgressTo.Create(2, 100);
            CCProgressTo to2 = CCProgressTo.Create(2, 100);

            CCProgressTimer left = CCProgressTimer.Create(s_pPathSister1);
            left.Type = CCProgressTimerType.Radial;
            AddChild(left);
            left.Position = new CCPoint(100, s.height / 2);
            left.RunAction(CCRepeatForever.Create(to1));

            CCProgressTimer right = CCProgressTimer.Create(s_pPathBlock);
            right.Type = CCProgressTimerType.Radial;
            right.ReverseProgress = true;
            AddChild(right);
            right.Position = new CCPoint(s.width - 100, s.height / 2);
            right.RunAction(CCRepeatForever.Create(to2));
        }

        public override string subtitle()
        {
            return "ProgressTo Radial";
        }
    }
}