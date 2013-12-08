using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;

namespace Cocos2D
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
        #region properties

        protected int m_nMapOrientation;
        protected List<CCTMXObjectGroup> m_pObjectGroups;
        protected Dictionary<string, string> m_pProperties;
        protected CCSize m_tMapSize;
        public CCTMXMapInfo MapInfo { get; set; }

        protected CCSize m_tTileSize;

        /// <summary>
        /// the map's size property measured in tiles
        /// </summary>
        public CCSize MapSize
        {
            get { return m_tMapSize; }
            set { m_tMapSize = value; }
        }

        /// <summary>
        /// the tiles's size property measured in pixels
        /// </summary>
        public CCSize TileSize
        {
            get { return m_tTileSize; }
            set { m_tTileSize = value; }
        }

        /// <summary>
        /// map orientation
        /// </summary>
        public int MapOrientation
        {
            get { return m_nMapOrientation; }
            set { m_nMapOrientation = value; }
        }

        /// <summary>
        /// object groups
        /// </summary>
        public List<CCTMXObjectGroup> ObjectGroups
        {
            get { return m_pObjectGroups; }
            set { m_pObjectGroups = value; }
        }

        /// <summary>
        /// properties
        /// </summary>
        public Dictionary<string, string> Properties
        {
            get { return m_pProperties; }
            set { m_pProperties = value; }
        }

        #endregion

        #region public

        /// <summary>
        /// Construct the Tiled map from the given TMX file, which is assumed to be a content managed file.
        /// </summary>
        /// <param name="tmxFile"></param>
        public CCTMXTiledMap(string tmxFile)
        {
            InitWithTmxFile(tmxFile);
        }

        /// <summary>
        /// Construct the Tiled map from the given stream containing the contents of the TMX file.
        /// </summary>
        /// <param name="tmxFile"></param>
        public CCTMXTiledMap(StreamReader tmxFile)
        {
            CCTMXMapInfo mapInfo = new CCTMXMapInfo(tmxFile);
            ContentSize = CCSize.Zero;
            BuildWithMapInfo(mapInfo);
        }

        /// <summary>
        /// Constructs the Tiled map from the map information that you provide.
        /// </summary>
        /// <param name="mapInfo"></param>
        public CCTMXTiledMap(CCTMXMapInfo mapInfo)
        {
            ContentSize = CCSize.Zero;
            BuildWithMapInfo(mapInfo);
        }

        /// <summary>
        /// creates a TMX Tiled Map with a TMX file.
        /// </summary>
        [Obsolete("Please use the ctor instead of the self factory pattern.")]
        public static CCTMXTiledMap Create(string tmxFile)
        {
            var pRet = new CCTMXTiledMap(tmxFile);
            return pRet;
        }

        /// <summary>
        /// initializes a TMX Tiled Map with a TMX file
        /// </summary>
        protected virtual bool InitWithTmxFile(string tmxFile)
        {
            Debug.Assert(!String.IsNullOrEmpty(tmxFile), "TMXTiledMap: tmx file should not be null");

            ContentSize = CCSize.Zero;

            CCTMXMapInfo mapInfo = new CCTMXMapInfo(tmxFile);

            if (mapInfo == null)
            {
                return false;
            }

            Debug.Assert(mapInfo.Tilesets.Count != 0, "TMXTiledMap: Map not found. Please check the filename.");

            BuildWithMapInfo(mapInfo);
            return true;
        }

        private void BuildWithMapInfo(CCTMXMapInfo mapInfo)
        {
            MapInfo = mapInfo;
            m_tMapSize = mapInfo.MapSize;
            m_tTileSize = mapInfo.TileSize;
            m_nMapOrientation = mapInfo.Orientation;
            ObjectGroups = mapInfo.ObjectGroups;
            Properties = mapInfo.Properties;
            m_pTileProperties = mapInfo.TileProperties;

            int idx = 0;

            //Layers
            List<CCTMXLayerInfo> layers = mapInfo.Layers;
            if (layers != null && layers.Count > 0)
            {
                for (int i = 0; i < layers.Count; i++)
                {
                    CCTMXLayerInfo layerInfo = layers[i];
                    if (layerInfo != null && layerInfo.Visible)
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

        /// <summary>
        /// return the TMXLayer for the specific layer
        /// </summary>
        public CCTMXLayer LayerNamed(string layerName)
        {
            for (int i = 0; i < m_pChildren.count; i++)
            {
                var layer = m_pChildren.Elements[i] as CCTMXLayer;
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
            if (m_pObjectGroups != null && m_pObjectGroups.Count > 0)
            {
                for (int i = 0; i < m_pObjectGroups.Count; i++)
                {
                    CCTMXObjectGroup objectGroup = m_pObjectGroups[i];
                    if (objectGroup != null && objectGroup.GroupName == groupName)
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
            return m_pProperties[propertyName];
        }

        /// <summary>
        /// return properties dictionary for tile GID
        /// </summary>
        public Dictionary<string, string> PropertiesForGID(uint GID)
        {
            return m_pTileProperties[GID];
        }

        #endregion

        #region private

        private CCTMXLayer ParseLayer(CCTMXLayerInfo layerInfo, CCTMXMapInfo mapInfo)
        {
            CCTMXTilesetInfo tileset = tilesetForLayer(layerInfo, mapInfo);
            CCTMXLayer layer = new CCTMXLayer(tileset, layerInfo, mapInfo);

            // tell the layerinfo to release the ownership of the tiles map.
            layerInfo.OwnTiles = false;
            layer.SetupTiles();

            return layer;
        }

        private CCTMXTilesetInfo tilesetForLayer(CCTMXLayerInfo layerInfo, CCTMXMapInfo mapInfo)
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

                                // gid are stored in little endian.
                                // if host is big endian, then swap
                                //if( o == CFByteOrderBigEndian )
                                //	gid = CFSwapInt32( gid );
                                /* We support little endian.*/

                                // XXX: gid == 0 --> empty tile
                                if (gid != 0)
                                {
                                    // Optimization: quick return
                                    // if the layer is invalid (more than 1 tileset per layer) an CCAssert will be thrown later
                                    if ((gid & CCTMXTileFlags.FlippedMask) >= tileset.m_uFirstGid)
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
            CCLog.Log("cocos2d: Warning: TMX Layer '{0}' has no tiles", layerInfo.Name);
            return null;
        }

        #endregion

        #region protected

        //! tile properties
        protected Dictionary<uint, Dictionary<string, string>> m_pTileProperties;

        #endregion
    }
}