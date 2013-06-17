using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace Cocos2D
{
    ///<summary>
    /// No-frills list that wraps an accessible array.
    ///</summary>
    ///<typeparam name="T">Type of elements contained by the list.</typeparam>
    public class CCRawList<T> : IList<T>
    {
        ///<summary>
        /// Direct access to the elements owned by the raw list.
        /// Be careful about the operations performed on this list;
        /// use the normal access methods if in doubt.
        ///</summary>
        public T[] Elements;

        public int count;

        public bool UseArrayPool;

        ///<summary>
        /// Constructs an empty list.
        ///</summary>
        public CCRawList(bool useArrayPool = false)
        {
            UseArrayPool = useArrayPool;
            
            if (useArrayPool)
            {
                Elements = ArrayPool<T>.Create(4);
            }
            else
            {
                Elements = new T[4];
            }

            Debug.Assert(Elements != null);
        }

        ///<summary>
        /// Constructs an empty list.
        ///</summary>
        ///<param name="initialCapacity">Initial capacity to allocate for the list.</param>
        ///<exception cref="ArgumentException">Thrown when the initial capacity is zero or negative.</exception>
        public CCRawList(int initialCapacity, bool useArrayPool = false)
        {
            UseArrayPool = useArrayPool;

            if (initialCapacity <= 0)
                throw new ArgumentException("Initial capacity must be positive.");

            Capacity = initialCapacity;
        }

        ///<summary>
        /// Constructs a raw list from another list.
        ///</summary>
        ///<param name="elements">List to copy.</param>
        public CCRawList(IList<T> elements, bool useArrayPool = false)
            : this(Math.Max(elements.Count, 4), useArrayPool)
        {
            elements.CopyTo(Elements, 0);
            count = elements.Count;
        }

        ///<summary>
        /// Gets or sets the current size allocated for the list.
        ///</summary>
        public int Capacity
        {
            get { return Elements.Length; }
            set
            {
                T[] newArray;
                
                if (UseArrayPool)
                {
                    var capacity = 4;
                    while (capacity < value)
                    {
                        capacity *= 2;
                    }
                    newArray = ArrayPool<T>.Create(capacity);
                }
                else
                {
                    newArray = new T[value];
                }

                if (Elements != null && count > 0)
                {
                    Array.Copy(Elements, newArray, count);
                }

                if (UseArrayPool && Elements != null)
                {
                    ArrayPool<T>.Free(Elements);
                }

                Elements = newArray;

                Debug.Assert(Elements != null);
            }
        }

        #region IList<T> Members

        /// <summary>
        /// Gets the number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <returns>
        /// The number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </returns>
        public int Count
        {
            get { return count; }
            set
            {
                if (Elements.Length < value)
                {
                    Capacity = value;
                }
                count = value;
            }
        }

        /// <summary>
        /// Removes an element from the list.
        /// </summary>
        /// <param name="index">Index of the element to remove.</param>
        public void RemoveAt(int index)
        {
            if (index >= count)
            {
                throw new ArgumentOutOfRangeException("index");
            }
            count--;
            if (index < count)
            {
                Array.Copy(Elements, index + 1, Elements, index, count - index);
            }

            Elements[count] = default(T);
        }

        public void RemoveAt(int index, int amount)
        {
            if (index + amount > count)
            {
                throw new ArgumentOutOfRangeException("index");
            }
            count -= amount;
            if (index < count)
            {
                Array.Copy(Elements, index + amount, Elements, index, count - index);
            }

            amount--;
            while (amount >= 0)
            {
                Elements[count + amount] = default(T);
                amount--;
            }
        }

        /// <summary>
        /// Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <param name="item">The object to add to the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param><exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.</exception>
        public void Add(T item)
        {
            if (count == Elements.Length)
            {
                Capacity = Elements.Length * 2;
            }
            Elements[count++] = item;
        }

        public void Add(ref T item)
        {
            if (count == Elements.Length)
            {
                Capacity = Elements.Length * 2;
            }
            Elements[count++] = item;
        }

        /// <summary>
        /// Removes all items from the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only. </exception>
        public void Clear()
        {
            //Array.Clear(Elements, 0, count);
            count = 0;
        }

        public void Free()
        {
            if (Elements != null && UseArrayPool)
            {
                ArrayPool<T>.Free(Elements);
                Elements = null;
            }
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <returns>
        /// true if <paramref name="item"/> was successfully removed from the <see cref="T:System.Collections.Generic.ICollection`1"/>; otherwise, false. This method also returns false if <paramref name="item"/> is not found in the original <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </returns>
        /// <param name="item">The object to remove from the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param><exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.</exception>
        public bool Remove(T item)
        {
            int index = IndexOf(item);
            if (index == -1)
                return false;
            RemoveAt(index);
            return true;
        }

        /// <summary>
        /// Determines the index of a specific item in the <see cref="T:System.Collections.Generic.IList`1"/>.
        /// </summary>
        /// <returns>
        /// The index of <paramref name="item"/> if found in the list; otherwise, -1.
        /// </returns>
        /// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.IList`1"/>.</param>
        public int IndexOf(T item)
        {
            return Array.IndexOf(Elements, item, 0, count);
        }

        /// <summary>
        /// Inserts the element at the specified index.
        /// </summary>
        /// <param name="index">Index to insert the item.</param>
        /// <param name="item">Element to insert.</param>
        public void Insert(int index, T item)
        {
            if (index < count)
            {
                if (count == Elements.Length)
                {
                    Capacity = Elements.Length * 2;
                }

                Array.Copy(Elements, index, Elements, index + 1, count - index);
                Elements[index] = item;
                count++;
            }
            else
                Add(item);
        }

        /// <summary>
        /// Gets or sets the element of the list at the given index.
        /// </summary>
        /// <param name="index">Index in the list.</param>
        /// <returns>Element at the given index.</returns>
        public T this[int index]
        {
            get
            {
                //if (index < count && index >= 0)
                    return Elements[index];
                //throw new IndexOutOfRangeException("Index is outside of the list's bounds.");
            }
            set
            {
                //if (index < count && index >= 0)
                    Elements[index] = value;
                //else
                //    throw new IndexOutOfRangeException("Index is outside of the list's bounds.");
            }
        }

        /// <summary>
        /// Determines if an item is present in the list.
        /// </summary>
        /// <param name="item">Item to be tested.</param>
        /// <returns>Whether or not the item was contained by the list.</returns>
        public bool Contains(T item)
        {
            return IndexOf(item) != -1;
        }

        /// <summary>
        /// Copies the list's contents to the array.
        /// </summary>
        /// <param name="array">Array to receive the list's contents.</param>
        /// <param name="arrayIndex">Index in the array to start the dump.</param>
        public void CopyTo(T[] array, int arrayIndex)
        {
            Array.Copy(Elements, 0, array, arrayIndex, count);
        }

        bool ICollection<T>.IsReadOnly
        {
            get { return false; }
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return new Enumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new Enumerator(this);
        }

        #endregion

        /// <summary>
        /// Removes an element from the list without maintaining order.
        /// </summary>
        /// <param name="index">Index of the element to remove.</param>
        public void FastRemoveAt(int index)
        {
            if (index >= count)
            {
                throw new ArgumentOutOfRangeException("index");
            }
            count--;
            if (index < count)
            {
                Elements[index] = Elements[count];
            }
            Elements[count] = default(T);
        }

        ///<summary>
        /// Adds a range of elements to the list from another list.
        ///</summary>
        ///<param name="items">Elements to add.</param>
        public void AddRange(CCRawList<T> items)
        {
            int neededLength = count + items.count;
            if (neededLength > Elements.Length)
            {
                int newLength = Elements.Length * 2;
                if (newLength < neededLength)
                    newLength = neededLength;
                Capacity = newLength;
            }
            Array.Copy(items.Elements, 0, Elements, count, items.count);
            count = neededLength;
        }

        public void AddRange(CCRawList<T> items, int offset, int c)
        {
            int neededLength = count + c;
            if (neededLength > Elements.Length)
            {
                int newLength = Elements.Length * 2;
                if (newLength < neededLength)
                    newLength = neededLength;
                Capacity = newLength;
            }
            Array.Copy(items.Elements, offset, Elements, count, c);
            count = neededLength;
        }

        ///<summary>
        /// Adds a range of elements to the list from another list.
        ///</summary>
        ///<param name="items">Elements to add.</param>
        public void AddRange(List<T> items)
        {
            int neededLength = count + items.Count;
            if (neededLength > Elements.Length)
            {
                int newLength = Elements.Length * 2;
                if (newLength < neededLength)
                    newLength = neededLength;
                Capacity = newLength;
            }
            items.CopyTo(0, Elements, count, items.Count);
            count = neededLength;
        }

        ///<summary>
        /// Adds a range of elements to the list from another list.
        ///</summary>
        ///<param name="items">Elements to add.</param>
        public void AddRange(IList<T> items)
        {
            int neededLength = count + items.Count;
            if (neededLength > Elements.Length)
            {
                int newLength = Elements.Length * 2;
                if (newLength < neededLength)
                    newLength = neededLength;
                Capacity = newLength;
            }
            items.CopyTo(Elements, 0);
            count = neededLength;
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the collection without maintaining element order.
        /// </summary>
        /// <returns>
        /// true if <paramref name="item"/> was successfully removed from the <see cref="T:System.Collections.Generic.ICollection`1"/>; otherwise, false. This method also returns false if <paramref name="item"/> is not found in the original <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </returns>
        /// <param name="item">The object to remove from the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param><exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.</exception>
        public bool FastRemove(T item)
        {
            int index = IndexOf(item);
            if (index == -1)
                return false;
            FastRemoveAt(index);
            return true;
        }

        /// <summary>
        /// Copies the elements from the list into an array.
        /// </summary>
        /// <returns>An array containing the elements in the list.</returns>
        public T[] ToArray()
        {
            var toReturn = new T[count];
            Array.Copy(Elements, toReturn, count);
            return toReturn;
        }

        /// <summary>
        /// Inserts the element at the specified index without maintaining list order.
        /// </summary>
        /// <param name="index">Index to insert the item.</param>
        /// <param name="item">Element to insert.</param>
        public void FastInsert(int index, T item)
        {
            if (index < count)
            {
                if (count == Elements.Length)
                {
                    Capacity = Elements.Length * 2;
                }

                //Array.Copy(Elements, index, Elements, index + 1, count - index);
                Elements[count] = Elements[index];
                Elements[index] = item;
                count++;
            }
            else
                Add(item);
        }

        ///<summary>
        /// Gets an enumerator for the list.
        ///</summary>
        ///<returns>Enumerator for the list.</returns>
        public Enumerator GetEnumerator()
        {
            return new Enumerator(this);
        }

        ///<summary>
        /// Sorts the list.
        ///</summary>
        ///<param name="comparer">Comparer to use to sort the list.</param>
        public void Sort(IComparer<T> comparer)
        {
            Array.Sort(Elements, 0, count, comparer);
        }

        public void Reverse()
        {
            Array.Reverse(Elements, 0, count);
        }

        public T First()
        {
            return Elements[0];
        }

        public T Last()
        {
            return Elements[count - 1];
        }

        public T Pop()
        {
            return Elements[--count];
        }

        public T Peek()
        {
            return Elements[count - 1];
        }

        public void Push(T item)
        {
            if (count == Elements.Length)
            {
                Capacity = Elements.Length * 2;
            }
            Elements[count++] = item;
        }
        
        #region Nested type: Enumerator

        ///<summary>
        /// Enumerator for the RawList.
        ///</summary>
        public struct Enumerator : IEnumerator<T>
        {
            private readonly CCRawList<T> _list;
            private int _index;

            ///<summary>
            /// Constructs a new enumerator.
            ///</summary>
            ///<param name="list"></param>
            public Enumerator(CCRawList<T> list)
            {
                _index = -1;
                _list = list;
            }

            #region IEnumerator<T> Members

            public T Current
            {
                get { return _list.Elements[_index]; }
            }

            public void Dispose()
            {
            }

            object IEnumerator.Current
            {
                get { return _list.Elements[_index]; }
            }

            public bool MoveNext()
            {
                return ++_index < _list.count;
            }

            public void Reset()
            {
                _index = -1;
            }

            #endregion
        }

        public void InsertRange(int index, CCRawList<T> c)
        {
            if (c == null)
            {
                throw new ArgumentNullException("c");
            }
            if (index < 0 || index > this.count)
            {
                throw new ArgumentOutOfRangeException("index");
            }
            int cnt = c.count;
            if (cnt > 0)
            {
                if (this.count + cnt > Elements.Length)
                    Capacity = this.count + cnt;
                if (index < this.count)
                {
                    Array.Copy(Elements, index, Elements, index + cnt, this.count - index);
                }
                T[] array = new T[cnt];
                c.CopyTo(array, 0);
                array.CopyTo(this.Elements, index);
                this.count += cnt;
            }
        }

        public virtual void RemoveRange(int index, int count)
        {
            if (index < 0)
            {
                throw new ArgumentOutOfRangeException("index");
            }
            if (count < 0)
            {
                throw new ArgumentOutOfRangeException("count");
            }
            if (this.count - index < count)
            {
                throw new ArgumentException("index");
            }
            if (count > 0)
            {
                int i = this.count;
                this.count -= count;
                if (index < this.count)
                {
                    Array.Copy(this.Elements, index + count, this.Elements, index, this.count - index);
                }
                while (i > this.count)
                {
                    this.Elements[--i] = default(T);
                }
            }
        }

        #endregion

        public void PackToCount()
        {
            if (Elements != null && count < Elements.Length)
            {
                var newArray = new T[count];
                Array.Copy(Elements, newArray, count);
                if (UseArrayPool)
                {
                    ArrayPool<T>.Free(Elements);
                }
                Elements = newArray;
            }
        }

        public void IncreaseCount(int size)
        {
            var newCount = count + size;
            if (Elements.Length < newCount)
            {
                Capacity = newCount;
            }
            count = newCount;
        }
    }
}
