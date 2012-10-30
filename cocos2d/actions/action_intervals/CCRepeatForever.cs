
using System.Diagnostics;

namespace cocos2d
{
    public class CCRepeatForever : CCActionInterval
    {
        protected CCActionInterval m_pInnerAction;

        public CCActionInterval InnerAction
        {
            get { return m_pInnerAction; }
            set { m_pInnerAction = value; }
        }

        public static CCRepeatForever Create(CCActionInterval action)
        {
            var ret = new CCRepeatForever();
            ret.InitWithAction(action);
            return ret;
        }

        public bool InitWithAction(CCActionInterval action)
        {
            Debug.Assert(action != null);
            m_pInnerAction = action;
            return true;
        }

        public override CCObject CopyWithZone(CCZone zone)
        {
            CCZone tmpZone = zone;
            CCRepeatForever ret;

            if (tmpZone != null && tmpZone.m_pCopyObject != null)
            {
                ret = tmpZone.m_pCopyObject as CCRepeatForever;
                if (ret == null)
                {
                    return null;
                }
            }
            else
            {
                ret = new CCRepeatForever();
                tmpZone = new CCZone(ret);
            }

            base.CopyWithZone(tmpZone);

            var param = m_pInnerAction.Copy() as CCActionInterval;
            if (param == null)
            {
                return null;
            }
            ret.InitWithAction(param);

            return ret;
        }

        public override void StartWithTarget(CCNode target)
        {
            base.StartWithTarget(target);
            m_pInnerAction.StartWithTarget(target);
        }

        public override void Step(float dt)
        {
            m_pInnerAction.Step(dt);
            
            if (m_pInnerAction.IsDone)
            {
                float diff = m_pInnerAction.Elapsed - m_pInnerAction.Duration;
                m_pInnerAction.StartWithTarget(m_pTarget);
                m_pInnerAction.Step(0f);
                m_pInnerAction.Step(diff);
            }
        }

        public override bool IsDone
        {
            get { return false; }
        }

        public override CCFiniteTimeAction Reverse()
        {
            return Create(m_pInnerAction.Reverse() as CCActionInterval);
        }

    }
}