namespace CocosSharp
{
    public class CCActionInstant : CCFiniteTimeAction
    {
        public override bool IsDone
        {
            get { return true; }
        }

        #region Constructors

        protected CCActionInstant()
        {
        }

        protected CCActionInstant(CCActionInstant actionInstant) : base(actionInstant)
        {
        }

        #endregion Constructors


        public override object Copy(ICCCopyable zone)
        {
            return new CCActionInstant(this);
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