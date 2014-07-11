using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using CocosSharp;

namespace tests
{
//------------------------------------------------------------------
//
// TileMapTest
//
//------------------------------------------------------------------
    public class TileMapTest : TileDemo
    {
        public TileMapTest()
        {
            CCTileMapAtlas map = new CCTileMapAtlas(s_TilesPng, s_LevelMapTga, 16, 16);
            // Convert it to "alias" (GL_LINEAR filtering)
			map.IsAntialiased = true;

            CCSize s = map.ContentSize;
            CCLog.Log("ContentSize: {0}, {1}", s.Width, s.Height);

            // If you are not going to use the Map, you can free it now
            // NEW since v0.7
            map.ReleaseMap();

            AddChild(map, 0, kTagTileMap);

			map.AnchorPoint = CCPoint.AnchorMiddleLeft;

            CCScaleBy scale = new CCScaleBy(4, 0.8f);
            CCFiniteTimeAction scaleBack = scale.Reverse();

            var seq = new CCSequence(scale, scaleBack);

            map.RunAction(new CCRepeatForever ((CCActionInterval)seq));
        }

		public override string Title
		{
			get
			{
				return "TileMapAtlas";
			}
		}
    }

//------------------------------------------------------------------
//
// TileMapEditTest
//
//------------------------------------------------------------------
    public class TileMapEditTest : TileDemo
    {
        public TileMapEditTest()
        {
            CCTileMapAtlas map = new CCTileMapAtlas(s_TilesPng, s_LevelMapTga, 16, 16);
            // Create an Aliased Atlas
			map.IsAntialiased = false;

            CCSize s = map.ContentSize;
            CCLog.Log("ContentSize: {0}, {1}", s.Width, s.Height);

            // If you are not going to use the Map, you can free it now
            // [tilemap releaseMap);
            // And if you are going to use, it you can access the data with:
            Schedule(updateMap, 0.2f);

            AddChild(map, 0, kTagTileMap);
            map.AnchorPoint = (new CCPoint(0, 0));
        }

        protected virtual void AddedToNewScene()
        {
            base.AddedToNewScene();

			var map = (CCTileMapAtlas) this[kTagTileMap];
			map.Position = new CCPoint(-20, -200);
		}

        private void updateMap(float dt)
        {
            // IMPORTANT
            //   The only limitation is that you cannot change an empty, or assign an empty tile to a tile
            //   The value 0 not rendered so don't assign or change a tile with value 0

			var tilemap = (CCTileMapAtlas) this[kTagTileMap];

            //
            // For example you can iterate over all the tiles
            // using this code, but try to avoid the iteration
            // over all your tiles in every frame. It's very expensive
            //    for(int x=0; x < tilemap.tgaInfo.width; x++) {
            //        for(int y=0; y < tilemap.tgaInfo.height; y++) {
            //            ccColor3B c =[tilemap tileAt:new ccGridSize(x,y));
            //            if( c.r != 0 ) {
            //                ////----UXLOG("%d,%d = %d", x,y,c.r);
            //            }
            //        }
            //    }

            // NEW since v0.7
			var c = tilemap.TileAt(new CCGridSize(13, 21));
            c.R++;
            c.R %= 50;
            if (c.R == 0)
                c.R = 1;

            // NEW since v0.7
            tilemap.SetTile(c, new CCGridSize(13, 21));
        }

		public override string Title
		{
			get
			{
				return "Editable TileMapAtlas";
			}
		}
    }

//------------------------------------------------------------------
//
// TMXOrthoTest
//
//------------------------------------------------------------------
    public class TMXOrthoTest : TileDemo
    {
        public TMXOrthoTest()
        {
            //
            // Test orthogonal with 3d camera and anti-alias textures
            //
            // it should not flicker. No artifacts should appear
            //
            //CCLayerColor* color = new CCLayerColor( ccc4(64,64,64,255) );
            //addChild(color, -1);

            CCTMXTiledMap map = new CCTMXTiledMap("TileMaps/orthogonal-test2");
            AddChild(map, 0, kTagTileMap);

            CCSize s = map.ContentSize;
            CCLog.Log("ContentSize: {0}, {1}", s.Width, s.Height);

            /*
            CCArray pChildrenArray = map.getChildren();
            CCSpriteBatchNode* child = NULL;
            object* pObject = NULL;
            CCARRAY_FOREACH(pChildrenArray, pObject)
            {
                child = (CCSpriteBatchNode*) pObject;

                if (!child)
                    break;

                child.Texture.setAntiAliasTexParameters();
            }
            */

            float x, y, z;
            //map.Camera.GetEyeXyz(out x, out y, out z);
            //map.Camera.SetEyeXyz(x - 200, y, z + 300);
        }

        public override void OnEnter()
        {
            base.OnEnter();
            //Scene.Director.Projection = CCDirectorProjection.Projection3D;
        }

        public override void OnExit()
        {
            //Scene.Director.Projection = CCDirectorProjection.Projection2D;
            base.OnExit();
        }

		public override string Title
		{
			get
			{
				return "TMX Orthogonal test";
			}
		}
    }

//------------------------------------------------------------------
//
// TMXOrthoTest2
//
//------------------------------------------------------------------

