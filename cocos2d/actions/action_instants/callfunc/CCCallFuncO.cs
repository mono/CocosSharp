
namespace cocos2d
{
    public class CCCallFuncO : CCCallFunc
    {
        private SEL_CallFuncO m_pCallFuncO;
        private object m_pObject;

        public CCCallFuncO()
        {
            m_pObject = null;
            m_pCallFuncO = null;
        }

        public static CCCallFuncO Create(SEL_CallFuncO selector, object pObject)
        {
            var pRet = new CCCallFuncO();
            pRet.InitWithTarget(selector, pObject);
            return pRet;
        }

        public bool InitWithTarget(SEL_CallFuncO selector, object pObject)
        {
            m_pObject = pObject;
            m_pCallFuncO = selector;
            return true;
        }

        // super methods
        public override object Copy(ICopyable zone)
        {
            CCCallFuncO pRet;

            if (zone != null)
            {
                //in case of being called at sub class
                pRet = (CCCallFuncO) (zone);
            }
            else
            {
                pRet = new CCCallFuncO();
                zone =  (pRet);
            }

            base.Copy(zone);
            pRet.InitWithTarget(m_pCallFuncO, m_pObject);
            return pRet;
        }

        public override void Execute()
        {
            if (null != m_pCallFuncO)
            {
                m_pCallFuncO(m_pObject);
            }

            //if (CCScriptEngineManager::sharedScriptEngineManager()->getScriptEngine()) {
            //    CCScriptEngineManager::sharedScriptEngineManager()->getScriptEngine()->executeCallFunc0(
            //            m_scriptFuncName.c_str(), m_pObject);
            //}
        }

        public object Object
        {
            get { return m_pObject; }
            set { m_pObject = value; }
        }
    }
}