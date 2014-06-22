using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;

namespace CocosSharp
{
    public enum CCMenuState
    {
        Waiting,
        TrackingTouch,
        Focused
    };

    /// <summary>
    /// A CCMenu
    /// Features and Limitation:
    ///  You can add MenuItem objects in runtime using addChild:
    ///  But the only accecpted children are MenuItem objects
    /// </summary>
    public class CCMenu : CCLayerRGBA
    {
        public const float DefaultPadding = 5;
        public const int DefaultMenuHandlerPriority = -128;

        List<CCMenuItem> menuItems = new List<CCMenuItem>();


        #region Properties

        public bool Enabled { get; set; }
        protected CCMenuState MenuState { get; set; }
        protected CCMenuItem SelectedMenuItem { get; set; }

        // Note that this only has a value if the GamePad or Keyboard is enabled.
        // Touch devices do not have a "focus" concept.
        public CCMenuItem FocusedItem
        {
            get 
            { 
                CCMenuItem focusedItem = null;

                foreach (CCMenuItem item in menuItems) 
                {
                    if (item.HasFocus) 
                    {
                        focusedItem = item;
                        break;
                    }
                }
                return focusedItem; 
            }
        }

        public override bool HasFocus
        {
            set
            {
                base.HasFocus = value;

                // Set the first menu item to have the focus
                if (FocusedItem == null && menuItems.Count > 0)
                {
                    menuItems[0].HasFocus = true;
                }
            }
        }

        #endregion Properties


        #region Constructors

        public CCMenu(params CCMenuItem[] items) : base()
        {
            Enabled = true;

            SelectedMenuItem = null;
            MenuState = CCMenuState.Waiting;

            IsColorCascaded = true;
            IsOpacityCascaded = true;

            AnchorPoint = new CCPoint(0.5f, 0.5f);
            IgnoreAnchorPointForPosition = true;

            if (items != null)
            {
                int z = 0;
                foreach (CCMenuItem item in items)
                {
                    AddChild(item, z);
                    z++;
                }
            }

        }

        #endregion Constructors


        #region Setup content

        protected override void RunningOnNewWindow(CCSize windowSize)
        {
            base.RunningOnNewWindow(windowSize);

            if (Director != null)
            {

                CCSize contentSize = windowSize;

				// If the position has already been set then we need to respect 
				// that position
				if (!IsTransformDirty)
					Position = contentSize.Center;

                ContentSize = contentSize;

                var touchListener = new CCEventListenerTouchOneByOne();
                touchListener.IsSwallowTouches = true;

                touchListener.OnTouchBegan = TouchBegan;
                touchListener.OnTouchMoved = TouchMoved;
                touchListener.OnTouchEnded = TouchEnded;
                touchListener.OnTouchCancelled = TouchCancelled;

                EventDispatcher.AddEventListener(touchListener, this);
            }
        }

        #endregion Setup content


        public void AddChild(CCMenuItem menuItem)
        {
            this.AddChild(menuItem, menuItem.ZOrder);
        }

        public void AddChild(CCMenuItem menuItem, int zOrder, int tag=0)
        {
            base.AddChild(menuItem, zOrder);

            menuItems.Add(menuItem);
        }

        public void RemoveChild(CCMenuItem menuItem, bool cleanup)
        {
            if (SelectedMenuItem == menuItem)
            {
                SelectedMenuItem = null;
            }

            base.RemoveChild(menuItem, cleanup);

            menuItems.Remove(menuItem);
        }

        public override void OnEnter()
        {
            base.OnEnter();

            CCFocusManager.Instance.Add(menuItems.ToArray());
        }

        public override void OnExit()
        {
            if (MenuState == CCMenuState.TrackingTouch)
            {
                if (SelectedMenuItem != null)
                {
                    SelectedMenuItem.Selected = false;
                    SelectedMenuItem = null;
                }
                MenuState = CCMenuState.Waiting;
            }

            CCFocusManager.Instance.Remove(menuItems.ToArray());

            base.OnExit();
        }


        #region Touch events

        protected virtual CCMenuItem ItemForTouch(CCTouch touch)
        {
            CCMenuItem touchedMenuItem = null;
            CCPoint touchLocation = touch.Location;

            if (menuItems != null && menuItems.Count > 0)
            {
                foreach (CCMenuItem menuItem in menuItems)
                {
                    if (menuItem != null && menuItem.Visible && menuItem.Enabled)
                    {
                        CCPoint local = menuItem.ConvertToNodeSpace(touchLocation);
                        CCRect r = menuItem.Rectangle;
                        r.Origin = CCPoint.Zero;

                        if (r.ContainsPoint(local))
                        {
                            touchedMenuItem = menuItem;
                            break;
                        }
                    }
                }
            }

            return touchedMenuItem;
        }


