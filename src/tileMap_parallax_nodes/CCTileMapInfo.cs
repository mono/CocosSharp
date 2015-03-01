using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace CocosSharp
{
    public class CCTileMapInfo : ICCSAXDelegator
    {
        const string MapElementVersion = "version";
        const string MapElementMapType = "orientation";
        const string MapElementNumOfColumns = "width";
        const string MapElementNumOfRows = "height";
        const string MapElementTileTexelWidth = "tilewidth";
        const string MapElementTileTexelHeight = "tileheight";

        const string TilesetElementFileSource = "source";
        const string TilesetElementFirstGid = "firstgid";
        const string TilesetElementName = "name";
        const string TilesetElementTileSpacing = "spacing";
        const string TilesetElementBorderSize = "margin";
        const string TilesetElementTexelWidth = "tilewidth";
        const string TilesetElementTexelHeight = "tileheight";

        const string TileElementId = "id";
        const string TileElementGidAndFlags = "gid";

        const string LayerElementName = "name";
        const string LayerElementNumOfColumns = "width";
        const string LayerElementNumOfRows = "height";
        const string LayerElementVisible = "visible";
        const string LayerElementOpacity = "opacity";
        const string LayerElementXOffset = "x";
        const string LayerElementYOffset = "y";

        const string ObjectGrpElementName = "name";
        const string ObjectGrpElementXOffset = "x";
        const string ObjectGrpElementYOffset = "y";

        const string ImageElementTilesheetName = "source";

        const string DataElementEncoding = "encoding";
        const string DataElementCompression = "compression";
        const string DataElementBase64 = "base64";
        const string DataElementGzip = "gzip";
        const string DataElementZlib = "zlib";

        const string ObjectElementName = "name"; 
        const string ObjectElementType = "type";
        const string ObjectElementWidth = "width";
        const string ObjectElementHeight = "height";
        const string ObjectElementGid = "gid";
        const string ObjectElementXPosition = "x";
        const string ObjectElementYPosition = "y";

        const string PropertyElementName = "name";
        const string PropertyElementValue = "value";

        static readonly Dictionary<string, CCTileMapType> mapTypeKeys;

        // ivars

        bool storingCharacters;
        CCTileDataCompressionType tileDataCompressionType;
        readonly Dictionary<string, Tuple<Action,Action>> mapFileElementActions;

        // Temp vars used during parsing

        CCTileMapProperty currentParentElement;
        short currentFirstGID;
        uint currentXmlTileIndex;
        byte[] currentString;
        Dictionary<string,string> currentAttributeDict;


        #region Properties

        public short ParentGID { get; private set; }
        public string TileMapFileName { get; private set; }

        public CCTileMapType MapType { get; private set; }
        public CCTileMapCoordinates MapDimensions { get; private set; }
        public CCSize TileTexelSize { get; private set; }

        internal List<CCTileLayerInfo> Layers { get; private set; }
        internal List<CCTileSetInfo> Tilesets { get; private set; }
        internal List<CCTileMapObjectGroup> ObjectGroups { get; private set; }
        internal Dictionary<string, string> MapProperties { get; private set; }
        internal Dictionary<short, Dictionary<string, string>> TileProperties { get; private set; }

        #endregion Properties


        #region Constructors

        static CCTileMapInfo()
        {
            mapTypeKeys = new Dictionary<string, CCTileMapType>
            {
                { "orthogonal", CCTileMapType.Ortho },
                { "isometric", CCTileMapType.Iso },
                { "hexagonal", CCTileMapType.Hex },
                { "staggered", CCTileMapType.Staggered }
            };
        }

        public CCTileMapInfo(string tileMapFile) : this()
        {
            TileMapFileName = CCFileUtils.FullPathFromRelativePath(tileMapFile);
            ParseXmlFile(TileMapFileName);
        }

        public CCTileMapInfo(StreamReader stream) : this()
        {
            string data = stream.ReadToEnd();

            var parser = new CCSAXParser();

            parser.SetDelegator(this);

            parser.ParseContent(data);
        }

        CCTileMapInfo()
        {
            Tilesets = new List<CCTileSetInfo>();
            Layers = new List<CCTileLayerInfo>();
            ObjectGroups = new List<CCTileMapObjectGroup>(4);
            MapProperties = new Dictionary<string, string>();
            TileProperties = new Dictionary<short, Dictionary<string, string>>();
            tileDataCompressionType = CCTileDataCompressionType.None;
            currentParentElement = CCTileMapProperty.None;
            currentFirstGID = 0;

            // Associating xml elements of tmx file with parse actions
            mapFileElementActions = new Dictionary<string, Tuple<Action,Action>>
            {
                { "map", Tuple.Create<Action,Action>(ParseMapElement, ParseMapEndElement) },
                { "tileset", Tuple.Create<Action,Action>(ParseTilesetElement, ParseTilesetEndElement) },
                { "tile", Tuple.Create<Action,Action>(ParseTileElement, ParseTileEndElement) },
                { "layer", Tuple.Create<Action,Action>(ParseLayerElement, ParseLayerEndElement) },
                { "objectgroup", Tuple.Create<Action,Action>(ParseObjectGroupElement, ParseObjectGroupEndElement) },
                { "image", Tuple.Create<Action,Action>(ParseImageElement, ParseImageEndElement) },
                { "data", Tuple.Create<Action,Action>(ParseDataElement, ParseDataEndElement) },
                { "object", Tuple.Create<Action,Action>(ParseObjectElement, ParseObjectEndElement) },
                { "property", Tuple.Create<Action,Action>(ParsePropertyElement, ParsePropertyEndElement) },
                { "polygon", Tuple.Create<Action,Action>(ParsePolygonElement, ParsePolygonEndElement) },
                { "polyline", Tuple.Create<Action,Action>(ParsePolylineElement, ParsePolylineEndElement) }
            };
        }

        // Initalises parsing of an XML file, either a tmx (Map) file or tsx (Tileset) file
        bool ParseXmlFile(string xmlFilename)
        {
            var parser = new CCSAXParser();

            parser.SetDelegator(this);

            return parser.ParseContentFile(xmlFilename);
        }

        #endregion Constructors


        #region ICCSAXDelegator methods

        public void StartElement(object ctx, string elementName, string[] atts)
        {
            currentAttributeDict = new Dictionary<string, string>();

            if (atts != null && atts[0] != null)
            {
                for (int i = 0; i + 1 < atts.Length; i += 2)
                {
                    string key = atts[i];
                    string value = atts[i + 1];
                    currentAttributeDict.Add(key, value);
                }
            }

            // Run the action that corresponds to the current element string found in xml file
            Tuple<Action,Action> elementParseActions = null;
            if (mapFileElementActions.TryGetValue(elementName, out elementParseActions))
            {
                // Item 1 of tuple corresponds to begin element action
                elementParseActions.Item1();
            }

            currentAttributeDict = null;
        }


        public void EndElement(object ctx, string elementName)
        {
            // Run the action that corresponds to the current element string found in xml file
            Tuple<Action,Action> elementParseActions = null;
            if (mapFileElementActions.TryGetValue(elementName, out elementParseActions))
            {
                // Item 2 of tuple corresponds to end element action
                elementParseActions.Item2();
            }
        }


        public void TextHandler(object ctx, byte[] ch, int len)
        {
            if (storingCharacters)
            {
                currentString = ch;
            }
        }

        #endregion ICCSAXDelegator methods


        #region Parse begin element methods

        void ParseMapElement()
        {
            // Version
            float version = CCUtils.CCParseFloat(currentAttributeDict[MapElementVersion]);
            if (version != 1.0f)
            {
                CCLog.Log("CocosSharp: CCTileMapInfo: Unsupported TMX version: {0}", version);
            }

            // Map type. Ortho, Iso, etc.
            string mapTypeStr = currentAttributeDict[MapElementMapType];
            CCTileMapType mapType = CCTileMapType.None;

            if (mapTypeKeys.TryGetValue(mapTypeStr, out mapType))
            {
                this.MapType = mapType;
            } else {
                CCLog.Log("CocosSharp: CCTileMapInfo: Unsupported TMX map type: {0}", mapTypeStr);
            }

            // Num of tile rows/columns in map
            CCTileMapCoordinates mapSize;
            mapSize.Column = (int)CCUtils.CCParseFloat(currentAttributeDict[MapElementNumOfColumns]);
            mapSize.Row = (int)CCUtils.CCParseFloat(currentAttributeDict[MapElementNumOfRows]);
            this.MapDimensions = mapSize;

            // Default tile texel dimensions
            CCSize tileTexelSize;
            tileTexelSize.Width = CCUtils.CCParseFloat(currentAttributeDict[MapElementTileTexelWidth]);
            tileTexelSize.Height = CCUtils.CCParseFloat(currentAttributeDict[MapElementTileTexelHeight]);
            this.TileTexelSize = tileTexelSize;

            this.currentParentElement = CCTileMapProperty.Map;
        }

        void ParseTilesetElement()
        {
            string externalTilesetFilename = null;

            // Tileset source
            if (currentAttributeDict.TryGetValue(TilesetElementFileSource, out externalTilesetFilename))
            {
                externalTilesetFilename = CCFileUtils.FullPathFromRelativeFile(externalTilesetFilename, 
                    TileMapFileName);

                currentFirstGID = short.Parse(currentAttributeDict[TilesetElementFirstGid]);

                ParseXmlFile(externalTilesetFilename);
            }
            else
            {
                var tileset = new CCTileSetInfo();

                tileset.Name = currentAttributeDict[TilesetElementName];

                // First GID
                if (currentFirstGID == 0)
                {
                    tileset.FirstGid = short.Parse(currentAttributeDict[TilesetElementFirstGid]);
                }
                else
                {
                    tileset.FirstGid = currentFirstGID;
                    currentFirstGID = 0;
                }

                string tileSpacingStr = null;
                string borderSizeStr = null;

                // Tilesheet tile spacing
                if(currentAttributeDict.TryGetValue(TilesetElementTileSpacing, out tileSpacingStr))
                    tileset.TileSpacing = int.Parse(tileSpacingStr);

                // Tilesheet border width
                if (currentAttributeDict.TryGetValue(TilesetElementBorderSize, out borderSizeStr))
                    tileset.BorderWidth = int.Parse(borderSizeStr);

                // Tile texel size
                CCSize tileTexelSize;
                tileTexelSize.Width = CCUtils.CCParseFloat(currentAttributeDict[TilesetElementTexelWidth]);
                tileTexelSize.Height = CCUtils.CCParseFloat(currentAttributeDict[TilesetElementTexelHeight]);
                tileset.TileTexelSize = tileTexelSize;

                Tilesets.Add(tileset);
            }
        }

        void ParseTileElement()
        {
            if (currentParentElement == CCTileMapProperty.Layer)
            {
                int layersCount = Layers != null ? Layers.Count : 0;
                CCTileLayerInfo layer = layersCount > 0 ? Layers[layersCount - 1] : null;

                uint gidAndFlags = uint.Parse(currentAttributeDict[TileElementGidAndFlags]);

                if (currentXmlTileIndex < layer.NumberOfTiles)
                    layer.TileGIDAndFlags[currentXmlTileIndex++] = CCTileMapFileEncodedTileFlags.DecodeGidAndFlags(gidAndFlags);
            }
            else
            {
                int tilesetCount = Tilesets != null ? Tilesets.Count : 0;
                CCTileSetInfo info = tilesetCount > 0 ? Tilesets[tilesetCount - 1] : null;

                var dict = new Dictionary<string, string>();

                ParentGID = (short)(info.FirstGid + short.Parse(currentAttributeDict[TileElementId]));
                TileProperties.Add(ParentGID, dict);
                currentParentElement = CCTileMapProperty.Tile;
            }

        }

        void ParseLayerElement()
        {
            var layerInfo = new CCTileLayerInfo();
            layerInfo.Name = currentAttributeDict[LayerElementName];

            CCTileMapCoordinates layerSize;
            layerSize.Column = (int)CCUtils.CCParseFloat(currentAttributeDict[LayerElementNumOfColumns]);
            layerSize.Row = (int)CCUtils.CCParseFloat(currentAttributeDict[LayerElementNumOfRows]);
            layerInfo.LayerDimensions = layerSize;

            layerInfo.TileGIDAndFlags = new CCTileGidAndFlags[layerSize.Column * layerSize.Row];

            if (currentAttributeDict.ContainsKey(LayerElementVisible))
            {
                string visible = currentAttributeDict[LayerElementVisible];
                layerInfo.Visible = !(visible == "0");
            }
            else
            {
                layerInfo.Visible = true;
            }

            if (currentAttributeDict.ContainsKey(LayerElementOpacity))
            {
                string opacity = currentAttributeDict[LayerElementOpacity];
                layerInfo.Opacity = (byte)(byte.MaxValue * CCUtils.CCParseFloat(opacity));
            }
            else
            {
                layerInfo.Opacity = byte.MaxValue;
            }

            float x = currentAttributeDict.ContainsKey(LayerElementXOffset) ? 
                CCUtils.CCParseFloat(currentAttributeDict[LayerElementXOffset]) : 0;
            float y = currentAttributeDict.ContainsKey(LayerElementYOffset) ? 
                CCUtils.CCParseFloat(currentAttributeDict[LayerElementYOffset]) : 0;
            layerInfo.TileCoordOffset = new CCPoint(x, y);

            Layers.Add(layerInfo);

            currentParentElement = CCTileMapProperty.Layer;
        }

        void ParseObjectGroupElement()
        {
            var objectGroup = new CCTileMapObjectGroup();
            objectGroup.GroupName = currentAttributeDict[ObjectGrpElementName];

            CCPoint positionOffset = CCPoint.Zero;
            if (currentAttributeDict.ContainsKey(ObjectGrpElementXOffset))
                positionOffset.X = CCUtils.CCParseFloat(currentAttributeDict[ObjectGrpElementXOffset]) * TileTexelSize.Width;
            if (currentAttributeDict.ContainsKey(ObjectGrpElementYOffset))
                positionOffset.Y = CCUtils.CCParseFloat(currentAttributeDict[ObjectGrpElementYOffset]) * TileTexelSize.Height;
            objectGroup.PositionOffset = positionOffset;

            ObjectGroups.Add(objectGroup);

            currentParentElement = CCTileMapProperty.ObjectGroup;
        }

        void ParseImageElement()
        {
            List<CCTileSetInfo> tilesets = Tilesets;
            int tilesetCount = tilesets != null ? tilesets.Count : 0;
            CCTileSetInfo tileset = tilesetCount > 0 ? tilesets[tilesetCount - 1] : null;

            string imagename = currentAttributeDict[ImageElementTilesheetName];
            tileset.TilesheetFilename = imagename;

            var directory = string.Empty;
            if (string.IsNullOrEmpty (TileMapFileName))
                tileset.TilesheetFilename = imagename;
            else 
            {
                if (!CCFileUtils.GetDirectoryName (imagename, out directory))
                    tileset.TilesheetFilename = CCFileUtils.FullPathFromRelativeFile (imagename, TileMapFileName);
            }
        }

        void ParseDataElement()
        {
            string encoding = currentAttributeDict.ContainsKey(DataElementEncoding) 
                ? currentAttributeDict[DataElementEncoding] : String.Empty;
            string compression = currentAttributeDict.ContainsKey(DataElementCompression) 
                ? currentAttributeDict[DataElementCompression] : String.Empty;

            if (encoding == DataElementBase64)
            {
                tileDataCompressionType = tileDataCompressionType | CCTileDataCompressionType.Base64;
                storingCharacters = true;

                if (compression == DataElementGzip)
                {
                    tileDataCompressionType = tileDataCompressionType | CCTileDataCompressionType.Gzip;
                }
                else if (compression == DataElementZlib)
                {
                    tileDataCompressionType = tileDataCompressionType | CCTileDataCompressionType.Zlib;
                }
                else if (compression != String.Empty)
                {
                    throw new NotImplementedException(
                        String.Format("CCTileMapInfo: ParseDataElement: Unsupported compression method {0}", compression));
                }
            }

            else if(encoding != String.Empty)
                throw new NotImplementedException("CTileMapInfo: ParseDataElement: Only base64 encoded maps are supported");
        }

        void ParseObjectElement()
        {
            int objectGroupCount = ObjectGroups != null ? ObjectGroups.Count : 0;
            CCTileMapObjectGroup objectGroup = objectGroupCount > 0 ? ObjectGroups[objectGroupCount - 1] : null;

            // The value for "type" was blank or not a valid class name
            // Create an instance of TMXObjectInfo to store the object and its properties
            var dict = new Dictionary<string, string>();

            var array = new[] { ObjectElementName, ObjectElementType, ObjectElementWidth, ObjectElementHeight, ObjectElementGid};

            for (int i = 0; i < array.Length; i++)
            {
                string key = array[i];
                if (currentAttributeDict.ContainsKey(key))
                {
                    dict.Add(key, currentAttributeDict[key]);
                }
            }

            int x = int.Parse(currentAttributeDict[ObjectElementXPosition]) + (int) objectGroup.PositionOffset.X;
            dict.Add(ObjectElementXPosition, x.ToString());

            int y = int.Parse(currentAttributeDict[ObjectElementYPosition]) + (int) objectGroup.PositionOffset.Y;

            // Correct y position. Tiled uses inverted y-coordinate system where top is y=0
            y = (int) (MapDimensions.Row * TileTexelSize.Height) - y -
                (currentAttributeDict.ContainsKey(ObjectElementHeight) ? int.Parse(currentAttributeDict[ObjectElementHeight]) : 0);
            dict.Add(ObjectElementYPosition, y.ToString());

            objectGroup.Objects.Add(dict);

            currentParentElement = CCTileMapProperty.Object;
        }

        void ParsePropertyElement()
        {
            if (currentParentElement == CCTileMapProperty.None)
            {
                CCLog.Log("CCTileMapInfo: ParsePropertyElement: Parent element is unsupported. Cannot add property named '{0}' with value '{1}'",
                    currentAttributeDict[PropertyElementName], currentAttributeDict[PropertyElementValue]);
            }
            else if (currentParentElement == CCTileMapProperty.Map)
            {
                // The parent element is the map
                string value = currentAttributeDict[PropertyElementValue];
                string key = currentAttributeDict[PropertyElementName];
                MapProperties.Add(key, value);
            }
            else if (currentParentElement == CCTileMapProperty.Layer)
            {
                int layersCount = Layers != null ? Layers.Count : 0;
                CCTileLayerInfo layer = layersCount > 0 ? Layers[layersCount - 1] : null;

                string value = currentAttributeDict[PropertyElementValue];
                string key = currentAttributeDict[PropertyElementName];
                // Add the property to the layer
                layer.Properties.Add(key, value);
            }
            else if (currentParentElement == CCTileMapProperty.ObjectGroup)
            {
                int objGroupsCount = ObjectGroups != null ? ObjectGroups.Count : 0;
                CCTileMapObjectGroup objectGroup = objGroupsCount > 0 ? ObjectGroups[objGroupsCount - 1] : null;
                string value = currentAttributeDict[PropertyElementValue];
                string key = currentAttributeDict[PropertyElementName];
                objectGroup.Properties.Add(key, value);
            }
            else if (currentParentElement == CCTileMapProperty.Object)
            {
                // The parent element is the last object
                int objGroupsCount = ObjectGroups != null ? ObjectGroups.Count : 0;
                CCTileMapObjectGroup objectGroup = objGroupsCount > 0 ? ObjectGroups[objGroupsCount - 1] : null;

                List<Dictionary<string, string>> objects = objectGroup.Objects;
                int objCount = objects != null ? objects.Count : 0;
                Dictionary<string, string> dict = objCount > 0 ? objects[objCount -1] : null;

                string propertyName = currentAttributeDict[PropertyElementName];
                string propertyValue = currentAttributeDict[PropertyElementValue];
                dict.Add(propertyName, propertyValue);
            }
            else if (currentParentElement == CCTileMapProperty.Tile)
            {
                Dictionary<string, string> dict = TileProperties[ParentGID];

                string propertyName = currentAttributeDict[PropertyElementName];
                string propertyValue = currentAttributeDict[PropertyElementValue];
                dict.Add(propertyName, propertyValue);
            }
        }

        void ParsePolygonElement()
        {
            // find parent object's dict and add polygon-points to it
            int objGroupsCount = ObjectGroups != null ? ObjectGroups.Count : 0;
            CCTileMapObjectGroup objectGroup = objGroupsCount > 0 ? ObjectGroups[objGroupsCount - 1] : null;

            List<Dictionary<string, string>> objects = objectGroup.Objects;
            int objCount = objects != null ? objects.Count : 0;
            Dictionary<string, string> dict = objCount > 0 ? objects[objCount -1] : null;

            // get points value string
            var value = currentAttributeDict["points"];
            if (!String.IsNullOrEmpty(value))
            {
                var pointsArray = new List<CCPoint>();
                var pointPairs = value.Split(' ');

                foreach (var pontPair in pointPairs)
                {
                    //TODO: Parse points
                    //CCPoint point;
                    //point.X = x + objectGroup.PositionOffset.X;
                    //point.Y = y + objectGroup.PositionOffset.Y;

                    //pPointsArray.Add(point);
                }

                //dict.Add("points", pPointsArray);
            }
        }

        void ParsePolylineElement()
        {
            // find parent object's dict and add polyline-points to it
            // CCTMXObjectGroup* objectGroup = (CCTMXObjectGroup*)ObjectGroups->lastObject();
            // CCDictionary* dict = (CCDictionary*)objectGroup->getObjects()->lastObject();
            // TODO: dict->setObject:[currentAttributeDict objectForKey:@"points"] forKey:@"polylinePoints"];
        }

        #endregion Parse begin element methods


        #region Parse end element methods

        void ParseMapEndElement()
        {
            currentParentElement = CCTileMapProperty.None;
        }

        void ParseTileEndElement()
        {
        }

        void ParseTilesetEndElement()
        {
        }

        void ParseLayerEndElement()
        {
            currentParentElement = CCTileMapProperty.None;
        }

        void ParseObjectGroupEndElement()
        {
            currentParentElement = CCTileMapProperty.None;
        }

        void ParseImageEndElement()
        {
        }

        void ParseDataEndElement()
        {
            byte[] encoded = null;

            if ((tileDataCompressionType & CCTileDataCompressionType.Base64) != 0)
            {
                storingCharacters = false;

                int layersCount = Layers != null ? Layers.Count : 0;
                CCTileLayerInfo layer = layersCount > 0 ? Layers[layersCount - 1] : null;

                if ((tileDataCompressionType & (CCTileDataCompressionType.Gzip | CCTileDataCompressionType.Zlib)) != 0)
                {
                    if ((tileDataCompressionType & CCTileDataCompressionType.Gzip) != 0)
                    {
                        try
                        {
                            encoded = ZipUtils.Inflate(new MemoryStream(currentString), ZipUtils.CompressionFormat.Gzip);
                        }
                        catch (Exception ex)
                        {
                            CCLog.Log("failed to decompress embedded data object in TMX file.");
                            CCLog.Log(ex.ToString());
                        }
                    }

                    if ((tileDataCompressionType & CCTileDataCompressionType.Zlib) != 0)
                    {
                        encoded = ZipUtils.Inflate (new MemoryStream (currentString), ZipUtils.CompressionFormat.Zlib);
                    }
                }
                else
                {
                    encoded = currentString;
                }

                for (int i = 0; i < layer.TileGIDAndFlags.Length; i++)
                {
                    int i4 = i * 4;
                    uint gidAndFlags = (uint) (
                        encoded[i4] |
                        encoded[i4 + 1] << 8 |
                        encoded[i4 + 2] << 16 |
                        encoded[i4 + 3] << 24);

                    layer.TileGIDAndFlags[i] = CCTileMapFileEncodedTileFlags.DecodeGidAndFlags(gidAndFlags);
                }

                currentString = null;
            }
            else if((tileDataCompressionType & CCTileDataCompressionType.None) != 0)
            {
                currentXmlTileIndex = 0;
            }
        }

        void ParseObjectEndElement()
        {
            currentParentElement = CCTileMapProperty.None;
        }

        void ParsePropertyEndElement()
        {
        }

        void ParsePolygonEndElement()
        {
        }

        void ParsePolylineEndElement()
        {
        }

        #endregion Parse end element methods
    }
}