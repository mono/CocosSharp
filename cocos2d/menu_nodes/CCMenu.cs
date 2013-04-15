using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace cocos2d
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
    public class CCMenu : CCLayer, ICCRGBAProtocol, ICCTouchDelegate
    {
        public const float kDefaultPadding = 5;
        public const int kCCMenuHandlerPriority = -128;

        protected bool m_bEnabled;
        protected CCMenuState m_eState;
        protected CCMenuItem m_pSelectedItem;

        private byte m_cOpacity;
        private CCColor3B m_tColor;

        private LinkedList<CCMenuItem> _Items = new LinkedList<CCMenuItem>();


        /// <summary>
        /// Default ctor that sets the content size of the menu to match the window size.
        /// </summary>
        private CCMenu() 
        {
            Init();
//            ContentSize = CCDirector.SharedDirector.WinSize;
        }
        public CCMenu(params CCMenuItem[] items)
        {
            InitWithItems(items);
        }

        public override bool HasFocus
        {
            set
            {
                base.HasFocus = value;
                // Set the first menu item to have the focus
                if (ItemWithFocus == null)
                {
                    _Items.First.Value.HasFocus = true;
                }
            }
        }

        /// <summary>
        /// Handles the button press event to track which focused menu item will get the activation
        /// </summary>
        /// <param name="backButton"></param>
        /// <param name="startButton"></param>
        /// <param name="systemButton"></param>
        /// <param name="aButton"></param>
        /// <param name="bButton"></param>
        /// <param name="xButton"></param>
        /// <param name="yButton"></param>
        /// <param name="leftShoulder"></param>
        /// <param name="rightShoulder"></param>
        /// <param name="player"></param>
        protected override void OnGamePadButtonUpdate(CCGamePadButtonStatus backButton, CCGamePadButtonStatus startButton, CCGamePadButtonStatus systemButton, CCGamePadButtonStatus aButton, CCGamePadButtonStatus bButton, CCGamePadButtonStatus xButton, CCGamePadButtonStatus yButton, CCGamePadButtonStatus leftShoulder, CCGamePadButtonStatus rightShoulder, Microsoft.Xna.Framework.PlayerIndex player)
        {
            base.OnGamePadButtonUpdate(backButton, startButton, systemButton, aButton, bButton, xButton, yButton, leftShoulder, rightShoulder, player);
            if (!HasFocus)
            {
                return;
            }
            if (backButton == CCGamePadButtonStatus.Pressed || aButton == CCGamePadButtonStatus.Pressed || bButton == CCGamePadButtonStatus.Pressed ||
                xButton == CCGamePadButtonStatus.Pressed || yButton == CCGamePadButtonStatus.Pressed || leftShoulder == CCGamePadButtonStatus.Pressed ||
                rightShoulder == CCGamePadButtonStatus.Pressed)
            {
                CCMenuItem item = ItemWithFocus;
                item.Selected();
                m_pSelectedItem = item;
                m_eState = CCMenuState.TrackingTouch;
            }
            else if (backButton == CCGamePadButtonStatus.Released || aButton == CCGamePadButtonStatus.Released || bButton == CCGamePadButtonStatus.Released ||
                xButton == CCGamePadButtonStatus.Released || yButton == CCGamePadButtonStatus.Released || leftShoulder == CCGamePadButtonStatus.Released ||
                rightShoulder == CCGamePadButtonStatus.Released)
            {
                if (m_eState == CCMenuState.TrackingTouch)
                {
                    // Now we are selecting the menu item
                    CCMenuItem item = ItemWithFocus;
                    if (item != null && m_pSelectedItem == item)
                    {
                        // Activate this item
                        item.Unselected();
                        item.Activate();
                        m_eState = CCMenuState.Waiting;
                        m_pSelectedItem = null;
                    }
                }
            }
        }

        /// <summary>
        /// Returns the menu item with the focus. Note that this only has a value if the GamePad or Keyboard is enabled. Touch
        /// devices do not have a "focus" concept.
        /// </summary>
        public CCMenuItem ItemWithFocus
        {
            get
            {
                // Find the item with the focus
                foreach(CCMenuItem item in _Items) 
                {
                    if (item.HasFocus)
                    {
                        return (item);
                    }
                }
                return (null);
            }
        }

        public bool Enabled
        {
            get { return m_bEnabled; }
            set { m_bEnabled = value; }
        }

        public override bool Init()
        {
            return InitWithArray(null);
        }

        public bool InitWithItems(params CCMenuItem[] items)
        {
            return InitWithArray(items);
        }

        public override void RemoveChild(CCNode child, bool cleanup)
        {
            base.RemoveChild(child, cleanup);
            if (_Items.Contains(child as CCMenuItem))
            {
                _Items.Remove(child as CCMenuItem);
            }
        }
        /// <summary>
        /// The position of the menu is set to the center of the main screen
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        private bool InitWithArray(params CCMenuItem[] items)
        {
            if (_Items.Count > 0)
            {
                List<CCMenuItem> copy = new List<CCMenuItem>(_Items);
                foreach (CCMenuItem i in copy)
                {
                    RemoveChild(i, false);
                }
            }
            if (base.Init())
            {
                TouchEnabled = true;

                m_bEnabled = true;
                // menu in the center of the screen
                CCSize s = CCDirector.SharedDirector.WinSize;

                IgnoreAnchorPointForPosition = true;
                AnchorPoint = new CCPoint(0.5f, 0.5f);
                ContentSize = s;

                Position = (new CCPoint(s.Width / 2, s.Height / 2));

                if (items != null)
                {
                    int z = 0;
                    foreach (CCMenuItem item in items)
                    {
                        AddChild(item, z);
                        z++;
                    }
                }

                //    [self alignItemsVertically];
                m_pSelectedItem = null;
                m_eState = CCMenuState.Waiting;
                return true;
            }
            return false;
        }

        /*
        * override add:
        */

        public override void AddChild(CCNode child, int zOrder, int tag)
        {
            Debug.Assert(child is CCMenuItem, "Menu only supports MenuItem objects as children");
            base.AddChild(child, zOrder, tag);
            if (_Items.Count == 0)
            {
                _Items.AddFirst(child as CCMenuItem);
            }
            else
            {
                _Items.AddLast(child as CCMenuItem);
            }
        }

        public override void OnEnter()
        {
            base.OnEnter();
            foreach (CCMenuItem item in _Items)
            {
                CCFocusManager.Instance.Add(item);
        }
        }

        public override void OnExit()
        {
            if (m_eState == CCMenuState.TrackingTouch)
            {
                m_pSelectedItem.Unselected();
                m_eState = CCMenuState.Waiting;
                m_pSelectedItem = null;
            }
            foreach (CCMenuItem item in _Items)
            {
                CCFocusManager.Instance.Remove(item);
            }
            base.OnExit();
        }

        #region Menu - Events

        public void SetHandlerPriority(int newPriority)
        {
            CCTouchDispatcher pDispatcher = CCDirector.SharedDirector.TouchDispatcher;
            pDispatcher.SetPriority(newPriority, this);
        }

        public override void RegisterWithTouchDispatcher()
        {
            CCDirector pDirector = CCDirector.SharedDirector;
            pDirector.TouchDispatcher.AddTargetedDelegate(this, kCCMenuHandlerPriority, true);
        }

        public override bool TouchBegan(CCTouch touch, CCEvent e)
        {
            if (m_eState != CCMenuState.Waiting || !m_bIsVisible || !m_bEnabled)
            {
                return false;
            }

            for (CCNode c = m_pParent; c != null; c = c.Parent)
            {
                if (c.Visible == false)
                {
                    return false;
                }
            }

            m_pSelectedItem = ItemForTouch(touch);
            if (m_pSelectedItem != null)
            {
                m_eState = CCMenuState.TrackingTouch;
                m_pSelectedItem.Selected();
                return true;
            }
            return false;
        }

        public override void TouchEnded(CCTouch touch, CCEvent e)
        {
            Debug.Assert(m_eState == CCMenuState.TrackingTouch, "[Menu TouchEnded] -- invalid state");
            if (m_pSelectedItem != null)
            {
                m_pSelectedItem.Unselected();
                m_pSelectedItem.Activate();
            }
            m_eState = CCMenuState.Waiting;
        }

        public override void TouchCancelled(CCTouch touch, CCEvent e)
        {
            Debug.Assert(m_eState == CCMenuState.TrackingTouch, "[Menu ccTouchCancelled] -- invalid state");
            if (m_pSelectedItem != null)
            {
                m_pSelectedItem.Unselected();
            }
            m_eState = CCMenuState.Waiting;
        }

        public override void TouchMoved(CCTouch touch, CCEvent e)
        {
            Debug.Assert(m_eState == CCMenuState.TrackingTouch, "[Menu TouchMoved] -- invalid state");
            CCMenuItem currentItem = ItemForTouch(touch);
            if (currentItem != m_pSelectedItem)
            {
                if (m_pSelectedItem != null)
                {
                    m_pSelectedItem.Unselected();
                }

                m_pSelectedItem = currentItem;

                if (m_pSelectedItem != null)
                {
                    m_pSelectedItem.Selected();
                }
            }
        }

        #endregion

        #region Menu - Alignment

        public void AlignItemsVertically()
        {
            AlignItemsVerticallyWithPadding(kDefaultPadding);
        }

        public void AlignItemsVerticallyWithPadding(float padding)
        {
            float height = -padding;

            if (m_pChildren != null && m_pChildren.count > 0)
            {
                for (int i = 0, count = m_pChildren.count; i < count; i++)
                {
                    CCNode pChild = m_pChildren[i];
                    if (!pChild.Visible)
                    {
                        continue;
                    }
                    height += pChild.ContentSize.Height * pChild.ScaleY + padding;
                }
            }

            float y = height / 2.0f;

            if (m_pChildren != null && m_pChildren.count > 0)
            {
                for (int i = 0, count = m_pChildren.count; i < count; i++)
                {
                    CCNode pChild = m_pChildren[i];
                    if (!pChild.Visible)
                    {
                        continue;
                    }
                    pChild.Position = new CCPoint(0, y - pChild.ContentSize.Height * pChild.ScaleY / 2.0f);
                    y -= pChild.ContentSize.Height * pChild.ScaleY + padding;
                }
            }
        }

        public void AlignItemsHorizontally()
        {
            AlignItemsHorizontallyWithPadding(kDefaultPadding);
        }

        public void AlignItemsHorizontallyWithPadding(float padding)
        {
            float width = -padding;
            if (m_pChildren != null && m_pChildren.count > 0)
            {
                for (int i = 0, count = m_pChildren.count; i < count; i++)
                {
                    CCNode pChild = m_pChildren[i];
                    if (pChild.Visible)
                    {
                    width += pChild.ContentSize.Width * pChild.ScaleX + padding;
                }
            }
            }

            float x = -width / 2.0f;

            if (m_pChildren != null && m_pChildren.count > 0)
            {
                for (int i = 0, count = m_pChildren.count; i < count; i++)
                {
                    CCNode pChild = m_pChildren[i];
                    if (pChild.Visible)
                    {
                    pChild.Position = new CCPoint(x + pChild.ContentSize.Width * pChild.ScaleX / 2.0f, 0);
                    x += pChild.ContentSize.Width * pChild.ScaleX + padding;
                }
            }
        }
        }

        public void AlignItemsInColumns(params int[] columns)
        {
            int[] rows = columns;

            int height = -5;
            int row = 0;
            int rowHeight = 0;
            int columnsOccupied = 0;
            int rowColumns;

            if (m_pChildren != null && m_pChildren.count > 0)
            {
                for (int i = 0, count = m_pChildren.count; i < count; i++)
                {
                    CCNode pChild = m_pChildren.Elements[i];
                    if (!pChild.Visible)
                    {
                        continue;
                    }
                    Debug.Assert(row < rows.Length, "");

                    rowColumns = rows[row];
                    // can not have zero columns on a row
                    Debug.Assert(rowColumns > 0, "");

                    float tmp = pChild.ContentSize.Height;
                    rowHeight = (int) ((rowHeight >= tmp || float.IsNaN(tmp)) ? rowHeight : tmp);

                    ++columnsOccupied;
                    if (columnsOccupied >= rowColumns)
                    {
                            height += rowHeight + (int)kDefaultPadding;

                        columnsOccupied = 0;
                        rowHeight = 0;
                        ++row;
                    }
                }
            }

            // check if too many rows/columns for available menu items
            Debug.Assert(columnsOccupied == 0, "");

            CCSize winSize = ContentSize; // CCDirector.SharedDirector.WinSize;

            row = 0;
            rowHeight = 0;
            rowColumns = 0;
            float w = 0.0f;
            float x = 0.0f;
            float y = (height / 2f);

            if (m_pChildren != null && m_pChildren.count > 0)
            {
                for (int i = 0, count = m_pChildren.count; i < count; i++)
                {
                    CCNode pChild = m_pChildren.Elements[i];
                    if (!pChild.Visible)
                    {
                        continue;
                    }
                    if (rowColumns == 0)
                    {
                        rowColumns = rows[row];
                            if (rowColumns == 0)
                            {
                                throw (new ArgumentException("Can not have a zero column size for a row."));
                            }
                            w = (winSize.Width - 2 * kDefaultPadding) / rowColumns; // 1 + rowColumns
                            x = w/2f; // center of column
                    }

                        float tmp = pChild.ContentSize.Height*pChild.ScaleY;
                    rowHeight = (int) ((rowHeight >= tmp || float.IsNaN(tmp)) ? rowHeight : tmp);

                        pChild.Position = new CCPoint(kDefaultPadding + x - (winSize.Width - 2*kDefaultPadding) / 2,
                                               y - pChild.ContentSize.Height*pChild.ScaleY / 2);

                    x += w;
                    ++columnsOccupied;

                    if (columnsOccupied >= rowColumns)
                    {
                        y -= rowHeight + 5;

                        columnsOccupied = 0;
                        rowColumns = 0;
                        rowHeight = 0;
                        ++row;
                    }
                }
            }
        }


        public void AlignItemsInRows(params int[] rows)
        {
            int[] columns = rows;

            List<int> columnWidths = new List<int>();

            List<int> columnHeights = new List<int>();


            int width = -10;
            int columnHeight = -5;
            int column = 0;
            int columnWidth = 0;
            int rowsOccupied = 0;
            int columnRows;

            if (m_pChildren != null && m_pChildren.count > 0)
            {
                for (int i = 0, count = m_pChildren.count; i < count; i++)
                {
                    CCNode pChild = m_pChildren.Elements[i];
                    if (!pChild.Visible)
                    {
                        continue;
                    }

                    // check if too many menu items for the amount of rows/columns
                    Debug.Assert(column < columns.Length, "");

                    columnRows = columns[column];
                    // can't have zero rows on a column
                    Debug.Assert(columnRows > 0, "");

                    // columnWidth = fmaxf(columnWidth, [item contentSize].width);
                    float tmp = pChild.ContentSize.Width * pChild.ScaleX;
                    columnWidth = (int)((columnWidth >= tmp || float.IsNaN(tmp)) ? columnWidth : tmp);


                    columnHeight += (int)(pChild.ContentSize.Height * pChild.ScaleY + 5);
                    ++rowsOccupied;

                    if (rowsOccupied >= columnRows)
                    {
                        columnWidths.Add(columnWidth);
                        columnHeights.Add(columnHeight);
                        width += columnWidth + 10;

                        rowsOccupied = 0;
                        columnWidth = 0;
                        columnHeight = -5;
                        ++column;
                    }
                }
            }

            // check if too many rows/columns for available menu items.
            Debug.Assert(rowsOccupied == 0, "");

            CCSize winSize = ContentSize; // CCDirector.SharedDirector.WinSize;

            column = 0;
            columnWidth = 0;
            columnRows = 0;
            float x = (-width / 2f);
            float y = 0.0f;

            if (m_pChildren != null && m_pChildren.count > 0)
            {
                for (int i = 0, count = m_pChildren.count; i < count; i++)
                {
                    CCNode pChild = m_pChildren.Elements[i];
                    if (!pChild.Visible)
                    {
                        continue;
                    }

                    if (columnRows == 0)
                    {
                        columnRows = columns[column];
                        y = columnHeights[column];
                    }

                    // columnWidth = fmaxf(columnWidth, [item contentSize].width);
                    float tmp = pChild.ContentSize.Width * pChild.ScaleX;
                    columnWidth = (int)((columnWidth >= tmp || float.IsNaN(tmp)) ? columnWidth : tmp);

                    pChild.Position = new CCPoint(x + columnWidths[column] / 2,
                                                  y - winSize.Height / 2);
                    y -= pChild.ContentSize.Height * pChild.ScaleY + 10;
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

        #endregion

        #region Opacity Protocol

        public CCColor3B Color
        {
            get { return m_tColor; }
            set
            {
                m_tColor = value;

                if (m_pChildren != null && m_pChildren.count > 0)
                {
                    for (int i = 0, count = m_pChildren.count; i < count; i++)
                    {
                        var pRGBAProtocol = m_pChildren.Elements[i] as ICCRGBAProtocol;
                        if (pRGBAProtocol != null)
                        {
                            pRGBAProtocol.Color = m_tColor;
                        }
                    }
                }
            }
        }

        public byte Opacity
        {
            get { return m_cOpacity; }
            set
            {
                m_cOpacity = value;

                if (m_pChildren != null && m_pChildren.count > 0)
                {
                    for (int i = 0, count = m_pChildren.count; i < count; i++)
                    {
                        var pRGBAProtocol = m_pChildren.Elements[i] as ICCRGBAProtocol;
                        if (pRGBAProtocol != null)
                        {
                            pRGBAProtocol.Opacity = m_cOpacity;
                        }
                    }
                }
            }
        }

        public bool IsOpacityModifyRGB { get; set; }

        #endregion

        protected virtual CCMenuItem ItemForTouch(CCTouch touch)
        {
            CCPoint touchLocation = touch.Location;

            if (m_pChildren != null && m_pChildren.count > 0)
            {
                for (int i = 0, count = m_pChildren.count; i < count; i++)
                {
                    var pChild = m_pChildren.Elements[i] as CCMenuItem;
                    if (pChild != null && pChild.Visible && pChild.Enabled)
                    {
                        CCPoint local = pChild.ConvertToNodeSpace(touchLocation);
                        CCRect r = pChild.Rect();
                        r.Origin = CCPoint.Zero;

                        if (r.ContainsPoint(local))
                        {
                            return pChild;
                        }
                    }
                }
            }

            return null;
        }
    }
}