    public class TMXOrthoTest2 : TileDemo
    {
        public TMXOrthoTest2()
        {
            CCTMXTiledMap map = new CCTMXTiledMap("TileMaps/orthogonal-test1");
            AddChild(map, 0, kTagTileMap);

            /*
            CCArray* pChildrenArray = map.getChildren();
            CCSpriteBatchNode* child = NULL;
            object* pObject = NULL;
            CCARRAY_FOREACH(pChildrenArray, pObject)
            {
                child = (CCSpriteBatchNode*) pObject;

                if (!child)
                    break;

                child.Texture.setAntiAliasTexParameters();
            }
            */

			map.RunAction(SCALE_2X_Half );
        }

		public override string Title
		{
			get
			{
				return "TMX Ortho test2";
			}
		}
    }

//------------------------------------------------------------------
//
// TMXOrthoTest3
//
//------------------------------------------------------------------
    public class TMXOrthoTest3 : TileDemo
    {
        public TMXOrthoTest3()
        {
            CCTMXTiledMap map = new CCTMXTiledMap("TileMaps/orthogonal-test3");
            AddChild(map, 0, kTagTileMap);

            /*
            CCArray* pChildrenArray = map.getChildren();
            CCSpriteBatchNode* child = NULL;
            object* pObject = NULL;
            CCARRAY_FOREACH(pChildrenArray, pObject)
            {
                child = (CCSpriteBatchNode*) pObject;

                if (!child)
                    break;

                child.Texture.setAntiAliasTexParameters();
            }
            */

            map.Scale = 0.2f;
            map.AnchorPoint = (new CCPoint(0.5f, 0.5f));
        }
		public override string Title
		{
			get
			{
				return "TMX anchorPoint test";
			}
		}
    }

//------------------------------------------------------------------
//
// TMXOrthoTest4
//
//------------------------------------------------------------------

    public class TMXOrthoTest4 : TileDemo
    {
        public TMXOrthoTest4()
        {
            CCTMXTiledMap map = new CCTMXTiledMap("TileMaps/orthogonal-test4");
            AddChild(map, 0, kTagTileMap);

            /*
            CCArray* pChildrenArray = map.getChildren();
            CCSpriteBatchNode* child = NULL;
            object* pObject = NULL;
            CCARRAY_FOREACH(pChildrenArray, pObject)
            {
                child = (CCSpriteBatchNode*) pObject;

                if (!child)
                    break;

                child.Texture.setAntiAliasTexParameters();
            }
            */

            map.AnchorPoint = (new CCPoint(0, 0));

            CCTMXLayer layer = map.LayerNamed("Layer 0");
            CCSize s = layer.LayerSize;

            CCSprite sprite;
            sprite = layer.TileAt(new CCPoint(0, 0));
            sprite.Scale = (2);
            sprite = layer.TileAt(new CCPoint(s.Width - 1, 0));
            sprite.Scale = (2);
            sprite = layer.TileAt(new CCPoint(0, s.Height - 1));
            sprite.Scale = (2);
            sprite = layer.TileAt(new CCPoint(s.Width - 1, s.Height - 1));
            sprite.Scale = (2);

            Schedule(removeSprite, 2);
        }

        private void removeSprite(float dt)
        {
            Unschedule(removeSprite);

            var map = (CCTMXTiledMap) GetChildByTag(kTagTileMap);
            CCTMXLayer layer = map.LayerNamed("Layer 0");
            CCSize s = layer.LayerSize;

            CCSprite sprite = layer.TileAt(new CCPoint(s.Width - 1, 0));
            layer.RemoveChild(sprite, true);
        }

		public override string Title
		{
			get
			{
				return "TMX width/height test";
			}
		}
    }

//------------------------------------------------------------------
//
// TMXReadWriteTest
//
//------------------------------------------------------------------

    public class TMXReadWriteTest : TileDemo
    {
        private uint m_gid;
        private uint m_gid2;

        public TMXReadWriteTest()
        {
            m_gid = 0;

            CCTMXTiledMap map = new CCTMXTiledMap("TileMaps/orthogonal-test2");
            AddChild(map, 0, kTagTileMap);

            CCTMXLayer layer = map.LayerNamed("Layer 0");
			layer.IsAntialiased = true;

            map.Scale = (1);

            CCSprite tile0 = layer.TileAt(new CCPoint(1, 63));
            CCSprite tile1 = layer.TileAt(new CCPoint(2, 63));
            CCSprite tile2 = layer.TileAt(new CCPoint(3, 62)); //new CCPoint(1,62));
            CCSprite tile3 = layer.TileAt(new CCPoint(2, 62));
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


            m_gid = layer.TileGIDAt(new CCPoint(0, 63));
            ////----UXLOG("Tile GID at:(0,63) is: %d", m_gid);

            Schedule(updateCol, 2.0f);
            Schedule(repaintWithGID, 2.0f);
            Schedule(removeTiles, 1.0f);

            ////----UXLOG("++++atlas quantity: %d", layer.textureAtlas().getTotalQuads());
            ////----UXLOG("++++children: %d", layer.getChildren().count() );

            m_gid2 = 0;
        }

