using CocosSharp;

namespace tests
{
    public class StressTest2 : TestCocosNodeDemo
    {

        public StressTest2()
        {
			var sublayer = new CCLayer();

			var sp1 = new CCSprite(TestResource.s_pPathSister1);
            

			var move = new CCMoveBy (3, new CCPoint(350, 0));
			var move_ease_inout3 = new CCEaseInOut(move, 2.0f);
			var move_ease_inout_back3 = move_ease_inout3.Reverse();
			var seq3 = new CCSequence(move_ease_inout3, move_ease_inout_back3);
			sp1.RepeatForever(seq3);

            sublayer.AddChild(sp1, 1, CocosNodeTestStaticLibrary.kTagSprite2);

			var fire = new CCParticleFire(CCPoint.Zero) { Tag = CocosNodeTestStaticLibrary.kTagSprite3 };
            fire.Texture = (CCTextureCache.SharedTextureCache.AddImage("Images/fire"));

			fire.RepeatForever(seq3);
            sublayer.AddChild(fire, 2);

            Schedule(shouldNotLeak, 6.0f);

            AddChild(sublayer, 0, CocosNodeTestStaticLibrary.kTagSprite1);
        }

        protected override void AddedToScene()
        {
            base.AddedToScene();

            var s = Layer.VisibleBoundsWorldspace.Size;

			var sublayer = this[CocosNodeTestStaticLibrary.kTagSprite1];
			sublayer[CocosNodeTestStaticLibrary.kTagSprite2].Position = (new CCPoint(80, s.Height / 2));
			var fire = sublayer[CocosNodeTestStaticLibrary.kTagSprite3];
			fire.Position = new CCPoint(80, s.Height / 2 - 50);
		}

        private void shouldNotLeak(float dt)
        {
            Unschedule((shouldNotLeak));
            var sublayer = (CCLayer) this[CocosNodeTestStaticLibrary.kTagSprite1];
            sublayer.RemoveAllChildren(true);
        }

        public override string title()
        {
            return "stress test #2: no leaks";
        }
    }
}