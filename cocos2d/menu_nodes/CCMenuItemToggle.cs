using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace CocosSharp
{
    public class CCMenuItemToggle : CCMenuItem
    {
        CCMenuItem previouslySelectedItem;
        int selectedIndex;


        #region Properties

        public List<CCMenuItem> SubItems { get; set; }

        public CCMenuItem SelectedItem 
        {
            get { return SubItems[SelectedIndex]; }
        }

        public int SelectedIndex
        {
            get { return selectedIndex; }
            set
            {
                if (value != selectedIndex && SubItems.Count > 0)
                {
                    selectedIndex = value;
                    if (previouslySelectedItem != null)
                    {
                        previouslySelectedItem.RemoveFromParentAndCleanup(false);
                    }

                    CCMenuItem selectedItem = SubItems[selectedIndex];
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
                if (SubItems != null) 
                {
                    foreach (CCMenuItem item in SubItems) {
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
                SubItems[selectedIndex].Selected = value;
            }
        }

        #endregion Properties


        #region Constructors

        public CCMenuItemToggle(Action<object> target, params CCMenuItem[] items)
            : base(target)
        {
            Debug.Assert(items != null && items.Length > 0, "List of toggle items must be non-empty");

            SubItems = new List<CCMenuItem>(items);

            selectedIndex = int.MaxValue;

            // Set the property to 0 to ensure the first toggle item is added as a node child
            SelectedIndex = 0;

            CascadeColorEnabled = true;
            CascadeOpacityEnabled = true;
        }

        public CCMenuItemToggle(params CCMenuItem[] items)
            : this(null, items)
        {
        }

        #endregion Constructors

                    
        public override void Activate()
        {
            // update index
            if (Enabled)
            {
                int newIndex = (SelectedIndex + 1) % SubItems.Count;
                SelectedIndex = newIndex;
            }
            base.Activate();
        }
    }
}