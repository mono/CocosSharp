using System.Diagnostics;

namespace Cocos2D
{
    public class CCSpeed : CCAction
    {
        protected float m_fSpeed;
        protected CCActionInterval m_pInnerAction;

        public CCSpeed(CCActionInterval action, float fRate)
        {
            InitWithAction(action, fRate);
        }

        protected CCSpeed(CCSpeed speed) : base(speed)
        {
            InitWithAction((CCActionInterval) speed.m_pInnerAction.Copy(), speed.m_fSpeed);
        }

        protected bool InitWithAction(CCActionInterval action, float fRate)
        {
            Debug.Assert(action != null);

            m_pInnerAction = action;
            m_fSpeed = fRate;

            return true;
        }

        public override object Copy(ICCCopyable zone)
        {
            if (zone != null)
            {
                var ret = (CCSpeed) zone;
                base.Copy(zone);

                ret.InitWithAction((CCActionInterval) m_pInnerAction.Copy(), m_fSpeed);

                return ret;
            }
            else
            {
                return new CCSpeed(this);
            }
        }

        public float Speed
        {
            get { return m_fSpeed; }
            set { m_fSpeed = value; }
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

        public override bool IsDone
        {
            get { return m_pInnerAction.IsDone; }
        }

        public virtual CCActionInterval Reverse()
        {
            return (CCActionInterval) (CCAction) new CCSpeed((CCActionInterval) m_pInnerAction.Reverse(), m_fSpeed);
        }
    }
}