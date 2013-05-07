using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using cocos2d;

namespace tests
{
    public class RenderTextureSave : RenderTextureTestDemo
    {
        private readonly CCSprite m_pBrush;
        private readonly CCRenderTexture m_pTarget;
        private static int counter = 0;

        public RenderTextureSave()
        {
            CCSize s = CCDirector.SharedDirector.WinSize;

            // create a render texture, this is what we are going to draw into
            m_pTarget = new CCRenderTexture((int) s.Width, (int) s.Height, SurfaceFormat.Color, DepthFormat.None, RenderTargetUsage.PreserveContents);

			// Let's clear the rendertarget here so that we start off fresh.
			// Some platforms do not seem to be initializing the rendertarget color so this will make sure the background shows up colored instead of 
			// what looks like non initialized.  Mostly MacOSX for now.
			m_pTarget.Clear(0,0,0,255);

            m_pTarget.Position = new CCPoint(s.Width / 2, s.Height / 2);

            // It's possible to modify the RenderTexture blending function by
            //CCBlendFunc tbf = new CCBlendFunc (OGLES.GL_ONE, OGLES.GL_ONE_MINUS_SRC_ALPHA);
            //m_pTarget.Sprite.BlendFunc = tbf;

            // note that the render texture is a CCNode, and contains a sprite of its texture for convience,
            // so we can just parent it to the scene like any other CCNode
            AddChild(m_pTarget, -1);

            // create a brush image to draw into the texture with
            m_pBrush = new CCSprite("Images/fire");
            // It's possible to modify the Brushes blending function by
            CCBlendFunc bbf = new CCBlendFunc (OGLES.GL_ONE, OGLES.GL_ONE_MINUS_SRC_ALPHA);
            m_pBrush.BlendFunc = bbf;

            m_pBrush.Color = new CCColor3B (Color.Red);
            m_pBrush.Opacity = 20;
            TouchEnabled = true;

            // Save Image menu
            CCMenuItemFont.FontSize = 16;
            CCMenuItem item1 = new CCMenuItemFont("Save Image", saveImage);
            CCMenuItem item2 = new CCMenuItemFont("Clear", clearImage);
            var menu = new CCMenu(item1, item2);
            AddChild(menu);
            menu.AlignItemsVertically();
            menu.Position = new CCPoint(s.Width - 80, s.Height - 30);
        }

        public override string title()
        {
            return "Touch the screen";
        }

        public override string subtitle()
        {
            return "Press 'Save Image' to create an snapshot of the render texture";
        }

        public override void TouchesMoved(List<CCTouch> touches, CCEvent events)
        {
            CCTouch touch = touches[0];
            CCPoint start = touch.Location;
            CCPoint end = touch.PreviousLocation;

            // begin drawing to the render texture
            m_pTarget.Begin();

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
                    m_pBrush.Position = new CCPoint(start.X + (difx * delta), start.Y + (dify * delta));
                    m_pBrush.Rotation = Random.Next() % 360;
                    float r = (Random.Next() % 50 / 50f) + 0.25f;
                    m_pBrush.Scale = r;

                    // Comment out the following line to show just the initial red color set
                    m_pBrush.Color = new CCColor3B((byte) (Random.Next() % 127 + 128), 255, 255);

                    // Call visit to draw the brush, don't call draw..
                    m_pBrush.Visit();
                }
            }

            // finish drawing and return context back to the screen
            m_pTarget.End();
        }

        public void clearImage(object pSender)
        {
            m_pTarget.Clear(CCMacros.CCRandomBetween0And1(), CCMacros.CCRandomBetween0And1(), CCMacros.CCRandomBetween0And1(), CCMacros.CCRandomBetween0And1());
        }

        public void saveImage(object pSender)
        {
            using (var stream = new MemoryStream())
            {

                m_pTarget.SaveToStream(stream, ImageFormat.PNG);
                //m_pTarget.saveToFile(jpg, ImageFormat.JPG);

                stream.Position = 0;

                Texture2D xnatex = Texture2D.FromStream(DrawManager.graphicsDevice, stream);
                var tex = new CCTexture2D();
                tex.InitWithTexture(xnatex);
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