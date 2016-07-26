using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
    public class IterateSpriteSheetCArray : IterateSpriteSheet
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
            EndTimer("Visible change array walk");

            //#if CC_ENABLE_PROFILERS
            //    CCProfilingEndTimingBlock(_profilingTimer);
            //#endif
            bDone = true;
        }

        public override string title()
        {
            return "B - Iterate SpriteSheet";
        }

        public override string subtitle()
        {
            return "Iterate children using C Array API. See console";
        }

        public override string profilerName()
        {
            return "iter c-array";
        }
    }
}
