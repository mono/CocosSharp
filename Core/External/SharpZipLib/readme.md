SharpZipLib
-----------

Original source files from github : https://github.com/icsharpcode/SharpZipLib

Webpage for this library is here: http://icsharpcode.github.io/SharpZipLib/

CocosSharp uses this library's source code to implement Zip decompression from streams loaded from external configuration files like Tiled.

The sources have been modified from the original source only to mark the `public` accessors as `internal` so that
the PCL API and CocosSharp assemblies that we provide do not allow access to these modules externally.  Other than that 
there are no modifications to original functionality.
