using System.Diagnostics;

namespace cocos2d
{
    public interface CCActionTweenDelegate
    {
        void UpdateTweenAction(float value, string key);
    }

    public class CCActionTween : CCActionInterval
    {
        protected float m_fDelta;
        protected float m_fFrom, m_fTo;
        protected string m_strKey;

        public static CCActionTween Create(float aDuration, string key, float from, float to)
        {
            var pRet = new CCActionTween();
            pRet.InitWithDuration(aDuration, key, from, to);
            return pRet;
        }

        public bool InitWithDuration(float aDuration, string key, float from, float to)
        {
            if (base.InitWithDuration(aDuration))
            {
                m_strKey = key;
                m_fTo = to;
                m_fFrom = from;
                return true;
            }

            return false;
        }

        public override void StartWithTarget(CCNode target)
        {
            Debug.Assert(target is CCActionTweenDelegate, "target must implement CCActionTweenDelegate");
            base.StartWithTarget(target);
            m_fDelta = m_fTo - m_fFrom;
        }

        public override void Update(float dt)
        {
            ((CCActionTweenDelegate) m_pTarget).UpdateTweenAction(m_fTo - m_fDelta * (1 - dt), m_strKey);
        }

        public override CCFiniteTimeAction Reverse()
        {
            return Create(m_fDuration, m_strKey, m_fTo, m_fFrom);
        }
    }
}