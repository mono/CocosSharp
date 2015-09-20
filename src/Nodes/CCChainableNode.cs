using System;

namespace CocosSharp
{
    public abstract class CCChainableNode : CCVisitableNode
    {
        byte displayedOpacity;
        CCColor3B displayedColor;

        CCScene scene;

        protected byte RealOpacity { get; set; }
        protected CCColor3B RealColor { get; set; }

        public virtual CCColor3B Color
        {
            get { return RealColor; }
            set
            {
                displayedColor = RealColor = value;

                UpdateCascadeColor();
            }
        }


        public virtual byte Opacity
        {
            get { return RealOpacity; }
            set
            {
                displayedOpacity = RealOpacity = value;

                UpdateCascadeOpacity();
            }
        }

        public byte DisplayedOpacity 
        { 
            get { return displayedOpacity; }
            protected set 
            {
                displayedOpacity = value;
            }
        }


        public virtual CCScene Scene
        {
            get { return scene; }
            internal set 
            {
                if(scene != value) 
                {

                    scene = value;

                    // All the children should belong to same scene
                    if (Children != null) 
                    {
                        foreach (CCNode child in Children) 
                        {
                            child.Scene = scene;
                        }
                    }

                    if (scene != null) 
                    {
                        AddedToScene();

                        AttachActions();
                        AttachSchedules();
                    }

                    AttachEvents();
                }
            }
        }



        public CCChainableNode() 
        {
        }


        protected abstract void AddedToScene();

        protected abstract void VisibleBoundsChanged();



        protected internal void UpdateCascadeColor()
        {
            var parentColor = CCColor3B.White;
            if (Parent != null && Parent.IsColorCascaded)
            {
                parentColor = Parent.DisplayedColor;
            }

            UpdateDisplayedColor(parentColor);
        }

        protected internal void DisableCascadeColor()
        {
            if (Children == null)
                return;

            foreach (var child in Children)
            {
                child.UpdateDisplayedColor(CCColor3B.White);
            }
        }

        #region Color and Opacity

        protected internal virtual void UpdateDisplayedOpacity(byte parentOpacity)
        {
            displayedOpacity = (byte) (RealOpacity * parentOpacity / 255.0f);

            UpdateColor();

            if (IsOpacityCascaded && Children != null)
            {
                foreach(CCNode node in Children)
                {
                    node.UpdateDisplayedOpacity(DisplayedOpacity);
                }
            }
        }

        protected internal virtual void UpdateCascadeOpacity ()
        {
            byte parentOpacity = 255;
            var pParent = Parent;
            if (pParent != null && pParent.IsOpacityCascaded)
            {
                parentOpacity = pParent.DisplayedOpacity;
            }
            UpdateDisplayedOpacity(parentOpacity);

        }

        protected virtual void DisableCascadeOpacity()
        {
            DisplayedOpacity = RealOpacity;

            foreach(CCNode node in Children.Elements)
            {
                node.UpdateDisplayedOpacity(255);
            }
        }

        public virtual void UpdateColor()
        {
            // Override the opdate of color here
        }

        public virtual void UpdateDisplayedColor(CCColor3B parentColor)
        {
            displayedColor.R = (byte)(RealColor.R * parentColor.R / 255.0f);
            displayedColor.G = (byte)(RealColor.G * parentColor.G / 255.0f);
            displayedColor.B = (byte)(RealColor.B * parentColor.B / 255.0f);

            UpdateColor();

            if (IsColorCascaded)
            {
                if (IsOpacityCascaded && Children != null)
                {
                    foreach(CCNode node in Children)
                    {
                        if (node != null)
                        {
                            node.UpdateDisplayedColor(DisplayedColor);
                        }
                    }
                }
            }
        }

        #endregion Color and Opacity
    }
}

