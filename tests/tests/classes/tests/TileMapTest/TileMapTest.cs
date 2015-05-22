using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using CocosSharp;

namespace tests
{
    public class TMXOrthoTest : TileDemo
    {
        public TMXOrthoTest() : base("TileMaps/orthogonal-test2")
        {
            // it should not flicker. No artifacts should appear

            var scale = new CCScaleBy(10, 0.1f);
            var back = scale.Reverse();
            tileLayersContainer.RepeatForever(scale, back);
        }

		public override string Title
		{
			get { return "TMX Orthogonal test"; }
		}
    }

    public class TMXOrthoTest2 : TileDemo
    {
        public TMXOrthoTest2() : base("TileMaps/orthogonal-test1")
        {
            tileLayersContainer.RunAction(SCALE_2X_Half );
        }

		public override string Title
		{
			get { return "TMX Ortho test2"; }
		}
    }

    public class TMXOrthoTest3 : TileDemo
    {
        public TMXOrthoTest3() : base("TileMaps/orthogonal-test3")
        {
            tileLayersContainer.Scale = 0.2f;
            tileLayersContainer.AnchorPoint = CCPoint.AnchorMiddle;
        }

		public override string Title
		{
			get
			{ return "TMX anchorPoint test"; }
		}
    }

    public class TMXOrthoTest4 : TileDemo
    {
        CCSprite sprite, sprite2, sprite3, sprite4;

        public TMXOrthoTest4() : base("TileMaps/orthogonal-test4")
        {
            CCTileMapLayer layer = tileMap.LayerNamed("Layer 0");
            CCTileMapCoordinates s = layer.LayerSize;

            sprite = layer.ExtractTile(0, 0);
            sprite.Scale = 2;

            sprite2 = layer.ExtractTile(s.Column - 1, 0);
            sprite2.Scale = 2;

            sprite3 = layer.ExtractTile(0, s.Row - 1);
            sprite3.Scale = 2;

            sprite4 = layer.ExtractTile(s.Column - 1, s.Row - 1);
            sprite4.Scale = 2;

            Schedule(removeSprite, 2);
        }

        void removeSprite(float dt)
        {
            Unschedule(removeSprite);

            var layer = tileMap.LayerNamed("Layer 0");
            var s = layer.LayerSize;

            layer.RemoveChild(sprite, true);
            layer.RemoveChild(sprite3, true);
            layer.RemoveChild(sprite2, true);
            layer.RemoveChild(sprite4, true);
        }

		public override string Title
		{
			get { return "TMX width/height test"; }
		}
    }

    public class TMXReadWriteTest : TileDemo
    {
        CCTileGidAndFlags m_gid;
        CCTileGidAndFlags m_gid2;

        public TMXReadWriteTest() : base("TileMaps/orthogonal-test2")
        {
            m_gid = CCTileGidAndFlags.EmptyTile;

            CCTileMapLayer layer = tileMap.LayerNamed("Layer 0");
            layer.Antialiased = true;

            tileMap.Scale = (1);

            CCSprite tile0 = layer.ExtractTile(1, 63);
            CCSprite tile1 = layer.ExtractTile(2, 63);
            CCSprite tile2 = layer.ExtractTile(3, 62); //new CCPoint(1,62));
            CCSprite tile3 = layer.ExtractTile(2, 62);
            tile0.AnchorPoint = (new CCPoint(0.5f, 0.5f));
            tile1.AnchorPoint = (new CCPoint(0.5f, 0.5f));
            tile2.AnchorPoint = (new CCPoint(0.5f, 0.5f));
            tile3.AnchorPoint = (new CCPoint(0.5f, 0.5f));

            CCMoveBy move = new CCMoveBy (0.5f, new CCPoint(0, 160));
            CCRotateBy rotate = new CCRotateBy (2, 360);
            CCScaleBy scale = new CCScaleBy(2, 5);
            CCFadeOut opacity = new CCFadeOut  (2);
            CCFadeIn fadein = new CCFadeIn  (2);
            CCScaleTo scaleback = new CCScaleTo(1, 1);
            CCCallFuncN finish = new CCCallFuncN(removeSprite);
			CCSequence sequence = new CCSequence(move, rotate, scale, opacity, fadein, scaleback, finish);

			tile0.RunAction(sequence);
			tile1.RunAction(sequence);
			tile2.RunAction(sequence);
			tile3.RunAction(sequence);


            m_gid = layer.TileGIDAndFlags(0, 63);

            Schedule(updateCol, 2.0f);
            Schedule(repaintWithGID, 2.0f);
            Schedule(removeTiles, 1.0f);


            m_gid2 = CCTileGidAndFlags.EmptyTile;
        }

        void removeSprite(CCNode sender)
        {
            CCNode p = sender.Parent;

            if (p != null)
            {
                p.RemoveChild(sender, true);
            }
        }

        void updateCol(float dt)
        {
            var layer = (CCTileMapLayer) tileLayersContainer.GetChildByTag(0);


            var s = layer.LayerSize;

            for (int y = 0; y < s.Row; y++)
            {
                layer.SetTileGID(m_gid2, new CCTileMapCoordinates(3, y));
            }

            m_gid2.Gid = (short)((m_gid2.Gid + 1) % 80);
        }

        void repaintWithGID(float dt)
        {
            var layer = (CCTileMapLayer) tileLayersContainer.GetChildByTag(0);

            var s = layer.LayerSize;
            for (int x = 0; x < s.Column; x++)
            {
                int y = s.Row - 1;
                var tmpgid = layer.TileGIDAndFlags(x, y);
                tmpgid.Gid += 1;
                layer.SetTileGID(tmpgid, new CCTileMapCoordinates(x, y));
            }
        }

        void removeTiles(float dt)
        {
            Unschedule(removeTiles);

            var layer = (CCTileMapLayer) tileLayersContainer.GetChildByTag(0);
            var s = layer.LayerSize;

            for (int y = 0; y < s.Row; y++)
            {
                layer.RemoveTile(5, y);
            }
        }

