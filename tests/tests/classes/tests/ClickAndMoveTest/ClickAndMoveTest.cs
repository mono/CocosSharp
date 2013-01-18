using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class ClickAndMoveTest : TestScene
    {
        public static int kTagSprite = 1;
        public static string s_pPathGrossini = "Images/grossini";
        public override void runThisTest()
        {
            CCLayer pLayer = new MainLayer();
            //pLayer->autorelease();

            AddChild(pLayer);
            CCDirector.SharedDirector.ReplaceScene(this);
        }

        protected override void NextTestCase()
        {
        }
        protected override void PreviousTestCase()
        {
        }
        protected override void RestTestCase()
        {
        }
    }

    public class MainLayer : CCLayer
    {
        public MainLayer()
        {
            base.TouchEnabled = true;

            CCSprite sprite = CCSprite.Create(ClickAndMoveTest.s_pPathGrossini);

            CCLayer layer = CCLayerColor.Create(new ccColor4B(255, 255, 0, 255));
            AddChild(layer, -1);

            AddChild(sprite, 0, ClickAndMoveTest.kTagSprite);
            sprite.Position = new CCPoint(20, 150);

            sprite.RunAction(CCJumpTo.Create(4, new CCPoint(300, 48), 100, 4));

            layer.RunAction(CCRepeatForever.Create(
                                                                (CCActionInterval)(CCSequence.Create(
                                                                                    CCFadeIn.Create(1),
                                                                                    CCFadeOut.Create(1)))
                                                                ));
        }

        public override void TouchesEnded(List<CCTouch> touches, CCEvent event_)
        {
            //base.ccTouchesEnded(touches, event_);
            object it = touches.First();
            CCTouch touch = (CCTouch)(it);

            CCPoint location = touch.LocationInView;
            CCPoint convertedLocation = CCDirector.SharedDirector.ConvertToGl(location);

            CCNode s = GetChildByTag(ClickAndMoveTest.kTagSprite);
            s.StopAllActions();
            s.RunAction(CCMoveTo.Create(1, new CCPoint(convertedLocation.x, convertedLocation.y)));
            float o = convertedLocation.x - s.Position.x;
            float a = convertedLocation.y - s.Position.y;
            float at = (float)(Math.Atan(o / a) * 57.29577951f);

            if (a < 0)
            {
                if (o < 0)
                    at = 180 + Math.Abs(at);
                else
                    at = 180 - Math.Abs(at);
            }

            s.RunAction(CCRotateTo.Create(1, at));
        }
    }
}
