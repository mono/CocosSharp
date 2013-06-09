using System;

namespace Cocos2D
{
    public partial class CCEaseCustom : CCActionEase
    {
        private Func<float, float> m_EaseFunc;

        public Func<float, float> EaseFunc
        {
            get { return m_EaseFunc; }
            set { m_EaseFunc = value; }
        }

        public CCEaseCustom(CCActionInterval pAction, Func<float, float> easeFunc)
        {
            InitWithAction(pAction, easeFunc);
        }

        protected CCEaseCustom(CCEaseCustom easeCustom)
            : base(easeCustom)
        {
            InitWithAction((CCActionInterval) easeCustom.InnerAction.Copy(), easeCustom.EaseFunc);
        }

        public void InitWithAction(CCActionInterval action, Func<float, float> easeFunc)
        {
            base.InitWithAction(action);
            m_EaseFunc = easeFunc;
        }

        public override void Update(float time)
        {
            m_pInner.Update(m_EaseFunc(time));
        }

        public override CCFiniteTimeAction Reverse()
        {
            return new CCReverseTime(new CCEaseCustom(this));
        }

        public override object Copy(ICCCopyable pZone)
        {
            if (pZone != null)
            {
                //in case of being called at sub class
                var pCopy = pZone as CCEaseCustom;
                base.Copy(pCopy);
                pCopy.InitWithAction((CCActionInterval) m_pInner.Copy(), m_EaseFunc);

                return pCopy;
            }
            return new CCEaseCustom(this);
        }
    }
}