		public override string Title
		{
			get { return "TMX Read/Write test"; }
		}

        #region Nested type: SID

        enum SID
        {
            SID_UPDATECOL = 100,
            SID_REPAINTWITHGID,
            SID_REMOVETILES
        }

        #endregion
    }

    public class TMXHexTest : TileDemo
    {
        CCDrawNode drawNode;

        public TMXHexTest() : base("TileMaps/hexa-test1")
        {
            CCLayerColor color = new CCLayerColor(new CCColor4B(64, 64, 64, 255));
            AddChild(color, -1);

            drawNode = new CCDrawNode();
            tileLayersContainer.AddChild(drawNode);

            var touchListener = new CCEventListenerTouchOneByOne();
            touchListener.OnTouchBegan = OnTouchBegan;

            AddEventListener(touchListener);
        }

		public override string Title
		{
			get
			{ return "TMX Hex test"; }
		}

        bool OnTouchBegan(CCTouch touch, CCEvent touchEvent)
        {

            var layer = tileMap.LayerNamed("Layer 0");

            var location = layer.WorldToParentspace(touch.Location);
            var tileCoordinates = layer.ClosestTileCoordAtNodePosition(location);

            // Convert the tile coordinates position to world coordinates for
            // our outline drawing
            var world = layer.TilePosition(tileCoordinates);


            // Calculate our width and height of the tile
            CCSize texToContentScaling = CCTileMapLayer.DefaultTexelToContentSizeRatios;
            float width = layer.TileTexelSize.Width * texToContentScaling.Width;
            float height = layer.TileTexelSize.Height * texToContentScaling.Height;

            var rect = new CCRect(world.X, world.Y, width, height);

            drawNode.Clear();

            drawNode.Color = CCColor3B.Magenta;
            drawNode.Opacity = 255;

            var center = rect.Center;

            var right = center;
            right.X += width / 2;

            var top = center;
            top.Y += height / 2;

            var left = right;
            left.X -= width;

            var bottom = center;
            bottom.Y -= height / 2;

            // Hightlight our iso tile
            drawNode.DrawPolygon (new CCPoint[] {right, top, left, bottom}, 4, CCColor4B.Transparent, 1, new CCColor4F(CCColor4B.Magenta));

            return true;
        }
    }

    public class TMXIsoTest : TileDemo
    {
        public TMXIsoTest() : base("TileMaps/iso-test01")
        {
            CCLayerColor color = new CCLayerColor(new CCColor4B(64, 64, 64, 255));
            AddChild(color, -1);

            // move map to the center of the screen
            var ms = tileMap.MapDimensions;
            var ts = tileMap.TileTexelSize;
            tileLayersContainer.RunAction(new CCMoveTo (1.0f, new CCPoint(-ms.Column * ts.Width / 2, -ms.Row * ts.Height / 2)));
        }

		public override string Title
		{
			get
			{ return "TMX Isometric test 0"; }
		}
    }

    public class TMXIsoTest1 : TileDemo
    {
        public TMXIsoTest1() : base("TileMaps/iso-test11")
        {
            CCLayerColor color = new CCLayerColor(new CCColor4B(64, 64, 64, 255));
            AddChild(color, -1);

            tileMap.AnchorPoint = CCPoint.AnchorMiddle;
        }

		public override string Title
		{
			get { return "TMX Isometric test + anchorPoint"; }
		}
    }

    public class TMXIsoTest2 : TileDemo
    {
        public TMXIsoTest2() : base("TileMaps/iso-test22")
        {
            CCLayerColor color = new CCLayerColor(new CCColor4B(64, 64, 64, 255));
            AddChild(color, -1);

            // move map to the center of the screen
            var ms = tileMap.MapDimensions;
            var ts = tileMap.TileTexelSize;
            tileLayersContainer.RunAction(new CCMoveTo (1.0f, new CCPoint(-ms.Column * ts.Width / 2, -ms.Row * ts.Height / 2)));
        }

		public override string Title
		{
			get { return "TMX Isometric test 2"; }
		}
    }

//------------------------------------------------------------------
//
// TMXUncompressedTest
//
//------------------------------------------------------------------
    public class TMXUncompressedTest : TileDemo
    {
        public TMXUncompressedTest() : base("TileMaps/iso-test2-uncompressed")
        {
            CCLayerColor color = new CCLayerColor(new CCColor4B(64, 64, 64, 255));
            AddChild(color, -1);

            // move map to the center of the screen
            var ms = tileMap.MapDimensions;
            var ts = tileMap.TileTexelSize;
            tileLayersContainer.RunAction(new CCMoveTo (1.0f, new CCPoint(-ms.Column * ts.Width / 2, -ms.Row * ts.Height / 2)));
        }

		public override string Title
		{
			get { return "TMX Uncompressed test"; }
		}
    }

    public class TMXTilesetTest : TileDemo
    {
        public TMXTilesetTest() : base("TileMaps/orthogonal-test5")
        {

            CCTileMapLayer layer;
            layer = tileMap.LayerNamed("Layer 0");
            layer.Antialiased = true;

            layer = tileMap.LayerNamed("Layer 1");
            layer.Antialiased = true;

            layer = tileMap.LayerNamed("Layer 2");
            layer.Antialiased = true;
        }

		public override string Title
		{
			get { return "TMX Tileset test"; }
		}
    }

    public class TMXOrthoObjectsTest : TileDemo
    {
        public TMXOrthoObjectsTest() : base("TileMaps/ortho-objects")
        {
            var objectGroup = tileMap.ObjectGroupNamed("Object Group 1");
            var objects = objectGroup.Objects;

            var drawNode = new CCDrawNode();

            foreach (var dict in objects)
            {

                float x = float.Parse(dict["x"]);
                float y = float.Parse(dict["y"]);
                float width = (dict.ContainsKey("width") ? float.Parse(dict["width"]) : 0f);
                float height = (dict.ContainsKey("height") ? float.Parse(dict["height"]) : 0f);

                var color = new CCColor4B(255, 255, 255, 255);

                drawNode.DrawRect(new CCRect(x, y, width, height), CCColor4B.Transparent, 1, color);
            }

            tileLayersContainer.AddChild(drawNode);
        }

