using System;
using System.Reflection;
using Cocos2D;

namespace tests.Extensions
{
    internal class Loader<T> : CCLayerLoader where T : CCLayer, new()
    {
        public override CCNode CreateCCNode()
        {
            var ret = new T();
            ret.Init();
            return ret;
        }
    }

    public class BaseLayer : CCLayer, ICCBMemberVariableAssigner, ICCBSelectorResolver, ICCNodeLoaderListener
    {
        #region CCBMemberVariableAssigner Members

        public bool OnAssignCCBMemberVariable(object target, string memberVariableName, CCNode node)
        {
            FieldInfo fieldInfo = GetType().GetField(memberVariableName);
            if (fieldInfo == null)
            {
                fieldInfo = GetType().GetField(memberVariableName.Substring(0, 1).ToUpper() + memberVariableName.Substring(1));
            }
            if (fieldInfo != null)
            {
                fieldInfo.SetValue(this, node);
                return true;
            }
            else
            {
                CCLog.Log("Oops, could not find field named: {0}", memberVariableName);
            }
            return false;
        }

        #endregion

        #region CCBSelectorResolver Members

		public Action<object> OnResolveCCBCCMenuItemSelector(object target, string pSelectorName)
        {
            MethodInfo methodInfo = GetType().GetMethod(pSelectorName);
            if (methodInfo == null)
            {
                methodInfo = GetType().GetMethod(pSelectorName.Substring(0, 1).ToUpper() + pSelectorName.Substring(1));
            }
            if (methodInfo != null)
            {
#if NETFX_CORE
				return (Action<object>)methodInfo.CreateDelegate(typeof(Action<object>), target);
#else
				return (Action<object>)Delegate.CreateDelegate(typeof(Action<object>), this, methodInfo);
#endif
            }
            else
            {
                CCLog.Log("Oops, could not find menu selector named: {0}", pSelectorName);
            }
            return null;
        }

		public Action<object, CCControlEvent> OnResolveCCBCCControlSelector(object target, string pSelectorName)
        {
            MethodInfo methodInfo = GetType().GetMethod(pSelectorName);
            if (methodInfo == null)
            {
                methodInfo = GetType().GetMethod(pSelectorName.Substring(0, 1).ToUpper() + pSelectorName.Substring(1));
            }
            if (methodInfo != null)
            {
#if NETFX_CORE
				return (Action<object, CCControlEvent>) methodInfo.CreateDelegate(typeof(Action<object, CCControlEvent>), target);
#else
				return (Action<object, CCControlEvent>)Delegate.CreateDelegate(typeof(Action<object, CCControlEvent>), this, methodInfo);
#endif
            }
            else
            {
                CCLog.Log("Oops, could not find selector named: {0}", pSelectorName);
            }
            return null;
        }

        #endregion

        #region CCNodeLoaderListener Members

        public virtual void OnNodeLoaded(CCNode node, CCNodeLoader nodeLoader)
        {
        }

        #endregion
    }
}