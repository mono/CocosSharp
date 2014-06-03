using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace CocosSharp
{
    public class CCMenuItemToggle : CCMenuItem
    {
        CCMenuItem previouslySelectedItem;
        int selectedIndex;
        List<CCMenuItem> subItems;

        #region Properties

        public CCMenuItem SelectedItem 
        {
            get { return subItems[SelectedIndex]; }
        }

        public int SelectedIndex
        {
            get { return selectedIndex; }
            set
            {
                if (value != selectedIndex && subItems.Count > 0)
                {
                    selectedIndex = value;
                    if (previouslySelectedItem != null)
                    {
                        previouslySelectedItem.RemoveFromParent(false);
                    }

                    CCMenuItem selectedItem = subItems[selectedIndex];
                    AddChild(selectedItem, 0);

                    CCSize itemContentSize = selectedItem.ContentSize;
                    ContentSize = itemContentSize;
                    selectedItem.Position = new CCPoint(itemContentSize.Width / 2, itemContentSize.Height / 2);

                    previouslySelectedItem = selectedItem;
                }
            }
        }

        public override bool Enabled
        {
            get { return base.Enabled; }
            set
            {
                base.Enabled = value;
                if (subItems != null) 
                {
                    foreach (CCMenuItem item in subItems) {
                        item.Enabled = value;
                    }
                }
            }
        }

        public override bool Selected 
        {
            set
            {
                base.Selected = value;
                subItems[selectedIndex].Selected = value;
            }
        }

        internal override CCDirector Director 
        { 
            get { return base.Director; }
            set 
            {
                base.Director = value;

                if(value != null && subItems !=null)
                {
                    foreach(CCMenuItem item in subItems) 
                    {
                        item.Director = value;
                    }
                }
            }
        }

        #endregion Properties


        #region Constructors

        public CCMenuItemToggle(Action<object> target, params CCMenuItem[] items)
            : base(target)
        {
            Debug.Assert(items != null && items.Length > 0, "List of toggle items must be non-empty");

            AddToggleMenuItems(items);

            selectedIndex = int.MaxValue;

            // Set the property to 0 to ensure the first toggle item is added as a node child
            SelectedIndex = 0;

            IsColorCascaded = true;
            IsOpacityCascaded = true;
        }

        public CCMenuItemToggle(params CCMenuItem[] items)
            : this(null, items)
        {
        }

        #endregion Constructors


        #region Adding/removing subitems

        public void AddToggleMenuItems(params CCMenuItem[] items)
        {
            if (subItems == null)
                subItems = new List<CCMenuItem> ();

            foreach(CCMenuItem item in items) 
            {
                subItems.Add(item);
                if (Director != null)
                    item.Director = Director;
            }
        }

        public void RemoveToggleMenuItems(params CCMenuItem[] items)
        {
            foreach(CCMenuItem item in items) 
            {
                subItems.Remove(item);
            }
        }

        #endregion Adding/removing

                    
        public override void Activate()
        {
            // update index
            if (Enabled)
            {
                int newIndex = (SelectedIndex + 1) % subItems.Count;
                SelectedIndex = newIndex;
            }
            base.Activate();
        }
    }
}