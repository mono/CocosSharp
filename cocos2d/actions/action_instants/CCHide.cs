namespace cocos2d
{
    public class CCHide : CCActionInstant
    {
        public CCHide()
        {
        }

        public override void StartWithTarget(CCNode target)
        {
            base.StartWithTarget(target);
            target.Visible = false;
        }

        public override CCFiniteTimeAction Reverse()
        {
            return (new CCShow());
        }

        public override object Copy(ICopyable pZone)
        {
            CCHide pRet;

            if (pZone != null)
            {
                pRet = (CCHide) (pZone);
            }
            else
            {
                pRet = new CCHide();
                pZone =  (pRet);
            }

            base.Copy(pZone);
            return pRet;
        }
    }
}