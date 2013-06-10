using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cocos2D
{
    internal class CCRemoveSelf : CCActionInstant
    {
        protected bool m_bIsNeedCleanUp;

        public CCRemoveSelf(bool isNeedCleanUp)
        {
            Init(isNeedCleanUp);
        }

        protected CCRemoveSelf(CCRemoveSelf removeSelf)
            : base(removeSelf)
        {
            Init(removeSelf.m_bIsNeedCleanUp);
        }

        protected bool Init(bool isNeedCleanUp)
        {
            m_bIsNeedCleanUp = isNeedCleanUp;
            return true;
        }

        public override object Copy(ICCCopyable zone)
        {
            if (zone != null)
            {
                var ret = (CCRemoveSelf) zone;
                base.Copy(zone);
                ret.Init(m_bIsNeedCleanUp);
                return ret;
            }
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