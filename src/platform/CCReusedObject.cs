using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace CocosSharp
{
    public abstract class CCReusedObject<T> where T : CCReusedObject<T>, new()
    {
        static readonly List<CCReusedObject<T>> unused = new List<CCReusedObject<T>>();  
		bool used;


		#region Constructors

		public static T Create()
		{
			var count = unused.Count;
			if (count > 0)
			{
				var result = unused[count - 1];
				result.used = true;
				unused.RemoveAt(count - 1);
				return (T)result;
			}
			return new T();
		}

        protected CCReusedObject()
        {
            used = true;
        }

		#endregion Constructors


        protected abstract void PrepareForReuse();

        public void Free()
        {
            Debug.Assert(!used, "Already free");
            
            used = false;
            
            PrepareForReuse();
            
			unused.Add(this);
        }
    }
}
