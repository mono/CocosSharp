using System;
using System.Diagnostics;

namespace CocosSharp
{
    public class CCSpawn : CCActionInterval
    {
        protected CCFiniteTimeAction m_pOne;
        protected CCFiniteTimeAction m_pTwo;


        #region Constructors

        protected CCSpawn(CCFiniteTimeAction action1, CCFiniteTimeAction action2) : base(Math.Max(action1.Duration, action2.Duration))
        {
            InitCCSpawn(action1, action2);
        }

        public CCSpawn(params CCFiniteTimeAction[] actions)
        {
            CCFiniteTimeAction prev = actions[0];

            if (actions.Length == 1)
            {
                InitCCSpawn(prev, new CCExtraAction());
            }
            else
            {
                for (int i = 1; i < actions.Length - 1; i++)
                {
                    prev = new CCSpawn(prev, actions[i]);
                }

                InitCCSpawn(prev, actions[actions.Length - 1]);
            }
        }

        protected CCSpawn(CCSpawn spawn) : base(spawn)
        {
            CCFiniteTimeAction param1 = new CCFiniteTimeAction(spawn.m_pOne);
            CCFiniteTimeAction param2 = new CCFiniteTimeAction(spawn.m_pTwo);

            InitCCSpawn(param1, param2);
        }

        private void InitCCSpawn(CCFiniteTimeAction action1, CCFiniteTimeAction action2)
        {
            Debug.Assert(action1 != null);
            Debug.Assert(action2 != null);

            float d1 = action1.Duration;
            float d2 = action2.Duration;

            m_pOne = action1;
            m_pTwo = action2;

            if (d1 > d2)
            {
                m_pTwo = new CCSequence(action2, new CCDelayTime(d1 - d2));
            }
            else if (d1 < d2)
            {
                m_pOne = new CCSequence(action1, new CCDelayTime(d2 - d1));
            }
        }

        #endregion Constructors


        public override object Copy(ICCCopyable zone)
        {
            return new CCSpawn(this);
        }

        protected internal override void StartWithTarget(CCNode target)
        {
            base.StartWithTarget(target);
            m_pOne.StartWithTarget(target);
            m_pTwo.StartWithTarget(target);
        }

        public override void Stop()
        {
            m_pOne.Stop();
            m_pTwo.Stop();
            base.Stop();
        }

        public override void Update(float time)
        {
            if (m_pOne != null)
            {
                m_pOne.Update(time);
            }

            if (m_pTwo != null)
            {
                m_pTwo.Update(time);
            }
        }

        public override CCFiniteTimeAction Reverse()
        {
            return new CCSpawn(m_pOne.Reverse(), m_pTwo.Reverse());
        }
    }
}
