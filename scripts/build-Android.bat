call "C:\Program Files (x86)\Microsoft Visual Studio 10.0\VC\vcvarsall.bat x86"
set path=%path%;C:\Windows\Microsoft.NET\Framework\v4.0.30319;c:\windows\system32;c:\nant-0.92\bin
nant -buildfile:android.build > android.out 2>&1
nant -buildfile:deploy-Android.build > deploy.out 2>&1
