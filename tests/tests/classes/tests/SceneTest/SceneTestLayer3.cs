using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
    public class SceneTestLayer3 : CCLayerColor
    {
        string s_pPathGrossini = "Images/grossini";

        public SceneTestLayer3() : base(new CCColor4B(0, 0, 255, 255))
        {
            var item0 = new CCMenuItemFont("(3) Touch to pushScene (self)", item0Clicked);
            var item1 = new CCMenuItemFont("(3) Touch to popScene", item1Clicked);
            var item2 = new CCMenuItemFont("(3) Touch to popToRootScene", item2Clicked);
            var item3 = new CCMenuItemFont("(3) Touch to popToSceneStackLevel(2)", item3Clicked);

            CCMenu menu = new CCMenu(item0, item1, item2, item3);
            menu.AlignItemsVertically();

            AddChild(menu);

            CCSize s = CCApplication.SharedApplication.MainWindowDirector.WindowSizeInPoints;
            CCSprite sprite = new CCSprite(s_pPathGrossini);
            AddChild(sprite);
            
            sprite.Position = new CCPoint(s.Width /2, 40);
            CCActionInterval rotate = new CCRotateBy (2, 360);
            CCAction repeat = new CCRepeatForever (rotate);
            sprite.RunAction(repeat);

            Schedule(testDealloc);
        }

        public virtual void testDealloc(float dt)
        {

        }

        public void item0Clicked(object pSender)
        {
            var newScene = new CCScene();
            newScene.AddChild(new SceneTestLayer3());
            CCApplication.SharedApplication.MainWindowDirector.PushScene(new CCTransitionFade(0.5f, newScene, new CCColor3B(0, 255, 255)));
        }

        public void item1Clicked(object pSender)
        {
            CCApplication.SharedApplication.MainWindowDirector.PopScene();
        }

        public void item2Clicked(object pSender)
        {
            CCApplication.SharedApplication.MainWindowDirector.PopToRootScene();
        }

        public void item3Clicked(object pSender)
        {
            CCApplication.SharedApplication.MainWindowDirector.PopToSceneStackLevel(2);
        }
    }
}
