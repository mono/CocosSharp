namespace cocos2d
{
    public class CCScaleBy : CCScaleTo
    {
        public override void StartWithTarget(CCNode target)
        {
            base.StartWithTarget(target);
            m_fDeltaX = m_fStartScaleX * m_fEndScaleX - m_fStartScaleX;
            m_fDeltaY = m_fStartScaleY * m_fEndScaleY - m_fStartScaleY;
        }

        public override CCFiniteTimeAction Reverse()
        {
            return Create(m_fDuration, 1 / m_fEndScaleX, 1 / m_fEndScaleY);
        }

        public override object Copy(ICopyable pZone)
        {
            CCScaleTo pCopy;

            if (pZone != null)
            {
                //in case of being called at sub class
                pCopy = (CCScaleBy) (pZone);
            }
            else
            {
                pCopy = new CCScaleBy();
                pZone =  (pCopy);
            }

            base.Copy(pZone);

            pCopy.InitWithDuration(m_fDuration, m_fEndScaleX, m_fEndScaleY);

            return pCopy;
        }

        public new static CCScaleBy Create(float duration, float s)
        {
            var pScaleBy = new CCScaleBy();
            pScaleBy.InitWithDuration(duration, s);

            return pScaleBy;
        }

        public new static CCScaleBy Create(float duration, float sx, float sy)
        {
            var pScaleBy = new CCScaleBy();
            pScaleBy.InitWithDuration(duration, sx, sy);
            return pScaleBy;
        }
    }
}