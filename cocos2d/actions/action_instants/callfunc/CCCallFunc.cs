
namespace cocos2d
{
    public class CCCallFunc : CCActionInstant
    {
        private SEL_CallFunc m_pCallFunc;
        protected string m_scriptFuncName;

        public CCCallFunc()
        {
            m_scriptFuncName = "";
            m_pCallFunc = null;
        }

        public CCCallFunc (SEL_CallFunc selector) : base ()
        {
            m_pCallFunc = selector;
        }

		protected CCCallFunc (CCCallFunc callFunc) : base (callFunc)
		{
			m_pCallFunc = callFunc.m_pCallFunc;
			m_scriptFuncName = callFunc.m_scriptFuncName;

		}

        public virtual void Execute()
        {
            if (null != m_pCallFunc)
            {
                m_pCallFunc();
            }
            //if (m_nScriptHandler) {
            //    CCScriptEngineManager::sharedManager()->getScriptEngine()->executeCallFuncActionEvent(this);
            //}
        }

        public override void Update(float time)
        {
            Execute();
        }

        public override object Copy(ICopyable pZone)
        {

            if (pZone != null)
            {
                //in case of being called at sub class
                var pRet = (CCCallFunc) (pZone);
				base.Copy(pZone);
				pRet.m_pCallFunc = m_pCallFunc;
				pRet.m_scriptFuncName = m_scriptFuncName;
				return pRet;
			}
            else
            {
                return new CCCallFunc(this);
            }

        }
    }
}