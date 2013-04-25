namespace cocos2d
{
    public class CCStopGrid : CCActionInstant
    {
        public override void StartWithTarget(CCNode target)
        {
            base.StartWithTarget(target);

            CCGridBase pGrid = m_pTarget.Grid;
            if (pGrid != null && pGrid.Active)
            {
                pGrid.Active = false;
            }
        }

        public CCStopGrid()
        {
        }
    }
}