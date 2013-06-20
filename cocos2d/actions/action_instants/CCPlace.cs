namespace Cocos2D
{
    public class CCPlace : CCActionInstant
    {
        private CCPoint m_tPosition;

        protected CCPlace()
        {
        }

        protected CCPlace(CCPlace place) : base(place)
        {
            InitWithPosition(m_tPosition);
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

        public override object Copy(ICCCopyable pZone)
        {
            if (pZone != null)
            {
                var pRet = (CCPlace) (pZone);
                base.Copy(pZone);
                pRet.InitWithPosition(m_tPosition);
                return pRet;
            }
            return new CCPlace(this);
        }

        protected internal override void StartWithTarget(CCNode target)
        {
            base.StartWithTarget(target);
            m_pTarget.Position = m_tPosition;
        }
    }
}