		public override string Title
		{
			get { return "TMX Ortho object test"; }
		}

		public override string Subtitle
		{
			get { return "You should see a white box around the 3 platforms"; }
		}
    }

    public class TMXIsoObjectsTest : TileDemo
    {
        public TMXIsoObjectsTest() : base("TileMaps/iso-test-objectgroup")
        {
            var objectGroup = tileMap.ObjectGroupNamed("Object Group 1");
            var objects = objectGroup.Objects;

            var drawNode = new CCDrawNode();

            foreach (var dict in objects)
            {

                float x = float.Parse(dict["x"]);
                float y = float.Parse(dict["y"]);
                float width = (dict.ContainsKey("width") ? float.Parse(dict["width"]) : 0f);
                float height = (dict.ContainsKey("height") ? float.Parse(dict["height"]) : 0f);

                var color = new CCColor4B(255, 255, 255, 255);

                drawNode.DrawRect(new CCRect(x, y, width, height), CCColor4B.Transparent, 1, color);
            }

            tileLayersContainer.AddChild(drawNode, 10);
        }

		public override string Title
		{
			get { return "TMX Iso object test"; }
		}

		public override string Subtitle
		{
			get { return "You need to parse them manually. See bug #810"; }
		}
    }

    public class TMXResizeTest : TileDemo
    {
        public TMXResizeTest() : base("TileMaps/orthogonal-test5")
        {
            CCTileMapLayer layer = tileMap.LayerNamed("Layer 0");

            var ls = layer.LayerSize;
            for (int y = 0; y < ls.Row; y++)
            {
                for (int x = 0; x < ls.Column; x++)
                {
                    layer.SetTileGID(new CCTileGidAndFlags(1), new CCTileMapCoordinates(x, y));
                }
            }
        }

		public override string Title
		{
			get { return "TMX resize test"; }
		}

		public override string Subtitle
		{
			get { return "Should not crash. Testing issue #740"; }
		}
    }

    public class TMXIsoZorder : TileDemo
    {
        readonly CCSprite m_tamara;

		static readonly CCMoveBy move = new CCMoveBy (10, new CCPoint(300, 250));
		static readonly CCFiniteTimeAction back = move.Reverse();

        public TMXIsoZorder() : base("TileMaps/iso-test-zorder")
        {
            m_tamara = new CCSprite(pathSister1);
            tileLayersContainer.AddChild(m_tamara, tileMap.Children.Count);
            tileLayersContainer.Position = new CCPoint(-50.0f, -50.0f);

            m_tamara.AnchorPoint = CCPoint.Zero;

			m_tamara.RepeatForever(move, back);

            Schedule(repositionSprite);
        }

        protected override void AddedToScene()
        {
            base.AddedToScene();

            m_tamara.Position = tileMap.LayerNamed("grass").TilePosition(29, 29);
		}


        public override void OnExit()
        {
            Unschedule(repositionSprite);
            base.OnExit();
        }

        private void repositionSprite(float dt)
        {
            CCPoint p = m_tamara.Position;


            // there are only 4 layers. (grass and 3 trees layers)
            // if tamara < 48, z=4
            // if tamara < 96, z=3
            // if tamara < 144,z=2

            int newZ = (int)(4 - (p.Y / 48));
            newZ = Math.Max(newZ, 0);

            tileLayersContainer.ReorderChild(m_tamara, newZ);
        }

		public override string Title
		{
			get { return "TMX Iso Zorder"; }
		}

		public override string Subtitle
		{
			get { return "Sprite should hide behind the trees"; }
		}
    }

    public class TMXIsoZorderFromStream : TileDemo
    {
        readonly CCSprite m_tamara;

        static readonly CCMoveBy move = new CCMoveBy (10, new CCPoint(300, 250));
        static readonly CCFiniteTimeAction back = move.Reverse();

        public TMXIsoZorderFromStream() : base(new System.IO.StreamReader(CCFileUtils.GetFileStream("TileMaps/iso-test-zorder1.tmx")))
        {
            m_tamara = new CCSprite(pathSister1);
            tileLayersContainer.AddChild(m_tamara, tileMap.Children.Count);
            tileLayersContainer.Position = new CCPoint(-50.0f, -50.0f);

            m_tamara.AnchorPoint = CCPoint.Zero;

            m_tamara.RepeatForever(move, back);

            Schedule(repositionSprite);
        }

        protected override void AddedToScene()
        {
            base.AddedToScene();

            m_tamara.Position = tileMap.LayerNamed("grass").TilePosition(29, 29);
        }


        public override void OnExit()
        {
            Unschedule(repositionSprite);
            base.OnExit();
        }

        private void repositionSprite(float dt)
        {
            CCPoint p = m_tamara.Position;


            // there are only 4 layers. (grass and 3 trees layers)
            // if tamara < 48, z=4
            // if tamara < 96, z=3
            // if tamara < 144,z=2

            int newZ = (int)(4 - (p.Y / 48));
            newZ = Math.Max(newZ, 0);

            tileLayersContainer.ReorderChild(m_tamara, newZ);
        }

        public override string Title
        {
            get { return "TMX Iso Zorder using StreamReader"; }
        }

        public override string Subtitle
        {
            get { return "Sprite should hide behind the trees"; }
        }
    }


    public class TMXOrthoZorder : TileDemo
    {
        readonly CCSprite m_tamara;

		static CCMoveBy move = new CCMoveBy (10, new CCPoint(400, 450));
		static CCFiniteTimeAction back = move.Reverse();

        public TMXOrthoZorder() : base("TileMaps/orthogonal-test-zorder")
        {
            m_tamara = new CCSprite(pathSister1);
            tileLayersContainer.AddChild(m_tamara, tileMap.Children.Count);
			m_tamara.AnchorPoint = CCPoint.AnchorMiddleBottom;

			m_tamara.RepeatForever(move, back);

            Schedule(repositionSprite);
        }

