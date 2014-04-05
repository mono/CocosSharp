using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using CocosSharp.Compression.Zlib;
using WP7Contrib.Communications.Compression;

namespace CocosSharp
{
    /// <summary>
    /// CCTMXMapInfo contains the information about the map like:
    ///- Map orientation (hexagonal, isometric or orthogonal)
    ///- Tile size
    ///- Map size
    ///
    ///	And it also contains:
    ///- Layers (an array of TMXLayerInfo objects)
    ///- Tilesets (an array of TMXTilesetInfo objects)
    ///- ObjectGroups (an array of TMXObjectGroupInfo objects)
    ///
    ///This information is obtained from the TMX file.
    /// </summary>
    public class CCTMXMapInfo : ICCSAXDelegator
    {
		// ivars
		uint currentFirstGID;


		#region properties

		public uint ParentGID { get; private set; }
		public string TMXFileName { get; private set; }

		public int Orientation { get; private set; }
		public CCSize MapSize { get; private set; }
		public CCSize TileSize { get; private set; }

		public bool StoringCharacters { get; private set; }
		internal int ParentElement { get; private set; }
		protected int LayerAttribs { get; private set; }
		internal virtual List<CCTMXLayerInfo> Layers { get; private set; }
		internal virtual List<CCTMXTilesetInfo> Tilesets { get; private set; }
		internal virtual List<CCTMXObjectGroup> ObjectGroups { get; private set; }

		internal Dictionary<string, string> Properties { get; private set; }
		internal Dictionary<uint, Dictionary<string, string>> TileProperties { get; private set; }
		internal byte[] CurrentString { get; private set; }

        #endregion


        #region Constructors

		public CCTMXMapInfo(string tmxFile) : this()
        {
			TMXFileName = CCFileUtils.FullPathFromRelativePath(tmxFile);
			ParseXmlFile(TMXFileName);
		}

		public CCTMXMapInfo(StreamReader stream) : this()
        {
			string data = stream.ReadToEnd();
			ParseXmlString(data);
        }

		CCTMXMapInfo()
        {
            Tilesets = new List<CCTMXTilesetInfo>();
            Layers = new List<CCTMXLayerInfo>();
            ObjectGroups = new List<CCTMXObjectGroup>(4);
            Properties = new Dictionary<string, string>();
            TileProperties = new Dictionary<uint, Dictionary<string, string>>();
            LayerAttribs = (int) CCTMXLayerAttrib.None;
            ParentElement = (int) CCTMXProperty.None;
			currentFirstGID = 0;
        }

		bool ParseXmlString(string data)
		{
			var parser = new CCSAXParser();

			if (false == parser.Init("UTF-8"))
			{
				return false;
			}

			parser.SetDelegator(this);

			return parser.ParseContent(data);
		}

		// Initalises parsing of an XML file, either a tmx (Map) file or tsx (Tileset) file
		bool ParseXmlFile(string xmlFilename)
		{
			var parser = new CCSAXParser();

			if (false == parser.Init("UTF-8"))
			{
				return false;
			}

			parser.SetDelegator(this);

			return parser.ParseContentFile(xmlFilename);
		}

        #endregion Constructors


		#region ICCSAXDelegator methods

