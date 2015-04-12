# CocosSharp v1.4.0.0 release notes

## Breaking changes 
 ---

* Changes to CCLog custom logging interface as described [here](https://github.com/mono/CocosSharp/wiki/Logging)
* Please also see details of Unified labels for future breaking changes

## Key new features
 ---
### Unified labels

The previous support for labels was disjoint and offered inconsistent behaviour across different formats/platforms. Motivated by a desire to do away with these issues, we now offer a single point of control for creating bitmap, sprite and system fonts -- namely, <code>CCLabel</code>, where the desired format is specified within the constructor. For example, 

<pre>
<code>
// CCLabel Sprite Font
var label1 = new CCLabel("Hello SpriteFont", "fonts/arial", 26, CCLabelFormat.SpriteFont);

// CCLabel Bitmap Font - No need to pass a CCLabelFormat because the default for this constructor is BitmapFont
var label2 = new CCLabel("Hello Bitmap Font", "fonts/bitmapFontTest3.fnt");

// CCLabel using the system font Arial
var label3 = new CCLabel("Hello System Font", "Arial", 20, CCLabelFormat.SystemFont);

// CCLabel using the MorrisRoman-Black.ttf font included as content in the fonts folder
var label4 = new CCLabel("Hello MorrisRoman-Black", "fonts/MorrisRoman-Black.ttf", size, CCLabelFormat.SystemFont)
</code>
</pre>

Note, that while the now obsolete <code>CCLabelBMFont</code> and <code>CCLabelTtf</code> classes remain in this release, to avoid confusion, we will aim to remove these classes entirely in subsequent releases. Thus it is recommended that users migrate their existing code-base to use unified labels as soon as possible. 

Further in-depth details on the features of the new unified <code>CCLabel</code> implementation can be found [here](https://github.com/mono/CocosSharp/wiki/Labels).

Also, a special thanks to René Ruppert for testing and putting the new labels through their paces!

 ---
### Beefed up tile map support

Some great new features for our [Tiled](http://www.mapeditor.org) tile map support including

* support for CSV and xml encoded tile maps
* support for TMX-specified tile animations
* support for staggered isometric tile map types
* a more robust and faster tile-map renderer (tile visibility culling and support for very large tile maps)



## Fixes and enhancements 
 ---
* [172](https://github.com/mono/CocosSharp/issues/172) Windows Phone 8.1 support with Nugets
* [171](https://github.com/mono/CocosSharp/issues/171) CCLog could be made simpler by not having an interface
* [170](https://github.com/mono/CocosSharp/issues/170) CCTileMap: Add support for reading TileMap with CSV encoding
* [169](https://github.com/mono/CocosSharp/issues/169) CCTileMapLayer: Add support for animated tiles specified in TMX
* [168](https://github.com/mono/CocosSharp/issues/168) CCFadeOutBLTiles leaks memory
* [166](https://github.com/mono/CocosSharp/issues/166) CCLabel leaks memory
* [165](https://github.com/mono/CocosSharp/issues/165) CCTileMap.TilePropertiesForGID should not throw a KeyNotFoundException when passing a GID - it should return null
* [158](https://github.com/mono/CocosSharp/issues/158) CCEmitterMode not longer able to be set but also no way to set it for ParticleSystems on creation.
* [156](https://github.com/mono/CocosSharp/pull/156) Fixed a bug where TouchesEnabled does not work correctly. --- thanks to [Tonisson](https://github.com/Tonisson)
* [155](https://github.com/mono/CocosSharp/issues/155) Tiled map with object layer causes index out of range exception on ios
* [154](https://github.com/mono/CocosSharp/issues/154) Enhance CCDrawNode with the ability to draw Ellipses
* [152](https://github.com/mono/CocosSharp/issues/152) CCTileMap incorrectly uses the first tileset for all layers
* [151](https://github.com/mono/CocosSharp/pull/151) Cc tile map polylines and shapes --- thanks to [charliekilian](https://github.com/charliekilian)
* [149](https://github.com/mono/CocosSharp/issues/149) iOS: System Japanese font character width/x-advance is too large
* [148](https://github.com/mono/CocosSharp/issues/148) CCTileMap: Elegantly handle drawing of tile map with vertices > 65535 --- thanks to [charliekilian](https://github.com/charliekilian)
* [138](https://github.com/mono/CocosSharp/issues/138) CommonHeight property exposed on Labels
* [137](https://github.com/mono/CocosSharp/issues/137) Attribute of CCLabel that tells me the exact rendered size of my label
* [136](https://github.com/mono/CocosSharp/issues/136) TestFlight won't accept the binary without the 64 bit support.
* [128](https://github.com/mono/CocosSharp/issues/128) CCMenuItem doesn't trigger Action target (for example void OnClick(T obj) { ... }) in arm64 architecture 
* [92](https://github.com/mono/CocosSharp/issues/92) CCTileMapLayer :  Fix Hexagonal tile map transformation
* [76](https://github.com/mono/CocosSharp/issues/76) CCLabel: border artefacts when initially rendering characters
* [75](https://github.com/mono/CocosSharp/issues/75) CCLabel: robust handling of font filenames
* [73](https://github.com/mono/CocosSharp/issues/73) Allow more flexibility in referencing sprite font XNBs
* [37](https://github.com/mono/CocosSharp/issues/37) Unified labels
* [13](https://github.com/mono/CocosSharp/issues/13) Fast tile map rendering with culling support
* [9](https://github.com/mono/CocosSharp/issues/9) Add support in TMX parser for isometric(staggered) map types