        private void repositionSprite(float dt)
        {
            CCPoint p = m_tamara.Position;

            // there are only 4 layers. (grass and 3 trees layers)
            // if tamara < 81, z=4
            // if tamara < 162, z=3
            // if tamara < 243,z=2

            // -10: customization for this particular sample
            int newZ = (int)(4 - ((p.Y - 10) / 81));
            newZ = Math.Max(newZ, 0);

            tileLayersContainer.ReorderChild(m_tamara, newZ);
        }

		public override string Title
		{
			get
			{ return "TMX Ortho Zorder"; }
		}

		public override string Subtitle
		{
			get { return "Sprite should hide behind the trees"; }
		}
    }

    public class TMXIsoVertexZ : TileDemo
    {
        readonly CCSprite m_tamara;

        public TMXIsoVertexZ() : base("TileMaps/iso-test-vertexz")
        {
            CCTileMapLayer layer = tileMap.LayerNamed("Trees");
            m_tamara = layer.ExtractTile(29, 29);
        }

        #region Setup content

        public override void OnEnter()
        {
            base.OnEnter(); 

            CCSize s = tileLayersContainer.ContentSize;
            tileLayersContainer.Position = new CCPoint(-s.Width / 2, 0);
            CCTileMapLayer layer = tileMap.LayerNamed("Trees");
            layer.Position = new CCPoint(s.Width, 0);

            CCMoveBy move = new CCMoveBy (10, new CCPoint(300, 250));
            CCFiniteTimeAction back = move.Reverse();
            CCSequence seq = new CCSequence(move, back);
            m_tamara.RunAction(new CCRepeatForever (seq));

            Schedule(repositionSprite);

            tileMap.Camera.NearAndFarOrthographicZClipping 
                = new CCNearAndFarClipping(-2000f, 2000f);

            Window.IsUseDepthTesting = true;
        }

        public override void OnExit()
        {
            base.OnExit();
            Window.IsUseDepthTesting = false;
        }

        #endregion Setup content

        private void repositionSprite(float dt)
        {
            // tile height is 64x32
            // map size: 30x30
			CCPoint p = m_tamara.Position;
            float newZ = -(p.Y + 32f) / 16f;
            m_tamara.VertexZ = newZ;
        }

		public override string Title
		{
			get
			{ return "TMX Iso VertexZ"; }
		}

		public override string Subtitle
		{
			get { return "Sprite should hide behind the trees"; }
		}
    }

    public class TMXOrthoVertexZ : TileDemo
    {
        readonly CCSprite m_tamara;

        public TMXOrthoVertexZ() : base("TileMaps/orthogonal-test-vertexz")
        {
            CCTileMapLayer layer = tileMap.LayerNamed("trees");
            m_tamara = layer.ExtractTile(0, 11);

            CCLog.Log("tamara vertexZ: {0}", m_tamara.VertexZ);
        }

        #region Setup content

        public override void OnEnter()
        {
            base.OnEnter();

            var move = new CCMoveBy (10, new CCPoint(400, 450));
            var back = move.Reverse();
			m_tamara.RepeatForever(move, back);

            Schedule(repositionSprite);

            tileMap.Camera.NearAndFarOrthographicZClipping 
            = new CCNearAndFarClipping(-2000f, 2000f);

            Window.IsUseDepthTesting = true;
        }

        public override void OnExit()
        {
            base.OnExit();
            Window.IsUseDepthTesting = false;
        }

        #endregion Setup content

        void repositionSprite(float dt)
        {
            // tile height is 101x81
            // map size: 12x12
            CCPoint p = m_tamara.Position;
            m_tamara.VertexZ = -((p.Y + 81) / 81);
        }

		public override string Title
		{
			get { return "TMX Ortho vertexZ"; }
		}

		public override string Subtitle
		{
			get { return "Sprite should hide behind the trees"; }
		}
    }

    public class TMXIsoMoveLayer : TileDemo
    {
        public TMXIsoMoveLayer() : base("TileMaps/iso-test-movelayer")
        {
            tileLayersContainer.Position = new CCPoint(-700, -50);
        }

		public override string Title
		{
			get { return "TMX Iso Move Layer"; }
		}

		public override string Subtitle
		{
			get { return "Trees should be horizontally aligned"; }
		}
    }

    public class TMXOrthoMoveLayer : TileDemo
    {
        public TMXOrthoMoveLayer() : base("TileMaps/orthogonal-test-movelayer")
        {
        }

		public override string Title
		{
			get { return "TMX Ortho Move Layer"; }
		}

		public override string Subtitle
		{
			get { return "Trees should be horizontally aligned"; }
		}
    }

    public class TMXTilePropertyTest : TileDemo
    {
        public TMXTilePropertyTest() : base("TileMaps/ortho-tile-property")
        {
            for (short i = 1; i <= 20; i++)
            {
                CCLog.Log("GID:{0}, Properties:{1}", i, tileMap.TilePropertiesForGID(i));
            }
        }

		public override string Title
		{
			get { return "TMX Tile Property Test"; }
		}

		public override string Subtitle
		{
			get { return "In the console you should see tile properties"; }
		}
    }

    public class TMXOrthoFlipTest : TileDemo
    {
        public TMXOrthoFlipTest() : base("TileMaps/ortho-rotation-test")
        {
            tileMap.Antialiased = true;
				
            tileLayersContainer.RunAction(SCALE_2X_Half );
        }

		public override string Title
		{
			get { return "TMX tile flip test"; }
		}
    }

    public class TMXOrthoFlipRunTimeTest : TileDemo
    {
        public TMXOrthoFlipRunTimeTest() : base("TileMaps/ortho-rotation-test")
        {
            tileMap.Antialiased = true;
				
            tileLayersContainer.RunAction(SCALE_2X_Half );

            Schedule(flipIt, 1.0f);
        }

