namespace CocosSharp
{
    public class CCActionEase : CCActionInterval
    {
        protected CCActionInterval m_pInner;


        #region Constructors

        // This can be taken out once all the classes that extend it have had their constructors created.
        protected CCActionEase()
        {
        }

        public CCActionInterval InnerAction
        {
            get { return m_pInner; }
        }

        public CCActionEase(CCActionInterval pAction) : base(pAction.Duration)
        {
            InitWithAction(pAction);
        }

        // Perform a deep copy of CCActionEase
        protected CCActionEase(CCActionEase actionEase) : base(actionEase)
        {
            InitWithAction(new CCActionInterval(actionEase.m_pInner));
        }

        private void InitWithAction(CCActionInterval pAction)
        {
            m_pInner = pAction;
        }

        #endregion Constructors


        // This should be changed to DeepCopy()
        public override object Copy(ICCCopyable pZone)
        {
            return new CCActionEase(this);
        }

        protected internal override void StartWithTarget(CCNode target)
        {
            base.StartWithTarget(target);
            m_pInner.StartWithTarget(m_pTarget);
        }

        public override void Stop()
        {
            m_pInner.Stop();
            base.Stop();
        }

        public override void Update(float time)
        {
            m_pInner.Update(time);
        }

        public override CCFiniteTimeAction Reverse()
        {
            return new CCActionEase((CCActionInterval) m_pInner.Reverse());
        }
    }
}