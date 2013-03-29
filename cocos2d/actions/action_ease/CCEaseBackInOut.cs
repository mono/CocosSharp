namespace cocos2d
{
    public class CCEaseBackInOut : CCActionEase
    {
        public override void Update(float time)
        {
            const float overshoot = 1.70158f * 1.525f;

            time = time * 2;
            if (time < 1)
            {
                m_pOther.Update((time * time * ((overshoot + 1) * time - overshoot)) / 2);
            }
            else
            {
                time = time - 2;
                m_pOther.Update((time * time * ((overshoot + 1) * time + overshoot)) / 2 + 1);
            }
        }

        public override object Copy(ICopyable pZone)
        {
            CCEaseBackInOut pCopy;

            if (pZone != null)
            {
                //in case of being called at sub class
                pCopy = pZone as CCEaseBackInOut;
            }
            else
            {
                pCopy = new CCEaseBackInOut();
            }

            pCopy.InitWithAction((CCActionInterval) (m_pOther.Copy()));

            return pCopy;
        }

        public override CCFiniteTimeAction Reverse()
        {
            return Create((CCActionInterval) m_pOther.Reverse());
        }

        public new static CCEaseBackInOut Create(CCActionInterval pAction)
        {
            var pRet = new CCEaseBackInOut();
            pRet.InitWithAction(pAction);
            return pRet;
        }
    }
}