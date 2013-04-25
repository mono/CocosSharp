using System.Diagnostics;

namespace cocos2d
{
    internal class ccColor3BWapper 
    {
        private CCColor3B color;

        public static ccColor3BWapper Create(CCColor3B color)
        {
            var ret = new ccColor3BWapper();
            ret.color.R = color.R;
            ret.color.G = color.G;
            ret.color.B = color.B;
            return ret;
        }

        public CCColor3B getColor()
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

        public CCBValue (int nValue)
        {
            nValue = nValue;
            mType = ValueType.kIntValue;
        }

        public CCBValue (bool bValue)
        {
            nValue = bValue ? 1 : 0;
            mType = ValueType.kBoolValue;
        }

        public CCBValue (float fValue)
        {
            fValue = fValue;
            mType = ValueType.kFloatValue;
        }

        public CCBValue (byte bValue)
        {
            nValue = bValue;
            mType = ValueType.kUnsignedCharValue;
        }

        public CCBValue (byte[] pointer)
        {
            pointer = pointer;
            mType = ValueType.kPointerValue;
        }

        public int GetIntValue()
        {
            Debug.Assert(mType == ValueType.kIntValue);
            return nValue;
        }

        public float GetFloatValue()
        {
            Debug.Assert(mType == ValueType.kFloatValue);
            return fValue;
        }

        public bool GetBoolValue()
        {
            Debug.Assert(mType == ValueType.kBoolValue);
            return nValue == 1;
        }

        public byte GetByteValue()
        {
            Debug.Assert(mType == ValueType.kUnsignedCharValue);
            return (byte) nValue;
        }

        public byte[] getPointer()
        {
            Debug.Assert(mType == ValueType.kPointerValue);
            return pointer;
        }
    }
}