        private void removeSprite(CCNode sender)
        {
            ////----UXLOG("removing tile: %x", sender);
            CCNode p = sender.Parent;

            if (p != null)
            {
                p.RemoveChild(sender, true);
            }

            //////----UXLOG("atlas quantity: %d", p.textureAtlas().totalQuads());
        }

        private void updateCol(float dt)
        {
            var map = (CCTMXTiledMap) GetChildByTag(kTagTileMap);
            var layer = (CCTMXLayer) map.GetChildByTag(0);

            ////----UXLOG("++++atlas quantity: %d", layer.textureAtlas().getTotalQuads());
            ////----UXLOG("++++children: %d", layer.getChildren().count() );


            CCSize s = layer.LayerSize;

            for (int y = 0; y < s.Height; y++)
            {
                layer.SetTileGID(m_gid2, new CCPoint(3, y));
            }

            m_gid2 = (m_gid2 + 1) % 80;
        }

        private void repaintWithGID(float dt)
        {
            var map = (CCTMXTiledMap) GetChildByTag(kTagTileMap);
            var layer = (CCTMXLayer) map.GetChildByTag(0);

            CCSize s = layer.LayerSize;
            for (int x = 0; x < s.Width; x++)
            {
                int y = (int) s.Height - 1;
                uint tmpgid = layer.TileGIDAt(new CCPoint(x, y));
                layer.SetTileGID(tmpgid + 1, new CCPoint(x, y));
            }
        }

        private void removeTiles(float dt)
        {
            Unschedule(removeTiles);

            var map = (CCTMXTiledMap) GetChildByTag(kTagTileMap);
            var layer = (CCTMXLayer) map.GetChildByTag(0);
            CCSize s = layer.LayerSize;

            for (int y = 0; y < s.Height; y++)
            {
                layer.RemoveTileAt(new CCPoint(5.0f, y));
            }
        }

		public override string Title
		{
			get
			{
				return "TMX Read/Write test";
			}
		}

        #region Nested type: SID

        private enum SID
        {
            SID_UPDATECOL = 100,
            SID_REPAINTWITHGID,
            SID_REMOVETILES
        }

        #endregion
    }

//------------------------------------------------------------------
//
// TMXHexTest
//
//------------------------------------------------------------------
    public class TMXHexTest : TileDemo
    {
        public TMXHexTest()
        {
            CCLayerColor color = new CCLayerColor(new CCColor4B(64, 64, 64, 255));
            AddChild(color, -1);

            CCTMXTiledMap map = new CCTMXTiledMap("TileMaps/hexa-test1");
            AddChild(map, 0, kTagTileMap);

            CCSize s = map.ContentSize;
        }

		public override string Title
		{
			get
			{
				return "TMX Hex tes";
			}
		}
    }

//------------------------------------------------------------------
//
// TMXIsoTest
//
//------------------------------------------------------------------
    public class TMXIsoTest : TileDemo
    {
        public TMXIsoTest()
        {
            CCLayerColor color = new CCLayerColor(new CCColor4B(64, 64, 64, 255));
            AddChild(color, -1);

            CCTMXTiledMap map = new CCTMXTiledMap("TileMaps/iso-test01");
            AddChild(map, 0, kTagTileMap);

            // move map to the center of the screen
            CCSize ms = map.MapSize;
            CCSize ts = map.TileSize;
            map.RunAction(new CCMoveTo (1.0f, new CCPoint(-ms.Width * ts.Width / 2, -ms.Height * ts.Height / 2)));
        }

		public override string Title
		{
			get
			{
				return "TMX Isometric test 0";
			}
		}
    }

//------------------------------------------------------------------
//
// TMXIsoTest1
//
//------------------------------------------------------------------
    public class TMXIsoTest1 : TileDemo
    {
        public TMXIsoTest1()
        {
            CCLayerColor color = new CCLayerColor(new CCColor4B(64, 64, 64, 255));
            AddChild(color, -1);

            CCTMXTiledMap map = new CCTMXTiledMap("TileMaps/iso-test11");
            AddChild(map, 0, kTagTileMap);

            CCSize s = map.ContentSize;

            map.AnchorPoint = (new CCPoint(0.5f, 0.5f));
        }

		public override string Title
		{
			get
			{
				return "TMX Isometric test + anchorPoint";
			}
		}
    }

//------------------------------------------------------------------
//
// TMXIsoTest2
//
//------------------------------------------------------------------
    public class TMXIsoTest2 : TileDemo
    {
        public TMXIsoTest2()
        {
            CCLayerColor color = new CCLayerColor(new CCColor4B(64, 64, 64, 255));
            AddChild(color, -1);

            CCTMXTiledMap map = new CCTMXTiledMap("TileMaps/iso-test22");
            AddChild(map, 0, kTagTileMap);

            CCSize s = map.ContentSize;

            // move map to the center of the screen
            CCSize ms = map.MapSize;
            CCSize ts = map.TileSize;
            map.RunAction(new CCMoveTo (1.0f, new CCPoint(-ms.Width * ts.Width / 2, -ms.Height * ts.Height / 2)));
        }

