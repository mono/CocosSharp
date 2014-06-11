using System;
using System.Collections.Generic;

namespace CocosSharp
{
    public class CCScale9SpriteFrame : CCScale9Sprite
    {
        #region Constructors

        public CCScale9SpriteFrame(CCSpriteFrame spriteFrame, CCRect capInsets) : base(spriteFrame, capInsets)
        {
        }

        public CCScale9SpriteFrame(CCSpriteFrame spriteFrame) : base(spriteFrame)
        {
        }

        public CCScale9SpriteFrame(string spriteFrameName, CCRect capInsets) : base()
        {
            // Can't call base(string,...) because we're using the string parameter for file names
            base.InitWithSpriteFrameName(spriteFrameName, capInsets);
        }

        public CCScale9SpriteFrame(string alias)
        {
            // Can't call base(string,...) because we're using the string parameter for file names
            base.InitWithSpriteFrameName(alias);
        }

        #endregion Constructors
    }
}
