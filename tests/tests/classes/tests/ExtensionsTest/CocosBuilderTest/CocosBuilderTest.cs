using cocos2d;

namespace tests.Extensions
{
    public class CocosBuilderTest : TestScene
    {
        public override void runThisTest()
        {
            /* Create an autorelease CCNodeLoaderLibrary. */
            CCNodeLoaderLibrary ccNodeLoaderLibrary = CCNodeLoaderLibrary.NewDefaultCCNodeLoaderLibrary();

            ccNodeLoaderLibrary.RegisterCCNodeLoader("HelloCocosBuilderLayer", new Loader<HelloCocosBuilderLayer>());

            /* Create an autorelease CCBReader. */
            var ccbReader = new CCBReader(ccNodeLoaderLibrary);

            /* Read a ccbi file. */
            CCNode node = ccbReader.ReadNodeGraphFromFile("ccb/HelloCocosBuilder.ccbi", this);

            if (node != null)
            {
                AddChild(node);
            }

            CCDirector.SharedDirector.ReplaceScene(this);
        }
    }
}