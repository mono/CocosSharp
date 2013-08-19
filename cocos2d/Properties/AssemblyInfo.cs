using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Resources;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("cocos2d-xna")]
[assembly: AssemblyProduct("cocos2d-xna")]

#if OUYA
[assembly: AssemblyDescription("Cocos2D-XNA for Ouya")]
#else
#if WINDOWSDX
[assembly: AssemblyDescription("Cocos2D-XNA for Windows Desktop (DX)")]
#elif WINDOWSGL
[assembly: AssemblyDescription("Cocos2D-XNA for Windows Desktop (OpenGL)")]
#elif ANDROID
[assembly: AssemblyDescription("Cocos2D-XNA for Android")]
#elif IPHONE
[assembly: AssemblyDescription("Cocos2D-XNA for iOS")]
#elif WINRT
[assembly: AssemblyDescription("Cocos2D-XNA for Windows RT")]
#elif WINDOWS_PHONE8
[assembly: AssemblyDescription("Cocos2D-XNA for Windows Phone 8")]
#elif WINDOWS_PHONE
[assembly: AssemblyDescription("Cocos2D-XNA for Windows Phone 7")]
#elif XBOX360
[assembly: AssemblyDescription("Cocos2D-XNA for XBox 360")]
#elif MACOS
[assembly: AssemblyDescription("Cocos2D-XNA for Mac OSX")]
#else
[assembly: AssemblyDescription("Cocos2D-XNA for Windows Desktop (XNA)")]
#endif
#endif

[assembly: AssemblyCompany("Open Source Software Provided As-Is")]
[assembly: AssemblyCopyright("Copyright © Cocos2D-XNA Team; TotallyEvil Entertainment, LLC; Xamarin, Inc.")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type. Only Windows
// assemblies support COM.
[assembly: ComVisible(false)]

// On Windows, the following GUID is for the ID of the typelib if this
// project is exposed to COM. On other platforms, it unique identifies the
// title storage container when deploying this assembly to the device.
[assembly: Guid("b9c87433-71ae-45dc-963f-6d3166d5c4d7")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
[assembly: AssemblyVersion("2.0.3.0")]
[assembly: NeutralResourcesLanguageAttribute("en-US")]