using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class Sprite1 : SpriteTestDemo
    {
        public Sprite1()
        {
            TouchEnabled = true;

            CCSize s = CCDirector.SharedDirector.WinSize;
            addNewSpriteWithCoords(new CCPoint(s.Width / 2, s.Height / 2));
        }

        public void addNewSpriteWithCoords(CCPoint p)
        {
            int idx = (int)(CCMacros.CCRandomBetween0And1() * 1400.0f / 100.0f);
            int x = (idx % 5) * 85;
            int y = (idx / 5) * 121;

            CCSprite sprite = CCSprite.Create("Images/grossini_dance_atlas", new CCRect(x, y, 85, 121));
            AddChild(sprite);

            sprite.Position = p;

            CCActionInterval action;
            float random = CCMacros.CCRandomBetween0And1();

            if (random < 0.20)
                action = CCScaleBy.Create(3, 2);
            else if (random < 0.40)
                action = new CCRotateBy (3, 360);
            else if (random < 0.60)
                action = new CCBlink (1, 3);
            else if (random < 0.8)
                action = CCTintBy.Create(2, 0, -255, -255);
            else
                action = CCFadeOut.Create(2);
            object obj = action.Reverse();
            CCActionInterval action_back = (CCActionInterval)action.Reverse();
            CCActionInterval seq = (CCActionInterval)(CCSequence.Create(action, action_back));

            sprite.RunAction(CCRepeatForever.Create(seq));
        }

        public override string title()
        {
            return "Sprite (tap screen)";
        }


        public override void TouchesEnded(List<CCTouch> touches, CCEvent eventArgs)
        {
            foreach (CCTouch touch in touches)
            {
                CCPoint location = touch.LocationInView;
                location = CCDirector.SharedDirector.ConvertToGl(location);
                addNewSpriteWithCoords(location);
            }
        }
    }
}
