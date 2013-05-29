using System;
using System.Collections.Generic;

namespace Cocos2D
{
    public enum CCTableViewVerticalFillOrder
    {
        FillTopDown,
        FillBottomUp
    };

    ///**
    // * Sole purpose of this delegate is to single touch event in this version.
    // */
    public interface CCTableViewDelegate : CCScrollViewDelegate
    {
        /**
         * Delegate to respond touch event
         *
         * @param table table contains the given cell
         * @param cell  cell that is touched
         */
        void TableCellTouched(CCTableView table, CCTableViewCell cell);

        /**
         * Delegate to respond a table cell press event.
         *
         * @param table table contains the given cell
         * @param cell  cell that is pressed
         */
        void TableCellHighlight(CCTableView table, CCTableViewCell cell);

        /**
         * Delegate to respond a table cell release event
         *
         * @param table table contains the given cell
         * @param cell  cell that is pressed
         */
        void TableCellUnhighlight(CCTableView table, CCTableViewCell cell);

        /**
         * Delegate called when the cell is about to be recycled. Immediately
         * after this call the cell will be removed from the scene graph and
         * recycled.
         *
         * @param table table contains the given cell
         * @param cell  cell that is pressed
         */
        void TableCellWillRecycle(CCTableView table, CCTableViewCell cell);
    }


    /**
     * Data source that governs table backend data.
     */

    public interface CCTableViewDataSource
    {
        /**
         * cell height for a given table.
         *
         * @param table table to hold the instances of Class
         * @return cell size
         */
        CCSize TableCellSizeForIndex(CCTableView table, int idx);
        /**
         * a cell instance at a given index
         *
         * @param idx index to search for a cell
         * @return cell found at idx
         */
        CCTableViewCell TableCellAtIndex(CCTableView table, int idx);
        /**
         * Returns number of cells in a given table view.
         * 
         * @return number of cells
         */
        int NumberOfCellsInTableView(CCTableView table);
    };


    /**
     * UITableView counterpart for cocos2d for iphone.
     *
     * this is a very basic, minimal implementation to bring UITableView-like component into cocos2d world.
     * 
     */
    public class CCTableView : CCScrollView, CCScrollViewDelegate
    {
        protected CCTableViewCell m_pTouchedCell;
        protected CCTableViewVerticalFillOrder m_eVordering;
        protected List<int> m_pIndices;
        protected List<float> m_vCellsPositions;
        protected CCArrayForObjectSorting m_pCellsUsed;
        protected CCArrayForObjectSorting m_pCellsFreed;
        protected CCTableViewDataSource m_pDataSource;
        protected CCTableViewDelegate m_pTableViewDelegate;
        protected CCScrollViewDirection m_eOldDirection;

        /**
         * data source
         */
        public CCTableViewDataSource DataSource
        {
            get { return m_pDataSource; }
            set { m_pDataSource = value; }
        }

        /**
         * delegate
         */
        public new CCTableViewDelegate Delegate
        {
            get { return m_pTableViewDelegate; }
            set { m_pTableViewDelegate = value; }
        }

        public CCTableView()
        {
            m_eOldDirection = CCScrollViewDirection.None;
        }

        /**
         * An intialized table view object
         *
         * @param dataSource data source
         * @param size view size
         * @return table view
         */
        public CCTableView(CCTableViewDataSource dataSource, CCSize size)
            : this(dataSource, size, null)
        { }

        /**
         * An initialized table view object
         *
         * @param dataSource data source;
         * @param size view size
         * @param container parent object for cells
         * @return table view
         */
        public CCTableView(CCTableViewDataSource dataSource, CCSize size, CCNode container)
        {
            InitWithViewSize(size, container);
            DataSource = dataSource;
            _updateCellPositions();
            _updateContentSize();
        }

        public new bool InitWithViewSize(CCSize size, CCNode container)
        {
            if (base.InitWithViewSize(size, container))
            {
                m_vCellsPositions = new List<float>();
                m_pCellsUsed = new CCArrayForObjectSorting();
                m_pCellsFreed = new CCArrayForObjectSorting();
                m_pIndices = new List<int>();
                m_eVordering = CCTableViewVerticalFillOrder.FillBottomUp;
                Direction = CCScrollViewDirection.Vertical;

                base.Delegate = this;
                return true;
            }
            return false;
        }

        /**
         * determines how cell is ordered and filled in the view.
         */
        public CCTableViewVerticalFillOrder VerticalFillOrder
        {
            get { return m_eVordering; }
            set
            {
                if (m_eVordering != value)
                {
                    m_eVordering = value;
                    if (m_pCellsUsed.Count > 0)
                    {
                        ReloadData();
                    }
                }
            }
        }

