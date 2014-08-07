using CocosSharp;
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
            CCLayer layer = MenuLayer.menuWithEntryID (0);

            AddChild(layer);

            Scene.Director.ReplaceScene(this);
        }
    }
}