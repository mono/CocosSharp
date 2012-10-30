
namespace cocos2d
{
    public class CCActionInstant : CCFiniteTimeAction
    {
        public override CCObject CopyWithZone(CCZone zone)
        {
            CCZone tmpZone = zone;
            CCActionInstant ret;

            if (tmpZone != null && tmpZone.m_pCopyObject != null)
            {
                ret = (CCActionInstant) tmpZone.m_pCopyObject;
            }
            else
            {
                ret = new CCActionInstant();
                tmpZone = new CCZone(ret);
            }

            base.CopyWithZone(tmpZone);
            return ret;
        }

        public override bool IsDone
        {
            get { return true; }
        }

        public override void Step(float dt)
        {
            Update(1);
        }

        public override void Update(float time)
        {
            // ignore
        }

        public override CCFiniteTimeAction Reverse()
        {
            return (CCFiniteTimeAction) Copy();
        }
    }
}