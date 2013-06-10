namespace Cocos2D
{
    public class CCStopGrid : CCActionInstant
    {
        public CCStopGrid()
        {
        }

        public override void StartWithTarget(CCNode target)
        {
            base.StartWithTarget(target);

            CCGridBase pGrid = m_pTarget.Grid;
            if (pGrid != null && pGrid.Active)
            {
                pGrid.Active = false;
            }
        }
    }
}