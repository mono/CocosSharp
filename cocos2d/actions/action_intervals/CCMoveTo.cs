namespace cocos2d
{
    public class CCMoveTo : CCActionInterval
    {
        protected CCPoint m_delta;
        protected CCPoint m_endPosition;
        protected CCPoint m_startPosition;

        public bool InitWithDuration(float duration, CCPoint position)
        {
            if (base.InitWithDuration(duration))
            {
                m_endPosition = position;
                return true;
            }

            return false;
        }

        public override object Copy(ICopyable zone)
        {
            ICopyable tmpZone = zone;
            CCMoveTo ret;

            if (tmpZone != null && tmpZone != null)
            {
                ret = (CCMoveTo) tmpZone;
            }
            else
            {
                ret = new CCMoveTo();
                tmpZone =  (ret);
            }

            base.Copy(tmpZone);
            ret.InitWithDuration(m_fDuration, m_endPosition);

            return ret;
        }

        public override void StartWithTarget(CCNode target)
        {
            base.StartWithTarget(target);
            m_startPosition = target.Position;
            m_delta = CCPointExtension.Subtract(m_endPosition, m_startPosition);
        }

        public override void Update(float time)
        {
            if (m_pTarget != null)
            {
                m_pTarget.Position = CCPointExtension.CreatePoint(m_startPosition.X + m_delta.X * time,
                                                          m_startPosition.Y + m_delta.Y * time);
            }
        }

        public static CCMoveTo Create(float duration, CCPoint position)
        {
            var moveTo = new CCMoveTo();
            moveTo.InitWithDuration(duration, position);

            return moveTo;
        }
    }
}