		public override string Title
		{
			get
			{
				return "TMX Isometric test 2";
			}
		}
    }

//------------------------------------------------------------------
//
// TMXUncompressedTest
//
//------------------------------------------------------------------
    public class TMXUncompressedTest : TileDemo
    {
        public TMXUncompressedTest()
        {
            CCLayerColor color = new CCLayerColor(new CCColor4B(64, 64, 64, 255));
            AddChild(color, -1);

            CCTMXTiledMap map = new CCTMXTiledMap("TileMaps/iso-test2-uncompressed");
            AddChild(map, 0, kTagTileMap);

            CCSize s = map.ContentSize;

            // move map to the center of the screen
            CCSize ms = map.MapSize;
            CCSize ts = map.TileSize;
            map.RunAction(new CCMoveTo (1.0f, new CCPoint(-ms.Width * ts.Width / 2, -ms.Height * ts.Height / 2)));

            /*
            // testing release map
            CCArray* pChildrenArray = map.getChildren();
            CCTMXLayer layer;
            object* pObject = NULL;
            CCARRAY_FOREACH(pChildrenArray, pObject)
            {
                layer = (CCTMXLayer) pObject;

                if (!layer)
                    break;

                layer.releaseMap();
            }
            */
        }

		public override string Title
		{
			get
			{
				return "TMX Uncompressed test";
			}
		}
    }

//------------------------------------------------------------------
//
// TMXTilesetTest
//
//------------------------------------------------------------------
    public class TMXTilesetTest : TileDemo
    {
        public TMXTilesetTest()
        {
            CCTMXTiledMap map = new CCTMXTiledMap("TileMaps/orthogonal-test5");
            AddChild(map, 0, kTagTileMap);

            CCSize s = map.ContentSize;

            CCTMXLayer layer;
            layer = map.LayerNamed("Layer 0");
			layer.IsAntialiased = true;

            layer = map.LayerNamed("Layer 1");
			layer.IsAntialiased = true;

            layer = map.LayerNamed("Layer 2");
			layer.IsAntialiased = true;
        }

		public override string Title
		{
			get
			{
				return "TMX Tileset test";
			}
		}
    }

//------------------------------------------------------------------
//
// TMXOrthoObjectsTest
//
//------------------------------------------------------------------
    public class TMXOrthoObjectsTest : TileDemo
    {
        public TMXOrthoObjectsTest()
        {
            CCTMXTiledMap map = new CCTMXTiledMap("TileMaps/ortho-objects");
            AddChild(map, -1, kTagTileMap);

            CCSize s = map.ContentSize;

            /*
            ////----UXLOG("---. Iterating over all the group objets");
            CCTMXObjectGroup group = map.objectGroupNamed("Object Group 1");
            CCArray objects = group.getObjects();

            CCDictionary* dict = NULL;
            object* pObj = NULL;
            CCARRAY_FOREACH(objects, pObj)
            {
                dict = (CCDictionary*) pObj; //dynamic_cast<CCStringToStringDictionary*>(*it);

                if (!dict)
                    break;

                ////----UXLOG("object: %x", dict);
            }
            */

            ////----UXLOG("---. Fetching 1 object by name");
            // CCStringToStringDictionary* platform = group.objectNamed("platform");
            ////----UXLOG("platform: %x", platform);
        }

        protected override void Draw()
        {
            var map = (CCTMXTiledMap) GetChildByTag(kTagTileMap);
            CCTMXObjectGroup group = map.ObjectGroupNamed("Object Group 1");
            
            List<Dictionary<string, string>> objects = group.Objects;
            foreach (var dict in objects)
            {
                float x = float.Parse(dict["x"]);
                float y = float.Parse(dict["y"]);
                float width = (dict.ContainsKey("width") ? float.Parse(dict["width"]) : 0f);
                float height = (dict.ContainsKey("height") ? float.Parse(dict["height"]) : 0f);

                var color = new CCColor4B(255, 255, 0, 255);

                CCDrawingPrimitives.Begin();
                CCDrawingPrimitives.DrawLine(this.AffineWorldTransform.Transform(new CCPoint(x, y)), this.AffineWorldTransform.Transform(new CCPoint((x + width), y)), color);
                CCDrawingPrimitives.DrawLine(this.AffineWorldTransform.Transform(new CCPoint((x + width), y)), this.AffineWorldTransform.Transform(new CCPoint((x + width), (y + height))), color);
                CCDrawingPrimitives.DrawLine(this.AffineWorldTransform.Transform(new CCPoint((x + width), (y + height))), this.AffineWorldTransform.Transform(new CCPoint(x, (y + height))), color);
                CCDrawingPrimitives.DrawLine(this.AffineWorldTransform.Transform(new CCPoint(x, (y + height))), this.AffineWorldTransform.Transform(new CCPoint(x, y)), color);
                CCDrawingPrimitives.End();

                //glLineWidth(1);
            }
        }

		public override string Title
		{
			get
			{
				return "TMX Ortho object test";
			}
		}

