namespace CocosSharp
{
    public class CCFiniteTimeAction : CCAction
    {
        protected float m_fDuration;

        protected CCFiniteTimeAction()
        {
        }

        protected CCFiniteTimeAction(CCFiniteTimeAction finiteTimeAction) : base(finiteTimeAction)
        {
        }

        public float Duration
        {
            get { return m_fDuration; }
            set { m_fDuration = value; }
        }

        public virtual CCFiniteTimeAction Reverse()
        {
            CCLog.Log("cocos2d: FiniteTimeAction#reverse: Implement me");
            return null;
        }
    }
}