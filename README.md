This is a divergent repository from the cocos2d/cocos2d-x-for-xna repository that was originally created for XNA but was later abandoned.

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

This will set your game to scale itself into the window appropriately. The cocos2d-xna code is written from a professional game design perspective. You design your game UI for a target resolution and then use that resoluion in your SetDesignResolutionSize() call. In this way, your game fidelity does not change, and your display does not appear truncated on devices that are larger or smaller than your design resolution.

Download and Run
----------------
	https://raw.github.com/altercation/solarized/master/README.md	
### Code on GitHub

To obtain the code you will need a git client.  Either command line or Graphical.

Using the git command line you will need to clone the git repository.

Pick a repository to clone:  

https://github.com/mono/cocos2d-xna.git 
or the parent repo https://github.com/totallyevil/cocos2d-xna.git.  

The two are identical so either one will do.

> git clone https://github.com/mono/cocos2d-xna.git
  or
> git clone https://github.com/totallyevil/cocos2d-xna.git

Wait until the clone has finished.


To support Android, iOS, and other platforms, you must have a version of MonoGame (develop) version 3.0 available. The MonoGame repository is a submodule of this project now, so make sure that you update and init the submodule if you do not have your own clone of MonoGame. Don't forget to use develop if you want the latest changes, or master if you want the most recent stable release.



