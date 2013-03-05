namespace cocos2d
{
    public class CCJumpTo : CCJumpBy
    {
        public new static CCJumpTo Create(float duration, CCPoint position, float height, uint jumps)
        {
            var ret = new CCJumpTo();
            ret.InitWithDuration(duration, position, height, jumps);
            return ret;
        }

        public override void StartWithTarget(CCNode target)
        {
            base.StartWithTarget(target);
            m_delta = CCPointExtension.CreatePoint(m_delta.x - m_startPosition.x, m_delta.y - m_startPosition.y);
        }

        public override CCObject CopyWithZone(CCZone zone)
        {
            CCZone tmpZone = zone;
            CCJumpTo ret;

            if (tmpZone != null && tmpZone.m_pCopyObject != null)
            {
                ret = tmpZone.m_pCopyObject as CCJumpTo;
                if (ret == null)
                {
                    return null;
                }
            }
            else
            {
                ret = new CCJumpTo();
                tmpZone = new CCZone(ret);
            }

            base.CopyWithZone(tmpZone);

            ret.InitWithDuration(m_fDuration, m_delta, m_height, m_nJumps);

            return ret;
        }
    }
}