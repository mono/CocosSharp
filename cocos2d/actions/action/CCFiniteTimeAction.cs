namespace CocosSharp
{
    public class CCFiniteTimeAction : CCAction
    {
        protected float m_fDuration;

        public virtual float Duration
        {
            get { return m_fDuration; }
            set { m_fDuration = value; }
        }


        #region Constructors

        protected CCFiniteTimeAction()
        {
        }

        protected CCFiniteTimeAction(float d)
        {
            m_fDuration = d;
        }

        public CCFiniteTimeAction(CCFiniteTimeAction finiteTimeAction) : base(finiteTimeAction)
        {
            m_fDuration = finiteTimeAction.Duration;
        }

        #endregion Constructors
      

        public virtual CCFiniteTimeAction Reverse()
        {
            CCLog.Log("cocos2d: FiniteTimeAction#reverse: Implement me");
            return null;
        }
    }
}