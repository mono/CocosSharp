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

        public override CCObject CopyWithZone(CCZone zone)
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

            ret.InitWithDuration(m_fDuration, m_to.r, m_to.g, m_to.b);

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
                protocol.Color = new CCColor3B((byte) (m_from.r + (m_to.r - m_from.r) * time),
                                               (byte) (m_from.g + (m_to.g - m_from.g) * time),
                                               (byte) (m_from.b + (m_to.b - m_from.b) * time));
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