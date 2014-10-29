# CocosSharp v1.2.0.0 release notes

## Breaking changes 
 ---
* <code>CCViewportResolutionPolicy</code> has now been removed. This allowed for some finer, albeit unnecessary, customisation of the viewport. However it is unlikely that this will have any impact on users who typically rely on setting the <code>CCSceneResolutionPolicy</code> during initialisation. In fact, the existence of two distinct resolution policies for <code>CCScene</code> and <code>CCViewport</code> was likely to cause some confusion to users, which was one of the motivations for its removal.
 
## Fixes and enhancements 
 ---
* [48](https://github.com/mono/CocosSharp/issues/48) Add Default Projection property
* [47](https://github.com/mono/CocosSharp/issues/47) CCLayer: Add constructor to specify camera projection
* [46](https://github.com/mono/CocosSharp/issues/46) iOS: CCMusicPlayer: Volume ignored when opening a file
* [44](https://github.com/mono/CocosSharp/issues/44) iOS: BackgroundMusicVolume has no effect
* [43](https://github.com/mono/CocosSharp/issues/43) AnimationInterval on CCWindow not working
* [42](https://github.com/mono/CocosSharp/issues/42) Add new Xamarin Studio template for Mac projects
* [41](https://github.com/mono/CocosSharp/issues/41) CCLabelBMFont positioning when using constructor specifying width
* [40](https://github.com/mono/CocosSharp/issues/40) iOS: If only landscape orientation is enabled, app with a wrong window position
* [39](https://github.com/mono/CocosSharp/issues/39) iOS: Errors when changing orientation
* [38](https://github.com/mono/CocosSharp/issues/38) Remove superfluous CCViewportResolutionPolicy
* [36](https://github.com/mono/CocosSharp/issues/36) CCLabelBMFont: ContentSize is 0x0 during initialisation
* [32](https://github.com/mono/CocosSharp/issues/32) Error calling CCApplication with no size
* [30](https://github.com/mono/CocosSharp/issues/30) CCSceneResolutionPolicy.ShowAll not behaving correctly
* [26](https://github.com/mono/CocosSharp/issues/26) Android app crashes when started in landscape mode but the app requires portrait mode
* [23](https://github.com/mono/CocosSharp/issues/23) CCLabelBMFont: Unable to run CCAction on label
* [20](https://github.com/mono/CocosSharp/issues/20) Visible bounds not perfectly matching default set by DesignResolutionSize
* [18](https://github.com/mono/CocosSharp/issues/18) Add new Xamarin Studio template for Android projects
* [17](https://github.com/mono/CocosSharp/issues/17) Add new Xamarin Studio template for iOS projects
