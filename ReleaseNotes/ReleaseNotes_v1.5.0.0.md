# CocosSharp v1.5.0.0 release notes

## Key new features
 ---

### Renderer pipeline

We have overhauled the way CocosSharp performs rendering, with a focus on improved performance. Please check out this article [here](link_to_forum_post) for an in-depth discussion on the design of our renderer pipeline and what it means for developers.

### Templates

Xamarin Studio and Visual Studio templates have been updated with the latest PCL package for this release.

Visual Studio templates now have three new templates.
* Windows Phone 8.1
* Mobile
 - Portable - Android, iOS and Windows Phone 8.1
   - This template will create a portable PCL project solution.  
 - Shared - Android, iOS and Windows Phone 8,1
   - This template will create a shared project solution 

## Breaking changes
 ---

* Due to the inclusion of the new renderer pipeline, __<code>CCSpriteBatchNode</code> has been marked obsolete__, however to avoid confusion will me omitted entirely in future releases. Please see the article [here](link_to_forum_post) for details on how to migrate away from this class.


* __<code>CCRenderTexture</code> is longer derived from <code>CCNode</code>__. Previously, <code>CCRenderTexture</code> served a dual role where it could both be attached to the scene-graph or used externally, with the corresponding <code>Sprite</code> instead added to a scene. For example

<pre>
<code>
CCRender renderTexture;
CCSprite renderTexSprite;

// Initialise render texture

renderTexture.Begin();
// Draw into texture
renderTexture.End();

// Previously, could add texture to a parent node
parentNode.AddChild(renderTexture);


// Alternatively, get corresponding sprite and add that
// This is the approach to use from v.1.5.0.0 onwards
renderTexSprite = renderTexture.Sprite;
parentNode.AddChild(renderTexSprite);
</code>
</pre>

So in CocosSharp v1.5.0.0 onwards we have simply removed the ability to add a render texture to a parent node, which is consistent with the role it serves.

* __Attach <code>CCGeometryBatch</code> to scene and rename to <code>CCGeometryNode</code>__. To integrate with our new renderer pipeline, <code>CCGeometryBatch</code> is now derived from <code>CCNode</code> and needs to be attached to a parent node to be renderable. To emphasise this change in behavior, we renamed <code>CCGeometryBatch</code> to <code>CCGeometryNode</code>. Migrating to use <code>CCGeometryNode</code> should be relatively straight-forward. For example

<pre>
<code>
	
// Create node and add to parent
CCGeometryNode geoNode = new CCGeometryNode();
parentNode.AddChild(geoNode);

// Thereafter, setting up drawing geometry should be exactly the same as with CCGeometryBatch

// e.g. Create geom instance with 3 vertices and 3 indices
CCGeometryInstance geomInstance = geoBatch.CreateGeometryInstance(3, 3);

// Populate instance as with CCGeometryBatch


// No need to explicitly flush render commands
// By being added to the scene-graph, rendering is performed implicitly!

</code>
</pre>


* __<code>CCDrawPrimitives</code>__ has now been marked obsolete. Users should instead make use of the pre-existing <code>CCDrawNode</code> class which integrates with the new renderer pipeline, or alternatively use <code>CCGeometryNode</code> to construct more customisable primitives.

* __NuGet packages__: To cut down on the confusion of which NuGet package to include in your projects we will only be delivering one NuGet package that includes only the PCL assemblies for the following platforms.  Developers will need to update their projects to use this new package to obtain the updates.  __CococsSharp.PCL.Shared.nupkg__
  *  Android
  *  iOS
  *  Windows DX
  *  Windows 8
  *  Windows Phone 8.1
  *  Windows Phone 8.0 - __Note:__ Windows Phone 8.0 will be obsolete in the the next release.

