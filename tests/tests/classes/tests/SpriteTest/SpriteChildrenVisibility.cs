using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
    public class SpriteChildrenVisibility : SpriteTestDemo
    {
        public SpriteChildrenVisibility()
        {
            CCSize s = CCApplication.SharedApplication.MainWindowDirector.WindowSizeInPoints;

            CCApplication.SharedApplication.SpriteFrameCache.AddSpriteFrames("animations/grossini.plist");

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

            aParent.AddChild(sprite1);
            sprite1.AddChild(sprite2, -2);
            sprite1.AddChild(sprite3, 2);

            sprite1.RunAction(new CCBlink (5, 10));

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

            aParent.AddChild(sprite1);
            sprite1.AddChild(sprite2, -2);
            sprite1.AddChild(sprite3, 2);

            sprite1.RunAction(new CCBlink (5, 10));
        }

        public override void OnExit()
        {
            base.OnExit();
            CCApplication.SharedApplication.SpriteFrameCache.RemoveUnusedSpriteFrames();
        }

        public override string title()
        {
            return "Sprite & SpriteBatchNode Visibility";
        }
    }
}
