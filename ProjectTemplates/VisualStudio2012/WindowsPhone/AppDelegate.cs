#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Input;
using Cocos2D;
using CocosDenshion;
#endregion

namespace $safeprojectname$
{
	/// <summary>
	/// This is your extension of the main Cocos2D application object.
    /// </summary>
    internal class AppDelegate : CCApplication
    {
        public AppDelegate(Game game, GraphicsDeviceManager graphics)
            : base(game, graphics)
        {
            s_pSharedApplication = this;
			//
			// TODO: Set the display orientation that you want for this game.
			// 
            CCDrawManager.InitializeDisplay(game, graphics, DisplayOrientation.Portrait);
        }

        /// <summary>
        ///  Implement CCDirector and CCScene init code here.
        /// </summary>
        /// <returns>
        ///  true  Initialize success, app should continue.
        ///  false Initialize failed, app should terminate.
        /// </returns>
        public override bool ApplicationDidFinishLaunching()
        {
            CCSimpleAudioEngine.SharedEngine.SaveMediaState();

            CCDirector pDirector = null;
            try
            {
				// Set your design resolution here, which is the target resolution of your primary
				// design hardware.
				//
                CCDrawManager.SetDesignResolutionSize(480f, 800f, ResolutionPolicy.ShowAll);
                CCApplication.SharedApplication.GraphicsDevice.Clear(Color.Black);
                //initialize director
                pDirector = CCDirector.SharedDirector;
                pDirector.SetOpenGlView();

                //turn on display FPS
                pDirector.DisplayStats = false;

                // set FPS. the default value is 1.0/60 if you don't call this
#if WINDOWS_PHONE
                pDirector.AnimationInterval = 1f / 30f;
#else
                pDirector.AnimationInterval = 1.0 / 60;
#endif
            }
            catch (Exception ex)
            {
                CCLog.Log("ApplicationDidFinishLaunching(): Error " + ex.ToString());
            }
            return true;
		}

        /// <summary>
        /// The function be called when the application enter background
        /// </summary>
        public override void ApplicationDidEnterBackground()
        {
            base.ApplicationDidEnterBackground();
			//
			// TODO: Save the game state and pause your music
			//
            CCDirector.SharedDirector.Pause();
		}

        /// <summary>
        /// The function be called when the application enter foreground  
        /// </summary>
        public override void ApplicationWillEnterForeground()
        {
            base.ApplicationWillEnterForeground();
			//
			// reset the playback of audio
			//
            CCDirector.SharedDirector.ResumeFromBackground();
		}
	}
}
