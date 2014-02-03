using System.Diagnostics;

namespace CocosSharp
{
    public class CCRepeatForever : CCActionInterval
    {
        protected CCActionInterval m_pInnerAction;


        #region Constructors

        public CCActionInterval InnerAction
        {
            get { return m_pInnerAction; }
            set { m_pInnerAction = value; }
        }

		public CCRepeatForever (params CCFiniteTimeAction[] actions)
		{
			InitCCRepeatForever (new CCSequence (actions));

		}

        public CCRepeatForever(CCActionInterval action)
        {
            InitCCRepeatForever(action);
        }

        // Perform deep copy of CCRepeatForever
        public CCRepeatForever(CCRepeatForever repeatForever) : base(repeatForever)
        {
            // The InnerAction may be a subclass of CCActionInterval
            // Therefore, use the Copy method to ensure the "deepest" constructor is used
            InitCCRepeatForever(repeatForever.m_pInnerAction.Copy() as CCActionInterval);
        }

        private void InitCCRepeatForever(CCActionInterval action)
        {
            Debug.Assert(action != null);
            m_pInnerAction = action;
        }

        #endregion Constructors


        public override object Copy(ICCCopyable zone)
        {
            return new CCRepeatForever(this);
        }

        protected internal override void StartWithTarget(CCNode target)
        {
            base.StartWithTarget(target);
            m_pInnerAction.StartWithTarget(target);
        }

        public override void Step(float dt)
        {
            m_pInnerAction.Step(dt);

            if (m_pInnerAction.IsDone)
            {
                float diff = m_pInnerAction.Elapsed - m_pInnerAction.Duration;
                m_pInnerAction.StartWithTarget(m_pTarget);
                m_pInnerAction.Step(0f);
                m_pInnerAction.Step(diff);
            }
        }

        public override bool IsDone
        {
            get { return false; }
        }

        public override CCFiniteTimeAction Reverse()
        {
            return new CCRepeatForever(m_pInnerAction.Reverse() as CCActionInterval);
        }
    }
}