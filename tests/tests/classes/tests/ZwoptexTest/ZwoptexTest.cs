using CocosSharp;

namespace tests
{
    public class ZwoptexTest : CCLayer
    {
        public static int MAX_LAYER = 1;

        public static int sceneIdx = -1;

        public override void OnEnter()
        {
            base.OnEnter();

            CCSize s = CCDirector.SharedDirector.WinSize;

            CCLabelTTF label = new CCLabelTTF(title(), "arial", 26);
            AddChild(label, 1);
            label.Position = (new CCPoint(s.Width / 2, s.Height - 50));

            string strSubTitle = subtitle();
            if (strSubTitle.Length > 0)
            {
                CCLabelTTF l = new CCLabelTTF(strSubTitle, "Thonburi", 16);
                AddChild(l, 1);
                l.Position = (new CCPoint(s.Width / 2, s.Height - 80));
            }

            CCMenuItemImage item1 = new CCMenuItemImage(TestResource.s_pPathB1, TestResource.s_pPathB2, backCallback);
            CCMenuItemImage item2 = new CCMenuItemImage(TestResource.s_pPathR1, TestResource.s_pPathR2, restartCallback);
            CCMenuItemImage item3 = new CCMenuItemImage(TestResource.s_pPathF1, TestResource.s_pPathF2, nextCallback);

            CCMenu menu = new CCMenu(item1, item2, item3);

            menu.Position = (new CCPoint(0, 0));
            item1.Position = (new CCPoint(s.Width / 2 - 100, 30));
            item2.Position = (new CCPoint(s.Width / 2, 30));
            item3.Position = (new CCPoint(s.Width / 2 + 100, 30));
            AddChild(menu, 1);
        }

        public void restartCallback(object pSender)
        {
            CCScene s = ZwoptexTestScene.node();
            s.AddChild(restartZwoptexTest());
            CCDirector.SharedDirector.ReplaceScene(s);
        }

        public void nextCallback(object pSender)
        {
            CCScene s = ZwoptexTestScene.node();
            s.AddChild(nextZwoptexTest());
            CCDirector.SharedDirector.ReplaceScene(s);
        }

        public void backCallback(object pSender)
        {
            CCScene s = ZwoptexTestScene.node();
            s.AddChild(backZwoptexTest());
            CCDirector.SharedDirector.ReplaceScene(s);
        }

        public virtual string title()
        {
            return "No title";
        }

        public virtual string subtitle()
        {
            return "";
        }


        public static CCLayer createZwoptexLayer(int nIndex)
        {
            switch (nIndex)
            {
                case 0:
                    return new ZwoptexGenericTest();
            }

            return null;
        }

        public static CCLayer nextZwoptexTest()
        {
            sceneIdx++;
            sceneIdx = sceneIdx % MAX_LAYER;

            CCLayer pLayer = createZwoptexLayer(sceneIdx);

            return pLayer;
        }

        public static CCLayer backZwoptexTest()
        {
            sceneIdx--;
            int total = MAX_LAYER;
            if (sceneIdx < 0)
                sceneIdx += total;

            CCLayer pLayer = createZwoptexLayer(sceneIdx);

            return pLayer;
        }

        public static CCLayer restartZwoptexTest()
        {
            CCLayer pLayer = createZwoptexLayer(sceneIdx);

            return pLayer;
        }
    }
}