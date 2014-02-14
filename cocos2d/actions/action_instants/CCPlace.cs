namespace CocosSharp
{
    public class CCPlace : CCActionInstant
    {
		public CCPoint Position { get; private set; }


        #region Constructors

        public CCPlace(CCPoint pos)
        {
            Position = pos;
        }

		public CCPlace(int posX, int posY)
		{
			Position = new CCPoint (posX, posY);
		}

        #endregion Constructors

		protected internal override CCActionState StartAction (CCNode target)
		{
			return new CCPlaceState (this, target);

		}

		// Take me out later - See comments in CCAction
		public override bool HasState 
		{ 
			get { return true; }
		}

//        protected internal override void StartWithTarget(CCNode target)
//        {
//            base.StartWithTarget(target);
//            m_pTarget.Position = Position;
//        }
    }

	public class CCPlaceState : CCFiniteTimeActionState
	{
		protected CCPoint Position { get; set; }

		public CCPlaceState (CCPlace action, CCNode target)
			: base(action, target)
		{ 
			Position = action.Position;
			Target.Position = Position;
		}

		// This can be taken out once CCActionInstant has it's State separated
		public override void Step(float dt)
		{
			Update(1);
		}
	}

}