namespace cocos2d
{
    public class CCMoveBy : CCMoveTo
    {
        public new bool InitWithDuration(float duration, CCPoint position)
        {
            if (base.InitWithDuration(duration))
            {
                m_delta = position;
                return true;
            }

            return false;
        }

        public override CCObject CopyWithZone(CCZone zone)
        {
            CCZone tmpZone = zone;
            CCMoveBy ret;
            if (tmpZone != null && tmpZone.m_pCopyObject != null)
            {
                ret = tmpZone.m_pCopyObject as CCMoveBy;

                if (ret == null)
                {
                    return null;
                }
            }
            else
            {
                ret = new CCMoveBy();
                tmpZone = new CCZone(ret);
            }

            base.CopyWithZone(tmpZone);

            ret.InitWithDuration(m_fDuration, m_delta);

            return ret;
        }

        public override void StartWithTarget(CCNode target)
        {
            CCPoint dTmp = m_delta;
            base.StartWithTarget(target);
            m_delta = dTmp;
        }

        public override CCFiniteTimeAction Reverse()
        {
            return Create(m_fDuration, CCPointExtension.CreatePoint(-m_delta.x, -m_delta.y));
        }

        public new static CCMoveBy Create(float duration, CCPoint position)
        {
            var ret = new CCMoveBy();
            ret.InitWithDuration(duration, position);
            return ret;
        }
    }
}