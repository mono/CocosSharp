using Cocos2D;
using tests;

namespace Box2D.TestBed
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