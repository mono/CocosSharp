using Cocos2D;

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
            pSprite.Position = new CCPoint((Random.Next() % (int) size.Width), (Random.Next() % (int) size.Height));

            float period = 0.5f + (Random.Next() % 1000) / 500.0f;
            CCRotateBy rot = new CCRotateBy (period, 360.0f * Random.Float_0_1());
            var rot_back = rot.Reverse();
            CCAction permanentRotation = new CCRepeatForever (CCSequence.FromActions(rot, rot_back));
            pSprite.RunAction(permanentRotation);

            float growDuration = 0.5f + (Random.Next() % 1000) / 500.0f;
            CCActionInterval grow = new CCScaleBy(growDuration, 0.5f, 0.5f);
            CCAction permanentScaleLoop = new CCRepeatForever (new CCSequence (grow, grow.Reverse()));
            pSprite.RunAction(permanentScaleLoop);
        }
    }
}