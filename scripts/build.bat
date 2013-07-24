call "C:\Program Files (x86)\Microsoft Visual Studio 10.0\VC\vcvarsall.bat x86"
set path=%path%;C:\Windows\Microsoft.NET\Framework\v4.0.30319;c:\windows\system32
nant -buildfile:android.build
nant -buildfile:microsoft.build
nant -buildfile:windows8.build
nant -buildfile:macos.build
nant -buildfile:deployments.build
pause
