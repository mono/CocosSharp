To build a new version you will need to update the .nupkg reference to the updated version of Cocos2D-XNA.Windows.<version>.nupkg

1. In GameStarterKitInstaller add a folder named "packages" if it does not already exist.
2. On "packages" folder: Add->Existing item. Find Cocos2D-XNA.Windows.<version>.nupkg and make sure to "Add as Link".
3. In the Properties grid for the Cocos2D-XNA.Windows.<version>.nupkg file set Build Action=Content & Include in VSIX=True
(this step causes the nupkg to be packaged in the VSIX in the packages subfolder where the template wizard expects to find the file)
4. Make sure that the version of the package is updated:
	a) Open the linked to file GameStarterKit.Windwos.vstemplate that is linked to within this project
	b) Change the version of the  <package id="Cocos2D-XNA.Windows" version="2.0.3.2" /> tag to the correct version to include.

   <packages repository="extension"
          repositoryId="GameStarterKitInstaller.852d8d2b-8ddc-4fd2-8312-53657ec6dfa3">
      <package id="Cocos2D-XNA.Windows" version="2.0.3.2" />
    </packages>

5. After modifying the template in step 4. above you will need to repackage the template:
	a) Delete the *.zip files in the Template Directory for the project that you are packaging
	b) Select all files in the directory and then Send To Compressed File - Name it GameStarterKit.Windows.zip ** Note ** Make sure it is in the bin directory
	
6. Ctrl+F5 to launch a test instance of VS, create a new project using the Cocos2D-XNA template that shows up that should now show up. Success!!
 

To build this project in batch

Execute the following:
msbuild GameStarterKitInstaller.sln /p:Configuration=Release

Example:
CocosSharp\ProjectTemplates\GameStarterKit\VSIX\GameStarterKitInstaller>msbuild GameStarterKitInstaller.sln /p:Configuration=Release