To build the .nuspec 
====================

There are two ways to build the .nuspec, via command line or 

Creating Packages Using the NuGet Command Line
--------------------

- 1) Open a command prompt and navigate to an empty folder. 
- 2) Make sure the nuget.exe file can be found, and run the following command:
	
	NuGet.exe pack Cocos2D.Windows.nuspec -OutputDirectory "bin"

** Note ** make sure to add the -OutputDirectory "bin" to keep them from git`s view or they will be committed to the repo.  If necassary create the bin directory as well.

Creating Packages using NuGet Package Explorer
--------------------

- 1) Load the package up in the NuGet Package Explorer
- 2) Modify what you want
- 3) Save the new .nupkg in the bin file

