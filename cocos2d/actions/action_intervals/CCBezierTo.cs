
namespace Cocos2D
{
    public class CCBezierTo : CCBezierBy
    {
        public CCBezierTo (float t, CCBezierConfig c) : base (t, c)
        {
            InitWithDuration(t, c);
        }

        protected CCBezierTo (CCBezierTo bezierTo) : base (bezierTo)
        {
            InitWithDuration(bezierTo.m_fDuration, bezierTo.m_sConfig);
        }

        public override void StartWithTarget(CCNode target)
        {
            base.StartWithTarget(target);
            m_sConfig.ControlPoint1 = (m_sConfig.ControlPoint1 - m_startPosition);
            m_sConfig.ControlPoint2 = (m_sConfig.ControlPoint2 - m_startPosition);
            m_sConfig.EndPosition = (m_sConfig.EndPosition - m_startPosition);
        }

        public override object Copy(ICopyable zone)
        {

            if (zone != null && zone != null)
            {
                var ret = zone as CCBezierTo;
                if (ret == null)
                {
                    return null;
                }
                base.Copy(zone);
                
                ret.InitWithDuration(m_fDuration, m_sConfig);
                
                return ret;
            }
            else
            {
                return new CCBezierTo(this);
            }

        }
    }
}