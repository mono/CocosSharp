using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
    public class SpriteBatchNodeChildrenAnchorPoint : SpriteTestDemo
    {
        CCSprite sprite1, sprite2, sprite3, sprite4, point;
        CCSprite sprite5, sprite6, sprite7, sprite8, point2;
        CCSprite sprite9, sprite10, sprite11, sprite12, point3;

        #region Properties

        public override string Title
        {
            get { return "SpriteBatchNode: children + anchor"; }
        }

        #endregion Properties


        #region Constructors

        public SpriteBatchNodeChildrenAnchorPoint()
        {
            CCSpriteFrameCache.SharedSpriteFrameCache.AddSpriteFrames("animations/grossini.plist");

            CCNode aParent;

            aParent = new CCSpriteBatchNode("animations/grossini", 50);
            AddChild(aParent, 0);

            sprite1 = new CCSprite("grossini_dance_08.png");
            sprite2 = new CCSprite("grossini_dance_02.png");
            sprite3 = new CCSprite("grossini_dance_03.png");
            sprite4 = new CCSprite("grossini_dance_04.png");

            aParent.AddChild(sprite1);
            sprite1.AddChild(sprite2, -2);
            sprite1.AddChild(sprite3, -2);
            sprite1.AddChild(sprite4, 3);

            point = new CCSprite("Images/r1");
            AddChild(point, 10);

            sprite5 = new CCSprite("grossini_dance_08.png");
            sprite6 = new CCSprite("grossini_dance_02.png");
            sprite7 = new CCSprite("grossini_dance_03.png");
            sprite8 = new CCSprite("grossini_dance_04.png");

            aParent.AddChild(sprite5);
            sprite5.AddChild(sprite6, -2);
            sprite5.AddChild(sprite7, -2);
            sprite5.AddChild(sprite8, 3);

            point2 = new CCSprite("Images/r1");
            AddChild(point2, 10);

            sprite9 = new CCSprite("grossini_dance_08.png");
            sprite10 = new CCSprite("grossini_dance_02.png");
            sprite11 = new CCSprite("grossini_dance_03.png");

            sprite12 = new CCSprite("grossini_dance_04.png");
            sprite12.Position = (new CCPoint(0, 0));
            sprite12.Scale = 0.5f;

            aParent.AddChild(sprite9);
            sprite9.AddChild(sprite10, -2);
            sprite9.AddChild(sprite11, -2);
            sprite9.AddChild(sprite12, 3);

            point3 = new CCSprite("Images/r1");
            AddChild(point3, 10);
        }

        #endregion Constructors


        #region Setup content

        public override void OnEnter()
        {
base.OnEnter(); CCSize windowSize = Layer.VisibleBoundsWorldspace.Size;

            sprite1.Position = (new CCPoint(windowSize.Width / 4, windowSize.Height / 2));
            sprite1.AnchorPoint = (new CCPoint(0, 0));
            sprite2.Position = (new CCPoint(20, 30));
            sprite3.Position = (new CCPoint(-20, 30));
            sprite4.Position = (new CCPoint(0, 0));
            sprite4.Scale = 0.5f;

            sprite5.Position = (new CCPoint(windowSize.Width / 2, windowSize.Height / 2));
            sprite5.AnchorPoint = (new CCPoint(0.5f, 0.5f));
            sprite6.Position = (new CCPoint(20, 30));
            sprite7.Position = (new CCPoint(-20, 30));
            sprite8.Position = (new CCPoint(0, 0));
            sprite8.Scale = 0.5f;

            sprite9.Position = (new CCPoint(windowSize.Width / 2 + windowSize.Width / 4, windowSize.Height / 2));
            sprite9.AnchorPoint = (new CCPoint(1, 1));
            sprite10.Position = (new CCPoint(20, 30));
            sprite11.Position = (new CCPoint(-20, 30));

            point.Scale = 0.25f;
            point.Position = sprite1.Position;

            point2.Scale = 0.25f;
            point2.Position = sprite5.Position;

            point3.Scale = 0.25f;
            point3.Position = sprite9.Position;
        }

        #endregion Setup content


        public override void OnExit()
        {
            base.OnExit();
            CCSpriteFrameCache.SharedSpriteFrameCache.RemoveUnusedSpriteFrames();
        }

    }
}
