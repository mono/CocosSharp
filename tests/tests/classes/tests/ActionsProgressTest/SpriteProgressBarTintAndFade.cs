using cocos2d;

namespace tests
{
    internal class SpriteProgressBarTintAndFade : SpriteDemo
    {
        private string s_pPathSister1 = "Images/grossinis_sister1";
        private string s_pPathSister2 = "Images/grossinis_sister2";

        public override void OnEnter()
        {
            base.OnEnter();

            CCSize s = CCDirector.SharedDirector.WinSize;

            CCProgressTo to = CCProgressTo.Create(6, 100);
            CCAction tint = CCSequence.Create(new CCTintTo (1, 255, 0, 0),
                                              new CCTintTo (1, 0, 255, 0),
                                              new CCTintTo (1, 0, 0, 255));
            CCAction fade = CCSequence.Create(new CCFadeTo (1.0f, 0),
                                              new CCFadeTo (1.0f, 255));

            CCProgressTimer left = CCProgressTimer.Create(CCSprite.Create(s_pPathSister1));
            left.Type = CCProgressTimerType.Bar;

            //    Setup for a bar starting from the bottom since the midpoint is 0 for the y
            left.Midpoint = new CCPoint(0.5f, 0.5f);
            //    Setup for a vertical bar since the bar change rate is 0 for x meaning no horizontal change
            left.BarChangeRate = new CCPoint(1, 0);
            AddChild(left);
            left.Position = new CCPoint(100, s.Height / 2);
            left.RunAction(CCRepeatForever.Create((CCActionInterval) to.Copy()));
            left.RunAction(CCRepeatForever.Create((CCActionInterval) tint.Copy()));

            left.AddChild(CCLabelTTF.Create("Tint", "arial", 20.0f));

            CCProgressTimer middle = CCProgressTimer.Create(CCSprite.Create(s_pPathSister2));
            middle.Type = CCProgressTimerType.Bar;
            //    Setup for a bar starting from the bottom since the midpoint is 0 for the y
            middle.Midpoint = new CCPoint(0.5f, 0.5f);
            //    Setup for a vertical bar since the bar change rate is 0 for x meaning no horizontal change
            middle.BarChangeRate = new CCPoint(1, 1);
            AddChild(middle);
            middle.Position = new CCPoint(s.Width / 2, s.Height / 2);
            middle.RunAction(CCRepeatForever.Create((CCActionInterval) to.Copy()));
            middle.RunAction(CCRepeatForever.Create((CCActionInterval) fade.Copy()));

            middle.AddChild(CCLabelTTF.Create("Fade", "arial", 20.0f));

            CCProgressTimer right = CCProgressTimer.Create(CCSprite.Create(s_pPathSister2));
            right.Type = CCProgressTimerType.Bar;
            //    Setup for a bar starting from the bottom since the midpoint is 0 for the y
            right.Midpoint = new CCPoint(0.5f, 0.5f);
            //    Setup for a vertical bar since the bar change rate is 0 for x meaning no horizontal change
            right.BarChangeRate = new CCPoint(0, 1);
            AddChild(right);
            right.Position = new CCPoint(s.Width - 100, s.Height / 2);
            right.RunAction(CCRepeatForever.Create((CCActionInterval) to.Copy()));
            right.RunAction(CCRepeatForever.Create((CCActionInterval) tint.Copy()));
            right.RunAction(CCRepeatForever.Create((CCActionInterval) fade.Copy()));

            right.AddChild(CCLabelTTF.Create("Tint and Fade", "arial", 20.0f));
        }

        public override string subtitle()
        {
            return "ProgressTo Tint And Fade";
        }
    }
}