        bool TouchBegan(CCTouch touch, CCEvent touchEvent)
        {
            if (MenuState != CCMenuState.Waiting || !Visible || !Enabled)
            {
                return false;
            }

            for (CCNode c = Parent; c != null; c = c.Parent)
            {
                if (c.Visible == false)
                {
                    return false;
                }
            }

            SelectedMenuItem = ItemForTouch(touch);
            if (SelectedMenuItem != null)
            {
                MenuState = CCMenuState.TrackingTouch;
                SelectedMenuItem.Selected = true;
                return true;
            }
            return false;
        }

        void TouchEnded(CCTouch touch, CCEvent touchEvent)
        {
            Debug.Assert(MenuState == CCMenuState.TrackingTouch, "[Menu TouchEnded] -- invalid state");
            if (SelectedMenuItem != null)
            {
                SelectedMenuItem.Selected = false;
                SelectedMenuItem.Activate();
            }
            MenuState = CCMenuState.Waiting;
        }

        void TouchCancelled(CCTouch touch, CCEvent touchEvent)
        {
            Debug.Assert(MenuState == CCMenuState.TrackingTouch, "[Menu ccTouchCancelled] -- invalid state");
            if (SelectedMenuItem != null)
            {
                SelectedMenuItem.Selected = false;
            }
            MenuState = CCMenuState.Waiting;
        }

        void TouchMoved(CCTouch touch, CCEvent touchEvent)
        {
            Debug.Assert(MenuState == CCMenuState.TrackingTouch, "[Menu TouchMoved] -- invalid state");
            CCMenuItem currentItem = ItemForTouch(touch);

            if (currentItem != SelectedMenuItem)
            {
                if(SelectedMenuItem != null)
                {
                    SelectedMenuItem.Selected = false;
                }

                if(currentItem != null)
                {
                    currentItem.Selected = true;
                }

                SelectedMenuItem = currentItem;
            }

        }

        #endregion Touch events

        #region Alignment

        public void AlignItemsVertically(float padding = DefaultPadding)
        {
            float width = 0f;
            float height = -padding;

            if (menuItems != null && menuItems.Count > 0)
            {
                // First pass to get width and height
                foreach (CCMenuItem menuItem in menuItems)
                {
                    if (menuItem.Visible) 
                    {
                        height += menuItem.ContentSize.Height * menuItem.ScaleY + padding;
                        width = Math.Max(width, menuItem.ContentSize.Width);
                    }
                }

                float y = height / 2.0f;

                foreach (CCMenuItem menuItem in menuItems)
                {
                    if (menuItem.Visible)
                    {                    
                        menuItem.Position = new CCPoint(0, y - menuItem.ContentSize.Height * menuItem.ScaleY / 2.0f);
                        y -= menuItem.ContentSize.Height * menuItem.ScaleY + padding;
                        width = Math.Max(width, menuItem.ContentSize.Width);
                    }
                }
            }

            ContentSize = new CCSize(width, height);
        }

        public void AlignItemsHorizontally(float padding = DefaultPadding)
        {
            float height = 0f;
            float width = -padding;

            if (menuItems != null && menuItems.Count > 0)
            {
                // First pass to get width and height
                foreach (CCMenuItem menuItem in menuItems)
                {
                    if (menuItem.Visible)
                    {
                        width += menuItem.ContentSize.Width * menuItem.ScaleX + padding;
                        height = Math.Max(height, menuItem.ContentSize.Height);
                    }
                }

                float x = -width / 2.0f;

                foreach (CCMenuItem menuItem in menuItems)
                {
                    if (menuItem.Visible)
                    {
                        menuItem.Position = new CCPoint(x + menuItem.ContentSize.Width * menuItem.ScaleX / 2.0f, 0);
                        x += menuItem.ContentSize.Width * menuItem.ScaleX + padding;
                        height = Math.Max(height, menuItem.ContentSize.Height);
                    }
                }
            }

            ContentSize = new CCSize(width, height);
        }

