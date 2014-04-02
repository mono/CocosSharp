using System;

namespace CocosSharp
{
	/**
	 * Abstract class for SWTableView cell node
	 */
	public class CCTableViewCell: CCNode, ICCSortableObject
	{
        int _idx;

        /**
         * The index used internally by SWTableView and its subclasses
         */
        public int Index 
        {
            get { return _idx; }
            set { _idx = value; }
        }

        public int ObjectID
        {
            set { _idx = value; }
            get { return _idx; }
        }


        #region Constructors

        public CCTableViewCell() {}

        #endregion Constructors


		/**
		 * Cleans up any resources linked to this cell and resets <code>idx</code> property.
		 */
		public void Reset()
		{
			_idx = CCArrayForObjectSorting.CC_INVALID_INDEX;
		}
	}
}