
namespace Cocos2D
{
    public class CCActionEase : CCActionInterval
    {
        protected CCActionInterval m_pOther;

		// This can be taken out once all the classes that extend it have had their constructors created.
//		protected CCActionEase ()
//		{}

        public CCActionEase (CCActionInterval pAction)
        {
			InitWithAction(pAction);
        }

        protected CCActionEase (CCActionEase actionEase) : base (actionEase)
        {
            InitWithAction((CCActionInterval) (actionEase.m_pOther.Copy()));
        }

        protected bool InitWithAction(CCActionInterval pAction)
        {
            if (base.InitWithDuration(pAction.Duration))
            {
                m_pOther = pAction;
                return true;
            }
            return false;
        }

        public override object Copy(ICopyable pZone)
        {
            if (pZone != null)
            {
                //in case of being called at sub class
                var pCopy = pZone as CCActionEase;
                base.Copy(pZone);
                
                pCopy.InitWithAction((CCActionInterval) (m_pOther.Copy()));
                
                return pCopy;
            }
            else
            {
                return new CCActionEase(this);
            }

        }

        public override void StartWithTarget(CCNode target)
        {
            base.StartWithTarget(target);
            m_pOther.StartWithTarget(m_pTarget);
        }

        public override void Stop()
        {
            m_pOther.Stop();
            base.Stop();
        }

        public override void Update(float time)
        {
            m_pOther.Update(time);
        }

        public override CCFiniteTimeAction Reverse()
        {
            return new CCActionEase((CCActionInterval) m_pOther.Reverse());
        }

    }
}