using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cocos2D
{
    public class CCParallel : CCFiniteTimeAction
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
            m_pActions.CopyTo(rev, 0);
            return (new CCParallel(rev));
        }

        /// <summary>
        /// Makea full copy of this object and does not make any reference copies.
        /// </summary>
        /// <param name="zone"></param>
        /// <returns></returns>
        public override object Copy(ICopyable zone)
        {
            ICopyable tmpZone = zone;
            CCParallel ret;

            if (tmpZone != null && tmpZone != null)
            {
                ret = tmpZone as CCParallel;
                base.Copy(tmpZone);

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

        public override bool IsDone
        {
            get
            {
                foreach (CCFiniteTimeAction action in m_pActions)
                {
                    if (!action.IsDone)
                    {
                        return (false);
                    }
                }
                return (true);
            }
        }

        public override void Update(float time)
        {
            base.Update(time);
            foreach (CCFiniteTimeAction action in m_pActions)
            {
                if (!action.IsDone)
                {
                    action.Update(time);
                }
            }
        }
    }
}
