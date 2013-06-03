
using System;
using System.Diagnostics;

namespace Cocos2D
{
    public class CCActionInterval : CCFiniteTimeAction
    {
        protected bool m_bFirstTick;
        protected float m_elapsed;

        protected CCActionInterval () {}

        public CCActionInterval (float d)
        {
            InitWithDuration(d);
        }

        protected CCActionInterval (CCActionInterval actionInterval) : base (actionInterval)
        {
            InitWithDuration(actionInterval.m_fDuration);

        }

        public float Elapsed
        {
            get { return m_elapsed; }
        }

        protected bool InitWithDuration(float d)
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

            return true;
        }

        public override bool IsDone
        {
            get { return m_elapsed >= m_fDuration; }
        }

        public override object Copy(ICCCopyable zone)
        {

            if (zone != null)
            {
                var ret = (CCActionInterval)(zone);
                base.Copy(zone);
                
                ret.InitWithDuration(m_fDuration);
                return ret;

            }
            else
            {
                return new CCActionInterval(this);
            }

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

        public override void StartWithTarget(CCNode target)
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