using System;
using System.Collections.Generic;

namespace CocosSharp
{
	#region Enums

	public enum CCScrollViewDirection
    {
        None = -1,
        Horizontal = 0,
        Vertical,
        Both
    }

	#endregion Enums


    public interface ICCScrollViewDelegate
    {
        void ScrollViewDidScroll(CCScrollView view);
        void ScrollViewDidZoom(CCScrollView view);
    }

    /**
     * ScrollView support for cocos2d for iphone.
     * It provides scroll view functionalities to cocos2d projects natively.
     */

    public class CCScrollView : CCLayer
    {
        const float SCROLL_DEACCEL_RATE = 0.95f;
        const float SCROLL_DEACCEL_DIST = 1.0f;
        const float BOUNCE_DURATION = 0.15f;
        const float INSET_RATIO = 0.2f;
        const float MOVE_INCH = 7.0f / 160.0f;

        bool clippingToBounds;
		bool isTouchEnabled;
		float touchLength;

		float minScale;
        float maxScale;
        CCPoint minInset;
		CCPoint maxInset;

		List<CCTouch> touches;
		CCPoint scrollDistance;

		CCPoint touchPoint;
		CCSize viewSize;

		CCNode container;

		CCEventListener TouchListener;


		#region Properties

		public bool Bounceable { get ; set; }
		public bool Dragging { get; private set; }
		public bool IsTouchMoved { get; private set; }
		public CCScrollViewDirection Direction { get; set; }
		public ICCScrollViewDelegate Delegate { get; set; }

		public bool ClippingToBounds
		{
			get { return clippingToBounds; }
			set 
			{ 
				clippingToBounds = value; 
				ChildClippingMode = clippingToBounds ? CCClipMode.Bounds : CCClipMode.None;
			}
		}

		public bool TouchEnabled
		{
			get { return isTouchEnabled; }
			set
			{

				if (value != isTouchEnabled) 
				{

					isTouchEnabled = value;

					if (isTouchEnabled) 
					{

						// Register Touch Event
						var touchListener = new CCEventListenerTouchOneByOne();
						touchListener.IsSwallowTouches = true;

						touchListener.OnTouchBegan = TouchBegan;
						touchListener.OnTouchMoved = TouchMoved;
						touchListener.OnTouchEnded = TouchEnded;
						touchListener.OnTouchCancelled = TouchCancelled;

						EventDispatcher.AddEventListener(touchListener, this);

						TouchListener = touchListener;

					}
				}
				else
				{
					Dragging = false;
					IsTouchMoved = false;
					touches.Clear();
					EventDispatcher.RemoveEventListener(TouchListener);
					TouchListener = null;
				}
			}
		}

        public float MinScale
        {
            get { return minScale; }
            set
            {
                minScale = value;
                ZoomScale = ZoomScale;
            }
        }

        public float MaxScale
        {
            get { return maxScale; }
            set
            {
                maxScale = value;
                ZoomScale = ZoomScale;
            }
        }

		public float ZoomScale
		{
			get { return container.ScaleX; }
			set
			{
				if (container.ScaleX != value)
				{
					CCPoint center;

					if (touchLength == 0.0f)
					{
						center = new CCPoint(viewSize.Width * 0.5f, viewSize.Height * 0.5f);
						center = ConvertToWorldSpace(center);
					}
					else
					{
						center = touchPoint;
					}

					CCPoint oldCenter = container.ConvertToNodeSpace(center);
					container.Scale = Math.Max(minScale, Math.Min(maxScale, value));
					CCPoint newCenter = container.ConvertToWorldSpace(oldCenter);

					CCPoint offset = center - newCenter;
					if (Delegate != null)
					{
						Delegate.ScrollViewDidZoom(this);
					}
					SetContentOffset(container.Position + offset, false);
				}
			}
		}

		public CCPoint ContentOffset
		{
			get { return container.Position; }
			set { SetContentOffset(value); }
		}

		public CCPoint MinContainerOffset
		{
			get
			{
				return new CCPoint(viewSize.Width - container.ContentSize.Width * container.ScaleX, 
					viewSize.Height - container.ContentSize.Height * container.ScaleY);
			}
		}

		public CCPoint MaxContainerOffset
		{
			get { return CCPoint.Zero; }
		}

        public override CCSize ContentSize
        {
            get { return container.ContentSize; }
            set
            {
                if (Container != null)
                {
                    Container.ContentSize = value;
                    UpdateInset();
                }
            }
        }

