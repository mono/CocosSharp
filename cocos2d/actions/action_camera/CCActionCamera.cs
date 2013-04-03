
namespace cocos2d
{
    public class CCActionCamera : CCActionInterval
    {
        protected float m_fCenterXOrig;
        protected float m_fCenterYOrig;
        protected float m_fCenterZOrig;

        protected float m_fEyeXOrig;
        protected float m_fEyeYOrig;
        protected float m_fEyeZOrig;

        protected float m_fUpXOrig;
        protected float m_fUpYOrig;
        protected float m_fUpZOrig;

        public CCActionCamera()
        {
            m_fCenterXOrig = 0;
            m_fCenterYOrig = 0;
            m_fCenterZOrig = 0;

            m_fEyeXOrig = 0;
            m_fEyeYOrig = 0;
            m_fEyeZOrig = 0;

            m_fUpXOrig = 0;
            m_fUpYOrig = 0;
            m_fUpZOrig = 0;
        }

        public override void StartWithTarget(CCNode target)
        {
            base.StartWithTarget(target);

            CCCamera camera = target.Camera;

            camera.GetCenterXyz(out m_fCenterXOrig, out m_fCenterYOrig, out m_fCenterZOrig);
            camera.GetEyeXyz(out m_fEyeXOrig, out m_fEyeYOrig, out m_fEyeZOrig);
            camera.GetUpXyz(out m_fUpXOrig, out m_fUpYOrig, out m_fUpZOrig);
        }

        public override CCFiniteTimeAction Reverse()
        {
            return new CCReverseTime(this);
        }
    }
}