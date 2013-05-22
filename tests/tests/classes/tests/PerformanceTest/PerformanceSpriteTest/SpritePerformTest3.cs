using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cocos2D;
using Random = Cocos2D.CCRandom;

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
            pSprite.Position = new CCPoint((CCRandom.Next() % (int)size.Width), (CCRandom.Next() % (int)size.Height));
            pSprite.Rotation = CCRandom.Float_0_1() * 360;
            pSprite.Scale = CCRandom.Float_0_1() * 2;
        }
    }
}
