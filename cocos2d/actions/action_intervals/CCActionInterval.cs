
using System;
using System.Diagnostics;

namespace cocos2d
{
    public class CCActionInterval : CCFiniteTimeAction
    {
        protected bool m_bFirstTick;
        protected float m_elapsed;

        public float Elapsed
        {
            get { return m_elapsed; }
        }

        public bool InitWithDuration(float d)
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

        public override object CopyWithZone(CCZone zone)
        {
            CCActionInterval ret;

            if (zone != null && zone.m_pCopyObject != null)
            {
                ret = (CCActionInterval)(zone.m_pCopyObject);
            }
            else
            {
                ret = new CCActionInterval();
                zone = new CCZone(ret);
            }

            base.CopyWithZone(zone);

            ret.InitWithDuration(m_fDuration);

            return ret;
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

        public static CCActionInterval Create(float d)
        {
            var ret = new CCActionInterval();
            ret.InitWithDuration(d);
            return ret;
        }
    }
}