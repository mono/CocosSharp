using System;

namespace Cocos2D
{
    public class CCCallFuncO : CCCallFunc
    {
        private Action<object> m_pCallFuncO;
        private object m_pObject;

        public CCCallFuncO()
        {
            m_pObject = null;
            m_pCallFuncO = null;
        }

		public CCCallFuncO (Action<object> selector, object pObject) : this()
        {
            InitWithTarget(selector, pObject);
        }

		protected CCCallFuncO (CCCallFuncO callFuncO) : base (callFuncO)
		{
			InitWithTarget(callFuncO.m_pCallFuncO, callFuncO.m_pObject);
		}

		public bool InitWithTarget(Action<object> selector, object pObject)
        {
            m_pObject = pObject;
            m_pCallFuncO = selector;
            return true;
        }

        // super methods
        public override object Copy(ICopyable zone)
        {

            if (zone != null)
            {
                //in case of being called at sub class
                var pRet = (CCCallFuncO) (zone);
				base.Copy(zone);
				pRet.InitWithTarget(m_pCallFuncO, m_pObject);
				return pRet;
			}
            else
            {
                return new CCCallFuncO(this);
            }

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