using System.Collections.Generic;
using cocos2d;

namespace tests
{
    public class ConvertToNode : TestCocosNodeDemo
    {
        public ConvertToNode()
        {
            TouchEnabled = true;
            CCSize s = CCDirector.SharedDirector.WinSize;

            CCRotateBy rotate = new CCRotateBy (10, 360);
            CCRepeatForever action = CCRepeatForever.Create(rotate);
            for (int i = 0; i < 3; i++)
            {
                CCSprite sprite = CCSprite.Create("Images/grossini");
                sprite.Position = (new CCPoint(s.Width / 4 * (i + 1), s.Height / 2));

                CCSprite point = CCSprite.Create("Images/r1");
                point.Scale = 0.25f;
                point.Position = sprite.Position;
                AddChild(point, 10, 100 + i);

                switch (i)
                {
                    case 0:
                        sprite.AnchorPoint = new CCPoint(0, 0);
                        break;
                    case 1:
                        sprite.AnchorPoint = (new CCPoint(0.5f, 0.5f));
                        break;
                    case 2:
                        sprite.AnchorPoint = (new CCPoint(1, 1));
                        break;
                }

                point.Position = (sprite.Position);

                var copy = (CCRepeatForever) action.Copy();
                sprite.RunAction(copy);
                AddChild(sprite, i);
            }
        }

        public override string title()
        {
            return "Convert To Node Space";
        }

        public override void TouchesEnded(List<CCTouch> touches, CCEvent event_)
        {
            base.TouchesEnded(touches, event_);
        }

        public override string subtitle()
        {
            return "testing convertToNodeSpace / AR. Touch and see console";
        }
    }
}