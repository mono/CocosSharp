using Cocos2D;

namespace tests.Extensions
{
    public class CocosBuilderTest : TestScene
    {
        protected override void NextTestCase()
        {
        }
        protected override void PreviousTestCase()
        {
        }
        protected override void RestTestCase()
        {
        }
        public override void runThisTest()
        {
            /* Create an autorelease CCNodeLoaderLibrary. */
            CCNodeLoaderLibrary ccNodeLoaderLibrary = CCNodeLoaderLibrary.SharedInstance;

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