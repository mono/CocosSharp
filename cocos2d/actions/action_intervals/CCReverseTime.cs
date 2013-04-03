using System.Diagnostics;

namespace cocos2d
{
    public class CCReverseTime : CCActionInterval
    {
        protected CCFiniteTimeAction m_pOther;

        public CCReverseTime (CCFiniteTimeAction action)
        {
            InitWithAction(action);
        }

        protected CCReverseTime (CCReverseTime reverseTime) : base (reverseTime)
        {
            InitWithAction(reverseTime.m_pOther.Copy() as CCFiniteTimeAction);
        }

        protected bool InitWithAction(CCFiniteTimeAction action)
        {
            Debug.Assert(action != null);
            Debug.Assert(action != m_pOther);

            if (base.InitWithDuration(action.Duration))
            {
                m_pOther = action;

                return true;
            }

            return false;
        }

        public override object Copy(ICopyable zone)
        {

            if (zone != null)
            {
                var ret = zone as CCReverseTime;
                if (ret == null)
                {
                    return null;
                }
                base.Copy(zone);
                
                ret.InitWithAction(m_pOther.Copy() as CCFiniteTimeAction);
                
                return ret;
            }
            else
            {
                return new CCReverseTime(this);
            }

        }

        public override void StartWithTarget(CCNode target)
        {
            base.StartWithTarget(target);
            m_pOther.StartWithTarget(target);
        }

        public override void Stop()
        {
            m_pOther.Stop();
            base.Stop();
        }

        public override void Update(float time)
        {
            if (m_pOther != null)
            {
                m_pOther.Update(1 - time);
            }
        }

        public override CCFiniteTimeAction Reverse()
        {
            return m_pOther.Copy() as CCFiniteTimeAction;
        }

    }
}