using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Cocos2D;

namespace tests
{
    public class AddSpriteSheet : AddRemoveSpriteSheet
    {
        private bool bTested = false;

        public override void Update(float dt)
        {
            // reset seed
            //srandom(0);
            if (bTested)
            {
                return;
            }
            // 15 percent
            int totalToAdd = (int)(currentQuantityOfNodes * 0.15f);

            if (totalToAdd > 0)
            {
                List<CCSprite> sprites = new List<CCSprite>(totalToAdd);
                //int		 zs      = new int[totalToAdd];
                int[] zs = new int[totalToAdd];

                // Don't include the sprite creation time and random as part of the profiling
                long lStart = DateTime.Now.Ticks;
                for (int i = 0; i < totalToAdd; i++)
                {
                    CCSprite pSprite = new CCSprite(batchNode.Texture, new CCRect(0, 0, 32, 32));
                    sprites.Add(pSprite);
                    zs[i] = (int)(CCMacros.CCRandomBetweenNegative1And1() * 50);

                }
                long ldiff = DateTime.Now.Ticks - lStart;
                CCLog.Log("Add Sprite took {0} sec", (new TimeSpan(ldiff)).TotalSeconds);

                // add them with random Z (very important!)
                //#if CC_ENABLE_PROFILERS
                //        CCProfilingBeginTimingBlock(_profilingTimer);
                //#endif

                lStart = DateTime.Now.Ticks;
                for (int i = 0; i < totalToAdd; i++)
                {
                    batchNode.AddChild((CCNode)(sprites[i]), zs[i], PerformanceNodeChildrenTest.kTagBase + i);
                }
                ldiff = DateTime.Now.Ticks - lStart;
                CCLog.Log("Add Child to Batch took {0} sec", (new TimeSpan(ldiff)).TotalSeconds);

                //#if CC_ENABLE_PROFILERS
                //        CCProfilingEndTimingBlock(_profilingTimer);
                //#endif

                // remove them
                lStart = DateTime.Now.Ticks;
                for (int i = 0; i < totalToAdd; i++)
                {
                    batchNode.RemoveChildByTag(PerformanceNodeChildrenTest.kTagBase + i, true);
                }
                ldiff = DateTime.Now.Ticks - lStart;
                CCLog.Log("Remove Child took {0} sec", (new TimeSpan(ldiff)).TotalSeconds);

            }
            bTested = true;
        }

        public override string title()
        {
            return "C - Add to spritesheet";
        }

        public override string subtitle()
        {
            return "Adds %10 of total sprites with random z. See console";
        }

        public override string profilerName()
        {
            return "add sprites";
        }
    }
}
