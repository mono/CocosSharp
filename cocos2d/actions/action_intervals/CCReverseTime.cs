using System.Diagnostics;

namespace cocos2d
{
    public class CCReverseTime : CCActionInterval
    {
        protected CCFiniteTimeAction m_pOther;

        {
            InitWithAction(action);
        }

        protected CCReverseTime (CCReverseTime reverseTime) : base (reverseTime)
        {
            InitWithAction(reverseTime.m_pOther.Copy() as CCFiniteTimeAction);
        }

        protected bool InitWithAction(CCFiniteTimeAction action)
=======
        public CCReverseTime(CCFiniteTimeAction action) : base(action.Duration)
>>>>>>> upstream/master
        {
            m_pOther = action;
        }

        protected CCReverseTime(CCReverseTime copy)
            : base(copy)
        {
            m_pOther = copy.m_pOther;
        }

        public override object Copy(ICopyable zone)
        {
<<<<<<< HEAD

            if (zone != null)
            {
                var ret = zone as CCReverseTime;
                if (ret == null)
                {
                    return null;
                }
                base.Copy(zone);
                
                ret.InitWithAction(m_pOther.Copy() as CCFiniteTimeAction);
                
=======
            if (zone != null)
            {
                var ret = zone as CCReverseTime;
                base.Copy(zone);
                m_pOther = (CCFiniteTimeAction)ret.m_pOther; // .Copy() was in here before
>>>>>>> upstream/master
                return ret;
            }
            else
            {
                return new CCReverseTime(this);
            }
<<<<<<< HEAD

=======
>>>>>>> upstream/master
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
