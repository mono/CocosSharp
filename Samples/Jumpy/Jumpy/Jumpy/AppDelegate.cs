// AppDelegate.cs
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
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using CocosSharp;

namespace Jumpy
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    public partial class AppDelegate : CCApplicationDelegate
    {
        int preferredWidth;
        int preferredHeight;

        /// <summary>
        ///  Implement CCDirector and CCScene init code here.
        /// </summary>
        /// <returns>
        ///  true  Initialize success, app continue.
        ///  false Initialize failed, app terminate.
        /// </returns>
        public override void ApplicationDidFinishLaunching(CCApplication application)
        {


            //1280 x 768
#if WINDOWS_PHONE
            preferredWidth = 1280;
            preferredHeight = 768;
#else
            preferredWidth = 1024;
            preferredHeight = 768;
#endif

            application.PreferredBackBufferWidth = preferredWidth;
            application.PreferredBackBufferHeight = preferredHeight;

            application.PreferMultiSampling = true;
            application.ContentRootDirectory = "Content";

            //CCSpriteFontCache.FontScale = 0.5f;
            //CCSpriteFontCache.RegisterFont("MarkerFelt", 22);
            //CCSpriteFontCache.RegisterFont("arial", 12, 24);

            CCDirector director = CCApplication.SharedApplication.MainWindowDirector;
            director.DisplayStats = true;
            director.AnimationInterval = 1.0 / 60;

            CCSize designSize = new CCSize(480, 320);

            if (CCDrawManager.FrameSize.Height > preferredHeight)
            {
                //CCSize resourceSize = new CCSize(960, 640);
                CCSize resourceSize = new CCSize(preferredWidth, preferredHeight);
                application.ContentSearchPaths.Add("hd");
                director.ContentScaleFactor = resourceSize.Height / designSize.Height;
            }

            CCDrawManager.SetDesignResolutionSize(designSize.Width, designSize.Height, CCResolutionPolicy.ShowAll);

            // turn on display FPS
            director.DisplayStats = true;

            // set FPS. the default value is 1.0/60 if you don't call this
            director.AnimationInterval = 1.0 / 60;

            CCScene pScene = GameLayer.Scene;

            director.RunWithScene(pScene);
        }

        /// <summary>
        /// The function be called when the application enters the background
        /// </summary>
        //		public override void ApplicationDidEnterBackground()
        //		{
        //			// stop all of the animation actions that are running.
        //			CCDirector.SharedDirector.Pause();
        //
        //			// if you use SimpleAudioEngine, your music must be paused
        //			//CCSimpleAudioEngine.SharedEngine.PauseBackgroundMusic = true;
        //		}

        /// <summary>
        /// The function be called when the application enter foreground  
        /// </summary>
        //		public override void ApplicationWillEnterForeground()
        //		{
        //			CCDirector.SharedDirector.Resume();
        //
        //			// if you use SimpleAudioEngine, your background music track must resume here. 
        //			//CCSimpleAudioEngine.SharedEngine.PauseBackgroundMusic = false;
        //
        //		}
    }
}

