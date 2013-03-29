using System;
using System.Reflection;
using cocos2d;

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

    public class BaseLayer : CCLayer, CCBMemberVariableAssigner, CCBSelectorResolver, CCNodeLoaderListener
    {
        #region CCBMemberVariableAssigner Members

        public bool OnAssignCCBMemberVariable(object target, string memberVariableName, CCNode node)
        {
            FieldInfo fieldInfo = GetType().GetField(memberVariableName);
            if (fieldInfo != null)
            {
                fieldInfo.SetValue(this, node);
                return true;
            }
            return false;
        }

        #endregion

        #region CCBSelectorResolver Members

        public SEL_MenuHandler OnResolveCCBCCMenuItemSelector(object target, string pSelectorName)
        {
            MethodInfo methodInfo = GetType().GetMethod(pSelectorName);
            if (methodInfo != null)
            {
#if NETFX_CORE
                return (SEL_MenuHandler)methodInfo.CreateDelegate(typeof(SEL_MenuHandler), target);
#else
                return (SEL_MenuHandler) Delegate.CreateDelegate(typeof(SEL_MenuHandler), this, methodInfo);
#endif
            }
            return null;
        }

        public SEL_CCControlHandler OnResolveCCBCCControlSelector(object target, string pSelectorName)
        {
            MethodInfo methodInfo = GetType().GetMethod(pSelectorName);
            if (methodInfo != null)
            {
#if NETFX_CORE
                return (SEL_CCControlHandler) methodInfo.CreateDelegate(typeof(SEL_CCControlHandler), target);
#else
                return (SEL_CCControlHandler) Delegate.CreateDelegate(typeof(SEL_CCControlHandler), this, methodInfo);
#endif
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