		public override string Subtitle
		{
			get
			{
				return "You should see a white box around the 3 platforms";
			}
		}
    }

//------------------------------------------------------------------
//
// TMXIsoObjectsTest
//
//------------------------------------------------------------------
    public class TMXIsoObjectsTest : TileDemo
    {
        public TMXIsoObjectsTest()
        {
            CCTMXTiledMap map = new CCTMXTiledMap("TileMaps/iso-test-objectgroup");
            AddChild(map, -1, kTagTileMap);

            CCSize s = map.ContentSize;

            /*
            CCTMXObjectGroup group = map.objectGroupNamed("Object Group 1");

            //UxMutableArray* objects = group.objects();
            CCArray* objects = group.getObjects();
            //UxMutableDictionary<string>* dict;
            CCDictionary* dict;
            object* pObj = NULL;
            CCARRAY_FOREACH(objects, pObj)
            {
                dict = (CCDictionary*) pObj;

                if (!dict)
                    break;

                ////----UXLOG("object: %x", dict);
            }
            */
        }

        protected override void Draw()
        {
            var map = (CCTMXTiledMap) GetChildByTag(kTagTileMap);
            CCTMXObjectGroup group = map.ObjectGroupNamed("Object Group 1");

            List<Dictionary<string, string>> objects = group.Objects;
            foreach (var dict in objects)
            {
                int x = int.Parse(dict["x"]);
                int y = int.Parse(dict["y"]);
                int width = dict.ContainsKey("width") ? int.Parse(dict["width"]) : 0;
                int height = dict.ContainsKey("height") ? int.Parse(dict["height"]) : 0;

                //glLineWidth(3);

                var color = new CCColor4B(255, 255, 0, 255);

                CCDrawingPrimitives.Begin();
                CCDrawingPrimitives.DrawLine(new CCPoint(x, y), new CCPoint(x + width, y), color);
                CCDrawingPrimitives.DrawLine(new CCPoint(x + width, y), new CCPoint(x + width, y + height), color);
                CCDrawingPrimitives.DrawLine(new CCPoint(x + width, y + height), new CCPoint(x, y + height), color);
                CCDrawingPrimitives.DrawLine(new CCPoint(x, y + height), new CCPoint(x, y), color);
                CCDrawingPrimitives.End();

                //glLineWidth(1);
            }
        }

		public override string Title
		{
			get
			{
				return "TMX Iso object test";
			}
		}

		public override string Subtitle
		{
			get
			{
				return "You need to parse them manually. See bug #810";
			}
		}
    }


//------------------------------------------------------------------
//
// TMXResizeTest
//
//------------------------------------------------------------------
    public class TMXResizeTest : TileDemo
    {
        public TMXResizeTest()
        {
            CCTMXTiledMap map = new CCTMXTiledMap("TileMaps/orthogonal-test5");
            AddChild(map, 0, kTagTileMap);

            CCSize s = map.ContentSize;

            CCTMXLayer layer;
            layer = map.LayerNamed("Layer 0");

            CCSize ls = layer.LayerSize;
            for (uint y = 0; y < ls.Height; y++)
            {
                for (uint x = 0; x < ls.Width; x++)
                {
                    layer.SetTileGID(1, new CCPoint(x, y));
                }
            }
        }

		public override string Title
		{
			get
			{
				return "TMX resize test";
			}
		}

		public override string Subtitle
		{
			get
			{
				return "Should not crash. Testing issue #740";

			}
		}
    }

//------------------------------------------------------------------
//
// TMXIsoZorder
//
//------------------------------------------------------------------
    public class TMXIsoZorder : TileDemo
    {
        private readonly CCSprite m_tamara;

		static CCMoveBy move = new CCMoveBy (10, new CCPoint(300, 250));
		static CCFiniteTimeAction back = move.Reverse();

        public TMXIsoZorder()
        {
            CCTMXTiledMap map = new CCTMXTiledMap("TileMaps/iso-test-zorder");
            AddChild(map, 0, kTagTileMap);

            CCSize s = map.ContentSize;
            CCLog.Log("ContentSize: {0}, {1}", s.Width, s.Height);

            m_tamara = new CCSprite(pathSister1);
            map.AddChild(m_tamara, map.Children.Count);

            m_tamara.AnchorPoint = (new CCPoint(0.5f, 0));

			m_tamara.RepeatForever(move, back);

            Schedule(repositionSprite);
        }

        protected override void AddedToNewScene()
        {
            base.AddedToNewScene();

			var map = (CCTMXTiledMap)this[kTagTileMap];
			CCSize s = map.ContentSize;
			CCLog.Log("ContentSize: {0}, {1}", s.Width, s.Height);
			map.Position = new CCPoint(-s.Width / 2, 0);

			float mapWidth = map.MapSize.Width * map.TileSize.Width;
            m_tamara.Position = Scene.ScreenToWorldspace(new CCPoint (mapWidth / 2, 0));
		}


        public override void OnExit()
        {
            Unschedule(repositionSprite);
            base.OnExit();
        }

