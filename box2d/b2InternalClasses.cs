using System;
using System.Collections.Generic;

namespace Box2D
{
    internal static class b2ArrayPool<T>
    {
        private class PoolEntry
        {
            public T[][] Elements = new T[4][];
            public int Count;

            public void Add(T[] array)
            {
                if (Count == Elements.Length)
                {
                    var newElements = new T[Elements.Length * 2][];
                    Array.Copy(Elements, newElements, Elements.Length);
                    Elements = newElements;
                }

                Elements[Count++] = array;
            }
        }

        private static readonly Dictionary<int, PoolEntry> _unused = new Dictionary<int, PoolEntry>();

        public static T[] Create(int length, bool pow)
        {
            PoolEntry entry;

            if (pow)
            {
                var l = 2;
                while (l < length) l <<= 1;
                length = l;
            }

            if (_unused.TryGetValue(length, out entry))
            {
                if (entry.Count > 0)
                {
                    var result = entry.Elements[--entry.Count];
                    entry.Elements[entry.Count] = null;
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
            PoolEntry entry;

            if (!_unused.TryGetValue(array.Length, out entry))
            {
                entry = new PoolEntry();
                _unused.Add(array.Length, entry);
            }

            entry.Add(array);
        }
    }

    public abstract class b2ReusedObject<T> where T : b2ReusedObject<T>, new()
    {
        private int index;

        private static int _unuseIndex;
        private static int _count;
        private static T[] _created = new T[4];

        public static T Create()
        {
            if (_unuseIndex < _count)
            {
                return _created[_unuseIndex++];
            }

            var result = new T();

            result.index = _count;

            if (_count == _created.Length)
            {
                var tmp = _created;
                _created = new T[_created.Length * 2];
                Array.Copy(tmp, _created, _count);
            }

            _unuseIndex++;

            _created[_count++] = result;

            return result;
        }

        public static void FreeAll()
        {
            _unuseIndex = 0;
        }

        public void Free(bool skipAssert = false)
        {
            if (index < _unuseIndex - 1)
            {
                var tmp = _created[--_unuseIndex];

                tmp.index = index;
                _created[index] = tmp;

                index = _unuseIndex;
                _created[_unuseIndex] = (T)this;
            }
            else
            {
                _unuseIndex--;
            }
        }
    }

    internal class b2IntStack : b2ReusedObject<b2IntStack>
    {
        private int[] _array;
        private int _capacity;
        private int _count;

        public b2IntStack()
        {
            _capacity = 128;
            _array = b2ArrayPool<int>.Create(128, true);
        }

        public int Count
        {
            get { return _count; }
        }

        public void Push(int value)
        {
            if (_count >= _capacity - 1)
            {
                _capacity *= 2;
                var newArray = b2ArrayPool<int>.Create(_capacity, true);
                Array.Copy(_array, newArray, _array.Length);
                b2ArrayPool<int>.Free(_array);
                _array = newArray;
            }
            _array[_count++] = value;
        }

        public int Pop()
        {
            return _array[--_count];
        }

        public void Reset()
        {
            _count = 0;
        }
    }

}
