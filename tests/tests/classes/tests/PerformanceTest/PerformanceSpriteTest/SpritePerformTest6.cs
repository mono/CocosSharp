using CocosSharp;

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
            CCSize size = Layer.VisibleBoundsWorldspace.Size;
            pSprite.Position = new CCPoint((CCRandom.Next() % (int) size.Width), (CCRandom.Next() % (int) size.Height));

            float period = 0.5f + (CCRandom.Next() % 1000) / 500.0f;
            CCRotateBy rot = new CCRotateBy (period, 360.0f * CCRandom.Float_0_1());
            var rot_back = rot.Reverse();
            CCAction permanentRotation = new CCRepeatForever (new CCSequence(rot, rot_back));
            pSprite.RunAction(permanentRotation);

            float growDuration = 0.5f + (CCRandom.Next() % 1000) / 500.0f;
            CCFiniteTimeAction grow = new CCScaleBy(growDuration, 0.5f, 0.5f);
            CCAction permanentScaleLoop = new CCRepeatForever (new CCSequence (grow, grow.Reverse()));
            pSprite.RunAction(permanentScaleLoop);
        }
    }
}