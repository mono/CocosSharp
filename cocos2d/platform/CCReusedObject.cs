using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Cocos2D
{
    public abstract class CCReusedObject<T> where T : CCReusedObject<T>, new()
    {
        private bool used;
        private static readonly List<CCReusedObject<T>> _unused = new List<CCReusedObject<T>>();  
        
        protected CCReusedObject()
        {
            used = true;
        }

        public static T Create()
        {
            var count = _unused.Count;
            if (count > 0)
            {
                var result = _unused[count - 1];
                result.used = true;
                _unused.RemoveAt(count - 1);
                return (T)result;
            }
            return new T();
        }

        protected abstract void PrepareForReuse();

        public void Free()
        {
            Debug.Assert(!used, "Already free");
            
            used = false;
            
            PrepareForReuse();
            
            _unused.Add(this);
        }
    }
}
