namespace cocos2d
{
    public class CCHide : CCActionInstant
    {
        public new static CCHide Create()
        {
            var pRet = new CCHide();
            return pRet;
        }

        public override void StartWithTarget(CCNode target)
        {
            base.StartWithTarget(target);
            target.Visible = false;
        }

        public override CCFiniteTimeAction Reverse()
        {
            return (CCShow.Create());
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