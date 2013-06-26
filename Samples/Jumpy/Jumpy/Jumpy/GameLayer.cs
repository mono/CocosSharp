// GameLayer.cs
//
// Author(s)
//	Stephane Delcroix <stephane@delcroix.org>
//
// Copyright (C) 2012 s. Delcroix
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software 
// and associated documentation files (the "Software"), to deal in the Software without restriction, 
// including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, 
// and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, 
// subject to the following conditions:
//		
// 		The above copyright notice and this permission notice shall be included in all copies or 
//		substantial portions of the Software.
//		
//		THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING 
//		BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND 
//		NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, 
//		DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, 
//		OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System;
using Cocos2D;
using Microsoft.Xna.Framework;

namespace Jumpy
{
	public class GameLayer : MainLayer
	{
		public enum Bonus : int {
			Bonus5 = 0,
			Bonus10,
			Bonus50,
			Bonus100,
			NumBonuses
		}

		int score;
		int currentPlatformTag;
		bool gameSuspended;
		int numPlatforms = 10;
		int minPlatformStep = 50;
		int maxPlatformStep = 300;
		int minBonusStep = 20;
		int maxBonusStep = 50;
		int platformTopPadding = 10;
		bool birdLookingRight;

		CCPoint bird_pos;
		Vector2 bird_vel;
		Vector2 bird_acc;

		public static CCScene Scene
		{
			get {
				var scene = new CCScene (); 
				var layer = new GameLayer ();
				scene.AddChild(layer);
				return scene;
			}
		}

		public GameLayer ()
		{
			gameSuspended = true;
			var batchnode = GetChildByTag((int)Tags.SpriteManager) as CCSpriteBatchNode;

			InitPlatforms ();

			var bird = new CCSprite(batchnode.Texture, new CCRect(608,16,44,32));
			batchnode.AddChild (bird,4,(int)Tags.Bird);

			CCSprite bonus;
			
			for(int i=0; i<(int)Bonus.NumBonuses; i++) {
				bonus = new CCSprite(batchnode.Texture, new CCRect(608+i*32,256,25,25));
				batchnode.AddChild(bonus,4,(int)Tags.BomusStart+i);
				bonus.Visible = false;
			}

			var scoreLabel = new CCLabelBMFont("0", "Fonts/bitmapFont.fnt");
            scoreLabel.Position = new CCPoint(160,430);
			AddChild(scoreLabel, 5, (int)Tags.ScoreLabel);


		}

		public override void OnEnter ()
		{
			base.OnEnter ();
			Schedule(Step);

#if WINDOWS || MONOMAC
            AccelerometerEnabled = false;
            TouchEnabled = true;
            TouchMode = CCTouchMode.OneByOne;
#else	
            AccelerometerEnabled = true;
            SingleTouchEnabled = false;
#endif
            //IsAccelerometerEnabled = true;

			// CCDirector.SharedDirector.Accelerometer.SetDelegate(new AccelerometerDelegate(DidAccelerate);
			StartGame ();
		}



		void InitPlatforms ()
		{
			currentPlatformTag = (int)Tags.PlatformsStart;
			while(currentPlatformTag < (int)Tags.PlatformsStart + numPlatforms) {
				InitPlatform();
				currentPlatformTag++;
			}
			
			ResetPlatforms ();
		}

        private Random ran = new Random();

		void InitPlatform ()
		{
			CCRect rect;
			switch(ran.Next()%2) {
			case 0: 
				rect = new CCRect(608,64,102,36); 
				break;
			default:
				rect = new CCRect(608,128,90,32); 
				break;
			}

			var batchnode = GetChildByTag((int)Tags.SpriteManager) as CCSpriteBatchNode;
			var platform = new CCSprite(batchnode.Texture, rect);
			batchnode.AddChild(platform,3,currentPlatformTag);
		}

		float currentPlatformY;
		float currentMaxPlatformStep;
		int currentBonusPlatformIndex;
		int currentBonusType;
		int platformCount;

		void ResetPlatforms ()
		{
			currentPlatformY = -1;
			currentPlatformTag = (int)Tags.PlatformsStart;
			currentMaxPlatformStep = 60.0f;
			currentBonusPlatformIndex = 0;
			currentBonusType = 0;
			platformCount = 0;
			
			while(currentPlatformTag < (int)Tags.PlatformsStart + numPlatforms) {
				ResetPlatform();
				currentPlatformTag++;
			}
		}

