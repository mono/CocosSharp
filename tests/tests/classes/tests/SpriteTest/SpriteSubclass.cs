using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
    internal class MySprite1 : CCSprite
    {
        int ivar;

        public MySprite1(CCSpriteFrame frame): base(frame)
        {
            ivar = 10;
        }

        public static MySprite1 Create(string spriteFrameName)
        {
            CCSpriteFrame frame = CCApplication.SharedApplication.SpriteFrameCache[spriteFrameName];
            MySprite1 sprite = new MySprite1(frame);

            return sprite;
        }

    }

    internal class MySprite2 : CCSprite
    {
        int ivar;

        public MySprite2(string fileName): base(fileName)
        {
            ivar = 10;
        }
    }

    public class SpriteSubclass : SpriteTestDemo
    {
        MySprite1 sprite;
        MySprite2 sprite2;

        #region Properties

        public override string Title
        {
            get { return "Sprite subclass"; }
        }

        public override string Subtitle
        {
            get { return "Testing initWithTexture:rect method"; }
        }

        #endregion Properties


        #region Constructors

        public SpriteSubclass()
        {
            CCApplication.SharedApplication.SpriteFrameCache.AddSpriteFrames("animations/ghosts.plist");
            CCSpriteBatchNode aParent = new CCSpriteBatchNode("animations/ghosts");

            // MySprite1
            sprite = MySprite1.Create("father.gif");
            aParent.AddChild(sprite);
            AddChild(aParent);

            // MySprite2
            sprite2 = new MySprite2("Images/grossini");
            AddChild(sprite2);
        }

        #endregion Constructors


        #region Setup content

        protected override void RunningOnNewWindow(CCSize windowSize)
        {
            base.RunningOnNewWindow (windowSize);
            sprite.Position = (new CCPoint(windowSize.Width / 4 * 1, windowSize.Height / 2));
            sprite2.Position = (new CCPoint(windowSize.Width / 4 * 3, windowSize.Height / 2));
        }

        #endregion Setup content
    }
}
