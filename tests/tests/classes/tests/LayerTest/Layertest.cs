using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
    public class LayerTest : TestNavigationLayer
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

        public override void OnEnter()
        {
            base.OnEnter();

            CCSize s = Layer.VisibleBoundsWorldspace.Size;

            CCLabelTtf label = new CCLabelTtf(Title, "arial", 32);
            AddChild(label, 1);
            label.Position = (new CCPoint(s.Width / 2, s.Height - 50));

            string subtitle_ = Subtitle;
            if (subtitle_.Length > 0)
            {
                CCLabelTtf l = new CCLabelTtf(subtitle_, "arial", 16);
                AddChild(l, 1);
                l.Position = (new CCPoint(s.Width / 2, s.Height - 80));
            }

            CCMenuItemImage item1 = new CCMenuItemImage(s_pPathB1, s_pPathB2, (BackCallback));
            CCMenuItemImage item2 = new CCMenuItemImage(s_pPathR1, s_pPathR2, (RestartCallback));
            CCMenuItemImage item3 = new CCMenuItemImage(s_pPathF1, s_pPathF2, (NextCallback));

            CCMenu menu = new CCMenu(item1, item2, item3);

            menu.Position = new CCPoint(0, 0);
            item1.Position = new CCPoint(s.Width / 2 - 100, 30);
            item2.Position = new CCPoint(s.Width / 2, 30);
            item3.Position = new CCPoint(s.Width / 2 + 100, 30);

            AddChild(menu, 1);
        }

		public override string Title
		{
			get
			{
				return "Layer Test";
			}
		}

		public override string Subtitle
		{
			get
			{
				return string.Empty;
			}
		}

		public override void RestartCallback(object sender)
		{
            CCScene s = new LayerTestScene();
            s.AddChild(LayerTestScene.restartTestAction());
            Director.ReplaceScene(s);
        }

		public override void NextCallback(object sender)
		{
            CCScene s = new LayerTestScene();
            s.AddChild(LayerTestScene.nextTestAction());

            Director.ReplaceScene(s);
        }

		public override void BackCallback(object sender)
		{
            CCScene s = new LayerTestScene();
            s.AddChild(LayerTestScene.backTestAction());

            Director.ReplaceScene(s);
        }

        protected static void SetEnableRecursiveCascading(CCNode node, bool enable)
        {
            node.IsColorCascaded = true;
            node.IsOpacityCascaded = enable;

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

		public override string Title
		{
			get
			{
				return "Layer Multiplex Test";
			}
		}

		public override string Subtitle
		{
			get
			{
				return "Three layers switch every 3 seconds";
			}
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

