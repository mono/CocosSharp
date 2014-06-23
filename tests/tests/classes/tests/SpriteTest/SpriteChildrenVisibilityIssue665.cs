using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
    public class SpriteChildrenVisibilityIssue665 : SpriteTestDemo
    {
        CCNode aParent, aParent2;
        CCSprite sprite1, sprite2, sprite3, sprite4, sprite5, sprite6;


        #region Properties

        public override string Title
        {
            get { return "Sprite & SpriteBatchNode Visibility"; }
        }
        public override string Subtitle
        {
            get { return "No sprites should be visible"; }
        }

        #endregion Properties


        #region Constructors

        public SpriteChildrenVisibilityIssue665()
        {
            CCApplication.SharedApplication.SpriteFrameCache.AddSpriteFrames("animations/grossini.plist");


            aParent = new CCSpriteBatchNode("animations/grossini", 50);
            AddChild(aParent, 0);

            sprite1 = new CCSprite("grossini_dance_01.png");
            sprite2 = new CCSprite("grossini_dance_02.png");
            sprite3 = new CCSprite("grossini_dance_03.png");

            // test issue #665
            sprite1.Visible = false;

            aParent.AddChild(sprite1);
            sprite1.AddChild(sprite2, -2);
            sprite1.AddChild(sprite3, 2);

            aParent2 = new CCNode();
            AddChild(aParent, 0);

            sprite4 = new CCSprite("grossini_dance_01.png");
            sprite5 = new CCSprite("grossini_dance_02.png");
            sprite6 = new CCSprite("grossini_dance_03.png");

            // test issue #665
            sprite4.Visible = false;

            aParent2.AddChild(sprite4);
            sprite4.AddChild(sprite5, -2);
            sprite4.AddChild(sprite6, 2);
        }

        #endregion Constructors


        #region Setup content

        protected override void RunningOnNewWindow(CCSize windowSize)
        {
            base.RunningOnNewWindow (windowSize);

            sprite1.Position = (new CCPoint(0, 0));
            sprite2.Position = (new CCPoint(20, 30));
            sprite3.Position = (new CCPoint(-20, 30));
            sprite4.Position = (new CCPoint(0, 0));
            sprite5.Position = (new CCPoint(20, 30));
            sprite6.Position = (new CCPoint(-20, 30));

            aParent.Position = (new CCPoint(windowSize.Width / 3, windowSize.Height / 2));
            aParent2.Position = (new CCPoint(2 * windowSize.Width / 3, windowSize.Height / 2));
        }

        #endregion Setup content
    }
}
