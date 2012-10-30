This is a divergent repository from the cocos2d/cocos2d-x-for-xna repository that was originally created for XNA.

There are many differences between cocos2d-xna and cocos2d-x-for-xna. This repository
reflects the cocos2d-xna source base which was written for .NET and C#. Attributes and
other language/platform constructs specific to .NET have been used in lieu of the literal
translation in the prior cocos2d-x-for-xna repository.

==================================================================================================

Most importantly, your AppDelegate will change:

        public AppDelegate(Game game, GraphicsDeviceManager graphics)
            : base(game, graphics)
        {
            s_pSharedApplication = this;
            DrawManager.InitializeDisplay(game, graphics, DisplayOrientation.LandscapeRight | DisplayOrientation.LandscapeLeft);


            graphics.PreferMultiSampling = false;

        }



        public override bool ApplicationDidFinishLaunching()
        {
            //initialize director
            CCDirector pDirector = CCDirector.SharedDirector;
            pDirector.SetOpenGlView();

            DrawManager.SetDesignResolutionSize(480, 320, ResolutionPolicy.ShowAll);

            // turn on display FPS
            pDirector.DisplayStats = true;

            // set FPS. the default value is 1.0/60 if you don't call this
            pDirector.AnimationInterval = 1.0 / 60;

            // create a scene. it's an autorelease object
            CCScene pScene = CCScene.Create();
            CCLayer pLayer = new TestController();
            
            pScene.AddChild(pLayer);
            pDirector.RunWithScene(pScene);

            return true;
        }

==================================================================================================

Note the two new calls:

            DrawManager.InitializeDisplay(game, graphics, DisplayOrientation.LandscapeRight | DisplayOrientation.LandscapeLeft);

This will setup your display orientation and preferred back buffer.

            DrawManager.SetDesignResolutionSize(480, 320, ResolutionPolicy.ShowAll);

This will set your game to scale itself into the window appropriately. The cocos2d-xna code
is written from a professional game design perspective. You design your game UI for a target resolution
and then use that resoluion in your SetDesignResolutionSize() call. In this way, your game fidelity
does not change, and your display does not appear truncated on devices that are larger or smaller 
than your design resolution.

==================================================================================================
"external lib"
==================================================================================================

To support Android, iOS, and other platforms, you must have a version of MonoGame (develop3d) 
version 3.0 available. We include a binary of a recent pull from that repository, but you should 
get your own version and test with it. We only provide a binary of MonoGame for the test cases to
run on Android.

