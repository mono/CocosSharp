using CocosSharp;
using Microsoft.Xna.Framework.Graphics;

namespace tests
{
    public class SubTest
    {
        protected CCSpriteBatchNode batchNode;
        protected CCNode parent;
        protected int subtestNumber;

        public void removeByTag(int tag)
        {
            switch (subtestNumber)
            {
                case 1:
                case 4:
                case 7:
                    parent.RemoveChildByTag(tag + 100, true);
                    break;
                case 2:
                case 3:
                case 5:
                case 6:
                case 8:
                case 9:
                    break;
                default:
                    break;
            }
        }

        public CCSprite createSpriteWithTag(int tag)
        {
            // create 

            CCSprite sprite = null;
            switch (subtestNumber)
            {
                case 1:
                    {
                        sprite = new CCSprite("Images/grossinis_sister1");
                        parent.AddChild(sprite, 0, tag + 100);
                        break;
                    }
                case 2:
                case 3:
                    {
                        sprite = new CCSprite(batchNode.Texture, new CCRect(0, 0, 52, 139));
                        batchNode.AddChild(sprite, 0, tag + 100);
                        break;
                    }
                case 4:
                    {
                        var idx = CCRandom.Next(1, 14);
                        string str = string.Format("Images/grossini_dance_{0:00}", idx);
                        sprite = new CCSprite(str);
                        parent.AddChild(sprite, 0, tag + 100);
                        break;
                    }
                case 5:
                case 6:
                    {
                        int y, x;
                        var r = (int) (CCRandom.Float_0_1() * 1400 / 100);

                        y = r / 5;
                        x = r % 5;

                        x *= 85;
                        y *= 121;
                        sprite = new CCSprite(batchNode.Texture, new CCRect(x, y, 85, 121));
                        batchNode.AddChild(sprite, 0, tag + 100);
                        break;
                    }

                case 7:
                    {
                        int y, x;
                        var r = (int) (CCRandom.Float_0_1() * 1400 / 100);

                        y = r / 8;
                        x = r % 8;

                        string str = string.Format("Images/sprites_test/sprite-{0}-{1}", x, y);
                        sprite = new CCSprite(str);
                        parent.AddChild(sprite, 0, tag + 100);
                        break;
                    }

                case 8:
                case 9:
                    {
                        int y, x;
                        var r = (int) (CCRandom.Float_0_1() * 6400 / 100);

                        y = r / 8;
                        x = r % 8;

                        x *= 32;
                        y *= 32;
                        sprite = new CCSprite(batchNode.Texture, new CCRect(x, y, 32, 32));
                        batchNode.AddChild(sprite, 0, tag + 100);
                        break;
                    }

                default:
                    break;
            }

            return sprite;
        }

        public void initWithSubTest(int nSubTest, CCNode p)
        {
            subtestNumber = nSubTest;
            parent = p;
            batchNode = null;
            /*
            * Tests:
            * 1: 1 (32-bit) PNG sprite of 52 x 139
            * 2: 1 (32-bit) PNG Batch Node using 1 sprite of 52 x 139
            * 3: 1 (16-bit) PNG Batch Node using 1 sprite of 52 x 139
            * 4: 1 (4-bit) PVRTC Batch Node using 1 sprite of 52 x 139

            * 5: 14 (32-bit) PNG sprites of 85 x 121 each
            * 6: 14 (32-bit) PNG Batch Node of 85 x 121 each
            * 7: 14 (16-bit) PNG Batch Node of 85 x 121 each
            * 8: 14 (4-bit) PVRTC Batch Node of 85 x 121 each

            * 9: 64 (32-bit) sprites of 32 x 32 each
            *10: 64 (32-bit) PNG Batch Node of 32 x 32 each
            *11: 64 (16-bit) PNG Batch Node of 32 x 32 each
            *12: 64 (4-bit) PVRTC Batch Node of 32 x 32 each
            */

            // purge textures
            CCTextureCache mgr = CCTextureCache.SharedTextureCache;
            mgr.RemoveTexture(mgr.AddImage("Images/grossinis_sister1"));
            mgr.RemoveTexture(mgr.AddImage("Images/grossini_dance_atlas"));
            mgr.RemoveTexture(mgr.AddImage("Images/spritesheet1"));

            switch (subtestNumber)
            {
                case 1:
                case 4:
                case 7:
                    break;

                case 2:
					CCTexture2D.DefaultAlphaPixelFormat = CCSurfaceFormat.Color;
                    batchNode = new CCSpriteBatchNode("Images/grossinis_sister1", 100);
                    p.AddChild(batchNode, 0);
                    break;
                
                case 3:
					CCTexture2D.DefaultAlphaPixelFormat = CCSurfaceFormat.Bgra4444;
                    batchNode = new CCSpriteBatchNode("Images/grossinis_sister1", 100);
                    p.AddChild(batchNode, 0);
                    break;

                case 5:
					CCTexture2D.DefaultAlphaPixelFormat = CCSurfaceFormat.Color;
                    batchNode = new CCSpriteBatchNode("Images/grossini_dance_atlas", 100);
                    p.AddChild(batchNode, 0);
                    break;

                case 6:
					CCTexture2D.DefaultAlphaPixelFormat = CCSurfaceFormat.Bgra4444;
                    batchNode = new CCSpriteBatchNode("Images/grossini_dance_atlas", 100);
                    p.AddChild(batchNode, 0);
                    break;

                    ///
                case 8:
					CCTexture2D.DefaultAlphaPixelFormat = CCSurfaceFormat.Color;
                    batchNode = new CCSpriteBatchNode("Images/spritesheet1", 100);
                    p.AddChild(batchNode, 0);
                    break;
                case 9:
					CCTexture2D.DefaultAlphaPixelFormat = CCSurfaceFormat.Bgra4444;
                    batchNode = new CCSpriteBatchNode("Images/spritesheet1", 100);
                    p.AddChild(batchNode, 0);
                    break;

                default:
                    break;
            }

            //if (batchNode != null)
            //{
            //    batchNode.retain();
            //}

			CCTexture2D.DefaultAlphaPixelFormat = CCSurfaceFormat.Color;
        }
    }
}