namespace cocos2d
{
    public class CCShow : CCActionInstant
    {
        public CCShow()
        {
        }

        public override void StartWithTarget(CCNode target)
        {
            base.StartWithTarget(target);
            target.Visible = true;
        }

        public override CCFiniteTimeAction Reverse()
        {
            return (new CCHide());
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