using System;
using CocosSharp;
using Microsoft.Xna.Framework;

namespace $safeprojectname$
{
	public class IntroLayer : CCLayerColor
	{
		CCLabelTtf label;

		public IntroLayer () 
		{

			// create and initialize a Label
			label = new CCLabelTtf("Hello CocosSharp", "MarkerFelt", 22);
			label.AnchorPoint = CCPoint.AnchorMiddle;

			// add the label as a child to this Layer
			AddChild(label);

			// setup our color for the background
			Color = new CCColor3B (CCColor4B.Blue);
			Opacity = 255;

		}
	        protected override void AddedToScene()
        	{
	        	base.AddedToScene();

			var windowSize = VisibleBoundsWorldspace.Size;

			// position the label on the center of the screen
			label.Position = windowSize.Center;
        	}

		public static CCScene CreateScene (CCWindow mainWindow) 
		{
			var scene = new CCScene(mainWindow);
			var layer = new IntroLayer();

			// add layer as a child to scene
			scene.AddChild(layer);

			// return the scene
			return scene;
		}

	}
}

