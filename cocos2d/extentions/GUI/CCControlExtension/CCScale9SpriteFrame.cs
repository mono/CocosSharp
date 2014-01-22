using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CocosSharp
{
    public class CCScale9SpriteFrame : CCScale9Sprite
    {
        public CCScale9SpriteFrame(CCSpriteFrame spriteFrame, CCRect capInsets) : base(spriteFrame, capInsets)
        {
        }

        public CCScale9SpriteFrame(CCSpriteFrame spriteFrame) : base(spriteFrame)
        {
        }

        public CCScale9SpriteFrame(string spriteFrameName, CCRect capInsets) : base()
        {
            // Can't call base(string,...) because we're using that for file names
            base.InitWithSpriteFrameName(spriteFrameName, capInsets);
        }

        public CCScale9SpriteFrame(string alias)
        {
            // Can't call base(string,...) because we're using that for file names
            base.InitWithSpriteFrameName(alias);
        }
    }
}
