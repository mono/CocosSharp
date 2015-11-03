# CocosSharp v1.7.0.0-pre1 release notes 

## All new embeddable game view and Xamarin.Forms support
 ---
You asked for it and we have delivered! In this prerelease, we have substantially changed the way a user initialises their game with the new <code>CCGameView</code> class that allows users to embed their game view within a native application. 

Additionally, Xamarin.Forms users are also covered with the new custom Forms view, <code>CocosSharpView</code>.

To get started, we strongly recommend reading more about these design changes [here](https://github.com/mono/CocosSharp/blob/forms_support/ReleaseNotes/Forms.md).

## Installing the prerelease packages
 ---
From v1.7.0.0-pre1 onwards, two packages will be made available to users &mdash; <code>CocosSharp</code> and <code>CocosSharp.Forms</code>, with the latter package required for Xamarin.Forms users.

Please keep in mind that as this is currently a prerelease, your NuGet package manager may not be immediately able to locate these packages. In particular, in Xamarin Studio, make sure to tick the <em>Show pre-release packages</em> option when adding packages, while in Visual Studio's package manager, ensure that you change the dropdown box from <em>Stable only</em> to <em>Include Prerelease</em>.

## Project templates
 ---
We have also updated our suite of Xamarin Studio and Visual Studio project templates to reflect the changes in initialisation. Remember, to install these templates for Xamarin Studio and Visual Studio refer to the guides [here](http://forums.xamarin.com/discussion/26822/cocossharp-project-templates-for-xamarin-studio) and [here](http://forums.xamarin.com/discussion/30701/cocossharp-project-templates-for-visual-studio) respectively. 

## Pitfalls
 ---
* The design document [here](https://github.com/mono/CocosSharp/blob/forms_support/ReleaseNotes/Forms.md) covers some of the limitations of using the new <code>CCGameView</code>.

* Additionally, for Xamarin.Forms users targeting the Android platform, please ensure that you're compile target is for API 23 (6.0). 

## Other breaking changes
 ---

* __Fixed Content directory:__ We have fixed the location for where the Content root directory should reside to be in the folder <em>Content</em>.  In practice, providing the users with this flexibility to specify the location gained them nothing and, in the worse case, forgetting to set the root directory resulted in a missing content exception being thrown. So just like other platform-specific resource files, there’s a consistent location where content should reside and a user doesn’t have to worry about specifying it during initialisation.

* __CCSimpleAudioEngine has been marked as obsolete and replaced with CCAudioEngine:__ Currently, <code>CCSimpleAudioEngine</code> is under the <code>CocosDenshion</code> namespace while all other classes reside within <code>CocosSharp</code>. This resulted in the odd situation where users were required to import this additional namespace to use audio. Hence, we have moved to replace <code>CCSimpleAudio</code> with <code>CCAudioEngine</code> which resides in <code>CocosSharp</code>.

* __CCScene: AddChild has been marked obsolete and replaced with the AddLayer:__  In an attempt to further differentiate the roles of <code>CCScene</code> and <code>CCLayer</code>, we have replaced the CCScene:AddChild(CCNode) method with CCScene:AddLayer(CCLayer) to emphasise that a scene’s sole role is to house game layers. In subsequent releases, we will move to make this type-safe, so that a user is unable to add nodes that aren’t layers to a scene.

## Other fixes and enhancements 
 ---
* [319](https://github.com/mono/CocosSharp/pull/319) [wp81] fix for CCUserDefaults.Flush corrupting files &mdash; thanks to [alexsorokoletov](https://github.com/alexsorokoletov)
* [318](https://github.com/mono/CocosSharp/pull/318) Update mono path to reflect el capitan &mdash; thanks to [Therzok](https://github.com/Therzok)
* [317](https://github.com/mono/CocosSharp/pull/317) Fixed typographical error, changed aggresive to aggressive in README &mdash; thanks to [orthographic-pedant](https://github.com/orthographic-pedant)
* [223](https://github.com/mono/CocosSharp/issues/223) Assigning CCLayer.Viewport throws uninformative exception
* [64](https://github.com/mono/CocosSharp/issues/64) Significant Stutter on Android
