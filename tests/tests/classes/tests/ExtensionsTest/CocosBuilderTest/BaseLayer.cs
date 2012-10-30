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

        public bool OnAssignCCBMemberVariable(CCObject target, string memberVariableName, CCNode node)
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

        public SEL_MenuHandler OnResolveCCBCCMenuItemSelector(CCObject target, string pSelectorName)
        {
            MethodInfo methodInfo = GetType().GetMethod(pSelectorName);
            if (methodInfo != null)
            {
                return (SEL_MenuHandler) Delegate.CreateDelegate(typeof(SEL_MenuHandler), this, methodInfo);
            }
            return null;
        }

        public SEL_CCControlHandler OnResolveCCBCCControlSelector(CCObject target, string pSelectorName)
        {
            MethodInfo methodInfo = GetType().GetMethod(pSelectorName);
            if (methodInfo != null)
            {
                return (SEL_CCControlHandler) Delegate.CreateDelegate(typeof(SEL_CCControlHandler), this, methodInfo);
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