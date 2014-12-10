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

# Update iOS Template NuGet version to be included in built Template
sed -i '' "s/version=\".*\" \/>/version=\"$1\" \/>/g" ../../ProjectTemplates/Templates/StarterTemplates/iOS/Template/EmptyProject.iOS.vstemplate 
sed -i '' "s/CocosSharp.iOS\..*.nupkg/CocosSharp.iOS.$1.nupkg/g" ../../ProjectTemplates/Templates/StarterTemplates/iOS/VSIX/EmptyProject.iOS/EmptyProject.iOS/EmptyProject.iOS.csproj
