call "C:\Program Files (x86)\Microsoft Visual Studio 10.0\VC\vcvarsall.bat x86"
set path=%path%;C:\Windows\Microsoft.NET\Framework\v4.0.30319;c:\windows\system32;c:\nant-0.92\bin
nant -buildfile:windows8.build > windows8.out 2>&1
nant -buildfile:deploy-Windows8.build > deploy.out 2>&1
