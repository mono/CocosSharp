using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
    public class IterateSpriteSheetFastEnum : IterateSpriteSheet
    {
        bool bDone = false;
        public override void Update(float dt)
        {
            if (bDone) return;
            // iterate using fast enumeration protocol
            var pChildren = batchNode.Children;

            //#if CC_ENABLE_PROFILERS
            //    CCProfilingBeginTimingBlock(_profilingTimer);
            //#endif

            StartTimer();
            foreach (var pObject in pChildren)
            {
                CCSprite pSprite = (CCSprite)pObject;
                pSprite.Visible = false;
            }
            EndTimer("Set visible array walk");

            //#if CC_ENABLE_PROFILERS
            //    CCProfilingEndTimingBlock(_profilingTimer);
            //#endif
            bDone = true;
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
