namespace CocosSharp
{

//		public class CCTintBy : CCActionInterval
//		{
//			protected short m_deltaB;
//			protected short m_deltaG;
//			protected short m_deltaR;
//			protected short m_fromB;
//			protected short m_fromG;
//			protected short m_fromR;
//
//
//			#region Constructors
//
//			public CCTintBy(float duration, short deltaRed, short deltaGreen, short deltaBlue) : base(duration)
//			{
//				InitCCTintBy(deltaRed, deltaGreen, deltaBlue);
//			}
//
//			// Perform deep copy of CCTintBy
//			protected CCTintBy(CCTintBy tintBy) : base(tintBy)
//			{
//				InitCCTintBy(tintBy.m_deltaR, tintBy.m_deltaG, tintBy.m_deltaB);
//			}
//
//			private void InitCCTintBy(short deltaRed, short deltaGreen, short deltaBlue)
//			{
//				m_deltaR = deltaRed;
//				m_deltaG = deltaGreen;
//				m_deltaB = deltaBlue;
//			}
//
//			#endregion Constructors
//
//
//			public override object Copy(ICCCopyable zone)
//			{
//				return new CCTintBy(this);
//			}
//
//			protected internal override void StartWithTarget(CCNode target)
//			{
//				base.StartWithTarget(target);
//
//				var protocol = target as ICCColor;
//				if (protocol != null)
//				{
//					CCColor3B color = protocol.Color;
//					m_fromR = color.R;
//					m_fromG = color.G;
//					m_fromB = color.B;
//				}
//			}
//
//			public override void Update(float time)
//			{
//				var protocol = m_pTarget as ICCColor;
//				if (protocol != null)
//				{
//					protocol.Color = new CCColor3B((byte) (m_fromR + m_deltaR * time),
//						(byte) (m_fromG + m_deltaG * time),
//						(byte) (m_fromB + m_deltaB * time));
//				}
//			}
//
//			public override CCFiniteTimeAction Reverse()
//			{
//			return new CCTintBy(m_fDuration, (short) -m_deltaR, (short) -m_deltaG, (short) -m_deltaB);
//			}
//		}

    public class CCTintBy : CCActionInterval
    {
		public short DeltaB { get; private set; }
		public short DeltaG { get; private set; }
		public short DeltaR { get; private set; }


        #region Constructors

        public CCTintBy(float duration, short deltaRed, short deltaGreen, short deltaBlue) : base(duration)
        {
			DeltaR = deltaRed;
			DeltaG = deltaGreen;
			DeltaB = deltaBlue;
        }

        #endregion Constructors


		protected internal override CCActionState StartAction (CCNode target)
		{
			return new CCTintByState (this, target);
		}

        public override CCFiniteTimeAction Reverse()
        {
			return new CCTintBy(Duration, (short) -DeltaR, (short) -DeltaG, (short) -DeltaB);
        }
    }


	public class CCTintByState : CCActionIntervalState
	{
		protected short DeltaB { get; set; }
		protected short DeltaG { get; set; }
		protected short DeltaR { get; set; }

		protected short FromB { get; set; }
		protected short FromG { get; set; }
		protected short FromR { get; set; }

		public CCTintByState (CCTintBy action, CCNode target)
			: base(action, target)
		{	
			DeltaB = action.DeltaB;
			DeltaG = action.DeltaG;
			DeltaR = action.DeltaR;

			var protocol = target as ICCColor;
			if (protocol != null)
			{
				var color = protocol.Color;
				FromR = color.R;
				FromG = color.G;
				FromB = color.B;
			}
		}

		public override void Update(float time)
		{
			var protocol = Target as ICCColor;
			if (protocol != null)
			{
				protocol.Color = new CCColor3B((byte) (FromR + DeltaR * time),
					(byte) (FromG + DeltaG * time),
					(byte) (FromB + DeltaB * time));
			}
		}

	}

}