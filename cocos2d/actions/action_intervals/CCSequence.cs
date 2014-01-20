using System.Diagnostics;
using System.Linq;

namespace CocosSharp
{
    public class CCSequence : CCActionInterval
    {
        protected int m_last;
        protected CCFiniteTimeAction[] m_pActions = new CCFiniteTimeAction[2];
        protected float m_split;
        private bool _HasInfiniteAction = false;

        public override bool IsDone
        {
            get
            {
                if (_HasInfiniteAction && m_pActions[m_last] is CCRepeatForever)
                {
                    return (false);
                }
                return base.IsDone;
            }
        }


        #region Constructors

        public CCSequence(CCFiniteTimeAction action1, CCFiniteTimeAction action2) : base(action1.Duration + action2.Duration)
        {
            InitCCSequence(action1, action2);
        }

        public CCSequence(params CCFiniteTimeAction[] actions) : base()
        {
            CCFiniteTimeAction prev = actions[0];

            // Can't call base(duration) because we need to calculate duration here
            float combinedDuration = actions.Sum(action => action.Duration);
            base.InitWithDuration(combinedDuration);

            if (actions.Length == 1)
            {
                InitCCSequence(prev, new CCExtraAction());
            }
            else
            {
                for (int i = 1; i < actions.Length - 1; i++)
                {
                    prev = new CCSequence(prev, actions[i]);
                }

                InitCCSequence(prev, actions[actions.Length - 1]);
            }
        }

        // Perform deep copy of CCSequence
        protected CCSequence(CCSequence sequence) : base(sequence)
        {
            CCFiniteTimeAction param1 = new CCFiniteTimeAction(sequence.m_pActions [0]);
            CCFiniteTimeAction param2 = new CCFiniteTimeAction(sequence.m_pActions [1]);

            InitCCSequence(param1, param2);
        }

        private void InitCCSequence(CCFiniteTimeAction actionOne, CCFiniteTimeAction actionTwo)
        {
            Debug.Assert(actionOne != null);
            Debug.Assert(actionTwo != null);

            m_pActions[0] = actionOne;
            m_pActions[1] = actionTwo;

            _HasInfiniteAction = (actionOne is CCRepeatForever) || (actionTwo is CCRepeatForever);
        }

        #endregion Constructors


        public override object Copy(ICCCopyable zone)
        {
            return new CCSequence(this);
        }

        protected internal override void StartWithTarget(CCNode target)
        {
            base.StartWithTarget(target);
            m_split = m_pActions[0].Duration / m_fDuration;
            m_last = -1;
        }

        public override void Stop()
        {
            // Issue #1305
            if (m_last != - 1)
            {
                m_pActions[m_last].Stop();
            }

            base.Stop();
        }

        public override void Step(float dt)
        {
            if (m_last > -1 && (m_pActions[m_last] is CCRepeat || m_pActions[m_last] is CCRepeatForever))
            {
                // Repeats are step based, not update
                m_pActions[m_last].Step(dt);
            }
            else
            {
                base.Step(dt);
            }
        }

        public override void Update(float t)
        {
            bool bRestart = false;
            int found;
            float new_t;

            if (t < m_split)
            {
                // action[0]
                found = 0;
                if (m_split != 0)
                    new_t = t / m_split;
                else
                    new_t = 1;
            }
            else
            {
                // action[1]
                found = 1;
                if (m_split == 1)
                    new_t = 1;
                else
                    new_t = (t - m_split) / (1 - m_split);
            }

            if (found == 1)
            {
                if (m_last == -1)
                {
                    // action[0] was skipped, execute it.
                    m_pActions[0].StartWithTarget(m_pTarget);
                    m_pActions[0].Update(1.0f);
                    m_pActions[0].Stop();
                }
                else if (m_last == 0)
                {
                    // switching to action 1. stop action 0.
                    m_pActions[0].Update(1.0f);
                    m_pActions[0].Stop();
                }
            }
            else if (found == 0 && m_last == 1)
            {
                // Reverse mode ?
                // XXX: Bug. this case doesn't contemplate when _last==-1, found=0 and in "reverse mode"
                // since it will require a hack to know if an action is on reverse mode or not.
                // "step" should be overriden, and the "reverseMode" value propagated to inner Sequences.
                m_pActions[1].Update(0);
                m_pActions[1].Stop();
            }
            // Last action found and it is done.
            if (found == m_last && m_pActions[found].IsDone)
            {
                return;
            }

            // Last action found and it is done
            if (found != m_last || bRestart)
            {
                m_pActions[found].StartWithTarget(m_pTarget);
            }

            m_pActions[found].Update(new_t);
            m_last = found;
        }

        public override CCFiniteTimeAction Reverse()
        {
            return new CCSequence(m_pActions[1].Reverse(), m_pActions[0].Reverse());
        }
    }
}