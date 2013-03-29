namespace cocos2d
{
    public class CCTintTo : CCActionInterval
    {
        protected CCColor3B m_from;
        protected CCColor3B m_to;

        public bool InitWithDuration(float duration, byte red, byte green, byte blue)
        {
            if (base.InitWithDuration(duration))
            {
                m_to = new CCColor3B(red, green, blue);
                return true;
            }

            return false;
        }

        public override object CopyWithZone(CCZone zone)
        {
            CCZone tmpZone = zone;
            CCTintTo ret;

            if (tmpZone != null && tmpZone.m_pCopyObject != null)
            {
                ret = tmpZone.m_pCopyObject as CCTintTo;
                if (ret == null)
                {
                    return null;
                }
            }
            else
            {
                ret = new CCTintTo();
                tmpZone = new CCZone(ret);
            }

            base.CopyWithZone(tmpZone);

            ret.InitWithDuration(m_fDuration, m_to.R, m_to.G, m_to.B);

            return ret;
        }

        public override void StartWithTarget(CCNode target)
        {
            base.StartWithTarget(target);
            var protocol = m_pTarget as ICCRGBAProtocol;
            if (protocol != null)
            {
                m_from = protocol.Color;
            }
        }

        public override void Update(float time)
        {
            var protocol = m_pTarget as ICCRGBAProtocol;
            if (protocol != null)
            {
                protocol.Color = new CCColor3B((byte) (m_from.R + (m_to.R - m_from.R) * time),
                                               (byte) (m_from.G + (m_to.G - m_from.G) * time),
                                               (byte) (m_from.B + (m_to.B - m_from.B) * time));
            }
        }

        public static CCTintTo Create(float duration, byte red, byte green, byte blue)
        {
            var ret = new CCTintTo();
            ret.InitWithDuration(duration, red, green, blue);
            return ret;
        }
    }
}