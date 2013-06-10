namespace Cocos2D
{
    public class CCActionInstant : CCFiniteTimeAction
    {
        protected CCActionInstant()
        {
        }

        protected CCActionInstant(CCActionInstant actionInstant) : base(actionInstant)
        {
        }

        public override object Copy(ICCCopyable zone)
        {
            if (zone != null)
            {
                var ret = (CCActionInstant) zone;
                base.Copy(zone);
                return ret;
            }
            return new CCActionInstant(this);
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