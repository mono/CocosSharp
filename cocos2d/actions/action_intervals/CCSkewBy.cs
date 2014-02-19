namespace CocosSharp
{
    public class CCSkewBy : CCSkewTo
    {
        #region Constructors

        public CCSkewBy(float t, float deltaSkewX, float deltaSkewY) : base(t, deltaSkewX, deltaSkewY)
        {
			m_fSkewX = deltaSkewX;
			m_fSkewY = deltaSkewY;
        }

		public CCSkewBy(float t, float deltaSkewXY) : this(t, deltaSkewXY, deltaSkewXY)
		{ }

        #endregion Constructors

		public float SkewByX
		{
			get { return m_fSkewX; }
		}

		public float SkewByY
		{
			get { return m_fSkewY; }
		}

		protected internal override CCActionState StartAction (CCNode target)
		{
			return new CCSkewByState (this, target);

		}

        public override CCFiniteTimeAction Reverse()
        {
            return new CCSkewBy(m_fDuration, -m_fSkewX, -m_fSkewY);
        }
    }

	public class CCSkewByState : CCSkewToState
	{

		public CCSkewByState (CCSkewBy action, CCNode target)
			: base(action, target)
		{ 

			m_fDeltaX = m_fSkewX = action.SkewByX;
			m_fDeltaY = m_fSkewY = action.SkewByY;
            m_fEndSkewX = m_fStartSkewX + m_fDeltaX;
            m_fEndSkewY = m_fStartSkewY + m_fDeltaY;
		}
	}
}