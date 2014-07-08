using System;

namespace CocosSharp
{
    //
    // TODO: Add CCGesture
    //

    public class CCTouch
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

        private CCPoint m_startPoint;
        private bool m_startPointCaptured;
		internal CCDirector Director { get; set; }

		internal CCTouch(CCDirector director = null)
            : this(0, 0, 0, director)
        { }

		internal CCTouch(int id, float x, float y, CCDirector director = null)
        {
            m_nId = id;
            m_point = new CCPoint(x, y);
            m_prevPoint = new CCPoint(x, y);
			Director = director;
        }

        /** returns the start touch location in OpenGL coordinates */
        public CCPoint StartLocation
        {
            get { return Director.ConvertToGl(m_startPoint); }
        }

        public CCPoint LocationInView
        {
            get { return m_point; }
        }

        /** returns the start touch location in screen coordinates */
        public CCPoint StartLocationInView
        {
            get { return m_startPoint; }
        }

        public CCPoint PreviousLocationInView
        {
            get { return m_prevPoint; }
        }

        public CCPoint Location
        {
            get { return Director.ConvertToGl(m_point); }
        }

        public CCPoint PreviousLocation
        {
            get { return Director.ConvertToGl(m_prevPoint); }
        }


        public int Id
        {
            get { return m_nId; }
        }

        public CCPoint Delta
        {
            get { return Location - PreviousLocation; }
        }

        internal void SetTouchInfo(int id, float x, float y)
        {
            m_nId = id;
            m_prevPoint = m_point;
            m_point.X = x;
            m_point.Y = y;
            if (!m_startPointCaptured)
            {
                m_startPoint = m_point;
                m_startPointCaptured = true;
            }
        }
    }
}