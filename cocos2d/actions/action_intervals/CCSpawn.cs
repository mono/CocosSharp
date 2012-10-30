
using System;
using System.Diagnostics;

namespace cocos2d
{
    public class CCSpawn : CCActionInterval
    {
        protected CCFiniteTimeAction m_pOne;
        protected CCFiniteTimeAction m_pTwo;

        public static CCSpawn Create(params CCFiniteTimeAction[] actions)
        {
            CCFiniteTimeAction prev = actions[0];

            for (int i = 1; i < actions.Length; i++)
            {
                prev = ActionOneTwo(prev, actions[i]);
            }

            return (CCSpawn) prev;
        }

        protected static CCSpawn ActionOneTwo(CCFiniteTimeAction action1, CCFiniteTimeAction action2)
        {
            var spawn = new CCSpawn();
            spawn.InitOneTwo(action1, action2);

            return spawn;
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
                    m_pTwo = CCSequence.ActionOneTwo(action2, CCDelayTime.Create(d1 - d2));
                }
                else if (d1 < d2)
                {
                    m_pOne = CCSequence.ActionOneTwo(action1, CCDelayTime.Create(d2 - d1));
                }

                bRet = true;
            }

            return bRet;
        }

        public override CCObject CopyWithZone(CCZone zone)
        {
            CCZone tmpZone = zone;
            CCSpawn ret;

            if (tmpZone != null && tmpZone.m_pCopyObject != null)
            {
                ret = tmpZone.m_pCopyObject as CCSpawn;
                if (ret == null)
                {
                    return null;
                }
            }
            else
            {
                ret = new CCSpawn();
                tmpZone = new CCZone(ret);
            }

            base.CopyWithZone(tmpZone);

            var param1 = m_pOne.Copy() as CCFiniteTimeAction;
            var param2 = m_pTwo.Copy() as CCFiniteTimeAction;
            if (param1 == null || param2 == null)
            {
                return null;
            }

            ret.InitOneTwo(param1, param2);

            return ret;
        }

        public override void StartWithTarget(CCNode target)
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
            return ActionOneTwo(m_pOne.Reverse(), m_pTwo.Reverse());
        }
    }
}