using System;
using System.Collections.Generic;
using CocosSharp;

namespace GoneBananas
{
	public class GameOverLayer : CCLayerColor
	{
		public GameOverLayer (int score)
		{

			var touchListener = new CCEventListenerTouchAllAtOnce();
			touchListener.OnTouchesEnded = (touches, ccevent) => CCDirector.SharedDirector.ReplaceScene (GameLayer.Scene);

			EventDispatcher.AddEventListener(touchListener, this);

			string scoreMessage = String.Format ("Game Over. You collected {0} bananas!", score);
       
            var scoreLabel = new CCLabelTtf (scoreMessage, "MarkerFelt", 22) {
				Position = new CCPoint( CCDirector.SharedDirector.WinSize.Center.X,  CCDirector.SharedDirector.WinSize.Center.Y + 50),
                Color = new CCColor3B (CCColor4B.Yellow),
				HorizontalAlignment = CCTextAlignment.Center,
				VerticalAlignment = CCVerticalTextAlignment.Center,
				Dimensions = ContentSize
			};

			AddChild (scoreLabel);

            var playAgainLabel = new CCLabelTtf ("Tap to Play Again", "MarkerFelt", 22) {
				Position = CCDirector.SharedDirector.WinSize.Center,
                Color = new CCColor3B (CCColor4B.Green),
				HorizontalAlignment = CCTextAlignment.Center,
				VerticalAlignment = CCVerticalTextAlignment.Center,
				Dimensions = ContentSize
			};

			AddChild (playAgainLabel);

            Color = new CCColor3B (CCColor4B.Black);
			Opacity = 255;

		}

		public static CCScene SceneWithScore (int score)
		{
			var scene = new CCScene ();
			var layer = new GameOverLayer (score);

			scene.AddChild (layer);

			return scene;
		}
	}
}

