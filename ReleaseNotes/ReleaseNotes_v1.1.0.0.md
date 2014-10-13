# CocosSharp v1.1.0.0 release notes
 
## Breaking changes 
 ---
* Previously, within the overridden <code>CCApplicationDelegate</code> classes users would specify the desired <code>CCWindow:SupportedDisplayOrientations</code>. For example,
<pre>
<code>
	public override void ApplicationDidFinishLaunching(CCApplication application, CCWindow mainWindow)
    {
		...
		mainWindow.SupportedDisplayOrientations = CCDisplayOrientation.Portrait;
		...
</code>
</pre>
* From v1.1.0.0 there is no longer a need to do this as the supported orientations will be determined from the platform-specific metadata (e.g. Info.plist for iOS).
* Additionally, the property setters for <code>CCWindow:SupportedDisplayOrientations</code> and <code>CCWindow.CurrentDisplayOrientation</code> no longer exist as certain platforms do not support programmatically setting these properties after initialisation and these setters previously did nothing. Thus, so that users aren't misled the setters have been removed.  



## Fixes and enhancements 
 ---
* [25](https://github.com/mono/CocosSharp/issues/25) Remove need to set supported orientations in overridden CCApplicationDelegate
* [24](https://github.com/mono/CocosSharp/issues/24) Mac: Full-screen app crashes
* [22](https://github.com/mono/CocosSharp/issues/22) iOS: Viewport not correctly set after orientation change
* [21](https://github.com/mono/CocosSharp/issues/21) CCSprite: ContentSize change after instantiation not working
* [16](https://github.com/mono/CocosSharp/issues/16) Latest MonoGame linking fails when deploying Android tests project to device
* [15](https://github.com/mono/CocosSharp/issues/15) Android: Graphics context being destroyed when Pausing/Resuming
* [14](https://github.com/mono/CocosSharp/issues/14) CCBox2dDraw constructor/draw calls don't accommodate for PTM_RATIO
* [12](https://github.com/mono/CocosSharp/issues/12) CCScene Layer property can be set
* [10](https://github.com/mono/CocosSharp/pull/10) CCNode: Fix loop in Visit() to not break on invisible children.
