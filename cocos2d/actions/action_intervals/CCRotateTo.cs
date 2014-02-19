namespace CocosSharp
{
    public class CCRotateTo : CCActionInterval
    {
		public float DistanceAngleX { get; private set; }
		public float DistanceAngleY { get; private set; }


        #region Constructors

        public CCRotateTo(float duration, float fDeltaAngleX, float fDeltaAngleY) : base(duration)
        {
			DistanceAngleX = fDeltaAngleX;
			DistanceAngleY = fDeltaAngleY;

        }

        public CCRotateTo(float duration, float fDeltaAngle) : this(duration, fDeltaAngle, fDeltaAngle)
        {
        }

        #endregion Constructors

		protected internal override CCActionState StartAction (CCNode target)
		{
			return new CCRotateToState (this, target);

		}
    }


	public class CCRotateToState : CCActionIntervalState
	{
		protected float m_fDiffAngleY;
		protected float m_fDiffAngleX;
		protected float DistanceAngleX { get; set; }
		protected float DistanceAngleY { get; set; }
		protected float m_fStartAngleX;
		protected float m_fStartAngleY;

		public CCRotateToState (CCRotateTo action, CCNode target)
			: base(action, target)
		{ 
			DistanceAngleX = action.DistanceAngleX;
			DistanceAngleY = action.DistanceAngleY;

			// Calculate X
			m_fStartAngleX = Target.RotationX;
			if (m_fStartAngleX > 0)
			{
				m_fStartAngleX = m_fStartAngleX % 360.0f;
			}
			else
			{
				m_fStartAngleX = m_fStartAngleX % -360.0f;
			}

			m_fDiffAngleX = DistanceAngleX - m_fStartAngleX;
			if (m_fDiffAngleX > 180)
			{
				m_fDiffAngleX -= 360;
			}
			if (m_fDiffAngleX < -180)
			{
				m_fDiffAngleX += 360;
			}

			//Calculate Y: It's duplicated from calculating X since the rotation wrap should be the same
			m_fStartAngleY = Target.RotationY;

			if (m_fStartAngleY > 0)
			{
				m_fStartAngleY = m_fStartAngleY % 360.0f;
			}
			else
			{
				m_fStartAngleY = m_fStartAngleY % -360.0f;
			}

			m_fDiffAngleY = DistanceAngleY - m_fStartAngleY;
			if (m_fDiffAngleY > 180)
			{
				m_fDiffAngleY -= 360;
			}

			if (m_fDiffAngleY < -180)
			{
				m_fDiffAngleY += 360;
			}
		}

		public override void Update(float time)
		{
			if (Target != null)
			{
				Target.RotationX = m_fStartAngleX + m_fDiffAngleX * time;
				Target.RotationY = m_fStartAngleY + m_fDiffAngleY * time;
			}
		}

	}
}