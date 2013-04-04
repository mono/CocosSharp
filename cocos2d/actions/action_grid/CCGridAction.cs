            }
        }

        public override CCFiniteTimeAction Reverse()
        {

        public virtual CCGridBase Grid
        {
            set { }
            get { return null; }
        }

        public static CCGridAction Create(CCGridSize gridSize, float duration)
        {
            var pAction = new CCGridAction();
            pAction.InitWithSize(gridSize, duration);
            return pAction;
        }

        public virtual bool InitWithSize(CCGridSize gridSize, float duration)
        {
            if (base.InitWithDuration(duration))
            {
                m_sGridSize = gridSize;
                return true;
            }
            return false;
        }
    }
}
