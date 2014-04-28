using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
    public class SpriteChildrenAnchorPoint : SpriteTestDemo
    {
        public SpriteChildrenAnchorPoint()
        {
            CCSize s = CCDirector.SharedDirector.WinSize;

            CCSpriteFrameCache.Instance.AddSpriteFrames("animations/grossini.plist");

            CCNode aParent;
            CCSprite sprite1, sprite2, sprite3, sprite4, point;
            //
            // SpriteBatchNode
            //
            // parents

            aParent = new CCNode ();
            AddChild(aParent, 0);

            // anchor (0,0)
            sprite1 = new CCSprite("grossini_dance_08.png");
            sprite1.Position = (new CCPoint(s.Width / 4, s.Height / 2));
            sprite1.AnchorPoint = (new CCPoint(0, 0));


            sprite2 = new CCSprite("grossini_dance_02.png");
            sprite2.Position = (new CCPoint(20, 30));

            sprite3 = new CCSprite("grossini_dance_03.png");
            sprite3.Position = (new CCPoint(-20, 30));

            sprite4 = new CCSprite("grossini_dance_04.png");
            sprite4.Position = (new CCPoint(0, 0));
            sprite4.Scale = 0.5f;


            aParent.AddChild(sprite1);
            sprite1.AddChild(sprite2, -2);
            sprite1.AddChild(sprite3, -2);
            sprite1.AddChild(sprite4, 3);

            point = new CCSprite("Images/r1");
            point.Scale = 0.25f;
            point.Position = sprite1.Position;
            AddChild(point, 10);


            // anchor (0.5, 0.5)
            sprite1 = new CCSprite("grossini_dance_08.png");
            sprite1.Position = (new CCPoint(s.Width / 2, s.Height / 2));
            sprite1.AnchorPoint = (new CCPoint(0.5f, 0.5f));

            sprite2 = new CCSprite("grossini_dance_02.png");
            sprite2.Position = (new CCPoint(20, 30));

            sprite3 = new CCSprite("grossini_dance_03.png");
            sprite3.Position = (new CCPoint(-20, 30));

            sprite4 = new CCSprite("grossini_dance_04.png");
            sprite4.Position = (new CCPoint(0, 0));
            sprite4.Scale = 0.5f;

            aParent.AddChild(sprite1);
            sprite1.AddChild(sprite2, -2);
            sprite1.AddChild(sprite3, -2);
            sprite1.AddChild(sprite4, 3);

            point = new CCSprite("Images/r1");
            point.Scale = 0.25f;
            point.Position = sprite1.Position;
            AddChild(point, 10);


            // anchor (1,1)
            sprite1 = new CCSprite("grossini_dance_08.png");
            sprite1.Position = (new CCPoint(s.Width / 2 + s.Width / 4, s.Height / 2));
            sprite1.AnchorPoint = new CCPoint(1, 1);


            sprite2 = new CCSprite("grossini_dance_02.png");
            sprite2.Position = (new CCPoint(20, 30));

            sprite3 = new CCSprite("grossini_dance_03.png");
            sprite3.Position = (new CCPoint(-20, 30));

            sprite4 = new CCSprite("grossini_dance_04.png");
            sprite4.Position = (new CCPoint(0, 0));
            sprite4.Scale = 0.5f;

            aParent.AddChild(sprite1);
            sprite1.AddChild(sprite2, -2);
            sprite1.AddChild(sprite3, -2);
            sprite1.AddChild(sprite4, 3);

            point = new CCSprite("Images/r1");
            point.Scale = 0.25f;
            point.Position = sprite1.Position;
            AddChild(point, 10);
        }

        public override void OnExit()
        {
            base.OnExit();
            CCSpriteFrameCache.Instance.RemoveUnusedSpriteFrames();
        }

        public override string title()
        {
            return "Sprite: children + anchor";
        }
    }
}
