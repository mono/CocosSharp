namespace cocos2d
{
    public class CCFadeTo : CCActionInterval
    {
        protected byte m_fromOpacity;
        protected byte m_toOpacity;

        public bool InitWithDuration(float duration, byte opacity)
        {
            if (base.InitWithDuration(duration))
            {
                m_toOpacity = opacity;
                return true;
            }

            return false;
        }

        public override CCObject CopyWithZone(CCZone pZone)
        {
            CCFadeTo pCopy;
            if (pZone != null && pZone.m_pCopyObject != null)
            {
                //in case of being called at sub class
                pCopy = (CCFadeTo) (pZone.m_pCopyObject);
            }
            else
            {
                pCopy = new CCFadeTo();
                pZone = new CCZone(pCopy);
            }

            base.CopyWithZone(pZone);

            pCopy.InitWithDuration(m_fDuration, m_toOpacity);

            return pCopy;
        }

        public override void StartWithTarget(CCNode target)
        {
            base.StartWithTarget(target);

            var pRGBAProtocol = target as ICCRGBAProtocol;
            if (pRGBAProtocol != null)
            {
                m_fromOpacity = pRGBAProtocol.Opacity;
            }
        }

        public override void Update(float time)
        {
            var pRGBAProtocol = m_pTarget as ICCRGBAProtocol;
            if (pRGBAProtocol != null)
            {
                pRGBAProtocol.Opacity = (byte) (m_fromOpacity + (m_toOpacity - m_fromOpacity) * time);
            }
        }

        public static CCFadeTo Create(float duration, byte opacity)
        {
            var pFadeTo = new CCFadeTo();
            pFadeTo.InitWithDuration(duration, opacity);

            return pFadeTo;
        }
    }
}