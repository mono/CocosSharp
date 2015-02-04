using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using CocosSharp;

namespace tests
{
    public class RenderTextureSave : RenderTextureTestDemo
    {
        static int counter = 0;

        readonly CCSprite brush;

        CCRenderTexture target;
        CCMenu saveImageMenu;


        #region Properties

        public override string Title
        {
            get { return "Touch the screen"; }
        }

        public override string Subtitle
        {
            get { return "Press 'Save Image' to create an snapshot of the render texture"; }
        }

        #endregion Properties


        #region Constructors

        public RenderTextureSave()
        {
            // create a brush image to draw into the texture with
            brush = new CCSprite("Images/fire");
            // It's possible to modify the Brushes blending function by
            CCBlendFunc bbf = new CCBlendFunc (CCOGLES.GL_ONE, CCOGLES.GL_ONE_MINUS_SRC_ALPHA);
            brush.BlendFunc = bbf;

            brush.Color = CCColor3B.Red;
            brush.Opacity = 20;

            AddChild(brush);

            // Save image menu
            CCMenuItemFont.FontSize = 16;
            CCMenuItemFont.FontName = "arial";
            CCMenuItem item1 = new CCMenuItemFont("Save Image", SaveImage);
            CCMenuItem item2 = new CCMenuItemFont("Clear", ClearImage);

            saveImageMenu = new CCMenu(item1, item2);
            AddChild(saveImageMenu);
        }

        #endregion Constructors


        #region Setup content

        public override void OnEnter()
        {
            base.OnEnter(); CCSize windowSize = Layer.VisibleBoundsWorldspace.Size;

            // create a render texture, this is what we are going to draw into
            target = new CCRenderTexture(windowSize, windowSize,
                CCSurfaceFormat.Color, 
                CCDepthFormat.None, CCRenderTargetUsage.PreserveContents);

            // Let's clear the rendertarget here so that we start off fresh.
            // Some platforms do not seem to be initializing the rendertarget color so this will make sure the background shows up colored instead of 
            // what looks like non initialized.  Mostly MacOSX for now.
            target.Clear(0,0,0,255);

            target.Position = new CCPoint(windowSize.Width / 2, windowSize.Height / 2);

            target.AnchorPoint = CCPoint.AnchorMiddle;

            // It's possible to modify the RenderTexture blending function by
            //CCBlendFunc tbf = new CCBlendFunc (OGLES.GL_ONE, OGLES.GL_ONE_MINUS_SRC_ALPHA);
            //target.Sprite.BlendFunc = tbf;

            // note that the render texture is a CCNode, and contains a sprite of its texture for convience,
            // so we can just parent it to the scene like any other CCNode
            AddChild(target, -1);

            saveImageMenu.AlignItemsVertically();
            saveImageMenu.Position = new CCPoint(windowSize.Width - 80, windowSize.Height - 30);

            // Register Touch Event
            var touchListener = new CCEventListenerTouchAllAtOnce();
            touchListener.OnTouchesMoved = OnTouchesMoved;

            AddEventListener(touchListener);
        }

        #endregion Setup content


        #region Event handling

        void OnTouchesMoved(List<CCTouch> touches, CCEvent touchEvent)
        {
            CCTouch touch = touches[0];
            CCPoint start = touch.Location;
            CCPoint end = touch.PreviousLocation;

            // begin drawing to the render texture
            target.Begin();

            // for extra points, we'll draw this smoothly from the last position and vary the sprite's
            // scale/rotation/offset
            float distance = CCPoint.Distance(start, end);
            if (distance > 1)
            {
                var d = (int) distance;
                for (int i = 0; i < d; i++)
                {
                    float difx = end.X - start.X;
                    float dify = end.Y - start.Y;
                    float delta = i / distance;
                    brush.Position = new CCPoint(start.X + (difx * delta), start.Y + (dify * delta));
                    brush.Rotation = CCRandom.Next() % 360;
                    float r = (CCRandom.Next() % 50 / 50f) + 0.25f;
                    brush.Scale = r;

                    // Comment out the following line to show just the initial red color set
                    brush.Color = new CCColor3B((byte) (CCRandom.Next() % 127 + 128), 255, 255);

                    // Call visit to draw the brush, don't call draw..
                    brush.Visit();
                }
            }

            // finish drawing and return context back to the screen
            target.End();
        }

        #endregion Event handling


        void ClearImage(object sender)
        {
            target.Clear(CCMacros.CCRandomBetween0And1(), CCMacros.CCRandomBetween0And1(), 
                CCMacros.CCRandomBetween0And1(), CCMacros.CCRandomBetween0And1());
        }

        void SaveImage(object sender)
        {
            using (var stream = new MemoryStream())
            {
                target.SaveToStream(stream, CCImageFormat.Png);

                stream.Position = 0;

                var tex = new CCTexture2D(stream);

                CCSprite sprite = new CCSprite(tex);

                sprite.Scale = 0.3f;
                AddChild(sprite);
                sprite.Position = new CCPoint(40, 40);
                sprite.Rotation = counter * 3;
            }
            counter++;
        }
    }
}