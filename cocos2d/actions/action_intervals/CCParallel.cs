using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CocosSharp
{
    public class CCParallel : CCActionInterval
    {
        protected CCFiniteTimeAction[] m_pActions;


        #region Constructors

        public CCParallel()
        {
        }

        public CCParallel(params CCFiniteTimeAction[] actions) : base()
        {
            // Can't call base(duration) because max action duration needs to be determined here
            float maxDuration = actions.OrderByDescending (action => action.Duration).First().Duration;
            Duration = maxDuration;

            InitCCParallel(actions);
        }

        public CCParallel(CCParallel parallel) : base(parallel)
        {
            CCFiniteTimeAction[] cp = new CCFiniteTimeAction[parallel.m_pActions.Length];
            for (int i = 0; i < parallel.m_pActions.Length; i++)
            {
                cp[i] = new CCFiniteTimeAction(parallel.m_pActions [i]);
            }

            InitCCParallel(cp);
        }

        private void InitCCParallel(CCFiniteTimeAction[] actions)
        {
            m_pActions = actions;

            for (int i = 0; i < m_pActions.Length; i++)
            {
                var actionDuration = m_pActions[i].Duration;
                if (actionDuration < m_fDuration)
                {
                    m_pActions[i] = new CCSequence(m_pActions[i], new CCDelayTime(m_fDuration - actionDuration));
                }
            }
        }

        #endregion Constructors


        protected internal override void StartWithTarget(CCNode target)
        {
            base.StartWithTarget(target);
            for (int i = 0; i < m_pActions.Length; i++)
            {
                m_pActions[i].StartWithTarget(target);
            }
        }

        /// <summary>
        /// Reverses the current parallel sequence.
        /// </summary>
        /// <returns></returns>
        public override CCFiniteTimeAction Reverse()
        {
            CCFiniteTimeAction[] rev = new CCFiniteTimeAction[m_pActions.Length];
            for (int i = 0; i < m_pActions.Length; i++)
            {
                rev[i] = m_pActions[i].Reverse();
            }

            return new CCParallel(rev);
        }

        /// <summary>
        /// Makea full copy of this object and does not make any reference copies.
        /// </summary>
        /// <param name="zone"></param>
        /// <returns></returns>
        public override object Copy(ICCCopyable zone)
        {
            ICCCopyable tmpZone = zone;
            CCParallel ret;

            if (tmpZone != null && tmpZone != null)
            {
                ret = zone as CCParallel;
                base.Copy(zone);

                CCFiniteTimeAction[] cp = new CCFiniteTimeAction[m_pActions.Length];
                for (int i = 0; i < m_pActions.Length; i++)
                {
                    cp[i] = m_pActions[i].Copy() as CCFiniteTimeAction;
                }
                ret.m_pActions = cp;
                return ret;
            }
            else
            {
                return new CCParallel(this);
            }
        }

        public override void Stop()
        {
            for (int i = 0; i < m_pActions.Length; i++)
            {
                m_pActions[i].Stop();
            }
            base.Stop();
        }

        public override void Update(float time)
        {
            for (int i = 0; i < m_pActions.Length; i++)
            {
                m_pActions[i].Update(time);
            }
        }
    }
}