		void ResetPlatform ()
		{

			if(currentPlatformY < 0) {
				currentPlatformY = 30.0f;
			} else {
				currentPlatformY += ran.Next() % (int)(currentMaxPlatformStep - minPlatformStep) + minPlatformStep;
				if(currentMaxPlatformStep < maxPlatformStep) {
					currentMaxPlatformStep += 0.5f;
				}
			}

			var batchnode = GetChildByTag ((int)Tags.SpriteManager) as CCSpriteBatchNode;
			var platform = batchnode.GetChildByTag(currentPlatformTag) as CCSprite;

            if (ran.Next() % 2 == 1) platform.ScaleX = -1.0f;
			
			float x;
			var size = platform.ContentSize;
			if(currentPlatformY == 30.0f) {
				x = 160.0f;
			} else {
                x = ran.Next() % (320 - (int)size.Width) + size.Width / 2;
			}
			
			platform.Position = new CCPoint(x,currentPlatformY);
			platformCount++;
			
			if(platformCount == currentBonusPlatformIndex) {

				var bonus = batchnode.GetChildByTag((int)Tags.BomusStart+currentBonusType) as CCSprite;
				bonus.Position = new CCPoint(x,currentPlatformY+30);
				bonus.Visible = true;
			}

		}

		void StartGame ()
		{
			score = 0;

			ResetClouds ();
			ResetPlatforms ();
			ResetBird ();
			ResetBonus();

			// UIApplication.SharedApplication.IdleTimerDisabled=true;
			gameSuspended = false;;
		}

		void ResetBird() {
			var batchnode = GetChildByTag((int)Tags.SpriteManager) as CCSpriteBatchNode;
			var bird = batchnode.GetChildByTag((int)Tags.Bird) as CCSprite;

			bird_pos.X = 160;
			bird_pos.Y = 160;
			bird.Position = bird_pos;
			
			bird_vel.X = 0;
			bird_vel.Y = 0;
			
			bird_acc.X = 0;
			bird_acc.Y = -550.0f;
			
			birdLookingRight = true;
			bird.ScaleX = 1.0f;
		}

		void ResetBonus () {
			var batchnode = GetChildByTag((int)Tags.SpriteManager) as CCSpriteBatchNode;
			var bonus = batchnode.GetChildByTag((int)Tags.BomusStart+currentBonusType) as CCSprite;
			bonus.Visible = false;
            currentBonusPlatformIndex += ran.Next() % (maxBonusStep - minBonusStep) + minBonusStep;
			if(score < 10000) {
				currentBonusType = 0;
			} else if(score < 50000) {
                currentBonusType = ran.Next() % 2;
			} else if(score < 100000) {
                currentBonusType = ran.Next() % 3;
			} else {
                currentBonusType = ran.Next() % 2 + 2;
			}
		}

