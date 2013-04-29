using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class Bug1159Layer : BugsTestBaseLayer
    {
        public override bool Init()
        {
            if (base.Init())
            {
                CCSize s = CCDirector.SharedDirector.WinSize;

                CCLayerColor background = new CCLayerColor(new CCColor4B(255, 0, 255, 255));
                AddChild(background);

                CCLayerColor sprite_a = new CCLayerColor(new CCColor4B(255, 0, 0, 255), 700, 700);
                sprite_a.AnchorPoint = new CCPoint(0.5f, 0.5f);
                sprite_a.IgnoreAnchorPointForPosition = true;
                sprite_a.Position = new CCPoint(0.0f, s.Height / 2);
                AddChild(sprite_a);

                sprite_a.RunAction(new CCRepeatForever ((CCActionInterval)CCSequence.FromActions(
                                                                       new CCMoveTo (1.0f, new CCPoint(1024.0f, 384.0f)),
                                                                       new CCMoveTo (1.0f, new CCPoint(0.0f, 384.0f)))));

                CCLayerColor sprite_b = new CCLayerColor(new CCColor4B(0, 0, 255, 255), 400, 400);
                sprite_b.AnchorPoint = new CCPoint(0.5f, 0.5f);
                sprite_b.IgnoreAnchorPointForPosition = true;
                sprite_b.Position = new CCPoint(s.Width / 2, s.Height / 2);
                AddChild(sprite_b);

                CCMenuItemLabel label = CCMenuItemLabel.Create(new CCLabelTTF("Flip Me", "Helvetica", 24), callBack);
                CCMenu menu = new CCMenu(label);
                menu.Position = new CCPoint(s.Width - 200.0f, 50.0f);
                AddChild(menu);

                return true;
            }

            return false;
        }

        public static CCScene scene()
        {
            CCScene pScene = new CCScene();
            //Bug1159Layer layer = Bug1159Layer.node();
            //pScene.addChild(layer);

            return pScene;
        }

        public void callBack(object pSender)
        {
            CCDirector.SharedDirector.ReplaceScene(new CCTransitionPageTurn(1.0f, Bug1159Layer.scene(), false));
        }

        //LAYER_NODE_FUNC(Bug1159Layer);
    }
}
