using System;

namespace CocosSharp
{
    //
    // TODO: Add CCGesture
    //

    public class CCTouch
    {
        bool startPointCaptured;

        CCPoint point;
        CCPoint prevPoint;
        CCPoint startPoint;

        internal CCNode Target { get; set; }
        #region Properties

        public int Id { get; private set; }

        public CCPoint Location
        {
            get { return Target.ScreenToWorldspace(point); }
        }


        public CCPoint LocationOnScreen
        {
            get { return point; }
        }

        /** returns the start touch location in screen coordinates */
        public CCPoint StartLocationOnScreen
        {
            get { return startPoint; }
        }

        public CCPoint PreviousLocationOnScreen
        {
            get { return prevPoint; }
        }

        public CCPoint Delta
        {
            get { return LocationOnScreen - PreviousLocationOnScreen; }
        }

        #endregion Properties


        #region Constructors

		internal CCTouch(int id=0, float x=0.0f, float y=0.0f)
        {
            Id = id;
            point = new CCPoint(x, y);
            prevPoint = new CCPoint(x, y);
        }

        #endregion Constructors


        internal void SetTouchInfo(int id, float x, float y)
        {
            Id = id;
            prevPoint = point;
            point.X = x;
            point.Y = y;
            if (!startPointCaptured)
            {
                startPoint = point;
                startPointCaptured = true;
            }
        }
    }
}