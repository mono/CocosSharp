using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;
using Random = CocosSharp.CCRandom;

namespace tests
{
    public class SpritePerformTest5 : SpriteMainScene
    {
        public override void doTest(CCSprite sprite)
        {
            performanceout20(sprite);
        }

        public override string title()
        {
            return string.Format("E {0} 80% out", subtestNumber);
        }

        private void performanceout20(CCSprite pSprite)
        {
            CCSize size = CCApplication.SharedApplication.MainWindowDirector.WindowSizeInPoints;

            if (CCRandom.Float_0_1() < 0.2f)
                pSprite.Position = new CCPoint((CCRandom.Next() % (int)size.Width), (CCRandom.Next() % (int)size.Height));
            else
                pSprite.Position = new CCPoint(-1000, -1000);
        }
    }
}
