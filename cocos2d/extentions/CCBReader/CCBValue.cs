using System.Diagnostics;

namespace Cocos2D
{
    internal class CCColor3BWapper 
    {
        private CCColor3B color;

        internal CCColor3BWapper(CCColor3B xcolor)
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
        Int,
        Float,
        Pointer,
        Bool,
        UnsignedChar,
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
            mType = ValueType.Int;
        }

        internal CCBValue(bool bValue)
        {
            nValue = bValue ? 1 : 0;
            mType = ValueType.Bool;
        }

        internal CCBValue(float xfValue)
        {
            fValue = xfValue;
            mType = ValueType.Float;
        }

        internal CCBValue(byte bValue)
        {
            nValue = bValue;
            mType = ValueType.UnsignedChar;
        }

        internal CCBValue(byte[] xpointer)
        {
            pointer = xpointer;
            mType = ValueType.Pointer;
        }

        internal int GetIntValue()
        {
            Debug.Assert(mType == ValueType.Int);
            return nValue;
        }

        internal float GetFloatValue()
        {
            Debug.Assert(mType == ValueType.Float);
            return fValue;
        }

        internal bool GetBoolValue()
        {
            Debug.Assert(mType == ValueType.Bool);
            return nValue == 1;
        }

        internal byte GetByteValue()
        {
            Debug.Assert(mType == ValueType.UnsignedChar);
            return (byte) nValue;
        }

        internal byte[] getPointer()
        {
            Debug.Assert(mType == ValueType.Pointer);
            return pointer;
        }
    }
}