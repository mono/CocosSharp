namespace cocos2d
{
    public class CCPlace : CCActionInstant
    {
        private CCPoint m_tPosition;

        public static CCPlace Create(CCPoint pos)
        {
            var pRet = new CCPlace();
            pRet.InitWithPosition(pos);
            return pRet;
        }

        public bool InitWithPosition(CCPoint pos)
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