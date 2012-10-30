namespace cocos2d
{
    public class CCFadeOut : CCActionInterval
    {
        public new static CCFadeOut Create(float d)
        {
            var pAction = new CCFadeOut();
            pAction.InitWithDuration(d);
            return pAction;
        }

        public override CCObject CopyWithZone(CCZone pZone)
        {
            CCFadeOut pCopy;
            if (pZone != null && pZone.m_pCopyObject != null)
            {
                //in case of being called at sub class
                pCopy = (CCFadeOut) (pZone.m_pCopyObject);
            }
            else
            {
                pCopy = new CCFadeOut();
                pZone = new CCZone(pCopy);
            }

            base.CopyWithZone(pZone);

            return pCopy;
        }

        public override void Update(float time)
        {
            var pRGBAProtocol = m_pTarget as ICCRGBAProtocol;
            if (pRGBAProtocol != null)
            {
                pRGBAProtocol.Opacity = (byte) (255 * (1 - time));
            }
        }

        public override CCFiniteTimeAction Reverse()
        {
            return CCFadeIn.Create(m_fDuration);
        }
    }
}