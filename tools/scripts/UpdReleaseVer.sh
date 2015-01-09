#!/bin/sh
if test x$1 = x; then
   echo usage is: UpdReleaseVer version
   echo example: ./UpdReleaseVer.sh 1.0.0.0
   exit 0
fi

# Update NuGet versions
find ../../ProjectTemplates/NuGet/*.nuspec -type f -exec sed -i '' "s#<version>.*</version>#<version>$1</version>#g" {} \;
# Update NuGet release note versions
find ../../ProjectTemplates/NuGet/*.nuspec -type f -exec sed -i '' "s#ReleaseNotes_v.*.md#ReleaseNotes_v$1.md#g" {} \;

# Update Android Template NuGet version to be included in built Template
sed -i '' "s/version=\".*\" \/>/version=\"$1\" \/>/g" ../../ProjectTemplates/Templates/StarterTemplates/Android/Template/EmptyProject.Android.vstemplate 
sed -i '' "s/CocosSharp.Android\..*.nupkg/CocosSharp.Android.$1.nupkg/g" ../../ProjectTemplates/Templates/StarterTemplates/Android/VSIX/EmptyProject.Android/EmptyProject.Android/EmptyProject.Android.csproj
sed -i '' "s#<Version>.*</Version>#<Version>$1</Version>#g" ../../ProjectTemplates/Templates/StarterTemplates/Android/VSIX/EmptyProject.Android/EmptyProject.Android/source.extension.vsixmanifest

# Update iOS Template NuGet version to be included in built Template
sed -i '' "s/version=\".*\" \/>/version=\"$1\" \/>/g" ../../ProjectTemplates/Templates/StarterTemplates/iOS/Template/EmptyProject.iOS.vstemplate 
sed -i '' "s/CocosSharp.iOS\..*.nupkg/CocosSharp.iOS.$1.nupkg/g" ../../ProjectTemplates/Templates/StarterTemplates/iOS/VSIX/EmptyProject.iOS/EmptyProject.iOS/EmptyProject.iOS.csproj
sed -i '' "s#<Version>.*</Version>#<Version>$1</Version>#g" ../../ProjectTemplates/Templates/StarterTemplates/iOS/VSIX/EmptyProject.iOS/EmptyProject.iOS/source.extension.vsixmanifest

# Update Windows Phone 8 Template NuGet version to be included in built Template
sed -i '' "s/version=\".*\" \/>/version=\"$1\" \/>/g" ../../ProjectTemplates/Templates/StarterTemplates/WP8/Template/EmptyProject.WP8.vstemplate 
sed -i '' "s/CocosSharp.WindowsPhone8\..*.nupkg/CocosSharp.WindowsPhone8.$1.nupkg/g" ../../ProjectTemplates/Templates/StarterTemplates/WP8/VSIX/EmptyProject.WP8/EmptyProject.WP8/EmptyProject.WP8.csproj
sed -i '' "s#<Version>.*</Version>#<Version>$1</Version>#g" ../../ProjectTemplates/Templates/StarterTemplates/WP8/VSIX/EmptyProject.WP8/EmptyProject.WP8/source.extension.vsixmanifest

# Update Windows DX Template NuGet version to be included in built Template
sed -i '' "s/version=\".*\" \/>/version=\"$1\" \/>/g" ../../ProjectTemplates/Templates/StarterTemplates/WindowsDX/Template/EmptyProject.WindowsDX.vstemplate 
sed -i '' "s/CocosSharp.WindowsDX\..*.nupkg/CocosSharp.WindowsDX.$1.nupkg/g" ../../ProjectTemplates/Templates/StarterTemplates/WindowsDX/VSIX/EmptyProject.WindowsDX/EmptyProject.WindowsDX/EmptyProject.WindowsDX.csproj
sed -i '' "s#<Version>.*</Version>#<Version>$1</Version>#g" ../../ProjectTemplates/Templates/StarterTemplates/WindowsDX/VSIX/EmptyProject.WindowsDX/EmptyProject.WindowsDX/source.extension.vsixmanifest

# Update Windows Store Template NuGet version to be included in built Template
sed -i '' "s/version=\".*\" \/>/version=\"$1\" \/>/g" ../../ProjectTemplates/Templates/StarterTemplates/Windows8/Template/EmptyProject.Windows8.vstemplate 
sed -i '' "s/CocosSharp.Windows8\..*.nupkg/CocosSharp.Windows8.$1.nupkg/g" ../../ProjectTemplates/Templates/StarterTemplates/Windows8/VSIX/EmptyProject.Windows8/EmptyProject.Windows8/EmptyProject.Windows8.csproj
sed -i '' "s#<Version>.*</Version>#<Version>$1</Version>#g" ../../ProjectTemplates/Templates/StarterTemplates/Windows8/VSIX/EmptyProject.Windows8/EmptyProject.Windows8/source.extension.vsixmanifest

# Update Windows8 XAML Template NuGet version to be included in built Template
sed -i '' "s/version=\".*\" \/>/version=\"$1\" \/>/g" ../../ProjectTemplates/Templates/StarterTemplates/Win8XAML/Template/EmptyProject.Win8XAML.vstemplate 
sed -i '' "s/CocosSharp.Windows8\..*.nupkg/CocosSharp.Windows8.$1.nupkg/g" ../../ProjectTemplates/Templates/StarterTemplates/Win8XAML/VSIX/EmptyProject.Win8XAML/EmptyProject.Win8XAML/EmptyProject.Win8XAML.csproj
sed -i '' "s#<Version>.*</Version>#<Version>$1</Version>#g" ../../ProjectTemplates/Templates/StarterTemplates/Win8XAML/VSIX/EmptyProject.Win8XAML/EmptyProject.Win8XAML/source.extension.vsixmanifest

# Update Windows GL Template NuGet version to be included in built Template
sed -i '' "s/version=\".*\" \/>/version=\"$1\" \/>/g" ../../ProjectTemplates/Templates/StarterTemplates/WindowsGL/Template/EmptyProject.WindowsGL.vstemplate 
sed -i '' "s/CocosSharp.WindowsGL\..*.nupkg/CocosSharp.WindowsGL.$1.nupkg/g" ../../ProjectTemplates/Templates/StarterTemplates/WindowsGL/VSIX/EmptyProject.WindowsGL/EmptyProject.WindowsGL/EmptyProject.WindowsGL.csproj
sed -i '' "s#<Version>.*</Version>#<Version>$1</Version>#g" ../../ProjectTemplates/Templates/StarterTemplates/WindowsGL/VSIX/EmptyProject.WindowsGL/EmptyProject.WindowsGL/source.extension.vsixmanifest

# Update XS project template
sed -i '' "1,/version.*=\".*\".*>/s/version.*=\".*\".*>/version         =\"$1\">/" ../../ProjectTemplates/XamarinStudio/MonoDevelop.CocosSharp.addin.xml
sed -i '' "s/CocosSharp\.Android\..*.nupkg/CocosSharp\.Android\.$1\.nupkg/g" ../../ProjectTemplates/XamarinStudio/MonoDevelop.CocosSharp.addin.xml
sed -i '' "s/CocosSharp\.iOS\..*.nupkg/CocosSharp\.iOS\.$1\.nupkg/g" ../../ProjectTemplates/XamarinStudio/MonoDevelop.CocosSharp.addin.xml
sed -i '' "s/CocosSharp\.MacOS\..*.nupkg/CocosSharp\.MacOS\.$1\.nupkg/g" ../../ProjectTemplates/XamarinStudio/MonoDevelop.CocosSharp.addin.xml
sed -i '' "s/CocosSharp\.PCL.Shared\..*.nupkg/CocosSharp\.PCL\.Shared\.$1\.nupkg/g" ../../ProjectTemplates/XamarinStudio/MonoDevelop.CocosSharp.addin.xml

# Update CocosSharp core library assembly version
sed -i '' "s/AssemblyVersion(\".*\")/AssemblyVersion(\"$1\")/g" ../../src/Properties/AssemblyInfo.cs
sed -i '' "s/AssemblyInformationalVersion(\".*\")/AssemblyInformationalVersion(\"$1\")/g" ../../src/Properties/AssemblyInfo.cs

# Update CocosSharp PCL assembly version
sed -i '' "s/AssemblyVersion(\".*\")/AssemblyVersion(\"$1\")/g" ../../PCL/CocosSharpPCLShared/Properties/AssemblyInfo.cs
sed -i '' "s/AssemblyInformationalVersion(\".*\")/AssemblyInformationalVersion(\"$1\")/g" ../../PCL/CocosSharpPCLShared/Properties/AssemblyInfo.cs
