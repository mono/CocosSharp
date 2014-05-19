using System;

namespace CocosSharp
{
	/**
	 * Abstract class for SWTableView cell node
	 */
	public class CCTableViewCell: CCNode, ICCSortableObject
	{
        int idx;

        /**
         * The index used internally by SWTableView and its subclasses
         */
		public int Index
        {
			get { return idx; }
			set { idx = value; }
        }

        public int ObjectID
        {
			set { idx = value; }
			get { return idx; }
        }


        #region Constructors

        public CCTableViewCell() {}

        #endregion Constructors


		/**
		 * Cleans up any resources linked to this cell and resets <code>idx</code> property.
		 */
		public void Reset()
		{
			idx = CCArrayForObjectSorting.CC_INVALID_INDEX;
		}
	}
}