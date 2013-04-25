using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cocos2d
{
    public class CCScale9SpriteFrame : CCScale9Sprite
    {
        public CCScale9SpriteFrame(CCSpriteFrame spriteFrame, CCRect capInsets)
        {
            InitWithSpriteFrame(spriteFrame, capInsets);
        }
        public CCScale9SpriteFrame(CCSpriteFrame spriteFrame)
        {
            InitWithSpriteFrame(spriteFrame);
        }

        public CCScale9SpriteFrame(string spriteFrameName, CCRect capInsets)
        {
            InitWithSpriteFrameName(spriteFrameName, capInsets);
        }

        public CCScale9SpriteFrame(string alias)
        {
            InitWithSpriteFrameName(alias);
        }
    }
}