		public override string Title
		{
			get { return "TMX tile flip run time test"; }
		}

		public override string Subtitle
		{
			get { return "in 2 sec bottom left tiles will flip"; }
		}

        void flipIt(float dt)
        {
            CCTileMapLayer layer = tileMap.LayerNamed("Layer 0");

            //blue diamond 
            var tileCoord = new CCTileMapCoordinates(1, 10);

            CCTileGidAndFlags gidAndFlags = layer.TileGIDAndFlags(tileCoord);
            CCTileFlags flags = gidAndFlags.Flags;
            short GID = gidAndFlags.Gid;

            // Vertical
            if ((flags & CCTileFlags.Vertical) != 0)
                flags &= ~CCTileFlags.Vertical;
            else
                flags |= CCTileFlags.Vertical;


            layer.SetTileGID(new CCTileGidAndFlags(GID, flags), tileCoord);


            tileCoord = new CCTileMapCoordinates(1, 8);
            gidAndFlags = layer.TileGIDAndFlags(tileCoord);
            GID = gidAndFlags.Gid;
            flags = gidAndFlags.Flags;

            // Vertical
            if ((flags & CCTileFlags.Vertical) != 0)
                flags &= ~CCTileFlags.Vertical;
            else
                flags |= CCTileFlags.Vertical;

            layer.SetTileGID(new CCTileGidAndFlags(GID, flags), tileCoord);

            tileCoord = new CCTileMapCoordinates(2, 8);
            gidAndFlags = layer.TileGIDAndFlags(tileCoord);
            GID = gidAndFlags.Gid;
            flags = gidAndFlags.Flags;

            // Horizontal
            if ((flags & CCTileFlags.Horizontal) != 0)
                flags &= ~CCTileFlags.Horizontal;
            else
                flags |= CCTileFlags.Horizontal;

            layer.SetTileGID(new CCTileGidAndFlags(GID, flags), tileCoord);
        }
    }

    public class TMXOrthoFromXMLTest : TileDemo
    {
        public TMXOrthoFromXMLTest() : base("TileMaps/orthogonal-test1")
        {
            tileMap.Antialiased = true;	
            tileLayersContainer.RunAction(SCALE_2X_Half);
        }

		public override string Title
		{
			get { return "TMX created from XML test"; }
		}
    }

    public class TMXBug987 : TileDemo
    {
        public TMXBug987() : base("TileMaps/orthogonal-test6")
        {
            CCTileMapLayer layer = tileMap.LayerNamed("Tile Layer 1");
            layer.SetTileGID(new CCTileGidAndFlags(3), new CCTileMapCoordinates(2, 2));

        }

        protected override void AddedToScene()
        {
            base.AddedToScene();

            tileLayersContainer.Position = new CCPoint(100, 100);
		}

		public override string Title
		{
			get { return "TMX Bug 987"; }
		}

		public override string Subtitle
		{
			get { return "You should see an square"; }
		}
    }

    public class TMXBug787 : TileDemo
    {
        public TMXBug787() : base("TileMaps/iso-test-bug787")
        {
            tileLayersContainer.Scale = (0.25f);
        }

		public override string Title
		{
			get { return "TMX Bug 787"; }
		}

		public override string Subtitle
		{
			get { return "You should see a map"; }
		}
    }

    public class TileDemo : TestNavigationLayer
    {
        protected const string s_TilesPng = "TileMaps/tiles";
        protected const string s_LevelMapTga = "TileMaps/levelmap";
		protected const string pathSister1 = TestResource.s_pPathSister1;

        protected CCTileMap tileMap;
        protected CCNode tileLayersContainer;

		protected CCScaleBy SCALE_2X_Half = new CCScaleBy(2, 0.5f);

        public TileDemo (System.IO.StreamReader tilemapReader )
        {
            tileMap = new CCTileMap(tilemapReader);
            tileLayersContainer = tileMap.TileLayersContainer;

            AddChild(tileMap);

            // Register Touch Event
            var touchListener = new CCEventListenerTouchAllAtOnce();
            touchListener.OnTouchesMoved = onTouchesMoved;

            AddEventListener(touchListener);
        }

        public TileDemo(string tilemapName)
        {
            tileMap = new CCTileMap(tilemapName);
            tileLayersContainer = tileMap.TileLayersContainer;

            AddChild(tileMap);

            // Register Touch Event
			var touchListener = new CCEventListenerTouchAllAtOnce();
			touchListener.OnTouchesMoved = onTouchesMoved;

			AddEventListener(touchListener);
        }

		public override string Title
		{
			get
			{
				return string.Empty;
			}
		}

		public override string Subtitle
		{
			get
			{
				return string.Empty;
			}
		}

		public override void RestartCallback(object sender)
		{
            CCScene s = new TileMapTestScene();
            s.AddChild(TileMapTestScene.restartTileMapAction());
            Director.ReplaceScene(s);
        }

		public override void NextCallback(object sender)
		{
            CCScene s = new TileMapTestScene();
            s.AddChild(TileMapTestScene.nextTileMapAction());

            Director.ReplaceScene(s);
        }

		public override void BackCallback(object sender)
		{
            CCScene s = new TileMapTestScene();
            s.AddChild(TileMapTestScene.backTileMapAction());
            Director.ReplaceScene(s);
        }

		void onTouchesMoved(List<CCTouch> touches, CCEvent touchEvent)
        {
			var touch = touches [0];
            CCPoint diff = touch.Delta;
            tileLayersContainer.Position += diff;
        }
    }

    public class IsoNodePosition : TileDemo
    {
        CCDrawNode drawNode;

        public IsoNodePosition() : base("TileMaps/iso-test-zorder")
        {

            drawNode = new CCDrawNode();
            tileLayersContainer.AddChild(drawNode);

            var touchListener = new CCEventListenerTouchOneByOne();
            touchListener.OnTouchBegan = OnTouchBegan;

            AddEventListener(touchListener);

        }

