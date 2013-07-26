call "C:\Program Files (x86)\Microsoft Visual Studio 10.0\VC\vcvarsall.bat x86"
set path=%path%;C:\Windows\Microsoft.NET\Framework\v4.0.30319;c:\windows\system32
nant -buildfile:android.build > android.out 2>&1
nant -buildfile:microsoft.build > microsoft.out 2>&1
nant -buildfile:windows8.build > windows8.out 2>&1
nant -buildfile:macos.build > macos.out 2>&1
nant -buildfile:deployments.build > deployments.out 2>&1
pause
