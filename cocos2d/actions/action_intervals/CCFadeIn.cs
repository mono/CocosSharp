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

        public override object Copy(ICopyable pZone)
        {
            CCFadeIn pCopy;
            if (pZone != null)
            {
                //in case of being called at sub class
                pCopy = (CCFadeIn) (pZone);
            }
            else
            {
                pCopy = new CCFadeIn();
                pZone =  (pCopy);
            }

            base.Copy(pZone);

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