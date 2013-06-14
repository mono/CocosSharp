using System;

using Cocos2D;

/*
 
 The GameData class mostly maintains data through all levels of the game. 

 */


namespace GameStarterKit
{
	public class GameData
	{

		static GameData sharedData;

		bool soundFXMuted;
		bool voiceFXMuted;
		bool ambientFXMuted;
		bool firstRunEver;
		
		CCUserDefault defaults;
		
		private GameData ()
		{
			defaults = CCUserDefault.SharedUserDefault;

			firstRunEver = defaults.GetBoolForKey("firstRunEverKey", true);

			soundFXMuted = defaults.GetBoolForKey("soundFXMutedKey");   //will default to NO if there's no previous default value
			voiceFXMuted = defaults.GetBoolForKey("voiceFXMutedKey");   //will default to NO if there's no previous default value
			ambientFXMuted = defaults.GetBoolForKey("ambientFXMutedKey");   //will default to NO if there's no previous default value


		}

		/// <summary>
		/// returns a shared instance of the GameData
		/// </summary>
		/// <value> </value>
		public static GameData SharedData
		{
			get
			{
				if (sharedData == null)
				{
					sharedData = new GameData();
				}
				return sharedData;
			}
		}

		public bool FirstRunEver
		{
			get { return firstRunEver; }
			set 
			{ 
				firstRunEver = value; 
				defaults.SetBoolForKey("firstRunEverKey", firstRunEver );
				defaults.Flush();
			}
		}


#region sounds
		
		public bool AreSoundFXMuted
		{
			
			get {
				return soundFXMuted;
			}

			set 
			{
				soundFXMuted = value;
				defaults.SetBoolForKey("soundFXMutedKey", soundFXMuted );
				defaults.Flush();
			}

		}

		/////////////////////////
		
		public bool AreVoiceFXMuted 
		{

			get {	
				return voiceFXMuted;
			}

			set 
			{
				voiceFXMuted = value;
				defaults.SetBoolForKey("voiceFXMutedKey",voiceFXMuted );
				defaults.Flush();
			}
			
			
			
		}

		/////////////////////////
		
		public bool AreAmbientFXMuted
		{

			get {	
				return ambientFXMuted;
			}

			set 
			{
				ambientFXMuted = value;
				defaults.SetBoolForKey("ambientFXMutedKey",ambientFXMuted );
				defaults.Flush();
			}
		}

#endregion

	}
}

