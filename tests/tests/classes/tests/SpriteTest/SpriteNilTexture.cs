using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;
using Microsoft.Xna.Framework;

namespace tests
{
    public class SpriteNilTexture : SpriteTestDemo
    {
        public SpriteNilTexture()
        {
            CCSize s = CCDirector.SharedDirector.WinSize;

            CCSprite sprite = null;

            // TEST: If no texture is given, then Opacity + Color should work.

            sprite = new CCSprite();
            sprite.Init();
            sprite.TextureRect = new CCRect(0, 0, 300, 300);
            sprite.Color = ccTypes.ccRED;
            sprite.Opacity = 128;
            sprite.Position = (new CCPoint(3 * s.width / 4, s.height / 2));
            AddChild(sprite, 100);

            sprite = new CCSprite();
            sprite.Init();
            sprite.TextureRect = new CCRect(0, 0, 300, 300);
            sprite.Color = ccTypes.ccBLUE;
            sprite.Opacity = 128;
            sprite.Position = (new CCPoint(1 * s.width / 4, s.height / 2));
            AddChild(sprite, 100);
        }

        public override string title()
        {
            return "Sprite without texture";
        }

        public override string subtitle()
        {
            return "opacity and color should work";
        }
    }
}
