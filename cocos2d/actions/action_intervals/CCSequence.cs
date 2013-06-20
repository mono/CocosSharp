using System.Diagnostics;

namespace Cocos2D
{
    public class CCSequence : CCActionInterval
    {
        protected int m_last;
        protected CCFiniteTimeAction[] m_pActions = new CCFiniteTimeAction[2];
        protected float m_split;
        private bool _HasInfiniteAction = false;

        public CCSequence(CCFiniteTimeAction action1, CCFiniteTimeAction action2)
        {
            InitOneTwo(action1, action2);
        }

        public CCSequence(params CCFiniteTimeAction[] actions)
        {
            CCFiniteTimeAction prev = actions[0];

            if (actions.Length == 1)
            {
                InitOneTwo(prev, new CCExtraAction());
            }
            else
            {
                for (int i = 1; i < actions.Length - 1; i++)
                {
                    prev = new CCSequence(prev, actions[i]);
                }

                InitOneTwo(prev, actions[actions.Length - 1]);
            }
        }

        protected CCSequence(CCSequence sequence) : base(sequence)
        {
            var param1 = sequence.m_pActions[0].Copy() as CCFiniteTimeAction;
            var param2 = sequence.m_pActions[1].Copy() as CCFiniteTimeAction;

            InitOneTwo(param1, param2);
        }

        protected bool InitOneTwo(CCFiniteTimeAction actionOne, CCFiniteTimeAction actionTwo)
        {
            Debug.Assert(actionOne != null);
            Debug.Assert(actionTwo != null);

            float d = actionOne.Duration + actionTwo.Duration;
            base.InitWithDuration(d);

            m_pActions[0] = actionOne;
            m_pActions[1] = actionTwo;

            _HasInfiniteAction = (actionOne is CCRepeatForever) || (actionTwo is CCRepeatForever);

            return true;
        }

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

        public override object Copy(ICCCopyable zone)
        {
            ICCCopyable tmpZone = zone;
            CCSequence ret;

            if (tmpZone != null && tmpZone != null)
            {
                ret = tmpZone as CCSequence;
                if (ret == null)
                {
                    return null;
                }
                base.Copy(tmpZone);

                var param1 = m_pActions[0].Copy() as CCFiniteTimeAction;
                var param2 = m_pActions[1].Copy() as CCFiniteTimeAction;

                if (param1 == null || param2 == null)
                {
                    return null;
                }

                ret.InitOneTwo(param1, param2);

                return ret;
            }
            else
            {
                return new CCSequence(this);
            }
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