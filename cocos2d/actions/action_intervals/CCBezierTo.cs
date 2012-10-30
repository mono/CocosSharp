
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
            m_sConfig.ControlPoint1 = CCPointExtension.ccpSub(m_sConfig.ControlPoint1, m_startPosition);
            m_sConfig.ControlPoint2 = CCPointExtension.ccpSub(m_sConfig.ControlPoint2, m_startPosition);
            m_sConfig.EndPosition = CCPointExtension.ccpSub(m_sConfig.EndPosition, m_startPosition);
        }

        public override CCObject CopyWithZone(CCZone zone)
        {
            CCZone tmpZone = zone;
            CCBezierTo ret;

            if (tmpZone != null && tmpZone.m_pCopyObject != null)
            {
                ret = tmpZone.m_pCopyObject as CCBezierTo;
                if (ret == null)
                {
                    return null;
                }
            }
            else
            {
                ret = new CCBezierTo();
                tmpZone = new CCZone(ret);
            }

            base.CopyWithZone(tmpZone);

            ret.InitWithDuration(m_fDuration, m_sConfig);

            return ret;
        }
    }
}