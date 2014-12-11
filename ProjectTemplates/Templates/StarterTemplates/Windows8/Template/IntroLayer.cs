using System;
using System.Collections.Generic;
using CocosSharp;
using Microsoft.Xna.Framework;

namespace $safeprojectname$
{
	public class IntroLayer : CCLayerColor
	{
		CCLabelTtf label;

        public IntroLayer()
        {

            // create and initialize a Label
            label = new CCLabelTtf("Hello CocosSharp", "MarkerFelt", 22);
            label.AnchorPoint = CCPoint.AnchorMiddle;

            // add the label as a child to this Layer
            AddChild(label);

            // setup our color for the background
            Color = new CCColor3B(CCColor4B.Blue);
            Opacity = 255;

        }

        protected override void AddedToScene()
        {
            base.AddedToScene();

            // Use the bounds to layout the positioning of our drawable assets
            var bounds = VisibleBoundsWorldspace;

            // position the label on the center of the screen
            label.Position = bounds.Center;

            // Register for touch events
            var touchListener = new CCEventListenerTouchAllAtOnce();
            touchListener.OnTouchesEnded = OnTouchesEnded;
            AddEventListener(touchListener, this);
        }

        void OnTouchesEnded(List<CCTouch> touches, CCEvent touchEvent)
        {
            if (touches.Count > 0)
            {
                // Perform touch handling here
            }
        }
	}
}

