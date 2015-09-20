using System;

namespace CocosSharp
{
    public abstract class CCVisitableNode
    {
        protected bool transformIsDirty;
        protected CCAffineTransform affineLocalTransform;

        #region Properties

        public virtual bool Visible { get; set; }

        protected bool IsReorderChildDirty { get; set; }

        public int ChildrenCount
        {
            get { return Children == null ? 0 : Children.Count; }
        }

        public CCRawList<CCVisitableNode> Children { get; protected set; }

        #endregion Properties


        #region Constructors

        public CCVisitableNode()
        {
        }

        #endregion Constructors

        public abstract void Update(float dt);

        protected abstract void UpdateTransform();

        protected abstract void ViewportChanged();


        public void Visit()
        {
            var identity = CCAffineTransform.Identity;
            Visit(ref identity);
        }

        public virtual void Visit(ref CCAffineTransform parentWorldTransform)
        {
            if(!Visible)
                return;

            if(transformIsDirty)
                UpdateTransform();


            var worldTransform = CCAffineTransform.Identity;
            CCAffineTransform.Concat(ref affineLocalTransform, ref parentWorldTransform, out worldTransform);

            SortAllChildren();

            VisitRenderer(ref worldTransform);

            if(Children != null)
            {
                var elements = Children.Elements;
                for(int i = 0, N = Children.Count; i < N; ++i)
                {
                    var child = elements[i];
                    if (child.Visible)
                        child.Visit(ref worldTransform);
                }
            }
        }

        protected virtual void VisitRenderer(ref CCAffineTransform worldTransform)
        {
        }


        public void SortAllChildren()
        {
            if (IsReorderChildDirty)
            {
                Array.Sort(Children.Elements, 0, Children.Count, this);
                IsReorderChildDirty = false;
            }
        }



        #region Entering and exiting

        public virtual void OnEnter()
        {

            if (Children != null && Children.Count > 0)
            {
                CCNode[] elements = Children.Elements;
                for (int i = 0, count = Children.Count; i < count; i++)
                {
                    elements[i].OnEnter();
                }
            }

            Resume();

            IsRunning = true;

        }

        public virtual void OnEnterTransitionDidFinish()
        {
            if (Children != null && Children.Count > 0)
            {
                CCNode[] elements = Children.Elements;
                for (int i = 0, count = Children.Count; i < count; i++)
                {
                    elements[i].OnEnterTransitionDidFinish();
                }
            }
        }

        public virtual void OnExitTransitionDidStart()
        {
            if (Children != null && Children.Count > 0)
            {
                CCNode[] elements = Children.Elements;
                for (int i = 0, count = Children.Count; i < count; i++)
                {
                    elements[i].OnExitTransitionDidStart();
                }
            }
        }

        public virtual void OnExit()
        {
            Pause();

            IsRunning = false;

            if (Children != null && Children.Count > 0)
            {
                CCNode[] elements = Children.Elements;
                for (int i = 0, count = Children.Count; i < count; i++)
                {
                    elements[i].OnExit();
                }
            }
        }

        #endregion Entering and exiting

    }
}

