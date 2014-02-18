using System;

namespace CocosSharp
{
    public class CCCallFuncN : CCCallFunc
    {
		public Action<CCNode> CallFunctionN { get; private set; }

        #region Constructors

        public CCCallFuncN() : base()
        {
        }

        public CCCallFuncN(Action<CCNode> selector) : base()
        {
            CallFunctionN = selector;
        }

        #endregion Constructors

		/// <summary>
		/// Start the Call Function operation on the given target.
		/// </summary>
		/// <param name="target"></param>
		protected internal override CCActionState StartAction (CCNode target)
		{
			return new CCCallFuncNState (this, target);

		}

    }

	public class CCCallFuncNState : CCCallFuncState
	{

		protected Action<CCNode> CallFunctionN { get; set; }

		public CCCallFuncNState (CCCallFuncN action, CCNode target)
			: base(action, target)
		{	
			CallFunctionN = action.CallFunctionN;
		}

		public override void Execute()
		{
			if (null != CallFunctionN)
			{
				CallFunctionN(Target);
			}
			//if (m_nScriptHandler) {
			//    CCScriptEngineManager::sharedManager()->getScriptEngine()->executeFunctionWithobject(m_nScriptHandler, m_pTarget, "CCNode");
			//}
		}

	}
}