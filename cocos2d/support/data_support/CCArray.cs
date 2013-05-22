using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Cocos2D
{
    public class CCArray
    {
        public int num, max;
        public int[] arr; //equals object** arr;

        /// <summary>
        /// Allocates and initializes a new C array with specified capacity
        /// </summary>
        public CCArray (int capacity)
        {
            if (capacity == 0)
            {
                capacity = 1;
            }

            num = 0;
            arr = new int[capacity];
            max = capacity;
        }

        /// <summary>
        /// Inserts a value at a certain position. The valid index is [0, num] 
        /// </summary>
        public static void InsertValueAtIndex(CCArray arr, int value, int index)
        {
            Debug.Assert(index < arr.max, "ccCArrayInsertValueAtIndex: invalid index");
            int remaining = arr.num - index;

            // make sure it has enough capacity
            if (arr.num + 1 == arr.max)
            {
                DoubleCapacity(arr);
            }

            //int[] temp = new int[arr.arr.Length];
            // last Value doesn't need to be moved
            if (remaining > 0)
            {
                // tex coordinates
                //Array.Copy(arr.arr, temp, arr.arr.Length);
                //Array.Copy(arr.arr, index, temp, index + 1, remaining);


                for (int i = arr.arr.Length - 1; i > index; i--)
                {
                    arr.arr[i] = arr.arr[i - 1];
                }
            }

            arr.num++;
            //temp[index] = value;

            arr.arr[index] = value;// temp;
        }

        /// <summary>
        /// Doubles C array capacity
        /// </summary>
        public static void DoubleCapacity(CCArray arr)
        {
            arr.max *= 2;
            int[] newArr = new int[arr.max];
            Array.Copy(arr.arr, newArr, arr.arr.Length);

            // will fail when there's not enough memory
            Debug.Assert(newArr != null, "ccArrayDoubleCapacity failed. Not enough memory");
            arr.arr = newArr;
        }

        /// <summary>
        /// Removes value at specified index and pushes back all subsequent values.
        /// Behaviour undefined if index outside [0, num-1].
        /// </summary>
        public static void RemoveValueAtIndex(CCArray arr, int index)
        {
            for (int last = --arr.num; index < last; index++)
            {
                arr.arr[index] = arr.arr[index + 1];
            }
        }
    }
}
