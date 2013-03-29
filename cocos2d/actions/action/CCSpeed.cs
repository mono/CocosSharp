
using System.Diagnostics;

namespace cocos2d
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

        public static CCSpeed Create(CCActionInterval action, float fRate)
        {
            var ret = new CCSpeed();
            ret.InitWithAction(action, fRate);
            return ret;
        }

        public bool InitWithAction(CCActionInterval action, float fRate)
        {
            Debug.Assert(action != null);

            m_pInnerAction = action;
            m_fSpeed = fRate;

            return true;
        }

        public override object CopyWithZone(CCZone zone)
        {
            CCSpeed ret;

            if (zone != null && zone.m_pCopyObject != null)
            {
                ret = (CCSpeed) zone.m_pCopyObject;
            }
            else
            {
                ret = new CCSpeed();
            }

            base.CopyWithZone(zone);

            ret.InitWithAction((CCActionInterval) m_pInnerAction.Copy(), m_fSpeed);

            return ret;
        }

        public override void StartWithTarget(CCNode target)
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
            return (CCActionInterval) (CCAction) Create((CCActionInterval) m_pInnerAction.Reverse(), m_fSpeed);
        }
    }
}