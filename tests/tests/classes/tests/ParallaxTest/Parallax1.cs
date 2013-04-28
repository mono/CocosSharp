using cocos2d;

namespace tests
{
    public class Parallax1 : ParallaxDemo
    {
        private string s_LevelMapTga = "TileMaps/levelmap";
        private string s_Power = "Images/powered";
        private string s_TilesPng = "TileMaps/tiles";

        public Parallax1()
        {
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
            CCParallaxNode voidNode = CCParallaxNode.Create();

            // NOW add the 3 layers to the 'void' node

            // background image is moved at a ratio of 0.4x, 0.5y
            voidNode.AddChild(background, -1, new CCPoint(0.4f, 0.5f), new CCPoint(0, 0));

            // tiles are moved at a ratio of 2.2x, 1.0y
            voidNode.AddChild(tilemap, 1, new CCPoint(2.2f, 1.0f), new CCPoint(0, -200));

            // top image is moved at a ratio of 3.0x, 2.5y
            voidNode.AddChild(cocosImage, 2, new CCPoint(3.0f, 2.5f), new CCPoint(200, 800));


            // now create some actions that will move the 'void' node
            // and the children of the 'void' node will move at different
            // speed, thus, simulation the 3D environment
            CCMoveBy goUp = new CCMoveBy (4, new CCPoint(0, -500));
            CCFiniteTimeAction goDown = goUp.Reverse();
            CCMoveBy go = new CCMoveBy (8, new CCPoint(-1000, 0));
            CCFiniteTimeAction goBack = go.Reverse();
            CCFiniteTimeAction seq = CCSequence.FromActions(goUp, go, goDown, goBack);

            voidNode.RunAction(new CCRepeatForever ((CCActionInterval) seq));

            AddChild(voidNode, -1, kTagTileMap);
        }

        public override string title()
        {
            return "Parallax: parent and 3 children";
        }
    }
}