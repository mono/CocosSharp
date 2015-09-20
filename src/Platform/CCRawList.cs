using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace CocosSharp
{
    // No-frills list that wraps an accessible array.
    public class CCRawList<T> : IList<T>
    {
        int count;


		#region Properties

		// Direct access to the elements owned by the raw list.
		// Be careful about the operations performed on this list;
		// use the normal access methods if in doubt.
		public T[] Elements { get; private set; }

		public bool UseArrayPool { get; set; }

		bool ICollection<T>.IsReadOnly
		{
			get { return false; }
		}

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

		// Gets or sets the element of the list at the given index.
		public T this[int index]
		{
			get { return Elements[index]; }
			set { Elements[index] = value; }
		}

		#endregion Properties


		#region Consturctors

        public CCRawList(bool useArrayPool=false)
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

        public CCRawList(int initialCapacity, bool useArrayPool=false)
        {
            UseArrayPool = useArrayPool;

            if (initialCapacity <= 0)
                throw new ArgumentException("Initial capacity must be positive.");

            Capacity = initialCapacity;
        }

		public CCRawList(IList<T> elements, bool useArrayPool=false)
            : this(Math.Max(elements.Count, 4), useArrayPool)
        {
            elements.CopyTo(Elements, 0);
            count = elements.Count;
        }

		#endregion Constructors


		public void CopyTo(T[] array, int arrayIndex)
		{
			Array.Copy(Elements, 0, array, arrayIndex, count);
		}

		IEnumerator<T> IEnumerable<T>.GetEnumerator()
		{
			return new Enumerator(this);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return new Enumerator(this);
		}

		public Enumerator GetEnumerator()
		{
			return new Enumerator(this);
		}

		public T[] ToArray()
		{
			var toReturn = new T[count];
			Array.Copy(Elements, toReturn, count);
			return toReturn;
		}

		public void Sort(IComparer<T> comparer)
		{
			Array.Sort(Elements, 0, count, comparer);
		}

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


		#region Fetching

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

		// Determines the index of a specific item
		public int IndexOf(T item)
		{
			return Array.IndexOf(Elements, item, 0, count);
		}

		public bool Contains(T item)
		{
			return IndexOf(item) != -1;
		}

		#endregion Fetching


		#region Adding and inserting 

		public void Add(T item)
		{
			Add(ref item);
		}

		public void Add(ref T item)
		{
			if (count == Elements.Length)
			{
				Capacity = Elements.Length * 2;
			}
			Elements[count++] = item;
		}

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

		// Inserts the element at the specified index without maintaining list order.
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

		// Adds a range of elements to the list from another list.
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

		#endregion Adding and inserting


		#region Removing

		public void Clear()
		{
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

		public void RemoveAt(int index)
		{
			RemoveAt(index, 1);
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

		// Removes the first occurrence of a specific object
		public bool Remove(T item)
		{
			int index = IndexOf(item);
			if (index == -1)
				return false;
			RemoveAt(index);
			return true;
		}

		// Removes an element from the list without maintaining order.
		public void FastRemove(int index)
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

		// Removes the first occurrence of a specific object from the collection without maintaining element order.
		public bool FastRemove(T item)
		{
			int index = IndexOf(item);
			if (index == -1) {
				return false;
			}
			FastRemove(index);
			return true;
		}

		#endregion Removing



		#region Raw list enumerator

		// Enumerator for the RawList.
        public struct Enumerator : IEnumerator<T>
        {
            readonly CCRawList<T> list;
            int index;

            ///<summary>
            /// Constructs a new enumerator.
            ///</summary>
            ///<param name="list"></param>
            public Enumerator(CCRawList<T> list)
            {
				index = -1;
				this.list = list;
            }

            #region IEnumerator<T> Members

            public T Current
            {
				get { return list.Elements[index]; }
            }

            public void Dispose()
            {
            }

            object IEnumerator.Current
            {
				get { return list.Elements[index]; }
            }

            public bool MoveNext()
            {
				return ++index < list.count;
            }

            public void Reset()
            {
				index = -1;
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

		#endregion Raw list enumerator
    }
}
