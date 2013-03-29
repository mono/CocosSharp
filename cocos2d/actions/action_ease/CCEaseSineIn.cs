
using System;
using Microsoft.Xna.Framework;

namespace cocos2d
{
    public class CCEaseSineIn : CCActionEase
    {
        public override void Update(float time)
        {
            m_pOther.Update(-1 * (float) Math.Cos(time * MathHelper.PiOver2) + 1);
        }

        public override CCFiniteTimeAction Reverse()
        {
            return CCEaseSineOut.Create((CCActionInterval) m_pOther.Reverse());
        }

        public override object CopyWithZone(CCZone pZone)
        {
            CCEaseSineIn pCopy;

            if (pZone != null && pZone.m_pCopyObject != null)
            {
                //in case of being called at sub class
                pCopy = (CCEaseSineIn) (pZone.m_pCopyObject);
            }
            else
            {
                pCopy = new CCEaseSineIn();
            }

            pCopy.InitWithAction((CCActionInterval) (m_pOther.Copy()));

            return pCopy;
        }


        public new static CCEaseSineIn Create(CCActionInterval pAction)
        {
            var pRet = new CCEaseSineIn();
            pRet.InitWithAction(pAction);
            return pRet;
        }
    }
}