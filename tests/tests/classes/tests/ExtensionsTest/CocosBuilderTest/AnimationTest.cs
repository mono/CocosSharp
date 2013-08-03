using Cocos2D;

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
            mAnimationManager.RunAnimationsForSequenceNamedTweenDuration("Idle", 0.3f);
        }

        public void onCCControlButtonWaveClicked(object pSender, CCControlEvent pCCControlEvent)
        {
            mAnimationManager.RunAnimationsForSequenceNamedTweenDuration("Wave", 0.3f);
        }

        public void onCCControlButtonJumpClicked(object pSender, CCControlEvent pCCControlEvent)
        {
            mAnimationManager.RunAnimationsForSequenceNamedTweenDuration("Jump", 0.3f);
        }

        public void onCCControlButtonFunkyClicked(object pSender, CCControlEvent pCCControlEvent)
        {
            mAnimationManager.RunAnimationsForSequenceNamedTweenDuration("Funky", 0.3f);
        }
    }
}