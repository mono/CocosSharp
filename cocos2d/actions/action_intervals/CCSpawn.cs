using System;
using System.Diagnostics;

namespace Cocos2D
{
    public class CCSpawn : CCActionInterval
    {
        protected CCFiniteTimeAction m_pOne;
        protected CCFiniteTimeAction m_pTwo;

        protected CCSpawn(CCFiniteTimeAction action1, CCFiniteTimeAction action2)
        {
            InitOneTwo(action1, action2);
        }

        public CCSpawn(params CCFiniteTimeAction[] actions)
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
                    prev = new CCSpawn(prev, actions[i]);
                }

                InitOneTwo(prev, actions[actions.Length - 1]);
            }
        }

        protected CCSpawn(CCSpawn spawn) : base(spawn)
        {
            var param1 = spawn.m_pOne.Copy() as CCFiniteTimeAction;
            var param2 = spawn.m_pTwo.Copy() as CCFiniteTimeAction;

            InitOneTwo(param1, param2);
        }

        protected bool InitOneTwo(CCFiniteTimeAction action1, CCFiniteTimeAction action2)
        {
            Debug.Assert(action1 != null);
            Debug.Assert(action2 != null);

            bool bRet = false;

            float d1 = action1.Duration;
            float d2 = action2.Duration;

            if (base.InitWithDuration(Math.Max(d1, d2)))
            {
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

                bRet = true;
            }

            return bRet;
        }

        public override object Copy(ICCCopyable zone)
        {
            if (zone != null)
            {
                var ret = zone as CCSpawn;
                if (ret == null)
                {
                    return null;
                }
                base.Copy(zone);

                var param1 = m_pOne.Copy() as CCFiniteTimeAction;
                var param2 = m_pTwo.Copy() as CCFiniteTimeAction;
                if (param1 == null || param2 == null)
                {
                    return null;
                }

                ret.InitOneTwo(param1, param2);

                return ret;
            }
            else
            {
                return new CCSpawn(this);
            }
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
