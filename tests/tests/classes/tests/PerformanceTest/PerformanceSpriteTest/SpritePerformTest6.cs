using cocos2d;

namespace tests
{
    public class SpritePerformTest6 : SpriteMainScene
    {
        public override void doTest(CCSprite sprite)
        {
            performanceActions(sprite);
        }

        public override string title()
        {
            return string.Format("F {0} actions", subtestNumber);
        }

        private void performanceActions(CCSprite pSprite)
        {
            CCSize size = CCDirector.SharedDirector.WinSize;
            pSprite.Position = new CCPoint((Random.Next() % (int) size.width), (Random.Next() % (int) size.height));

            float period = 0.5f + (Random.Next() % 1000) / 500.0f;
            CCRotateBy rot = CCRotateBy.Create(period, 360.0f * Random.Float_0_1());
            var rot_back = rot.Reverse();
            CCAction permanentRotation = CCRepeatForever.Create(CCSequence.Create(rot, rot_back));
            pSprite.RunAction(permanentRotation);

            float growDuration = 0.5f + (Random.Next() % 1000) / 500.0f;
            CCActionInterval grow = CCScaleBy.Create(growDuration, 0.5f, 0.5f);
            CCAction permanentScaleLoop = CCRepeatForever.Create(CCSequence.ActionOneTwo(grow, grow.Reverse()));
            pSprite.RunAction(permanentScaleLoop);
        }
    }
}