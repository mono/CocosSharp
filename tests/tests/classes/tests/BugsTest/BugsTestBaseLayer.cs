using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
    public class BugsTestBaseLayer : CCLayer
    {
        public override void OnEnter()
        {
            base.OnEnter();

            CCSize s = Layer.VisibleBoundsWorldspace.Size;

            CCMenuItemFont pMainItem = new CCMenuItemFont("Back", backCallback);
            pMainItem.Position = new CCPoint(s.Width - 50, 25);
            CCMenu pMenu = new CCMenu(pMainItem, null);
            pMenu.Position = new CCPoint(0, 0);
            AddChild(pMenu);
        }

        public void backCallback(object pSender)
        {
            //Scene.Director.EnableRetinaDisplay(false);
            BugsTestScene pScene = new BugsTestScene();
            pScene.runThisTest();
        }
    }
}
