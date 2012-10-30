using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;
using Random = cocos2d.Random;

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
            CCSize size = CCDirector.SharedDirector.WinSize;
            pSprite.Position = new CCPoint((Random.Next() % (int)size.width), (Random.Next() % (int)size.height));
            pSprite.Scale = Random.Float_0_1() * 100 / 50;
        }
    }
}
