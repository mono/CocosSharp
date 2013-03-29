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

        public override object Copy(ICopyable pZone)
        {
            CCFadeOut pCopy;
            if (pZone != null)
            {
                //in case of being called at sub class
                pCopy = (CCFadeOut) (pZone);
            }
            else
            {
                pCopy = new CCFadeOut();
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
                pRGBAProtocol.Opacity = (byte) (255 * (1 - time));
            }
        }

        public override CCFiniteTimeAction Reverse()
        {
            return CCFadeIn.Create(m_fDuration);
        }
    }
}