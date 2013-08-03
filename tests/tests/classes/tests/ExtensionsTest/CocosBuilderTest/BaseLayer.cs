using System;
using System.Reflection;
using Cocos2D;
using ValueType = Cocos2D.ValueType;

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
            if (string.IsNullOrEmpty(memberVariableName))
            {
                return false;
            }

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

        public bool OnAssignCCBCustomProperty(object pTarget, string pMemberVariableName, CCBValue pCCBValue)
        {
            bool bRet = false;

            if (pTarget == this)
            {
                FieldInfo fieldInfo = GetType().GetField(pMemberVariableName);

                if (fieldInfo == null)
                {
                    fieldInfo =
                        GetType()
                            .GetField(pMemberVariableName.Substring(0, 1).ToUpper() + pMemberVariableName.Substring(1));
                }

                if (fieldInfo != null)
                {
                    switch (pCCBValue.Type)
                    {
                        case ValueType.Int:
                            fieldInfo.SetValue(this, pCCBValue.GetIntValue());
                            break;
                        case ValueType.Float:
                            fieldInfo.SetValue(this, pCCBValue.GetFloatValue());
                            break;
                        case ValueType.Bool:
                            fieldInfo.SetValue(this, pCCBValue.GetBoolValue());
                            break;
                        case ValueType.UnsignedChar:
                            fieldInfo.SetValue(this, pCCBValue.GetByteValue());
                            break;
                        case ValueType.String:
                            fieldInfo.SetValue(this, pCCBValue.GetStringValue());
                            break;
                        case ValueType.Array:
                            fieldInfo.SetValue(this, pCCBValue.GetArrayValue());
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    return true;
                }
                else
                {
                    CCLog.Log("Oops, could not find field named: {0}", pMemberVariableName);
                }
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
				return (Action<object>)methodInfo.CreateDelegate(typeof(Action<object>), this);
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

        public Action<CCNode> OnResolveCCBCCCallFuncSelector(object pTarget, string pSelectorName)
        {
            MethodInfo methodInfo = GetType().GetMethod(pSelectorName);
            if (methodInfo == null)
            {
                methodInfo = GetType().GetMethod(pSelectorName.Substring(0, 1).ToUpper() + pSelectorName.Substring(1));
            }
            if (methodInfo != null)
            {
#if NETFX_CORE
				return (Action<CCNode>)methodInfo.CreateDelegate(typeof(Action<CCNode>), this);
#else
                return (Action<CCNode>)Delegate.CreateDelegate(typeof(Action<CCNode>), this, methodInfo);
#endif
            }
            else
            {
                CCLog.Log("Oops, could not find call selector named: {0}", pSelectorName);
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
            CCLog.Log("BaseLayer::onNodeLoaded");
        }

        #endregion
    }
}