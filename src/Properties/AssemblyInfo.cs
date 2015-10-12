using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Resources;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("CocosSharp")]
[assembly: AssemblyProduct("CocosSharp")]

#if WINDOWSDX
[assembly: AssemblyDescription("CocosSharp for Windows Desktop (DX)")]
#elif WINDOWSGL
[assembly: AssemblyDescription("CocosSharp for Windows Desktop (OpenGL)")]
#elif ANDROID
[assembly: AssemblyDescription("CocosSharp for Android")]
#elif IPHONE
[assembly: AssemblyDescription("CocosSharp for iOS")]
#elif NETFX_CORE
[assembly: AssemblyDescription("CocosSharp for Windows RT")]
#elif WINDOWS_PHONE8
[assembly: AssemblyDescription("CocosSharp for Windows Phone 8")]
#elif MACOS
[assembly: AssemblyDescription("CocosSharp for Mac OSX")]
#else
[assembly: AssemblyDescription("CocosSharp for Windows Desktop (XNA)")]
#endif

[assembly: AssemblyCompany("Open Source Software Provided As-Is")]
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
[assembly: AssemblyVersion("1.7.0.0")]
[assembly: AssemblyInformationalVersion("1.7.0.0-pre1")]
[assembly: NeutralResourcesLanguageAttribute("en-US")]

