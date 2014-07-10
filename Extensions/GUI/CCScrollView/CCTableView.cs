using System;
using System.Collections.Generic;

namespace CocosSharp
{
    public enum CCTableViewVerticalFillOrder
    {
        FillTopDown,
        FillBottomUp
    };
		
    // Sole purpose of this delegate is to single touch event in this version.
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
        CCTableViewCell touchedCell;
		CCTableViewVerticalFillOrder vordering;

		CCArrayForObjectSorting cellsUsed;
        CCArrayForObjectSorting cellsFreed;
        
		CCScrollViewDirection oldDirection;

		List<int> indices;
		List<float> cellsPositions;


		#region Properties

		public ICCTableViewDataSource DataSource { get; set; }
		public new ICCTableViewDelegate Delegate { get; set; }

		// Determines how cell is ordered and filled in the view.
        public CCTableViewVerticalFillOrder VerticalFillOrder
        {
            get { return vordering; }
            set
            {
                if (vordering != value)
                {
                    vordering = value;
                    if (cellsUsed.Count > 0)
                    {
                        ReloadData();
                    }
                }
            }
        }

		#endregion Properties


        #region Constructors

        public CCTableView()
        {
            oldDirection = CCScrollViewDirection.None;
        }

        public CCTableView(ICCTableViewDataSource dataSource, CCSize size)
            : this(dataSource, size, null)
        {
		}

        public CCTableView(ICCTableViewDataSource dataSource, CCSize size, CCNode container) : base(size, container)
        {
            cellsPositions = new List<float>();
            cellsUsed = new CCArrayForObjectSorting();
            cellsFreed = new CCArrayForObjectSorting();
            indices = new List<int>();
            vordering = CCTableViewVerticalFillOrder.FillBottomUp;
            Direction = CCScrollViewDirection.Vertical;

            base.Delegate = this;

			DataSource = dataSource;
			UpdateCellPositions();
			UpdateContentSize();
        }

        #endregion Constructors


		// reloads data from data source.  the view will be refreshed.
        public void ReloadData()
        {
            oldDirection = CCScrollViewDirection.None;

            foreach (CCTableViewCell cell in cellsUsed)
            {
                if (Delegate != null)
                {
                    Delegate.TableCellWillRecycle(this, cell);
                }

                cellsFreed.Add(cell);
                cell.Reset();
                if (cell.Parent == Container)
                {
                    Container.RemoveChild(cell, true);
                }
            }

            indices.Clear();
            cellsUsed = new CCArrayForObjectSorting();

            UpdateCellPositions();
            UpdateContentSize();
            if (DataSource.NumberOfCellsInTableView(this) > 0)
            {
                ScrollViewDidScroll(this);
            }
        }

		#region Cell management

