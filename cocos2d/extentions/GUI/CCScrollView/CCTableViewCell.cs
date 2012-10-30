using System;

namespace cocos2d
{
	/**
	 * Abstract class for SWTableView cell node
	 */
	public class CCTableViewCell: CCNode, CCSortableObject
	{
		public CCTableViewCell() {}
		/**
		 * The index used internally by SWTableView and its subclasses
		 */
		public int getIdx()
		{
			return m_uIdx;
		}

		public void setIdx(int uIdx)
		{
			m_uIdx = uIdx;
		}

		/**
		 * Cleans up any resources linked to this cell and resets <code>idx</code> property.
		 */
		public void Reset()
		{
			m_uIdx = CCArrayForObjectSorting.CC_INVALID_INDEX;
		}

	    public int ObjectID
	    {
	        set { m_uIdx = value; }
	        get { return m_uIdx; }
	    }

	    private int m_uIdx;
	}
}