        public void AlignItemsInColumns(params uint[] numOfItemsPerRow)
        {
            float height = -DefaultPadding;
            int row = 0;
            int rowHeight = 0;
            int columnsOccupied = 0;
            uint rowColumns;

            if (menuItems != null && menuItems.Count > 0)
            {
                foreach (CCMenuItem item in menuItems)
                {
                    if (item.Visible) 
                    {
                        Debug.Assert (row < numOfItemsPerRow.Length);

                        rowColumns = numOfItemsPerRow[row];
                        // can not have zero columns on a row
                        Debug.Assert (rowColumns > 0, "");

                        float tmp = item.ContentSize.Height;
                        rowHeight = (int)((rowHeight >= tmp || float.IsNaN (tmp)) ? rowHeight : tmp);

                        ++columnsOccupied;
                        if (columnsOccupied >= rowColumns) 
                        {
                            height += rowHeight + (int)DefaultPadding;

                            columnsOccupied = 0;
                            rowHeight = 0;
                            ++row;
                        }
                    }
                }

                // check if too many rows/columns for available menu items
                Debug.Assert(columnsOccupied == 0, "");

                CCSize menuSize = ContentSize;

                row = 0;
                rowHeight = 0;
                rowColumns = 0;
                float w = 0.0f;
                float x = 0.0f;
                float y = (height / 2f);

                foreach (CCMenuItem item in menuItems)
                {
                    if (item.Visible) 
                    {
                        if (rowColumns == 0) 
                        {
                            rowColumns = numOfItemsPerRow[row];
                            if (rowColumns == 0) {
                                throw (new ArgumentException ("Can not have a zero column size for a row."));
                            }
                            w = (menuSize.Width - 2 * DefaultPadding) / rowColumns; // 1 + rowColumns
                            x = w / 2f; // center of column
                        }

                        float tmp = item.ContentSize.Height * item.ScaleY;
                        rowHeight = (int)((rowHeight >= tmp || float.IsNaN (tmp)) ? rowHeight : tmp);

                        item.Position 
                        = new CCPoint(DefaultPadding + x - (menuSize.Width - 2 * DefaultPadding) / 2, 
                            y - item.ContentSize.Height * item.ScaleY / 2);

                        x += w;
                        ++columnsOccupied;

                        if (columnsOccupied >= rowColumns) 
                        {
                            y -= rowHeight + DefaultPadding;

                            columnsOccupied = 0;
                            rowColumns = 0;
                            rowHeight = 0;
                            ++row;
                        }
                    }
                }
            }
        }

        public void AlignItemsInRows(params uint[] numOfItemsPerColumn)
        {
            List<float> columnWidths = new List<float>();
            List<float> columnHeights = new List<float>();

            float width = -DefaultPadding * 2.0f;
            float columnHeight = -DefaultPadding;
            int column = 0;
            int columnWidth = 0;
            int rowsOccupied = 0;
            uint columnRows;

            if (menuItems != null && menuItems.Count > 0)
            {
                foreach (CCMenuItem item in menuItems)
                {
                    if(item.Visible) 
                    {
                        // check if too many menu items for the amount of rows/columns
                        Debug.Assert (column < numOfItemsPerColumn.Length, "");

                        columnRows = numOfItemsPerColumn[column];
                        // can't have zero rows on a column
                        Debug.Assert (columnRows > 0, "");

                        float tmp = item.ContentSize.Width * item.ScaleX;
                        columnWidth = (int)((columnWidth >= tmp || float.IsNaN (tmp)) ? columnWidth : tmp);


                        columnHeight += (int)(item.ContentSize.Height * item.ScaleY + DefaultPadding);
                        ++rowsOccupied;

                        if (rowsOccupied >= columnRows) 
                        {
                            columnWidths.Add(columnWidth);
                            columnHeights.Add(columnHeight);
                            width += columnWidth + DefaultPadding * 2.0f;

                            rowsOccupied = 0;
                            columnWidth = 0;
                            columnHeight = -DefaultPadding;
                            ++column;
                        }
                    }
                }

                // check if too many rows/columns for available menu items.
                Debug.Assert(rowsOccupied == 0, "");

                CCSize menuSize = ContentSize;

                column = 0;
                columnWidth = 0;
                columnRows = 0;
                float x = (-width / 2f);
                float y = 0.0f;

                foreach(CCMenuItem item in menuItems)
                {
                    if (item.Visible) 
                    {
                        if (columnRows == 0) 
                        {
                            columnRows = numOfItemsPerColumn[column];
                            y = columnHeights [column];
                        }

                        // columnWidth = fmaxf(columnWidth, [item contentSize].width);
                        float tmp = item.ContentSize.Width * item.ScaleX;
                        columnWidth = (int)((columnWidth >= tmp || float.IsNaN (tmp)) ? columnWidth : tmp);

                        item.Position = new CCPoint (x + columnWidths [column] / 2, y - menuSize.Height / 2);
                        y -= item.ContentSize.Height * item.ScaleY + 10;
                        ++rowsOccupied;

                        if (rowsOccupied >= columnRows) 
                        {
                            x += columnWidth + 5;
                            rowsOccupied = 0;
                            columnRows = 0;
                            columnWidth = 0;
                            ++column;
                        }
                    }
                }
            }
        }

        #endregion Alignment

    }
}