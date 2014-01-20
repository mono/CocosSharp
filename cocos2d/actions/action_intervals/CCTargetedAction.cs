namespace CocosSharp
{
    public class CCTargetedAction : CCActionInterval
    {
        protected CCFiniteTimeAction m_pAction;
        protected CCNode m_pForcedTarget;

        public CCNode ForcedTarget
        {
            get { return m_pForcedTarget; }
        }


        #region Constructors

        public CCTargetedAction(CCNode target, CCFiniteTimeAction pAction) : base(pAction.Duration)
        {
            InitCCTargetedAction(target, pAction);
        }

        public CCTargetedAction(CCTargetedAction targetedAction) : base(targetedAction)
        {
            InitCCTargetedAction(targetedAction.m_pForcedTarget, new CCFiniteTimeAction(targetedAction.m_pAction));
        }

        private void InitCCTargetedAction(CCNode target, CCFiniteTimeAction pAction)
        {
            m_pForcedTarget = target;
            m_pAction = pAction;
        }

        #endregion Constructors


        public override object Copy(ICCCopyable pZone)
        {
            return new CCTargetedAction(this);
        }

        protected internal override void StartWithTarget(CCNode target)
        {
            base.StartWithTarget(target);
            m_pAction.StartWithTarget(m_pForcedTarget);
        }

        public override void Stop()
        {
            m_pAction.Stop();
        }

        public override void Update(float time)
        {
            m_pAction.Update(time);
        }

        public override CCFiniteTimeAction Reverse()
        {
            return new CCTargetedAction(m_pForcedTarget, m_pAction.Reverse());
        }
    }
}