using cocos2d;

namespace tests
{
    public class StressTest1 : TestCocosNodeDemo
    {
        public StressTest1()
        {
            CCSize s = CCDirector.SharedDirector.WinSize;

            CCSprite sp1 = CCSprite.Create(TestResource.s_pPathSister1);
            AddChild(sp1, 0, CocosNodeTestStaticLibrary.kTagSprite1);

            sp1.Position = (new CCPoint(s.Width / 2, s.Height / 2));

            Schedule((shouldNotCrash), 1.0f);
        }

        private void shouldNotCrash(float dt)
        {
            Unschedule((shouldNotCrash));

            CCSize s = CCDirector.SharedDirector.WinSize;

            // if the node has timers, it crashes
            CCParticleSun explosion = CCParticleSun.Create();
            explosion.Texture = CCTextureCache.SharedTextureCache.AddImage("Images/fire");

            // if it doesn't, it works Ok.
            //	CocosNode *explosion = [Sprite create:@"grossinis_sister2.png");

            explosion.Position = new CCPoint(s.Width / 2, s.Height / 2);

            RunAction(CCSequence.FromActions(
                new CCRotateBy (2, 360),
                CCCallFuncN.Create((removeMe))
                          ));

            AddChild(explosion);
        }

        private void removeMe(CCNode node)
        {
            m_pParent.RemoveChild(node, true);
            nextCallback(this);
        }

        public override string title()
        {
            return "stress test #1: no crashes";
        }
    }
}