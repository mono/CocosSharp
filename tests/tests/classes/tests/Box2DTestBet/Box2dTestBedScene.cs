using cocos2d;

namespace tests.classes.tests.Box2DTestBet
{
    public class Box2dTestBedScene : TestScene
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
            AddChild(MenuLayer.menuWithEntryID(0));

            CCDirector.SharedDirector.ReplaceScene(this);
        }
    }
}