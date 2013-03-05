using System;
using System.Collections.Generic;

namespace cocos2d
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
        CCSize CellSizeForTable(CCTableView table);
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
        protected CCScrollViewDirection m_eOldDirection;
        protected CCTableViewVerticalFillOrder m_eVordering;
        protected CCArrayForObjectSorting m_pCellsFreed;
        protected CCArrayForObjectSorting m_pCellsUsed;
        protected CCTableViewDataSource m_pDataSource;
        protected List<int> m_pIndices;
        protected CCTableViewDelegate m_pTableViewDelegate;

        public CCTableView()
        {
            m_eOldDirection = CCScrollViewDirection.None;
        }

        #region CCScrollViewDelegate Members

        public virtual void scrollViewDidScroll(CCScrollView view)
        {
            var uCountOfItems = m_pDataSource.NumberOfCellsInTableView(this);
            if (uCountOfItems == 0)
            {
                return;
            }

            int startIdx = 0, endIdx = 0, idx = 0, maxIdx = 0;

            CCPoint offset = CCPointExtension.ccpMult(GetContentOffset(), -1);
            maxIdx = Math.Max(uCountOfItems - 1, 0);

            CCSize cellSize = m_pDataSource.CellSizeForTable(this);

            if (m_eVordering == CCTableViewVerticalFillOrder.FillTopDown)
            {
                offset.y = offset.y + m_tViewSize.Height / Container.ScaleY - cellSize.Height;
            }
            startIdx = _indexFromOffset(offset);

            if (m_eVordering == CCTableViewVerticalFillOrder.FillTopDown)
            {
                offset.y -= m_tViewSize.Height / Container.ScaleY;
            }
            else
            {
                offset.y += m_tViewSize.Height / Container.ScaleY;
            }
            offset.x += m_tViewSize.Width / Container.ScaleX;

            endIdx = _indexFromOffset(offset);

#if DEBUG_ // For Testing.
			int i = 0;
			foreach (CCObject pObj in m_pCellsUsed)
			{
				var pCell = (CCTableViewCell)pObj;
				CCLog.Log("cells Used index {0}, value = {1}", i, pCell.getIdx());
				i++;
			}
			CCLog.Log("---------------------------------------");
			i = 0;
			foreach(CCObject pObj in m_pCellsFreed)
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

                idx = cell.getIdx();
                while (idx < startIdx)
                {
                    _moveCellOutOfSight(cell);
                    if (m_pCellsUsed.Count > 0)
                    {
                        cell = (CCTableViewCell) m_pCellsUsed[0];
                        idx = cell.getIdx();
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
                idx = cell.getIdx();

                while (idx <= maxIdx && idx > endIdx)
                {
                    _moveCellOutOfSight(cell);
                    if (m_pCellsUsed.Count > 0)
                    {
                        cell = (CCTableViewCell) m_pCellsUsed[m_pCellsUsed.Count - 1];
                        idx = cell.getIdx();
                    }
                    else
                    {
                        break;
                    }
                }
            }

            for (int j = startIdx; j <= endIdx; j++)
            {
                if (m_pIndices.Contains(j))
                {
                    continue;
                }
                UpdateCellAtIndex(j);
            }
        }

        public virtual void scrollViewDidZoom(CCScrollView view)
        {
        }

        #endregion

        /**
         * An intialized table view object
         *
         * @param dataSource data source
         * @param size view size
         * @return table view
         */

        public static CCTableView Create(CCTableViewDataSource dataSource, CCSize size)
        {
            return Create(dataSource, size, null);
        }

        /**
         * An initialized table view object
         *
         * @param dataSource data source;
         * @param size view size
         * @param container parent object for cells
         * @return table view
         */

        public static CCTableView Create(CCTableViewDataSource dataSource, CCSize size, CCNode container)
        {
            var table = new CCTableView();
            table.InitWithViewSize(size, container);
            table.DataSource = dataSource;
            table._updateContentSize();
            return table;
        }

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

        /**
         * determines how cell is ordered and filled in the view.
         */

        public CCTableViewVerticalFillOrder VerticalFillOrder
        {
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
            get { return m_eVordering; }
        }


        public new bool InitWithViewSize(CCSize size, CCNode container)
        {
            if (base.InitWithViewSize(size, container))
            {
                m_pCellsUsed = new CCArrayForObjectSorting();
                m_pCellsFreed = new CCArrayForObjectSorting();
                m_pIndices = new List<int>();
                m_pTableViewDelegate = null;
                m_eVordering = CCTableViewVerticalFillOrder.FillBottomUp;
                Direction = CCScrollViewDirection.Vertical;


                base.Delegate = this;
                return true;
            }
            return false;
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

            CCTableViewCell cell = _cellWithIndex(idx);
            if (cell != null)
            {
                _moveCellOutOfSight(cell);
            }
            cell = m_pDataSource.TableCellAtIndex(this, idx);
            _setIndexForCell(idx, cell);
            _addCellIfNecessary(cell);
        }

        /**
         * Inserts a new cell at a given index
         *
         * @param idx location to insert
         */

        public void InsertCellAtIndex(int idx)
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
            var cell = (CCTableViewCell) m_pCellsUsed.ObjectWithObjectID(idx);
            if (cell != null)
            {
                newIdx = m_pCellsUsed.IndexOfSortedObject(cell);
                for (int i = newIdx; i < m_pCellsUsed.Count; i++)
                {
                    cell = (CCTableViewCell) m_pCellsUsed[i];
                    _setIndexForCell(cell.getIdx() + 1, cell);
                }
            }

            //   [m_pIndices shiftIndexesStartingAtIndex:idx by:1];

            //insert a new cell
            cell = m_pDataSource.TableCellAtIndex(this, idx);
            _setIndexForCell(idx, cell);
            _addCellIfNecessary(cell);

            _updateContentSize();
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
            CCTableViewCell cell = _cellWithIndex(idx);
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
                cell = (CCTableViewCell) m_pCellsUsed[i];
                _setIndexForCell(cell.getIdx() - 1, cell);
            }
        }

        /**
         * reloads data from data source.  the view will be refreshed.
         */

        public void ReloadData()
        {
            foreach (CCTableViewCell cell in m_pCellsUsed)
            {
                m_pCellsFreed.Add(cell);
                cell.Reset();
                if (cell.Parent == Container)
                {
                    Container.RemoveChild(cell, true);
                }
            }

            m_pIndices.Clear();
            m_pCellsUsed = new CCArrayForObjectSorting();

            _updateContentSize();
            if (m_pDataSource.NumberOfCellsInTableView(this) > 0)
            {
                scrollViewDidScroll(this);
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
                cell = (CCTableViewCell) m_pCellsFreed[0];
                m_pCellsFreed.RemoveAt(0);
            }
            return cell;
        }

        /**
         * Returns an existing cell at a given index. Returns nil if a cell is nonexistent at the moment of query.
         *
         * @param idx index
         * @return a cell at a given index
         */

        public CCTableViewCell CellAtIndex(int idx)
        {
            return _cellWithIndex(idx);
        }


        public override void TouchEnded(CCTouch pTouch, CCEvent pEvent)
        {
            if (!Visible)
            {
                return;
            }
            if (m_pTouches.Count == 1 && !IsTouchMoved)
            {
                int index;
                CCTableViewCell cell;

                CCPoint point = Container.ConvertTouchToNodeSpace(pTouch);

                if (m_eVordering == CCTableViewVerticalFillOrder.FillTopDown)
                {
                    CCSize cellSize = m_pDataSource.CellSizeForTable(this);
                    point.y -= cellSize.Height;
                }
                index = _indexFromOffset(point);
                cell = _cellWithIndex(index);

                if (cell != null)
                {
                    m_pTableViewDelegate.TableCellTouched(this, cell);
                }
            }
            base.TouchEnded(pTouch, pEvent);
        }


        /**
         * vertical direction of cell filling
         */

        protected int __indexFromOffset(CCPoint offset)
        {
            int index;

            CCSize cellSize = m_pDataSource.CellSizeForTable(this);

            switch (Direction)
            {
                case CCScrollViewDirection.Horizontal:
                    index = (int) (offset.x / cellSize.Width);
                    break;
                default:
                    index = (int) (offset.y / cellSize.Height);
                    break;
            }

            return index;
        }

        protected int _indexFromOffset(CCPoint offset)
        {
            int index = 0;
            int maxIdx = m_pDataSource.NumberOfCellsInTableView(this) - 1;

            CCSize cellSize = m_pDataSource.CellSizeForTable(this);
            if (m_eVordering == CCTableViewVerticalFillOrder.FillTopDown)
            {
                offset.y = Container.ContentSize.Height - offset.y - cellSize.Height;
            }
            index = Math.Max(0, __indexFromOffset(offset));
            index = Math.Min(index, maxIdx);

            return index;
        }

        protected CCPoint __offsetFromIndex(int index)
        {
            CCPoint offset;
            CCSize cellSize;

            cellSize = m_pDataSource.CellSizeForTable(this);
            switch (Direction)
            {
                case CCScrollViewDirection.Horizontal:
                    offset = new CCPoint(cellSize.Width * index, 0.0f);
                    break;
                default:
                    offset = new CCPoint(0.0f, cellSize.Height * index);
                    break;
            }

            return offset;
        }

        protected CCPoint _offsetFromIndex(int index)
        {
            CCPoint offset = __offsetFromIndex(index);

            CCSize cellSize = m_pDataSource.CellSizeForTable(this);
            if (m_eVordering == CCTableViewVerticalFillOrder.FillTopDown)
            {
                offset.y = Container.ContentSize.Height - offset.y - cellSize.Height;
            }
            return offset;
        }

        protected void _updateContentSize()
        {
            CCSize size, cellSize;
            int cellCount;

            cellSize = m_pDataSource.CellSizeForTable(this);
            cellCount = m_pDataSource.NumberOfCellsInTableView(this);

            switch (Direction)
            {
                case CCScrollViewDirection.Horizontal:
                    size = new CCSize(cellCount * cellSize.Width, cellSize.Height);
                    break;
                default:
                    size = new CCSize(cellSize.Width, cellCount * cellSize.Height);
                    break;
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
                    SetContentOffset(new CCPoint(0, MinContainerOffset.y));
                }
                m_eOldDirection = m_eDirection;
            }
        }

        protected CCTableViewCell _cellWithIndex(int cellIndex)
        {
            CCTableViewCell found = null;

            if (m_pIndices.Contains(cellIndex))
            {
                found = (CCTableViewCell) m_pCellsUsed.ObjectWithObjectID(cellIndex);
            }

            return found;
        }

        protected void _moveCellOutOfSight(CCTableViewCell cell)
        {
            m_pCellsFreed.Add(cell);
            m_pCellsUsed.RemoveSortedObject(cell);
            m_pIndices.Remove(cell.getIdx());
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
            cell.setIdx(index);
        }

        protected void _addCellIfNecessary(CCTableViewCell cell)
        {
            if (cell.Parent != Container)
            {
                Container.AddChild(cell);
            }
            m_pCellsUsed.InsertSortedObject(cell);

            if (!m_pIndices.Contains(cell.getIdx()))
                m_pIndices.Add(cell.getIdx());
        }
    }
}