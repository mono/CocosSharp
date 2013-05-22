
namespace Cocos2D
{
    public class CCActionInstant : CCFiniteTimeAction
    {

		protected CCActionInstant () {}

		protected CCActionInstant (CCActionInstant actionInstant) : base (actionInstant)
		{ }

        public override object Copy(ICopyable zone)
        {
            ICopyable tmpZone = zone;
            CCActionInstant ret;

            if (tmpZone != null && tmpZone != null)
            {
                ret = (CCActionInstant) tmpZone;
            }
            else
            {
                ret = new CCActionInstant();
                tmpZone =  (ret);
            }

            base.Copy(tmpZone);
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