using System.Diagnostics;

namespace CocosSharp
{
    public class CCReverseTime : CCActionInterval
    {
        protected CCFiniteTimeAction m_pOther;


        #region Constructors

        public CCReverseTime(CCFiniteTimeAction action) : base(action.Duration)
        {
            m_pOther = action;
        }

        protected CCReverseTime(CCReverseTime copy) : base(copy)
        {
            m_pOther = new CCFiniteTimeAction(copy.m_pOther);
        }

        #endregion Constructors


        public override object Copy(ICCCopyable zone)
        {
            return new CCReverseTime(this);
        }

        protected internal override void StartWithTarget(CCNode target)
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