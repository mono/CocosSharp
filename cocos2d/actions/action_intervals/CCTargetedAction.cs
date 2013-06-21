namespace Cocos2D
{
    public class CCTargetedAction : CCActionInterval
    {
        protected CCFiniteTimeAction m_pAction;
        protected CCNode m_pForcedTarget;

        public CCNode ForcedTarget
        {
            get { return m_pForcedTarget; }
        }

        public CCTargetedAction(CCNode target, CCFiniteTimeAction pAction)
        {
            InitWithTarget(target, pAction);
        }

        public CCTargetedAction(CCTargetedAction targetedAction) : base(targetedAction)
        {
            InitWithTarget(targetedAction.m_pForcedTarget, (CCFiniteTimeAction) targetedAction.m_pAction.Copy());
        }

        protected bool InitWithTarget(CCNode target, CCFiniteTimeAction pAction)
        {
            if (base.InitWithDuration(pAction.Duration))
            {
                m_pForcedTarget = target;
                m_pAction = pAction;
                return true;
            }
            return false;
        }

        public override object Copy(ICCCopyable pZone)
        {
            if (pZone != null) //in case of being called at sub class
            {
                var pRet = (CCTargetedAction) (pZone);
                base.Copy(pZone);
                // win32 : use the m_pOther's copy object.
                pRet.InitWithTarget(m_pForcedTarget, (CCFiniteTimeAction) m_pAction.Copy());
                return pRet;
            }
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