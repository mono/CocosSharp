using System;

namespace cocos2d
{
    //
    // TODO: Add CCGesture
    //

    public class CCTouch : CCObject
    {
        private int m_nId;
        /// <summary>
        /// Point of action
        /// </summary>
        private CCPoint m_point;
        /// <summary>
        /// Previous point in the action
        /// </summary>
        private CCPoint m_prevPoint;

        public CCTouch()
            : this(0, 0, 0)
        {
        }

        public CCTouch(int id, float x, float y)
        {
            m_nId = id;
            m_point = new CCPoint(x, y);
            m_prevPoint = new CCPoint(x, y);
        }

        public CCPoint LocationInView
        {
            get { return m_point; }
        }

        public CCPoint PreviousLocationInView
        {
            get { return m_prevPoint; }
        }

        public CCPoint Location
        {
            get { return CCDirector.SharedDirector.ConvertToGl(m_point); }
        }

        public CCPoint PreviousLocation
        {
            get { return CCDirector.SharedDirector.ConvertToGl(m_prevPoint); }
        }


        public int Id
        {
            get { return m_nId; }
        }

        public CCPoint Delta
        {
            get { return CCPointExtension.Subtract(Location, PreviousLocation); }
        }

        public void SetTouchInfo(int id, float x, float y)
        {
            m_nId = id;
            m_prevPoint = m_point;
            m_point.x = x;
            m_point.y = y;
        }
    }

    public class CCEvent : CCObject
    {
    }
}