using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CocosSharp
{
    internal class CCEventListenerVector
    {

		List<CCEventListener> sceneGraphListeners;
		List<CCEventListener> fixedListeners;
		public int Gt0Index { get; set; }

        public int Size
        {
            get
            {
				int size = 0;
				if (sceneGraphListeners != null)
					size += sceneGraphListeners.Count;
				if (fixedListeners != null)
					size += fixedListeners.Count;

				return size;
            }
        }

        public bool IsEmpty
        {
            get
            {
				return (sceneGraphListeners == null || sceneGraphListeners.Count == 0) 
					&& (fixedListeners == null || fixedListeners.Count == 0);
            }
        }

        public CCEventListenerVector()
        {
			Gt0Index = 0;
        }

        public void PushBack(CCEventListener listener)
        {
			if (listener.FixedPriority == 0)
            {
				if (sceneGraphListeners == null) 
				{
					sceneGraphListeners = new List<CCEventListener> (100);
				}

                sceneGraphListeners.Add(listener);
            }
            else
            {
				if (fixedListeners == null) 
				{
					fixedListeners = new List<CCEventListener> (100);
				}


                fixedListeners.Add(listener);
            }
        }

        public void ClearSceneGraphListeners()
        {
			if (sceneGraphListeners != null) 
			{
				sceneGraphListeners.Clear ();
				sceneGraphListeners = null;
			}
        }

        public void ClearFixedListeners()
        {
			if (fixedListeners != null) 
			{
				fixedListeners.Clear ();
				fixedListeners = null;
			}
        }

        public void Clear()
        {
            ClearSceneGraphListeners();
            ClearFixedListeners();
        }

        public List<CCEventListener> FixedPriorityListeners
        {
            get { return fixedListeners; }
        }

        public List<CCEventListener> SceneGraphPriorityListeners
        {
            get { return sceneGraphListeners; }
        }

    }
}
