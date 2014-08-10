using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
    public class TestLayer : CCNode
    {
		public TestLayer()
		{}

        public override void OnEnter()
        {
            base.OnEnter();
            CCSize size = Layer.VisibleBoundsWorldspace.Size;
            CCLabelTtf label = new CCLabelTtf("cocos2d", "arial", 64);
            label.Position = size.Center;
            label.AnchorPoint = CCPoint.AnchorMiddle;
            AddChild(label);
        }


    }
}
