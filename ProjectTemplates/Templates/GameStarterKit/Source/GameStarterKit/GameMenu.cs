using System;

using Microsoft.Xna.Framework;
using Cocos2D;

namespace GameStarterKit
{
	public class GameMenu : CCLayer
	{
		CCMenu VoiceFXMenu;
		CCMenu SoundFXMenu;
		CCMenu AmbientFXMenu;
		
		CCPoint VoiceFXMenuLocation;
		CCPoint SoundFXMenuLocation;
		CCPoint AmbientFXMenuLocation;
		
		string voiceButtonName;
		string voiceButtonNameDim;
		
		string soundButtonName;
		string soundButtonNameDim;
		
		string ambientButtonName;
		string ambientButtonNameDim;

		public GameMenu ()
		{
			var screenSize = CCDirector.SharedDirector.WinSize;
			
			voiceButtonName = "VoiceFX";
			voiceButtonNameDim = "VoiceFX";
			
			soundButtonName = "SoundFX";
			soundButtonNameDim = "SoundFX";
			
			ambientButtonName = "AmbientFX";
			ambientButtonNameDim = "AmbientFX";
			

			SoundFXMenuLocation = new CCPoint( 110, 55 );
			VoiceFXMenuLocation = new CCPoint( 230, 55 );
			AmbientFXMenuLocation = new CCPoint(355, 55 );
			

			TouchEnabled = true;
			

			IsSoundFXMenuItemActive = !GameData.SharedData.AreSoundFXMuted;

			IsVoiceFXMenuActive = !GameData.SharedData.AreVoiceFXMuted;

			IsAmbientFXMenuActive = !GameData.SharedData.AreAmbientFXMuted;

		}			

		void PlayNegativeSound (object sender)
		 {
			
			//play a sound indicating this level isn't available

		 }
									 
		public static CCScene Scene
		{
			get {
				// 'scene' is an autorelease object.
				CCScene scene = new CCScene();
				
				// 'layer' is an autorelease object.
				GameMenu layer = new GameMenu();
				
				// add layer as a child to scene
				scene.AddChild(layer);
				
				// return the scene
				return scene;
			}
		}


		#region SECTION BUTTONS
		

		#region POP (remove) SCENE and Transition to new level

		
		void PopAndTransition()
		{
			
			CCDirector.SharedDirector.PopScene();
			
			//when TheLevel scene reloads it will start with a new level
			GameLevel.SharedLevel.TransitionAfterMenuPop();
			

		}
		
		#endregion

		#region  POP (remove) SCENE and continue playing current level
		public override void TouchesBegan (System.Collections.Generic.List<CCTouch> touches)
		{
			CCDirector.SharedDirector.PopScene();
		}

		#endregion

		#region VOICE FX

		bool IsVoiceFXMenuActive
		{
			set
			{
				RemoveChild(VoiceFXMenu, true);
				CCMenuItem button1;
				CCLabelTTF label;

				if (!value)
				{
					label = new CCLabelTTF(voiceButtonNameDim, "MarkerFelt", 18);
					label.Color = new CCColor3B (Color.Gray);
					button1 = new CCMenuItemLabel(label, TurnVoiceFXOn);
				}
				else
				{
					label = new CCLabelTTF(voiceButtonName, "MarkerFelt", 18);
					label.Color = new CCColor3B (Color.Yellow);
					button1 = new CCMenuItemLabel(label, TurnVoiceFXOff);
				}

				VoiceFXMenu = new CCMenu(button1);
				VoiceFXMenu.Position= VoiceFXMenuLocation;
				
				AddChild(VoiceFXMenu, 10);
				
			}

		}


		void TurnVoiceFXOn(object sender)
		{
			
			GameData.SharedData.AreVoiceFXMuted = false;

			IsVoiceFXMenuActive = true;

		}

		void TurnVoiceFXOff(object sender)
		{
			
			GameData.SharedData.AreVoiceFXMuted = true;

			IsVoiceFXMenuActive = false;

		}

		#endregion

		#region Sound FX

		bool IsSoundFXMenuItemActive
		{
			set 
			{
				RemoveChild(SoundFXMenu, true);

				CCMenuItemLabel button1;

				CCLabelTTF label;

				if (!value)
				{
					label = new CCLabelTTF(soundButtonNameDim, "MarkerFelt", 18);
					label.Color = new CCColor3B (Color.Gray);
					button1 = new CCMenuItemLabel(label, TurnSoundFXOn);
				}
				else
				{
					label = new CCLabelTTF(soundButtonName, "MarkerFelt", 18);
					label.Color = new CCColor3B (Color.Yellow);
					button1 = new CCMenuItemLabel(label, TurnSoundFXOff);
				}

				SoundFXMenu = new CCMenu(button1);
				SoundFXMenu.Position= SoundFXMenuLocation;
				
				AddChild(SoundFXMenu, 10);

			}
		}

		void TurnSoundFXOn(object sender)
		{
			
			GameData.SharedData.AreSoundFXMuted = false;

			IsSoundFXMenuItemActive = true;

		}

		void TurnSoundFXOff (object sender)
		{
			
			GameData.SharedData.AreSoundFXMuted = true;

			IsSoundFXMenuItemActive = false;
		}

		#endregion

		#region Ambient FX

		bool IsAmbientFXMenuActive
		{
			set
			{
				RemoveChild(AmbientFXMenu, true);

				CCMenuItemLabel button1;

				CCLabelTTF label;

				if (!value)
				{
					label = new CCLabelTTF(ambientButtonName, "MarkerFelt", 18);
					label.Color = new CCColor3B (Color.Gray);
					button1 = new CCMenuItemLabel(label, TurnAmbientFXOn);
				}
				else
				{
					label = new CCLabelTTF(ambientButtonNameDim, "MarkerFelt", 18);
					label.Color = new CCColor3B (Color.Yellow);
					button1 = new CCMenuItemLabel(label, TurnAmbientFXOff);
				}

				AmbientFXMenu = new CCMenu(button1);
				AmbientFXMenu.Position= AmbientFXMenuLocation;

				AddChild(AmbientFXMenu, 10  );

			}

		}

		void TurnAmbientFXOn (object sender) {
			
			GameData.SharedData.AreAmbientFXMuted = true;

			IsAmbientFXMenuActive = true;
			
			
		}
		void TurnAmbientFXOff (object sender) {
			
			GameData.SharedData.AreAmbientFXMuted = false;

			IsAmbientFXMenuActive = false;
		}

		#endregion

		#endregion

	}
}

