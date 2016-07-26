using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
    public class SpritePerformTest4 : SpriteMainScene
    {

        public override void doTest(CCSprite sprite)
        {
            performanceOut100(sprite);
        }

        public override string title()
        {
            return string.Format("D {0} 100% out", subtestNumber);
        }

        private void performanceOut100(CCSprite pSprite)
        {
            pSprite.Position = new CCPoint(-1000, -1000);
        }
    }
}