        /**
         * reloads data from data source.  the view will be refreshed.
         */
        public void ReloadData()
        {
            m_eOldDirection = CCScrollViewDirection.None;

            foreach (CCTableViewCell cell in m_pCellsUsed)
            {
                if (m_pTableViewDelegate != null)
                {
                    m_pTableViewDelegate.TableCellWillRecycle(this, cell);
                }

                m_pCellsFreed.Add(cell);
                cell.Reset();
                if (cell.Parent == Container)
                {
                    Container.RemoveChild(cell, true);
                }
            }

            m_pIndices.Clear();
            m_pCellsUsed = new CCArrayForObjectSorting();

            _updateCellPositions();
            _updateContentSize();
            if (m_pDataSource.NumberOfCellsInTableView(this) > 0)
            {
                ScrollViewDidScroll(this);
            }
        }

        /**
         * Returns an existing cell at a given index. Returns nil if a cell is nonexistent at the moment of query.
         *
         * @param idx index
         * @return a cell at a given index
         */
        public CCTableViewCell CellAtIndex(int idx)
        {
            CCTableViewCell found = null;

            if (m_pIndices.Contains(idx))
            {
                found = (CCTableViewCell)m_pCellsUsed.ObjectWithObjectID(idx);
            }

            return found;
        }

        /**
         * Updates the content of the cell at a given index.
         *
         * @param idx index to find a cell
         */
        public void UpdateCellAtIndex(int idx)
        {
            if (idx == CCArrayForObjectSorting.CC_INVALID_INDEX)
            {
                return;
            }

            var uCountOfItems = m_pDataSource.NumberOfCellsInTableView(this);
            if (uCountOfItems == 0 || idx > uCountOfItems - 1)
            {
                return;
            }

            var cell = CellAtIndex(idx);
            if (cell != null)
            {
                _moveCellOutOfSight(cell);
            }
            cell = m_pDataSource.TableCellAtIndex(this, idx);
            _setIndexForCell(idx, cell);
            _addCellIfNecessary(cell);
        }

        /**
         * Removes a cell at a given index
         *
         * @param idx index to find a cell
         */
        public void RemoveCellAtIndex(int idx)
        {
            if (idx == CCArrayForObjectSorting.CC_INVALID_INDEX)
            {
                return;
            }

            var uCountOfItems = m_pDataSource.NumberOfCellsInTableView(this);
            if (uCountOfItems == 0 || idx > uCountOfItems - 1)
            {
                return;
            }

            int newIdx;
            CCTableViewCell cell = CellAtIndex(idx);
            if (cell == null)
            {
                return;
            }

            newIdx = m_pCellsUsed.IndexOfSortedObject(cell);

            //remove first
            _moveCellOutOfSight(cell);

            m_pIndices.Remove(idx);
            //    [m_pIndices shiftIndexesStartingAtIndex:idx+1 by:-1];
            for (int i = m_pCellsUsed.Count - 1; i > newIdx; i--)
            {
                cell = (CCTableViewCell)m_pCellsUsed[i];
                _setIndexForCell(cell.Index - 1, cell);
            }
        }

        /**
         * Dequeues a free cell if available. nil if not.
         *
         * @return free cell
         */
        public CCTableViewCell DequeueCell()
        {
            CCTableViewCell cell;

            if (m_pCellsFreed.Count == 0)
            {
                cell = null;
            }
            else
            {
                cell = (CCTableViewCell)m_pCellsFreed[0];
                m_pCellsFreed.RemoveAt(0);
            }
            return cell;
        }

        protected void _addCellIfNecessary(CCTableViewCell cell)
        {
            if (cell.Parent != Container)
            {
                Container.AddChild(cell);
            }
            m_pCellsUsed.InsertSortedObject(cell);
            m_pIndices.Add(cell.Index);
        }

        protected void _updateContentSize()
        {
            CCSize size = CCSize.Zero;
            int cellsCount = m_pDataSource.NumberOfCellsInTableView(this);

            if (cellsCount > 0)
            {
                float maxPosition = m_vCellsPositions[cellsCount];

                switch (Direction)
                {
                    case CCScrollViewDirection.Horizontal:
                        size = new CCSize(maxPosition, m_tViewSize.Height);
                        break;
                    default:
                        size = new CCSize(m_tViewSize.Width, maxPosition);
                        break;
                }
            }

            ContentSize = size;

            if (m_eOldDirection != m_eDirection)
            {
                if (m_eDirection == CCScrollViewDirection.Horizontal)
                {
                    SetContentOffset(CCPoint.Zero);
                }
                else
                {
                    SetContentOffset(new CCPoint(0, MinContainerOffset.Y));
                }
                m_eOldDirection = m_eDirection;
            }
        }

