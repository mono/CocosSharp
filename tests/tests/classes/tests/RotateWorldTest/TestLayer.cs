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

            var label = new CCLabel("CocosSharp", "arial", 64, CCLabelFormat.SpriteFont);
            label.Position = VisibleBoundsWorldspace.Center;
            AddChild(label);
        }


    }
}
