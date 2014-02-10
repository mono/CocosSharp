namespace CocosSharp
{
    public class CCEaseElastic : CCActionEase
    {
        public float Period { get; private set; }


        #region Constructors

        public CCEaseElastic(CCActionInterval pAction, float fPeriod) : base(pAction)
        {
            Period = fPeriod;
        }

        public CCEaseElastic(CCActionInterval pAction) : this(pAction, 0.3f)
        {
        }

        #endregion Constructors


        protected internal override CCActionState StartAction(CCNode target)
        {
            return new CCEaseElasticState(this, target);
        }

        public override CCFiniteTimeAction Reverse()
        {
            //assert(0);
            return null;
        }
    }


    #region Action state

    public class CCEaseElasticState : CCActionEaseState
    {
        protected CCEaseElastic EaseElasticAction 
        { 
            get { return Action as CCEaseElastic; } 
        }

        public CCEaseElasticState(CCEaseElastic action, CCNode target) : base(action, target)
        {
        }
    }

    #endregion Action state
}