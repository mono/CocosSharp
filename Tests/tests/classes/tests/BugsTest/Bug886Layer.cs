using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
    public class Bug886Layer : BugsTestBaseLayer
    {

        public Bug886Layer()
        {
            // ask director the the window size
            //		CGSize size = [[CCDirector sharedDirector] winSize];

            CCSprite sprite = new CCSprite("Images/bugs/bug886");
            sprite.AnchorPoint = new CCPoint(0, 0);
            sprite.Position = new CCPoint(0, 0);
            sprite.ScaleX = 0.6f;
            AddChild(sprite);

            CCSprite sprite2 = new CCSprite("Images/bugs/bug886");
            sprite2.AnchorPoint = new CCPoint(0, 0);
            sprite2.ScaleX = 0.6f;
            sprite2.Position = new CCPoint(sprite.ContentSize.Width * 0.6f + 10, 0);
            AddChild(sprite2);

        }

    }
}
