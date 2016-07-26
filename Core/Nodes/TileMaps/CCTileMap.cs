using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;

namespace CocosSharp
{
    public class CCTileMap : CCLayerColor
    {
        #region Properties

        public static bool DefaultHalfTexelOffset { get; set; }

        public CCTileMapType Type { get; private set; }
        public CCTileMapCoordinates MapDimensions { get; private set; }
        public CCSize TileTexelSize { get; private set; }
        public CCNode TileLayersContainer { get; private set; }

        CCTileMapInfo MapInfo { get; set; }
        List<CCTileMapObjectGroup> ObjectGroups { get; set; }
        Dictionary<string, string> MapProperties { get; set; }
        Dictionary<short, Dictionary<string, string>> TileProperties { get; set; }

        public bool Antialiased
        {
            set
            {
                foreach (CCNode child in TileLayersContainer.Children)
                {
                    var layer = child as CCTileMapLayer;
                    if (layer != null)
                    {
                        layer.Antialiased = value;
                    }
                }
            }
        }

        public bool HalfTexelOffset
        {
            set
            {
                foreach (CCNode child in TileLayersContainer.Children)
                {
                    var layer = child as CCTileMapLayer;
                    if (layer != null)
                    {
                        layer.HalfTexelOffset = value;
                    }
                }
            }
        }

        #endregion Properties


        #region Constructors

        static CCTileMap()
        {
            DefaultHalfTexelOffset = true;
        }

        public CCTileMap(CCTileMapInfo mapInfo)
            : base(CCCameraProjection.Projection2D)
        {
            Type = mapInfo.MapType;
            MapDimensions = mapInfo.MapDimensions;
            MapInfo = mapInfo;
            TileTexelSize = mapInfo.TileTexelSize;

            ObjectGroups = mapInfo.ObjectGroups;
            MapProperties = mapInfo.MapProperties;
            TileProperties = mapInfo.TileProperties;

            TileLayersContainer
                = new CCNode(MapDimensions.Size * TileTexelSize * CCTileMapLayer.DefaultTexelToContentSizeRatios);

            AddChild(TileLayersContainer);

            int idx = 0;

            List<CCTileLayerInfo> layers = mapInfo.Layers;
            if (layers != null)
            {
                foreach (CCTileLayerInfo layerInfo in layers)
                {
                    if (layerInfo.Visible)
                    {
                        CCTileSetInfo[] tilesets = TilesetsForLayer(layerInfo);
                        CCTileMapLayer child = new CCTileMapLayer(tilesets, layerInfo, mapInfo);
                        TileLayersContainer.AddChild(child, idx, idx);

                        idx++;
                    }
                }
            }
        }

        public CCTileMap(StreamReader tmxFile)
            : this(new CCTileMapInfo(tmxFile))
        {
        }

        public CCTileMap(string tmxFile)
            : this(new CCTileMapInfo(tmxFile))
        {
        }

        CCTileSetInfo[] TilesetsForLayer(CCTileLayerInfo layerInfo)
        {
            CCTileMapCoordinates size = layerInfo.LayerDimensions;
            List<CCTileSetInfo> tilesets = MapInfo.Tilesets;
            List<CCTileSetInfo> results = new List<CCTileSetInfo>();
            int numOfTiles = size.Row * size.Column;

            if (tilesets != null)
            {
                for (int tilesetIdx = 0; tilesetIdx < tilesets.Count; tilesetIdx++)
                {
                    CCTileSetInfo tileset = tilesets[tilesetIdx];
                    short tilesetLastGid = (short)(tilesetIdx < tilesets.Count - 1 ? tilesets[tilesetIdx + 1].FirstGid - 1 : short.MaxValue);
                    for (uint tileIdx = 0; tileIdx < numOfTiles; tileIdx++)
                    {
                        CCTileGidAndFlags gidAndFlags = layerInfo.TileGIDAndFlags[tileIdx];
                        if (gidAndFlags.Gid != 0 && gidAndFlags.Gid >= tileset.FirstGid && gidAndFlags.Gid <= tilesetLastGid)
                        {
                            results.Add(tileset);
                            break;
                        }
                    }
                }
            }
            if (results.Count > 0)
                return results.ToArray();
            else
            {
                CCLog.Log("CocosSharp: Warning: CCTileMapLayer: TileMap layer '{0}' has no tiles", layerInfo.Name);
                return null;
            }
        }

        #endregion Constructors


        #region Fetching tile map data

        public CCTileMapLayer LayerNamed(string layerName)
        {
            foreach (CCNode child in TileLayersContainer.Children.Elements)
            {
                var layer = child as CCTileMapLayer;
                if (layer != null && layer.LayerName == layerName)
                {
                    return layer;
                }
            }
            return null;
        }

        public CCTileMapObjectGroup ObjectGroupNamed(string groupName)
        {
            if (ObjectGroups != null)
            {
                foreach (CCTileMapObjectGroup objectGroup in ObjectGroups)
                {
                    if (objectGroup.GroupName == groupName)
                    {
                        return objectGroup;
                    }
                }
            }

            return null;
        }

        public string MapPropertyNamed(string propertyName)
        {
            return MapProperties[propertyName];
        }

        public Dictionary<string, string> TilePropertiesForGID(short tileGid)
        {
            Dictionary<string, string> propertiesDict = null;

            TileProperties.TryGetValue(tileGid, out propertiesDict);

            return propertiesDict;
        }

        #endregion Fetching tile map data
    }
}