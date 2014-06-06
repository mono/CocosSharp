using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;

namespace CocosSharp
{
    /** @brief CCTMXTiledMap knows how to parse and render a TMX map.

It adds support for the TMX tiled map format used by http://www.mapeditor.org
It supports isometric, hexagonal and orthogonal tiles.
It also supports object groups, objects, and properties.

Features:
- Each tile will be treated as an CCSprite
- The sprites are created on demand. They will be created only when you call "layer->tileAt(position)"
- Each tile can be rotated / moved / scaled / tinted / "opacitied", since each tile is a CCSprite
- Tiles can be added/removed in runtime
- The z-order of the tiles can be modified in runtime
- Each tile has an anchorPoint of (0,0)
- The anchorPoint of the TMXTileMap is (0,0)
- The TMX layers will be added as a child
- The TMX layers will be aliased by default
- The tileset image will be loaded using the CCTextureCache
- Each tile will have a unique tag
- Each tile will have a unique z value. top-left: z=1, bottom-right: z=max z
- Each object group will be treated as an CCMutableArray
- Object class which will contain all the properties in a dictionary
- Properties can be assigned to the Map, Layer, Object Group, and Object

Limitations:
- It only supports one tileset per layer.
- Embeded images are not supported
- It only supports the XML format (the JSON format is not supported)

Technical description:
Each layer is created using an CCTMXLayer (subclass of CCSpriteBatchNode). If you have 5 layers, then 5 CCTMXLayer will be created,
unless the layer visibility is off. In that case, the layer won't be created at all.
You can obtain the layers (CCTMXLayer objects) at runtime by:
- map->getChildByTag(tag_number);  // 0=1st layer, 1=2nd layer, 2=3rd layer, etc...
- map->layerNamed(name_of_the_layer);

Each object group is created using a CCTMXObjectGroup which is a subclass of CCMutableArray.
You can obtain the object groups at runtime by:
- map->objectGroupNamed(name_of_the_object_group);

Each object is a CCTMXObject.

Each property is stored as a key-value pair in an CCMutableDictionary.
You can obtain the properties at runtime by:

map->propertyNamed(name_of_the_property);
layer->propertyNamed(name_of_the_property);
objectGroup->propertyNamed(name_of_the_property);
object->propertyNamed(name_of_the_property);

@since v0.8.1
*/

    public class CCTMXTiledMap : CCNode
    {
        #region Properties

        public int MapOrientation { get; private set; }
        public CCSize MapSize { get; private set; }             // measured in tiles
        public CCSize TileSize { get; private set; }            // measured in pixels
        CCTMXMapInfo MapInfo { get; set; }

        List<CCTMXObjectGroup> ObjectGroups { get; set; }
        Dictionary<string, string> Properties { get; set; }
        Dictionary<uint, Dictionary<string, string>> TileProperties { get; set; }

        #endregion Properties



        #region Constructors

        public CCTMXTiledMap(CCTMXMapInfo mapInfo)
        {
            ContentSize = CCSize.Zero;
            BuildWithMapInfo(mapInfo);
        }

        public CCTMXTiledMap(StreamReader tmxFile) 
            : this(new CCTMXMapInfo(tmxFile))
        {
        }

        // Construct the Tiled map from the given TMX file, which is assumed to be a content managed file.
        public CCTMXTiledMap(string tmxFile) 
            : this(new CCTMXMapInfo(tmxFile))
        {
        }

        // Methods solely used for construction

        void BuildWithMapInfo(CCTMXMapInfo mapInfo)
        {
            MapInfo = mapInfo;
            MapSize = mapInfo.MapSize;
            TileSize = mapInfo.TileSize;
            MapOrientation = mapInfo.Orientation;
            ObjectGroups = mapInfo.ObjectGroups;
            Properties = mapInfo.Properties;
            TileProperties = mapInfo.TileProperties;

            int idx = 0;

            List<CCTMXLayerInfo> layers = mapInfo.Layers;
            if (layers != null && layers.Count > 0)
            {
                foreach(CCTMXLayerInfo layerInfo in layers)
                {
                    if (layerInfo.Visible)
                    {
                        CCTMXLayer child = ParseLayer(layerInfo, mapInfo);
                        AddChild(child, idx, idx);

                        // update content size with the max size
                        CCSize childSize = child.ContentSize;
                        CCSize currentSize = ContentSize;
                        currentSize.Width = Math.Max(currentSize.Width, childSize.Width);
                        currentSize.Height = Math.Max(currentSize.Height, childSize.Height);
                        ContentSize = currentSize;

                        idx++;
                    }
                }
            }
        }

        CCTMXLayer ParseLayer(CCTMXLayerInfo layerInfo, CCTMXMapInfo mapInfo)
        {
            CCTMXTilesetInfo tileset = TilesetForLayer(layerInfo, mapInfo);
            CCTMXLayer layer = new CCTMXLayer(tileset, layerInfo, mapInfo);

            // tell the layerinfo to release the ownership of the tiles map.
            layerInfo.OwnTiles = false;
            layer.SetupTiles();

            return layer;
        }

        CCTMXTilesetInfo TilesetForLayer(CCTMXLayerInfo layerInfo, CCTMXMapInfo mapInfo)
        {
            CCSize size = layerInfo.LayerSize;
            List<CCTMXTilesetInfo> tilesets = mapInfo.Tilesets;

            if (tilesets != null && tilesets.Count > 0)
            {
                for (int i = tilesets.Count - 1; i >= 0; i--)
                {
                    CCTMXTilesetInfo tileset = tilesets[i];
                    if (tileset != null)
                    {
                        for (int y = 0; y < size.Height; y++)
                        {
                            for (int x = 0; x < size.Width; x++)
                            {
                                var pos = (int) (x + size.Width * y);
                                uint gid = layerInfo.Tiles[pos];

                                // XXX: gid == 0 --> empty tile
                                if (gid != 0)
                                {
                                    // Optimization: quick return
                                    // if the layer is invalid (more than 1 tileset per layer) an CCAssert will be thrown later
                                    if ((gid & CCTMXTileFlags.FlippedMask) >= tileset.FirstGid)
                                    {
                                        return tileset;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            // If all the tiles are 0, return empty tileset
            CCLog.Log("CocosSharp: Warning: TMX Layer '{0}' has no tiles", layerInfo.Name);
            return null;
        }

        #endregion Constructors

        /// <summary>
        /// return the TMXLayer for the specific layer
        /// </summary>
        public CCTMXLayer LayerNamed(string layerName)
        {
            foreach(CCNode child in Children.Elements)
            {
                var layer = child as CCTMXLayer;
                if (layer != null && layer.LayerName == layerName)
                {
                    return layer;
                }
            }
            return null;
        }

        /// <summary>
        /// return the TMXObjectGroup for the secific group 
        /// </summary>
        public CCTMXObjectGroup ObjectGroupNamed(string groupName)
        {
            if (ObjectGroups != null && ObjectGroups.Count > 0)
            {
                foreach(CCTMXObjectGroup objectGroup in ObjectGroups)
                {
                    if (objectGroup.GroupName == groupName)
                    {
                        return objectGroup;
                    }
                }
            }

            // objectGroup not found
            return null;
        }

        /// <summary>
        ///  return the value for the specific property name
        /// </summary>
        public string PropertyNamed(string propertyName)
        {
            return Properties[propertyName];
        }

        /// <summary>
        /// return properties dictionary for tile GID
        /// </summary>
        public Dictionary<string, string> PropertiesForGID(uint GID)
        {
            return TileProperties[GID];
        }
    }
}