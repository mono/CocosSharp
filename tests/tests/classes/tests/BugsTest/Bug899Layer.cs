using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cocos2D;

namespace tests
{
    public class Bug899Layer : BugsTestBaseLayer
    {
        public override bool Init()
        {
            //CCDirector.SharedDirector.EnableRetinaDisplay(true);
            if (base.Init())
            {
                CCSprite bg = new CCSprite("Images/bugs/RetinaDisplay");
                AddChild(bg, 0);
                bg.AnchorPoint = new CCPoint(0, 0);

                return true;
            }
            return false;
        }
    }
}
