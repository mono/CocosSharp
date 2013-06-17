using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Cocos2D
{
    public static class ArrayPool<T>
    {
        private static readonly Dictionary<int, CCRawList<T[]>> _unused = new Dictionary<int, CCRawList<T[]>>();

        public static T[] Create(int length, bool pow = true)
        {
            CCRawList<T[]> list;

            if (pow)
            {
                var l = 2;
                while (l < length) 
                {
                    l <<= 1;
                }
                length = l;
            }

            if (_unused.TryGetValue(length, out list))
            {
                if (list.Count > 0)
                {
                    var result = list.Elements[--list.count];
                    list.Elements[list.count] = null;
                    return result;
                }
            }

            return new T[length];
        }

        public static void Resize(ref T[] array, int length, bool pow = true)
        {
            Free(array);
            array = Create(length, pow);
        }

        public static void Free(T[] array)
        {
            CCRawList<T[]> list;

            if (!_unused.TryGetValue(array.Length, out list))
            {
                list = new CCRawList<T[]>(false);
                _unused.Add(array.Length, list);
            }

            list.Add(array);
        }
    }
}
