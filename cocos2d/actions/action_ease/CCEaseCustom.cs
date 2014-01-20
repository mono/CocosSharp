using System;

namespace CocosSharp
{
    public partial class CCEaseCustom : CCActionEase
    {
        private Func<float, float> m_EaseFunc;

        public Func<float, float> EaseFunc
        {
            get { return m_EaseFunc; }
            set { m_EaseFunc = value; }
        }


        #region Constructors

        public CCEaseCustom(CCActionInterval pAction, Func<float, float> easeFunc) : base(pAction)
        {
            EaseFunc = easeFunc;
        }

        // Perform a deep copy of CCEaseCustom
        protected CCEaseCustom(CCEaseCustom easeCustom) : base(easeCustom)
        {
            EaseFunc = easeCustom.EaseFunc;
        }

        #endregion Constructors


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
            return new CCEaseCustom(this);
        }
    }
}