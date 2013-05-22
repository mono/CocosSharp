using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cocos2D
{
    public class CCScale9SpriteFile : CCScale9Sprite
    {
        public CCScale9SpriteFile(string file, CCRect rect, CCRect capInsets)
        {
            InitWithFile(file, rect, capInsets);
        }
        public CCScale9SpriteFile(string file, CCRect rect)
        {
            InitWithFile(file, rect);
        }
        public CCScale9SpriteFile(CCRect capInsets, string file)
        {
            InitWithFile(file, capInsets);
        }
        public CCScale9SpriteFile(string file)
        {
            InitWithFile(file);
        }
    }
}
