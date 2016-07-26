using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;
using Microsoft.Xna.Framework;

namespace tests
{
    public class SpriteNilTexture : SpriteTestDemo
    {
        CCSprite sprite1;
        CCSprite sprite2;

        #region Properties

        public override string Title
        {
            get { return "Sprite without texture"; }
        }

        public override string Subtitle
        {
            get { return "opacity and color should work"; }
        }

        #endregion Properties


        #region Constructors

        public SpriteNilTexture()
        {
            sprite1 = new CCSprite();
            sprite1.TextureRectInPixels = new CCRect(0, 0, 300, 300);
            sprite1.Color = CCColor3B.Red;
            sprite1.Opacity = 128;
            sprite1.AnchorPoint = CCPoint.AnchorMiddle;
            AddChild(sprite1, 100);

            sprite2 = new CCSprite();
            sprite2.TextureRectInPixels = new CCRect(0, 0, 300, 300);
            sprite2.Color = CCColor3B.Blue;
            sprite2.Opacity = 128;
            sprite2.AnchorPoint = CCPoint.AnchorMiddle;
            AddChild(sprite2, 100);
        }

        #endregion Constructors


        #region Setup content

        public override void OnEnter()
        {
            base.OnEnter(); 

            var windowSize = VisibleBoundsWorldspace.Size;

            sprite1.Position = (new CCPoint(3 * windowSize.Width / 4, windowSize.Height / 2));
            sprite2.Position = (new CCPoint(1 * windowSize.Width / 4, windowSize.Height / 2));
        }

        #endregion Setup content
    }
}