        protected CCPoint _offsetFromIndex(int index)
        {
            CCPoint offset = __offsetFromIndex(index);

            CCSize cellSize = m_pDataSource.TableCellSizeForIndex(this, index);
            if (m_eVordering == CCTableViewVerticalFillOrder.FillTopDown)
            {
                offset.Y = Container.ContentSize.Height - offset.Y - cellSize.Height;
            }
            return offset;
        }

        protected CCPoint __offsetFromIndex(int index)
        {
            CCPoint offset;

            switch (Direction)
            {
                case CCScrollViewDirection.Horizontal:
                    offset = new CCPoint(m_vCellsPositions[index], 0.0f);
                    break;
                default:
                    offset = new CCPoint(0.0f, m_vCellsPositions[index]);
                    break;
            }

            return offset;
        }

        protected int _indexFromOffset(CCPoint offset)
        {
            int index = 0;
            int maxIdx = m_pDataSource.NumberOfCellsInTableView(this) - 1;

            if (m_eVordering == CCTableViewVerticalFillOrder.FillTopDown)
            {
                offset.Y = Container.ContentSize.Height - offset.Y;
            }
            index = __indexFromOffset(offset);
            if (index != -1)
            {
                index = Math.Max(0, index);
                if (index > maxIdx)
                {
                    index = CCArrayForObjectSorting.CC_INVALID_INDEX;
                }
            }
            return index;
        }

        protected int __indexFromOffset(CCPoint offset)
        {
            int low = 0;
            int high = m_pDataSource.NumberOfCellsInTableView(this) - 1;
            float search;
            switch (Direction)
            {
                case CCScrollViewDirection.Horizontal:
                    search = offset.X;
                    break;
                default:
                    search = offset.Y;
                    break;
            }

            while (high >= low)
            {
                int index = low + (high - low) / 2;
                float cellStart = m_vCellsPositions[index];
                float cellEnd = m_vCellsPositions[index + 1];

                if (search >= cellStart && search <= cellEnd)
                {
                    return index;
                }
                else if (search < cellStart)
                {
                    high = index - 1;
                }
                else
                {
                    low = index + 1;
                }
            }

            if (low <= 0)
            {
                return 0;
            }

            return -1;
        }

        protected void _moveCellOutOfSight(CCTableViewCell cell)
        {
            if (m_pTableViewDelegate != null)
            {
                m_pTableViewDelegate.TableCellWillRecycle(this, cell);
            }

            m_pCellsFreed.Add(cell);
            m_pCellsUsed.RemoveSortedObject(cell);
            m_pIndices.Remove(cell.Index);
            cell.Reset();
            if (cell.Parent == Container)
            {
                Container.RemoveChild(cell, true);
            }
        }

        protected void _setIndexForCell(int index, CCTableViewCell cell)
        {
            cell.AnchorPoint = CCPoint.Zero;
            cell.Position = _offsetFromIndex(index);
            cell.Index = index;
        }

        protected void _updateCellPositions() 
        {
            int cellsCount = m_pDataSource.NumberOfCellsInTableView(this);
            m_vCellsPositions.Clear();

            if (cellsCount > 0)
            {
                float currentPos = 0;
                CCSize cellSize;
                for (int i=0; i < cellsCount; i++)
                {
                    m_vCellsPositions.Add(currentPos);
                    cellSize = m_pDataSource.TableCellSizeForIndex(this, i);
                    switch (Direction)
                    {
                        case CCScrollViewDirection.Horizontal:
                            currentPos += cellSize.Width;
                            break;
                        default:
                            currentPos += cellSize.Height;
                            break;
                    }
                }
                m_vCellsPositions.Add(currentPos);//1 extra value allows us to get right/bottom of the last cell
            }
        }


        #region CCScrollViewDelegate Members

