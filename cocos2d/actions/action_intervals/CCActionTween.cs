using System.Diagnostics;

namespace Cocos2D
{
    public interface ICCActionTweenDelegate
    {
        void UpdateTweenAction(float value, string key);
    }

    public class CCActionTween : CCActionInterval
    {
        protected float m_fDelta;
        protected float m_fFrom, m_fTo;
        protected string m_strKey;

        public CCActionTween(float aDuration, string key, float from, float to)
        {
            InitWithDuration(aDuration, key, from, to);
        }

        protected bool InitWithDuration(float aDuration, string key, float from, float to)
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
            Debug.Assert(target is ICCActionTweenDelegate, "target must implement CCActionTweenDelegate");
            base.StartWithTarget(target);
            m_fDelta = m_fTo - m_fFrom;
        }

        public override void Update(float dt)
        {
            ((ICCActionTweenDelegate) m_pTarget).UpdateTweenAction(m_fTo - m_fDelta * (1 - dt), m_strKey);
        }

        public override CCFiniteTimeAction Reverse()
        {
            return new CCActionTween(m_fDuration, m_strKey, m_fTo, m_fFrom);
        }
    }
}