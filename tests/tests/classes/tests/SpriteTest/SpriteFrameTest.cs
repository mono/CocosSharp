using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class SpriteFrameTest : SpriteTestDemo
    {
        public SpriteFrameTest()
        {
            CCSize s = CCDirector.SharedDirector.WinSize;

            // IMPORTANT:
            // The sprite frames will be cached AND RETAINED, and they won't be released unless you call
            //     CCSpriteFrameCache::sharedSpriteFrameCache()->removeUnusedSpriteFrames);
            CCSpriteFrameCache cache = CCSpriteFrameCache.SharedSpriteFrameCache;
            cache.AddSpriteFramesWithFile("animations/grossini.plist");
            cache.AddSpriteFramesWithFile("animations/grossini_gray.plist", "animations/grossini_gray");
            cache.AddSpriteFramesWithFile("animations/grossini_blue.plist", "animations/grossini_blue");

            //
            // Animation using Sprite BatchNode
            //
            m_pSprite1 = CCSprite.Create("grossini_dance_01.png");
            m_pSprite1.Position = (new CCPoint(s.width / 2 -80, s.height / 2));

            CCSpriteBatchNode spritebatch = CCSpriteBatchNode.Create("animations/grossini");
            spritebatch.AddChild(m_pSprite1);
            AddChild(spritebatch);

            var animFrames = new List<CCSpriteFrame>(15);

            string str = "";
            for (int i = 1; i < 15; i++)
            {
                str = string.Format("grossini_dance_{0:00}.png", i);
                CCSpriteFrame frame = cache.SpriteFrameByName(str);
                animFrames.Add(frame);
            }

            CCAnimation animation = CCAnimation.Create(animFrames, 0.3f);
            m_pSprite1.RunAction(CCRepeatForever.Create(CCAnimate.Create(animation)));

            // to test issue #732, uncomment the following line
            m_pSprite1.FlipX = false;
            m_pSprite1.FlipY = false;

            //
            // Animation using standard Sprite
            //
            m_pSprite2 = CCSprite.Create("grossini_dance_01.png");
            m_pSprite2.Position = (new CCPoint(s.width / 2 + 80, s.height / 2));
            AddChild(m_pSprite2);


            var moreFrames = new List<CCSpriteFrame>(20);
            for (int i = 1; i < 15; i++)
            {
                string temp;
                str = string.Format("grossini_dance_gray_{0:00}.png", i);
                CCSpriteFrame frame = cache.SpriteFrameByName(str);
                moreFrames.Add(frame);
            }


            for (int i = 1; i < 5; i++)
            {
                str = string.Format("grossini_blue_{0:00}.png", i);
                CCSpriteFrame frame = cache.SpriteFrameByName(str);
                moreFrames.Add(frame);
            }

            // append frames from another batch
            moreFrames.AddRange(animFrames);
            CCAnimation animMixed = CCAnimation.Create(moreFrames, 0.3f);


            m_pSprite2.RunAction(CCRepeatForever.Create(CCAnimate.Create(animMixed)));



            // to test issue #732, uncomment the following line
            m_pSprite2.FlipX = false;
            m_pSprite2.FlipY = false;

            Schedule(startIn05Secs, 0.5f);
            m_nCounter = 0;
        }

        public override void OnExit()
        {
            base.OnExit();
            CCSpriteFrameCache cache = CCSpriteFrameCache.SharedSpriteFrameCache;
            cache.RemoveSpriteFramesFromFile("animations/grossini.plist");
            cache.RemoveSpriteFramesFromFile("animations/grossini_gray.plist");
            cache.RemoveSpriteFramesFromFile("animations/grossini_blue.plist");
        }

        public override string title()
        {
            return "Sprite vs. SpriteBatchNode animation";
        }

        public override string subtitle()
        {
            return "Testing issue #792";
        }

        public void startIn05Secs(float dt)
        {
            Unschedule(startIn05Secs);
            Schedule(flipSprites, 1.0f);
        }

        public void flipSprites(float dt)
        {
            m_nCounter++;

            bool fx = false;
            bool fy = false;
            int i = m_nCounter % 4;

            switch (i)
            {
                case 0:
                    fx = false;
                    fy = false;
                    break;
                case 1:
                    fx = true;
                    fy = false;
                    break;
                case 2:
                    fx = false;
                    fy = true;
                    break;
                case 3:
                    fx = true;
                    fy = true;
                    break;
            }

            m_pSprite1.FlipX = (fx);
            m_pSprite1.FlipY = (fy);
            m_pSprite2.FlipX = (fx);
            m_pSprite2.FlipY = (fy);
            //NSLog(@"flipX:%d, flipY:%d", fx, fy);
        }

        private CCSprite m_pSprite1;
        private CCSprite m_pSprite2;
        private int m_nCounter;
    }
}
