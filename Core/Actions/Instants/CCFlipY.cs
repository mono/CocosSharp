namespace CocosSharp
{
    public class CCFlipY : CCActionInstant
    {
        public bool FlipY { get; private set; }


        #region Constructors

        public CCFlipY (bool y)
        {
            FlipY = y;
        }

        #endregion Constructors


        protected internal override CCActionState StartAction(CCNode target)
        {
            return new CCFlipYState (this, target);

        }

        public override CCFiniteTimeAction Reverse ()
        {
            return new CCFlipY (!FlipY);
        }

    }

    public class CCFlipYState : CCActionInstantState
    {

        public CCFlipYState (CCFlipY action, CCNode target)
            : base (action, target)
        {   

            if (!(target is CCSprite))
            {
                throw (new System.NotSupportedException ("FlipX and FlipY actions only work on CCSprite instances."));
            }
            ((CCSprite)(target)).FlipY = action.FlipY;      
        }

    }

}