* __Effects:__ During the Renderer Pipeline modifications we made changes to all the effects that use a GridBase, examples being <code>CCLiquid</code>, <code>CCShaky</code> etc.  Any of these GridBase effects will need to target a <code>CCNodeGrid</code>.  To do that add the <code>CCNode</code> that will be targeted to an instance of a <code>CCNodeGrid</code>.  This breaking change greatly simplifies the rendering code and our source code base.  Every rendering loop there were checks for Grid usage even when not being used which resulted in unnessasary processing cycles and making the rendering source hard to manage. The following demonstrates how to run an <code>Effect</code> targeting a <code>CCSprite</code> that is wrapped in a <code>CCNodeGrid</code>.
<code>

// Define our Effect Action
var fadeOut = new CCFadeOutBLTiles(1f, new CCGridSize(10, 10));

// Create a target CCNodeGrid because the FadeOutBLTiles uses a grid
var targetNode = new CCNodeGrid();
AddChild(targetNode);

// Create our sprite
var sprite = new CCSprite("grosinni");
sprite.Position = this.ContentSize.Center;

// our sprite needs to be wrapped in a CCNodeGrid
targetNode.AddChild(sprite);

// Run the action against the wrapping CCNodeGrid targetNode 
targetNode.RunAction (fadeOut);
</code>

