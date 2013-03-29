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

        public void onCCControlButtonIdleClicked(object pSender, CCControlEvent pCCControlEvent)
        {
            mAnimationManager.RunAnimations("Idle", 0.3f);
        }

        public void onCCControlButtonWaveClicked(object pSender, CCControlEvent pCCControlEvent)
        {
            mAnimationManager.RunAnimations("Wave", 0.3f);
        }

        public void onCCControlButtonJumpClicked(object pSender, CCControlEvent pCCControlEvent)
        {
            mAnimationManager.RunAnimations("Jump", 0.3f);
        }

        public void onCCControlButtonFunkyClicked(object pSender, CCControlEvent pCCControlEvent)
        {
            mAnimationManager.RunAnimations("Funky", 0.3f);
        }
    }
}