        bool OnTouchBegan (CCTouch touch, CCEvent touchEvent)
        {

            var layer = tileMap.LayerNamed("grass");

            var location = layer.WorldToParentspace(touch.Location);
            var tileCoordinates = layer.ClosestTileCoordAtNodePosition(location);
            var gid = layer.TileGIDAndFlags(tileCoordinates);

            if (gid.Gid != 1)  // we only want the green grass
                return false;

            // Convert the tile coordinates position to world coordinates for
            // our outline drawing
            var world = layer.TilePosition(tileCoordinates);


            // Calculate our width and height of the tile
            CCSize texToContentScaling = CCTileMapLayer.DefaultTexelToContentSizeRatios;
            float width = layer.TileTexelSize.Width * texToContentScaling.Width;
            float height = layer.TileTexelSize.Height * texToContentScaling.Height;

            var rect = new CCRect(world.X, world.Y, width, height);

            drawNode.Clear();

            drawNode.Color = CCColor3B.Magenta;
            drawNode.Opacity = 255;

            var center = rect.Center;

            var right = center;
            right.X += width / 2;

            var top = center;
            top.Y += height / 2;

            var left = right;
            left.X -= width;

            var bottom = center;
            bottom.Y -= height / 2;

            // Hightlight our iso tile
            drawNode.DrawPolygon (new CCPoint[] {right, top, left, bottom}, 4, CCColor4B.Transparent, 1, new CCColor4F(CCColor4B.Magenta));

            return true;
        }

        public override string Title
        {
            get { return "Iso Node Position"; }
        }

        public override string Subtitle
        {
            get { return "Click on the grass to hightlight tile"; }
        }
    }


    public class TileMapTestScene : TestScene
    {
        static int sceneIdx = -1;
        static int MAX_LAYER = 40;

        static CCLayer createTileMapLayer(int nIndex)
        {
            switch (nIndex)
            {
#if XBOX || OUYA
                case 0:
                    return new TMXIsoZorder();
                case 1:
                    return new TMXOrthoZorder();
                case 2:
                case 3:
                    return new TMXIsoVertexZ();
/*                case 3:
                    return new TMXOrthoVertexZ();
 */
                case 4:
                    return new TMXOrthoTest();
                case 5:
                    return new TMXOrthoTest();
                case 6:
                    return new TMXOrthoTest3();
                case 7:
                    return new TMXOrthoTest4();
                case 8:
                    return new TMXIsoTest();
                case 9:
                    return new TMXIsoTest1();
                case 10:
                    return new TMXIsoTest2();
                case 11:
                case 12:
                    return new TMXUncompressedTest();
//                    return new TMXHexTest();
                case 13:
                    return new TMXReadWriteTest();
                case 14:
                    return new TMXTilesetTest();
                case 15:
                    return new TMXOrthoObjectsTest();
                case 16:
                    return new TMXIsoObjectsTest();
                case 17:
                    return new TMXResizeTest();
                case 18:
                    return new TMXIsoMoveLayer();
                case 19:
                case 20:
                    return new TMXOrthoMoveLayer();
/*                case 20:
                    return new TMXOrthoFlipTest();
*/
                case 21:
                case 22:
                    return new TMXOrthoFlipRunTimeTest();
//                    return new TMXOrthoFromXMLTest();
                case 23:
                    return new TileMapTest();
                case 24:
                    return new TileMapEditTest();
                case 25:
                    return new TMXBug987();
                case 26:
                    return new TMXBug787();
                case 27:
                    return new TMXGIDObjectsTest();
#else
                case 0:
                    return new TMXIsoZorder();
                case 1:
                    return new TMXOrthoZorder();
                case 2:
                    return new TMXIsoVertexZ();
                case 3:
                    return new TMXOrthoVertexZ();
                case 4:
                    return new TMXOrthoTest();
                case 5:
                    return new TMXOrthoTest2();
                case 6:
                    return new TMXOrthoTest3();
                case 7:
                    return new TMXOrthoTest4();
                case 8:
                    return new TMXIsoTest();
                case 9:
                    return new TMXIsoTest1();
                case 10:
                    return new TMXIsoTest2();
                case 11:
                    return new TMXUncompressedTest();
                case 12:
                    return new TMXHexTest();
                case 13:
                    return new TMXReadWriteTest();
                case 14:
                    return new TMXTilesetTest();
                case 15:
                    return new TMXOrthoObjectsTest();
                case 16:
                    return new TMXIsoObjectsTest();
                case 17:
                    return new TMXResizeTest();
                case 18:
                    return new TMXIsoMoveLayer();
                case 19:
                    return new TMXOrthoMoveLayer();
                case 20:
                    return new TMXOrthoFlipTest();
                case 21:
                    return new TMXOrthoFlipRunTimeTest();
                case 22:
                    return new TMXOrthoFromXMLTest();
                case 23:
                    return new TMXBug987();
                case 24:
                    return new TMXBug787();
                case 25:
                    return new TMXGIDObjectsTest();
                case 26:
                    return new IsoNodePosition();
                case 27:
                    return new TMXIsoZorderFromStream();
                case 28:
                    return new TMXNoEncodingTest();
                case 29:
                    return new TMXPolylineTest();
                case 30:
                    return new TMXMultiLayerTest();
                case 31:
                    return new TMXStaggeredMapTest();
                case 32:
                    return new TMXLargeMapTest();
                case 33:
                    return new TMXLargeMapScalingTest();
                case 34:
                    return new TMXSuperLargeMapTest();
                case 35:
                    return new TMXTileDifferentTileDimAnimationTest();
                case 36:
                    return new TMXAnimationLargeMapTest();
                case 37:
                    return new TMXTileAnimationTest();
                case 38:
                    return new TMXLayerWithMutiTilesets();
                case 39:
                    return new TMXLayerRepositionTileLayer();
#endif
            }

            return null;
        }

        public static CCLayer nextTileMapAction()
        {
            sceneIdx++;
            sceneIdx = sceneIdx % MAX_LAYER;

            CCLayer pLayer = createTileMapLayer(sceneIdx);

            return pLayer;
        }