        /**
         * Returns an existing cell at a given index. Returns nil if a cell is nonexistent at the moment of query.
         *
         * @param idx index
         * @return a cell at a given index
         */
        public CCTableViewCell CellAtIndex(int idx)
        {
            CCTableViewCell found = null;

            if (indices.Contains(idx))
            {
                found = (CCTableViewCell)cellsUsed.ObjectWithObjectID(idx);
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

            var uCountOfItems = DataSource.NumberOfCellsInTableView(this);
            if (uCountOfItems == 0 || idx > uCountOfItems - 1)
            {
                return;
            }

            var cell = CellAtIndex(idx);
            if (cell != null)
            {
                MoveCellOutOfSight(cell);
            }
            cell = DataSource.TableCellAtIndex(this, idx);
            SetIndexForCell(idx, cell);
            AddCellIfNecessary(cell);
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

            var uCountOfItems = DataSource.NumberOfCellsInTableView(this);
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

            newIdx = cellsUsed.IndexOfSortedObject(cell);

            //remove first
            MoveCellOutOfSight(cell);

            indices.Remove(idx);
            //    [indices shiftIndexesStartingAtIndex:idx+1 by:-1];
            for (int i = cellsUsed.Count - 1; i > newIdx; i--)
            {
                cell = (CCTableViewCell)cellsUsed[i];
                SetIndexForCell(cell.Index - 1, cell);
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

            if (cellsFreed.Count == 0)
            {
                cell = null;
            }
            else
            {
                cell = (CCTableViewCell)cellsFreed[0];
                cellsFreed.RemoveAt(0);
            }
            return cell;
        }

		void AddCellIfNecessary(CCTableViewCell cell)
        {
            if (cell.Parent != Container)
            {
                Container.AddChild(cell);
            }
            cellsUsed.InsertSortedObject(cell);
            indices.Add(cell.Index);
        }

		void MoveCellOutOfSight(CCTableViewCell cell)
		{
			if (Delegate != null)
			{
				Delegate.TableCellWillRecycle(this, cell);
			}

			cellsFreed.Add(cell);
			cellsUsed.RemoveSortedObject(cell);
			indices.Remove(cell.Index);
			cell.Reset();
			if (cell.Parent == Container)
			{
				Container.RemoveChild(cell, true);
			}
		}

		void SetIndexForCell(int index, CCTableViewCell cell)
		{
			cell.AnchorPoint = CCPoint.Zero;
			cell.Position = OffsetFromIndex(index);
			cell.Index = index;
		}

		void UpdateCellPositions() 
		{
			int cellsCount = DataSource.NumberOfCellsInTableView(this);
			cellsPositions.Clear();

			if (cellsCount > 0)
			{
				float currentPos = 0;
				CCSize cellSize;
				for (int i=0; i < cellsCount; i++)
				{
					cellsPositions.Add(currentPos);
					cellSize = DataSource.TableCellSizeForIndex(this, i);
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
				cellsPositions.Add(currentPos);//1 extra value allows us to get right/bottom of the last cell
			}
		}

		#endregion Cell management


		void UpdateContentSize()
        {
            CCSize size = CCSize.Zero;
            int cellsCount = DataSource.NumberOfCellsInTableView(this);

            if (cellsCount > 0)
            {
                float maxPosition = cellsPositions[cellsCount];

                switch (Direction)
                {
                    case CCScrollViewDirection.Horizontal:
                        size = new CCSize(maxPosition, ViewSize.Height);
                        break;
                    default:
                        size = new CCSize(ViewSize.Width, maxPosition);
                        break;
                }
            }

            ContentSize = size;

            if (oldDirection != Direction)
            {
                if (Direction == CCScrollViewDirection.Horizontal)
                {
                    SetContentOffset(CCPoint.Zero);
                }
                else
                {
                    SetContentOffset(new CCPoint(0, MinContainerOffset.Y));
                }
                oldDirection = Direction;
            }
        }

		CCPoint OffsetFromIndex(int index)
        {
			CCPoint offset;

			switch (Direction)
			{
			case CCScrollViewDirection.Horizontal:
				offset = new CCPoint(cellsPositions[index], 0.0f);
				break;
			default:
				offset = new CCPoint(0.0f, cellsPositions[index]);
				break;
			}

            CCSize cellSize = DataSource.TableCellSizeForIndex(this, index);
            if (vordering == CCTableViewVerticalFillOrder.FillTopDown)
            {
                offset.Y = Container.ContentSize.Height - offset.Y - cellSize.Height;
            }
            return offset;
        }

		int IndexFromOffset(CCPoint offset)
        {
            int index = 0;
            int maxIdx = DataSource.NumberOfCellsInTableView(this) - 1;

            if (vordering == CCTableViewVerticalFillOrder.FillTopDown)
            {
                offset.Y = Container.ContentSize.Height - offset.Y;
            }
			index = PrimitiveIndexFromOffset(offset);
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

		int PrimitiveIndexFromOffset(CCPoint offset)
        {
            int low = 0;
            int high = DataSource.NumberOfCellsInTableView(this) - 1;
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
                float cellStart = cellsPositions[index];
                float cellEnd = cellsPositions[index + 1];

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



        #region CCScrollViewDelegate Members

        public virtual void ScrollViewDidScroll(CCScrollView view)
        {
            var uCountOfItems = DataSource.NumberOfCellsInTableView(this);
            if (uCountOfItems == 0)
            {
                return;
            }

            if (Delegate != null)
            {
                Delegate.ScrollViewDidScroll(this);
            }

            int startIdx = 0, endIdx = 0, idx = 0, maxIdx = 0;
            CCPoint offset = ContentOffset * -1;
            maxIdx = Math.Max(uCountOfItems - 1, 0);

            if (vordering == CCTableViewVerticalFillOrder.FillTopDown)
            {
                offset.Y = offset.Y + ViewSize.Height / Container.ScaleY;
            }
            startIdx = IndexFromOffset(offset);
            if (startIdx == CCArrayForObjectSorting.CC_INVALID_INDEX)
            {
                startIdx = uCountOfItems - 1;
            }

            if (vordering == CCTableViewVerticalFillOrder.FillTopDown)
            {
                offset.Y -= ViewSize.Height / Container.ScaleY;
            }
            else
            {
                offset.Y += ViewSize.Height / Container.ScaleY;
            }
            offset.X += ViewSize.Width / Container.ScaleX;

            endIdx = IndexFromOffset(offset);
            if (endIdx == CCArrayForObjectSorting.CC_INVALID_INDEX)
            {
                endIdx = uCountOfItems - 1;
            }

#if DEBUG_ // For Testing.
			int i = 0;
			foreach (object pObj in cellsUsed)
			{
				var pCell = (CCTableViewCell)pObj;
				CCLog.Log("cells Used index {0}, value = {1}", i, pCell.getIdx());
				i++;
			}
			CCLog.Log("---------------------------------------");
			i = 0;
			foreach(object pObj in cellsFreed)
			{
				var pCell = (CCTableViewCell)pObj;
				CCLog.Log("cells freed index {0}, value = {1}", i, pCell.getIdx());
				i++;
			}
			CCLog.Log("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
#endif

            if (cellsUsed.Count > 0)
            {
                var cell = (CCTableViewCell) cellsUsed[0];

                idx = cell.Index;
                while (idx < startIdx)
                {
					MoveCellOutOfSight(cell);
                    if (cellsUsed.Count > 0)
                    {
                        cell = (CCTableViewCell) cellsUsed[0];
                        idx = cell.Index;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            if (cellsUsed.Count > 0)
            {
                var cell = (CCTableViewCell) cellsUsed[cellsUsed.Count - 1];
                idx = cell.Index;

                while (idx <= maxIdx && idx > endIdx)
                {
                    MoveCellOutOfSight(cell);
                    if (cellsUsed.Count > 0)
                    {
                        cell = (CCTableViewCell) cellsUsed[cellsUsed.Count - 1];
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
                if (indices.Contains(i))
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


		#region Event handling

        public void TouchEnded(CCTouch pTouch)
        {
            if (!Visible)
            {
                return;
            }

            if (touchedCell != null)
            {
                CCRect bb = BoundingBox;

                if (bb.ContainsPoint(Scene.ScreenToWorldspace(pTouch.LocationOnScreen)) && Delegate != null)
                {
                    Delegate.TableCellUnhighlight(this, touchedCell);
                    Delegate.TableCellTouched(this, touchedCell);
                }

                touchedCell = null;
            }

        }

//        public bool TouchBegan(CCTouch pTouch)
//        {
//            if (!Visible)
//            {
//                return false;
//            }
//            
//			//bool touchResult = base.TouchBegan(pTouch);
//
//            if (_touches.Count == 1)
//            {
//                var point = Container.ConvertTouchToNodeSpace(pTouch);
//                var index = IndexFromOffset(point);
//                if (index == CCArrayForObjectSorting.CC_INVALID_INDEX)
//                {
//                    touchedCell = null;
//                }
//                else
//                {
//                    touchedCell = CellAtIndex(index); 
//                }
//
//                if (touchedCell != null && Delegate != null)
//                {
//                    Delegate.TableCellHighlight(this, touchedCell);
//                }
//            }
//            else if (touchedCell != null)
//            {
//                if (Delegate != null)
//                {
//                    Delegate.TableCellUnhighlight(this, touchedCell);
//                }
//
//                touchedCell = null;
//            }
//
//            return touchResult;
//        }
//
//        public void TouchMoved(CCTouch touch)
//        {
//            base.TouchMoved(touch);
//
//            if (touchedCell != null && IsTouchMoved)
//            {
//                if (Delegate != null)
//                {
//                    Delegate.TableCellUnhighlight(this, touchedCell);
//                }
//
//                touchedCell = null;
//            }
//        }
//
//        public void TouchCancelled(CCTouch touch)
//        {
//            base.TouchCancelled(touch);
//
//            if (touchedCell != null)
//            {
//                if (Delegate != null)
//                {
//                    Delegate.TableCellUnhighlight(this, touchedCell);
//                }
//
//                touchedCell = null;
//            }
//        }
    
		#endregion Event handling
	}
}