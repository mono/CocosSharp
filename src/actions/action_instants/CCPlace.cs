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
    }

	public class CCPlaceState : CCActionInstantState
	{

		public CCPlaceState (CCPlace action, CCNode target)
			: base(action, target)
		{ 
			Target.Position = action.Position;
		}

	}

}