using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CocosSharp
{
    public class CCRemoveSelf : CCActionInstant
    {
        protected bool m_bIsNeedCleanUp;


        #region Constructors

        public CCRemoveSelf()
        {
            InitCCRemoveSelf(true);
        }

        public CCRemoveSelf(bool isNeedCleanUp)
        {
            InitCCRemoveSelf(isNeedCleanUp);
        }

        protected CCRemoveSelf(CCRemoveSelf removeSelf)
            : base(removeSelf)
        {
            InitCCRemoveSelf(removeSelf.m_bIsNeedCleanUp);
        }

        private void InitCCRemoveSelf(bool isNeedCleanUp)
        {
            m_bIsNeedCleanUp = isNeedCleanUp;
        }

        #endregion Constructors


        public override object Copy(ICCCopyable zone)
        {
            return new CCRemoveSelf(this);
        }

        public override void Update(float time)
        {
            m_pTarget.RemoveFromParentAndCleanup(m_bIsNeedCleanUp);
        }

        public override CCFiniteTimeAction Reverse()
        {
            return new CCRemoveSelf(this);
        }
    }
}