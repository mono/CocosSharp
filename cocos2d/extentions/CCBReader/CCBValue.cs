using System.Diagnostics;

namespace Cocos2D
{
    internal class ccColor3BWapper 
    {
        private CCColor3B color;

        internal ccColor3BWapper(CCColor3B xcolor)
        {
            color = xcolor;
        }

        internal CCColor3B getColor()
        {
            return color;
        }
    };

    internal enum ValueType
    {
        kIntValue,
        kFloatValue,
        kPointerValue,
        kBoolValue,
        kUnsignedCharValue,
    }

    internal class CCBValue 
    {
        private float fValue;
        private ValueType mType;
        private int nValue;
        private byte[] pointer;

        internal CCBValue (int xnValue)
        {
            nValue = xnValue;
            mType = ValueType.kIntValue;
        }

        internal CCBValue(bool bValue)
        {
            nValue = bValue ? 1 : 0;
            mType = ValueType.kBoolValue;
        }

        internal CCBValue(float xfValue)
        {
            fValue = xfValue;
            mType = ValueType.kFloatValue;
        }

        internal CCBValue(byte bValue)
        {
            nValue = bValue;
            mType = ValueType.kUnsignedCharValue;
        }

        internal CCBValue(byte[] xpointer)
        {
            pointer = xpointer;
            mType = ValueType.kPointerValue;
        }

        internal int GetIntValue()
        {
            Debug.Assert(mType == ValueType.kIntValue);
            return nValue;
        }

        internal float GetFloatValue()
        {
            Debug.Assert(mType == ValueType.kFloatValue);
            return fValue;
        }

        internal bool GetBoolValue()
        {
            Debug.Assert(mType == ValueType.kBoolValue);
            return nValue == 1;
        }

        internal byte GetByteValue()
        {
            Debug.Assert(mType == ValueType.kUnsignedCharValue);
            return (byte) nValue;
        }

        internal byte[] getPointer()
        {
            Debug.Assert(mType == ValueType.kPointerValue);
            return pointer;
        }
    }
}