        public virtual void ScrollViewDidScroll(CCScrollView view)
        {
            var uCountOfItems = m_pDataSource.NumberOfCellsInTableView(this);
            if (uCountOfItems == 0)
            {
                return;
            }

            if (m_pTableViewDelegate != null)
            {
                m_pTableViewDelegate.ScrollViewDidScroll(this);
            }

            int startIdx = 0, endIdx = 0, idx = 0, maxIdx = 0;
            CCPoint offset = GetContentOffset() * -1;
            maxIdx = Math.Max(uCountOfItems - 1, 0);

            if (m_eVordering == CCTableViewVerticalFillOrder.FillTopDown)
            {
                offset.Y = offset.Y + m_tViewSize.Height / Container.ScaleY;
            }
            startIdx = _indexFromOffset(offset);
            if (startIdx == CCArrayForObjectSorting.CC_INVALID_INDEX)
            {
                startIdx = uCountOfItems - 1;
            }

            if (m_eVordering == CCTableViewVerticalFillOrder.FillTopDown)
            {
                offset.Y -= m_tViewSize.Height / Container.ScaleY;
            }
            else
            {
                offset.Y += m_tViewSize.Height / Container.ScaleY;
            }
            offset.X += m_tViewSize.Width / Container.ScaleX;

            endIdx = _indexFromOffset(offset);
            if (endIdx == CCArrayForObjectSorting.CC_INVALID_INDEX)
            {
                endIdx = uCountOfItems - 1;
            }

#if DEBUG_ // For Testing.
			int i = 0;
			foreach (object pObj in m_pCellsUsed)
			{
				var pCell = (CCTableViewCell)pObj;
				CCLog.Log("cells Used index {0}, value = {1}", i, pCell.getIdx());
				i++;
			}
			CCLog.Log("---------------------------------------");
			i = 0;
			foreach(object pObj in m_pCellsFreed)
			{
				var pCell = (CCTableViewCell)pObj;
				CCLog.Log("cells freed index {0}, value = {1}", i, pCell.getIdx());
				i++;
			}
			CCLog.Log("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
#endif

            if (m_pCellsUsed.Count > 0)
            {
                var cell = (CCTableViewCell) m_pCellsUsed[0];

                idx = cell.Index;
                while (idx < startIdx)
                {
                    _moveCellOutOfSight(cell);
                    if (m_pCellsUsed.Count > 0)
                    {
                        cell = (CCTableViewCell) m_pCellsUsed[0];
                        idx = cell.Index;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            if (m_pCellsUsed.Count > 0)
            {
                var cell = (CCTableViewCell) m_pCellsUsed[m_pCellsUsed.Count - 1];
                idx = cell.Index;

                while (idx <= maxIdx && idx > endIdx)
                {
                    _moveCellOutOfSight(cell);
                    if (m_pCellsUsed.Count > 0)
                    {
                        cell = (CCTableViewCell) m_pCellsUsed[m_pCellsUsed.Count - 1];
                        idx = cell.Index;
                    }
                    else
                    {
                        break;
                    }
                }
            }

            for (int i = startIdx; i <= endIdx; i++)
            {
                if (m_pIndices.Contains(i))
                {
                    continue;
                }
                UpdateCellAtIndex(i);
            }
        }

        public virtual void ScrollViewDidZoom(CCScrollView view)
        {
        }

        #endregion


        public override void TouchEnded(CCTouch pTouch, CCEvent pEvent)
        {
            if (!Visible)
            {
                return;
            }

            if (m_pTouchedCell != null)
            {
                CCRect bb = BoundingBox;
                bb.Origin = Parent.ConvertToWorldSpace(bb.Origin);

                if (bb.ContainsPoint(pTouch.Location) && m_pTableViewDelegate != null)
                {
                    m_pTableViewDelegate.TableCellUnhighlight(this, m_pTouchedCell);
                    m_pTableViewDelegate.TableCellTouched(this, m_pTouchedCell);
                }

                m_pTouchedCell = null;
            }

            base.TouchEnded(pTouch, pEvent);
        }

        public override bool TouchBegan(CCTouch pTouch, CCEvent pEvent)
        {
            if (!Visible)
            {
                return false;
            }
            
            bool touchResult = base.TouchBegan(pTouch, pEvent);

            if (m_pTouches.Count == 1)
            {
                var point = Container.ConvertTouchToNodeSpace(pTouch);
                var index = _indexFromOffset(point);
                if (index == CCArrayForObjectSorting.CC_INVALID_INDEX)
                {
                    m_pTouchedCell = null;
                }
                else
                {
                    m_pTouchedCell = CellAtIndex(index); 
                }

                if (m_pTouchedCell != null && m_pTableViewDelegate != null)
                {
                    m_pTableViewDelegate.TableCellHighlight(this, m_pTouchedCell);
                }
            }
            else if (m_pTouchedCell != null)
            {
                if (m_pTableViewDelegate != null)
                {
                    m_pTableViewDelegate.TableCellUnhighlight(this, m_pTouchedCell);
                }

                m_pTouchedCell = null;
            }

            return touchResult;
        }

        public override void TouchMoved(CCTouch touch, CCEvent pEvent)
        {
            base.TouchMoved(touch, pEvent);

            if (m_pTouchedCell != null && IsTouchMoved)
            {
                if (m_pTableViewDelegate != null)
                {
                    m_pTableViewDelegate.TableCellUnhighlight(this, m_pTouchedCell);
                }

                m_pTouchedCell = null;
            }
        }

        public override void TouchCancelled(CCTouch touch, CCEvent pEvent)
        {
            base.TouchCancelled(touch, pEvent);

            if (m_pTouchedCell != null)
            {
                if (m_pTableViewDelegate != null)
                {
                    m_pTableViewDelegate.TableCellUnhighlight(this, m_pTouchedCell);
                }

                m_pTouchedCell = null;
            }
        }
    }
}