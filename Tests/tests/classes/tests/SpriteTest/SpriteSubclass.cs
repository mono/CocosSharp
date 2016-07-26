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
            CCSpriteFrame frame = CCSpriteFrameCache.SharedSpriteFrameCache[spriteFrameName];
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
            CCSpriteFrameCache.SharedSpriteFrameCache.AddSpriteFrames("animations/ghosts.plist");

            // MySprite1
            sprite = MySprite1.Create("father.gif");
            AddChild(sprite);

            // MySprite2
            sprite2 = new MySprite2("Images/grossini");
            AddChild(sprite2);
        }

        #endregion Constructors


        #region Setup content

        public override void OnEnter()
        {
            base.OnEnter(); CCSize windowSize = Layer.VisibleBoundsWorldspace.Size;
            sprite.Position = (new CCPoint(windowSize.Width / 4 * 1, windowSize.Height / 2));
            sprite2.Position = (new CCPoint(windowSize.Width / 4 * 3, windowSize.Height / 2));
        }

        #endregion Setup content
    }


    public class SpriteUntrimmedSizeInPixels : SpriteTestDemo
    {

        // Create new CCSprite to access the protected property UntrimmedSizeInPixels
        class MySprite3 : CCSprite
        {

            public MySprite3(string fileName): base(fileName)
            {   }

            public CCSize UntrimmedSizeInPixels
            {
                get { return base.UntrimmedSizeInPixels; }
                set 
                { 
                    base.UntrimmedSizeInPixels = value;
                }
            }
        }


        MySprite3 sprite3;

        #region Properties

        public override string Title
        {
            get { return "Sprite subclass"; }
        }

        public override string Subtitle
        {
            get { return "Testing UntrimmedSizeInPixels\ntouch and drag on the screen\nto see sprite resize."; }
        }

        #endregion Properties


        #region Constructors

        public SpriteUntrimmedSizeInPixels()
        {
            // MySprite3
            sprite3 = new MySprite3("Images/grossini");
            AddChild(sprite3);

            // Register Touch Event
            var touchListener = new CCEventListenerTouchAllAtOnce();
            touchListener.OnTouchesMoved = OnTouchesMoved;

            AddEventListener(touchListener);
        }

        #endregion Constructors


        #region Setup content

        public override void OnEnter()
        {
            base.OnEnter(); 
            CCSize windowSize = Layer.VisibleBoundsWorldspace.Size;
            sprite3.Position = windowSize.Center;
        }

        #endregion Setup content

        void OnTouchesMoved(List<CCTouch> touches, CCEvent touchEvent)
        {
            foreach (CCTouch touch in touches)
            {
                var delta = -touch.Delta;
                var newWidth = sprite3.UntrimmedSizeInPixels.Width + delta.X;
                var newHeight = sprite3.UntrimmedSizeInPixels.Height + delta.Y;
                var newSize = new CCSize(newWidth, newHeight);
                sprite3.UntrimmedSizeInPixels = newSize;
            }
        }
    }
}