		/**
		* size to clip. CCNode boundingBox uses contentSize directly.
		* It's semantically different what it actually means to common scroll views.
		* Hence, this scroll view will use a separate size property.
		*/

        public CCSize ViewSize
        {
            get { return viewSize; }
            set
            {
                viewSize = value;
                base.ContentSize = value;
            }
        }

		CCRect ViewRect
		{
			get 
			{
				var rect = new CCRect (0, 0, viewSize.Width, viewSize.Height);
				return CCAffineTransform.Transform (rect, NodeToWorldTransform);
			}
		}

        public CCNode Container
        {
            get { return container; }
            set
            {
                if (value == null)
                {
                    return;
                }

				RemoveAllChildren(true);
                container = value;

                container.IgnoreAnchorPointForPosition = false;
                container.AnchorPoint = CCPoint.Zero;

                AddChild(container);

                ViewSize = viewSize;
            }
        }

		#endregion Properties


        #region Constructors

        public CCScrollView() 
            : this (new CCSize(200, 200), null)
        {
		}

        public CCScrollView(CCSize size)
            : this(size, null)
        {
		}

        public CCScrollView(CCSize size, CCNode container)
        {
            if (container == null)
            {
                container = new CCLayer();
                container.IgnoreAnchorPointForPosition = false;
                container.AnchorPoint = CCPoint.Zero;
            }
			container.Position = new CCPoint(0.0f, 0.0f);

			this.container = container;

            ViewSize = size;
            TouchEnabled = true;
            Delegate = null;
            Bounceable = true;
			ClippingToBounds = true;
			Direction = CCScrollViewDirection.Both;
			MinScale = MaxScale = 1.0f;
			touches = new List<CCTouch>();
			touchLength = 0.0f;

			AddChild(container);
        }


        #endregion Constructors


		static float ConvertDistanceFromPointToInch(float pointDis)
		{
			float factor = (CCDrawManager.ScaleX + CCDrawManager.ScaleY) / 2;
			return pointDis * factor / CCDevice.DPI;
		}

		/**
		* Determines if a given node's bounding box is in visible bounds
		*
		* @return YES if it is in visible bounds
		*/

		public bool IsNodeVisible(CCNode node)
		{
			CCPoint offset = ContentOffset;
			CCSize size = ViewSize;
			float scale = ZoomScale;

			var viewRect = new CCRect(-offset.X / scale, -offset.Y / scale, size.Width / scale, size.Height / scale);

			return viewRect.IntersectsRect(node.BoundingBox);
		}

		public void UpdateInset()
		{
			if (Container != null)
			{
				maxInset = MaxContainerOffset;
				maxInset = new CCPoint(maxInset.X + viewSize.Width * INSET_RATIO,
					maxInset.Y + viewSize.Height * INSET_RATIO);
				minInset = MinContainerOffset;
				minInset = new CCPoint(minInset.X - viewSize.Width * INSET_RATIO,
					minInset.Y - viewSize.Height * INSET_RATIO);
			}
		}

		public void SetContentOffset(CCPoint offset, bool animated=false)
        {
            if (animated)
            {
                //animate scrolling
                SetContentOffsetInDuration(offset, BOUNCE_DURATION);
            }
            else
            {
                //set the container position directly
                if (!Bounceable)
                {
                    CCPoint minOffset = MinContainerOffset;
                    CCPoint maxOffset = MaxContainerOffset;

                    offset.X = Math.Max(minOffset.X, Math.Min(maxOffset.X, offset.X));
                    offset.Y = Math.Max(minOffset.Y, Math.Min(maxOffset.Y, offset.Y));
                }

                container.Position = offset;

                if (Delegate != null)
                {
                    Delegate.ScrollViewDidScroll(this);
                }
            }
        }

		/**
		* Sets a new content offset. It ignores max/min offset. It just sets what's given. (just like UIKit's UIScrollView)
		* You can override the animation duration with this method.
		*
		* @param offset new offset
		* @param animation duration
		*/

        public void SetContentOffsetInDuration(CCPoint offset, float dt)
        {
            CCMoveTo scroll = new CCMoveTo (dt, offset);
            CCCallFuncN expire = new CCCallFuncN(StoppedAnimatedScroll);
            container.RunAction(new CCSequence(scroll, expire));
            Schedule(PerformedAnimatedScroll);
        }

		public void SetZoomScale(float value, bool animated=false)
        {
            if (animated)
            {
                SetZoomScaleInDuration(value, BOUNCE_DURATION);
            }
            else
            {
                ZoomScale = value;
            }
        }

