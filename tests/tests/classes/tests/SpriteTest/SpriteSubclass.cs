using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    internal class MySprite1 : CCSprite
    {
        public MySprite1()
        {
            ivar = 10;
        }

        public static MySprite1 Create(string pszSpriteFrameName)
        {
            CCSpriteFrame pFrame = CCSpriteFrameCache.SharedSpriteFrameCache.SpriteFrameByName(pszSpriteFrameName);
            MySprite1 pobSprite = new MySprite1();
            pobSprite.InitWithSpriteFrame(pFrame);

            return pobSprite;
        }

        private int ivar;
    }

    internal class MySprite2 : CCSprite
    {

        public MySprite2()
        {
            ivar = 10;
        }

        public new static MySprite2 Create(string pszName)
        {
            MySprite2 pobSprite = new MySprite2();
            pobSprite.InitWithFile(pszName);
            return pobSprite;
        }

        private int ivar;
    }

    public class SpriteSubclass : SpriteTestDemo
    {
        public SpriteSubclass()
        {
            CCSize s = CCDirector.SharedDirector.WinSize;

            CCSpriteFrameCache.SharedSpriteFrameCache.AddSpriteFramesWithFile("animations/ghosts.plist");
            CCSpriteBatchNode aParent = CCSpriteBatchNode.Create("animations/ghosts");

            // MySprite1
            MySprite1 sprite = MySprite1.Create("father.gif");
            sprite.Position = (new CCPoint(s.width / 4 * 1, s.height / 2));
            aParent.AddChild(sprite);
            AddChild(aParent);

            // MySprite2
            MySprite2 sprite2 = MySprite2.Create("Images/grossini");
            AddChild(sprite2);
            sprite2.Position = (new CCPoint(s.width / 4 * 3, s.height / 2));
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
