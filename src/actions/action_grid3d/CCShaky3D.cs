namespace CocosSharp
{
    public class CCShaky3D : CCGrid3DAction
    {
        protected internal bool Shake { get; private set; }
        protected internal int Range { get; private set; }


        #region Constructors

        public CCShaky3D (float duration, CCGridSize gridSize, int range = 0, bool shakeZ = true)
            : base (duration, gridSize)
        {
            Range = range;
            Shake = shakeZ;
        }

        #endregion Constructors


        protected internal override CCActionState StartAction(CCNode target)
        {
            return new CCShaky3DState (this, GridNode(target));
        }
    }


    #region Action state

    public class CCShaky3DState : CCGrid3DActionState
    {
        public bool Shake { get; set; }

        public int Range { get; set; }

        public CCShaky3DState (CCShaky3D action, CCNodeGrid target) : base (action, target)
        {
            Shake = action.Shake;
            Range = action.Range;
        }

        public override void Update (float time)
        {
            int i, j;

            for (i = 0; i < (GridSize.X + 1); ++i)
            {
                for (j = 0; j < (GridSize.Y + 1); ++j)
                {
                    CCVertex3F v = OriginalVertex (i, j);
                    v.X += (CCRandom.Next () % (Range * 2)) - Range;
                    v.Y += (CCRandom.Next () % (Range * 2)) - Range;

                    if (Shake)
                    {
                        v.Z += (CCRandom.Next () % (Range * 2)) - Range;
                    }

                    SetVertex (i, j, ref v);
                }
            }
        }

    }

    #endregion Action state
}