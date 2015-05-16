using System.Collections.Generic;
using CocosSharp;

namespace tests
{
    public class RenderTextureZbuffer : RenderTextureTestDemo
    {
        readonly CCSpriteBatchNode mgr;

        readonly CCSprite sp1;
        readonly CCSprite sp2;
        readonly CCSprite sp3;
        readonly CCSprite sp4;
        readonly CCSprite sp5;
        readonly CCSprite sp6;
        readonly CCSprite sp7;
        readonly CCSprite sp8;
        readonly CCSprite sp9;

        CCLabelTtf label;
        CCLabelTtf label2;
        CCLabelTtf label3;


        #region Properties

        public override string Title
        {
            get { return "Testing Z Buffer in Render Texture"; }
        }

        public override string Subtitle
        {
            get { return "Touch screen. It should be green"; }
        }

        #endregion Properties


        #region Constructors

        public RenderTextureZbuffer()
        {
            label = new CCLabelTtf("vertexZ = 50", "Marker Felt", 32);
            AddChild(label);

            label2 = new CCLabelTtf("vertexZ = 0", "Marker Felt", 32);
            AddChild(label2);

            label3 = new CCLabelTtf("vertexZ = -50", "Marker Felt", 32);
            AddChild(label3);

            CCSpriteFrameCache.SharedSpriteFrameCache.AddSpriteFrames("Images/bugs/circle.plist");

            mgr = new CCSpriteBatchNode("Images/bugs/circle", 9);
            AddChild(mgr);

            sp1 = new CCSprite("Images/bugs/circle");
            sp2 = new CCSprite("Images/bugs/circle");
            sp3 = new CCSprite("Images/bugs/circle");
            sp4 = new CCSprite("Images/bugs/circle");
            sp5 = new CCSprite("Images/bugs/circle");
            sp6 = new CCSprite("Images/bugs/circle");
            sp7 = new CCSprite("Images/bugs/circle");
            sp8 = new CCSprite("Images/bugs/circle");
            sp9 = new CCSprite("Images/bugs/circle");

            mgr.AddChild(sp1, 9);
            mgr.AddChild(sp2, 8);
            mgr.AddChild(sp3, 7);
            mgr.AddChild(sp4, 6);
            mgr.AddChild(sp5, 5);
            mgr.AddChild(sp6, 4);
            mgr.AddChild(sp7, 3);
            mgr.AddChild(sp8, 2);
            mgr.AddChild(sp9, 1);

            sp9.Color = CCColor3B.Yellow;
        }

        #endregion Constructors


        #region Setup content

        public override void OnEnter()
        {

            base.OnEnter(); 

            CCSize windowSize = Layer.VisibleBoundsWorldspace.Size;

            label.Position = new CCPoint(windowSize.Width / 2, windowSize.Height * 0.25f);
            label2.Position = new CCPoint(windowSize.Width / 2, windowSize.Height * 0.5f);
            label3.Position = new CCPoint(windowSize.Width / 2, windowSize.Height * 0.75f);

            label.VertexZ = 50;
            label2.VertexZ = 0;
            label3.VertexZ = -50;

            sp1.VertexZ = 400;
            sp2.VertexZ = 300;
            sp3.VertexZ = 200;
            sp4.VertexZ = 100;
            sp5.VertexZ = 0;
            sp6.VertexZ = -100;
            sp7.VertexZ = -200;
            sp8.VertexZ = -300;
            sp9.VertexZ = -400;

            sp9.Scale = 2;

            // Register Touch Event
            var touchListener = new CCEventListenerTouchAllAtOnce();

            touchListener.OnTouchesBegan = onTouchesBegan;
            touchListener.OnTouchesMoved = onTouchesMoved;
            touchListener.OnTouchesEnded = onTouchesEnded;

            AddEventListener(touchListener); 
        }

        #endregion Setup content


        #region Event handling

        void onTouchesBegan(List<CCTouch> touches, CCEvent touchEvent)
        {
            foreach (CCTouch touch in touches)
            {
                CCPoint location = touch.Location;

                sp1.Position = location;
                sp2.Position = location;
                sp3.Position = location;
                sp4.Position = location;
                sp5.Position = location;
                sp6.Position = location;
                sp7.Position = location;
                sp8.Position = location;
                sp9.Position = location;
            }
        }

        void onTouchesMoved(List<CCTouch> touches, CCEvent touchEvent)
        {
            foreach (CCTouch touch in touches)
            {
                CCPoint location = touch.Location;

                sp1.Position = location;
                sp2.Position = location;
                sp3.Position = location;
                sp4.Position = location;
                sp5.Position = location;
                sp6.Position = location;
                sp7.Position = location;
                sp8.Position = location;
                sp9.Position = location;
            }
        }

        void onTouchesEnded(List<CCTouch> touches, CCEvent touchEvent)
        {
            renderScreenShot();
        }

        #endregion Event handling


        public void renderScreenShot()
        {
            var size = Layer.VisibleBoundsWorldspace.Size;
            var texture = new CCRenderTexture(size,size);
            //var texture = new CCRenderTexture(512, 512);

            texture.Sprite.AnchorPoint = new CCPoint(0, 0);
            texture.BeginWithClear(CCColor4B.Transparent);

            Visit();

            texture.End();

            CCSprite sprite = new CCSprite(texture.Sprite.Texture);

            //sprite.Position = new CCPoint(256, 256);
            sprite.Position = new CCPoint(size.Width/2, size.Height / 2);
            sprite.Opacity = 182;
            //sprite.IsFlipY = true;
            AddChild(sprite, 999999);
            sprite.Color = CCColor3B.Green;

            sprite.RunAction(
                new CCSequence(
                    new CCFadeTo (2, 0),
                    new CCHide()
                )
            );
        }
    }
}