using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class Bug899Layer : BugsTestBaseLayer
    {
        public override bool Init()
        {
            //CCDirector.SharedDirector.EnableRetinaDisplay(true);
            if (base.Init())
            {
                CCSprite bg = CCSprite.Create("Images/bugs/RetinaDisplay");
                AddChild(bg, 0);
                bg.AnchorPoint = new CCPoint(0, 0);

                return true;
            }
            return false;
        }
    }
}
