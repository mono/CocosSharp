
namespace cocos2d
{
    public class CCEaseBounce : CCActionEase
    {

        public CCEaseBounce (CCActionInterval pAction) : base (pAction)
        { }

        public CCEaseBounce (CCEaseBounce easeBounce) : base (easeBounce)
        { }

        public float BounceTime(float time)
        {
            if (time < 1 / 2.75)
            {
                return 7.5625f * time * time;
            }
            
            if (time < 2 / 2.75)
            {
                time -= 1.5f / 2.75f;
                return 7.5625f * time * time + 0.75f;
            }
            
            if (time < 2.5 / 2.75)
            {
                time -= 2.25f / 2.75f;
                return 7.5625f * time * time + 0.9375f;
            }

            time -= 2.625f / 2.75f;
            return 7.5625f * time * time + 0.984375f;
        }

        public override object Copy(ICopyable pZone)
        {

            if (pZone != null)
            {
                //in case of being called at sub class
                var pCopy = pZone as CCEaseBounce;
                pCopy.InitWithAction((CCActionInterval) (m_pOther.Copy()));
                
                return pCopy;
            }
            else
            {
                return new CCEaseBounce(this);

            }

        }

    }
}