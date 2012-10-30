using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class IterateSpriteSheetFastEnum : IterateSpriteSheet
    {
        public override void Update(float dt)
        {
            // iterate using fast enumeration protocol
            var pChildren = batchNode.Children;

            //#if CC_ENABLE_PROFILERS
            //    CCProfilingBeginTimingBlock(_profilingTimer);
            //#endif

            foreach (var pObject in pChildren)
            {
                CCSprite pSprite = (CCSprite)pObject;
                pSprite.Visible = false;
            }

            //#if CC_ENABLE_PROFILERS
            //    CCProfilingEndTimingBlock(_profilingTimer);
            //#endif
        }

        public override string title()
        {
            return "A - Iterate SpriteSheet";
        }

        public override string subtitle()
        {
            return "Iterate children using Fast Enum API. See console";
        }

        public override string profilerName()
        {
            return "iter fast enum";
        }
    }
}
