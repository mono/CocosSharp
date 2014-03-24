using System;
using System.Collections.Generic;
using CocosDenshion;
using CocosSharp;

namespace GoneBananas
{
	public class GameLayer : CCLayerColor
	{
		const float MONKEY_SPEED = 500.0f;
		float dt = 0;
		CCSprite monkey;
		List<CCSprite> visibleBananas;
		List<CCSprite> hitBananas;

		// define our banana rotation action
		CCRotateBy rotateBanana = new CCRotateBy (0.8f, 360);

		// define our completion action to remove the banana once it hits the
		// bottom of the screen
		CCCallFuncN moveBananaComplete = new CCCallFuncN ((node) => {
			node.RemoveFromParentAndCleanup (true);
		});


		public GameLayer ()
        {
            var touchListener = new CCEventListenerTouchAllAtOnce();
            touchListener.OnTouchesEnded = OnTouchesEnded;

            EventDispatcher.AddEventListener(touchListener, this);
            Color = new CCColor3B (CCColor4B.White);
			Opacity = 255;

			visibleBananas = new List<CCSprite> ();
			hitBananas = new List<CCSprite> ();
	
			monkey = new CCSprite ("Monkey");
			monkey.Position = CCDirector.SharedDirector.WinSize.Center;
			AddChild (monkey);

			Schedule ((t) => {
				visibleBananas.Add (AddBanana ());
				dt += t;
				if(ShouldEndGame ()){
					var gameOverScene = GameOverLayer.SceneWithScore(hitBananas.Count);
					CCTransitionFadeDown transitionToGameOver = new CCTransitionFadeDown(1.0f, gameOverScene);
					CCDirector.SharedDirector.ReplaceScene (transitionToGameOver);
				}
			}, 1.0f);

			Schedule ((t) => {
				CheckCollision ();
			});
		}

		CCSprite AddBanana ()
		{
			var banana = new CCSprite ("Banana");

			double rnd = new Random ().NextDouble ();
			double randomX = (rnd > 0) 
				? rnd * CCDirector.SharedDirector.WinSize.Width - banana.ContentSize.Width / 2 
				: banana.ContentSize.Width / 2;
	
			banana.Position = new CCPoint ((float)randomX, CCDirector.SharedDirector.WinSize.Height - banana.ContentSize.Height / 2);

			AddChild (banana);

			var moveBanana = new CCMoveTo (5.0f, new CCPoint (banana.Position.X, 0));

			banana.RunActions (moveBanana, moveBananaComplete);

			banana.RepeatForever (rotateBanana);

			return banana;
		}

		void CheckCollision ()
		{
			visibleBananas.ForEach ((banana) => {
				bool hit = banana.BoundingBox.IntersectsRect (monkey.BoundingBox);
				if (hit) {
					hitBananas.Add (banana);
					banana.RemoveFromParent ();
				}
			});

			hitBananas.ForEach ((banana) => {
				visibleBananas.Remove (banana); });
		}

		bool ShouldEndGame()
		{
			return dt > 20.0;
		}

        void OnTouchesEnded (List<CCTouch> touches, CCEvent touchEvent)
        {
            base.TouchesEnded (touches, touchEvent);

            var location = touches [0].Location;

            float ds = CCPoint.Distance (monkey.Position, location);

            float dt = ds / MONKEY_SPEED;

            var moveMonkey = new CCMoveTo (dt, location);
            monkey.RunAction (moveMonkey);  

            CCSimpleAudioEngine.SharedEngine.PlayEffect ("Sounds/tap.mp3");
        }

		public static CCScene Scene {
			get {
				var scene = new CCScene ();
				var layer = new GameLayer ();
			
				scene.AddChild (layer);

				return scene;
			}
		}
	}
}

