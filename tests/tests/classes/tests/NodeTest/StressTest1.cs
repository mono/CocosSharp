using CocosSharp;

namespace tests
{
    public class StressTest1 : TestCocosNodeDemo
    {
        public StressTest1()
        {
			var sp1 = new CCSprite(TestResource.s_pPathSister1);
            AddChild(sp1, 0, CocosNodeTestStaticLibrary.kTagSprite1);

            Schedule(shouldNotCrash, 1.0f);
        }

        protected override void AddedToNewScene()
        {
            base.AddedToNewScene();

            var s = Scene.VisibleBoundsWorldspace.Size;

			this[CocosNodeTestStaticLibrary.kTagSprite1].Position = s.Center;

		}


        private void shouldNotCrash(float dt)
        {
            Unschedule(shouldNotCrash);

            CCSize s = Scene.VisibleBoundsWorldspace.Size;

            // if the node has timers, it crashes
			CCParticleSun explosion = new CCParticleSun(s.Center);
            explosion.Texture = CCApplication.SharedApplication.TextureCache.AddImage("Images/fire");

            // if it doesn't, it works Ok.
            //	CocosNode *explosion = [Sprite create:@"grossinis_sister2.png");

			explosion.Position = s.Center;

			RunActions(new CCRotateBy (2, 360),
				new CCCallFuncN(removeMe));

            AddChild(explosion);
        }

        private void removeMe(CCNode node)
        {
            Parent.RemoveChild(node, true);
            NextCallback(this);
        }

        public override string title()
        {
            return "stress test #1: no crashes";
        }
    }
}