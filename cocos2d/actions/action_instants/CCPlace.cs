namespace cocos2d
{
    public class CCPlace : CCActionInstant
    {
        private CCPoint m_tPosition;

        protected CCPlace()
        {
        }
        public CCPlace(CCPoint pos)
        {
            InitWithPosition(pos);
        }

        protected virtual bool InitWithPosition(CCPoint pos)
        {
            m_tPosition = pos;
            return true;
        }

        public override object Copy(ICopyable pZone)
        {
            CCPlace pRet;

            if (pZone != null)
            {
                pRet = (CCPlace) (pZone);
            }
            else
            {
                pRet = new CCPlace();
                pZone =  (pRet);
            }

            base.Copy(pZone);
            pRet.InitWithPosition(m_tPosition);
            return pRet;
        }

        public override void StartWithTarget(CCNode target)
        {
            base.StartWithTarget(target);
            m_pTarget.Position = m_tPosition;
        }
    }
}