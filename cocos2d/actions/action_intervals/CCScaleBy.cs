namespace CocosSharp
{
    public class CCScaleBy : CCScaleTo
    {
        #region Constructors


        public CCScaleBy(float duration, float s) : base(duration, s)
        {
        }

        public CCScaleBy(float duration, float sx, float sy) : base(duration, sx, sy)
        {
        }

        #endregion Constructors

		protected internal override CCActionState StartAction (CCNode target)
		{
			return new CCScaleByState (this, target);

		}

        public override CCFiniteTimeAction Reverse()
        {
            return new CCScaleBy(m_fDuration, 1 / m_fEndScaleX, 1 / m_fEndScaleY);
        }

    }

	public class CCScaleByState : CCScaleToState
	{

		public CCScaleByState (CCScaleTo action, CCNode target)
			: base(action, target)
		{ 
			m_fDeltaX = m_fStartScaleX * m_fEndScaleX - m_fStartScaleX;
			m_fDeltaY = m_fStartScaleY * m_fEndScaleY - m_fStartScaleY;
		}

	}

}