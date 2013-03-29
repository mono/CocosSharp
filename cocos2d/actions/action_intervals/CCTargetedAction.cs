namespace cocos2d
{
    public class CCTargetedAction : CCActionInterval
    {
        protected CCFiniteTimeAction m_pAction;
        protected CCNode m_pForcedTarget;

        public CCNode ForcedTarget
        {
            get { return m_pForcedTarget; }
        }

        public static CCTargetedAction Create(CCNode target, CCFiniteTimeAction pAction)
        {
            var p = new CCTargetedAction();
            p.InitWithTarget(target, pAction);
            return p;
        }

        public bool InitWithTarget(CCNode target, CCFiniteTimeAction pAction)
        {
            if (base.InitWithDuration(pAction.Duration))
            {
                m_pForcedTarget = target;
                m_pAction = pAction;
                return true;
            }
            return false;
        }

        public override object Copy(ICopyable pZone)
        {
            CCTargetedAction pRet;
            if (pZone != null) //in case of being called at sub class
            {
                pRet = (CCTargetedAction) (pZone);
            }
            else
            {
                pRet = new CCTargetedAction();
                pZone =  (pRet);
            }
            base.Copy(pZone);
            // win32 : use the m_pOther's copy object.
            pRet.InitWithTarget(m_pTarget, (CCFiniteTimeAction) m_pAction.Copy());
            return pRet;
        }

        public override void StartWithTarget(CCNode target)
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
            return Create(m_pForcedTarget, m_pAction.Reverse());
        }
    }
}