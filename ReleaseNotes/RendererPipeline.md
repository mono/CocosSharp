
## What is it?

In CocosSharp v.1.5.0.0, we have overhauled the way we perform rendering, with a focus on improved performance. Our implementation was inspired by cocos2d-x v.3's renderer [pipeline](http://www.cocos2d-x.org/docs/manual/framework/native/v3/new-renderer/en), but is by no means a one-for-one port as described below.


## Background

A CocosSharp game comprises of a root <code>CCScene</code> object along with a subsequent chain of <code>CCNode</code> children &mdash; the so-called <em>scene graph</em>. Starting a game initialises a run-loop that periodically calls the scene's <code>Visit</code> method, that in-turn traverses through the entire scene-graph, calling <code>Visit</code> on all node children as in the diagram below.

![Scene traversal](https://raw.githubusercontent.com/mono/CocosSharp/develop/ReleaseNotes/RendererPipelineNotesContent/SceneTraversal.png "Scene traversal")

Previously, rendering was performed within a node's <code>Visit</code> method and thus the number of draw calls grew linearly with the number of nodes in a given scene. This is very much a naive approach to rendering as it doesn't leverage the possibility that the rendering of adjacent nodes can be grouped together into a single draw call. For instance, looking at our sample scene-graph, we notice that our children sprites can be grouped into two, with each group sharing the same underlying texture. Now, drawing a sprite essentially consists of specifying the quad vertices and texture region. Hence, for each group of sprites, if we batch all these quads together we can draw all of them with a <em>single</em> draw call. This was precisely the role of <code>CCSpriteBatchNode</code>. A user would add common sprites as children to the batch node to reduce the number of draw calls. But this class is bundled with a few problems:

* Not every user was aware of this class' existence!

* It broke the ordering of a scene-graph. The implicit pre-condition of using the <code>CCSpriteBatchNode</code> was that it assumed that the child sprites you were adding to it were adjacent to one another, however there was no way to enforce this. For example, suppose we had the following scene-graph

![SpriteBatch scene breaking](https://raw.githubusercontent.com/mono/CocosSharp/develop/ReleaseNotes/RendererPipelineNotesContent/SpriteBatchSceneBreaking.png "SpriteBatch scene breaking")

The problem is that because the grouped sprites will all be drawn together, we have now broken the ordering of the scene-graph. Ultimately <code>CCSpriteBatchNode</code> is not in and of itself truly a renderable object and has no place being part of scene-graph.

* No support for sorting by depth. Virtually all sprites will contain some amount of transparency and so correct ordering when using depth-testing is critically important to avoid any rendering artefacts as seen in the example below

* Clunky code under the hood. Supporting <code>CCSpriteBatchNode</code> made the code-base, particularly, <code>CCSprite</code>, quite clumsy as it had to support this dual role of being in charge of its own rendering or potentially delegating that responsibility to its parent <code>CCSpriteBatchNode</code>.

## Motivation

With the previous background in mind, the primary motivation for the new renderer pipeline was to eliminate the problems and limitations of <code>CCSpriteBatchNode</code>. In particular, we now have the new <code>CCRenderer</code> class that maintains a render queue that is associated with a given scene. When traversing a scene-graph and calling <code>Visit</code> on a give node, rather than performing the drawing then and there, instead a corresponding <code>CCRenderCommand</code> is passed to the renderer. Once the entire scene-graph has been visited, the render queue is then sorted (based on traits of the render command such as order of arrival, depth, material id) and then the render commands are processed as seen below.

![Renderer](https://raw.githubusercontent.com/mono/CocosSharp/develop/ReleaseNotes/RendererPipelineNotesContent/RendererSteps.png "Renderer")

The main benefit is that sprite render commands adjacent to one another in the queue that share the same texture (more precisely the same texture and blending options) will be processed concurrently &mdash; that is, we get automatic batching on the fly <em>without</em> the need for <code>CCSpriteBatchNode</code>! Moreover, if a user is making use of depth-testing (see upcoming section), we further get correct depth-ordering as well!

Keep in mind that sprites don't necessarily have to be exactly the same to leverage this benefit. As long as they share the same texture, then automatic batching will be performed. This is particularly true when using sprite-sheets.

## How to use it?

Thankfully, in the majority of cases, making use of the new renderer pipeline should be almost automatic. If you previously haven't used <code>CCSpriteBatchNode</code> in your code, then these new changes should work right out of the box. If you have made use of <code>CCSpriteBatchNode</code>, then migration should be simple &mdash; just remove usage of it! For example,

<pre>
<code>
CCNode parent;
CCSpriteBatchNode batchNode;
CCSprite sprite;

// Instantiate nodes accordingly

// Previously

parent.AddChild(batchNode);
batchNode.AddChild(sprite);

// Now

parent.AddChild(sprite);
</code>
</pre>

Note that in CocosSharp v1.5.0.0, <code>CCSpriteBatchNode</code> has been marked obsolete, but to avoid confusion will be removed entirely in future releases.


## Adding custom drawing

As drawing is not performed immediately, the virtual method <code>CCNode:Draw</code> is no longer relevant. Instead, if you have subclassed <code>CCNode</code> and overridden <code>Draw</code> to provide some custom drawing logic, then you can migrate to use the renderer by making use of the <code>CCCustomCommand</code> render command. For example,

<pre>
<code>

CCCustomCommand renderCommand;

public MyNode()
{
	renderCommand = new CCCustomCommand(RenderNode);
	
	// Other initialisation
}

// Called when visiting the node
protected override void VisitRenderer(ref CCAffineTransform worldTransform)
{
    base.VisitRenderer(ref worldTransform);
	
	// Give the render command the global depth for depth-ordering (if used)
	// i.e. if Window.IsUseDepthTesting = true
	renderCommand.GlobalDepth = worldTransform.Tz;
	
	// Provide the world transform which will be set when processing render command
    renderCommand.WorldTransform = worldTransform;

	Renderer.AddCommand(renderCommand);
}

// Called by renderer when it's time to process command
// All drawing commands will be relative to the world transform passed in VisitRenderer
void RenderNode()
{
	// Perform custom drawing
}
</code>
</pre>

__Warning:__ It should be extremely rare that you will need to provide your own custom drawing logic. All nodes are linked up the renderer, so by adding them to the scene-graph they will automatically have corresponding render commands associated with them without any user intervention. 

Currently, the only valid use case for providing custom drawing is if a user is making use of <code>CCDrawingPrimitives</code> that provides a static interface for drawing primitive shapes. However, this class has since been superseded by <code>CCDrawNode</code> which is derived from <code>CCNode</code> and hence connected to the renderer. While <code>CCDrawingPrimitives</code> is currently marked as obsolete, future releases of CocosSharp will omit this class entirely and so we recommend users migrate over to <code>CCDrawNode</code> as soon as possible.


## Visiting in a render texture

Users should also be aware of the new <code>CCNode</code> method <code>public virtual void Visit(ref CCAffineTransform parentWorldTransform)</code>. This method is used internally when traversing the scene-graph but an added benefit is that it gives the user a lot more flexibility when attempting to render a node into a <code>CCRenderTexture</code> by supplying an alternate transform. For example,

<pre>
<code>
	
CCRenderTexture renderTexture;
CCNode parentNode;
CCNode childNode;

// Initialise render texture and node

parentNode.AddChild(childNode);

CCAffineTransform transform = parentNode.AffineWorldTransform;

renderTexture.Begin();
// Draw into render texture
// Use the true parent's world transform
// This corresponds to the exact same transform used when traversing the scene-graph
childNode.Visit(ref transform);
renderTexture.End();


// Alternatively, our render texture dimensions may not correspond with the entire scene
// and we may wish to supply an alternate parent transform 

// Create a transform that simply translates by 20
CCAffineTransform transform2 = CCAffineTransform.Identity;
transform2.Tx = 20.0f;

renderTexture.Begin();
childNode.Visit(ref transform2);
renderTexture.End();

</code>
</pre>

In this way, render textures now support a software-side <em>instancing</em> of nodes.

__Warning:__ There is a slight nuance that needs to be highlighted. The transform provided in <code>Visit(ref transform)</code> is the <em>parent</em> world transform, not the node's transform itself. Internally, this method will concatenate the passed in transform with the node's local transform (based on the node's position, scale, rotation etc.) to calculate the node's world transform.

Remember, the <em>local</em> transform is relative to a node's parent, while the <em>world</em> transform is relative to the entire layer.


## Depth testing and changes to z-ordering

Finally, we have overhauled the usage of <code>CCNode</code> z-ordering to more cleanly integrate with our renderer's sorting of render commands. Previously, <code>CCNode</code> contained a suite of z-ordering properties - namely, <code>ZOrder</code>, <code>LocalZOrder</code>, <code>GlobalZOrder</code> along with the internal <code>COrderOfArrival</code>. Aside from adding confusion, the new renderer pipeline has made all these different flavors of ordering obsolete. Instead, we know have the single property <code>int:ZOrder</code>, that simply orders node siblings by ascending order. In this way, the sibling with lowest <code>ZOrder</code> will be processed before all others. 

__Warning:__ Remember, <code>ZOrder</code> has nothing to do with the geometric depth of a node. It's simply an ordering amongst siblings. If you want to actually alter the z-position of node, then use the <code>VertexZ</code> property.

__Warning:__ The one caveat when ordering is when we have a collection of siblings with non-uniform depth (i.e. different <code>VertexZ</code> property values) and a user is making use of depth-testing. For example, consider the following scene

![ZOrdering vs depth ordering](https://raw.githubusercontent.com/mono/CocosSharp/develop/ReleaseNotes/RendererPipelineNotesContent/ZOrderVsDepthOrder.png "ZOrdering vs depth ordering")

At this point, there is a disagreement between the depth and z-ordering, so the Renderer has to make the choice. In particular, for our implementation, __depth-ordering is prioritised over z-ordering__. The motivation is that a geometric ordering is the more appropriate choice to dynamically alter the render ordering of nodes that may be moving around in world. Below is a video showcasing this idea

<script src="http://vjs.zencdn.net/4.0/video.js"></script>

<video id="depth_ordering" class="video-js vjs-default-skin" controls
preload="auto" width="683" height="384"
data-setup="{}">
<source src="https://raw.githubusercontent.com/mono/CocosSharp/develop/ReleaseNotes/RendererPipelineNotesContent/DynamicDepthOrdering.mp4" type='video/mp4'>
</video>


