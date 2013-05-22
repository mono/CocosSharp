namespace Cocos2D
{
    public class CCReuseGrid : CCActionInstant
    {
        protected int m_nTimes;

        protected virtual bool InitWithTimes(int times)
        {
            m_nTimes = times;
            return true;
        }

        public override void StartWithTarget(CCNode target)
        {
            base.StartWithTarget(target);

            if (m_pTarget.Grid != null && m_pTarget.Grid.Active)
            {
                m_pTarget.Grid.ReuseGrid += m_nTimes;
            }
        }

        public CCReuseGrid()
        {
        }
        public CCReuseGrid(int times)
        {
            InitWithTimes(times);
        }
    }
}