using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

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

        const string TileAnimKeyFrameGid = "tileid";
        const string TileAnimKeyFrameDuration = "duration";

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
        const string DataElementCsv = "csv";
        const string DataElementGzip = "gzip";
        const string DataElementZlib = "zlib";

        const string ObjectElementName = "name"; 
        const string ObjectElementType = "type";
        const string ObjectElementWidth = "width";
        const string ObjectElementHeight = "height";
        const string ObjectElementGid = "gid";
        const string ObjectElementXPosition = "x";
        const string ObjectElementYPosition = "y";
	    const string ObjectElementPoints = "points";
	    const string ObjectElementShape = "shape";
	    const string ObjectElementShapeEllipse = "ellipse";
	    const string ObjectElementShapePolygon = "polygon";
	    const string ObjectElementShapePolyline = "polyline";


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
        List<CCTileAnimationKeyFrame> currentTileAnimationKeyFrames;


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
        internal Dictionary<short, List<CCTileAnimationKeyFrame>> TileAnimations { get; private set; }

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
            TileAnimations = new Dictionary<short, List<CCTileAnimationKeyFrame>>();
            tileDataCompressionType = CCTileDataCompressionType.None;
            currentParentElement = CCTileMapProperty.None;
            currentFirstGID = 0;

            // Associating xml elements of tmx file with parse actions
            mapFileElementActions = new Dictionary<string, Tuple<Action,Action>>
            {
                { "map", Tuple.Create<Action,Action>(ParseMapElement, ParseMapEndElement) },
                { "tileset", Tuple.Create<Action,Action>(ParseTilesetElement, ParseTilesetEndElement) },
                { "tile", Tuple.Create<Action,Action>(ParseTileElement, ParseTileEndElement) },
                { "animation", Tuple.Create<Action,Action>(ParseTileAnimationElement, ParseTileAnimationEndElement) },
                { "frame", Tuple.Create<Action,Action>(ParseTileAnimationKeyFrameElement, ParseTileAnimationKeyFrameEndElement) },
                { "layer", Tuple.Create<Action,Action>(ParseLayerElement, ParseLayerEndElement) },
                { "objectgroup", Tuple.Create<Action,Action>(ParseObjectGroupElement, ParseObjectGroupEndElement) },
                { "image", Tuple.Create<Action,Action>(ParseImageElement, ParseImageEndElement) },
                { "data", Tuple.Create<Action,Action>(ParseDataElement, ParseDataEndElement) },
                { "object", Tuple.Create<Action,Action>(ParseObjectElement, ParseObjectEndElement) },
                { "property", Tuple.Create<Action,Action>(ParsePropertyElement, ParsePropertyEndElement) },
                { "polygon", Tuple.Create<Action,Action>(ParsePolygonElement, ParsePolygonEndElement) },
                { "polyline", Tuple.Create<Action,Action>(ParsePolylineElement, ParsePolylineEndElement) },
                { "ellipse", Tuple.Create<Action,Action>(ParseEllipseElement, ParseEllipseEndElement) },
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

        void ParseTileAnimationElement()
        {
            if(currentParentElement == CCTileMapProperty.Tile) 
            {
                currentParentElement = CCTileMapProperty.TileAnimation;
                currentTileAnimationKeyFrames = new List<CCTileAnimationKeyFrame>();
            }
        }

        void ParseTileAnimationKeyFrameElement()
        {
            if(currentParentElement == CCTileMapProperty.TileAnimation && currentTileAnimationKeyFrames != null) 
            {
                int tilesetCount = Tilesets != null ? Tilesets.Count : 0;
                CCTileSetInfo info = tilesetCount > 0 ? Tilesets[tilesetCount - 1] : null;

                short frameGid = (short)(info.FirstGid + short.Parse(currentAttributeDict[TileAnimKeyFrameGid]));
                short frameDuration = short.Parse(currentAttributeDict [TileAnimKeyFrameDuration]);

                if(frameGid >= 0 && frameDuration > 0)
                    currentTileAnimationKeyFrames.Add (new CCTileAnimationKeyFrame(frameGid, frameDuration));
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

                if (compression == DataElementGzip) {
                    tileDataCompressionType = tileDataCompressionType | CCTileDataCompressionType.Gzip;
                } else if (compression == DataElementZlib) {
                    tileDataCompressionType = tileDataCompressionType | CCTileDataCompressionType.Zlib;
                } else if (compression != String.Empty) {
                    throw new NotImplementedException (
                        String.Format ("CCTileMapInfo: ParseDataElement: Unsupported compression method {0}", compression));
                }
            } 
            else if (encoding == DataElementCsv) 
            {
                tileDataCompressionType = CCTileDataCompressionType.Csv;
                storingCharacters = true;
            }

            else if(encoding != String.Empty)
                throw new NotImplementedException("CTileMapInfo: ParseDataElement: Only base64 encoded maps are supported");
        }

        void ParseObjectElement()
        {
	        if (ObjectGroups == null || ObjectGroups.Count == 0)
		        return;

            CCTileMapObjectGroup objectGroup = ObjectGroups[ObjectGroups.Count - 1];

            // The value for "type" was blank or not a valid class name
            // Create an instance of TMXObjectInfo to store the object and its properties
            var dict = new Dictionary<string, string>();

            var array = new[] { ObjectElementName, ObjectElementType, ObjectElementWidth, ObjectElementHeight, ObjectElementGid};

            foreach (string key in array)
            {
	            if (currentAttributeDict.ContainsKey(key))
	            {
		            dict.Add(key, currentAttributeDict[key]);
	            }
            }

            float x = float.Parse(currentAttributeDict[ObjectElementXPosition]) + objectGroup.PositionOffset.X;
            float y = float.Parse(currentAttributeDict[ObjectElementYPosition]) + objectGroup.PositionOffset.Y;

            // Correct y position. Tiled uses inverted y-coordinate system where top is y=0
            y = (MapDimensions.Row * TileTexelSize.Height) - y -
                (currentAttributeDict.ContainsKey(ObjectElementHeight) ? float.Parse(currentAttributeDict[ObjectElementHeight]) : 0);

            dict.Add(ObjectElementXPosition, ToFloatString(x));
            dict.Add(ObjectElementYPosition, ToFloatString(y));

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
			ParseMultilineShape(ObjectElementShapePolygon);
        }

        void ParsePolylineElement()
        {
			ParseMultilineShape(ObjectElementShapePolyline);
        }

	    void ParseMultilineShape(string shapeName)
	    {
            // Find parent object's dict and add points to it. If at any time we don't find the objects we are expecting 
			// based on the state of the parser, just return without doing anything instead of crashing.
 	        if (ObjectGroups == null || ObjectGroups.Count == 0)
		        return;

            CCTileMapObjectGroup objectGroup = ObjectGroups[ObjectGroups.Count - 1];
	        if (objectGroup == null || objectGroup.Objects.Count == 0)
		        return;

	        var dict = objectGroup.Objects[objectGroup.Objects.Count - 1];
	        if (!currentAttributeDict.ContainsKey(ObjectElementPoints))
		        return;

            string value = currentAttributeDict[ObjectElementPoints];
	        if (String.IsNullOrWhiteSpace(value))
		        return;

		    if (!dict.ContainsKey(ObjectElementXPosition) || !dict.ContainsKey(ObjectElementYPosition))
			    return;

		    float objectXOffset = float.Parse(dict[ObjectElementXPosition]);
		    float objectYOffset = float.Parse(dict[ObjectElementYPosition]);

	        string[] pointPairs = value.Split(' ');
	        var points = new CCPoint[pointPairs.Length];

	        var sb = new StringBuilder();
	        for (int i = 0; i < pointPairs.Length; i++)
	        {
		        string pointPair = pointPairs[i];
		        string[] pointCoords = pointPair.Split(',');
		        if (pointCoords.Length != 2)
			        return;

				// Adjust the offsets relative to the parent object. When adjusting the coordinates,
				// correct y position. Tiled uses inverted y-coordinate system where top is y=0.
				// We have to invert the y coordinate to make it move in the correct direction relative to the parent.
		        points[i].X = float.Parse(pointCoords[0]) + objectXOffset;
		        points[i].Y = float.Parse(pointCoords[1]) * -1 + objectYOffset;

		        sb.Append(ToFloatString(points[i].X));
		        sb.Append(",");
		        sb.Append(ToFloatString(points[i].Y));
		        sb.Append(" ");
	        }

			// Strip the trailing space
			string pointsString = sb.Length > 0 ? sb.ToString(0, sb.Length - 1) : null;
            dict.Add(ObjectElementPoints, pointsString);

			dict[ObjectElementShape] = shapeName;
	    }

	    void ParseEllipseElement()
	    {
 	        if (ObjectGroups == null || ObjectGroups.Count == 0)
		        return;

            CCTileMapObjectGroup objectGroup = ObjectGroups[ObjectGroups.Count - 1];
	        if (objectGroup == null || objectGroup.Objects.Count == 0)
		        return;

	        var dict = objectGroup.Objects[objectGroup.Objects.Count - 1];
			dict[ObjectElementShape] = ObjectElementShapeEllipse;
	    }

	    string ToFloatString(float f)
	    {
            double floorF = Math.Floor(f);

            // Different numeric dict values can be either int or float depending on context
            // When we call Parse.int(str), we need to format of the string to include no decimals
            // or else a bad format exception is thrown

            if (f == floorF)
                return ((int)f).ToString();
            else
                return f.ToString();
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

        void ParseTileAnimationEndElement()
        {
            if(currentParentElement == CCTileMapProperty.TileAnimation) 
            {
                if(currentTileAnimationKeyFrames != null && currentTileAnimationKeyFrames.Count > 0)
                    TileAnimations.Add(ParentGID, currentTileAnimationKeyFrames);

                currentTileAnimationKeyFrames = null;
            }

            currentParentElement = CCTileMapProperty.Tile;
        }

        void ParseTileAnimationKeyFrameEndElement()
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
                        (uint)encoded[i4] |
                        (uint)encoded[(int)(i4 + 1)] << 8 |
                        (uint)encoded[(int)(i4 + 2)] << 16 |
                        (uint)encoded[(int)(i4 + 3)] << 24);
                        

                    layer.TileGIDAndFlags[i] = CCTileMapFileEncodedTileFlags.DecodeGidAndFlags(gidAndFlags);
                }

                currentString = null;
            }
            else if(tileDataCompressionType == CCTileDataCompressionType.Csv)
            {
                storingCharacters = false;

                int layersCount = Layers != null ? Layers.Count : 0;
                CCTileLayerInfo layer = layersCount > 0 ? Layers[layersCount - 1] : null;

                var str = System.Text.Encoding.UTF8.GetString(currentString, 0, currentString.Length).Split(',');

                for (int i = 0; i < layer.TileGIDAndFlags.Length; i++)
                {
                    uint gidAndFlags = uint.Parse(str[i]);
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

	    void ParseEllipseEndElement()
	    {
	    }

        #endregion Parse end element methods
    }
}

