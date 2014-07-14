using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
    public class SpriteBatchNodeChildrenScale : SpriteTestDemo
    {
        CCNode aParent, aParent2, aParent3, aParent4;
        CCSprite sprite1, sprite2;
        CCSprite sprite3, sprite4;
        CCSprite sprite5, sprite6;
        CCSprite sprite7, sprite8;

        CCRepeatForever seq;

        #region Properties

        public override string Title
        {
            get { return "Sprite/BatchNode + child + scale + rot"; }
        }

        #endregion Properties


        #region Constructors

        public SpriteBatchNodeChildrenScale()
        {
            CCSpriteFrameCache.SharedSpriteFrameCache.AddSpriteFrames("animations/grossini_family.plist");

            var rot = new CCRotateBy (10, 360);

            seq = new CCRepeatForever(rot);

            // Children + Scale using Sprite
            // Test 1
            aParent = new CCNode();
            sprite1 = new CCSprite("grossinis_sister1.png");
            sprite2 = new CCSprite("grossinis_sister2.png");

            AddChild(aParent);
            aParent.AddChild(sprite1);
            sprite1.AddChild(sprite2);

            // Children + Scale using SpriteBatchNode
            // Test 2
            aParent2 = new CCSpriteBatchNode("animations/grossini_family");
            sprite3 = new CCSprite("grossinis_sister1.png");
            sprite4 = new CCSprite("grossinis_sister2.png");
            sprite4.Position = (new CCPoint(50, 0));

            AddChild(aParent2);
            aParent2.AddChild(sprite3);
            sprite3.AddChild(sprite4);

            // Children + Scale using Sprite
            // Test 3
            aParent3 = new CCNode ();
            sprite5 = new CCSprite("grossinis_sister1.png");
            sprite6 = new CCSprite("grossinis_sister2.png");

            AddChild(aParent3);
            aParent3.AddChild(sprite5);
            sprite5.AddChild(sprite6);

            // Children + Scale using Sprite
            // Test 4
            aParent4 = new CCSpriteBatchNode("animations/grossini_family");
            sprite7 = new CCSprite("grossinis_sister1.png");
            sprite8 = new CCSprite("grossinis_sister2.png");

            AddChild(aParent4);
            aParent4.AddChild(sprite7);
            sprite7.AddChild(sprite8);
        }

        #endregion Constructors


        #region Setup content

        public override void OnEnter()
        {
base.OnEnter(); CCSize windowSize = Scene.VisibleBoundsWorldspace.Size;

            sprite1.Position = new CCPoint(windowSize.Width / 4, windowSize.Height / 4);
            sprite1.ScaleX = -0.5f;
            sprite1.ScaleY = 2.0f;
            sprite2.Position = (new CCPoint(50, 0));

            sprite3.Position = new CCPoint(3 * windowSize.Width / 4, windowSize.Height / 4);
            sprite3.ScaleX = -0.5f;
            sprite3.ScaleY = 2.0f;

            sprite5.Position = (new CCPoint(windowSize.Width / 4, 2 * windowSize.Height / 3));
            sprite5.ScaleX = (1.5f);
            sprite5.ScaleY = -0.5f;
            sprite6.Position = (new CCPoint(50, 0));

            sprite7.Position = (new CCPoint(3 * windowSize.Width / 4, 2 * windowSize.Height / 3));
            sprite7.ScaleX = 1.5f;
            sprite7.ScaleY = -0.5f;
            sprite8.Position = (new CCPoint(50, 0));

            sprite1.RunAction(seq);
            sprite3.RunAction(seq);
            sprite5.RunAction(seq);
            sprite7.RunAction(seq);
        }

        #endregion Setup content
    }
}
