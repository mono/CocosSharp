using System;

namespace CocosSharp
{
    //
    // TODO: Add CCGesture
    //

    public class CCTouch
    {
        readonly CCPoint startPoint;
        CCPoint point;
        CCPoint prevPoint;


        internal CCNode Target { get; set; }

        #region Properties

        public int Id { get; private set; }

        // returns the current touch location in world coordinates 
        public CCPoint Location
        {
            get { return Target.ScreenToWorldspace(point); }
        }

        // returns the current touch location in screen coordinates 
        public CCPoint LocationOnScreen
        {
            get { return point; }
        }

        // returns the start touch location in world coordinates 
        public CCPoint StartLocation
        {
            get { return Target.ScreenToWorldspace(startPoint); }
        }

        // returns the start touch location in screen coordinates 
        public CCPoint StartLocationOnScreen
        {
            get { return startPoint; }
        }

        // returns the previous touch location in world coordinates 
        public CCPoint PreviousLocation
        {
            get { return Target.ScreenToWorldspace(prevPoint); }
        }

        // returns the previous touch location in screen coordinates 
        public CCPoint PreviousLocationOnScreen
        {
            get { return prevPoint; }
        }

        // returns the delta position between the current location and the previous location in world coordinates
        public CCPoint Delta
        {
            get { return Location - PreviousLocation; }
        }

        internal TimeSpan TimeStamp { get; private set; }

        #endregion Properties


        #region Constructors

        internal CCTouch(int id, float x, float y, TimeSpan timeStamp)
        {
            Id = id;
            TimeStamp = timeStamp;
            point = new CCPoint(x, y);
            prevPoint = point;
            startPoint = point;
        }

        internal CCTouch(int id, CCPoint pos, TimeSpan timeStamp) 
            : this(id, pos.X, pos.Y, timeStamp)
        {
        }

        #endregion Constructors


        internal void UpdateTouchInfo(int id, float x, float y, TimeSpan timeStamp)
        {
            Id = id;
            prevPoint = point;
            point.X = x;
            point.Y = y;
            TimeStamp = timeStamp;
        }
    }
}