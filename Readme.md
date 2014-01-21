# CocosSharp

CocosSharp is an easy to use library for simple games using C# and F#.
It is a .NET port of the popular Cocos2D engine, derived from the
Cocos2D-X engine via Cocos2D-XNA.

This library is MIT licensed.

License
-------

This project is open source, freely available, and free of royalties
or encumberance. The software is released under the highly permissive
MIT License.


Download and Run
----------------

### Code on GitHub

To obtain the code you will need a git client.  Either command line or graphical.

Using the git command line you will need to clone the git repository.

> $ git clone https://github.com/xamarin/CocosSharp.git

Wait until the clone has finished.

You should see something similar to the following:

	Cloning into 'CocosSharp'...
	remote: Counting objects: 20553, done.
	remote: Compressing objects: 100% (7677/7677), done.
	remote: Total 20553 (delta 14127), reused 18870 (delta 12446)
	Receiving objects: 100% (20553/20553), 100.83 MiB | 634 KiB/s, done.
	Resolving deltas: 100% (14127/14127), done.
	Checking out files: 100% (4130/4130), done.

To support Android, iOS, and other platforms, you must have a version
of MonoGame (develop branch) version 3.0 available. The MonoGame
repository is a submodule of the Cocos2D-XNA project that all the
solutions reference.

To initialise and update the required MonoGame submodules that we
reference you will need to do the following:

In the CocosSharp directory to issue the following submodule commands.

> $ git submodule init

Output from above command:

	Submodule 'MonoGame' (https://github.com/Cocos2DXNA/MonoGame.git) registered for path 'MonoGame'
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

Notice above that we only need the ThirdParty/Libs to actually build
Cocos2D-XNA.

You now have everything you need to start start developing with
Cocos2D-XNA

Getting Started
---------------

### Test Bed

We have created solutions for all the supported platforms that serves
as our TestBed for each platform.

You can find those in the [tests directory](https://github.com/xamarin/CocosSharp/tree/master/tests "Test Bed")

	CocosSharp.Tests.Android.sln	
	CocosSharp.Tests.Ouya.sln	
	CocosSharp.Tests.Windows.sln	
	CocosSharp.Tests.Windows8.sln	
	CocosSharp.Tests.WindowsDX.sln	
	CocosSharp.Tests.WindowsGL.sln	
	CocosSharp.Tests.WindowsPhone.sln	
	CocosSharp.Tests.WindowsPhone7.sln	
	CocosSharp.Tests.iOS.sln	

### Samples

[Xamarin's Angry Ninjas](https://github.com/xamarin/AngryNinjas
"Xamarin’s Angry Ninjas") sample application. We worked hard on
converting that cocos2d-iphone game over to Cocos2D-XNA. Take a look
at how it deploys raw assets and utilizes box2d.

Additional samples will be forthcoming as we find time to add more for you. 

History
-------

This project is a fork of the Cocos2D-XNA project, which is a port of
the C++-based Cocos2D-X API, which in turn is a cross-platform port of
the cocos2d-iphone project.  

The focus of this fork is to create a library that is idiomatically
correct for C# and remove many of the historical warts inherited from
the straight ports from C++ and Objective-C.