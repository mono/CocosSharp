using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CocosSharp
{

    public class CCTileMapLayer : CCNode, IDisposable
    {
        const int NumOfVerticesPerQuad = 6;
        const int NumOfCornersPerQuad = 4;
        const int NumOfPrimitivesPerQuad = 2;

        // Index buffer value types are unsigned shorts => (2^16) / 4 = 16384 max tiles
        const int MaxTilesPerDrawBuffer = (ushort.MaxValue + 1) / 4;

        bool visibleTileRangeDirty;
        bool useAutomaticVertexZ;
        float defaultTileVertexZ;

        CCColor4B tileColor;

        CCAffineTransform tileCoordsToNodeTransform;
        CCAffineTransform nodeToTileCoordsTransform;

        // Used by hexagonal and staggered isometric
        CCAffineTransform tileCoordsToNodeTransformOdd;
        CCAffineTransform nodeToTileCoordsTransformOdd;

        CCTexture2D tileSetTexture;
        CCTileGidAndFlags[] tileGIDAndFlagsArray;
        CCTileMapDrawBufferManager drawBufferManager;

        #region Properties

        // Static properties

        public static float DefaultTexelToContentSizeRatio
        {
            set { DefaultTexelToContentSizeRatios = new CCSize(value, value); }
        }

        public static CCSize DefaultTexelToContentSizeRatios { get; set; }

        // Instance properties

        public string LayerName { get; set; }

        public CCTileMapType MapType { get; private set; }                  
        public CCTileMapCoordinates LayerSize { get; private set; }
        public CCSize TileTexelSize { get; private set; }                                 
        public CCTileSetInfo TileSetInfo { get; private set; }
        public Dictionary<string, string> LayerProperties { get; set; }


        public bool Antialiased
        {
            get { return tileSetTexture.IsAntialiased; }
            set { tileSetTexture.IsAntialiased = value; }
        }

        public override byte Opacity
        {
            get { return base.Opacity; }
            set
            {
                base.Opacity = value;
                tileColor = CCColor4B.White;
                tileColor.A = Opacity;
            }
        }

        public CCSize TileContentSize
        {
            get { return TileTexelSize * CCTileMapLayer.DefaultTexelToContentSizeRatios; }
        }


        internal override Matrix XnaLocalMatrix 
        { 
            get { return base.XnaLocalMatrix; }
            set 
            {
                base.XnaLocalMatrix = value;
                visibleTileRangeDirty = true;
            }
        }

        uint NumberOfTiles
        {
            get { return (uint)(LayerSize.Row * LayerSize.Column); }
        }

        #endregion Properties


        #region Constructors

        static CCTileMapLayer()
        {
            DefaultTexelToContentSizeRatios = CCSize.One;
        }

        public CCTileMapLayer(CCTileSetInfo tileSetInfo, CCTileLayerInfo layerInfo, CCTileMapInfo mapInfo)
            : this(tileSetInfo, layerInfo, mapInfo, layerInfo.LayerDimensions)
        {
        }

        // Private constructor chaining

        CCTileMapLayer(CCTileSetInfo tileSetInfo, CCTileLayerInfo layerInfo, CCTileMapInfo mapInfo, CCTileMapCoordinates layerSize) 
            : this(tileSetInfo, layerInfo, mapInfo, layerSize, (int)(layerSize.Row * layerSize.Column))
        {
        }

        CCTileMapLayer(CCTileSetInfo tileSetInfo, CCTileLayerInfo layerInfo, CCTileMapInfo mapInfo, CCTileMapCoordinates layerSize, int totalNumberOfTiles) 
            : this(tileSetInfo, layerInfo, mapInfo, layerSize, totalNumberOfTiles, (int)(totalNumberOfTiles * 0.35f + 1), 
                CCTextureCache.SharedTextureCache.AddImage(tileSetInfo.TilesheetFilename))
        {
        }

        CCTileMapLayer(CCTileSetInfo tileSetInfo, CCTileLayerInfo layerInfo, CCTileMapInfo mapInfo, CCTileMapCoordinates layerSize, 
            int totalNumberOfTiles, int tileCapacity, CCTexture2D texture)
        {
            if (texture.ContentSizeInPixels == CCSize.Zero)
                CCLog.Log("Tilemap Layer Texture {0} not loaded for layer {1}", tileSetInfo.TilesheetFilename, layerInfo.Name);

            LayerName = layerInfo.Name;
            LayerSize = layerSize;
            Opacity = layerInfo.Opacity;
            LayerProperties = new Dictionary<string, string>(layerInfo.Properties);

            MapType = mapInfo.MapType;
            TileTexelSize = mapInfo.TileTexelSize;
            TileSetInfo = tileSetInfo;

            Position = LayerOffset(layerInfo.TileCoordOffset);
            ContentSize = LayerSize.Size * TileTexelSize * CCTileMapLayer.DefaultTexelToContentSizeRatios;

            tileSetTexture = texture;
            tileGIDAndFlagsArray = layerInfo.TileGIDAndFlags;

            TileSetInfo.TilesheetSize = tileSetTexture.ContentSizeInPixels;

            UpdateTileCoordsToNodeTransform();

            ParseInternalProperties();

            InitialiseDrawBuffers();
        }

        void ParseInternalProperties()
        {
            string vertexZStr = PropertyNamed("cc_vertexz");

            if (!String.IsNullOrEmpty(vertexZStr))
            {
                if (vertexZStr == "automatic")
                {
                    useAutomaticVertexZ = true;
                }
                else
                {
                    defaultTileVertexZ = CCUtils.CCParseInt(vertexZStr);
                }
            }
        }

        void InitialiseDrawBuffers()
        {
            drawBufferManager = new CCTileMapDrawBufferManager(LayerSize.Row, LayerSize.Column, tileGIDAndFlagsArray);

            // Initialize the QuadsVertexBuffers
            if (tileSetTexture.ContentSizeInPixels != CCSize.Zero)
            {
                for (int y = 0; y < LayerSize.Row; y++)
                {
                    for (int x = 0; x < LayerSize.Column; x++)
                    {
                        UpdateQuadAt(x, y, false);
                    }
                }
            }
        }

        #endregion Constructors

        protected override void AddedToScene()
        {
            base.AddedToScene();

            visibleTileRangeDirty = true;
        }

        protected override void VisibleBoundsChanged()
        {
            base.VisibleBoundsChanged();

            visibleTileRangeDirty = true;
        }

        protected override void ParentUpdatedTransform()
        {
            base.ParentUpdatedTransform();

            visibleTileRangeDirty = true;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) 
                drawBufferManager.Dispose();

            base.Dispose(disposing);
        }


        #region Drawing

        void UpdateTileCoordsToNodeTransform()
        {
            CCSize texToContentScaling = CCTileMapLayer.DefaultTexelToContentSizeRatios;
            float width = TileTexelSize.Width * texToContentScaling.Width;
            float height = TileTexelSize.Height * texToContentScaling.Height;

            float yOffset = (LayerSize.Row - 1) * height;

            tileCoordsToNodeTransformOdd = CCAffineTransform.Identity;

            switch (MapType)
            {
                case CCTileMapType.Ortho:
                    // Note: For an orthographic map, top-left represents the origin (0,0)
                    // Moving right increases the column
                    // Moving left increases the row
                    tileCoordsToNodeTransform = new CCAffineTransform(new Matrix
                        (
                            width, 0.0f, 0.0f, 0.0f,
                            0.0f, -height, 0.0f, 0.0f,
                            0.0f, 0.0f, 1.0f, 0.0f,
                            0.0f, yOffset, 0.0f, 1.0f
                        ));
                    break;
                case CCTileMapType.Iso:
                    // Note: For an isometric map, top-right tile represents the origin (0,0)
                    // Moving left increases the column
                    // Moving right increases the row
                    float xOffset = (LayerSize.Column - 1) * (width / 2);
                    tileCoordsToNodeTransform = new CCAffineTransform(new Matrix
                        (
                            width / 2 , -height / 2, 0.0f, 0.0f,
                            -width/ 2, -height / 2, 0.0f, 0.0f,
                            0.0f, 0.0f, 1.0f, 0.0f,
                            xOffset, yOffset, 0.0f, 1.0f
                        ));
                    break;
                case CCTileMapType.Hex:
                    tileCoordsToNodeTransform = new CCAffineTransform(new Matrix
                        (
                            height * (float)Math.Sqrt(0.75), 0.0f, 0.0f, 0.0f,
                            0.0f , -height, 0.0f, 0.0f,
                            0.0f, 0.0f, 1.0f, 0.0f,
                            0.0f, yOffset, 0.0f, 1.0f
                        ));

                    tileCoordsToNodeTransformOdd = new CCAffineTransform(new Matrix
                        (
                            height * (float)Math.Sqrt(0.75), -height/2, 0.0f, 0.0f,
                            0.0f , -height, 0.0f, 0.0f,
                            0.0f, 0.0f, 1.0f, 0.0f,
                            0.0f, yOffset, 0.0f, 1.0f
                        ));
                    break;
                case CCTileMapType.Staggered:
                    tileCoordsToNodeTransform = new CCAffineTransform(new Matrix
                        (
                            width, 0.0f, 0.0f, 0.0f,
                            0.0f , -height / 2, 0.0f, 0.0f,
                            0.0f, 0.0f, 1.0f, 0.0f,
                            0.0f, yOffset / 2, 0.0f, 1.0f
                        ));

                    tileCoordsToNodeTransformOdd = new CCAffineTransform(new Matrix
                        (
                            width, 0.0f, 0.0f, 0.0f,
                            0.0f , -height /2, 0.0f, 0.0f,
                            0.0f, 0.0f, 1.0f, 0.0f,
                            width / 2, yOffset / 2, 0.0f, 1.0f
                        ));
                    break;
                default:
                    tileCoordsToNodeTransform = CCAffineTransform.Identity;
                    break;
            }

            nodeToTileCoordsTransform = tileCoordsToNodeTransform.Inverse;
            nodeToTileCoordsTransformOdd = tileCoordsToNodeTransformOdd.Inverse;
        }

        protected override void Draw()
        {
            if(visibleTileRangeDirty)
                UpdateVisibleTileRange();

            base.Draw();

            CCDrawManager drawManager = Window.DrawManager;

            var alphaTest = drawManager.AlphaTestEffect;

            alphaTest.AlphaFunction = CompareFunction.Greater;
            alphaTest.ReferenceAlpha = 0;

            drawManager.PushEffect(alphaTest);
            drawManager.BindTexture(tileSetTexture);

            foreach (var buffer in drawBufferManager.Buffers)
            {
                if (buffer.ShouldDrawBuffer == false)
                    continue;

                drawManager.DrawRawBuffer(buffer.QuadsVertexBuffer, 
                    buffer.StartingQuadVertIndex, buffer.NumberOfVisibleVertices,
                    buffer.IndexBuffer, 0, buffer.NumberOfVisiblePrimitives);
            }

            drawManager.PopEffect();
        }

        #endregion Drawing


        #region Convenience methods 

        bool AreValidTileCoordinates(int xCoord, int yCoord)
        {
            bool isValid = xCoord < LayerSize.Column && yCoord < LayerSize.Row && xCoord >= 0 && yCoord >= 0;

            Debug.Assert(isValid, String.Format("CCTileMapLayer: Invalid tile coordinates x: {0} y: {1}", xCoord, yCoord));

            return isValid;
        }

        bool AreValidTileCoordinates(CCTileMapCoordinates tileCoords)
        {
            return AreValidTileCoordinates(tileCoords.Column, tileCoords.Row);
        }

        int FlattenedTileIndex(int col, int row)
        {
            return (int)(col + (row * (LayerSize.Column)));
        }

        int FlattenedTileIndex(CCTileMapCoordinates tileCoords)
        {
            return FlattenedTileIndex(tileCoords.Column, tileCoords.Row);
        }

        #endregion Convenience methods


        #region Fetching tile

        public CCSprite ExtractTile(CCTileMapCoordinates tileCoords, bool addToTileMapLayer = true)
        {
            return ExtractTile(tileCoords.Column, tileCoords.Row, addToTileMapLayer);
        }

        public CCSprite ExtractTile(int column, int row, bool addToTileMapLayer = true)
        {
            if (!AreValidTileCoordinates(column, row))
                return null;

            CCTileGidAndFlags gidAndFlags = TileGIDAndFlags(column, row);
            int flattendedIndex = FlattenedTileIndex(column, row);

            CCRect texRect = TileSetInfo.TextureRectForGID(gidAndFlags.Gid);
            CCSprite tileSprite = new CCSprite(tileSetTexture, texRect);
            tileSprite.ContentSize = texRect.Size * CCTileMapLayer.DefaultTexelToContentSizeRatios;
            tileSprite.Position = TilePosition(column, row);
            tileSprite.VertexZ = TileVertexZ(column, row);
            tileSprite.AnchorPoint = CCPoint.Zero;
            tileSprite.Opacity = Opacity;
            tileSprite.FlipX = false;
            tileSprite.FlipY = false;
            tileSprite.Rotation = 0.0f;

            if ((gidAndFlags.Flags & CCTileFlags.TileDiagonal) != 0)
            {
                CCSize halfContentSize = tileSprite.ContentSize * 0.5f;

                tileSprite.AnchorPoint = CCPoint.AnchorMiddle;
                tileSprite.Position += new CCPoint(halfContentSize.Width, halfContentSize.Height);

                CCTileFlags horAndVertFlag = gidAndFlags.Flags & (CCTileFlags.Horizontal | CCTileFlags.Vertical);

                // Handle the 4 diagonally flipped states.
                if (horAndVertFlag == CCTileFlags.Horizontal)
                {
                    tileSprite.Rotation = 90.0f;
                }
                else if (horAndVertFlag == CCTileFlags.Vertical)
                {
                    tileSprite.Rotation = 270.0f;
                }
                else if (horAndVertFlag == (CCTileFlags.Vertical | CCTileFlags.Horizontal))
                {
                    tileSprite.Rotation = 90.0f;
                    tileSprite.FlipX = true;
                }
                else
                {
                    tileSprite.Rotation = 270.0f;
                    tileSprite.FlipX = true;
                }
            }
            else
            {
                if ((gidAndFlags.Flags & CCTileFlags.Horizontal) != 0)
                {
                    tileSprite.FlipX = true;
                }

                if ((gidAndFlags.Flags & CCTileFlags.Vertical) != 0)
                {
                    tileSprite.FlipY = true;
                }
            }

            if(addToTileMapLayer)
            {
                AddChild(tileSprite, flattendedIndex, flattendedIndex);
            }

            RemoveTile(column, row);

            return tileSprite;
        }

        #endregion Fetching tile


        #region Fetching tile properties 

        public CCTileMapCoordinates ClosestTileCoordAtNodePosition(CCPoint nodePos)
        {
            // Tile positions are relative to bottom-left corner of quad
            // However the tile hit test should be relative to tile center
            // Therefore adjust node position
            CCSize offsetSize = (TileContentSize) * 0.5f;
            CCPoint offsetPt = new CCPoint(offsetSize.Width, offsetSize.Height);
            CCPoint offsetDiff = nodePos - offsetPt;
            CCPoint transformedPoint = nodeToTileCoordsTransform.Transform(offsetDiff).RoundToInteger();

            if (MapType == CCTileMapType.Hex || MapType == CCTileMapType.Staggered) 
            {
                CCPoint oddTransformedPoint = nodeToTileCoordsTransformOdd.Transform (offsetDiff).RoundToInteger ();

                if ((MapType == CCTileMapType.Hex && oddTransformedPoint.X % 2 == 1) ||
                    (MapType == CCTileMapType.Staggered && oddTransformedPoint.Y % 2 == 1))
                    transformedPoint = oddTransformedPoint;
            }

            return new CCTileMapCoordinates((int)transformedPoint.X, (int)transformedPoint.Y);
        }

        public CCTileGidAndFlags TileGIDAndFlags(CCTileMapCoordinates tileCoords)
        {
            return TileGIDAndFlags(tileCoords.Column, tileCoords.Row);
        }

        public CCTileGidAndFlags TileGIDAndFlags(int column, int row)
        {
            CCTileGidAndFlags tileGIDAndFlags = new CCTileGidAndFlags(0,0);

            if(AreValidTileCoordinates(column, row))
            {
                int flattenedIndex = FlattenedTileIndex(column, row);

                tileGIDAndFlags = tileGIDAndFlagsArray[flattenedIndex];
            }

            return tileGIDAndFlags;
        }

        public CCPoint TilePosition(int column, int row)
        {
            return TilePosition(new CCTileMapCoordinates(column, row));
        }

        public CCPoint TilePosition(CCTileMapCoordinates tileCoords)
        {
            if((MapType == CCTileMapType.Hex && tileCoords.Column % 2 == 1) ||
                (MapType == CCTileMapType.Staggered && tileCoords.Row % 2 == 1))
                return tileCoordsToNodeTransformOdd.Transform(tileCoords.Point);

            return tileCoordsToNodeTransform.Transform(tileCoords.Point);
        }

        public float TileVertexZ(int column, int row)
        {
            float vertexZ = 0;

            if (useAutomaticVertexZ)
            {
                switch (MapType)
                {
                case CCTileMapType.Iso:
                    float maxVal = LayerSize.Column + LayerSize.Row;
                    vertexZ = -(maxVal - (column + row));
                    break;
                case CCTileMapType.Ortho:
                    vertexZ = -(LayerSize.Row - row);
                    break;
                case CCTileMapType.Staggered:
                    vertexZ = -(LayerSize.Row - row);
                    break;
                case CCTileMapType.Hex:
                    Debug.Assert(false,"CCTMXLayer:TileVertexZ: Automatic z-ordering for Hex tiles not supported");
                    break;
                default:
                    Debug.Assert(false, "CCTMXLayer:TileVertexZ: Unsupported layer orientation");
                    break;
                }
            }
            else
            {
                vertexZ = defaultTileVertexZ;
            }

            return vertexZ;
        }

        public float TileVertexZ(CCTileMapCoordinates tileCoords)
        {
            return TileVertexZ(tileCoords.Column, tileCoords.Row);
        }

        String PropertyNamed(string propertyName)
        {
            string property = String.Empty;

            LayerProperties.TryGetValue(propertyName, out property);

            return property;
        }

        CCPoint LayerOffset(CCPoint offsetInTileCoords)
        {
            CCPoint offsetInNodespace = CCPoint.Zero;
            switch (MapType)
            {
            case CCTileMapType.Ortho:
                offsetInNodespace = new CCPoint(offsetInTileCoords.X * TileTexelSize.Width, -offsetInTileCoords.Y * TileTexelSize.Height);
                break;
            case CCTileMapType.Iso:
                offsetInNodespace = new CCPoint((TileTexelSize.Width / 2) * (offsetInTileCoords.X - offsetInTileCoords.Y),
                    (TileTexelSize.Height / 2) * (-offsetInTileCoords.X - offsetInTileCoords.Y));
                break;
            case CCTileMapType.Staggered:
                float diffX = 0;
                if ((int)offsetInTileCoords.Y % 2 == 1)
                    diffX = TileTexelSize.Width / 2;

                offsetInNodespace = new CCPoint(offsetInTileCoords.X * TileTexelSize.Width + diffX, 
                    -offsetInTileCoords.Y * TileTexelSize.Height/2);
                break;
            case CCTileMapType.Hex:
                Debug.Assert(offsetInTileCoords.Equals(CCPoint.Zero), "offset for hexagonal map not implemented yet");
                break;
            }

            offsetInNodespace *= CCTileMapLayer.DefaultTexelToContentSizeRatios;

            return offsetInNodespace;
        }

        #endregion Fetching tile properties 


        #region Removing tiles

        public void RemoveTile(int column, int row)
        {
            if (AreValidTileCoordinates(column, row))
            {
                CCTileGidAndFlags gidAndFlags = TileGIDAndFlags(column, row);

                if(gidAndFlags.Gid != 0) 
                {
                    // Remove tile from GID map
                    SetBatchRenderedTileGID(column, row, CCTileGidAndFlags.EmptyTile);
                }
            }
        }

        public void RemoveTile(CCTileMapCoordinates tileCoords)
        {
            RemoveTile(tileCoords.Column, tileCoords.Row);
        }

        #endregion Removing tiles


        #region Updating tiles

        public void SetTileGID(CCTileGidAndFlags gidAndFlags, CCTileMapCoordinates tileCoords)
        {
            if (gidAndFlags.Gid == 0)
            {
                RemoveTile (tileCoords);
                return;
            }

            if (gidAndFlags.Gid < TileSetInfo.FirstGid)
            {
                Debug.Assert (false, String.Format("CCTileMapLayer: SetTileGID: Invalid GID {0}", gidAndFlags.Gid));
                return;
            }

            if (AreValidTileCoordinates(tileCoords) == false)
            {
                Debug.Assert (false, String.Format("CCTileMapLayer: Invalid tile coordinates row: {0} column: {1}", 
                    tileCoords.Row, tileCoords.Column));
                return;
            }

            CCTileGidAndFlags currentGID = TileGIDAndFlags(tileCoords);

            if(currentGID == gidAndFlags)
                return;

            SetBatchRenderedTileGID(tileCoords.Column, tileCoords.Row, gidAndFlags);
        }

        void SetBatchRenderedTileGID(int column, int row, CCTileGidAndFlags gidAndFlags)
        {
            int flattenedIndex = FlattenedTileIndex(column, row);
            CCTileGidAndFlags prevGid = tileGIDAndFlagsArray[flattenedIndex];

            if(gidAndFlags != prevGid)
            {
                tileGIDAndFlagsArray[flattenedIndex] = gidAndFlags;
                UpdateQuadAt(column, row);
            }
        }

        #endregion Updating tiles


        #region Updating buffers

        void UpdateQuadAt(CCTileMapCoordinates tileCoords, bool updateBuffer = true)
        {
            UpdateQuadAt(tileCoords.Column, tileCoords.Row, updateBuffer);
        }

        void UpdateQuadAt(int tileCoordX, int tileCoordY, bool updateBuffer = true)
        {
            int flattenedTileIndex = FlattenedTileIndex(tileCoordX, tileCoordY);
            CCTileGidAndFlags tileGID = tileGIDAndFlagsArray[flattenedTileIndex];

            CCTileMapVertAndIndexBuffer drawBuffer = drawBufferManager.GetDrawBufferAtIndex(flattenedTileIndex);
            int adjustedTileIndex = flattenedTileIndex - drawBuffer.TileStartIndex;

            if (tileGID.Gid == 0)
            {
                drawBuffer.QuadsVertexBuffer[adjustedTileIndex] = new CCV3F_C4B_T2F_Quad();
                return;
            }

            float left, right, top, bottom, vertexZ;
            vertexZ = TileVertexZ(tileCoordX, tileCoordY);


            CCSize tileSize = TileSetInfo.TileTexelSize * CCTileMapLayer.DefaultTexelToContentSizeRatios;
            CCSize texSize = TileSetInfo.TilesheetSize;
            CCPoint tilePos = TilePosition(tileCoordX, tileCoordY);

            var quad = drawBuffer.QuadsVertexBuffer[adjustedTileIndex];

            // vertices
            if ((tileGID.Flags & CCTileFlags.TileDiagonal) != 0)
            {
                left = tilePos.X;
                right = tilePos.X + tileSize.Height;
                bottom = tilePos.Y + tileSize.Width;
                top = tilePos.Y;
            }
            else
            {
                left = tilePos.X;
                right = tilePos.X + tileSize.Width;
                bottom = tilePos.Y + tileSize.Height;
                top = tilePos.Y;
            }

            float temp;
            if((tileGID.Flags & CCTileFlags.Vertical) !=0 )
            {
                temp = top;
                top = bottom;
                bottom = temp;
            }

            if ((tileGID.Flags & CCTileFlags.Horizontal) != 0)
            {
                temp = left;
                left = right;
                right = temp;
            }

            if((tileGID.Flags & CCTileFlags.TileDiagonal) != 0)
            {
                // FIXME: not working correcly
                quad.BottomLeft.Vertices = new CCVertex3F(left, bottom, vertexZ);
                quad.BottomRight.Vertices = new CCVertex3F(left, top, vertexZ);
                quad.TopLeft.Vertices = new CCVertex3F(right, bottom, vertexZ);
                quad.TopRight.Vertices = new CCVertex3F(right, top, vertexZ);
            }
            else
            {
                quad.BottomLeft.Vertices = new CCVertex3F(left, bottom, vertexZ);
                quad.BottomRight.Vertices = new CCVertex3F(right, bottom, vertexZ);
                quad.TopLeft.Vertices = new CCVertex3F(left, top, vertexZ);
                quad.TopRight.Vertices = new CCVertex3F(right, top, vertexZ);
            }

            // texcoords
            CCRect tileTexture = TileSetInfo.TextureRectForGID(tileGID.Gid);
            left   = ((tileTexture.Origin.X ) / texSize.Width) + 0.5f / texSize.Width;
            right  = left + ((tileTexture.Size.Width) / texSize.Width) - 1.0f / texSize.Width;
            bottom = ((tileTexture.Origin.Y) / texSize.Height) + 0.5f / texSize.Height;
            top    = bottom + ((tileTexture.Size.Height) / texSize.Height) - 1.0f / texSize.Height;

            quad.BottomLeft.TexCoords = new CCTex2F(left, bottom);
            quad.BottomRight.TexCoords = new CCTex2F(right, bottom);
            quad.TopLeft.TexCoords = new CCTex2F(left, top);
            quad.TopRight.TexCoords = new CCTex2F(right, top);

            quad.BottomLeft.Colors = CCColor4B.White;
            quad.BottomRight.Colors = CCColor4B.White;
            quad.TopLeft.Colors = CCColor4B.White;
            quad.TopRight.Colors = CCColor4B.White;

            drawBuffer.QuadsVertexBuffer[adjustedTileIndex] = quad;
        }

        void UpdateVisibleTileRange()
        {
            var culledBounds = AffineWorldTransform.Inverse.Transform(VisibleBoundsWorldspace);
            culledBounds = culledBounds.Intersection(BoundingBox);

            CCSize tileSize = TileTexelSize * CCTileMapLayer.DefaultTexelToContentSizeRatios;
            float tileSizeMax = Math.Max(tileSize.Width, tileSize.Height);

            CCRect visibleTiles = nodeToTileCoordsTransform.Transform(culledBounds);
            visibleTiles = visibleTiles.IntegerRoundedUpRect();
            visibleTiles.Origin.Y += 1;
            visibleTiles.Origin.X -= 1;

            int tilesOverX = 0;
            int tilesOverY = 0;

            CCRect overTileRect = new CCRect(0.0f, 0.0f, tileSizeMax - tileSize.Width, tileSizeMax - tileSize.Height);
            overTileRect = nodeToTileCoordsTransform.Transform(overTileRect);

            tilesOverX = (int)(Math.Ceiling (overTileRect.Origin.X + overTileRect.Size.Width) - Math.Floor(overTileRect.Origin.X));
            tilesOverY = (int)(Math.Ceiling (overTileRect.Origin.Y + overTileRect.Size.Height) - Math.Floor(overTileRect.Origin.Y));

            int yBegin = (int)Math.Max(0, visibleTiles.Origin.Y - tilesOverY);
            int yEnd = (int)Math.Min(LayerSize.Row - 1, visibleTiles.Origin.Y + visibleTiles.Size.Height + tilesOverY);
            int xBegin = (int)Math.Max(0, visibleTiles.Origin.X - tilesOverX);
            int xEnd = (int)Math.Min(LayerSize.Column - 1, visibleTiles.Origin.X + visibleTiles.Size.Width + tilesOverX);

            drawBufferManager.StartUpdateVisibleTiles(FlattenedTileIndex(new CCTileMapCoordinates(xBegin, yBegin)));

            for (int y =  yBegin; y <= yEnd; ++y)
            {
                CCTileMapCoordinates startCoord = new CCTileMapCoordinates (xBegin, y); 
                CCTileMapCoordinates endCoord = new CCTileMapCoordinates (xEnd, y);
                drawBufferManager.AddVisibleTileRange (FlattenedTileIndex(startCoord), FlattenedTileIndex(endCoord));
            }

            drawBufferManager.EndUpdateVisibleTiles();

            visibleTileRangeDirty = false;
        }

        #endregion Updating buffers


        #region Internal draw buffer management

        class CCTileMapDrawBufferManager : IDisposable
        {
            public List<CCTileMapVertAndIndexBuffer> Buffers { get; private set; }
            CCTileGidAndFlags[] TileGIDAndFlagsArray { get; set; }

            public CCTileMapDrawBufferManager(int mapCols, int mapRows, CCTileGidAndFlags[] gidAndFlags)
            {
                Buffers = new List<CCTileMapVertAndIndexBuffer>();
                TileGIDAndFlagsArray = gidAndFlags;

                // Create the appropriate number of buffers to hold the map.
                int numOfTiles = mapCols * mapRows;
                int tilesProcessed = 0;
                while ( tilesProcessed < numOfTiles )
                {
                    int tilesLeft = numOfTiles - tilesProcessed;
                    int tilesInThisBuffer = Math.Min( tilesLeft, MaxTilesPerDrawBuffer );

                    CreateDrawBuffer( tilesInThisBuffer, tilesProcessed );

                    tilesProcessed += tilesInThisBuffer;
                }
            }

            public CCTileMapVertAndIndexBuffer GetDrawBufferAtIndex(int tileIndex)
            {
                foreach (var buffer in Buffers)
                {
                    if(tileIndex >= buffer.TileStartIndex && tileIndex <= buffer.TileEndIndex)
                        return buffer;
                }
                throw new Exception(String.Format("No DrawBuffer found for specified tile index: {0}", tileIndex));
            }

            public void StartUpdateVisibleTiles(int startTileIndex)
            {
                foreach(var buffer in Buffers)
                    buffer.StartUpdateIndexBuffer(startTileIndex);
            }

            public void AddVisibleTileRange(int startTileIndex, int endTileIndex)
            {
                foreach(var buffer in Buffers)
                    buffer.AddTileRange(startTileIndex, endTileIndex); 
            }

            public void EndUpdateVisibleTiles()
            {
                foreach(var buffer in Buffers)
                    buffer.EndUpdateIndexBuffer();
            }

            ~CCTileMapDrawBufferManager() 
            {
                Dispose(false);
            }

            public void Dispose()
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }

            protected virtual void Dispose(bool disposing)
            {
                if (disposing) 
                {
                    while (Buffers.Count > 0) 
                    {
                        CCTileMapVertAndIndexBuffer buffer = Buffers [0];
                        Buffers.RemoveAt (0);
                        buffer.QuadsVertexBuffer = null;
                        buffer.IndexBuffer = null;
                    }
                }
            }

            void CreateDrawBuffer(int tileCount, int firstTileId)
            {
                int quadsInThisBuffer = tileCount * NumOfCornersPerQuad;
                int verticesInThisBuffer = tileCount * NumOfVerticesPerQuad;

                var buffer = new CCTileMapVertAndIndexBuffer
                {
                    TileStartIndex    = firstTileId,
                    TileEndIndex      = firstTileId + tileCount - 1,
                    QuadsVertexBuffer = new CCV3F_C4B_T2F_Quad[quadsInThisBuffer],
                    IndexBuffer       = new short[verticesInThisBuffer],
                    TileGIDAndFlagsArray = TileGIDAndFlagsArray
                };

                Buffers.Add(buffer);
            }

        }

        class CCTileMapVertAndIndexBuffer
        {
            #region Properties

            public int TileStartIndex { get; set; }
            public int TileEndIndex { get; set; }
            public CCV3F_C4B_T2F_Quad[] QuadsVertexBuffer { get; set; }
            public short[] IndexBuffer { get; set; }
            public CCTileGidAndFlags[] TileGIDAndFlagsArray { get; set; }

            public bool ShouldDrawBuffer { get; private set; }
            public int IndexBufferSize { get; private set; }
            public int StartingQuadVertIndex { get; private set; }
            public int NumberOfVisibleVertices { get; private set; }
            public int NumberOfVisiblePrimitives { get; private set; }

            public int NumberOfTiles
            {
                get { return TileEndIndex - TileStartIndex + 1; }
            }

            #endregion Properties


            public void StartUpdateIndexBuffer(int startTileIndex) 
            { 
                IndexBufferSize = 0; 
                NumberOfVisiblePrimitives = 0;
                NumberOfVisibleVertices = 0;
                StartingQuadVertIndex = startTileIndex > TileStartIndex ? startTileIndex - TileStartIndex : 0;
                ShouldDrawBuffer = false;
            }

            public void AddTileRange(int startTileIndex, int endTileIndex)
            {
                startTileIndex = Math.Max (startTileIndex, TileStartIndex);
                endTileIndex = Math.Min (endTileIndex, TileEndIndex);

                if (endTileIndex >= startTileIndex)
                    ShouldDrawBuffer = true;
                else
                    return;

                for (int tileIndex = startTileIndex; tileIndex <= endTileIndex; tileIndex++)
                {
                    if (TileGIDAndFlagsArray[tileIndex].Gid == 0)
                        continue;

                    int quadVertIndex = (tileIndex - TileStartIndex) * NumOfCornersPerQuad - StartingQuadVertIndex;
                    int indexBufferOffset = IndexBufferSize;

                    IndexBuffer[indexBufferOffset + 0] = (short)(quadVertIndex + 0);
                    IndexBuffer[indexBufferOffset + 1] = (short)(quadVertIndex + 1);
                    IndexBuffer[indexBufferOffset + 2] = (short)(quadVertIndex + 2);
                    IndexBuffer[indexBufferOffset + 3] = (short)(quadVertIndex + 3);
                    IndexBuffer[indexBufferOffset + 4] = (short)(quadVertIndex + 2);
                    IndexBuffer[indexBufferOffset + 5] = (short)(quadVertIndex + 1);

                    IndexBufferSize += NumOfVerticesPerQuad;
                    NumberOfVisiblePrimitives += NumOfPrimitivesPerQuad;
                    NumberOfVisibleVertices += NumOfCornersPerQuad;
                }
            }

            public void EndUpdateIndexBuffer()
            {
                // No op for now
            }
        }

        #endregion Internal draw buffer management
    }
}