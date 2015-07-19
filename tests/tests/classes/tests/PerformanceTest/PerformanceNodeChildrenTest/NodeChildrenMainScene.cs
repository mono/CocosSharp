using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
    public abstract class NodeChildrenMainScene : CCScene
    {
        long lStart;

        public NodeChildrenMainScene(): base(AppDelegate.SharedWindow)
        {
        }

        protected void StartTimer()
        {
            lStart = DateTime.Now.Ticks;
        }

        protected void EndTimer(string msg)
        {
            long diff = DateTime.Now.Ticks - lStart;
            TimeSpan ts = new TimeSpan(diff);
            CCLog.Log("{0} took {1} s, or {2} ms", msg, ts.TotalSeconds, ts.TotalMilliseconds);
        }

        public virtual void initWithQuantityOfNodes(int nNodes)
        {
            //srand(time());
            CCSize s = Layer.VisibleBoundsWorldspace.Size;

            // Title
            var label = new CCLabel(title(), "arial", 32, CCLabelFormat.SpriteFont);
            AddChild(label, 1);
            label.Position = new CCPoint(s.Width / 2, s.Height - 32);
            label.Color = new CCColor3B(255, 255, 40);

            // Subtitle
            string strSubTitle = subtitle();
            if (strSubTitle.Length > 0)
            {
                var l = new CCLabel(strSubTitle, "arial", 16, CCLabelFormat.SpriteFont);
                AddChild(l, 1);
                l.Position = new CCPoint(s.Width / 2, s.Height - 80);
            }

            lastRenderedCount = 0;
            currentQuantityOfNodes = 0;
            quantityOfNodes = nNodes;

			CCMenuItemFont.FontSize = 64;
			CCMenuItemFont.FontName = "arial";

            CCMenuItemFont decrease = new CCMenuItemFont(" - ", onDecrease);
            decrease.Color = new CCColor3B(0, 200, 20);
            CCMenuItemFont increase = new CCMenuItemFont(" + ", onIncrease);
            increase.Color = new CCColor3B(0, 200, 20);

            CCMenu menu = new CCMenu(decrease, increase);
            menu.AlignItemsHorizontally();
            menu.Position = new CCPoint(s.Width / 2, s.Height / 2 + 15);
            AddChild(menu, 1);

            var infoLabel = new CCLabel("0 nodes", "arial", 30, CCLabelFormat.SpriteFont);
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

        public void onDecrease(object pSender)
        {
            quantityOfNodes -= PerformanceNodeChildrenTest.kNodesIncrease;
            if (quantityOfNodes < 0)
                quantityOfNodes = 0;

            updateQuantityLabel();
            updateQuantityOfNodes();
        }

        public void onIncrease(object pSender)
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
                var infoLabel = (CCLabel)GetChildByTag(PerformanceNodeChildrenTest.kTagInfoLayer);
                infoLabel.Text = (string.Format("{0} nodes", quantityOfNodes));

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