        private void repositionSprite(float dt)
        {
            CCPoint p = Scene.WorldToScreenspace(m_tamara.Position);
            CCNode map = this[kTagTileMap];

            // there are only 4 layers. (grass and 3 trees layers)
            // if tamara < 48, z=4
            // if tamara < 96, z=3
            // if tamara < 144,z=2

            int newZ = (int)(4 - (p.Y / 48));
            newZ = Math.Max(newZ, 0);

            if (m_tamara.ZOrder != newZ)
            {
                map.ReorderChild(m_tamara, newZ);
            }
        }

		public override string Title
		{
			get
			{
				return "TMX Iso Zorder";
			}
		}

		public override string Subtitle
		{
			get
			{
				return "Sprite should hide behind the trees";

			}
		}
    }

//------------------------------------------------------------------
//
// TMXOrthoZorder
//
//------------------------------------------------------------------
    public class TMXOrthoZorder : TileDemo
    {
        private readonly CCSprite m_tamara;

		static CCMoveBy move = new CCMoveBy (10, new CCPoint(400, 450));
		static CCFiniteTimeAction back = move.Reverse();

		public TMXOrthoZorder()
        {
            CCTMXTiledMap map = new CCTMXTiledMap("TileMaps/orthogonal-test-zorder");
            AddChild(map, 0, kTagTileMap);

            m_tamara = new CCSprite(pathSister1);
            map.AddChild(m_tamara, map.Children.Count);
			m_tamara.AnchorPoint = CCPoint.AnchorMiddleBottom;

			m_tamara.RepeatForever(move, back);

            Schedule(repositionSprite);
        }

        private void repositionSprite(float dt)
        {
            CCPoint p = Scene.WorldToScreenspace(m_tamara.Position);
			CCNode map = this[kTagTileMap];

            // there are only 4 layers. (grass and 3 trees layers)
            // if tamara < 81, z=4
            // if tamara < 162, z=3
            // if tamara < 243,z=2

            // -10: customization for this particular sample
            int newZ = (int)(4 - ((p.Y - 10) / 81));
            newZ = Math.Max(newZ, 0);

            map.ReorderChild(m_tamara, newZ);
        }

		public override string Title
		{
			get
			{
				return "TMX Ortho Zorder";
			}
		}

		public override string Subtitle
		{
			get
			{
				return "Sprite should hide behind the trees";
			}
		}
    }


//------------------------------------------------------------------
//
// TMXIsoVertexZ
//
//------------------------------------------------------------------
    public class TMXIsoVertexZ : TileDemo
    {
        readonly CCSprite m_tamara;
        CCTMXTiledMap map;


        public TMXIsoVertexZ()
        {
            map = new CCTMXTiledMap("TileMaps/iso-test-vertexz");
            AddChild(map, 0, kTagTileMap);

            // because I'm lazy, I'm reusing a tile as an sprite, but since this method uses vertexZ, you
            // can use any CCSprite and it will work OK.
            CCTMXLayer layer = map.LayerNamed("Trees");
            m_tamara = layer.TileAt(29, 29);
        }

        #region Setup content

        public override void OnEnter()
        {
            base.OnEnter(); CCSize windowSize = Scene.VisibleBoundsWorldspace.Size;

            CCSize s = map.ContentSize;
            map.Position = new CCPoint(-s.Width / 2, 0);

            CCMoveBy move = new CCMoveBy (10, new CCPoint(300, 250));
            CCFiniteTimeAction back = move.Reverse();
            CCSequence seq = new CCSequence(move, back);
            m_tamara.RunAction(new CCRepeatForever (seq));

            Schedule(repositionSprite);
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
			{
				return "TMX Iso VertexZ";
			}
		}

		public override string Subtitle
		{
			get
			{
				return "Sprite should hide behind the trees";

			}
		}
    }


//------------------------------------------------------------------
//
// TMXOrthoVertexZ
//
//------------------------------------------------------------------
    public class TMXOrthoVertexZ : TileDemo
    {
        readonly CCSprite m_tamara;

        public TMXOrthoVertexZ()
        {
            CCTMXTiledMap map = new CCTMXTiledMap("TileMaps/orthogonal-test-vertexz");
            AddChild(map, 0, kTagTileMap);

            // because I'm lazy, I'm reusing a tile as an sprite, but since this method uses vertexZ, you
            // can use any CCSprite and it will work OK.
            CCTMXLayer layer = map.LayerNamed("trees");
            m_tamara = layer.TileAt(0, 11);
            CCLog.Log("tamara vertexZ: {0}", m_tamara.VertexZ);
        }

        #region Setup content

        public override void OnEnter()
        {
            base.OnEnter(); CCSize windowSize = Scene.VisibleBoundsWorldspace.Size;

            var move = new CCMoveBy (10, Scene.ScreenToWorldspace(new CCPoint(400, 450)));
            var back = move.Reverse();
			m_tamara.RepeatForever(move, back);

            Schedule(repositionSprite);
        }

        #endregion Setup content

        private void repositionSprite(float dt)
        {
            // tile height is 101x81
            // map size: 12x12
            CCPoint p = m_tamara.Position;
            m_tamara.VertexZ = -((p.Y + 81) / 81);
        }

		public override string Title
		{
			get
			{
				return "TMX Ortho vertexZ";
			}
		}

