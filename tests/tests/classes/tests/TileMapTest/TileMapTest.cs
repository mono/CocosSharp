using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using cocos2d;

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
            CCTileMapAtlas map = CCTileMapAtlas.Create(s_TilesPng, s_LevelMapTga, 16, 16);
            // Convert it to "alias" (GL_LINEAR filtering)
            map.Texture.SetAntiAliasTexParameters();

            CCSize s = map.ContentSize;
            CCLog.Log("ContentSize: {0}, {1}", s.Width, s.Height);

            // If you are not going to use the Map, you can free it now
            // NEW since v0.7
            map.ReleaseMap();

            AddChild(map, 0, kTagTileMap);

            map.AnchorPoint = (new CCPoint(0, 0.5f));

            CCScaleBy scale = new CCScaleBy(4, 0.8f);
            CCFiniteTimeAction scaleBack = scale.Reverse();

            var seq = CCSequence.FromActions(scale, scaleBack);

            map.RunAction(new CCRepeatForever ((CCActionInterval)seq));
        }

        public override string title()
        {
            return "TileMapAtlas";
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
            CCTileMapAtlas map = CCTileMapAtlas.Create(s_TilesPng, s_LevelMapTga, 16, 16);
            // Create an Aliased Atlas
            map.Texture.SetAliasTexParameters();

            CCSize s = map.ContentSize;
            CCLog.Log("ContentSize: {0}, {1}", s.Width, s.Height);

            // If you are not going to use the Map, you can free it now
            // [tilemap releaseMap);
            // And if you are going to use, it you can access the data with:
            Schedule(updateMap, 0.2f);

            AddChild(map, 0, kTagTileMap);

            map.AnchorPoint = (new CCPoint(0, 0));
            map.Position = new CCPoint(-20, -200);
        }

        private void updateMap(float dt)
        {
            // IMPORTANT
            //   The only limitation is that you cannot change an empty, or assign an empty tile to a tile
            //   The value 0 not rendered so don't assign or change a tile with value 0

            var tilemap = (CCTileMapAtlas) GetChildByTag(kTagTileMap);

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
            Color c = tilemap.TileAt(new CCGridSize(13, 21));
            c.R++;
            c.R %= 50;
            if (c.R == 0)
                c.R = 1;

            // NEW since v0.7
            tilemap.SetTile(c, new CCGridSize(13, 21));
        }

        public override string title()
        {
            return "Editable TileMapAtlas";
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
            //CCLayerColor* color = CCLayerColor.Create( ccc4(64,64,64,255) );
            //addChild(color, -1);

            CCTMXTiledMap map = CCTMXTiledMap.Create("TileMaps/orthogonal-test2");
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
            map.Camera.GetEyeXyz(out x, out y, out z);
            map.Camera.SetEyeXyz(x - 200, y, z + 300);
        }

        public override void OnEnter()
        {
            base.OnEnter();
            CCDirector.SharedDirector.Projection = ccDirectorProjection.kCCDirectorProjection3D;
        }

        public override void OnExit()
        {
            CCDirector.SharedDirector.Projection = ccDirectorProjection.kCCDirectorProjection2D;
            base.OnExit();
        }

        public override string title()
        {
            return "TMX Orthogonal test";
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
            CCTMXTiledMap map = CCTMXTiledMap.Create("TileMaps/orthogonal-test1");
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

            map.RunAction(new CCScaleBy(2, 0.5f));
        }

        public override string title()
        {
            return "TMX Ortho test2";
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
            CCTMXTiledMap map = CCTMXTiledMap.Create("TileMaps/orthogonal-test3");
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

        public override string title()
        {
            return "TMX anchorPoint test";
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
            CCTMXTiledMap map = CCTMXTiledMap.Create("TileMaps/orthogonal-test4");
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

        public override string title()
        {
            return "TMX width/height test";
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

            CCTMXTiledMap map = CCTMXTiledMap.Create("TileMaps/orthogonal-test2");
            AddChild(map, 0, kTagTileMap);

            CCTMXLayer layer = map.LayerNamed("Layer 0");
            layer.Texture.SetAntiAliasTexParameters();

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
            CCCallFuncN finish = CCCallFuncN.Create(removeSprite);
            CCSequence seq0 = CCSequence.FromActions(move, rotate, scale, opacity, fadein, scaleback, finish);
            var seq1 = (CCActionInterval) (seq0.Copy());
            var seq2 = (CCActionInterval) (seq0.Copy());
            var seq3 = (CCActionInterval) (seq0.Copy());

            tile0.RunAction(seq0);
            tile1.RunAction(seq1);
            tile2.RunAction(seq2);
            tile3.RunAction(seq3);


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


        public override string title()
        {
            return "TMX Read/Write test";
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
            CCLayerColor color = CCLayerColor.Create(new CCColor4B(64, 64, 64, 255));
            AddChild(color, -1);

            CCTMXTiledMap map = CCTMXTiledMap.Create("TileMaps/hexa-test1");
            AddChild(map, 0, kTagTileMap);

            CCSize s = map.ContentSize;
        }

        public override string title()
        {
            return "TMX Hex tes";
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
            CCLayerColor color = CCLayerColor.Create(new CCColor4B(64, 64, 64, 255));
            AddChild(color, -1);

            CCTMXTiledMap map = CCTMXTiledMap.Create("TileMaps/iso-test01");
            AddChild(map, 0, kTagTileMap);

            // move map to the center of the screen
            CCSize ms = map.MapSize;
            CCSize ts = map.TileSize;
            map.RunAction(new CCMoveTo (1.0f, new CCPoint(-ms.Width * ts.Width / 2, -ms.Height * ts.Height / 2)));
        }

        public override string title()
        {
            return "TMX Isometric test 0";
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
            CCLayerColor color = CCLayerColor.Create(new CCColor4B(64, 64, 64, 255));
            AddChild(color, -1);

            CCTMXTiledMap map = CCTMXTiledMap.Create("TileMaps/iso-test11");
            AddChild(map, 0, kTagTileMap);

            CCSize s = map.ContentSize;

            map.AnchorPoint = (new CCPoint(0.5f, 0.5f));
        }

        public override string title()
        {
            return "TMX Isometric test + anchorPoint";
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
            CCLayerColor color = CCLayerColor.Create(new CCColor4B(64, 64, 64, 255));
            AddChild(color, -1);

            CCTMXTiledMap map = CCTMXTiledMap.Create("TileMaps/iso-test22");
            AddChild(map, 0, kTagTileMap);

            CCSize s = map.ContentSize;

            // move map to the center of the screen
            CCSize ms = map.MapSize;
            CCSize ts = map.TileSize;
            map.RunAction(new CCMoveTo (1.0f, new CCPoint(-ms.Width * ts.Width / 2, -ms.Height * ts.Height / 2)));
        }

        public override string title()
        {
            return "TMX Isometric test 2";
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
            CCLayerColor color = CCLayerColor.Create(new CCColor4B(64, 64, 64, 255));
            AddChild(color, -1);

            CCTMXTiledMap map = CCTMXTiledMap.Create("TileMaps/iso-test2-uncompressed");
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

        public override string title()
        {
            return "TMX Uncompressed test";
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
            CCTMXTiledMap map = CCTMXTiledMap.Create("TileMaps/orthogonal-test5");
            AddChild(map, 0, kTagTileMap);

            CCSize s = map.ContentSize;

            CCTMXLayer layer;
            layer = map.LayerNamed("Layer 0");
            layer.Texture.SetAntiAliasTexParameters();

            layer = map.LayerNamed("Layer 1");
            layer.Texture.SetAntiAliasTexParameters();

            layer = map.LayerNamed("Layer 2");
            layer.Texture.SetAntiAliasTexParameters();
        }

        public override string title()
        {
            return "TMX Tileset test";
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
            CCTMXTiledMap map = CCTMXTiledMap.Create("TileMaps/ortho-objects");
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

        public override void Draw()
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
                CCDrawingPrimitives.DrawLine(new CCPoint(x, y), new CCPoint((x + width), y), color);
                CCDrawingPrimitives.DrawLine(new CCPoint((x + width), y), new CCPoint((x + width), (y + height)), color);
                CCDrawingPrimitives.DrawLine(new CCPoint((x + width), (y + height)), new CCPoint(x, (y + height)), color);
                CCDrawingPrimitives.DrawLine(new CCPoint(x, (y + height)), new CCPoint(x, y), color);
                CCDrawingPrimitives.End();

                //glLineWidth(1);
            }
        }

        public override string title()
        {
            return "TMX Ortho object test";
        }

        public override string subtitle()
        {
            return "You should see a white box around the 3 platforms";
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
            CCTMXTiledMap map = CCTMXTiledMap.Create("TileMaps/iso-test-objectgroup");
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

        public override void Draw()
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

        public override string title()
        {
            return "TMX Iso object test";
        }

        public override string subtitle()
        {
            return "You need to parse them manually. See bug #810";
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
            CCTMXTiledMap map = CCTMXTiledMap.Create("TileMaps/orthogonal-test5");
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

        public override string title()
        {
            return "TMX resize test";
        }

        public override string subtitle()
        {
            return "Should not crash. Testing issue #740";
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

        public TMXIsoZorder()
        {
            CCTMXTiledMap map = CCTMXTiledMap.Create("TileMaps/iso-test-zorder");
            AddChild(map, 0, kTagTileMap);

            CCSize s = map.ContentSize;
            CCLog.Log("ContentSize: {0}, {1}", s.Width, s.Height);
            map.Position = new CCPoint(-s.Width / 2, 0);

            m_tamara = new CCSprite(s_pPathSister1);
            map.AddChild(m_tamara, map.Children.Count);
            float mapWidth = map.MapSize.Width * map.TileSize.Width;
            m_tamara.Position = CCMacros.CCPointPixelsToPoints(new CCPoint(mapWidth / 2, 0));
            m_tamara.AnchorPoint = (new CCPoint(0.5f, 0));


            CCMoveBy move = new CCMoveBy (10, new CCPoint(300, 250));
            CCFiniteTimeAction back = move.Reverse();
            CCSequence seq = CCSequence.FromActions(move, back);
            m_tamara.RunAction(new CCRepeatForever (seq));

            Schedule(repositionSprite);
        }

        public override void OnExit()
        {
            Unschedule(repositionSprite);
            base.OnExit();
        }

        private void repositionSprite(float dt)
        {
            CCPoint p = m_tamara.Position;
            p = CCMacros.CCPointPointsToPixels(p);
            CCNode map = GetChildByTag(kTagTileMap);

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

        public override string title()
        {
            return "TMX Iso Zorder";
        }

        public override string subtitle()
        {
            return "Sprite should hide behind the trees";
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

        public TMXOrthoZorder()
        {
            CCTMXTiledMap map = CCTMXTiledMap.Create("TileMaps/orthogonal-test-zorder");
            AddChild(map, 0, kTagTileMap);

            CCSize s = map.ContentSize;

            m_tamara = new CCSprite(s_pPathSister1);
            map.AddChild(m_tamara, map.Children.Count);
            m_tamara.AnchorPoint = (new CCPoint(0.5f, 0));


            CCMoveBy move = new CCMoveBy (10, new CCPoint(400, 450));
            CCFiniteTimeAction back = move.Reverse();
            CCSequence seq = CCSequence.FromActions(move, back);
            m_tamara.RunAction(new CCRepeatForever (seq));

            Schedule(repositionSprite);
        }

        private void repositionSprite(float dt)
        {
            CCPoint p = m_tamara.Position;
            p = CCMacros.CCPointPointsToPixels(p);
            CCNode map = GetChildByTag(kTagTileMap);

            // there are only 4 layers. (grass and 3 trees layers)
            // if tamara < 81, z=4
            // if tamara < 162, z=3
            // if tamara < 243,z=2

            // -10: customization for this particular sample
            int newZ = (int)(4 - ((p.Y - 10) / 81));
            newZ = Math.Max(newZ, 0);

            map.ReorderChild(m_tamara, newZ);
        }

        public override string title()
        {
            return "TMX Ortho Zorder";
        }

        public override string subtitle()
        {
            return "Sprite should hide behind the trees";
        }
    }


//------------------------------------------------------------------
//
// TMXIsoVertexZ
//
//------------------------------------------------------------------
    public class TMXIsoVertexZ : TileDemo
    {
        private readonly CCSprite m_tamara;

        public TMXIsoVertexZ()
        {
            CCTMXTiledMap map = CCTMXTiledMap.Create("TileMaps/iso-test-vertexz");
            AddChild(map, 0, kTagTileMap);

            CCSize s = map.ContentSize;
            map.Position = new CCPoint(-s.Width / 2, 0);

            // because I'm lazy, I'm reusing a tile as an sprite, but since this method uses vertexZ, you
            // can use any CCSprite and it will work OK.
            CCTMXLayer layer = map.LayerNamed("Trees");
            m_tamara = layer.TileAt(new CCPoint(29, 29));

            CCMoveBy move = new CCMoveBy (10, new CCPoint(300, 250) * (1f / CCMacros.CCContentScaleFactor()));
            CCFiniteTimeAction back = move.Reverse();
            CCSequence seq = CCSequence.FromActions(move, back);
            m_tamara.RunAction(new CCRepeatForever (seq));

            m_tamara.Position = new CCPoint(m_tamara.Position.X + 100, m_tamara.Position.Y + 100);

            Schedule(repositionSprite);
        }

        private void repositionSprite(float dt)
        {
            // tile height is 64x32
            // map size: 30x30
            CCPoint p = m_tamara.Position;
            p = CCMacros.CCPointPointsToPixels(p);
            float newZ = -(p.Y + 32) / 16;
            m_tamara.VertexZ = newZ;
        }

        public override void OnEnter()
        {
            base.OnEnter();

            // TIP: 2d projection should be used
            CCDirector.SharedDirector.Projection = ccDirectorProjection.kCCDirectorProjection2D;
        }

        public override void OnExit()
        {
            // At exit use any other projection. 
            //    CCDirector.sharedDirector().setProjection:kCCDirectorProjection3D);
            base.OnExit();
        }

        public override string title()
        {
            return "TMX Iso VertexZ";
        }

        public override string subtitle()
        {
            return "Sprite should hide behind the trees";
        }
    }


//------------------------------------------------------------------
//
// TMXOrthoVertexZ
//
//------------------------------------------------------------------
    public class TMXOrthoVertexZ : TileDemo
    {
        private readonly CCSprite m_tamara;

        public TMXOrthoVertexZ()
        {
            CCTMXTiledMap map = CCTMXTiledMap.Create("TileMaps/orthogonal-test-vertexz");
            AddChild(map, 0, kTagTileMap);

            CCSize s = map.ContentSize;

            // because I'm lazy, I'm reusing a tile as an sprite, but since this method uses vertexZ, you
            // can use any CCSprite and it will work OK.
            CCTMXLayer layer = map.LayerNamed("trees");
            m_tamara = layer.TileAt(new CCPoint(0, 11));
            CCLog.Log("tamara vertexZ: {0}", m_tamara.VertexZ);

            CCMoveBy move = new CCMoveBy (10, new CCPoint(400, 450) * (1f / CCMacros.CCContentScaleFactor()));
            CCFiniteTimeAction back = move.Reverse();
            CCSequence seq = CCSequence.FromActions(move, back);
            m_tamara.RunAction(new CCRepeatForever (seq));

            Schedule(repositionSprite);
        }

        private void repositionSprite(float dt)
        {
            // tile height is 101x81
            // map size: 12x12
            CCPoint p = m_tamara.Position;
            p = CCMacros.CCPointPointsToPixels(p);
            m_tamara.VertexZ = -((p.Y + 81) / 81);
        }

        public override void OnEnter()
        {
            base.OnEnter();

            // TIP: 2d projection should be used
            CCDirector.SharedDirector.Projection = ccDirectorProjection.kCCDirectorProjection2D;
        }

        public override void OnExit()
        {
            // At exit use any other projection. 
            //    CCDirector.sharedDirector().setProjection:kCCDirectorProjection3D);
            base.OnExit();
        }

        public override string title()
        {
            return "TMX Ortho vertexZ";
        }

        public override string subtitle()
        {
            return "Sprite should hide behind the trees";
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
            CCTMXTiledMap map = CCTMXTiledMap.Create("TileMaps/iso-test-movelayer");
            AddChild(map, 0, kTagTileMap);

            map.Position = new CCPoint(-700, -50);
        }

        public override string title()
        {
            return "TMX Iso Move Layer";
        }

        public override string subtitle()
        {
            return "Trees should be horizontally aligned";
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
            CCTMXTiledMap map = CCTMXTiledMap.Create("TileMaps/orthogonal-test-movelayer");
            AddChild(map, 0, kTagTileMap);
        }

        public override string title()
        {
            return "TMX Ortho Move Layer";
        }

        public override string subtitle()
        {
            return "Trees should be horizontally aligned";
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
            CCTMXTiledMap map = CCTMXTiledMap.Create("TileMaps/ortho-tile-property");
            AddChild(map, 0, kTagTileMap);

            for (uint i = 1; i <= 20; i++)
            {
                CCLog.Log("GID:%i, Properties:%p", i, map.PropertiesForGID(i));
            }
        }

        public override string title()
        {
            return "TMX Tile Property Test";
        }

        public override string subtitle()
        {
            return "In the console you should see tile properties";
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
            CCTMXTiledMap map = CCTMXTiledMap.Create("TileMaps/ortho-rotation-test");
            AddChild(map, 0, kTagTileMap);

            /*
            object* pObj = NULL;
            CCARRAY_FOREACH(map.getChildren(), pObj)
            {
                CCSpriteBatchNode* child = (CCSpriteBatchNode*) pObj;
                child.Texture.setAntiAliasTexParameters();
            }
            */

            CCScaleBy action = new CCScaleBy(2, 0.5f);
            map.RunAction(action);
        }

        public override string title()
        {
            return "TMX tile flip test";
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
            CCTMXTiledMap map = CCTMXTiledMap.Create("TileMaps/ortho-rotation-test");
            AddChild(map, 0, kTagTileMap);

            /*
            object* pObj = NULL;
            CCARRAY_FOREACH(map.getChildren(), pObj)
            {
                CCSpriteBatchNode* child = (CCSpriteBatchNode*) pObj;
                child.Texture.setAntiAliasTexParameters();
            }
            */

            CCScaleBy action = new CCScaleBy(2, 0.5f);
            map.RunAction(action);

            Schedule(flipIt, 1.0f);
        }

        public override string title()
        {
            return "TMX tile flip run time test";
        }

        public override string subtitle()
        {
            return "in 2 sec bottom left tiles will flip";
        }

        private void flipIt(float dt)
        {
            var map = (CCTMXTiledMap) GetChildByTag(kTagTileMap);
            CCTMXLayer layer = map.LayerNamed("Layer 0");

            //blue diamond 
            var tileCoord = new CCPoint(1, 10);
            uint flags;
            uint GID = layer.TileGIDAt(tileCoord, out flags);
            // Vertical
            if ((flags & ccTMXTileFlags.kCCTMXTileVerticalFlag) != 0)
                flags &= ~ccTMXTileFlags.kCCTMXTileVerticalFlag;
            else
                flags |= ccTMXTileFlags.kCCTMXTileVerticalFlag;
            layer.SetTileGID(GID, tileCoord, flags);


            tileCoord = new CCPoint(1, 8);
            GID = layer.TileGIDAt(tileCoord, out flags);
            // Vertical
            if ((flags & ccTMXTileFlags.kCCTMXTileVerticalFlag) != 0)
                flags &= ~ccTMXTileFlags.kCCTMXTileVerticalFlag;
            else
                flags |= ccTMXTileFlags.kCCTMXTileVerticalFlag;
            layer.SetTileGID(GID, tileCoord, flags);


            tileCoord = new CCPoint(2, 8);
            GID = layer.TileGIDAt(tileCoord, out flags);
            // Horizontal
            if ((flags & ccTMXTileFlags.kCCTMXTileHorizontalFlag) != 0)
                flags &= ~ccTMXTileFlags.kCCTMXTileHorizontalFlag;
            else
                flags |= ccTMXTileFlags.kCCTMXTileHorizontalFlag;
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
            CCTMXTiledMap map = CCTMXTiledMap.Create("TileMaps/orthogonal-test1");
            AddChild(map, 0, kTagTileMap);

            /*
            object* pObj = NULL;
            CCARRAY_FOREACH(map.getChildren(), pObj)
            {
                CCSpriteBatchNode* child = (CCSpriteBatchNode*) pObj;
                child.Texture.setAntiAliasTexParameters();
            }
            */

            CCScaleBy action = new CCScaleBy(2, 0.5f);
            map.RunAction(action);
        }

        public override string title()
        {
            return "TMX created from XML test";
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
            CCTMXTiledMap map = CCTMXTiledMap.Create("TileMaps/orthogonal-test6");
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

            map.AnchorPoint = (new CCPoint(0, 0));
            CCTMXLayer layer = map.LayerNamed("Tile Layer 1");
            layer.SetTileGID(3, new CCPoint(2, 2));
        }

        public override string title()
        {
            return "TMX Bug 987";
        }

        public override string subtitle()
        {
            return "You should see an square";
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
            CCTMXTiledMap map = CCTMXTiledMap.Create("TileMaps/iso-test-bug787");
            AddChild(map, 0, kTagTileMap);

            map.Scale = (0.25f);
        }

        public override string title()
        {
            return "TMX Bug 787";
        }

        public override string subtitle()
        {
            return "You should see a map";
        }
    }

//------------------------------------------------------------------
//
// TileDemo
//
//------------------------------------------------------------------
    public class TileDemo : CCLayer
    {
        protected const string s_TilesPng = "TileMaps/tiles";
        protected const string s_LevelMapTga = "TileMaps/levelmap";
        protected const string s_pPathSister1 = "Images/grossinis_sister1";
        public const int kTagTileMap = 1;

        protected CCLabelTTF m_label;
        protected CCLabelTTF m_subtitle;

        private string s_pPathB1 = "Images/b1";
        private string s_pPathB2 = "Images/b2";
        private string s_pPathF1 = "Images/f1";
        private string s_pPathF2 = "Images/f2";
        private string s_pPathR1 = "Images/r1";
        private string s_pPathR2 = "Images/r2";
        private CCGamePadButtonDelegate _GamePadButtonDelegate;
        private CCGamePadDPadDelegate _GamePadDPadDelegate;
        private CCGamePadStickUpdateDelegate _GamePadStickDelegate;
        private CCGamePadTriggerDelegate _GamePadTriggerDelegate;

        public TileDemo()
        {
            TouchEnabled = true;

            CCSize s = CCDirector.SharedDirector.WinSize;
            _GamePadDPadDelegate = new CCGamePadDPadDelegate(MyOnGamePadDPadUpdate);
            _GamePadButtonDelegate = new CCGamePadButtonDelegate(MyOnGamePadButtonUpdate);
            _GamePadStickDelegate = new CCGamePadStickUpdateDelegate(MyOnGameStickUpdate);
            _GamePadTriggerDelegate =  new CCGamePadTriggerDelegate(MyGamePadTriggerUpdate);

            m_label = CCLabelTTF.Create("", "arial", 28);
            AddChild(m_label, 1);
            m_label.Position = new CCPoint(s.Width / 2, s.Height - 50);

            string strSubtitle = subtitle();
            if (! string.IsNullOrEmpty(strSubtitle))
            {
                CCLabelTTF l = CCLabelTTF.Create(strSubtitle, "arial", 16);
                AddChild(l, 1);
                l.Position = new CCPoint(s.Width / 2, s.Height - 80);

                m_subtitle = l;
            }

            CCMenuItemImage item1 = CCMenuItemImage.Create(s_pPathB1, s_pPathB2, backCallback);
            CCMenuItemImage item2 = CCMenuItemImage.Create(s_pPathR1, s_pPathR2, restartCallback);
            CCMenuItemImage item3 = CCMenuItemImage.Create(s_pPathF1, s_pPathF2, nextCallback);

            CCMenu menu = CCMenu.Create(item1, item2, item3);

            menu.Position = CCPoint.Zero;
            item1.Position = new CCPoint(s.Width / 2 - item2.ContentSize.Width * 2, item2.ContentSize.Height / 2);
            item2.Position = new CCPoint(s.Width / 2, item2.ContentSize.Height / 2);
            item3.Position = new CCPoint(s.Width / 2 + item2.ContentSize.Width * 2, item2.ContentSize.Height / 2);

            AddChild(menu, 1);
        }

        private bool _aButtonWasPressed = false;
        private bool _yButtonWasPressed = false;
        private bool _xButtonWasPressed = false;

        private void MyOnGamePadButtonUpdate(CCGamePadButtonStatus backButton, CCGamePadButtonStatus startButton, CCGamePadButtonStatus systemButton, CCGamePadButtonStatus aButton, CCGamePadButtonStatus bButton, CCGamePadButtonStatus xButton, CCGamePadButtonStatus yButton, CCGamePadButtonStatus leftShoulder, CCGamePadButtonStatus rightShoulder, Microsoft.Xna.Framework.PlayerIndex player)
        {
            if (aButton == CCGamePadButtonStatus.Pressed)
            {
                _aButtonWasPressed = true;
            }
            else if (aButton == CCGamePadButtonStatus.Released && _aButtonWasPressed)
            {
                // Select the menu
                restartCallback(null);
            }

            if (yButton == CCGamePadButtonStatus.Pressed)
            {
                _yButtonWasPressed = true;
            }
            else if (yButton == CCGamePadButtonStatus.Released && _yButtonWasPressed)
            {
                CCNode node = GetChildByTag(kTagTileMap);
                node.RunAction(new CCRotateBy (1f,15f));
            }

            if (xButton == CCGamePadButtonStatus.Pressed)
            {
                _xButtonWasPressed = true;
            }
            else if (xButton == CCGamePadButtonStatus.Released && _xButtonWasPressed)
            {
                CCNode node = GetChildByTag(kTagTileMap);
                node.RunAction(new CCRotateBy (1f, -15f));
            }
        }

        private long _FirstTicks;
        private bool _bDownPress = false;
        private bool _bUpPress = false;

        private void MyOnGamePadDPadUpdate(CCGamePadButtonStatus leftButton, CCGamePadButtonStatus upButton, CCGamePadButtonStatus rightButton, CCGamePadButtonStatus downButton, Microsoft.Xna.Framework.PlayerIndex player)
        {
            // Down and Up only
            if (rightButton == CCGamePadButtonStatus.Pressed)
            {
                if (_FirstTicks == 0L)
                {
                    _FirstTicks = DateTime.Now.Ticks;
                    _bDownPress = true;
                }
            }
            else if (rightButton == CCGamePadButtonStatus.Released && _FirstTicks > 0L && _bDownPress)
            {
                _FirstTicks = 0L;
                nextCallback(null);
                _bDownPress = false;
            }
            if (leftButton == CCGamePadButtonStatus.Pressed)
            {
                if (_FirstTicks == 0L)
                {
                    _FirstTicks = DateTime.Now.Ticks;
                    _bUpPress = true;
                }
            }
            else if (leftButton == CCGamePadButtonStatus.Released && _FirstTicks > 0L && _bUpPress)
            {
                _FirstTicks = 0L;
                backCallback(null);
                _bUpPress = false;
            }
        }


        public virtual string title()
        {
            return "No title";
        }

        public virtual string subtitle()
        {
            return "drag the screen";
        }

        public override void OnEnter()
        {
            base.OnEnter();

            m_label.Label = (title());
            m_subtitle.Label = (subtitle());
            CCApplication.SharedApplication.GamePadButtonUpdate += _GamePadButtonDelegate;
            CCApplication.SharedApplication.GamePadDPadUpdate += _GamePadDPadDelegate;
            CCApplication.SharedApplication.GamePadStickUpdate += _GamePadStickDelegate;
            CCApplication.SharedApplication.GamePadTriggerUpdate += _GamePadTriggerDelegate;
        }

        public override void OnExit()
        {
            base.OnExit();
            CCDirector pDirector = CCDirector.SharedDirector;
            pDirector.TouchDispatcher.RemoveDelegate(this);
            CCApplication.SharedApplication.GamePadButtonUpdate -= _GamePadButtonDelegate;
            CCApplication.SharedApplication.GamePadDPadUpdate -= _GamePadDPadDelegate;
            CCApplication.SharedApplication.GamePadStickUpdate -= _GamePadStickDelegate;
            CCApplication.SharedApplication.GamePadTriggerUpdate -= _GamePadTriggerDelegate;
        }


        private void restartCallback(object pSender)
        {
            CCScene s = new TileMapTestScene();
            s.AddChild(TileMapTestScene.restartTileMapAction());
            CCDirector.SharedDirector.ReplaceScene(s);
        }

        private void nextCallback(object pSender)
        {
            CCScene s = new TileMapTestScene();
            s.AddChild(TileMapTestScene.nextTileMapAction());
            CCDirector.SharedDirector.ReplaceScene(s);
        }

        private void backCallback(object pSender)
        {
            CCScene s = new TileMapTestScene();
            s.AddChild(TileMapTestScene.backTileMapAction());
            CCDirector.SharedDirector.ReplaceScene(s);
        }

        public override void RegisterWithTouchDispatcher()
        {
            CCDirector pDirector = CCDirector.SharedDirector;
            pDirector.TouchDispatcher.AddTargetedDelegate(this, 0, true);
        }

        public override bool TouchBegan(CCTouch touch, CCEvent e)
        {
            return true;
        }

        public override void TouchEnded(CCTouch touch, CCEvent e)
        {
        }

        public override void TouchCancelled(CCTouch touch, CCEvent e)
        {
        }

        public override void TouchMoved(CCTouch touch, CCEvent e)
        {
            CCPoint diff = touch.Delta;
            CCNode node = GetChildByTag(kTagTileMap);
            CCPoint currentPos = node.Position;
            node.Position = currentPos + diff;
        }
        private void MyGamePadTriggerUpdate(float leftTriggerStrength, float rightTriggerStrength, PlayerIndex player)
        {
            CCNode node = GetChildByTag(kTagTileMap);
            node.Rotation += rightTriggerStrength * CCMacros.CCDegreesToRadians(15f) - leftTriggerStrength * CCMacros.CCDegreesToRadians(15f);
        }
        private void MyOnGameStickUpdate(CCGameStickStatus left, CCGameStickStatus right, PlayerIndex player)
        {
            CCNode node = GetChildByTag(kTagTileMap);
            if (left.Magnitude > 0f)
            {
                // use the left stick to move the map
                CCPoint diff = left.Direction.InvertY * left.Magnitude * 10f;
                CCPoint currentPos = node.Position;
                node.Position = currentPos + diff;
            }
            if (right.Magnitude > 0f)
            {
                float scale = (1f - right.Direction.Y * right.Magnitude);
                node.Scale += scale;
                if (node.Scale < 1f)
                {
                    node.Scale = 1f;
                }
            }
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
            CCDirector.SharedDirector.SetDepthTest(true);

            CCDirector.SharedDirector.ReplaceScene(this);
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
            CCTMXTiledMap map = CCTMXTiledMap.Create("TileMaps/test-object-layer");
            AddChild(map, -1, kTagTileMap);
        }

        public override void Draw()
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

        public override string title()
        {
            return "TMX GID objects";
        }

        public override string subtitle()
        {
            return "Tiles are created from an object group";
        }
    }
}