        public static CCLayer backTileMapAction()
        {
            sceneIdx--;
            int total = MAX_LAYER;
            if (sceneIdx < 0)
                sceneIdx += total;

            CCLayer pLayer = createTileMapLayer(sceneIdx);

            return pLayer;
        }

        public static CCLayer restartTileMapAction()
        {
            CCLayer pLayer = createTileMapLayer(sceneIdx);
            return pLayer;
        }

        protected override void RestTestCase()
        {
            restartTileMapAction();
        }

        protected override void NextTestCase()
        {
            nextTileMapAction();
        }

        protected override void PreviousTestCase()
        {
            backTileMapAction();
        }
        public override void runThisTest()
        {
            CCLayer pLayer = nextTileMapAction();
            AddChild(pLayer);

            // fix bug #486, #419. 
            // "test" is not the default value in CCDirector.setGLDefaultValues()
            // but TransitionTest may setDepthTest(false), we should revert it here

            //Window.IsUseDepthTesting = true;

            Director.ReplaceScene(this);
        }

        #region Nested type: Action

        private enum Action
        {
            IDC_NEXT = 100,
            IDC_BACK,
            IDC_RESTART
        }

        #endregion
    }

    public class TMXGIDObjectsTest : TileDemo
    {
        CCDrawNode drawNode;

        public TMXGIDObjectsTest() : base("TileMaps/test-object-layer")
        {
            drawNode = new CCDrawNode();
            tileLayersContainer.AddChild(drawNode);
        }

        protected override void AddedToScene ()
        {
            base.AddedToScene();
            CCTileMapObjectGroup group = tileMap.ObjectGroupNamed("Object Layer 1");

            drawNode.Color = CCColor3B.Magenta;
            drawNode.Opacity = 255;
            drawNode.Clear();

            foreach (var dict in group.Objects)
            {
                int x = int.Parse(dict["x"]);
                int y = int.Parse(dict["y"]);
;
                drawNode.DrawDot(new CCPoint (x, y), 10.0f, new CCColor4F (CCColor4B.Magenta));
            }
        }

		public override string Title
		{
			get
			{
				return "TMX GID objects";
			}
		}

		public override string Subtitle
		{
			get
			{
				return "Tiles are created from an object group";
			}
		}
    }

    public class TMXNoEncodingTest : TileDemo
    {
        public TMXNoEncodingTest() : base("TileMaps/tilemap_no_encoding")
        {
        }

        public override string Title
        {
            get { return "TMX No Encoding"; }
        }
    }

    public class TMXLargeMapTest : TileDemo
    {
        public TMXLargeMapTest() : base("TileMaps/large_map")
        {
        }

        public override string Title
        {
            get { return "TMX Large map"; }
        }
    }

    public class TMXLargeMapScalingTest : TileDemo
    {
        public TMXLargeMapScalingTest() : base("TileMaps/large_map")
        {
            var action = new CCRepeatForever(new CCSequence(new CCScaleTo(2.0f, 0.5f), new CCScaleTo(2.0f, 2f)));
            tileLayersContainer.RunAction(action);
        }

        public override string Title
        {
            get { return "TMX Large map"; }
        }

        public override string Subtitle
        {
            get { return "Making sure culling handles scaling of map"; }
        }
    }

    public class TMXTileAnimationTest : TileDemo
    {
        public TMXTileAnimationTest() : base("TileMaps/desert-palace")
        {
        }

        public override string Title
        {
            get { return "Tile animation"; }
        }

        public override string Subtitle
        {
            get { return "Water and lamp should be animated"; }
        }
    }

    public class TMXTileDifferentTileDimAnimationTest : TileDemo
    {
        public TMXTileDifferentTileDimAnimationTest() : base("TileMaps/animation_diff_tile_dim")
        {
        }

        public override string Title
        {
            get { return "Tile animation"; }
        }

        public override string Subtitle
        {
            get { return "Should correctly handle cycling over tiles with different dimensions"; }
        }
    }

    public class TMXAnimationLargeMapTest : TileDemo
    {
        public TMXAnimationLargeMapTest() : base("TileMaps/animation_large_map")
        {
        }

        public override string Title
        {
            get { return "Tile animation"; }
        }

        public override string Subtitle
        {
            get { return "Testing animation performance on a large map with culling"; }
        }
    }


    public class TMXSuperLargeMapTest : TileDemo
    {
        public TMXSuperLargeMapTest() : base("TileMaps/super_large_map")
        {
        }

        public override string Title
        {
            get { return "Super large map : Testing culling"; }
        }

        public override string Subtitle
        {
            get { return "400x400 isometric"; }
        }
    }

    public class TMXStaggeredMapTest : TileDemo
    {
        CCDrawNode drawNode;

        public TMXStaggeredMapTest() : base("TileMaps/staggered_test")
        {
            drawNode = new CCDrawNode();
            tileLayersContainer.AddChild(drawNode);

            var touchListener = new CCEventListenerTouchOneByOne();
            touchListener.OnTouchBegan = OnTouchBegan;

            AddEventListener(touchListener);
        }

        public override string Title
        {
            get { return "Staggered isometric tile map"; }
        }

        bool OnTouchBegan(CCTouch touch, CCEvent touchEvent)
        {
            var layer = tileMap.LayerNamed("grass");

            var location = layer.WorldToParentspace(touch.Location);
            var tileCoordinates = layer.ClosestTileCoordAtNodePosition(location);

            if(tileCoordinates.Row < 0 || tileCoordinates.Column < 0)
                return true;

            // Convert the tile coordinates position to world coordinates for
            // our outline drawing
            var world = layer.TilePosition(tileCoordinates);


            // Calculate our width and height of the tile
            CCSize texToContentScaling = CCTileMapLayer.DefaultTexelToContentSizeRatios;
            float width = layer.TileTexelSize.Width * texToContentScaling.Width;
            float height = layer.TileTexelSize.Height * texToContentScaling.Height;

            var rect = new CCRect(world.X, world.Y, width, height);

            drawNode.Clear();

            drawNode.Color = CCColor3B.Magenta;
            drawNode.Opacity = 255;

            var center = rect.Center;

            var right = center;
            right.X += width / 2;

            var top = center;
            top.Y += height / 2;

            var left = right;
            left.X -= width;

            var bottom = center;
            bottom.Y -= height / 2;

            // Hightlight our iso tile
            drawNode.DrawPolygon (new CCPoint[] {right, top, left, bottom}, 4, CCColor4B.Transparent, 1, new CCColor4F(CCColor4B.Magenta));

            return true;
        }
    }

