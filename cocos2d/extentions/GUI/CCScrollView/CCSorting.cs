using System.Collections.Generic;
using System.Diagnostics;

namespace cocos2d
{
    public interface CCSortableObject
    {
        int ObjectID { set; get; }
    }

    internal class CCSortedObject : CCObject, CCSortableObject
    {
        private int objectID;

        public CCSortedObject()
        {
            objectID = 0;
        }

        #region CCSortableObject Members

        public virtual int ObjectID
        {
            set { objectID = value; }
            get { return objectID; }
        }

        #endregion
    };

    public class CCArrayForObjectSorting : List<CCObject>
    {
        public const int CC_INVALID_INDEX = -1;

        /*!
		 * Inserts a given object into array.
		 * 
		 * Inserts a given object into array with key and value that are used in
		 * sorting. "value" must respond to message, compare:, which returns 
		 * (NSComparisonResult). If it does not respond to the message, it is appended.
		 * If the compare message does not result NSComparisonResult, sorting behavior
		 * is not defined. It ignores duplicate entries and inserts next to it.
		 *
		 * @param object to insert
		 */

        public void insertSortedObject(CCSortableObject obj)
        {
            int idx;
            var pObj = (CCObject) obj;
            Debug.Assert(pObj != null, "Invalid parameter.");
            idx = indexOfSortedObject(obj);

            Insert(idx, pObj);
        }

        /*!
		 * Removes an object in array.
		 *
		 * Removes an object with given key and value. If no object is found in array
		 * with the key and value, no action is taken.
		 *
		 * @param value to remove
		 */

        public void removeSortedObject(CCSortableObject obj)
        {
            if (Count == 0)
            {
                return;
            }
            int idx;
            CCSortableObject foundObj;
            idx = indexOfSortedObject(obj);

            if (idx < Count && idx != CC_INVALID_INDEX)
            {
                foundObj = (CCSortableObject) this[idx];

                if (foundObj.ObjectID == obj.ObjectID)
                {
                    RemoveAt(idx);
                }
            }
        }

        /*!
		 * Sets a new value of the key for the given object.
		 * 
		 * In case where sorting value must be changed, this message must be sent to
		 * keep consistency of being sorted. If it is changed externally, it must be
		 * sorted completely again.
		 *
		 * @param value to set
		 * @param object the object which has the value
		 */

        public void setObjectID_ofSortedObject(int tag, CCSortableObject obj)
        {
            CCSortableObject foundObj;
            int idx;

            idx = indexOfSortedObject(obj);
            if (idx < Count && idx != CC_INVALID_INDEX)
            {
                foundObj = (CCSortableObject) (this[idx]);
                var pObj = (CCObject) foundObj;

                if (foundObj.ObjectID == obj.ObjectID)
                {
                    RemoveAt(idx);
                    foundObj.ObjectID = tag;
                    insertSortedObject(foundObj);
                }
                else
                {
                }
            }
        }

        public CCSortableObject objectWithObjectID(int tag)
        {
            if (Count == 0)
            {
                return null;
            }

            CCSortableObject foundObj;

            foundObj = new CCSortedObject();
            foundObj.ObjectID = tag;

            int idx = indexOfSortedObject(foundObj);

            foundObj = null;

            if (idx < Count && idx != CC_INVALID_INDEX)
            {
                foundObj = (CCSortableObject) (this[idx]);
                if (foundObj.ObjectID != tag)
                {
                    foundObj = null;
                }
            }

            return foundObj;
        }

        /*!
		 * Returns an object with given key and value.
		 * 
		 * Returns an object with given key and value. If no object is found,
		 * it returns nil.
		 *
		 * @param value to locate object
		 * @return object found or nil.
		 */
        //public CCSortableObject getObjectWithObjectID(int tag)
        //{
        //}

        /*!
		 * Returns an index of the object with given key and value.
		 *
		 * Returns the index of an object with given key and value. 
		 * If no object is found, it returns an index at which the given object value
		 * would have been located. If object must be located at the end of array,
		 * it returns the length of the array, which is out of bound.
		 * 
		 * @param value to locate object
		 * @return index of an object found
		 */

        public int indexOfSortedObject(CCSortableObject obj)
        {
            int idx = 0;
            if (obj != null)
            {
                //       CCObject* pObj = (CCObject*)bsearch((CCObject*)&object, data->arr, data->num, sizeof(CCObject*), _compareObject);
                // FIXME: need to use binary search to improve performance
                int uPrevObjectID = 0;
                int uOfSortObjectID = obj.ObjectID;

                foreach (CCObject pObj in this)
                {
                    var pSortableObj = (CCSortableObject) pObj;

                    int uCurObjectID = pSortableObj.ObjectID;
                    if ((uOfSortObjectID == uCurObjectID)
                        || (uOfSortObjectID >= uPrevObjectID && uOfSortObjectID < uCurObjectID))
                    {
                        break;
                    }

                    uPrevObjectID = uCurObjectID;
                    idx++;
                }
            }
            else
            {
                idx = CC_INVALID_INDEX;
            }
            return idx;
        }
    };
}