        public void SetZoomScaleInDuration(float s, float dt)
        {
            if (dt > 0)
            {
                if (container.ScaleX != s)
                {
                    CCActionTween scaleAction = new CCActionTween (dt, "zoomScale", container.ScaleX, s);
                    RunAction(scaleAction);
                }
            }
            else
            {
                ZoomScale = s;
            }
        }

		/**
		* Provided to make scroll view compatible with SWLayer's pause method
		*/

        public void Pause(object sender)
        {
            container.Pause();

			var children = container.Children;

			if (children != null)
            {
				foreach(CCNode child in children)
                {
					child.Pause();
                }
            }
        }

		/**
		* Provided to make scroll view compatible with SWLayer's resume method
		*/

        public void Resume(object sender)
        {
			var children = container.Children;

			if (children != null)
            {
				foreach(CCNode child in children)
                {
					child.Resume();
                }
            }

            container.Resume();
        }

		#region Event handling

        /** override functions */
		public bool TouchBegan(CCTouch pTouch, CCEvent touchEvent)
        {
            if (!Visible)
            {
                return false;
            }

			var frame = ViewRect;

            //dispatcher does not know about clipping. reject touches outside visible bounds.
            if (touches.Count > 2 ||
                IsTouchMoved ||
                !frame.ContainsPoint(container.ConvertToWorldSpace(container.ConvertTouchToNodeSpace(pTouch))))
            {
                return false;
            }

            if (!touches.Contains(pTouch))
            {
                touches.Add(pTouch);
            }

            if (touches.Count == 1)
            {
                // scrolling
                touchPoint = ConvertTouchToNodeSpace(pTouch);
                IsTouchMoved = false;
                Dragging = true; //Dragging started
                scrollDistance = CCPoint.Zero;
                touchLength = 0.0f;
            }
            else if (touches.Count == 2)
            {
				touchPoint = CCPoint.Midpoint(ConvertTouchToNodeSpace(touches[0]), ConvertTouchToNodeSpace(touches[1]));
				touchLength = CCPoint.Distance(container.ConvertTouchToNodeSpace(touches[0]), container.ConvertTouchToNodeSpace(touches[1]));
                Dragging = false;
            }
            return true;
        }

		public void TouchMoved(CCTouch touch, CCEvent touchEvent)
        {
            if (!Visible)
            {
                return;
            }

            if (touches.Contains(touch))
            {
                if (touches.Count == 1 && Dragging)
                {// scrolling
                    CCPoint moveDistance, newPoint; //, maxInset, minInset;
                    float newX, newY;

					var frame = ViewRect;

                    newPoint = ConvertTouchToNodeSpace(touches[0]);
                    moveDistance = newPoint - touchPoint;

                    float dis = 0.0f;
                    if (Direction == CCScrollViewDirection.Vertical)
                    {
                        dis = moveDistance.Y;
                    }
                    else if (Direction == CCScrollViewDirection.Horizontal)
                    {
                        dis = moveDistance.X;
                    }
                    else
                    {
                        dis = (float)Math.Sqrt(moveDistance.X * moveDistance.X + moveDistance.Y * moveDistance.Y);
                    }

                    if (!IsTouchMoved && Math.Abs(ConvertDistanceFromPointToInch(dis)) < MOVE_INCH)
                    {
                        //CCLOG("Invalid movement, distance = [%f, %f], disInch = %f", moveDistance.x, moveDistance.y);
                        return;
                    }

                    if (!IsTouchMoved)
                    {
                        moveDistance = CCPoint.Zero;
                    }

                    touchPoint = newPoint;
                    IsTouchMoved = true;

                    if (frame.ContainsPoint(ConvertToWorldSpace(newPoint)))
                    {
                        switch (Direction)
                        {
                            case CCScrollViewDirection.Vertical:
                                moveDistance = new CCPoint(0.0f, moveDistance.Y);
                                break;
                            case CCScrollViewDirection.Horizontal:
                                moveDistance = new CCPoint(moveDistance.X, 0.0f);
                                break;
                            default:
                                break;
                        }

                        newX = container.Position.X + moveDistance.X;
                        newY = container.Position.Y + moveDistance.Y;

                        scrollDistance = moveDistance;
                        SetContentOffset(new CCPoint(newX, newY));
                    }
                }
                else if (touches.Count == 2 && !Dragging)
                {
                    float len = CCPoint.Distance(container.ConvertTouchToNodeSpace(touches[0]),
                                                             container.ConvertTouchToNodeSpace(touches[1]));
                    ZoomScale = ZoomScale * len / touchLength;
                }
            }
        }

