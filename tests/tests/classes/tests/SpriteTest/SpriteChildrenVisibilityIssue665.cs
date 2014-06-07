using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
    public class SpriteChildrenVisibilityIssue665 : SpriteTestDemo
    {
        public SpriteChildrenVisibilityIssue665()
        {
            CCSize s = CCApplication.SharedApplication.MainWindowDirector.WinSize;

            CCSpriteFrameCache.Instance.AddSpriteFrames("animations/grossini.plist");

            CCNode aParent;
            CCSprite sprite1, sprite2, sprite3;
            //
            // SpriteBatchNode
            //
            // parents
            aParent = new CCSpriteBatchNode("animations/grossini", 50);
            aParent.Position = (new CCPoint(s.Width / 3, s.Height / 2));
            AddChild(aParent, 0);

            sprite1 = new CCSprite("grossini_dance_01.png");
            sprite1.Position = (new CCPoint(0, 0));

            sprite2 = new CCSprite("grossini_dance_02.png");
            sprite2.Position = (new CCPoint(20, 30));

            sprite3 = new CCSprite("grossini_dance_03.png");
            sprite3.Position = (new CCPoint(-20, 30));

            // test issue #665
            sprite1.Visible = false;

            aParent.AddChild(sprite1);
            sprite1.AddChild(sprite2, -2);
            sprite1.AddChild(sprite3, 2);

            //
            // Sprite
            //
            aParent = new CCNode ();
            aParent.Position = (new CCPoint(2 * s.Width / 3, s.Height / 2));
            AddChild(aParent, 0);

            sprite1 = new CCSprite("grossini_dance_01.png");
            sprite1.Position = (new CCPoint(0, 0));

            sprite2 = new CCSprite("grossini_dance_02.png");
            sprite2.Position = (new CCPoint(20, 30));

            sprite3 = new CCSprite("grossini_dance_03.png");
            sprite3.Position = (new CCPoint(-20, 30));

            // test issue #665
            sprite1.Visible = false;

            aParent.AddChild(sprite1);
            sprite1.AddChild(sprite2, -2);
            sprite1.AddChild(sprite3, 2);
        }
        public override string title()
        {
            return "Sprite & SpriteBatchNode Visibility";
        }
        public override string subtitle()
        {
            return "No sprites should be visible";
        }
    }
}
