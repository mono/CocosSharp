using System;

namespace Cocos2D
{
	/**
	 * Abstract class for SWTableView cell node
	 */
	public class CCTableViewCell: CCNode, ICCSortableObject
	{
		public CCTableViewCell() {}
		/**
		 * The index used internally by SWTableView and its subclasses
		 */
        public int Index 
        {
            get { return _idx; }
            set { _idx = value; }
        }

		/**
		 * Cleans up any resources linked to this cell and resets <code>idx</code> property.
		 */
		public void Reset()
		{
			_idx = CCArrayForObjectSorting.CC_INVALID_INDEX;
		}

	    public int ObjectID
	    {
	        set { _idx = value; }
	        get { return _idx; }
	    }

	    private int _idx;
	}
}