        public void StartElement(object ctx, string name, string[] atts)
        {
            CCTMXMapInfo pTMXMapInfo = this;
            string elementName = name;
            var attributeDict = new Dictionary<string, string>();

            if (atts != null && atts[0] != null)
            {
                for (int i = 0; i + 1 < atts.Length; i += 2)
                {
                    string key = atts[i];
                    string value = atts[i + 1];
                    attributeDict.Add(key, value);
                }
            }

            if (elementName == "map")
            {
                string version = attributeDict["version"];
                if (version != "1.0")
                {
                    CCLog.Log("CocosSharp: TMXFormat: Unsupported TMX version: {0}", version);
                }
                string orientationStr = attributeDict["orientation"];
                if (orientationStr == "orthogonal")
                    pTMXMapInfo.Orientation = (int) (CCTMXOrientation.Ortho);
                else if (orientationStr == "isometric")
                    pTMXMapInfo.Orientation = (int) (CCTMXOrientation.Iso);
                else if (orientationStr == "hexagonal")
                    pTMXMapInfo.Orientation = (int) (CCTMXOrientation.Hex);
                else
                    CCLog.Log("CocosSharp: TMXFomat: Unsupported orientation: {0}", pTMXMapInfo.Orientation);

                CCSize sMapSize;
                sMapSize.Width = CCUtils.CCParseFloat(attributeDict["width"]);
                sMapSize.Height = CCUtils.CCParseFloat(attributeDict["height"]);
                pTMXMapInfo.MapSize = sMapSize;

                CCSize sTileSize;
                sTileSize.Width = CCUtils.CCParseFloat(attributeDict["tilewidth"]);
                sTileSize.Height = CCUtils.CCParseFloat(attributeDict["tileheight"]);
                pTMXMapInfo.TileSize = sTileSize;

                // The parent element is now "map"
                pTMXMapInfo.ParentElement = (int) CCTMXProperty.Map;
            }
            else if (elementName == "tileset")
            {
                // If this is an external tileset then start parsing that

                if (attributeDict.Keys.Contains("source"))
                {
                    string externalTilesetFilename = attributeDict["source"];

                    externalTilesetFilename = CCFileUtils.FullPathFromRelativeFile(externalTilesetFilename, 
						pTMXMapInfo.TMXFileName);

                    currentFirstGID = uint.Parse(attributeDict["firstgid"]);
                    
                    pTMXMapInfo.ParseXmlFile(externalTilesetFilename);
                }
                else
                {
                    var tileset = new CCTMXTilesetInfo();

                    tileset.Name = attributeDict["name"];
                    
                    if (currentFirstGID == 0)
                    {
                        tileset.FirstGid = uint.Parse(attributeDict["firstgid"]);
                    }
                    else
                    {
                        tileset.FirstGid = currentFirstGID;
                        currentFirstGID = 0;
                    }

                    if (attributeDict.Keys.Contains("spacing"))
                        tileset.Spacing = int.Parse(attributeDict["spacing"]);

                    if (attributeDict.Keys.Contains("margin"))
                        tileset.Margin = int.Parse(attributeDict["margin"]);

                    CCSize s;
                    s.Width = CCUtils.CCParseFloat(attributeDict["tilewidth"]);
                    s.Height = CCUtils.CCParseFloat(attributeDict["tileheight"]);
                    tileset.TileSize = s;

                    pTMXMapInfo.Tilesets.Add(tileset);
                }
            }
            else if (elementName == "tile")
            {
                CCTMXTilesetInfo info = pTMXMapInfo.Tilesets.LastOrDefault();
                var dict = new Dictionary<string, string>();
                pTMXMapInfo.ParentGID = (info.FirstGid + uint.Parse(attributeDict["id"]));
                pTMXMapInfo.TileProperties.Add(pTMXMapInfo.ParentGID, dict);

                pTMXMapInfo.ParentElement = (int) CCTMXProperty.Tile;
            }
            else if (elementName == "layer")
            {
                var layer = new CCTMXLayerInfo();
                layer.Name = attributeDict["name"];

                CCSize s;
                s.Width = CCUtils.CCParseFloat(attributeDict["width"]);
                s.Height = CCUtils.CCParseFloat(attributeDict["height"]);
                layer.LayerSize = s;

                layer.Tiles = new uint[(int) s.Width * (int) s.Height];

                if (attributeDict.Keys.Contains("visible"))
                {
                    string visible = attributeDict["visible"];
                    layer.Visible = !(visible == "0");
                }
                else
                {
                    layer.Visible = true;
                }

                if (attributeDict.Keys.Contains("opacity"))
                {
                    string opacity = attributeDict["opacity"];
                    layer.Opacity = (byte) (255 * CCUtils.CCParseFloat(opacity));
                }
                else
                {
                    layer.Opacity = 255;
                }

                float x = attributeDict.Keys.Contains("x") ? CCUtils.CCParseFloat(attributeDict["x"]) : 0;
                float y = attributeDict.Keys.Contains("y") ? CCUtils.CCParseFloat(attributeDict["y"]) : 0;
                layer.Offset = new CCPoint(x, y);

                pTMXMapInfo.Layers.Add(layer);

                // The parent element is now "layer"
                pTMXMapInfo.ParentElement = (int) CCTMXProperty.Layer;
            }
            else if (elementName == "objectgroup")
            {
                var objectGroup = new CCTMXObjectGroup();
                objectGroup.GroupName = attributeDict["name"];

                CCPoint positionOffset = CCPoint.Zero;
                if (attributeDict.ContainsKey("x"))
                    positionOffset.X = CCUtils.CCParseFloat(attributeDict["x"]) * pTMXMapInfo.TileSize.Width;
                if (attributeDict.ContainsKey("y"))
                    positionOffset.Y = CCUtils.CCParseFloat(attributeDict["y"]) * pTMXMapInfo.TileSize.Height;
                objectGroup.PositionOffset = positionOffset;

                pTMXMapInfo.ObjectGroups.Add(objectGroup);

                // The parent element is now "objectgroup"
                pTMXMapInfo.ParentElement = (int) CCTMXProperty.ObjectGroup;
            }
            else if (elementName == "image")
            {
                CCTMXTilesetInfo tileset = pTMXMapInfo.Tilesets.LastOrDefault();

                // build full path
                string imagename = attributeDict["source"];
                tileset.SourceImage = CCFileUtils.FullPathFromRelativeFile(imagename, pTMXMapInfo.TMXFileName);
            }
            else if (elementName == "data")
            {
                string encoding = attributeDict.ContainsKey("encoding") ? attributeDict["encoding"] : "";
                string compression = attributeDict.ContainsKey("compression") ? attributeDict["compression"] : "";

                if (encoding == "base64")
                {
                    int layerAttribs = pTMXMapInfo.LayerAttribs;
                    pTMXMapInfo.LayerAttribs = layerAttribs | (int) CCTMXLayerAttrib.Base64;
                    pTMXMapInfo.StoringCharacters = true;

                    if (compression == "gzip")
                    {
                        layerAttribs = pTMXMapInfo.LayerAttribs;
                        pTMXMapInfo.LayerAttribs = layerAttribs | (int) CCTMXLayerAttrib.Gzip;
                    }
                    else if (compression == "zlib")
                    {
                        layerAttribs = pTMXMapInfo.LayerAttribs;
                        pTMXMapInfo.LayerAttribs = layerAttribs | (int) CCTMXLayerAttrib.Zlib;
                    }
                    Debug.Assert(compression == "" || compression == "gzip" || compression == "zlib", "TMX: unsupported compression method");
                }
                Debug.Assert(pTMXMapInfo.LayerAttribs != (int) CCTMXLayerAttrib.None,
                             "TMX tile map: Only base64 and/or gzip/zlib maps are supported");
            }
            else if (elementName == "object")
            {
                CCTMXObjectGroup objectGroup = pTMXMapInfo.ObjectGroups.LastOrDefault();

                // The value for "type" was blank or not a valid class name
                // Create an instance of TMXObjectInfo to store the object and its properties
                var dict = new Dictionary<string, string>();

                var pArray = new[] {"name", "type", "width", "height", "gid"};

                for (int i = 0; i < pArray.Length; i++)
                {
                    string key = pArray[i];
                    if (attributeDict.ContainsKey(key))
                    {
                        dict.Add(key, attributeDict[key]);
                    }
                }

                // But X and Y since they need special treatment
                // X

                int x = int.Parse(attributeDict["x"]) + (int) objectGroup.PositionOffset.X;
                dict.Add("x", x.ToString());

                int y = int.Parse(attributeDict["y"]) + (int) objectGroup.PositionOffset.Y;
                // Correct y position. (Tiled uses Flipped, cocos2d uses Standard)
                y = (int) (pTMXMapInfo.MapSize.Height * pTMXMapInfo.TileSize.Height) - y -
                    (attributeDict.ContainsKey("height") ? int.Parse(attributeDict["height"]) : 0);
                dict.Add("y", y.ToString());

                // Add the object to the objectGroup
                objectGroup.Objects.Add(dict);

                // The parent element is now "object"
                pTMXMapInfo.ParentElement = (int) CCTMXProperty.Object;
            }
            else if (elementName == "property")
            {
                if (pTMXMapInfo.ParentElement == (int) CCTMXProperty.None)
                {
                    CCLog.Log("TMX tile map: Parent element is unsupported. Cannot add property named '{0}' with value '{1}'",
                              attributeDict["name"], attributeDict["value"]);
                }
                else if (pTMXMapInfo.ParentElement == (int) CCTMXProperty.Map)
                {
                    // The parent element is the map
                    string value = attributeDict["value"];
                    string key = attributeDict["name"];
                    pTMXMapInfo.Properties.Add(key, value);
                }
                else if (pTMXMapInfo.ParentElement == (int) CCTMXProperty.Layer)
                {
                    // The parent element is the last layer
                    CCTMXLayerInfo layer = pTMXMapInfo.Layers.LastOrDefault();
                    string value = attributeDict["value"];
                    string key = attributeDict["name"];
                    // Add the property to the layer
                    layer.Properties.Add(key, value);
                }
                else if (pTMXMapInfo.ParentElement == (int) CCTMXProperty.ObjectGroup)
                {
                    // The parent element is the last object group
                    CCTMXObjectGroup objectGroup = pTMXMapInfo.ObjectGroups.LastOrDefault();
                    string value = attributeDict["value"];
                    string key = attributeDict["name"];
                    objectGroup.Properties.Add(key, value);
                }
                else if (pTMXMapInfo.ParentElement == (int) CCTMXProperty.Object)
                {
                    // The parent element is the last object
                    CCTMXObjectGroup objectGroup = pTMXMapInfo.ObjectGroups.LastOrDefault();
                    Dictionary<string, string> dict = objectGroup.Objects.LastOrDefault();

                    string propertyName = attributeDict["name"];
                    string propertyValue = attributeDict["value"];
                    dict.Add(propertyName, propertyValue);
                }
                else if (pTMXMapInfo.ParentElement == (int) CCTMXProperty.Tile)
                {
                    Dictionary<string, string> dict = pTMXMapInfo.TileProperties[pTMXMapInfo.ParentGID];

                    string propertyName = attributeDict["name"];
                    string propertyValue = attributeDict["value"];
                    dict.Add(propertyName, propertyValue);
                }
            }
            else if (elementName == "polygon")
            {
                // find parent object's dict and add polygon-points to it
                CCTMXObjectGroup objectGroup = ObjectGroups.LastOrDefault();
                var dict = objectGroup.Objects.LastOrDefault();

                // get points value string
                var value = attributeDict["points"];
                if (!String.IsNullOrEmpty(value))
                {
                    var pPointsArray = new List<CCPoint>();
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
            else if (elementName == "polyline")
            {
                // find parent object's dict and add polyline-points to it
                // CCTMXObjectGroup* objectGroup = (CCTMXObjectGroup*)ObjectGroups->lastObject();
                // CCDictionary* dict = (CCDictionary*)objectGroup->getObjects()->lastObject();
                // TODO: dict->setObject:[attributeDict objectForKey:@"points"] forKey:@"polylinePoints"];
            }
        }

        public void EndElement(object ctx, string elementName)
        {
            CCTMXMapInfo pTMXMapInfo = this;
            byte[] encoded = null;

            if (elementName == "data" && (pTMXMapInfo.LayerAttribs & (int) CCTMXLayerAttrib.Base64) != 0)
            {
                pTMXMapInfo.StoringCharacters = false;
                CCTMXLayerInfo layer = pTMXMapInfo.Layers.LastOrDefault();
                if ((pTMXMapInfo.LayerAttribs & ((int) (CCTMXLayerAttrib.Gzip) | (int) CCTMXLayerAttrib.Zlib)) != 0)
                {
                    //gzip compress
                    if ((pTMXMapInfo.LayerAttribs & (int) CCTMXLayerAttrib.Gzip) != 0)
                    {
                        try
                        {
                            GZipStream inGZipStream = new GZipStream(new MemoryStream(pTMXMapInfo.CurrentString));

                            var outMemoryStream = new MemoryStream();

                            var buffer = new byte[1024];
                            while (true)
                            {
                                int bytesRead = inGZipStream.Read(buffer, 0, buffer.Length);
                                if (bytesRead == 0)
                                    break;
                                outMemoryStream.Write(buffer, 0, bytesRead);
                            }
                            encoded = outMemoryStream.ToArray();
                        }
                        catch (Exception ex)
                        {
                            CCLog.Log("failed to decompress embedded data object in TMX file.");
                            CCLog.Log(ex.ToString());
                        }
                    }

                    //zlib
                    if ((pTMXMapInfo.LayerAttribs & (int) CCTMXLayerAttrib.Zlib) != 0)
                    {
                        var inZInputStream = new ZInputStream(new MemoryStream(pTMXMapInfo.CurrentString));

                        var outMemoryStream = new MemoryStream();

                        var buffer = new byte[1024];
                        while (true)
                        {
                            int bytesRead = inZInputStream.Read(buffer, 0, buffer.Length);
                            if (bytesRead == 0)
                                break;
                            outMemoryStream.Write(buffer, 0, bytesRead);
                        }

                        encoded = outMemoryStream.ToArray();
                    }
                }
                else
                {
                    encoded = pTMXMapInfo.CurrentString;
                }

                for (int i = 0; i < layer.Tiles.Length; i++)
                {
                    int i4 = i * 4;
                    var gid = (uint) (
                                         encoded[i4] |
                                         encoded[i4 + 1] << 8 |
                                         encoded[i4 + 2] << 16 |
                                         encoded[i4 + 3] << 24);

                    layer.Tiles[i] = gid;
                }

                pTMXMapInfo.CurrentString = null;
            }
            else if (elementName == "map")
            {
                // The map element has ended
                pTMXMapInfo.ParentElement = (int) CCTMXProperty.None;
            }
            else if (elementName == "layer")
            {
                // The layer element has ended
                pTMXMapInfo.ParentElement = (int) CCTMXProperty.None;
            }
            else if (elementName == "objectgroup")
            {
                // The objectgroup element has ended
                pTMXMapInfo.ParentElement = (int) CCTMXProperty.None;
            }
            else if (elementName == "object")
            {
                // The object element has ended
                pTMXMapInfo.ParentElement = (int) CCTMXProperty.None;
            }
        }

		public void TextHandler(object ctx, byte[] ch, int len)
        {
            CCTMXMapInfo pTMXMapInfo = this;

            if (pTMXMapInfo.StoringCharacters)
            {
                pTMXMapInfo.CurrentString = ch;
            }
        }

		#endregion ICCSAXDelegator methods
    }
}