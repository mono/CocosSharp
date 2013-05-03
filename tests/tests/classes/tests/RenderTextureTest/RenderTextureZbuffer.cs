using System.Collections.Generic;
using cocos2d;

namespace tests
{
    public class RenderTextureZbuffer : RenderTextureTestDemo
    {
        private readonly CCSpriteBatchNode mgr;

        private readonly CCSprite sp1;
        private readonly CCSprite sp2;
        private readonly CCSprite sp3;
        private readonly CCSprite sp4;
        private readonly CCSprite sp5;
        private readonly CCSprite sp6;
        private readonly CCSprite sp7;
        private readonly CCSprite sp8;
        private readonly CCSprite sp9;

        public RenderTextureZbuffer()
        {
            //this->setIsTouchEnabled(true);
            TouchEnabled = true;
            CCSize size = CCDirector.SharedDirector.WinSize;
            CCLabelTTF label = new CCLabelTTF("vertexZ = 50", "Marker Felt", 32);
            label.Position = new CCPoint(size.Width / 2, size.Height * 0.25f);
            AddChild(label);

            CCLabelTTF label2 = new CCLabelTTF("vertexZ = 0", "Marker Felt", 32);
            label2.Position = new CCPoint(size.Width / 2, size.Height * 0.5f);
            AddChild(label2);

            CCLabelTTF label3 = new CCLabelTTF("vertexZ = -50", "Marker Felt", 32);
            label3.Position = new CCPoint(size.Width / 2, size.Height * 0.75f);
            AddChild(label3);

            label.VertexZ = 50;
            label2.VertexZ = 0;
            label3.VertexZ = -50;

            CCSpriteFrameCache.SharedSpriteFrameCache.AddSpriteFramesWithFile("Images/bugs/circle.plist");

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
            sp9.Color = CCTypes.CCYellow;
        }

        public override void TouchesBegan(List<CCTouch> touches, CCEvent events)
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

        public override void TouchesMoved(List<CCTouch> touches, CCEvent events)
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

        public override void TouchesEnded(List<CCTouch> touches, CCEvent events)
        {
            renderScreenShot();
        }

        public override string title()
        {
            return "Testing Z Buffer in Render Texture";
        }

        public override string subtitle()
        {
            return "Touch screen. It should be green";
        }

        public void renderScreenShot()
        {
            var size = CCDirector.SharedDirector.WinSize;
            var texture = CCRenderTexture.Create((int)size.Width, (int)size.Height);
            //var texture = CCRenderTexture.Create(512, 512);

            texture.AnchorPoint = new CCPoint(0, 0);
            texture.Begin();

            Visit();

            texture.End();

            CCSprite sprite = new CCSprite(texture.Sprite.Texture);

            //sprite.Position = new CCPoint(256, 256);
            sprite.Position = new CCPoint(size.Width/2, size.Height / 2);
            sprite.Opacity = 182;
            //sprite.IsFlipY = true;
            AddChild(sprite, 999999);
            sprite.Color = CCTypes.CCGreen;

            sprite.RunAction(
                CCSequence.FromActions(
                    new CCFadeTo (2, 0),
                    new CCHide()
                    )
                );
        }
    }
}