# CocosSharp v1.3.2.0 release notes

## Breaking changes 
 ---
None

## Key new features
 ---
### Async/await support for CCActions

<code>CCNode</code> now comes bundled with the methods

* <code>RunActionAsync(CCFiniteTimeAction action)</code>
* <code>RunActionsAsync(params CCFiniteTimeAction[] actions)</code>

that allows a user to chain together a sequence of <code>CCAction</code>'s with more general <code>Task</code> objects. e.g.

<pre>
<code>
async void MyAwesomeActionSequence()
{
	await node.RunActionAsync(myaction1);
	await Task.Delay(100);
	await node.RunActionAsync(myaction2);
}
</code>
</pre>

Note, that the traditional approach of using a <code>CCSequence</code> to run a list of finite-timed actions sequentially is still available. **Also, please be aware that asynchronous programming within a PCL will require the additional NuGet package <code>Microsoft.Bcl.Async</code> to be installed.**

### CCGeometryBatch

The new <code>CCGeometryBatch</code> provides a means of efficiently drawing primitive custom vertex/index data. e.g.

<pre>
<code>
void initialiseGeoBatch()
{
	CCGeometryBatch geoBatch = new CCGeometryBatch();
	
	// We will not clear the primitives after creating them
	// This will allow us to keep drawing the same over and over.
	geoBatch.AutoClearInstances = false;
	
	geoBatch.Begin();
	
	// Vertex buffer of length 3. Index buffer of length 3
	var item = geoBatch.CreateGeometryInstance(3,3);
	
	// Set the texture that will be used
	item.GeometryPacket.Texture = someTexture;
	
	// Setup the vertex data
	var vertices = item.GeometryPacket.Vertices;
	vertices[0].Colors = CCColor4B.White;
	vertices[1].Colors = CCColor4B.White;
	vertices[2].Colors = CCColor4B.White;

	vertices[0].TexCoords.U = 0;
	vertices[0].TexCoords.V = 0;
	vertices[1].TexCoords.U = 1;
	vertices[1].TexCoords.V = 0;
	vertices[2].TexCoords.U = 1;
	vertices[2].TexCoords.V = 1;

	vertices[0].Vertices.X = 0;
	vertices[0].Vertices.Y = 0;
	vertices[1].Vertices.X = 10;
	vertices[1].Vertices.Y = 0;

	vertices[2].Vertices.X = 10;
	vertices[2].Vertices.Y = 10;
	
	// Set the index data
	item.GeometryPacket.Indicies = new int[] { 2, 1, 0 };

	// Provide some additional transform
	item.InstanceAttributes.AdditionalTransform = rotation;

	geoBatch.End();
}

protected override void Draw()
{
	base.Draw();
	
	// Alternatively, if geoBatch.AutoClearInstances = true (which is default)
	// and our vertex/index data was dynamically changing we could
	// reload our geometry batch after each draw call
	
	geoBatch.Draw();
}

</code>
</pre>


 
## Fixes and enhancements 
 ---
* [151](https://github.com/mono/CocosSharp/pull/151) Cc tile map polylines and shapes -- thanks to [charliekilian](https://github.com/charliekilian)
* [150](https://github.com/mono/CocosSharp/issues/150) CCTileMap: Add support for reading TileMap with no encoding -- thanks to [charliekilian](https://github.com/charliekilian)
* [147](https://github.com/mono/CocosSharp/issues/147) TileMap cutting off large maps
* [146](https://github.com/mono/CocosSharp/issues/146) CCSplitCols and CCSplitRows GridActions not working.
* [145](https://github.com/mono/CocosSharp/issues/145) Add support for CCGridNode.
* [143](https://github.com/mono/CocosSharp/pull/143) Set SharedDrawManager to null when disposing -- thanks to [hig-ag](https://github.com/hig-ag)
* [142](https://github.com/mono/CocosSharp/issues/142) CCSprite: Setting TextureRectInPixels incorrectly resets ContentSize
* [141](https://github.com/mono/CocosSharp/issues/141) TileMap without encoding causes null reference exception
* [135](https://github.com/mono/CocosSharp/issues/135) Feature request: add async/await support for actions -- thanks to [Krumelur](https://gist.github.com/Krumelur)
* [134](https://github.com/mono/CocosSharp/issues/134) Add test to test loading TileMap from Stream issue 125
* [132](https://github.com/mono/CocosSharp/issues/132) Using CCRotateTo sometimes causes jerking back to previous state
* [131](https://github.com/mono/CocosSharp/issues/131) Scene Management methods are not called in correct order.
* [130](https://github.com/mono/CocosSharp/issues/130) CCRepeat won't end
* [129](https://github.com/mono/CocosSharp/issues/129) CocosSharp iOS test solution not building because of invalid configuration of Lindgren Networking
* [127](https://github.com/mono/CocosSharp/issues/127) Remove old samples from CocosSharp repo
* [126](https://github.com/mono/CocosSharp/issues/126) Drawing or Visiting a CCDrawNode to a CCRenderTexture on first draw outside of Scene Graph displays weird results.
* [125](https://github.com/mono/CocosSharp/issues/125) CCTiledMap - loading TMX file from Stream causes exception.
* [124](https://github.com/mono/CocosSharp/issues/124) CCSprite not working correctly with SamplerState.LinearWrap
* [123](https://github.com/mono/CocosSharp/issues/123) Drawing CCSprite to RenderTexture causes NRE
* [122](https://github.com/mono/CocosSharp/issues/122) CCRenderTexture Issues
* [119](https://github.com/mono/CocosSharp/issues/119) CCDrawNode and CCDrawPrimitives needs a way to draw their own geometry primitives
* [116](https://github.com/mono/CocosSharp/issues/116) CCRenderTexture: fetching Texture not working
* [115](https://github.com/mono/CocosSharp/issues/115) Mac NuGet package targets wrong platform
* [114](https://github.com/mono/CocosSharp/issues/114) Migrate GoneBananas iOS project to Unified
* [112](https://github.com/mono/CocosSharp/issues/112) Problem with GameAppDelegate.ApplicationDidFinishLaunching method
