namespace cocos2d
{
    public class CCFadeIn : CCActionInterval
    {
        public new static CCFadeIn Create(float d)
        {
            var pAction = new CCFadeIn();
            pAction.InitWithDuration(d);
            return pAction;
        }

        public override object CopyWithZone(CCZone pZone)
        {
            CCFadeIn pCopy;
            if (pZone != null && pZone.m_pCopyObject != null)
            {
                //in case of being called at sub class
                pCopy = (CCFadeIn) (pZone.m_pCopyObject);
            }
            else
            {
                pCopy = new CCFadeIn();
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
                pRGBAProtocol.Opacity = (byte) (255 * time);
            }
        }

        public override CCFiniteTimeAction Reverse()
        {
            return CCFadeOut.Create(m_fDuration);
        }
    }
}