namespace cocos2d
{
    public class CCEaseBackIn : CCActionEase
    {
        public override void Update(float time)
        {
            const float overshoot = 1.70158f;
            m_pOther.Update(time * time * ((overshoot + 1) * time - overshoot));
        }

        public override CCFiniteTimeAction Reverse()
        {
            return CCEaseBackOut.Create((CCActionInterval) m_pOther.Reverse());
        }

        public override object Copy(ICopyable pZone)
        {
            CCEaseBackIn pCopy;

            if (pZone != null)
            {
                //in case of being called at sub class
                pCopy = pZone as CCEaseBackIn;
            }
            else
            {
                pCopy = new CCEaseBackIn();
            }

            pCopy.InitWithAction((CCActionInterval) (m_pOther.Copy()));

            return pCopy;
        }

        public new static CCEaseBackIn Create(CCActionInterval pAction)
        {
            var pRet = new CCEaseBackIn();
            pRet.InitWithAction(pAction);
            return pRet;
        }
    }
}