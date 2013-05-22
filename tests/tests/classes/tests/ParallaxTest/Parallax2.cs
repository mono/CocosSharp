using Cocos2D;

namespace tests
{
    internal class Parallax2 : ParallaxDemo
    {
        private string s_LevelMapTga = "TileMaps/levelmap";
        private string s_Power = "Images/powered";
        private string s_TilesPng = "TileMaps/tiles";

        public Parallax2()
        {
            TouchEnabled = true;

            // Top Layer, a simple image
            CCSprite cocosImage = new CCSprite(s_Power);
            // scale the image (optional)
            cocosImage.Scale = 2.5f;
            // change the transform anchor point to 0,0 (optional)
            cocosImage.AnchorPoint = new CCPoint(0, 0);


            // Middle layer: a Tile map atlas
            CCTileMapAtlas tilemap = CCTileMapAtlas.Create(s_TilesPng, s_LevelMapTga, 16, 16);
            tilemap.ReleaseMap();

            // change the transform anchor to 0,0 (optional)
            tilemap.AnchorPoint = new CCPoint(0, 0);

            // Anti Aliased images
            tilemap.Texture.SetAntiAliasTexParameters();

            // background layer: another image
            CCSprite background = new CCSprite(TestResource.s_back);
            // scale the image (optional)
            background.Scale = 1.5f;
            // change the transform anchor point (optional)
            background.AnchorPoint = new CCPoint(0, 0);


            // create a void node, a parent node
            CCParallaxNode voidNode = new CCParallaxNode();

            // NOW add the 3 layers to the 'void' node

            // background image is moved at a ratio of 0.4x, 0.5y
            voidNode.AddChild(background, -1, new CCPoint(0.4f, 0.5f), new CCPoint(0, 0));

            // tiles are moved at a ratio of 1.0, 1.0y
            voidNode.AddChild(tilemap, 1, new CCPoint(1.0f, 1.0f), new CCPoint(0, -200));

            // top image is moved at a ratio of 3.0x, 2.5y
            voidNode.AddChild(cocosImage, 2, new CCPoint(3.0f, 2.5f), new CCPoint(200, 1000));

            AddChild(voidNode, -1, kTagTileMap); // 0, (int)KTag.kTagNode);
        }

        public override void RegisterWithTouchDispatcher()
        {
            CCDirector.SharedDirector.TouchDispatcher.AddTargetedDelegate(this, 0, true);
        }

        public override bool TouchBegan(CCTouch touch, CCEvent event_)
        {
            return true;
        }

        public override void TouchMoved(CCTouch touch, CCEvent event_)
        {
            CCPoint touchLocation = touch.LocationInView;
            CCPoint prevLocation = touch.PreviousLocationInView;

            touchLocation = CCDirector.SharedDirector.ConvertToGl(touchLocation);
            prevLocation = CCDirector.SharedDirector.ConvertToGl(prevLocation);

            CCPoint diff = touchLocation - prevLocation;

            CCNode node = GetChildByTag(kTagTileMap); // (int) KTag.kTagNode);
            CCPoint currentPos = node.Position;
            node.Position = currentPos + diff;
        }

        public override string title()
        {
            return "Parallax: drag screen";
        }
    }
}