		protected override void Step (float dt) {
			base.Step (dt);

			if (gameSuspended)
				return;

			var batchnode = GetChildByTag((int)Tags.SpriteManager) as CCSpriteBatchNode;
			var bird = batchnode.GetChildByTag((int)Tags.Bird) as CCSprite;
			var particles = GetChildByTag((int)Tags.Particles) as CCParticleSystem;

			bird_pos.X += bird_vel.X * dt;

			if(bird_vel.X < -30.0f && birdLookingRight) {
				birdLookingRight = false;
				bird.ScaleX = -1.0f;
			} else if (bird_vel.X > 30.0f && !birdLookingRight) {
				birdLookingRight = true;
				bird.ScaleX = 1.0f;
			}

			var bird_size = bird.ContentSize;
			float max_x = 320-bird_size.Width/2;
			float min_x = 0+bird_size.Width/2;
			
			if(bird_pos.X>max_x) bird_pos.X = max_x;
			if(bird_pos.X<min_x) bird_pos.X = min_x;
			
			bird_vel.Y += bird_acc.Y * dt;
			bird_pos.Y += bird_vel.Y * dt;

			var bonus = batchnode.GetChildByTag((int)Tags.BomusStart + currentBonusType);
			if(bonus.Visible) {
				var bonus_pos = bonus.Position;
				float range = 20.0f;
				if(bird_pos.X > bonus_pos.X - range &&
				   bird_pos.X < bonus_pos.Y + range &&
				   bird_pos.Y > bonus_pos.Y - range &&
				   bird_pos.Y < bonus_pos.Y + range ) {
					switch(currentBonusType) {
					case (int)Bonus.Bonus5:   score += 5000;   break;
					case (int)Bonus.Bonus10:  score += 10000;  break;
					case (int)Bonus.Bonus50:  score += 50000;  break;
					case (int)Bonus.Bonus100: score += 100000; break;
					}
					var scorelabel = GetChildByTag((int)Tags.ScoreLabel) as CCLabelBMFont;
					scorelabel.Text = score.ToString();

					var a1 = new CCScaleTo(.2f,1.5f,.08f);
					var a2 = new CCScaleTo(.2f,1f,1f);
					var a3 = new CCSequence(a1,a2,a1,a2,a1,a2);
					scorelabel.RunAction(a3);
					ResetBonus ();
				}
			}
			
			int t;
			
			if(bird_vel.Y < 0) {
				
				t = (int)Tags.PlatformsStart;
				for(t = (int)Tags.PlatformsStart; t < (int)Tags.PlatformsStart + numPlatforms; t++) {
					var platform = batchnode.GetChildByTag(t) as CCSprite;
					
					var platform_size = platform.ContentSize;
					var platform_pos = platform.Position;
					
					max_x = platform_pos.X - platform_size.Width/2 - 10;
					min_x = platform_pos.X + platform_size.Width/2 + 10;
					float min_y = platform_pos.Y + (platform_size.Height+bird_size.Height)/2 - platformTopPadding;
					
					if(bird_pos.X > max_x &&
					   bird_pos.X < min_x &&
					   bird_pos.Y > platform_pos.Y &&
					   bird_pos.Y < min_y) {
						Jump();
					}
				}
				
				if(bird_pos.Y < -bird_size.Height/2) {
					ShowHighScores();
				}
				
			} else if(bird_pos.Y > 240) {
				
				float delta = bird_pos.Y - 240;
				bird_pos.Y = 240;
				
				currentPlatformY -= delta;
				

				for(t = (int)Tags.CloudsStart; t < (int)Tags.CloudsStart + numClouds; t++) {
					var cloud = batchnode.GetChildByTag(t) as CCSprite;
					var pos = cloud.Position;
					pos.Y -= delta * cloud.ScaleY * 0.8f;
					if(pos.Y < -cloud.ContentSize.Height/2) {
						currentCloudTag = t;
						ResetCloud();

					} else {
						cloud.Position = pos;
					}
				}
				

				for(t = (int)Tags.PlatformsStart; t < (int)Tags.PlatformsStart + numPlatforms; t++) {
					var platform = batchnode.GetChildByTag(t) as CCSprite;
					var pos = platform.Position;
					pos = new CCPoint(pos.X,pos.Y-delta);
					if(pos.Y < -platform.ContentSize.Height/2) {
						currentPlatformTag = t;
						ResetPlatform ();
					} else {
						platform.Position = pos;
					}
				}
				
				if(bonus.Visible) {
					var pos = bonus.Position;
					pos.Y -= delta;
					if(pos.Y < -bonus.ContentSize.Height/2) {
						ResetBonus ();
						//[self resetBonus];
					} else {
						bonus.Position = pos;
					}
				}
				
				score += (int)delta;

				var scoreLabel = GetChildByTag((int)Tags.ScoreLabel) as CCLabelBMFont;
				scoreLabel.SetString(score.ToString());
			}
			
			bird.Position = bird_pos;

			if (particles !=null) {
				var particle_pos = new CCPoint(bird_pos.X,bird_pos.Y-17);
				particles.Position = particle_pos;
			}
		}

		void Jump ()
		{
			bird_vel.Y = 400.0f + Math.Abs(bird_vel.X);

			var old_part = GetChildByTag((int)Tags.Particles);
			if (old_part!=null)
				RemoveChild(old_part,true);

			var particle = new CCParticleFireworks();
			particle.Position = bird_pos;
			particle.Gravity = new CCPoint(0,-5000);
			particle.Duration = .3f;
			AddChild(particle,-1,(int)Tags.Particles);
		}

        public override bool TouchBegan(CCTouch touch)
        {
            return (true);
        }
        public override void TouchEnded(CCTouch touch)
        {
            float accel_filter = 0.1f;
            float ax = ConvertTouchToNodeSpace(touch).X - ConvertToNodeSpace(touch.PreviousLocationInView).X;
            ax /= 500;
            bird_vel.X = bird_vel.X * accel_filter + (float)ax * (1.0f - accel_filter) * 500.0f;
        }

		void HandleAccelerate (CCAcceleration acceleration)
		{
			if(gameSuspended) 
				return;
			float accel_filter = 0.1f;
			bird_vel.X = bird_vel.X * accel_filter + (float)acceleration.X * (1.0f - accel_filter) * 500.0f;
		}

		void ShowHighScores ()
		{
			gameSuspended = true;
			CCDirector.SharedDirector.ReplaceScene(new CCTransitionFade(1, HighScoreLayer.Scene(score),new CCColor3B(255,255,255)));
		}
	}

	public class AccelerometerDelegate : ICCAccelerometerDelegate {

        public virtual void DidAccelerate(CCAcceleration pAccelerationValue)
        {
        }
	}
}