* __v.1.4.0.0 reminder__: Usage of the obsolete <code>CCLabelBMFont</code> and <code>CCLabelTtf</code> classes should be replaced with unified label class <code>CCLabel</code> (see [here](http://forums.xamarin.com/discussion/37873/cocossharp-v1-4-0-0-release)). These obsolete classes will be removed in future releases!

## Fixes and enhancements 
 ---
* [241](https://github.com/mono/CocosSharp/issues/241) CCSpriteFrameCache.RemoveSpriteFrames does not remove frames
* [240](https://github.com/mono/CocosSharp/issues/240) Layering CCLayerGradient doesn't obey ContentSize, Opacity, or Gradient
* [239](https://github.com/mono/CocosSharp/issues/239) Typo in parameter name in CCLayer's Visit() &mdash; thanks to [Krumelur](https://github.com/Krumelur)
* [238](https://github.com/mono/CocosSharp/issues/238) CocosSharp templates do not appear in Xamarin Studio on Windows &mdash; thanks to [vchelaru](https://github.com/vchelaru)
* [237](https://github.com/mono/CocosSharp/issues/237) CocosSharp Wrench .nuspec for signed assemblies
* [236](https://github.com/mono/CocosSharp/issues/236) CCDrawNode: Bounding Rectangle
* [235](https://github.com/mono/CocosSharp/issues/235) CCDrawNode incorrect calculation of ContentSize Height.
* [234](https://github.com/mono/CocosSharp/issues/234) Xamarin Studio CocosSharp templates should link against the latest NuGet &mdash; thanks to [vchelaru](https://github.com/vchelaru)
* [233](https://github.com/mono/CocosSharp/issues/233) PCL: Box2D.Dynamics.b2World could not be loaded
* [232](https://github.com/mono/CocosSharp/issues/232) Windows Cross-platform PCL project template for mobile devices.
* [231](https://github.com/mono/CocosSharp/issues/231) NuGet .nuspec definitions need to be changed to use -BasePath
* [230](https://github.com/mono/CocosSharp/issues/230) CocosSharp should support Visual Studio 2015 templates
* [228](https://github.com/mono/CocosSharp/issues/228) Implement renderer pipeline
* [227](https://github.com/mono/CocosSharp/issues/227) CocosSharp Windows Store template has error when created
* [226](https://github.com/mono/CocosSharp/issues/226) Windows Phone 8.1 templates
* [225](https://github.com/mono/CocosSharp/issues/225) Overhaul CCLayerMultiplex
* [220](https://github.com/mono/CocosSharp/issues/220) CCScene: Actions run on base scene do not work correctly
* [219](https://github.com/mono/CocosSharp/issues/219) CCSprite: Add convenience methods to reset texture rect region
* [218](https://github.com/mono/CocosSharp/issues/218) CCLabel : System Font Label layout differences between different Android versions 17,19 and 21
* [217](https://github.com/mono/CocosSharp/issues/217) CCLabel: Labels are causing flickering on Android Devices
* [216](https://github.com/mono/CocosSharp/issues/216) Replace internal zip implementation with MonoGame.Utilities implementation 
* [214](https://github.com/mono/CocosSharp/issues/214) CCTileMapLayer: Correct culling when local transform is not identity  
* [213](https://github.com/mono/CocosSharp/issues/213) Setting CCLabel.IsAntialiased = false throws NullReferenceException
* [211](https://github.com/mono/CocosSharp/issues/211) Curious artifacts with a CCLabel on top of a CCSprite
* [210](https://github.com/mono/CocosSharp/issues/210) TileMaps: Allow reading in of native external tsx files &mdash; thanks to [vchelaru](https://github.com/vchelaru)
* [208](https://github.com/mono/CocosSharp/issues/208) Rename CCGeometryBatch to CCGeometryNode
* [207](https://github.com/mono/CocosSharp/issues/207) CCGeometryBatch: Attach to scene graph
* [205](https://github.com/mono/CocosSharp/pull/205) CCTileMapLayer: Add support for multiple tile-sets &mdash; thanks to [gdwneo](https://github.com/gdwneo)
* [204](https://github.com/mono/CocosSharp/pull/204) CCTouch.StartLocation has correct value after constructor &mdash; thanks to [KalitaAlexey](https://github.com/KalitaAlexey)
* [202](https://github.com/mono/CocosSharp/issues/202) CCLabel: wrong color after fade animation
* [201](https://github.com/mono/CocosSharp/issues/201) Resetting properties of a CCNode when accessing it in CCLabel
* [200](https://github.com/mono/CocosSharp/issues/200) Render a CCLabel in a CCTextureRender causes NRE
* [199](https://github.com/mono/CocosSharp/issues/199) VS: Unable to install project templates
* [198](https://github.com/mono/CocosSharp/pull/198) Fix: Wrong rendering of hex tileset (handles #197) &mdash; thanks to [omd](https://github.com/omd)
* [196](https://github.com/mono/CocosSharp/issues/196) CCRenderTexture: Detach from scene graph
* [192](https://github.com/mono/CocosSharp/issues/192) Can't animate CCLabel by Opacity
* [191](https://github.com/mono/CocosSharp/issues/191) CCNode: Clean up z-ordering
* [186](https://github.com/mono/CocosSharp/pull/186) SearchResolutionOrder and SearchPaths contain at least empty string &mdash; thanks to [KalitaAlexey](https://github.com/KalitaAlexey)
* [184](https://github.com/mono/CocosSharp/issues/184) Unnecessary complexity and clearing stack at TileContainer
* [183](https://github.com/mono/CocosSharp/issues/183) CCSpriteFontCache InternalLoadFont wrong algorithm
* [181](https://github.com/mono/CocosSharp/issues/181) NullReferenceException on attempt create sprite font
* [180](https://github.com/mono/CocosSharp/issues/180) CCLabel Descendants property needs to be marked as non public
* [179](https://github.com/mono/CocosSharp/issues/179) XML files should be distributed with CocosSharp templates
* [177](https://github.com/mono/CocosSharp/issues/177) Signing of CocosSharp via Wrench.
* [174](https://github.com/mono/CocosSharp/issues/174) CCSprite (and CCNode) Rotation opposite of mathematical rotation
* [173](https://github.com/mono/CocosSharp/issues/173) CCLabel does not display alpha correctly on Android
* [164](https://github.com/mono/CocosSharp/issues/164) Setting PositionX of CCRenderTexture results in wobbling on iPhone devices
* [118](https://github.com/mono/CocosSharp/issues/118) WP8: CCSprite with PNG stream exception
* [81](https://github.com/mono/CocosSharp/issues/81) RemoveSpriteFrame
