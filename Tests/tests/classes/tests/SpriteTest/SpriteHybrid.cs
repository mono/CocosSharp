using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;
using Random = CocosSharp.CCRandom;

namespace tests
{
    public class SpriteHybrid : SpriteTestDemo
    {
        const int numOfSprites = 250;

        bool usingSpriteBatchNode;
        CCSprite[] sprites;

        #region Properties

        public override string Title
        {
            get { return "HybrCCSprite* sprite Test"; }
        }

        #endregion Properties


        #region Constructors

        public SpriteHybrid()
        {
            // parents
            CCNode parent1 = new CCNode();
            CCSprite parent2 = new CCSprite("animations/grossini");

            AddChild(parent1, 0, (int)kTags.kTagNode);
            AddChild(parent2, 0, (int)kTags.kTagSpriteBatchNode);

            sprites = new CCSprite[numOfSprites];

            CCSpriteFrameCache.SharedSpriteFrameCache.AddSpriteFrames("animations/grossini.plist");

            // create 250 sprites
            // only show 80% of them
            for (int i = 0; i < numOfSprites; i++)
            {
                int spriteIdx = (int)(CCRandom.NextDouble() * 14);
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
                CCSpriteFrame frame = CCSpriteFrameCache.SharedSpriteFrameCache[str];
                sprites[i] = new CCSprite(frame);
                parent1.AddChild(sprites[i], i, i);
            }

            usingSpriteBatchNode = false;
        }

        #endregion Constructors


        #region Setup content

        public override void OnEnter()
        {
base.OnEnter(); CCSize windowSize = Layer.VisibleBoundsWorldspace.Size;

            CCFiniteTimeAction action = new CCRotateBy(4, 360);

            for(int i = 0; i < numOfSprites; i++) 
            {
                float x = -1000;
                float y = -1000;

                if(CCRandom.NextDouble() < 0.2f)
                {
                    x = (float)(CCRandom.NextDouble() * windowSize.Width);
                    y = (float)(CCRandom.NextDouble() * windowSize.Height);
                }
                sprites[i].Position = (new CCPoint(x, y));

                sprites[i].RunAction(new CCRepeatForever(action));
            }

            Schedule(ReparentSprite, 2);
        }

        #endregion Setup content


        void ReparentSprite(float dt)
        {
            CCNode p1 = GetChildByTag((int)kTags.kTagNode);
            CCNode p2 = GetChildByTag((int)kTags.kTagSpriteBatchNode);

            List<CCNode> retArray = new List<CCNode>(250);

            if (usingSpriteBatchNode)
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
            p1.RemoveAllChildren(false);

            foreach (var item in retArray)
            {
                if (item == null)
                {
                    break;
                }
                p2.AddChild(item, i, i);
                i++;
            }

            usingSpriteBatchNode = !usingSpriteBatchNode;
        }

        public override void OnExit()
        {
            base.OnExit();
            CCSpriteFrameCache.SharedSpriteFrameCache.RemoveSpriteFrames("animations/grossini.plist");
        }
    }
}
