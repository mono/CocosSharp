using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
    public class TestLayer : CCLayer
    {
		public TestLayer()
		{}

        public override void OnEnter()
        {
            base.OnEnter();
            float x, y;
            CCSize size = Layer.VisibleBoundsWorldspace.Size;
            x = size.Width;
            y = size.Height;
            //CCMutableArray *array = [UIFont familyNames];
            //for( CCString *s in array )
            //	NSLog( s );
            CCLabelTtf label = new CCLabelTtf("cocos2d", "arial", 64);
            label.Position = new CCPoint(x / 2, y / 2);
            AddChild(label);
        }


    }
}
