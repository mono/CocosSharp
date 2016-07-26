using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;
using Random = CocosSharp.CCRandom;

namespace tests
{
    public class SpritePerformTest2 : SpriteMainScene
    {

        public override void doTest(CCSprite sprite)
        {
            performanceScale(sprite);
        }
        public override string title()
        {
            return string.Format("B {0} scale", subtestNumber);
        }

        private void performanceScale(CCSprite pSprite)
        {
            CCSize size = Layer.VisibleBoundsWorldspace.Size;
            pSprite.Position = new CCPoint((CCRandom.Next() % (int)size.Width), (CCRandom.Next() % (int)size.Height));
            pSprite.Scale = CCRandom.Float_0_1() * 100 / 50;
        }
    }
}
