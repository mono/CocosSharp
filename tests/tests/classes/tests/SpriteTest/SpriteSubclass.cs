using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
    internal class MySprite1 : CCSprite
    {
        private void InitSprite()
        {
            ivar = 10;
        }

        public MySprite1(CCSpriteFrame frame): base(frame)
        {
            InitSprite();
        }

        public static MySprite1 Create(string pszSpriteFrameName)
        {
            CCSpriteFrame pFrame = CCApplication.SharedApplication.SpriteFrameCache[pszSpriteFrameName];
            MySprite1 pobSprite = new MySprite1(pFrame);

            return pobSprite;
        }

        private int ivar;
    }

    internal class MySprite2 : CCSprite
    {
        private void InitSprite()
        {
            ivar = 10;
        }

        public MySprite2(string fileName): base(fileName)
        {
            InitSprite();
        }

        public new static MySprite2 Create(string pszName)
        {
            MySprite2 pobSprite = new MySprite2(pszName);
            return pobSprite;
        }

        private int ivar;
    }

    public class SpriteSubclass : SpriteTestDemo
    {
        public SpriteSubclass()
        {
            CCSize s = CCApplication.SharedApplication.MainWindowDirector.WindowSizeInPoints;

            CCApplication.SharedApplication.SpriteFrameCache.AddSpriteFrames("animations/ghosts.plist");
            CCSpriteBatchNode aParent = new CCSpriteBatchNode("animations/ghosts");

            // MySprite1
            MySprite1 sprite = MySprite1.Create("father.gif");
            sprite.Position = (new CCPoint(s.Width / 4 * 1, s.Height / 2));
            aParent.AddChild(sprite);
            AddChild(aParent);

            // MySprite2
            MySprite2 sprite2 = MySprite2.Create("Images/grossini");
            AddChild(sprite2);
            sprite2.Position = (new CCPoint(s.Width / 4 * 3, s.Height / 2));
        }

        public override string title()
        {
            return "Sprite subclass";
        }

        public override string subtitle()
        {
            return "Testing initWithTexture:rect method";
        }
    }
}
