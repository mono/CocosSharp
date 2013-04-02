
using System.Diagnostics;

namespace cocos2d
{
    public class CCSequence : CCActionInterval
    {
        protected int m_last;
        protected CCFiniteTimeAction[] m_pActions = new CCFiniteTimeAction[2];
        protected float m_split;


		public CCSequence (CCFiniteTimeAction action1, CCFiniteTimeAction action2)
		{
			InitOneTwo(action1, action2);
		}

		protected CCSequence (CCSequence sequence) : base (sequence)
		{

			var param1 = sequence.m_pActions[0].Copy() as CCFiniteTimeAction;
			var param2 = sequence.m_pActions[1].Copy() as CCFiniteTimeAction;

			InitOneTwo(param1, param2);
			
		}

        protected bool InitOneTwo(CCFiniteTimeAction actionOne, CCFiniteTimeAction aciontTwo)
        {
            Debug.Assert(actionOne != null);
            Debug.Assert(aciontTwo != null);

            float d = actionOne.Duration + aciontTwo.Duration;
            base.InitWithDuration(d);

            m_pActions[0] = actionOne;
            m_pActions[1] = aciontTwo;

            return true;
        }

        public override object Copy(ICopyable zone)
        {
            ICopyable tmpZone = zone;
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

        public override void StartWithTarget(CCNode target)
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

        public override void Update(float t)
        {
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

            // Last action found and it is done.
            if (found == m_last && m_pActions[found].IsDone)
            {
                return;
            }

            // New action. Start it.
            if (found != m_last)
            {
                m_pActions[found].StartWithTarget(m_pTarget);
            }

            m_pActions[found].Update(new_t);
            m_last = found;
        }

        public override CCFiniteTimeAction Reverse()
        {
            return new CCSequence (m_pActions[1].Reverse(), m_pActions[0].Reverse());
        }

		public static CCSequence FromActions(params CCFiniteTimeAction[] actions)
		{
			CCFiniteTimeAction prev = actions[0];
			
			for (int i = 1; i < actions.Length; i++)
			{
				prev = new CCSequence (prev, actions[i]);
			}
			
			return (CCSequence) prev;
		}
		

    }
}