		public override string Subtitle
		{
			get
			{
				return "Sprite should hide behind the trees";
			}
		}
    }

//------------------------------------------------------------------
//
// TMXIsoMoveLayer
//
//------------------------------------------------------------------
    public class TMXIsoMoveLayer : TileDemo
    {
        public TMXIsoMoveLayer()
        {
            CCTMXTiledMap map = new CCTMXTiledMap("TileMaps/iso-test-movelayer");
            AddChild(map, 0, kTagTileMap);

            map.Position = new CCPoint(-700, -50);
        }

		public override string Title
		{
			get
			{
				return "TMX Iso Move Layer";
			}
		}

		public override string Subtitle
		{
			get
			{
				return "Trees should be horizontally aligned";
			}
		}
    }

//------------------------------------------------------------------
//
// TMXOrthoMoveLayer
//
//------------------------------------------------------------------
    public class TMXOrthoMoveLayer : TileDemo
    {
        public TMXOrthoMoveLayer()
        {
            CCTMXTiledMap map = new CCTMXTiledMap("TileMaps/orthogonal-test-movelayer");
            AddChild(map, 0, kTagTileMap);
        }

		public override string Title
		{
			get
			{
				return "TMX Ortho Move Layer";
			}
		}

		public override string Subtitle
		{
			get
			{
				return "Trees should be horizontally aligned";
			}
		}
    }

//------------------------------------------------------------------
//
// TMXTilePropertyTest
//
//------------------------------------------------------------------

    public class TMXTilePropertyTest : TileDemo
    {
        public TMXTilePropertyTest()
        {
            CCTMXTiledMap map = new CCTMXTiledMap("TileMaps/ortho-tile-property");
            AddChild(map, 0, kTagTileMap);

            for (uint i = 1; i <= 20; i++)
            {
				CCLog.Log("GID:{0}, Properties:{1}", i, map.PropertiesForGID(i));
            }
        }

		public override string Title
		{
			get
			{
				return "TMX Tile Property Test";
			}
		}

		public override string Subtitle
		{
			get
			{
				return "In the console you should see tile properties";

			}
		}
    }

//------------------------------------------------------------------
//
// TMXOrthoFlipTest
//
//------------------------------------------------------------------
    public class TMXOrthoFlipTest : TileDemo
    {
        public TMXOrthoFlipTest()
        {
            CCTMXTiledMap map = new CCTMXTiledMap("TileMaps/ortho-rotation-test");
            AddChild(map, 0, kTagTileMap);

			foreach (var mapChild in map.Children)
			{
				var child = (CCSpriteBatchNode)mapChild;
				child.Texture.IsAntialiased = true;
			}
				
			map.RunAction(SCALE_2X_Half );
        }

		public override string Title
		{
			get
			{
				return "TMX tile flip test";
			}
		}
    }

//------------------------------------------------------------------
//
// TMXOrthoFlipRunTimeTest
//
//------------------------------------------------------------------
    public class TMXOrthoFlipRunTimeTest : TileDemo
    {
        public TMXOrthoFlipRunTimeTest()
        {
            CCTMXTiledMap map = new CCTMXTiledMap("TileMaps/ortho-rotation-test");
            AddChild(map, 0, kTagTileMap);

			foreach (var mapChild in map.Children)
			{
				var child = (CCSpriteBatchNode)mapChild;
				child.Texture.IsAntialiased = true;
			}
				
			map.RunAction(SCALE_2X_Half );

            Schedule(flipIt, 1.0f);
        }

		public override string Title
		{
			get
			{
				return "TMX tile flip run time test";
			}
		}

		public override string Subtitle
		{
			get
			{
				return "in 2 sec bottom left tiles will flip";
			}
		}

        private void flipIt(float dt)
        {
            var map = (CCTMXTiledMap) this[kTagTileMap];
            CCTMXLayer layer = map.LayerNamed("Layer 0");

            //blue diamond 
            var tileCoord = new CCPoint(1, 10);
            uint flags;
            uint GID = layer.TileGIDAt(tileCoord, out flags);
            // Vertical
            if ((flags & CCTMXTileFlags.Vertical) != 0)
                flags &= ~CCTMXTileFlags.Vertical;
            else
                flags |= CCTMXTileFlags.Vertical;
            layer.SetTileGID(GID, tileCoord, flags);


            tileCoord = new CCPoint(1, 8);
            GID = layer.TileGIDAt(tileCoord, out flags);
            // Vertical
            if ((flags & CCTMXTileFlags.Vertical) != 0)
                flags &= ~CCTMXTileFlags.Vertical;
            else
                flags |= CCTMXTileFlags.Vertical;
            layer.SetTileGID(GID, tileCoord, flags);


            tileCoord = new CCPoint(2, 8);
            GID = layer.TileGIDAt(tileCoord, out flags);
            // Horizontal
            if ((flags & CCTMXTileFlags.Horizontal) != 0)
                flags &= ~CCTMXTileFlags.Horizontal;
            else
                flags |= CCTMXTileFlags.Horizontal;
            layer.SetTileGID(GID, tileCoord, flags);
        }
    }

//------------------------------------------------------------------
//
// TMXOrthoFromXMLTest
//
//------------------------------------------------------------------
    public class TMXOrthoFromXMLTest : TileDemo
    {
        public TMXOrthoFromXMLTest()
        {
            CCTMXTiledMap map = new CCTMXTiledMap("TileMaps/orthogonal-test1");
            AddChild(map, 0, kTagTileMap);

			foreach (var mapChild in map.Children)
			{
				var child = (CCSpriteBatchNode)mapChild;
				child.Texture.IsAntialiased = true;
			}
				
			map.RunAction(SCALE_2X_Half );
        }

