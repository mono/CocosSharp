using System.Diagnostics;

namespace cocos2d
{
    public class CCReverseTime : CCActionInterval
    {
        protected CCFiniteTimeAction m_pOther;

        public bool InitWithAction(CCFiniteTimeAction action)
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

        public override CCObject CopyWithZone(CCZone zone)
        {
            CCZone tmpZone = zone;
            CCReverseTime ret;

            if (tmpZone != null && tmpZone.m_pCopyObject != null)
            {
                ret = tmpZone.m_pCopyObject as CCReverseTime;
                if (ret == null)
                {
                    return null;
                }
            }
            else
            {
                ret = new CCReverseTime();
                tmpZone = new CCZone(ret);
            }

            base.CopyWithZone(tmpZone);

            ret.InitWithAction(m_pOther.Copy() as CCFiniteTimeAction);

            return ret;
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

        public static CCReverseTime Create(CCFiniteTimeAction action)
        {
            var ret = new CCReverseTime();
            ret.InitWithAction(action);
            return ret;
        }
    }
}