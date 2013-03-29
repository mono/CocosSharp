namespace cocos2d
{
    public class CCShow : CCActionInstant
    {
        public new static CCShow Create()
        {
            var pRet = new CCShow();
            return pRet;
        }

        public override void StartWithTarget(CCNode target)
        {
            base.StartWithTarget(target);
            target.Visible = true;
        }

        public override CCFiniteTimeAction Reverse()
        {
            return (CCHide.Create());
        }

        public override object Copy(ICopyable pZone)
        {
            CCShow pRet;
            if (pZone != null)
            {
                pRet = (CCShow) (pZone);
            }
            else
            {
                pRet = new CCShow();
                pZone =  (pRet);
            }

            base.Copy(pZone);
            return pRet;
        }
    }
}