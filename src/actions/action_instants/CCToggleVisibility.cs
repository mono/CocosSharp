namespace CocosSharp
{
    public class CCToggleVisibility : CCActionInstant
    {
        #region Constructors

        public CCToggleVisibility ()
        {
        }

        #endregion Constructors


        protected internal override CCActionState StartAction(CCNode target)
        {
            return new CCToggleVisibilityState (this, target);

        }
    }

    internal class CCToggleVisibilityState : CCActionInstantState
    {

        public CCToggleVisibilityState (CCToggleVisibility action, CCNode target)
            : base (action, target)
        {   
            target.Visible = !target.Visible;
        }

    }

}