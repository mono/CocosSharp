namespace CocosSharp
{
    public class CCShaky3D : CCGrid3DAction
    {
		protected bool m_bShakeZ;
        protected int m_nRandrange;


        #region Constructors

		public CCShaky3D(float duration, CCGridSize gridSize, int range = 0, bool shakeZ = true) : base(duration, gridSize)
        {
            InitCCShaky3D(range, shakeZ);
        }

        // Perform deep copy of CCShaky3D
        public CCShaky3D(CCShaky3D shaky) : base(shaky)
        {
            InitCCShaky3D(shaky.m_nRandrange, shaky.m_bShakeZ);
        }

        private void InitCCShaky3D(int range, bool shakeZ)
        {
            m_nRandrange = range;
            m_bShakeZ = shakeZ;
        }

        #endregion Constructors

		protected bool Shake
		{
			get { return m_bShakeZ; }
			set { m_bShakeZ = value; }
		}

		protected int Range
		{
			get { return m_nRandrange; }
			set { m_nRandrange = value; }
		}

        public override object Copy(ICCCopyable pZone)
        {
            return new CCShaky3D(this);
        }

        public override void Update(float time)
        {
            int i, j;

            for (i = 0; i < (m_sGridSize.X + 1); ++i)
            {
                for (j = 0; j < (m_sGridSize.Y + 1); ++j)
                {
                    CCVertex3F v = OriginalVertex(i, j);
                    v.X += (CCRandom.Next() % (m_nRandrange * 2)) - m_nRandrange;
                    v.Y += (CCRandom.Next() % (m_nRandrange * 2)) - m_nRandrange;
                    if (m_bShakeZ)
                    {
                        v.Z += (CCRandom.Next() % (m_nRandrange * 2)) - m_nRandrange;
                    }

                    SetVertex(i, j, ref v);
                }
            }
        }
    }
}