		public override string Title
		{
			get
			{
				return "TMX created from XML test";
			}
		}
    }

//------------------------------------------------------------------
//
// TMXBug987
//
//------------------------------------------------------------------
    public class TMXBug987 : TileDemo
    {
        public TMXBug987()
        {
            CCTMXTiledMap map = new CCTMXTiledMap("TileMaps/orthogonal-test6");
            AddChild(map, 0, kTagTileMap);

            /*
            CCArray* childs = map.getChildren();
            CCTMXLayer* node;
            object* pObject = NULL;
            CCARRAY_FOREACH(childs, pObject)
            {
                node = (CCTMXLayer*) pObject;
                CC_BREAK_IF(!node);
                node.Texture.setAntiAliasTexParameters();
            }
            */

			map.AnchorPoint = CCPoint.AnchorLowerLeft;
            CCTMXLayer layer = map.LayerNamed("Tile Layer 1");
			layer.SetTileGID(3, new CCPoint(2, 2));

        }

        protected virtual void AddedToNewScene()
        {
            base.AddedToNewScene();

			var map = (CCTMXTiledMap)this[kTagTileMap];
			//map.Position = new CCPoint(100, 100);
		}

		public override string Title
		{
			get
			{
				return "TMX Bug 987";

			}
		}

		public override string Subtitle
		{
			get
			{
				return "You should see an square";

			}
		}
    }

//------------------------------------------------------------------
//
// TMXBug787
//
//------------------------------------------------------------------
    public class TMXBug787 : TileDemo
    {
        public TMXBug787()
        {
            CCTMXTiledMap map = new CCTMXTiledMap("TileMaps/iso-test-bug787");
            AddChild(map, 0, kTagTileMap);

            map.Scale = (0.25f);
        }

		public override string Title
		{
			get
			{
				return "TMX Bug 787";
			}
		}

		public override string Subtitle
		{
			get
			{
				return "You should see a map";
			}
		}
    }

//------------------------------------------------------------------
//
// TileDemo
//
//------------------------------------------------------------------
    public class TileDemo : TestNavigationLayer
    {
        protected const string s_TilesPng = "TileMaps/tiles";
        protected const string s_LevelMapTga = "TileMaps/levelmap";
		protected const string pathSister1 = TestResource.s_pPathSister1;
        public const int kTagTileMap = 1;

		protected CCScaleBy SCALE_2X_Half = new CCScaleBy(2, 0.5f);

        public TileDemo()
        {
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
            CCNode node = this[kTagTileMap];
            node.Position += diff;
        }
    }

    public class TileMapTestScene : TestScene
    {
        private static int sceneIdx = -1;
#if XBOX || OUYA
        private static int MAX_LAYER = 28;
#else
        private static int MAX_LAYER = 28;
#endif

        private static CCLayer createTileMapLayer(int nIndex)
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
                    return new TileMapTest();
                case 24:
                    return new TileMapEditTest();
                case 25:
                    return new TMXBug987();
                case 26:
                    return new TMXBug787();
                case 27:
                    return new TMXGIDObjectsTest();
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

            Window.IsUseDepthTesting = true;

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
        public TMXGIDObjectsTest()
        {
            CCTMXTiledMap map = new CCTMXTiledMap("TileMaps/test-object-layer");
            AddChild(map, -1, kTagTileMap);
        }

        protected override void Draw()
        {
            var map = (CCTMXTiledMap) GetChildByTag(kTagTileMap);
            CCTMXObjectGroup group = map.ObjectGroupNamed("Object Layer 1");

            foreach (var dict in group.Objects)
            {
                int x = int.Parse(dict["x"]);
                int y = int.Parse(dict["y"]);
                int width = dict.ContainsKey("width") ? int.Parse(dict["width"]) : 0;
                int height = dict.ContainsKey("height") ? int.Parse(dict["height"]) : 0;

                //glLineWidth(3);

                var color = new CCColor4B(255, 255, 0, 255);

                CCDrawingPrimitives.Begin();
                CCDrawingPrimitives.DrawLine(new CCPoint(x, y), new CCPoint(x + width, y), color);
                CCDrawingPrimitives.DrawLine(new CCPoint(x + width, y), new CCPoint(x + width, y + height), color);
                CCDrawingPrimitives.DrawLine(new CCPoint(x + width, y + height), new CCPoint(x, y + height), color);
                CCDrawingPrimitives.DrawLine(new CCPoint(x, y + height), new CCPoint(x, y), color);
                CCDrawingPrimitives.End();

                //glLineWidth(1);
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
}