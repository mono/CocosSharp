using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class ReorderSpriteSheet : AddRemoveSpriteSheet
    {
        public override void Update(float dt)
        {
            //srandom(0);

            // 15 percent
            int totalToAdd = (int)(currentQuantityOfNodes * 0.15f);

            if (totalToAdd > 0)
            {
                List<CCSprite> sprites = new List<CCSprite>();

                // Don't include the sprite creation time as part of the profiling
                for (int i = 0; i < totalToAdd; i++)
                {
                    CCSprite pSprite = new CCSprite(batchNode.Texture, new CCRect(0, 0, 32, 32));
                    sprites.Add(pSprite);
                }

                // add them with random Z (very important!)
                for (int i = 0; i < totalToAdd; i++)
                {
                    batchNode.AddChild((CCNode)(sprites[i]), (int)(CCMacros.CCRandomBetweenNegative1And1() * 50), PerformanceNodeChildrenTest.kTagBase + i);
                }

                //		[batchNode sortAllChildren];

                // reorder them
                //#if CC_ENABLE_PROFILERS
                //        CCProfilingBeginTimingBlock(_profilingTimer);
                //#endif

                for (int i = 0; i < totalToAdd; i++)
                {
                    CCNode node = (CCNode)(batchNode.Children[i]);
                    batchNode.ReorderChild(node, (int)(CCMacros.CCRandomBetweenNegative1And1() * 50));
                }

                //#if CC_ENABLE_PROFILERS
                //        CCProfilingEndTimingBlock(_profilingTimer);
                //#endif

                // remove them
                for (int i = 0; i < totalToAdd; i++)
                {
                    batchNode.RemoveChildByTag(PerformanceNodeChildrenTest.kTagBase + i, true);
                }
            }
        }

        public override string title()
        {
            return "E - Reorder from spritesheet";
        }

        public override string subtitle()
        {
            return "Reorder %10 of total sprites placed randomly. See console";
        }

        public override string profilerName()
        {
            return "reorder sprites";
        }
    }
}
