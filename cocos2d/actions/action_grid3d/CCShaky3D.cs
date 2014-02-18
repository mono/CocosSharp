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
        protected bool CachedShake { get; private set; }
        protected int CachedRange { get; private set; }

        public CCShaky3DState(CCShaky3D action, CCNode target) : base(action, target)
        {
            CachedShake = action.Shake;
            CachedRange = action.Range;
        }

        public override void Update(float time)
        {
            int i, j;

            for (i = 0; i < (CachedGridSize.X + 1); ++i)
            {
                for (j = 0; j < (CachedGridSize.Y + 1); ++j)
                {
                    CCVertex3F v = OriginalVertex(i, j);
                    v.X += (CCRandom.Next() % (CachedRange * 2)) - CachedRange;
                    v.Y += (CCRandom.Next() % (CachedRange * 2)) - CachedRange;

                    if (CachedShake)
                    {
                        v.Z += (CCRandom.Next() % (CachedRange * 2)) - CachedRange;
                    }

                    SetVertex(i, j, ref v);
                }
            }
        }

    }

    #endregion Action state
}