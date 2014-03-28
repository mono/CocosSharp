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

Git
---

When you first check out, run

   git clone --recursive git@github.com:xamarin/CocosSharp

so you will have all the submodules checked out for you.

With existing checkouts, run

   git submodule update --init --recursive

to make sure you get the latest changes in the submodules. Repos that
were checked out recursively will do this automatically, but it
doesn't hurt to run this manually.

To pull external changes into a submodule

   cd <submodule>
   git pull origin <branch>
   cd <top-level>; git add <submodule>
   git commit

To make changes in a submodule

   cd <submodule>

   # By default, submodules are detached because they point to a
   # specific commit. Use git-checkout to put yourself back on a
   # branch.
   #
   git checkout <branch>

   work as normal, the submodule is a normal repo

   git commit/push new changes to the repo (submodule)

   cd <top-level>; git add <submodule> # this will record the new commits to xamcore

   git commit

* To switch the repo of a submodule

   edit '.gitmodules' to point to the new location

   git submodule sync -- <path of the submodule> # updates .git/config

   # I think this will checkout from the new location, internally. It
   # may take a while for big repos.
   #
   git submodule update --recursive

   git checkout <desired new hash> # This changes the pointer of the submodule

The desired output diff is a change in .gitmodule to reflect the
change in the remote URL, and a change in /<submodule> where you see
the desired change in the commit hash


You now have everything you need to start start developing with
CocosSharp

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
	CocosSharp.Tests.MacOS.sln	

### Samples

[Xamarin's Angry Ninjas](https://github.com/xamarin/AngryNinjas
"Xamarin’s Angry Ninjas") sample application. We worked hard on
converting that cocos2d-iphone game over to Cocos2D-XNA. Take a look
at how it deploys raw assets and utilizes box2d.

Additional samples will be forthcoming as we find time to add more for you. 

### Statistics

There is a special case for Xamarin iOS MonoTouch running on the simulator where they aggresively call garbage collection themselves.  This should not affect the devices though.  On the Simulator the GC label will always be 0 (zero)


History
-------

This project is a fork of the Cocos2D-XNA project, which is a port of
the C++-based Cocos2D-X API, which in turn is a cross-platform port of
the cocos2d-iphone project.  

The focus of this fork is to create a library that is idiomatically
correct for C# and remove many of the historical warts inherited from
the straight ports from C++ and Objective-C.
