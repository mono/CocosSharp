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

        public override object Copy(ICopyable zone)
        {
            ICopyable tmpZone = zone;
            CCReverseTime ret;

            if (tmpZone != null && tmpZone != null)
            {
                ret = tmpZone as CCReverseTime;
                if (ret == null)
                {
                    return null;
                }
            }
            else
            {
                ret = new CCReverseTime();
                tmpZone =  (ret);
            }

            base.Copy(tmpZone);

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