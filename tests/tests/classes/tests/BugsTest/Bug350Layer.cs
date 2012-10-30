using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class Bug350Layer : BugsTestBaseLayer
    {
        public virtual bool init()
        {
            if (base.Init())
            {
                CCSize size = CCDirector.SharedDirector.WinSize;
                CCSprite background = CCSprite.Create("Hello");
                background.Position = new CCPoint(size.width / 2, size.height / 2);
                AddChild(background);
                return true;
            }

            return false;
        }
    }
}
