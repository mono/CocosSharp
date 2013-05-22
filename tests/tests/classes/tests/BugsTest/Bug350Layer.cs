using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cocos2D;

namespace tests
{
    public class Bug350Layer : BugsTestBaseLayer
    {
        public virtual bool init()
        {
            if (base.Init())
            {
                CCSize size = CCDirector.SharedDirector.WinSize;
                CCSprite background = new CCSprite("Hello");
                background.Position = new CCPoint(size.Width / 2, size.Height / 2);
                AddChild(background);
                return true;
            }

            return false;
        }
    }
}
