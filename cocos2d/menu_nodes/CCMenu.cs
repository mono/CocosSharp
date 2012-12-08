using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace cocos2d
{
    public enum CCMenuState
    {
        Waiting,
        TrackingTouch
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
        private ccColor3B m_tColor;

        /// <summary>
        /// Default ctor that sets the content size of the menu to match the window size.
        /// </summary>
        private CCMenu() 
        {
            ContentSize = CCDirector.SharedDirector.WinSize;
        }

        public bool Enabled
        {
            get { return m_bEnabled; }
            set { m_bEnabled = value; }
        }

        public new static CCMenu Create()
        {
            return Create(null);
        }

        public static CCMenu Create(params CCMenuItem[] items)
        {
            var pRet = new CCMenu();
            pRet.InitWithItems(items);
            return pRet;
        }

        public override bool Init()
        {
            return InitWithArray(null);
        }

        public bool InitWithItems(params CCMenuItem[] items)
        {
            return InitWithArray(items);
        }

        /// <summary>
        /// The position of the menu is set to the center of the main screen
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        private bool InitWithArray(params CCMenuItem[] items)
        {
            if (base.Init())
            {
                TouchEnabled = true;

                m_bEnabled = true;
                // menu in the center of the screen
                CCSize s = CCDirector.SharedDirector.WinSize;

                IgnoreAnchorPointForPosition = true;
                AnchorPoint = new CCPoint(0.5f, 0.5f);
                ContentSize = s;

                Position = (new CCPoint(s.width / 2, s.height / 2));

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
        }

        public override void OnExit()
        {
            if (m_eState == CCMenuState.TrackingTouch)
            {
                m_pSelectedItem.Unselected();
                m_eState = CCMenuState.Waiting;
                m_pSelectedItem = null;
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
                    height += pChild.ContentSize.height * pChild.ScaleY + padding;
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
                    pChild.Position = new CCPoint(0, y - pChild.ContentSize.height * pChild.ScaleY / 2.0f);
                    y -= pChild.ContentSize.height * pChild.ScaleY + padding;
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
                    width += pChild.ContentSize.width * pChild.ScaleX + padding;
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
                    pChild.Position = new CCPoint(x + pChild.ContentSize.width * pChild.ScaleX / 2.0f, 0);
                    x += pChild.ContentSize.width * pChild.ScaleX + padding;
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

                    float tmp = pChild.ContentSize.height;
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
                            w = (winSize.width - 2 * kDefaultPadding) / rowColumns; // 1 + rowColumns
                            x = w/2f; // center of column
                    }

                        float tmp = pChild.ContentSize.height*pChild.ScaleY;
                    rowHeight = (int) ((rowHeight >= tmp || float.IsNaN(tmp)) ? rowHeight : tmp);

                        pChild.Position = new CCPoint(kDefaultPadding + x - (winSize.width - 2*kDefaultPadding) / 2,
                                               y - pChild.ContentSize.height*pChild.ScaleY / 2);

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
                    float tmp = pChild.ContentSize.width * pChild.ScaleX;
                    columnWidth = (int)((columnWidth >= tmp || float.IsNaN(tmp)) ? columnWidth : tmp);


                    columnHeight += (int)(pChild.ContentSize.height * pChild.ScaleY + 5);
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
                    float tmp = pChild.ContentSize.width * pChild.ScaleX;
                    columnWidth = (int)((columnWidth >= tmp || float.IsNaN(tmp)) ? columnWidth : tmp);

                    pChild.Position = new CCPoint(x + columnWidths[column] / 2,
                                                  y - winSize.height / 2);
                    y -= pChild.ContentSize.height * pChild.ScaleY + 10;
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

        public ccColor3B Color
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
                        r.origin = CCPoint.Zero;

                        if (r.containsPoint(local))
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