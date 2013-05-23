using System;
using Cocos2D;
using Microsoft.Xna.Framework;

namespace GameStarterKit
{
	public class IntroLayer : CCLayerColor
	{
		public IntroLayer ()
		{

			// create and initialize a Label
			var label = new CCLabelTTF("Intro Layer", "MarkerFelt", 22);

			// position the label on the center of the screen
			label.Position = CCDirector.SharedDirector.WinSize.Center;

			// add the label as a child to this Layer
			AddChild(label);

			// setup our color for the background
			Color = new CCColor3B (Microsoft.Xna.Framework.Color.Blue);
			Opacity = 255;

			// set the size of our content area
			ContentSize = CCDirector.SharedDirector.WinSize;

			// Wait a little and then transition to the new scene
			ScheduleOnce (TransitionOut, 2);
		}

		void TransitionOut(float delta)
		{

			CCLog.Log("Make Transition to Game Level");

			// Too select a random transition comment the two lines below and uncomment the section below.
//			var transition = Transition2;
//			CCDirector.SharedDirector.ReplaceScene(transition);


			// other transition options...

		    int diceRoll = Cocos2D.CCRandom.Next(0,6); //0 to 6
			CCTransitionScene transition;

		    switch (diceRoll) {
		        case 0:
					transition = Transition0;
		            break;
		        case 1:
					transition = Transition1;
		            break;
		        case 2:
					transition = Transition2;
		            break;
		        case 3:
					transition = Transition3;
		            break;
		        case 4:
					transition = Transition4;
		            break;
		            
		        default:
					transition = Transition0;
		            break;
		    }
     
			CCDirector.SharedDirector.ReplaceScene(transition);
		}

		CCTransitionScene Transition0
		{
			get { return new CCTransitionFadeDown(1, GameLevel.Scene); }
		}

		CCTransitionScene Transition1
		{
			get { return new CCTransitionFlipX(1, GameLevel.Scene, CCTransitionOrientation.RightOver); }
		}

		CCTransitionScene Transition2 
		{
			get	{ return new CCTransitionFade (1 , GameLevel.Scene, new CCColor3B(Microsoft.Xna.Framework.Color.White)); }
		}

		CCTransitionScene Transition3 
		{
			get	{ return new CCTransitionFlipAngular (1 , GameLevel.Scene, CCTransitionOrientation.DownOver); }
		}

		CCTransitionScene Transition4 
		{
			get { return new CCTransitionFadeTR (1 , GameLevel.Scene); }
		}

		CCTransitionScene Transition5 
		{
			get { return new CCTransitionPageTurn (1 , GameLevel.Scene, false); }
		}

		CCTransitionScene Transition6
		{
			get { return new CCTransitionTurnOffTiles (1 , GameLevel.Scene); }
		}

		public static CCScene Scene 
		{
			get {
				// 'scene' is an autorelease object.
				var scene = new CCScene();

				// 'layer' is an autorelease object.
				var layer = new IntroLayer();

				// add layer as a child to scene
				scene.AddChild(layer);

				// return the scene
				return scene;

			}

		}

	}
}

