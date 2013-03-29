namespace cocos2d
{
    public class CCDelayTime : CCActionInterval
    {
        public new static CCDelayTime Create(float d)
        {
            var pAction = new CCDelayTime();
            pAction.InitWithDuration(d);
            return pAction;
        }

        public override object Copy(ICopyable pZone)
        {
            CCDelayTime pCopy;
            if (pZone != null)
            {
                //in case of being called at sub class
                pCopy = (CCDelayTime) (pZone);
            }
            else
            {
                pCopy = new CCDelayTime();
                pZone =  (pCopy);
            }


            base.Copy(pZone);

            return pCopy;
        }

        public override void Update(float time)
        {
        }

        public override CCFiniteTimeAction Reverse()
        {
            return Create(m_fDuration);
        }
    }
}