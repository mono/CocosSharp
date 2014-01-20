using System.Diagnostics;

namespace CocosSharp
{
    public class CCSpeed : CCAction
    {
        protected float m_fSpeed;
        protected CCActionInterval m_pInnerAction;

        public float Speed
        {
            get { return m_fSpeed; }
            set { m_fSpeed = value; }
        }

        public override bool IsDone
        {
            get { return m_pInnerAction.IsDone; }
        }


        #region Constructors

        public CCSpeed(CCActionInterval action, float fRate)
        {
            InitCCSpeed(action, fRate);
        }

        // Perform deep copy of CCSpeed
        protected CCSpeed(CCSpeed speed) : base(speed)
        {
            InitCCSpeed((CCActionInterval) speed.m_pInnerAction.Copy(), speed.m_fSpeed);
        }

        private void InitCCSpeed(CCActionInterval action, float fRate)
        {
            Debug.Assert(action != null);

            m_pInnerAction = action;
            m_fSpeed = fRate;
        }

        #endregion Constructors


        public override object Copy(ICCCopyable zone)
        {
            return new CCSpeed(this);
        }

        protected internal override void StartWithTarget(CCNode target)
        {
            base.StartWithTarget(target);
            m_pInnerAction.StartWithTarget(target);
        }

        public override void Stop()
        {
            m_pInnerAction.Stop();
            base.Stop();
        }

        public override void Step(float dt)
        {
            m_pInnerAction.Step(dt * m_fSpeed);
        }

        public virtual CCActionInterval Reverse()
        {
            return (CCActionInterval) (CCAction) new CCSpeed((CCActionInterval) m_pInnerAction.Reverse(), m_fSpeed);
        }
    }
}