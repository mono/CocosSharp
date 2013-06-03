using System.Diagnostics;

namespace Cocos2D
{
	public class CCGridAction : CCActionInterval
	{
		protected CCGridSize m_sGridSize;
		
        public CCGridAction()
        {
        }
        
        public CCGridAction(float duration)
            : base(duration)
        {
        }
        
        public CCGridAction(CCGridSize gridSize, float duration) : base(duration)
        {
            InitWithSize(gridSize, duration);
        }
        
        protected virtual bool InitWithSize(CCGridSize gridSize, float duration)
        {
            if (base.InitWithDuration(duration))
            {
                m_sGridSize = gridSize;
                return true;
            }
            return false;
        }

        public CCGridAction (CCGridAction gridAction) : this (gridAction.m_sGridSize, gridAction.m_fDuration)
        { }

		public override object Copy(ICCCopyable pZone)
		{
		
			if (pZone != null)
			{
				//in case of being called at sub class
				var pCopy = (CCGridAction) (pZone);
                base.Copy(pZone);
                
                pCopy.InitWithSize(m_sGridSize, m_fDuration);
                
                return pCopy;
            }
			else
			{
				return new CCGridAction(this);
			}
			
		}
		
		public override void StartWithTarget(CCNode target)
		{
			base.StartWithTarget(target);
			
			CCNode t = m_pTarget;
			CCGridBase targetGrid = t.Grid;
			
			if (targetGrid != null && targetGrid.ReuseGrid > 0)
			{
				Grid = targetGrid;
				
				if (targetGrid.Active && targetGrid.GridSize.X == m_sGridSize.X
				    && targetGrid.GridSize.Y == m_sGridSize.Y /*&& dynamic_cast<CCGridBase*>(targetGrid) != NULL*/)
				{
					targetGrid.Reuse();
				}
				else
				{
					Debug.Assert(false);
				}
			}
			else
			{
				if (targetGrid != null && targetGrid.Active)
				{
					targetGrid.Active = false;
				}
				
				CCGridBase newgrid = Grid;
				
				t.Grid = newgrid;
				t.Grid.Active = true;
			}
		}
		
		public override CCFiniteTimeAction Reverse()
		{
			return new CCReverseTime(this);
		}
		
		public virtual CCGridBase Grid
		{
			set { }
			get { return null; }
		}

	}
}