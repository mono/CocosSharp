using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
    public class RemoveSpriteSheet : AddRemoveSpriteSheet
    {

        bool bDone = false;
        public override void Update(float dt)
        {

            if (bDone) return;
            //srandom(0);

            // 15 percent
            int totalToAdd = (int)(currentQuantityOfNodes * 0.15f);

            if (totalToAdd > 0)
            {
                List<CCSprite> sprites = new List<CCSprite>();

                // Don't include the sprite creation time as part of the profiling
                StartTimer();
                for (int i = 0; i < totalToAdd; i++)
                {
                    CCSprite pSprite = new CCSprite(batchNode.Texture, new CCRect(0, 0, 32, 32));
                    sprites.Add(pSprite);
                }
                EndTimer("Add " + totalToAdd + " sprites");

                // add them with random Z (very important!)
                StartTimer();
                for (int i = 0; i < totalToAdd; i++)
                {
                    batchNode.AddChild((CCNode)(sprites[i]), (int)(CCMacros.CCRandomBetweenNegative1And1() * 50), PerformanceNodeChildrenTest.kTagBase + i);
                }
                EndTimer("Add sprites to a batch node");

                // remove them
                //#if CC_ENABLE_PROFILERS
                //        CCProfilingBeginTimingBlock(_profilingTimer);
                //#endif

                StartTimer();
                for (int i = 0; i < totalToAdd; i++)
                {
                    batchNode.RemoveChildByTag(PerformanceNodeChildrenTest.kTagBase + i, true);
                }
                EndTimer("Remove children by tag from the batch node");

                //#if CC_ENABLE_PROFILERS
                //        CCProfilingEndTimingBlock(_profilingTimer);
                //#endif
            }
            bDone = true;
        }

        public override string title()
        {
            return "D - Del from spritesheet";
        }

        public override string subtitle()
        {
            return "Remove %10 of total sprites placed randomly. See console";
        }

        public override string profilerName()
        {
            return "remove sprites";
        }
    }
}