    public class TMXPolylineTest : TileDemo
    {
        public TMXPolylineTest() : base("TileMaps/orthogonal-test-polylines")
        {
			// Polylines render purple
			// Polygons render yellow
			// Rectangles render green
			// Ellipses render blue

			var mainLayer = tileMap.LayerNamed( "Layer 0" );
			var boundsObject = tileMap.ObjectGroupNamed( "Object Layer 1" );

	        var draw = new CCDrawNode();
	        mainLayer.AddChild(draw);

			foreach ( var bound in boundsObject.Objects )
			{
				// There is no indication in a TMX file that a given object is a rectangle.
				// This is just the default way an object is interpreted by Tiled. 
				// So if the map loader didn't add a "shape" key, interpret it as a rectangle.
				if ( !bound.ContainsKey( "shape" ) )
				{
					DrawRectangle(bound, draw);
					continue;
				}

				string shape = bound["shape"];
				switch ( shape )
				{
					case "polyline":
						DrawPolyline( bound, draw );
						break;

					case "polygon":
						DrawPolygon( bound, draw );
						break;

					case "ellipse":
						mainLayer.AddChild(ParseEllipse(bound));
						break;
				}
			}
        }

		#region Parsing

		protected void DrawRectangle(Dictionary<string, string> dict, CCDrawNode draw)
		{
			float x = float.Parse( dict["x"] );
			float y = float.Parse( dict["y"] );
			float width = float.Parse( dict["width"] );
			float height = float.Parse( dict["height"] );
			draw.DrawRect(new CCRect(x, y, width, height), new CCColor4B(0, 0, 0, 0), 1.0f, new CCColor4B(0, 255, 0));
		}

		protected void DrawPolygon(Dictionary<string, string> dict, CCDrawNode draw)
		{
			List<CCPoint> points = ParsePoints(dict["points"]);
			draw.DrawPolygon(points.ToArray(), points.Count, new CCColor4B(0, 0, 0, 0), 1.0f, new CCColor4B(255, 255, 0) );
		}

		public void DrawPolyline(Dictionary<string, string> dict, CCDrawNode draw)
		{
			List<CCPoint> points = ParsePoints(dict["points"]);
			var color = new CCColor4B( 255, 0, 255 );
			for ( int i = 0; i < points.Count-1; i++ )
			{
				draw.AddLineVertex(new CCV3F_C4B(points[i], color));
				draw.AddLineVertex(new CCV3F_C4B(points[i+1], color));
			}
		}

		protected List<CCPoint> ParsePoints(string pointsString)
		{
			var list = new List<CCPoint>();
			string[] pointPairs = pointsString.Split(' ');
			foreach (string pointPair in pointPairs)
			{
				string[] pointCoords = pointPair.Split(',');
				if (pointCoords.Length != 2)
					return null;

				float x = float.Parse(pointCoords[0]);
				float y = float.Parse(pointCoords[1]);

				list.Add( new CCPoint(x, y) );
			}
			return list;
		}

		protected TmxMapEllipse ParseEllipse(Dictionary<string, string> dict)
		{
			var shape = new TmxMapEllipse { Color = new CCColor3B( 0, 0, 255 ) };
			float x = float.Parse( dict["x"] );
			float y = float.Parse( dict["y"] );
			float width = float.Parse( dict["width"] );
			float height = float.Parse( dict["height"] );
			shape.Rect = new CCRect(x, y, width, height);
			return shape;
		}

		#endregion


		public override string Title
		{
			get { return "TMX Polyline test"; }
		}
    }


    public class TMXMultiLayerTest : TileDemo
    {
        public TMXMultiLayerTest() : base("TileMaps/orthogonal-test-multilayer")
        {
			// Background layer should display red circles
        }

		public override string Title
		{
			get { return "TMX Multilayer test"; }
		}
    }

    public class TMXLayerWithMutiTilesets:TileDemo
    {
        public TMXLayerWithMutiTilesets()
            : base("TileMaps/desert-palace-multi-tileset")
        {
			// Background layer should display red circles
        }

		public override string Title
		{
			get { return "TMX Multiple Tilesets per layer"; }
		}
        public override string Subtitle
        {
            get { return "Using multiple tilesets in the one CCTileMapLayer with animations"; }
        }
    }


    public class TMXLayerRepositionTileLayer : TileDemo
    {
        public TMXLayerRepositionTileLayer()
            : base("TileMaps/iso-test-vertexz")
        {
        }

        public override string Title
        {
            get { return "Non-zero position of tilemap layer"; }
        }
        public override string Subtitle
        {
            get { return "Testing culling when tilemap layer position is non-zero"; }
        }

        public override void OnEnter()
        {
            base.OnEnter(); 

            CCSize s = tileLayersContainer.ContentSize;
            CCTileMapLayer layer = tileMap.LayerNamed("Trees");

            CCMoveBy move = new CCMoveBy (5, new CCPoint(500, 550));
            CCMoveBy move2 = new CCMoveBy (5, new CCPoint(250, 250));
            CCFiniteTimeAction back = move.Reverse();
            CCSequence seq = new CCSequence(move, back);

            tileMap.Camera.NearAndFarOrthographicZClipping 
              = new CCNearAndFarClipping(-2000f, 2000f);

            layer.RunAction(seq);
            tileLayersContainer.RunAction(new CCSequence(move2.Reverse(), move2));
        }
    }
}