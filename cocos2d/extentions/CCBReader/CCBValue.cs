using System.Collections.Generic;
using System.Diagnostics;

namespace Cocos2D
{
    public class CCColor3BWapper 
    {
        private CCColor3B color;

        public CCColor3BWapper(CCColor3B xcolor)
        {
            color = xcolor;
        }

        public CCColor3B Color
        {
            get { return color; }
        }
    }

    public enum ValueType
    {
        Int,
        Float,
        Bool,
        UnsignedChar,
        String,
        Array
    }

    public class CCBValue 
    {
        private float floatValue;
        private ValueType _type;
        private int intValue;
        private string _strValue;
        private object _arrValue;  

        public CCBValue (int nValue)
        {
            intValue = nValue;
            _type = ValueType.Int;
        }

        public CCBValue(bool bValue)
        {
            intValue = bValue ? 1 : 0;
            _type = ValueType.Bool;
        }

        public CCBValue(float fValue)
        {
            floatValue = fValue;
            _type = ValueType.Float;
        }

        public CCBValue(byte bValue)
        {
            intValue = bValue;
            _type = ValueType.UnsignedChar;
        }

        public CCBValue(string pStr)
        {
            _strValue = pStr;
            _type = ValueType.String;
        }

        public CCBValue(object pArr)
        {
            _arrValue = pArr;
            _type = ValueType.Array;
        }

        public int GetIntValue()
        {
            Debug.Assert(_type == ValueType.Int, "The type of CCBValue isn't integer.");
            return intValue;
        }

        public float GetFloatValue()
        {
            Debug.Assert(_type == ValueType.Float, "The type of CCBValue isn't float.");
            return floatValue;
        }

        public bool GetBoolValue()
        {
            Debug.Assert(_type == ValueType.Bool, "The type of CCBValue isn't boolean.");
            return intValue == 1;
        }

        public byte GetByteValue()
        {
            Debug.Assert(_type == ValueType.UnsignedChar, "The type of CCBValue isn't unsigned char.");
            return (byte) intValue;
        }

        public string GetStringValue()
        {
            Debug.Assert(_type == ValueType.String, "The type of CCBValue isn't string.");
            return _strValue;
        }

        public object GetArrayValue()
        {
            Debug.Assert(_type == ValueType.Array, "The type of CCBValue isn't array.");
            return _arrValue;
        }

        public ValueType Type
        {
            get { return _type; }
        }
    }
}