using System;
using System.Diagnostics;

namespace CocosSharp
{
    // Extra action for making a CCSequence or CCSpawn when only adding one action to it.
    internal class CCExtraAction : CCFiniteTimeAction
    {
        public override CCAction Copy()
        {
            return new CCExtraAction();
        }

        public override CCFiniteTimeAction Reverse()
        {
            return new CCExtraAction();
        }

        public override void Step(float dt)
        {
        }

        public override void Update(float time)
        {
        }
    }

    public class CCActionInterval : CCFiniteTimeAction
    {
        protected bool m_bFirstTick;
        protected float m_elapsed;

        public float Elapsed
        {
            get { return m_elapsed; }
        }

        public override bool IsDone
        {
            get { return m_elapsed >= m_fDuration; }
        }


        #region Constructors

        protected CCActionInterval()
        {
        }

        public CCActionInterval(float d)
        {
            InitWithDuration(d);
        }

        // Perform a deep copy of CCACtionInterval
        protected internal CCActionInterval(CCActionInterval actionInterval) : base(actionInterval)
        {
            InitWithDuration(actionInterval.m_fDuration);
        }

        // Used by CCSequence and CCParallel
        // In general though, subclasses should aim to call the base constructor, rather than this explicitly
        protected void InitWithDuration(float d)
        {
            m_fDuration = d;

            // prevent division by 0
            // This comparison could be in step:, but it might decrease the performance
            // by 3% in heavy based action games.
            if (m_fDuration == 0)
            {
                m_fDuration = float.Epsilon;
            }

            m_elapsed = 0;
            m_bFirstTick = true;
        }

        #endregion Constructors


        public override object Copy(ICCCopyable zone)
        {
            return new CCActionInterval(this);
        }

        public override void Step(float dt)
        {
            if (m_bFirstTick)
            {
                m_bFirstTick = false;
                m_elapsed = 0f;
            }
            else
            {
                m_elapsed += dt;
            }

            Update(Math.Max(0f,
                            Math.Min(1, m_elapsed /
                                        Math.Max(m_fDuration, float.Epsilon)
                                )
                       )
                );
        }

        protected internal override void StartWithTarget(CCNode target)
        {
            base.StartWithTarget(target);
            m_elapsed = 0.0f;
            m_bFirstTick = true;
        }

        public override CCFiniteTimeAction Reverse()
        {
            throw new NotImplementedException();
        }

        public virtual float AmplitudeRate
        {
            get
            {
                Debug.Assert(false);
                return 0;
            }
            set { Debug.Assert(false); }
        }
    }
}