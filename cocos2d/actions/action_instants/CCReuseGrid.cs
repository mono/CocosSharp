namespace cocos2d
{
    public class CCReuseGrid : CCActionInstant
    {
        protected int m_nTimes;

        public bool InitWithTimes(int times)
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

        public static CCReuseGrid Create(int times)
        {
            var pAction = new CCReuseGrid();
            pAction.InitWithTimes(times);
            return pAction;
        }
    }
}