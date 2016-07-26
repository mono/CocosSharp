namespace CocosSharp
{
    public class CCEaseElastic : CCActionEase
    {
        public float Period { get; private set; }


        #region Constructors

        public CCEaseElastic (CCFiniteTimeAction action, float period) : base (action)
        {
            Period = period;
        }

        public CCEaseElastic (CCFiniteTimeAction action) : this (action, 0.3f)
        {
        }

        #endregion Constructors


        protected internal override CCActionState StartAction(CCNode target)
        {
            return new CCEaseElasticState (this, target);
        }

        public override CCFiniteTimeAction Reverse ()
        {
            return null;
        }
    }


    #region Action state

    public class CCEaseElasticState : CCActionEaseState
    {
        protected float Period { get; private set; }

        public CCEaseElasticState (CCEaseElastic action, CCNode target) : base (action, target)
        {
            Period = action.Period;
        }
    }

    #endregion Action state
}