namespace cocos2d
{
    public class CCTintBy : CCActionInterval
    {
        protected short m_deltaB;
        protected short m_deltaG;
        protected short m_deltaR;
        protected short m_fromB;
        protected short m_fromG;
        protected short m_fromR;

        public bool InitWithDuration(float duration, short deltaRed, short deltaGreen, short deltaBlue)
        {
            if (base.InitWithDuration(duration))
            {
                m_deltaR = deltaRed;
                m_deltaG = deltaGreen;
                m_deltaB = deltaBlue;

                return true;
            }

            return false;
        }

        public override CCObject CopyWithZone(CCZone zone)
        {
            CCZone tmpZone = zone;
            CCTintBy ret;

            if (tmpZone != null && tmpZone.m_pCopyObject != null)
            {
                ret = tmpZone.m_pCopyObject as CCTintBy;
                if (ret == null)
                {
                    return null;
                }
            }
            else
            {
                ret = new CCTintBy();
                tmpZone = new CCZone(ret);
            }

            base.CopyWithZone(tmpZone);

            ret.InitWithDuration(m_fDuration, m_deltaR, m_deltaG, m_deltaB);

            return ret;
        }

        public override void StartWithTarget(CCNode target)
        {
            base.StartWithTarget(target);

            var protocol = target as ICCRGBAProtocol;
            if (protocol != null)
            {
                CCColor3B color = protocol.Color;
                m_fromR = color.R;
                m_fromG = color.G;
                m_fromB = color.B;
            }
        }

        public override void Update(float time)
        {
            var protocol = m_pTarget as ICCRGBAProtocol;
            if (protocol != null)
            {
                protocol.Color = new CCColor3B((byte) (m_fromR + m_deltaR * time),
                                               (byte) (m_fromG + m_deltaG * time),
                                               (byte) (m_fromB + m_deltaB * time));
            }
        }

        public override CCFiniteTimeAction Reverse()
        {
            return Create(m_fDuration, (short) -m_deltaR, (short) -m_deltaG, (short) -m_deltaB);
        }

        public static CCTintBy Create(float duration, short deltaRed, short deltaGreen, short deltaBlue)
        {
            var ret = new CCTintBy();
            ret.InitWithDuration(duration, deltaRed, deltaGreen, deltaBlue);
            return ret;
        }
    }
}