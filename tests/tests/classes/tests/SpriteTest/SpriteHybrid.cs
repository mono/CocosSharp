using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;
using Random = cocos2d.Random;

namespace tests
{
    public class SpriteHybrid : SpriteTestDemo
    {
        bool m_usingSpriteBatchNode;

        public SpriteHybrid()
        {
            CCSize s = CCDirector.SharedDirector.WinSize;

            // parents
            CCNode parent1 = new CCNode ();
            CCSpriteBatchNode parent2 = CCSpriteBatchNode.Create("animations/grossini", 50);

            AddChild(parent1, 0, (int)kTags.kTagNode);
            AddChild(parent2, 0, (int)kTags.kTagSpriteBatchNode);


            // IMPORTANT:
            // The sprite frames will be cached AND RETAINED, and they won't be released unless you call
            //     CCSpriteFrameCache::sharedSpriteFrameCache()->removeUnusedSpriteFrames);
            CCSpriteFrameCache.SharedSpriteFrameCache.AddSpriteFramesWithFile("animations/grossini.plist");


            // create 250 sprites
            // only show 80% of them
            for (int i = 0; i < 250; i++)
            {
                int spriteIdx = (int)(Random.NextDouble() * 14);
                string str = "";
                string temp = "";
                if (spriteIdx+1<10)
                {
                    temp = "0" + (spriteIdx+1);
                }
                else
                {
                    temp = (spriteIdx+1).ToString();
                }
                str = string.Format("grossini_dance_{0}.png", temp);
                CCSpriteFrame frame = CCSpriteFrameCache.SharedSpriteFrameCache.SpriteFrameByName(str);
                CCSprite sprite = CCSprite.Create(frame);
                parent1.AddChild(sprite, i, i);

                float x = -1000;
                float y = -1000;
                if (Random.NextDouble() < 0.2f)
                {
                    x = (float)(Random.NextDouble() * s.Width);
                    y = (float)(Random.NextDouble() * s.Height);
                }
                sprite.Position = (new CCPoint(x, y));

                CCActionInterval action = new CCRotateBy (4, 360);
                sprite.RunAction(new CCRepeatForever (action));
            }

            m_usingSpriteBatchNode = false;

            Schedule(reparentSprite, 2);
        }

        public void reparentSprite(float dt)
        {
            CCNode p1 = GetChildByTag((int)kTags.kTagNode);
            CCNode p2 = GetChildByTag((int)kTags.kTagSpriteBatchNode);

            List<CCNode> retArray = new List<CCNode>(250);

            if (m_usingSpriteBatchNode)
            {
                CCNode tmp = p1;
                p1 = p2;
                p2 = tmp;
            }

            ////----UXLOG("New parent is: %x", p2);

            CCNode node;
            var children = p1.Children;
            foreach (var item in children)
            {
                if (item == null)
                {
                    break;
                }
                retArray.Add(item);
            }

            int i = 0;
            p1.RemoveAllChildrenWithCleanup(false);

            foreach (var item in retArray)
            {
                if (item == null)
                {
                    break;
                }
                p2.AddChild(item, i, i);
                i++;
            }
            m_usingSpriteBatchNode = !m_usingSpriteBatchNode;
        }

        public override string title()
        {
            return "HybrCCSprite* sprite Test";
        }

        public override void OnExit()
        {
            base.OnExit();
            CCSpriteFrameCache.SharedSpriteFrameCache.RemoveSpriteFramesFromFile("animations/grossini.plist");
        }
    }
}
