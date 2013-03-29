
namespace cocos2d
{
    public class CCBezierTo : CCBezierBy
    {
        public new static CCBezierTo Create(float t, ccBezierConfig c)
        {
            var ret = new CCBezierTo();
            ret.InitWithDuration(t, c);

            return ret;
        }

        public override void StartWithTarget(CCNode target)
        {
            base.StartWithTarget(target);
            m_sConfig.ControlPoint1 = CCPointExtension.Subtract(m_sConfig.ControlPoint1, m_startPosition);
            m_sConfig.ControlPoint2 = CCPointExtension.Subtract(m_sConfig.ControlPoint2, m_startPosition);
            m_sConfig.EndPosition = CCPointExtension.Subtract(m_sConfig.EndPosition, m_startPosition);
        }

        public override object Copy(ICopyable zone)
        {
            ICopyable tmpZone = zone;
            CCBezierTo ret;

            if (tmpZone != null && tmpZone != null)
            {
                ret = tmpZone as CCBezierTo;
                if (ret == null)
                {
                    return null;
                }
            }
            else
            {
                ret = new CCBezierTo();
                tmpZone =  (ret);
            }

            base.Copy(tmpZone);

            ret.InitWithDuration(m_fDuration, m_sConfig);

            return ret;
        }
    }
}