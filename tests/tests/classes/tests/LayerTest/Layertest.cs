using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
    public class LayerTest : CCLayer
    {
        private string s_pPathB1 = "Images/b1";
        private string s_pPathB2 = "Images/b2";
        private string s_pPathR1 = "Images/r1";
        private string s_pPathR2 = "Images/r2";
        private string s_pPathF1 = "Images/f1";
        private string s_pPathF2 = "Images/f2";

        protected string m_strTitle;

        public LayerTest()
        {

        }

        public virtual string title()
        {
            return "No title";
        }

        public virtual string subtitle()
        {
            return "";
        }

        public override void OnEnter()
        {
            base.OnEnter();

            CCSize s = CCDirector.SharedDirector.WinSize;

            CCLabelTTF label = new CCLabelTTF(title(), "arial", 32);
            AddChild(label, 1);
            label.Position = (new CCPoint(s.Width / 2, s.Height - 50));

            string subtitle_ = subtitle();
            if (subtitle_.Length > 0)
            {
                CCLabelTTF l = new CCLabelTTF(subtitle_, "arial", 16);
                AddChild(l, 1);
                l.Position = (new CCPoint(s.Width / 2, s.Height - 80));
            }

            CCMenuItemImage item1 = new CCMenuItemImage(s_pPathB1, s_pPathB2, (backCallback));
            CCMenuItemImage item2 = new CCMenuItemImage(s_pPathR1, s_pPathR2, (restartCallback));
            CCMenuItemImage item3 = new CCMenuItemImage(s_pPathF1, s_pPathF2, (nextCallback));

            CCMenu menu = new CCMenu(item1, item2, item3);

            menu.Position = new CCPoint(0, 0);
            item1.Position = new CCPoint(s.Width / 2 - 100, 30);
            item2.Position = new CCPoint(s.Width / 2, 30);
            item3.Position = new CCPoint(s.Width / 2 + 100, 30);

            AddChild(menu, 1);
        }

        public void restartCallback(object pSender)
        {
            CCScene s = new LayerTestScene();
            s.AddChild(LayerTestScene.restartTestAction());
            CCDirector.SharedDirector.ReplaceScene(s);
        }

        public void nextCallback(object pSender)
        {
            CCScene s = new LayerTestScene();
            s.AddChild(LayerTestScene.nextTestAction());
            CCDirector.SharedDirector.ReplaceScene(s);
        }

        public void backCallback(object pSender)
        {
            CCScene s = new LayerTestScene();
            s.AddChild(LayerTestScene.backTestAction());
            CCDirector.SharedDirector.ReplaceScene(s);
        }

        protected static void SetEnableRecursiveCascading(CCNode node, bool enable)
        {
            var rgba = node as ICCColor;
            
            if (rgba != null)
            {
                rgba.IsColorCascaded = true;
                rgba.IsOpacityCascaded = enable;
            }

            if (node.ChildrenCount > 0)
            {
                foreach (var children in node.Children)
                {
                    SetEnableRecursiveCascading(children, enable);
                }
            }
        }
    }
    public class LayerMultiplexTest : LayerTest
    {
        CCLayerMultiplex child = new CCLayerMultiplex();
        public LayerMultiplexTest()
        {
            for (int i = 0; i < 3; i++)
            {
				CCLayer l = new CCLayerColor(new CCColor4B(0,255,0));
                CCSprite img = null;
                switch (i)
                {
                    case 0:
                        img = new CCSprite("Images/grossini");
                        break;
                    case 1:
                        img = new CCSprite("Images/grossinis_sister1");
                        break;
                    case 2:
                        img = new CCSprite("Images/grossinis_sister2");
                        break;
                }
                img.AnchorPoint = CCPoint.Zero;
                img.Position = CCPoint.Zero;
				l.ContentSize = img.ContentSize;
                l.AddChild(img);
                l.Position = new CCPoint(128f, 128f);
                child.AddLayer(l);
            }
			child.InAction = new CCFadeIn(1);
            AddChild(child);
			Schedule(new Action<float>(AutoMultiplex), 3f);
        }

        public override string title()
        {
            return "Layer Multiplex Test";
        }

        public override string subtitle()
        {
            return "Three layers switch every 3 seconds";
        }

        private Random rand = new Random();
		private int lastRandom = 0;

        private void AutoMultiplex(float dt)
        {
			//CCLog.Log ("Switched");
			var newRand = -1;
			// make sure we always change to a new one
			do {
				newRand = rand.Next (3);
			} while (lastRandom == newRand);
			lastRandom = newRand;

			child.SwitchTo(newRand);
        }
    }
}

