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
    public interface ICCTableViewDelegate : ICCScrollViewDelegate
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

    public interface ICCTableViewDataSource
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
    public class CCTableView : CCScrollView, ICCScrollViewDelegate
    {
        protected CCTableViewCell _touchedCell;
        protected CCTableViewVerticalFillOrder _vordering;
        protected List<int> _indices;
        protected List<float> _cellsPositions;
        protected CCArrayForObjectSorting _cellsUsed;
        protected CCArrayForObjectSorting _cellsFreed;
        protected ICCTableViewDataSource _dataSource;
        protected ICCTableViewDelegate _tableViewDelegate;
        protected CCScrollViewDirection _oldDirection;

        /**
         * data source
         */
        public ICCTableViewDataSource DataSource
        {
            get { return _dataSource; }
            set { _dataSource = value; }
        }

        /**
         * delegate
         */
        public new ICCTableViewDelegate Delegate
        {
            get { return _tableViewDelegate; }
            set { _tableViewDelegate = value; }
        }

        public CCTableView()
        {
            _oldDirection = CCScrollViewDirection.None;
        }

        /**
         * An intialized table view object
         *
         * @param dataSource data source
         * @param size view size
         * @return table view
         */
        public CCTableView(ICCTableViewDataSource dataSource, CCSize size)
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
        public CCTableView(ICCTableViewDataSource dataSource, CCSize size, CCNode container)
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
                _cellsPositions = new List<float>();
                _cellsUsed = new CCArrayForObjectSorting();
                _cellsFreed = new CCArrayForObjectSorting();
                _indices = new List<int>();
                _vordering = CCTableViewVerticalFillOrder.FillBottomUp;
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
            get { return _vordering; }
            set
            {
                if (_vordering != value)
                {
                    _vordering = value;
                    if (_cellsUsed.Count > 0)
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
            _oldDirection = CCScrollViewDirection.None;

            foreach (CCTableViewCell cell in _cellsUsed)
            {
                if (_tableViewDelegate != null)
                {
                    _tableViewDelegate.TableCellWillRecycle(this, cell);
                }

                _cellsFreed.Add(cell);
                cell.Reset();
                if (cell.Parent == Container)
                {
                    Container.RemoveChild(cell, true);
                }
            }

            _indices.Clear();
            _cellsUsed = new CCArrayForObjectSorting();

            _updateCellPositions();
            _updateContentSize();
            if (_dataSource.NumberOfCellsInTableView(this) > 0)
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

            if (_indices.Contains(idx))
            {
                found = (CCTableViewCell)_cellsUsed.ObjectWithObjectID(idx);
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

            var uCountOfItems = _dataSource.NumberOfCellsInTableView(this);
            if (uCountOfItems == 0 || idx > uCountOfItems - 1)
            {
                return;
            }

            var cell = CellAtIndex(idx);
            if (cell != null)
            {
                _moveCellOutOfSight(cell);
            }
            cell = _dataSource.TableCellAtIndex(this, idx);
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

            var uCountOfItems = _dataSource.NumberOfCellsInTableView(this);
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

            newIdx = _cellsUsed.IndexOfSortedObject(cell);

            //remove first
            _moveCellOutOfSight(cell);

            _indices.Remove(idx);
            //    [_indices shiftIndexesStartingAtIndex:idx+1 by:-1];
            for (int i = _cellsUsed.Count - 1; i > newIdx; i--)
            {
                cell = (CCTableViewCell)_cellsUsed[i];
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

            if (_cellsFreed.Count == 0)
            {
                cell = null;
            }
            else
            {
                cell = (CCTableViewCell)_cellsFreed[0];
                _cellsFreed.RemoveAt(0);
            }
            return cell;
        }

        protected void _addCellIfNecessary(CCTableViewCell cell)
        {
            if (cell.Parent != Container)
            {
                Container.AddChild(cell);
            }
            _cellsUsed.InsertSortedObject(cell);
            _indices.Add(cell.Index);
        }

        protected void _updateContentSize()
        {
            CCSize size = CCSize.Zero;
            int cellsCount = _dataSource.NumberOfCellsInTableView(this);

            if (cellsCount > 0)
            {
                float maxPosition = _cellsPositions[cellsCount];

                switch (Direction)
                {
                    case CCScrollViewDirection.Horizontal:
                        size = new CCSize(maxPosition, _viewSize.Height);
                        break;
                    default:
                        size = new CCSize(_viewSize.Width, maxPosition);
                        break;
                }
            }

            ContentSize = size;

            if (_oldDirection != _direction)
            {
                if (_direction == CCScrollViewDirection.Horizontal)
                {
                    SetContentOffset(CCPoint.Zero);
                }
                else
                {
                    SetContentOffset(new CCPoint(0, MinContainerOffset.Y));
                }
                _oldDirection = _direction;
            }
        }

        protected CCPoint _offsetFromIndex(int index)
        {
            CCPoint offset = __offsetFromIndex(index);

            CCSize cellSize = _dataSource.TableCellSizeForIndex(this, index);
            if (_vordering == CCTableViewVerticalFillOrder.FillTopDown)
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
                    offset = new CCPoint(_cellsPositions[index], 0.0f);
                    break;
                default:
                    offset = new CCPoint(0.0f, _cellsPositions[index]);
                    break;
            }

            return offset;
        }

        protected int _indexFromOffset(CCPoint offset)
        {
            int index = 0;
            int maxIdx = _dataSource.NumberOfCellsInTableView(this) - 1;

            if (_vordering == CCTableViewVerticalFillOrder.FillTopDown)
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
            int high = _dataSource.NumberOfCellsInTableView(this) - 1;
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
                float cellStart = _cellsPositions[index];
                float cellEnd = _cellsPositions[index + 1];

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
            if (_tableViewDelegate != null)
            {
                _tableViewDelegate.TableCellWillRecycle(this, cell);
            }

            _cellsFreed.Add(cell);
            _cellsUsed.RemoveSortedObject(cell);
            _indices.Remove(cell.Index);
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
            int cellsCount = _dataSource.NumberOfCellsInTableView(this);
            _cellsPositions.Clear();

            if (cellsCount > 0)
            {
                float currentPos = 0;
                CCSize cellSize;
                for (int i=0; i < cellsCount; i++)
                {
                    _cellsPositions.Add(currentPos);
                    cellSize = _dataSource.TableCellSizeForIndex(this, i);
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
                _cellsPositions.Add(currentPos);//1 extra value allows us to get right/bottom of the last cell
            }
        }


        #region CCScrollViewDelegate Members

        public virtual void ScrollViewDidScroll(CCScrollView view)
        {
            var uCountOfItems = _dataSource.NumberOfCellsInTableView(this);
            if (uCountOfItems == 0)
            {
                return;
            }

            if (_tableViewDelegate != null)
            {
                _tableViewDelegate.ScrollViewDidScroll(this);
            }

            int startIdx = 0, endIdx = 0, idx = 0, maxIdx = 0;
            CCPoint offset = GetContentOffset() * -1;
            maxIdx = Math.Max(uCountOfItems - 1, 0);

            if (_vordering == CCTableViewVerticalFillOrder.FillTopDown)
            {
                offset.Y = offset.Y + _viewSize.Height / Container.ScaleY;
            }
            startIdx = _indexFromOffset(offset);
            if (startIdx == CCArrayForObjectSorting.CC_INVALID_INDEX)
            {
                startIdx = uCountOfItems - 1;
            }

            if (_vordering == CCTableViewVerticalFillOrder.FillTopDown)
            {
                offset.Y -= _viewSize.Height / Container.ScaleY;
            }
            else
            {
                offset.Y += _viewSize.Height / Container.ScaleY;
            }
            offset.X += _viewSize.Width / Container.ScaleX;

            endIdx = _indexFromOffset(offset);
            if (endIdx == CCArrayForObjectSorting.CC_INVALID_INDEX)
            {
                endIdx = uCountOfItems - 1;
            }

#if DEBUG_ // For Testing.
			int i = 0;
			foreach (object pObj in _cellsUsed)
			{
				var pCell = (CCTableViewCell)pObj;
				CCLog.Log("cells Used index {0}, value = {1}", i, pCell.getIdx());
				i++;
			}
			CCLog.Log("---------------------------------------");
			i = 0;
			foreach(object pObj in _cellsFreed)
			{
				var pCell = (CCTableViewCell)pObj;
				CCLog.Log("cells freed index {0}, value = {1}", i, pCell.getIdx());
				i++;
			}
			CCLog.Log("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
#endif

            if (_cellsUsed.Count > 0)
            {
                var cell = (CCTableViewCell) _cellsUsed[0];

                idx = cell.Index;
                while (idx < startIdx)
                {
                    _moveCellOutOfSight(cell);
                    if (_cellsUsed.Count > 0)
                    {
                        cell = (CCTableViewCell) _cellsUsed[0];
                        idx = cell.Index;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            if (_cellsUsed.Count > 0)
            {
                var cell = (CCTableViewCell) _cellsUsed[_cellsUsed.Count - 1];
                idx = cell.Index;

                while (idx <= maxIdx && idx > endIdx)
                {
                    _moveCellOutOfSight(cell);
                    if (_cellsUsed.Count > 0)
                    {
                        cell = (CCTableViewCell) _cellsUsed[_cellsUsed.Count - 1];
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
                if (_indices.Contains(i))
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


        public override void TouchEnded(CCTouch pTouch)
        {
            if (!Visible)
            {
                return;
            }

            if (_touchedCell != null)
            {
                CCRect bb = BoundingBox;
                bb.Origin = Parent.ConvertToWorldSpace(bb.Origin);

                if (bb.ContainsPoint(pTouch.Location) && _tableViewDelegate != null)
                {
                    _tableViewDelegate.TableCellUnhighlight(this, _touchedCell);
                    _tableViewDelegate.TableCellTouched(this, _touchedCell);
                }

                _touchedCell = null;
            }

            base.TouchEnded(pTouch);
        }

        public override bool TouchBegan(CCTouch pTouch)
        {
            if (!Visible)
            {
                return false;
            }
            
            bool touchResult = base.TouchBegan(pTouch);

            if (_touches.Count == 1)
            {
                var point = Container.ConvertTouchToNodeSpace(pTouch);
                var index = _indexFromOffset(point);
                if (index == CCArrayForObjectSorting.CC_INVALID_INDEX)
                {
                    _touchedCell = null;
                }
                else
                {
                    _touchedCell = CellAtIndex(index); 
                }

                if (_touchedCell != null && _tableViewDelegate != null)
                {
                    _tableViewDelegate.TableCellHighlight(this, _touchedCell);
                }
            }
            else if (_touchedCell != null)
            {
                if (_tableViewDelegate != null)
                {
                    _tableViewDelegate.TableCellUnhighlight(this, _touchedCell);
                }

                _touchedCell = null;
            }

            return touchResult;
        }

        public override void TouchMoved(CCTouch touch)
        {
            base.TouchMoved(touch);

            if (_touchedCell != null && IsTouchMoved)
            {
                if (_tableViewDelegate != null)
                {
                    _tableViewDelegate.TableCellUnhighlight(this, _touchedCell);
                }

                _touchedCell = null;
            }
        }

        public override void TouchCancelled(CCTouch touch)
        {
            base.TouchCancelled(touch);

            if (_touchedCell != null)
            {
                if (_tableViewDelegate != null)
                {
                    _tableViewDelegate.TableCellUnhighlight(this, _touchedCell);
                }

                _touchedCell = null;
            }
        }
    }
}