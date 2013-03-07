using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace cocos2d
{
    public class CCTMXLayer : CCSpriteBatchNode
    {
        /** size of the layer in tiles */

        protected bool m_bUseAutomaticVertexZ;
        protected byte m_cOpacity;
        protected float m_fAlphaFuncValue;
        protected float m_fContentScaleFactor;
        protected int m_nVertexZvalue;
        protected List<int> m_pAtlasIndexArray;
        private Dictionary<string, string> m_pProperties;
        protected CCSprite m_pReusedTile;
        private CCTMXTilesetInfo m_pTileSet;
        private uint[] m_pTiles;
        protected string m_sLayerName;
        private CCSize m_tLayerSize;

        /** size of the map's tile (could be differnt from the tile's size) */

        private CCSize m_tMapTileSize;
        private CCTMXOrientation m_uLayerOrientation;
        protected uint m_uMaxGID;
        protected uint m_uMinGID;

        public CCSize LayerSize
        {
            get { return m_tLayerSize; }
            set { m_tLayerSize = value; }
        }

        public CCSize MapTileSize
        {
            get { return m_tMapTileSize; }
            set { m_tMapTileSize = value; }
        }

        /** pointer to the map of tiles */

        public uint[] Tiles
        {
            get { return m_pTiles; }
            set { m_pTiles = value; }
        }

        /** Tilset information for the layer */

        public CCTMXTilesetInfo TileSet
        {
            get { return m_pTileSet; }
            set { m_pTileSet = value; }
        }

        /** Layer orientation, which is the same as the map orientation */

        public CCTMXOrientation LayerOrientation
        {
            get { return m_uLayerOrientation; }
            set { m_uLayerOrientation = value; }
        }

        /** properties from the layer. They can be added using Tiled */

        public Dictionary<string, string> Properties
        {
            get { return m_pProperties; }
            set { m_pProperties = value; }
        }

        public string LayerName
        {
            get { return m_sLayerName; }
            set { m_sLayerName = value; }
        }

        /** creates a CCTMXLayer with an tileset info, a layer info and a map info */

        public static CCTMXLayer Create(CCTMXTilesetInfo tilesetInfo, CCTMXLayerInfo layerInfo, CCTMXMapInfo mapInfo)
        {
            var pRet = new CCTMXLayer();
            pRet.InitWithTilesetInfo(tilesetInfo, layerInfo, mapInfo);
            return pRet;
        }

        /** initializes a CCTMXLayer with a tileset info, a layer info and a map info */

        public bool InitWithTilesetInfo(CCTMXTilesetInfo tilesetInfo, CCTMXLayerInfo layerInfo, CCTMXMapInfo mapInfo)
        {
            // XXX: is 35% a good estimate ?
            CCSize size = layerInfo.m_tLayerSize;
            float totalNumberOfTiles = size.Width * size.Height;
            float capacity = totalNumberOfTiles * 0.35f + 1; // 35 percent is occupied ?

            CCTexture2D texture = null;
            if (tilesetInfo != null)
            {
                texture = CCTextureCache.SharedTextureCache.AddImage(tilesetInfo.m_sSourceImage);
            }

            if (base.InitWithTexture(texture, (int) capacity))
            {
                // layerInfo
                m_sLayerName = layerInfo.m_sName;
                m_tLayerSize = size;
                m_pTiles = layerInfo.m_pTiles;
                m_uMinGID = layerInfo.m_uMinGID;
                m_uMaxGID = layerInfo.m_uMaxGID;
                m_cOpacity = layerInfo.m_cOpacity;
                Properties = new Dictionary<string, string>(layerInfo.Properties);
                m_fContentScaleFactor = CCDirector.SharedDirector.ContentScaleFactor;

                // tilesetInfo
                m_pTileSet = tilesetInfo;

                // mapInfo
                m_tMapTileSize = mapInfo.TileSize;
                m_uLayerOrientation = (CCTMXOrientation) mapInfo.Orientation;

                // offset (after layer orientation is set);
                CCPoint offset = CalculateLayerOffset(layerInfo.m_tOffset);
                Position = ccMacros.CC_POINT_PIXELS_TO_POINTS(offset);

                m_pAtlasIndexArray = new List<int>((int) totalNumberOfTiles);

                ContentSize =
                    ccMacros.CC_SIZE_PIXELS_TO_POINTS(new CCSize(m_tLayerSize.Width * m_tMapTileSize.Width,
                                                                 m_tLayerSize.Height * m_tMapTileSize.Height));

                m_bUseAutomaticVertexZ = false;
                m_nVertexZvalue = 0;

                return true;
            }
            return false;
        }

        /** dealloc the map that contains the tile position from memory.
        Unless you want to know at runtime the tiles positions, you can safely call this method.
        If you are going to call layer.tileGIDAt() then, don't release the map
        */

        public void ReleaseMap()
        {
            m_pTiles = null;
            m_pAtlasIndexArray = null;
        }

        /** returns the tile (CCSprite) at a given a tile coordinate.
        The returned CCSprite will be already added to the CCTMXLayer. Don't add it again.
        The CCSprite can be treated like any other CCSprite: rotated, scaled, translated, opacity, color, etc.
        You can remove either by calling:
        - layer.removeChild(sprite, cleanup);
        - or layer.removeTileAt(ccp(x,y));
        */

        public CCSprite TileAt(CCPoint pos)
        {
            Debug.Assert(pos.x < m_tLayerSize.Width && pos.y < m_tLayerSize.Height && pos.x >= 0 && pos.y >= 0, "TMXLayer: invalid position");
            Debug.Assert(m_pTiles != null && m_pAtlasIndexArray != null, "TMXLayer: the tiles map has been released");

            CCSprite tile = null;
            uint gid = TileGIDAt(pos);

            // if GID == 0, then no tile is present
            if (gid != 0)
            {
                var z = (int) (pos.x + pos.y * m_tLayerSize.Width);
                tile = (CCSprite) GetChildByTag(z);

                // tile not created yet. create it
                if (tile == null)
                {
                    CCRect rect = m_pTileSet.RectForGID(gid);
                    rect = ccMacros.CC_RECT_PIXELS_TO_POINTS(rect);

                    tile = new CCSprite();
                    //
                    // do the init AFTER the batch node is set so that the tile is set to 
                    // draw in batch mode instead of self draw mode.
                    //
                    tile.InitWithTexture(Texture, rect);
                    tile.BatchNode = this;
                    tile.Position = PositionAt(pos);
                    tile.VertexZ = VertexZForPos(pos);
                    tile.AnchorPoint = CCPoint.Zero;
                    tile.Opacity = m_cOpacity;
//                    tile.InitWithTexture(Texture, rect);
//                    tile.BatchNode = this;

                    int indexForZ = AtlasIndexForExistantZ(z);
                    AddSpriteWithoutQuad(tile, indexForZ, z);
                }
            }

            return tile;
        }

        /** returns the tile gid at a given tile coordinate.
        if it returns 0, it means that the tile is empty.
        This method requires the the tile map has not been previously released (eg. don't call layer.releaseMap())
        */

        public uint TileGIDAt(CCPoint pos)
        {
            uint tmp;
            return TileGIDAt(pos, out tmp);
        }


        /** returns the tile gid at a given tile coordinate. It also returns the tile flags.
         This method requires the the tile map has not been previously released (eg. don't call [layer releaseMap])
         */

        public uint TileGIDAt(CCPoint pos, out uint flags)
        {
            Debug.Assert(pos.x < m_tLayerSize.Width && pos.y < m_tLayerSize.Height && pos.x >= 0 && pos.y >= 0, "TMXLayer: invalid position");
            Debug.Assert(m_pTiles != null && m_pAtlasIndexArray != null, "TMXLayer: the tiles map has been released");

            var idx = (int) (pos.x + pos.y * m_tLayerSize.Width);
            // Bits on the far end of the 32-bit global tile ID are used for tile flags
            uint tile = m_pTiles[idx];

            // issue1264, flipped tiles can be changed dynamically
            flags = (tile & ccTMXTileFlags.kCCFlipedAll);

            return (tile & ccTMXTileFlags.kCCFlippedMask);
        }

        /** sets the tile gid (gid = tile global id) at a given tile coordinate.
        The Tile GID can be obtained by using the method "tileGIDAt" or by using the TMX editor . Tileset Mgr +1.
        If a tile is already placed at that position, then it will be removed.
        */

        public void SetTileGID(uint gid, CCPoint pos)
        {
            SetTileGID(gid, pos, 0);
        }

        /** sets the tile gid (gid = tile global id) at a given tile coordinate.
         The Tile GID can be obtained by using the method "tileGIDAt" or by using the TMX editor . Tileset Mgr +1.
         If a tile is already placed at that position, then it will be removed.
     
         Use withFlags if the tile flags need to be changed as well
         */

        public void SetTileGID(uint gid, CCPoint pos, uint flags)
        {
            Debug.Assert(pos.x < m_tLayerSize.Width && pos.y < m_tLayerSize.Height && pos.x >= 0 && pos.y >= 0, "TMXLayer: invalid position");
            Debug.Assert(m_pTiles != null && m_pAtlasIndexArray != null, "TMXLayer: the tiles map has been released");
            Debug.Assert(gid == 0 || gid >= m_pTileSet.m_uFirstGid, "TMXLayer: invalid gid");

            uint currentFlags;
            uint currentGID = TileGIDAt(pos, out currentFlags);

            if (currentGID != gid || currentFlags != flags)
            {
                uint gidAndFlags = gid | flags;

                // setting gid=0 is equal to remove the tile
                if (gid == 0)
                {
                    RemoveTileAt(pos);
                }
                    // empty tile. create a new one
                else if (currentGID == 0)
                {
                    InsertTileForGID(gidAndFlags, pos);
                }
                    // modifying an existing tile with a non-empty tile
                else
                {
                    var z = (int) (pos.x + pos.y * m_tLayerSize.Width);
                    var sprite = (CCSprite) GetChildByTag(z);
                    if (sprite != null)
                    {
                        CCRect rect = m_pTileSet.RectForGID(gid);
                        rect = ccMacros.CC_RECT_PIXELS_TO_POINTS(rect);

                        sprite.SetTextureRect(rect, false, rect.size);
                        if (flags != 0)
                        {
                            SetupTileSprite(sprite, sprite.Position, gidAndFlags);
                        }
                        m_pTiles[z] = gidAndFlags;
                    }
                    else
                    {
                        UpdateTileForGID(gidAndFlags, pos);
                    }
                }
            }
        }

        /** removes a tile at given tile coordinate */

        public void RemoveTileAt(CCPoint pos)
        {
            Debug.Assert(pos.x < m_tLayerSize.Width && pos.y < m_tLayerSize.Height && pos.x >= 0 && pos.y >= 0, "TMXLayer: invalid position");
            Debug.Assert(m_pTiles != null && m_pAtlasIndexArray != null, "TMXLayer: the tiles map has been released");

            uint gid = TileGIDAt(pos);

            if (gid != 0)
            {
                var z = (int) (pos.x + pos.y * m_tLayerSize.Width);
                int atlasIndex = AtlasIndexForExistantZ(z);

                // remove tile from GID map
                m_pTiles[z] = 0;

                // remove tile from atlas position array
                m_pAtlasIndexArray.RemoveAt(atlasIndex);

                // remove it from sprites and/or texture atlas
                var sprite = (CCSprite) GetChildByTag(z);
                if (sprite != null)
                {
                    base.RemoveChild(sprite, true);
                }
                else
                {
                    m_pobTextureAtlas.RemoveQuadAtIndex(atlasIndex);

                    // update possible children
                    if (m_pChildren != null && m_pChildren.count > 0)
                    {
                        CCNode[] elements = m_pChildren.Elements;
                        int count = m_pChildren.count;

                        for (int i = 0; i < count; i++)
                        {
                            var pChild = (CCSprite) elements[i];
                            int ai = pChild.AtlasIndex;
                            if (ai >= atlasIndex)
                            {
                                pChild.AtlasIndex = (ai - 1);
                            }
                        }
                    }
                }
            }
        }

        /** returns the position in points of a given tile coordinate */

        public CCPoint PositionAt(CCPoint pos)
        {
            CCPoint ret = CCPoint.Zero;
            switch (m_uLayerOrientation)
            {
                case CCTMXOrientation.CCTMXOrientationOrtho:
                    ret = PositionForOrthoAt(pos);
                    break;
                case CCTMXOrientation.CCTMXOrientationIso:
                    ret = PositionForIsoAt(pos);
                    break;
                case CCTMXOrientation.CCTMXOrientationHex:
                    ret = PositionForHexAt(pos);
                    break;
            }
            ret = ccMacros.CC_POINT_PIXELS_TO_POINTS(ret);
            return ret;
        }

        /** return the value for the specific property name */

        public String PropertyNamed(string propertyName)
        {
            if (m_pProperties.ContainsKey(propertyName))
            {
                return m_pProperties[propertyName];
            }
            else
            {
                return String.Empty;
            }
        }

        /** Creates the tiles */

        public void SetupTiles()
        {
            // Optimization: quick hack that sets the image size on the tileset
            m_pTileSet.m_tImageSize = m_pobTextureAtlas.Texture.ContentSizeInPixels;

            // By default all the tiles are aliased
            // pros:
            //  - easier to render
            // cons:
            //  - difficult to scale / rotate / etc.
            m_pobTextureAtlas.Texture.SetAliasTexParameters();

            //CFByteOrder o = CFByteOrderGetCurrent();

            // Parse cocos2d properties
            ParseInternalProperties();

            for (int y = 0; y < m_tLayerSize.Height; y++)
            {
                for (int x = 0; x < m_tLayerSize.Width; x++)
                {
                    var pos = (int) (x + m_tLayerSize.Width * y);
                    uint gid = m_pTiles[pos];

                    // gid are stored in little endian.
                    // if host is big endian, then swap
                    //if( o == CFByteOrderBigEndian )
                    //    gid = CFSwapInt32( gid );
                    /* We support little endian.*/

                    // XXX: gid == 0 -. empty tile
                    if (gid != 0)
                    {
                        AppendTileForGID(gid, new CCPoint(x, y));

                        // Optimization: update min and max GID rendered by the layer
                        m_uMinGID = Math.Min(gid, m_uMinGID);
                        m_uMaxGID = Math.Max(gid, m_uMaxGID);
                    }
                }
            }

            Debug.Assert(m_uMaxGID >= m_pTileSet.m_uFirstGid &&
                         m_uMinGID >= m_pTileSet.m_uFirstGid, "TMX: Only 1 tilset per layer is supported");
        }

        /** CCTMXLayer doesn't support adding a CCSprite manually.
        @warning addchild(z, tag); is not supported on CCTMXLayer. Instead of setTileGID.
        */

        public override void AddChild(CCNode child, int zOrder, int tag)
        {
            Debug.Assert(false, "addChild: is not supported on CCTMXLayer. Instead use setTileGID:at:/tileAt:");
        }

        // super method
        public override void RemoveChild(CCNode node, bool cleanup)
        {
            var sprite = (CCSprite) node;
            // allows removing nil objects
            if (sprite == null)
            {
                return;
            }

            Debug.Assert(m_pChildren.Contains(sprite), "Tile does not belong to TMXLayer");

            int atlasIndex = sprite.AtlasIndex;
            int zz = m_pAtlasIndexArray[atlasIndex];
            m_pTiles[zz] = 0;
            m_pAtlasIndexArray.RemoveAt(atlasIndex);
            base.RemoveChild(sprite, cleanup);
        }

        private CCPoint PositionForIsoAt(CCPoint pos)
        {
            var xy = new CCPoint(m_tMapTileSize.Width / 2 * (m_tLayerSize.Width + pos.x - pos.y - 1),
                                 m_tMapTileSize.Height / 2 * ((m_tLayerSize.Height * 2 - pos.x - pos.y) - 2));
            return xy;
        }

        private CCPoint PositionForOrthoAt(CCPoint pos)
        {
            CCPoint xy = new CCPoint(pos.x * m_tMapTileSize.Width,
                                 (m_tLayerSize.Height - pos.y - 1) * m_tMapTileSize.Height);
            return xy;
        }

        private CCPoint PositionForHexAt(CCPoint pos)
        {
            float diffY = 0;
            if ((int) pos.x % 2 == 1)
            {
                diffY = -m_tMapTileSize.Height / 2;
            }

            var xy = new CCPoint(pos.x * m_tMapTileSize.Width * 3 / 4,
                                 (m_tLayerSize.Height - pos.y - 1) * m_tMapTileSize.Height + diffY);
            return xy;
        }

        private CCPoint CalculateLayerOffset(CCPoint pos)
        {
            CCPoint ret = CCPoint.Zero;
            switch (m_uLayerOrientation)
            {
                case CCTMXOrientation.CCTMXOrientationOrtho:
                    ret = new CCPoint(pos.x * m_tMapTileSize.Width, -pos.y * m_tMapTileSize.Height);
                    break;
                case CCTMXOrientation.CCTMXOrientationIso:
                    ret = new CCPoint((m_tMapTileSize.Width / 2) * (pos.x - pos.y),
                                      (m_tMapTileSize.Height / 2) * (-pos.x - pos.y));
                    break;
                case CCTMXOrientation.CCTMXOrientationHex:
                    Debug.Assert(pos.Equals(CCPoint.Zero), "offset for hexagonal map not implemented yet");
                    break;
            }
            return ret;
        }

        /* optimization methos */

        private CCSprite AppendTileForGID(uint gid, CCPoint pos)
        {
            CCRect rect = m_pTileSet.RectForGID(gid);
            rect = ccMacros.CC_RECT_PIXELS_TO_POINTS(rect);

            var z = (int) (pos.x + pos.y * m_tLayerSize.Width);

            CCSprite tile = ReusedTileWithRect(rect);

            SetupTileSprite(tile, pos, gid);

            // optimization:
            // The difference between appendTileForGID and insertTileforGID is that append is faster, since
            // it appends the tile at the end of the texture atlas
            int indexForZ = m_pAtlasIndexArray.Count;

            // don't add it using the "standard" way.
            AddQuadFromSprite(tile, indexForZ);

            // append should be after addQuadFromSprite since it modifies the quantity values
            m_pAtlasIndexArray.Insert(indexForZ, z);

            return tile;
        }

        private CCSprite InsertTileForGID(uint gid, CCPoint pos)
        {
            CCRect rect = m_pTileSet.RectForGID(gid);
            rect = ccMacros.CC_RECT_PIXELS_TO_POINTS(rect);

            var z = (int) (pos.x + pos.y * m_tLayerSize.Width);

            CCSprite tile = ReusedTileWithRect(rect);

            SetupTileSprite(tile, pos, gid);

            // get atlas index
            int indexForZ = AtlasIndexForNewZ(z);

            // Optimization: add the quad without adding a child
            AddQuadFromSprite(tile, indexForZ);

            // insert it into the local atlasindex array
            m_pAtlasIndexArray.Insert(indexForZ, z);

            // update possible children
            if (m_pChildren != null && m_pChildren.count > 0)
            {
                CCNode[] elements = m_pChildren.Elements;
                int count = m_pChildren.count;

                for (int i = 0; i < count; i++)
                {
                    var sprite = (CCSprite) elements[i];
                    int ai = sprite.AtlasIndex;
                    if (ai >= indexForZ)
                    {
                        sprite.AtlasIndex = ai + 1;
                    }
                }
            }
            m_pTiles[z] = gid;
            return tile;
        }

        private CCSprite UpdateTileForGID(uint gid, CCPoint pos)
        {
            CCRect rect = m_pTileSet.RectForGID(gid);
            rect = new CCRect(rect.origin.x / m_fContentScaleFactor, rect.origin.y / m_fContentScaleFactor, rect.size.Width / m_fContentScaleFactor,
                              rect.size.Height / m_fContentScaleFactor);
            var z = (int) (pos.x + pos.y * m_tLayerSize.Width);

            CCSprite tile = ReusedTileWithRect(rect);

            SetupTileSprite(tile, pos, gid);

            // get atlas index
            int indexForZ = AtlasIndexForExistantZ(z);
            tile.AtlasIndex = indexForZ;
            tile.Dirty = true;
            tile.UpdateTransform();
            m_pTiles[z] = gid;

            return tile;
        }

        /* The layer recognizes some special properties, like cc_vertez */

        private void ParseInternalProperties()
        {
            // if cc_vertex=automatic, then tiles will be rendered using vertexz

            m_fAlphaFuncValue = 0;

            string vertexz = PropertyNamed("cc_vertexz");
            if (!String.IsNullOrEmpty(vertexz))
            {
                // If "automatic" is on, then parse the "cc_alpha_func" too
                if (vertexz == "automatic")
                {
                    m_bUseAutomaticVertexZ = true;
                    string alphaFuncVal = PropertyNamed("cc_alpha_func");
                    //float alphaFuncValue = 0.0f;
                    if (!String.IsNullOrEmpty(alphaFuncVal))
                    {
                        m_fAlphaFuncValue = CCUtils.ccParseFloat(alphaFuncVal);
                    }
                    //setShaderProgram(CCShaderCache::sharedShaderCache().programForKey(kCCShader_PositionTextureColorAlphaTest));

                    //GLint alphaValueLocation = glGetUniformLocation(getShaderProgram().getProgram(), kCCUniformAlphaTestValue);

                    // NOTE: alpha test shader is hard-coded to use the equivalent of a glAlphaFunc(GL_GREATER) comparison
                    //getShaderProgram().setUniformLocationWith1f(alphaValueLocation, alphaFuncValue);
                }
                else
                {
                    m_nVertexZvalue = CCUtils.ccParseInt(vertexz);
                }
            }
        }

        private void SetupTileSprite(CCSprite sprite, CCPoint pos, uint gid)
        {
            sprite.Position = PositionAt(pos);
            sprite.VertexZ = VertexZForPos(pos);
            sprite.AnchorPoint = CCPoint.Zero;
            sprite.Opacity = m_cOpacity;

            //issue 1264, flip can be undone as well
            sprite.FlipX = false;
            sprite.FlipX = false;
            sprite.Rotation = 0.0f;
            sprite.AnchorPoint = CCPoint.Zero;

            // Rotation in tiled is achieved using 3 flipped states, flipping across the horizontal, vertical, and diagonal axes of the tiles.
            if ((gid & ccTMXTileFlags.kCCTMXTileDiagonalFlag) != 0)
            {
                // put the anchor in the middle for ease of rotation.
                sprite.AnchorPoint = new CCPoint(0.5f, 0.5f);
                CCPoint pointAtPos = PositionAt(pos);
                sprite.Position = new CCPoint(pointAtPos.x + sprite.ContentSize.Height / 2,
                                              pointAtPos.y + sprite.ContentSize.Width / 2);

                uint flag = gid & (ccTMXTileFlags.kCCTMXTileHorizontalFlag | ccTMXTileFlags.kCCTMXTileVerticalFlag);

                // handle the 4 diagonally flipped states.
                if (flag == ccTMXTileFlags.kCCTMXTileHorizontalFlag)
                {
                    sprite.Rotation = 90.0f;
                }
                else if (flag == ccTMXTileFlags.kCCTMXTileVerticalFlag)
                {
                    sprite.Rotation = 270.0f;
                }
                else if (flag == (ccTMXTileFlags.kCCTMXTileVerticalFlag | ccTMXTileFlags.kCCTMXTileHorizontalFlag))
                {
                    sprite.Rotation = 90.0f;
                    sprite.FlipX = true;
                }
                else
                {
                    sprite.Rotation = 270.0f;
                    sprite.FlipX = true;
                }
            }
            else
            {
                if ((gid & ccTMXTileFlags.kCCTMXTileHorizontalFlag) != 0)
                {
                    sprite.FlipX = true;
                }

                if ((gid & ccTMXTileFlags.kCCTMXTileVerticalFlag) != 0)
                {
                    sprite.FlipY = true;
                }
            }
        }

        private CCSprite ReusedTileWithRect(CCRect rect)
        {
            if (m_pReusedTile == null)
            {
                m_pReusedTile = new CCSprite();
                m_pReusedTile.InitWithTexture(m_pobTextureAtlas.Texture, rect, false);
                m_pReusedTile.BatchNode = this;
            }
            else
            {
                // XXX: should not be re-init. Potential memeory leak. Not following best practices
                // XXX: it shall call directory  [setRect:rect]
                m_pReusedTile.InitWithTexture(m_pobTextureAtlas.Texture, rect, false);

                // Since initWithTexture resets the batchNode, we need to re add it.
                // but should be removed once initWithTexture is not called again
                m_pReusedTile.BatchNode = this;
            }

            return m_pReusedTile;
        }

        private int VertexZForPos(CCPoint pos)
        {
            int ret = 0;
            if (m_bUseAutomaticVertexZ)
            {
                switch (m_uLayerOrientation)
                {
                    case CCTMXOrientation.CCTMXOrientationIso:
                        var maxVal = (int) (m_tLayerSize.Width + m_tLayerSize.Height);
                        ret = (int) (-(maxVal - (pos.x + pos.y)));
                        break;
                    case CCTMXOrientation.CCTMXOrientationOrtho:
                        ret = (int) (-(m_tLayerSize.Height - pos.y));
                        break;
                    case CCTMXOrientation.CCTMXOrientationHex:
                        Debug.Assert(false, "TMX Hexa zOrder not supported");
                        break;
                    default:
                        Debug.Assert(false, "TMX invalid value");
                        break;
                }
            }
            else
            {
                ret = m_nVertexZvalue;
            }

            return ret;
        }

        // index
        private int AtlasIndexForExistantZ(int z)
        {
            int index = m_pAtlasIndexArray.BinarySearch(z);

            Debug.Assert(index != -1, "TMX atlas index not found. Shall not happen");

            return index;
        }

        private int AtlasIndexForNewZ(int z)
        {
            // XXX: This can be improved with a sort of binary search
            int i, count;
            for (i = 0, count = m_pAtlasIndexArray.Count; i < count; i++)
            {
                if (z < m_pAtlasIndexArray[i])
                {
                    break;
                }
            }

            return i;
        }
    }
}