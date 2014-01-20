namespace CocosSharp
{
    public class CCPlace : CCActionInstant
    {
        private CCPoint m_tPosition;


        #region Constructors

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

        private void InitWithPosition(CCPoint pos)
        {
            m_tPosition = pos;
        }

        #endregion Constructors


        public override object Copy(ICCCopyable pZone)
        {
            return new CCPlace(this);
        }

        protected internal override void StartWithTarget(CCNode target)
        {
            base.StartWithTarget(target);
            m_pTarget.Position = m_tPosition;
        }
    }
}