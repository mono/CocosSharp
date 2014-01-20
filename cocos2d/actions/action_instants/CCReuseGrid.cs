namespace CocosSharp
{
    public class CCReuseGrid : CCActionInstant
    {
        protected int m_nTimes;


        #region Constructors

        public CCReuseGrid()
        {
        }

        public CCReuseGrid(int times)
        {
            m_nTimes = times;
        }

        #endregion Constructors


        protected internal override void StartWithTarget(CCNode target)
        {
            base.StartWithTarget(target);

            if (m_pTarget.Grid != null && m_pTarget.Grid.Active)
            {
                m_pTarget.Grid.ReuseGrid += m_nTimes;
            }
        }
    }
}