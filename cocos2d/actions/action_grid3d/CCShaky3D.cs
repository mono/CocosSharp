namespace CocosSharp
{
    public class CCShaky3D : CCGrid3DAction
    {
        protected internal bool Shake { get; private set; }
        protected internal int Range { get; private set; }


        #region Constructors

		public CCShaky3D(float duration, CCGridSize gridSize, int range = 0, bool shakeZ = true) 
            : base(duration, gridSize)
        {
            Range = range;
            Shake = shakeZ;
        }

        #endregion Constructors


        protected internal override CCActionState StartAction(CCNode target)
        {
            return new CCShaky3DState(this, target);
        }
    }


    #region Action state

    public class CCShaky3DState : CCGrid3DActionState
    {
        protected CCShaky3D Shaky3DAction
        { 
            get { return Action as CCShaky3D; } 
        }

        public CCShaky3DState(CCShaky3D action, CCNode target) : base(action, target)
        {
        }

        public override void Update(float time)
        {
            int i, j;

            CCShaky3D shaky3DAction = Shaky3DAction;
            CCGridSize gridSize = shaky3DAction.GridSize;

            for (i = 0; i < (gridSize.X + 1); ++i)
            {
                for (j = 0; j < (gridSize.Y + 1); ++j)
                {
                    int range = shaky3DAction.Range;
                    CCVertex3F v = OriginalVertex(i, j);
                    v.X += (CCRandom.Next() % (range * 2)) - range;
                    v.Y += (CCRandom.Next() % (range * 2)) - range;

                    if (shaky3DAction.Shake)
                    {
                        v.Z += (CCRandom.Next() % (range * 2)) - range;
                    }

                    SetVertex(i, j, ref v);
                }
            }
        }

    }

    #endregion Action state
}