namespace CocosSharp
{
    public class CCShow : CCActionInstant
    {
        #region Constructors

        public CCShow ()
        {
        }

        #endregion Constructors

        protected internal override CCActionState StartAction(CCNode target)
        {
            return new CCShowState (this, target);

        }

        public override CCFiniteTimeAction Reverse ()
        {
            return (new CCHide ());
        }

    }

    internal class CCShowState : CCActionInstantState
    {

        public CCShowState (CCShow action, CCNode target)
            : base (action, target)
        {   
            target.Visible = true;
        }

    }

}