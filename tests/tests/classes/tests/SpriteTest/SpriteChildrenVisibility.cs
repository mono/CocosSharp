using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
    public class SpriteChildrenVisibility : SpriteTestDemo
    {
        CCNode aParent, aParent2;
        CCSprite sprite1, sprite2, sprite3, sprite4, sprite5, sprite6;


        #region Properties

        public override string Title
        {
            get { return "Sprite & SpriteBatchNode Visibility"; }
        }

        #endregion Properties


        #region Constructors

        public SpriteChildrenVisibility()
        {
            CCSpriteFrameCache.SharedSpriteFrameCache.AddSpriteFrames("animations/grossini.plist");

            // SpriteBatchNode
            aParent = new CCSprite("animations/grossini");

            sprite1 = new CCSprite("grossini_dance_01.png");
            sprite2 = new CCSprite("grossini_dance_02.png");
            sprite3 = new CCSprite("grossini_dance_03.png");

            AddChild(sprite1);
            sprite1.AddChild(sprite2, -2);
            sprite1.AddChild(sprite3, 2);

            // Sprite
            aParent2 = new CCNode();
            AddChild(aParent2, 0);

            sprite4 = new CCSprite("grossini_dance_01.png");
            sprite5 = new CCSprite("grossini_dance_02.png");
            sprite6 = new CCSprite("grossini_dance_03.png");

            aParent.AddChild(sprite4);
            sprite4.AddChild(sprite5, -2);
            sprite4.AddChild(sprite6, 2);

        }

        #endregion Constructors


        #region Setup content

        public override void OnEnter()
        {
base.OnEnter(); CCSize windowSize = Layer.VisibleBoundsWorldspace.Size;

            aParent.Position = (new CCPoint(windowSize.Width / 3, windowSize.Height / 2));
            aParent2.Position = (new CCPoint(2 * windowSize.Width / 3, windowSize.Height / 2));

            sprite1.Position = (new CCPoint(0, 0));
            sprite2.Position = (new CCPoint(20, 30));
            sprite3.Position = (new CCPoint(-20, 30));
            sprite4.Position = (new CCPoint(0, 0));
            sprite5.Position = (new CCPoint(20, 30));
            sprite6.Position = (new CCPoint(-20, 30));

            sprite1.RunAction(new CCBlink (5, 10));
            sprite4.RunAction(new CCBlink (5, 10));
        }

        #endregion Setup content


        public override void OnExit()
        {
            base.OnExit();
            CCSpriteFrameCache.SharedSpriteFrameCache.RemoveUnusedSpriteFrames();
        }
    }
}