		public void TouchEnded(CCTouch touch, CCEvent touchEvent)
        {
            if (!Visible)
            {
                return;
            }

            if (touches.Contains(touch))
            {
                if (touches.Count == 1 && IsTouchMoved)
                {
                    Schedule(DeaccelerateScrolling);
                }
                touches.Remove(touch);
            }

            if (touches.Count == 0)
            {
                Dragging = false;
                IsTouchMoved = false;
            }
        }

		public void TouchCancelled(CCTouch touch, CCEvent touchEvent)
        {
            if (!Visible)
            {
                return;
            }
            touches.Remove(touch);
            if (touches.Count == 0)
            {
                Dragging = false;
                IsTouchMoved = false;
            }
        }

		#endregion Event handling


        public override void AddChild(CCNode child, int zOrder, int tag)
        {
            child.IgnoreAnchorPointForPosition = false;
            // child.AnchorPoint = CCPoint.Zero;
            if (container != child)
            {
                container.AddChild(child, zOrder, tag);
            }
            else
            {
                base.AddChild(child, zOrder, tag);
            }
        }

		/**
		* Relocates the container at the proper offset, in bounds of max/min offsets.
		*
		* @param animated If YES, relocation is animated
		*/

        void RelocateContainer(bool animated)
        {
            CCPoint min = MinContainerOffset;
            CCPoint max = MaxContainerOffset;

            CCPoint oldPoint = container.Position;

            float newX = oldPoint.X;
            float newY = oldPoint.Y;
            if (Direction == CCScrollViewDirection.Both || Direction == CCScrollViewDirection.Horizontal)
            {
                newX = Math.Min(newX, max.X);
                newX = Math.Max(newX, min.X);
            }

            if (Direction == CCScrollViewDirection.Both || Direction == CCScrollViewDirection.Vertical)
            {
                newY = Math.Min(newY, max.Y);
                newY = Math.Max(newY, min.Y);
            }

            if (newY != oldPoint.Y || newX != oldPoint.X)
            {
                SetContentOffset(new CCPoint(newX, newY), animated);
            }
        }

		/**
		* implements auto-scrolling behavior. change SCROLL_DEACCEL_RATE as needed to choose
		* deacceleration speed. it must be less than 1.0f.
		*
		* @param dt delta
		*/

        void DeaccelerateScrolling(float dt)
        {
            if (Dragging)
            {
                Unschedule(DeaccelerateScrolling);
                return;
            }

            CCPoint maxInset, minInset;

            container.Position = container.Position + scrollDistance;

            if (Bounceable)
            {
				maxInset = this.maxInset;
				minInset = this.minInset;
            }
            else
            {
                maxInset = MaxContainerOffset;
                minInset = MinContainerOffset;
            }

            //check to see if offset lies within the inset bounds
            float newX = Math.Min(container.Position.X, maxInset.X);
            newX = Math.Max(newX, minInset.X);
            float newY = Math.Min(container.Position.Y, maxInset.Y);
            newY = Math.Max(newY, minInset.Y);

            scrollDistance = scrollDistance - new CCPoint(newX - container.Position.X, newY - container.Position.Y);
            scrollDistance = scrollDistance * SCROLL_DEACCEL_RATE;
            SetContentOffset(new CCPoint(newX, newY), false);

            if ((Math.Abs(scrollDistance.X) <= SCROLL_DEACCEL_DIST &&
                 Math.Abs(scrollDistance.Y) <= SCROLL_DEACCEL_DIST) ||
                newY > maxInset.Y || newY < minInset.Y ||
                newX > maxInset.X || newX < minInset.X ||
                newX == maxInset.X || newX == minInset.X ||
                newY == maxInset.Y || newY == minInset.Y)
            {
                Unschedule(DeaccelerateScrolling);
                RelocateContainer(true);
            }
        }

		/**
		* This method makes sure auto scrolling causes delegate to invoke its method
		*/

        void PerformedAnimatedScroll(float dt)
        {
            if (Dragging)
            {
                Unschedule(PerformedAnimatedScroll);
                return;
            }

            if (Delegate != null)
            {
                Delegate.ScrollViewDidScroll(this);
            }
        }

		/**
		* Expire animated scroll delegate calls
		*/

        void StoppedAnimatedScroll(CCNode node)
        {
            Unschedule(PerformedAnimatedScroll);
            // After the animation stopped, "scrollViewDidScroll" should be invoked, this could fix the bug of lack of tableview cells.
            if (Delegate != null)
            {
                Delegate.ScrollViewDidScroll(this);
            }
        }
    }
}