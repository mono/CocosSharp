using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public abstract class NodeChildrenMainScene : CCScene
    {
        public virtual void initWithQuantityOfNodes(int nNodes)
        {
            //srand(time());
            CCSize s = CCDirector.SharedDirector.WinSize;

            // Title
            CCLabelTTF label = CCLabelTTF.Create(title(), "arial", 32);
            AddChild(label, 1);
            label.Position = new CCPoint(s.Width / 2, s.Height - 32);
            label.Color = new CCColor3B(255, 255, 40);

            // Subtitle
            string strSubTitle = subtitle();
            if (strSubTitle.Length > 0)
            {
                CCLabelTTF l = CCLabelTTF.Create(strSubTitle, "arial", 16);
                AddChild(l, 1);
                l.Position = new CCPoint(s.Width / 2, s.Height - 80);
            }

            lastRenderedCount = 0;
            currentQuantityOfNodes = 0;
            quantityOfNodes = nNodes;

            CCMenuItemFont.FontSize = 64;
            CCMenuItemFont decrease = CCMenuItemFont.Create(" - ", onDecrease);
            decrease.Color = new CCColor3B(0, 200, 20);
            CCMenuItemFont increase = CCMenuItemFont.Create(" + ", onIncrease);
            increase.Color = new CCColor3B(0, 200, 20);

            CCMenu menu = CCMenu.Create(decrease, increase);
            menu.AlignItemsHorizontally();
            menu.Position = new CCPoint(s.Width / 2, s.Height / 2 + 15);
            AddChild(menu, 1);

            CCLabelTTF infoLabel = CCLabelTTF.Create("0 nodes", "arial", 30);
            infoLabel.Color = new CCColor3B(0, 200, 20);
            infoLabel.Position = new CCPoint(s.Width / 2, s.Height / 2 - 15);
            AddChild(infoLabel, 1, PerformanceNodeChildrenTest.kTagInfoLayer);

            NodeChildrenMenuLayer pMenu = new NodeChildrenMenuLayer(true, PerformanceNodeChildrenTest.TEST_COUNT, PerformanceNodeChildrenTest.s_nCurCase);
            AddChild(pMenu);

            updateQuantityLabel();
            updateQuantityOfNodes();
        }

        public virtual string title()
        {
            return "No title";
        }

        public virtual string subtitle()
        {
            return "";
        }

        public abstract void updateQuantityOfNodes();

        public void onDecrease(CCObject pSender)
        {
            quantityOfNodes -= PerformanceNodeChildrenTest.kNodesIncrease;
            if (quantityOfNodes < 0)
                quantityOfNodes = 0;

            updateQuantityLabel();
            updateQuantityOfNodes();
        }

        public void onIncrease(CCObject pSender)
        {
            quantityOfNodes += PerformanceNodeChildrenTest.kNodesIncrease;
            if (quantityOfNodes > PerformanceNodeChildrenTest.kMaxNodes)
                quantityOfNodes = PerformanceNodeChildrenTest.kMaxNodes;

            updateQuantityLabel();
            updateQuantityOfNodes();
        }

        public void updateQuantityLabel()
        {
            if (quantityOfNodes != lastRenderedCount)
            {
                CCLabelTTF infoLabel = (CCLabelTTF)GetChildByTag(PerformanceNodeChildrenTest.kTagInfoLayer);
                infoLabel.SetString(string.Format("{0} nodes", quantityOfNodes));

                lastRenderedCount = quantityOfNodes;
            }
        }

        public int getQuantityOfNodes()
        {
            return quantityOfNodes;
        }

        protected int lastRenderedCount;
        protected int quantityOfNodes;
        protected int currentQuantityOfNodes;
    }
}
