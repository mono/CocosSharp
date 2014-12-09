#!/bin/sh
if test x$1 = x; then
   echo usage is: UpdReleaseVer version
   echo example: UpdReleaseVer 1.0.0.0
   exit 0
fi

# Update NuGet versions
find ../../ProjectTemplates/NuGet/*.nuspec -type f -exec sed -i '' "s#<version>.*</version>#<version>$1</version>#g" {} \;
# Update NuGet release note versions
find ../../ProjectTemplates/NuGet/*.nuspec -type f -exec sed -i '' "s#ReleaseNotes_v.*.md#ReleaseNotes_v$1.md#g" {} \;

