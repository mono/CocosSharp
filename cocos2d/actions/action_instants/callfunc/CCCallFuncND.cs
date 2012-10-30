
namespace cocos2d
{
    public class CCCallFuncND : CCCallFuncN
    {
        protected SEL_CallFuncND m_pCallFuncND;
        protected object m_pData;

        public static CCCallFuncND Create(SEL_CallFuncND selector, object d)
        {
            var pRet = new CCCallFuncND();
            pRet.InitWithTarget(selector, d);
            return pRet;
        }

        public bool InitWithTarget(SEL_CallFuncND selector, object d)
        {
            m_pData = d;
            m_pCallFuncND = selector;
            return true;
        }

        public override CCObject CopyWithZone(CCZone zone)
        {
            CCCallFuncND pRet;

            if (zone != null && zone.m_pCopyObject != null)
            {
                //in case of being called at sub class
                pRet = (CCCallFuncND) (zone.m_pCopyObject);
            }
            else
            {
                pRet = new CCCallFuncND();
                zone = new CCZone(pRet);
            }

            base.CopyWithZone(zone);
            pRet.InitWithTarget(m_pCallFuncND, m_pData);
            return pRet;
        }

        public override void Execute()
        {
            if (null != m_pCallFuncND)
            {
                m_pCallFuncND(m_pTarget, m_pData);
            }

            //if (CCScriptEngineManager::sharedScriptEngineManager()->getScriptEngine()) {
            //    CCScriptEngineManager::sharedScriptEngineManager()->getScriptEngine()->executeCallFuncND(
            //            m_scriptFuncName.c_str(), m_pTarget, m_pData);
            //}
        }
    }
}