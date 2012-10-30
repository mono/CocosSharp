using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;
using Random = cocos2d.Random;

namespace tests
{
    public class SpritePerformTest3 : SpriteMainScene
    {
        public override void doTest(CCSprite sprite)
        {
            performanceRotationScale(sprite);
        }

        public override string title()
        {
            return string.Format("C {0:D} scale + rot", subtestNumber);
        }

        private void performanceRotationScale(CCSprite pSprite)
        {
            CCSize size = CCDirector.SharedDirector.WinSize;
            pSprite.Position = new CCPoint((Random.Next() % (int)size.width), (Random.Next() % (int)size.height));
            pSprite.Rotation = Random.Float_0_1() * 360;
            pSprite.Scale = Random.Float_0_1() * 2;
        }
    }
}
