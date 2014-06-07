using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
    public class SpriteMainScene : CCScene
    {

        public virtual string title()
        {
            return "No title";
        }

        public void initWithSubTest(int asubtest, int nNodes)
        {
            //srandom(0);

            subtestNumber = asubtest;
            m_pSubTest = new SubTest();
            m_pSubTest.initWithSubTest(asubtest, this);

            CCSize s = CCApplication.SharedApplication.MainWindowDirector.WinSize;

            lastRenderedCount = 0;
            quantityNodes = 0;

			CCMenuItemFont.FontSize = 64;
			CCMenuItemFont.FontName = "arial";
            CCMenuItemFont decrease = new CCMenuItemFont(" - ", onDecrease);
            decrease.Color = new CCColor3B(0, 200, 20);
            CCMenuItemFont increase = new CCMenuItemFont(" + ", onIncrease);
            increase.Color = new CCColor3B(0, 200, 20);

            CCMenu menu = new CCMenu(decrease, increase);
            menu.AlignItemsHorizontally();
            menu.Position = new CCPoint(s.Width / 2, s.Height - 65);
            AddChild(menu, 1);

            CCLabelTtf infoLabel = new CCLabelTtf("0 nodes", "Marker Felt", 30);
            infoLabel.Color = new CCColor3B(0, 200, 20);
            infoLabel.Position = new CCPoint(s.Width / 2, s.Height - 90);
            AddChild(infoLabel, 1, PerformanceSpriteTest.kTagInfoLayer);

            // add menu
            SpriteMenuLayer pMenu = new SpriteMenuLayer(true, PerformanceSpriteTest.TEST_COUNT, PerformanceSpriteTest.s_nSpriteCurCase);
            AddChild(pMenu, 1, PerformanceSpriteTest.kTagMenuLayer);

            // Sub Tests
			CCMenuItemFont.FontSize = 32;
			CCMenuItemFont.FontName = "arial";
            CCMenu pSubMenu = new CCMenu(null);
            for (int i = 1; i <= 9; ++i)
            {
                //char str[10] = {0};
                var str = string.Format("{0}", i);
                CCMenuItemFont itemFont = new CCMenuItemFont(str, testNCallback);
                itemFont.Tag = i;
                pSubMenu.AddChild(itemFont, 10);

                if (i <= 3)
                    itemFont.Color = new CCColor3B(200, 20, 20);
                else if (i <= 6)
                    itemFont.Color = new CCColor3B(0, 200, 20);
                else
                    itemFont.Color = new CCColor3B(0, 20, 200);
            }

            pSubMenu.AlignItemsHorizontally();
            pSubMenu.Position = new CCPoint(s.Width / 2, 80);
            AddChild(pSubMenu, 2);

            // add title label
            CCLabelTtf label = new CCLabelTtf(title(), "arial", 38);
            AddChild(label, 1);
            label.Position = new CCPoint(s.Width / 2, s.Height - 32);
            label.Color = new CCColor3B(255, 255, 40);

            while (quantityNodes < nNodes)
                onIncrease(this);
        }

        public void updateNodes()
        {
            if (quantityNodes != lastRenderedCount)
            {
                CCLabelTtf infoLabel = (CCLabelTtf)GetChildByTag(PerformanceSpriteTest.kTagInfoLayer);
                var str = string.Format("{0} nodes", quantityNodes);
                infoLabel.Text = (str);

                lastRenderedCount = quantityNodes;
            }
        }

        public void testNCallback(object pSender)
        {
            subtestNumber = ((CCMenuItemFont)pSender).Tag;
            SpriteMenuLayer pMenu = (SpriteMenuLayer)GetChildByTag(PerformanceSpriteTest.kTagMenuLayer);
            pMenu.restartCallback(pSender);
        }

        public void onIncrease(object pSender)
        {
            if (quantityNodes >= PerformanceSpriteTest.kMaxNodes)
                return;

            for (int i = 0; i < PerformanceSpriteTest.kNodesIncrease; i++)
            {
                CCSprite sprite = m_pSubTest.createSpriteWithTag(quantityNodes);
                doTest(sprite);
                quantityNodes++;
            }

            updateNodes();
        }

        public void onDecrease(object pSender)
        {
            if (quantityNodes <= 0)
                return;

            for (int i = 0; i < PerformanceSpriteTest.kNodesIncrease; i++)
            {
                quantityNodes--;
                m_pSubTest.removeByTag(quantityNodes);
            }

            updateNodes();
        }

        public virtual void doTest(CCSprite sprite)
        {
            throw new NotFiniteNumberException();
        }

        public int getSubTestNum()
        { return subtestNumber; }

        public int getNodesNum()
        { return quantityNodes; }


        protected int lastRenderedCount;
        protected int quantityNodes;
        protected SubTest m_pSubTest;
        protected int subtestNumber;
    }
}
