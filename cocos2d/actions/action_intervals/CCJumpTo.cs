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
            m_delta = CCPointExtension.CreatePoint(m_delta.X - m_startPosition.X, m_delta.Y - m_startPosition.Y);
        }

        public override object Copy(ICopyable zone)
        {
            ICopyable tmpZone = zone;
            CCJumpTo ret;

            if (tmpZone != null && tmpZone != null)
            {
                ret = tmpZone as CCJumpTo;
                if (ret == null)
                {
                    return null;
                }
            }
            else
            {
                ret = new CCJumpTo();
                tmpZone =  (ret);
            }

            base.Copy(tmpZone);

            ret.InitWithDuration(m_fDuration, m_delta, m_height, m_nJumps);

            return ret;
        }
    }
}