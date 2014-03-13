using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CocosSharp
{
    internal class CCEventListenerVector
    {

        List<CCEventListener> sceneGraphListeners = new List<CCEventListener>(100);
        List<CCEventListener> fixedListeners = new List<CCEventListener>(100);

        public int Size
        {
            get
            {
                return sceneGraphListeners.Count + fixedListeners.Count;
            }
        }

        public bool IsEmpty
        {
            get
            {
                return sceneGraphListeners.Count == 0 && fixedListeners.Count == 0;
            }
        }

        public CCEventListenerVector()
        {

        }

        public void PushBack(CCEventListener listener)
        {
            if (listener.FixePriority == 0)
            {
                sceneGraphListeners.Add(listener);
            }
            else
            {
                fixedListeners.Add(listener);
            }
        }

        public void ClearSceneGraphListeners()
        {
            sceneGraphListeners.Clear();
        }

        public void ClearFixedListeners()
        {
            fixedListeners.Clear();
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
