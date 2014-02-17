using CocosSharp;

namespace tests
{
    internal class SpriteProgressWithSpriteFrame : SpriteDemo
    {
        public override void OnEnter()
        {
            base.OnEnter();

            CCSize s = CCDirector.SharedDirector.WinSize;

			var progressTo = new CCProgressTo(6, 100);

            CCSpriteFrameCache.SharedSpriteFrameCache.AddSpriteFramesWithFile("zwoptex/grossini.plist");

            CCProgressTimer left = new CCProgressTimer(new CCSprite("grossini_dance_01.png"));
            left.Type = CCProgressTimerType.Bar;
            //    Setup for a bar starting from the bottom since the midpoint is 0 for the y
            left.Midpoint = new CCPoint(0.5f, 0.5f);
            //    Setup for a vertical bar since the bar change rate is 0 for x meaning no horizontal change
            left.BarChangeRate = new CCPoint(1, 0);
            AddChild(left);
            left.Position = new CCPoint(100, s.Height / 2);
			left.RepeatForever(progressTo);

            CCProgressTimer middle = new CCProgressTimer(new CCSprite("grossini_dance_02.png"));
            middle.Type = CCProgressTimerType.Bar;
            //    Setup for a bar starting from the bottom since the midpoint is 0 for the y
            middle.Midpoint = new CCPoint(0.5f, 0.5f);
            //    Setup for a vertical bar since the bar change rate is 0 for x meaning no horizontal change
            middle.BarChangeRate = new CCPoint(1, 1);
            AddChild(middle);
            middle.Position = new CCPoint(s.Width / 2, s.Height / 2);
			middle.RepeatForever(progressTo);

            CCProgressTimer right = new CCProgressTimer(new CCSprite("grossini_dance_03.png"));
            right.Type = CCProgressTimerType.Radial;
            //    Setup for a bar starting from the bottom since the midpoint is 0 for the y
            right.Midpoint = new CCPoint(0.5f, 0.5f);
            //    Setup for a vertical bar since the bar change rate is 0 for x meaning no horizontal change
            right.BarChangeRate = new CCPoint(0, 1);
            AddChild(right);
            right.Position = new CCPoint(s.Width - 100, s.Height / 2);
			right.RepeatForever(progressTo);
        }

        public override string subtitle()
        {
            return "Progress With Sprite Frame";
        }
    }
}