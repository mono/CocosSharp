using System;
using System.Diagnostics;

namespace CocosSharp
{
    public class CCArray
    {
		public int Num, Max;
		public int[] Arr;


		#region Constructors

        public CCArray(int capacity)
        {
            if (capacity == 0)
            {
                capacity = 1;
            }

			Num = 0;
            Arr = new int[capacity];
			Max = capacity;
        }

		#endregion Constructors


        public static void InsertValueAtIndex(CCArray Arr, int value, int index)
        {
            Debug.Assert(index < Arr.Max, "ccCArrayInsertValueAtIndex: invalid index");
            int remaining = Arr.Num - index;

            // make sure it has enough capacity
            if (Arr.Num + 1 == Arr.Max)
            {
                DoubleCapacity(Arr);
            }

            //int[] temp = new int[Arr.Arr.Length];
            // last Value doesn't need to be moved
            if (remaining > 0)
            {
                // tex coordinates
                //Array.Copy(Arr.Arr, temp, Arr.Arr.Length);
                //Array.Copy(Arr.Arr, index, temp, index + 1, remaining);


                for (int i = Arr.Arr.Length - 1; i > index; i--)
                {
                    Arr.Arr[i] = Arr.Arr[i - 1];
                }
            }

            Arr.Num++;
            //temp[index] = value;

            Arr.Arr[index] = value;// temp;
        }

        public static void DoubleCapacity(CCArray Arr)
        {
            Arr.Max *= 2;
            int[] newArr = new int[Arr.Max];
            Array.Copy(Arr.Arr, newArr, Arr.Arr.Length);

            // will fail when there's not enough memory
            Debug.Assert(newArr != null, "ccArrayDoubleCapacity failed. Not enough memory");
            Arr.Arr = newArr;
        }

        public static void RemoveValueAtIndex(CCArray Arr, int index)
        {
            for (int last = --Arr.Num; index < last; index++)
            {
                Arr.Arr[index] = Arr.Arr[index + 1];
            }
        }
    }
}
