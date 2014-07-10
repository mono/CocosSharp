using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
    public class Bug899Layer : BugsTestBaseLayer
    {
        public Bug899Layer()
        {
            //Scene.Director.EnableRetinaDisplay(true);
            CCSprite bg = new CCSprite("Images/bugs/RetinaDisplay");
            AddChild(bg, 0);
            bg.AnchorPoint = new CCPoint(0, 0);

        }
    }
}
