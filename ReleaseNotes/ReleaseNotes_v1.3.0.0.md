# CocosSharp v1.3.0.0 release notes

## Breaking changes / new features
---

* Overhaul of tile-map support. See [here](http://forums.xamarin.com/discussion/30568/cocossharp-v1-3-0-0-tile-map-support-overhaul-release-notes#latest) for in-depth details

* <code>CCWindow:SetDesignResolutionSize</code> has been marked obsolete and is replaced by the static method

<pre>
<code>
CCScene.SetDefaultDesignResolution(float width, float height, CCSceneResolutionPolicy resolutionPolicy)
</code>
</pre>

This change was motivated by issue [79](https://github.com/mono/CocosSharp/issues/79) that highlighted that certain resolution policies may result in changes to a scene's design resolution that differ from the default resolution specified by a user.

## Fixes and enhancements 
 ---
* [90](https://github.com/mono/CocosSharp/issues/90) Windows templates need to be incorporated into a single deliverable
* [89](https://github.com/mono/CocosSharp/issues/89) Overhaul Tile map support
* [88](https://github.com/mono/CocosSharp/issues/88) CCLayerColor: quad can obscure child nodes
* [87](https://github.com/mono/CocosSharp/issues/87) iOS 8.1: iPad rotation not working correctly
* [86](https://github.com/mono/CocosSharp/issues/86) CCSceneResolutionPolicy not respected
* [85](https://github.com/mono/CocosSharp/pull/85) Set property DeviceManager (thanks to hig-ag)
* [80](https://github.com/mono/CocosSharp/issues/80) CCScene: Should keep track of its own design resolution size
* [79](https://github.com/mono/CocosSharp/issues/79) CCWindow.SetDesignResolutionSize with FixedWidth/FixedHeight distorts game view
* [71](https://github.com/mono/CocosSharp/issues/71) Templates for Visual Studio for non mobile platforms
* [70](https://github.com/mono/CocosSharp/issues/70) XS Template build needs to be incorporated into build process
* [69](https://github.com/mono/CocosSharp/issues/69) CocosSharp XS Android Template creates unnecessary source items
* [67](https://github.com/mono/CocosSharp/issues/67) Templates for Visual Studio
* [66](https://github.com/mono/CocosSharp/issues/66) Memory Leak in CCNode
* [65](https://github.com/mono/CocosSharp/issues/65) CCShaky3D: strange behavior with HD textures and child elements
* [63](https://github.com/mono/CocosSharp/pull/63) Fixed null reference exception in CCRenderTexture
* [61](https://github.com/mono/CocosSharp/issues/61) CCRenderTexture.Begin() throws Null reference exception
* [56](https://github.com/mono/CocosSharp/issues/56) CCSpriteBatchNode resetting to position (0,0)
* [54](https://github.com/mono/CocosSharp/issues/54) CCTXTileMap: Scrolling issue
* [52](https://github.com/mono/CocosSharp/issues/52) Integrate CocosSharp content building in MonoGame pipeline tool
