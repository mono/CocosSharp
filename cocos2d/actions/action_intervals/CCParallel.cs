using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cocos2D
{
    public class CCParallel : CCActionInterval
    {
        protected CCFiniteTimeAction[] m_pActions;

        public CCParallel()
        {
        }

        /// <summary>
        /// Constructs the parallel sequence from the given array of actions.
        /// </summary>
        /// <param name="actions"></param>
        public CCParallel(params CCFiniteTimeAction[] actions)
        {
            m_pActions = actions;
            float duration = 0f;
            for (int i = 0; i < m_pActions.Length; i++)
            {
                var actionDuration = m_pActions[i].Duration;
                if (duration < actionDuration)
                {
                    duration = actionDuration;
                }
            }

            for (int i = 0; i < m_pActions.Length; i++)
            {
                var actionDuration = m_pActions[i].Duration;
                if (actionDuration < duration)
                {
                    m_pActions[i] = new CCSequence(m_pActions[i], new CCDelayTime(duration - actionDuration));
                }
            }

            base.InitWithDuration(duration);
        }

        protected internal override void StartWithTarget(CCNode target)
        {
            base.StartWithTarget(target);
            for (int i = 0; i < m_pActions.Length; i++)
            {
                m_pActions[i].StartWithTarget(target);
            }
        }

        public CCParallel(CCParallel copy) : base(copy)
        {
            CCFiniteTimeAction[] cp = new CCFiniteTimeAction[copy.m_pActions.Length];
            for (int i = 0; i < copy.m_pActions.Length; i++)
            {
                cp[i] = copy.m_pActions[i].Copy() as CCFiniteTimeAction;
            }
            m_pActions = cp;
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