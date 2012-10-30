using cocos2d;

namespace tests.Extensions
{
    public class AnimationsTestLayer : BaseLayer
    {
        private CCBAnimationManager mAnimationManager;

        public void setAnimationManager(CCBAnimationManager pAnimationManager)
        {
            mAnimationManager = pAnimationManager;
        }

        public void onCCControlButtonIdleClicked(CCObject pSender, CCControlEvent pCCControlEvent)
        {
            mAnimationManager.RunAnimations("Idle", 0.3f);
        }

        public void onCCControlButtonWaveClicked(CCObject pSender, CCControlEvent pCCControlEvent)
        {
            mAnimationManager.RunAnimations("Wave", 0.3f);
        }

        public void onCCControlButtonJumpClicked(CCObject pSender, CCControlEvent pCCControlEvent)
        {
            mAnimationManager.RunAnimations("Jump", 0.3f);
        }

        public void onCCControlButtonFunkyClicked(CCObject pSender, CCControlEvent pCCControlEvent)
        {
            mAnimationManager.RunAnimations("Funky", 0.3f);
        }
    }
}