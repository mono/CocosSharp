# Cocos2D-XNA

is a 2D/3D game developer’s framework that is written specifically for XNA. XNA is the game API developed by Microsoft for independently published games on Xbox Live Arcade. To support other platforms than the now deprecated Microsoft XNA targets, Cocos2D-XNA supports the MonoGame runtime.

This project is open source, freely available, and free of royalties or encumberance. The software is released under the highly permissive MIT License.


Download and Run
----------------

### Code on GitHub

To obtain the code you will need a git client.  Either command line or Graphical.

Using the git command line you will need to clone the git repository.

Pick a repository to clone:  

https://github.com/mono/cocos2d-xna.git 
or the parent repo https://github.com/totallyevil/cocos2d-xna.git.  

The two are identical so either one will do.

> $ git clone https://github.com/mono/cocos2d-xna.git
  
or

> $ git clone https://github.com/totallyevil/cocos2d-xna.git

Wait until the clone has finished.

You should see something similar to the following:

	Cloning into 'cocos2d-xna'...
	remote: Counting objects: 20553, done.
	remote: Compressing objects: 100% (7677/7677), done.
	remote: Total 20553 (delta 14127), reused 18870 (delta 12446)
	Receiving objects: 100% (20553/20553), 100.83 MiB | 634 KiB/s, done.
	Resolving deltas: 100% (14127/14127), done.
	Checking out files: 100% (4130/4130), done.

To support Android, iOS, and other platforms, you must have a version of MonoGame (develop branch) version 3.0 available. The MonoGame repository is a submodule of the Cocos2D-XNA project that all the solutions reference.

To initialise and update the required MonoGame submodules that we reference you will need to do the following:

Change into the cocos2d-xna directory to issue the following submodule commands.

> $ git submodule init

Output from above command:

	Submodule 'MonoGame' (git://github.com/mono/MonoGame.git) registered for path 'MonoGame'
	Submodule 'tools/ouya' (https://github.com/slygamer/ouya-csharp.git) registered for path 'tools/ouya'

You will then want to update the actual submodules:

> $ git submodule update

Output from above command:

	Cloning into 'MonoGame'...
	remote: Counting objects: 32905, done.
	remote: Compressing objects: 100% (10011/10011), done.
	remote: Total 32905 (delta 24991), reused 29779 (delta 22574)
	Receiving objects: 100% (32905/32905), 33.33 MiB | 305 KiB/s, done.
	Resolving deltas: 100% (24991/24991), done.
	Submodule path 'MonoGame': checked out 'bd6518a33c91c43a46f14aa68bdc854c08e6bc2a'
	Cloning into 'tools/ouya'...
	remote: Counting objects: 249, done.
	remote: Compressing objects: 100% (139/139), done.
	remote: Total 249 (delta 100), reused 231 (delta 86)
	Receiving objects: 100% (249/249), 2.88 MiB | 620 KiB/s, done.
	Resolving deltas: 100% (100/100), done.
	Submodule path 'tools/ouya': checked out '5f712a4b3845bad2974b30bc0c243eb503812ea9'

MonoGame has it's own external dependencies so we will also need to obtain those as well.

> $ cd MonoGame

> $ git submodule init

> $ git submodule update ThirdParty/Libs

Notice above that we only need the ThirdParty/Libs to actually build Cocos2D-XNA.

You now have everything you need to start start developing with Cocos2D-XNA

Templates for Visual Studio
---------------------------

To make things as easy as possible we also provide templates for Visual Studio.

There is tutorial on [Finding and Installing the templates](http://cocoa-mono.org/archives/494/cocos2d-xna-getting-started-part-1/ "Cocos2D-XNA Getting Started")

Templates for Xamarin Studio
----------------------------

We are working hard on getting these out.


Getting Started
---------------

### Test Bed

We have created solutions for all the supported platforms that serves as our TestBed for each platform.

You can find those in the [tests directory](https://github.com/mono/cocos2d-xna/tree/master/tests "Test Bed")

cocos2d-xna.Tests.Android.sln	
cocos2d-xna.Tests.Ouya.sln	
cocos2d-xna.Tests.Windows.sln	
cocos2d-xna.Tests.Windows8.sln	
cocos2d-xna.Tests.WindowsDX.sln	
cocos2d-xna.Tests.WindowsGL.sln	
cocos2d-xna.Tests.WindowsPhone.sln	
cocos2d-xna.Tests.WindowsPhone7.sln	
cocos2d-xna.Tests.iOS.sln	

### Samples

[Xamarin’s Angry Ninjas](https://github.com/xamarin/AngryNinjas "Xamarin’s Angry Ninjas") sample application. We worked hard on converting that cocos2d-iphone game over to Cocos2D-XNA. Take a look at how it deploys raw assets and utilizes box2d. 

Additional samples will be forthcoming as we find time to add more for you. 




