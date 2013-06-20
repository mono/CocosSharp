namespace Cocos2D
{
    public class CCRepeat : CCActionInterval
    {
        protected bool m_bActionInstant;
        protected float m_fNextDt;
        protected CCFiniteTimeAction m_pInnerAction;
        protected uint m_uTimes;
        protected uint m_uTotal;


        public CCRepeat(CCFiniteTimeAction action, uint times)
        {
            InitWithAction(action, times);
        }

        protected CCRepeat(CCRepeat repeat) : base(repeat)
        {
            var param = repeat.m_pInnerAction.Copy() as CCFiniteTimeAction;
            InitWithAction(param, repeat.m_uTimes);
        }

        public CCFiniteTimeAction InnerAction
        {
            get { return m_pInnerAction; }
            set { m_pInnerAction = value; }
        }

        public bool InitWithAction(CCFiniteTimeAction action, uint times)
        {
            float d = action.Duration * times;

            if (base.InitWithDuration(d))
            {
                m_uTimes = times;
                m_pInnerAction = action;

                m_bActionInstant = action is CCActionInstant;
                //an instant action needs to be executed one time less in the update method since it uses startWithTarget to execute the action
                if (m_bActionInstant)
                {
                    m_uTimes -= 1;
                }
                m_uTotal = 0;

                return true;
            }

            return false;
        }

        public override object Copy(ICCCopyable zone)
        {
            if (zone != null)
            {
                var ret = zone as CCRepeat;
                if (ret == null)
                {
                    return null;
                }
                base.Copy(zone);

                var param = m_pInnerAction.Copy() as CCFiniteTimeAction;
                if (param == null)
                {
                    return null;
                }
                ret.InitWithAction(param, m_uTimes);

                return ret;
            }
            else
            {
                return new CCRepeat(this);
            }
        }

        protected internal override void StartWithTarget(CCNode target)
        {
            m_uTotal = 0;
            m_fNextDt = m_pInnerAction.Duration / m_fDuration;
            base.StartWithTarget(target);
            m_pInnerAction.StartWithTarget(target);
        }

        public override void Stop()
        {
            m_pInnerAction.Stop();
            base.Stop();
        }

        // issue #80. Instead of hooking step:, hook update: since it can be called by any 
        // container action like Repeat, Sequence, AccelDeccel, etc..
        public override void Update(float dt)
        {
            if (dt >= m_fNextDt)
            {
                while (dt > m_fNextDt && m_uTotal < m_uTimes)
                {
                    m_pInnerAction.Update(1.0f);
                    m_uTotal++;

                    m_pInnerAction.Stop();
                    m_pInnerAction.StartWithTarget(m_pTarget);
                    m_fNextDt += m_pInnerAction.Duration / m_fDuration;
                }

                // fix for issue #1288, incorrect end value of repeat
                if (dt >= 1.0f && m_uTotal < m_uTimes)
                {
                    m_uTotal++;
                }

                // don't set an instant action back or update it, it has no use because it has no duration
                if (!m_bActionInstant)
                {
                    if (m_uTotal == m_uTimes)
                    {
                        m_pInnerAction.Update(1f);
                        m_pInnerAction.Stop();
                    }
                    else
                    {
                        // issue #390 prevent jerk, use right update
                        m_pInnerAction.Update(dt - (m_fNextDt - m_pInnerAction.Duration / m_fDuration));
                    }
                }
            }
            else
            {
                m_pInnerAction.Update((dt * m_uTimes) % 1.0f);
            }
        }

        public override bool IsDone
        {
            get { return m_uTotal == m_uTimes; }
        }

        public override CCFiniteTimeAction Reverse()
        {
            return new CCRepeat(m_pInnerAction.Reverse(), m_uTimes);
        }
    }
}