using CocosSharp;

namespace tests
{
    public class StressTest1 : TestCocosNodeDemo
    {
        public StressTest1()
        {
			var s = CCApplication.SharedApplication.MainWindowDirector.WindowSizeInPoints;

			var sp1 = new CCSprite(TestResource.s_pPathSister1);
            AddChild(sp1, 0, CocosNodeTestStaticLibrary.kTagSprite1);

            sp1.Position = new CCPoint(s.Width / 2, s.Height / 2);

            Schedule(shouldNotCrash, 1.0f);
        }

        private void shouldNotCrash(float dt)
        {
            Unschedule(shouldNotCrash);

            CCSize s = CCApplication.SharedApplication.MainWindowDirector.WindowSizeInPoints;

            // if the node has timers, it crashes
			CCParticleSun explosion = new CCParticleSun(new CCPoint(s.Width / 2, s.Height / 2));
            explosion.Texture = CCApplication.SharedApplication.TextureCache.AddImage("Images/fire");

            // if it doesn't, it works Ok.
            //	CocosNode *explosion = [Sprite create:@"grossinis_sister2.png");

            explosion.Position = new CCPoint(s.Width / 2, s.Height / 2);

			RunActions(new CCRotateBy (2, 360),
				new CCCallFuncN(removeMe));

            AddChild(explosion);
        }

        private void removeMe(CCNode node)
        {
            Parent.RemoveChild(node, true);
            nextCallback(this);
        }

        public override string title()
        {
            return "stress test #1: no crashes";
        }
    }
}