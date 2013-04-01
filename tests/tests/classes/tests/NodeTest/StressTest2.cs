using cocos2d;

namespace tests
{
    public class StressTest2 : TestCocosNodeDemo
    {
        public StressTest2()
        {
            CCSize s = CCDirector.SharedDirector.WinSize;

            CCLayer sublayer = CCLayer.Create();

            CCSprite sp1 = CCSprite.Create(TestResource.s_pPathSister1);
            sp1.Position = (new CCPoint(80, s.Height / 2));

            CCActionInterval move = new CCMoveBy (3, new CCPoint(350, 0));
            CCActionInterval move_ease_inout3 = CCEaseInOut.Create((CCActionInterval) (move.Copy()), 2.0f);
            var move_ease_inout_back3 = (CCActionInterval) move_ease_inout3.Reverse();
            CCFiniteTimeAction seq3 = CCSequence.Create(move_ease_inout3, move_ease_inout_back3);
            sp1.RunAction(CCRepeatForever.Create((CCActionInterval) seq3));
            sublayer.AddChild(sp1, 1);

            CCParticleFire fire = CCParticleFire.Create();
            fire.Texture = (CCTextureCache.SharedTextureCache.AddImage("Images/fire"));
            fire.Position = (new CCPoint(80, s.Height / 2 - 50));

            var copy_seq3 = (CCActionInterval) (seq3.Copy());

            fire.RunAction(CCRepeatForever.Create(copy_seq3));
            sublayer.AddChild(fire, 2);

            Schedule((shouldNotLeak), 6.0f);

            AddChild(sublayer, 0, CocosNodeTestStaticLibrary.kTagSprite1);
        }

        private void shouldNotLeak(float dt)
        {
            Unschedule((shouldNotLeak));
            var sublayer = (CCLayer) GetChildByTag(CocosNodeTestStaticLibrary.kTagSprite1);
            sublayer.RemoveAllChildrenWithCleanup(true);
        }

        public override string title()
        {
            return "stress test #2: no leaks";
        }
    }
}