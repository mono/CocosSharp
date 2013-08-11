using System;
using System.Collections.Generic;

namespace Cocos2D
{
    public enum CCScrollViewDirection
    {
        None = -1,
        Horizontal = 0,
        Vertical,
        Both
    }

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
        private const float SCROLL_DEACCEL_RATE = 0.95f;
        private const float SCROLL_DEACCEL_DIST = 1.0f;
        private const float BOUNCE_DURATION = 0.15f;
        private const float INSET_RATIO = 0.2f;
        private const float MOVE_INCH = 7.0f / 160.0f;

        protected bool _bounceable;

        protected bool _clippingToBounds;
        protected bool _dragging;
        protected bool _touchMoved;
        protected CCScrollViewDirection _direction = CCScrollViewDirection.Both;
        protected CCPoint _maxInset;
        protected float _maxScale;
        protected CCPoint _minInset;
        protected float _minScale;
        protected float _touchLength;
        protected CCNode _container;
        protected ICCScrollViewDelegate _delegate;
        protected List<CCTouch> _touches;
        protected CCPoint _contentOffset;
        protected CCPoint _scrollDistance;

        //Touch point
        protected CCPoint _touchPoint;
        protected CCSize _viewSize;

        public CCScrollView()
        {
            Init();
        }

        public CCScrollView(CCSize size)
        {
            InitWithViewSize(size, null);
        }

        /**
        * Returns an autoreleased scroll view object.
        *
        * @param size view size
        * @param container parent object
        * @return autoreleased scroll view object
        */
        public CCScrollView(CCSize size, CCNode container)
        {
            InitWithViewSize(size, container);
        }

        public float MinScale
        {
            get { return _minScale; }
            set
            {
                _minScale = value;
                ZoomScale = ZoomScale;
            }
        }

        public float MaxScale
        {
            get { return _maxScale; }
            set
            {
                _maxScale = value;
                ZoomScale = ZoomScale;
            }
        }

        public override CCSize ContentSize
        {
            get { return _container.ContentSize; }
            set
            {
                if (Container != null)
                {
                    Container.ContentSize = value;
                    UpdateInset();
                }
            }
        }

        public override bool TouchEnabled
        {
            get { return base.TouchEnabled; }
            set
            {
                base.TouchEnabled = value;
                if (!value)
                {
                    _dragging = false;
                    _touchMoved = false;
                    _touches.Clear();
                }
            }
        }

        public float ZoomScale
        {
            get { return _container.Scale; }
            set
            {
                if (_container.Scale != value)
                {
                    CCPoint center;

                    if (_touchLength == 0.0f)
                    {
                        center = new CCPoint(_viewSize.Width * 0.5f, _viewSize.Height * 0.5f);
                        center = ConvertToWorldSpace(center);
                    }
                    else
                    {
                        center = _touchPoint;
                    }

                    CCPoint oldCenter = _container.ConvertToNodeSpace(center);
                    _container.Scale = Math.Max(_minScale, Math.Min(_maxScale, value));
                    CCPoint newCenter = _container.ConvertToWorldSpace(oldCenter);

                    CCPoint offset = center - newCenter;
                    if (_delegate != null)
                    {
                        _delegate.ScrollViewDidZoom(this);
                    }
                    SetContentOffset(_container.Position + offset, false);
                }
            }
        }

        public bool Bounceable
        {
            get { return _bounceable; }
            set { _bounceable = value; }
        }

        /**
     * size to clip. CCNode boundingBox uses contentSize directly.
     * It's semantically different what it actually means to common scroll views.
     * Hence, this scroll view will use a separate size property.
     */

        public CCSize ViewSize
        {
            get { return _viewSize; }
            set
            {
                _viewSize = value;
                base.ContentSize = value;
            }
        }

        public CCNode Container
        {
            get { return _container; }
            set
            {
                if (value == null)
                {
                    return;
                }

                RemoveAllChildrenWithCleanup(true);
                _container = value;

                _container.IgnoreAnchorPointForPosition = false;
                _container.AnchorPoint = CCPoint.Zero;

                AddChild(_container);

                ViewSize = _viewSize;
            }
        }

        /**
     * direction allowed to scroll. CCScrollViewDirectionBoth by default.
     */

        public CCScrollViewDirection Direction
        {
            get { return _direction; }
            set { _direction = value; }
        }

        public ICCScrollViewDelegate Delegate
        {
            get { return _delegate; }
            set { _delegate = value; }
        }

        public bool ClippingToBounds
        {
            get { return _clippingToBounds; }
            set 
			{ 
				_clippingToBounds = value; 
				ChildClippingMode =_clippingToBounds ? CCClipMode.Bounds : CCClipMode.None;
			}
        }

        public override bool Init()
        {
            return InitWithViewSize(new CCSize(200, 200), null);
        }

        public override void RegisterWithTouchDispatcher()
        {
            CCDirector.SharedDirector.TouchDispatcher.AddTargetedDelegate(this, TouchPriority, false);
        }

        /**
     * Returns a scroll view object
     *
     * @param size view size
     * @param container parent object
     * @return scroll view object
     */

        protected virtual bool InitWithViewSize(CCSize size, CCNode container)
        {
            if (base.Init())
            {
				_container = container;

                if (_container == null)
                {
                    _container = new CCLayer();
                    _container.IgnoreAnchorPointForPosition = false;
                    _container.AnchorPoint = CCPoint.Zero;
                }

                ViewSize = size;

                TouchEnabled = true;
                _touches = new List<CCTouch>();
                _delegate = null;
                _bounceable = true;
                _clippingToBounds = true;
                _direction = CCScrollViewDirection.Both;
                _container.Position = new CCPoint(0.0f, 0.0f);
                _touchLength = 0.0f;

                AddChild(_container);
                _minScale = _maxScale = 1.0f;

                return true;
            }
            return false;
        }

        /**
     * Sets a new content offset. It ignores max/min offset. It just sets what's given. (just like UIKit's UIScrollView)
     *
     * @param offset new offset
     * @param If YES, the view scrolls to the new offset
     */

        public void SetContentOffset(CCPoint offset)
        {
            SetContentOffset(offset, false);
        }

        public void SetContentOffset(CCPoint offset, bool animated)
        {
            if (animated)
            {
                //animate scrolling
                SetContentOffsetInDuration(offset, BOUNCE_DURATION);
            }
            else
            {
                //set the container position directly
                if (!_bounceable)
                {
                    CCPoint minOffset = MinContainerOffset;
                    CCPoint maxOffset = MaxContainerOffset;

                    offset.X = Math.Max(minOffset.X, Math.Min(maxOffset.X, offset.X));
                    offset.Y = Math.Max(minOffset.Y, Math.Min(maxOffset.Y, offset.Y));
                }

                _container.Position = offset;

                if (_delegate != null)
                {
                    _delegate.ScrollViewDidScroll(this);
                }
            }
        }

        public CCPoint GetContentOffset()
        {
            return _container.Position;
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
            _container.RunAction(new CCSequence(scroll, expire));
            Schedule(PerformedAnimatedScroll);
        }

        /**
     * Sets a new scale and does that for a predefined duration.
     *
     * @param s a new scale vale
     * @param animated if YES, scaling is animated
     */

        public void SetZoomScale(float value, bool animated)
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

        /**
     * Sets a new scale for container in a given duration.
     *
     * @param s a new scale value
     * @param animation duration
     */

        public void SetZoomScaleInDuration(float s, float dt)
        {
            if (dt > 0)
            {
                if (_container.Scale != s)
                {
                    CCActionTween scaleAction = new CCActionTween (dt, "zoomScale", _container.Scale, s);
                    RunAction(scaleAction);
                }
            }
            else
            {
                ZoomScale = s;
            }
        }

        /**
     * Returns the current container's minimum offset. You may want this while you animate scrolling by yourself
     */

        public CCPoint MinContainerOffset
        {
            get
            {
                return new CCPoint(_viewSize.Width - _container.ContentSize.Width * _container.ScaleX,
                                   _viewSize.Height - _container.ContentSize.Height * _container.ScaleY);
            }
        }

        /**
     * Returns the current container's maximum offset. You may want this while you animate scrolling by yourself
     */

        public CCPoint MaxContainerOffset
        {
            get { return CCPoint.Zero; }
        }

        /**
     * Determines if a given node's bounding box is in visible bounds
     *
     * @return YES if it is in visible bounds
     */

        public bool IsNodeVisible(CCNode node)
        {
            CCPoint offset = GetContentOffset();
            CCSize size = ViewSize;
            float scale = ZoomScale;

            var viewRect = new CCRect(-offset.X / scale, -offset.Y / scale, size.Width / scale, size.Height / scale);

            return viewRect.IntersectsRect(node.BoundingBox);
        }

        /**
     * Provided to make scroll view compatible with SWLayer's pause method
     */

        public void Pause(object sender)
        {
            _container.PauseSchedulerAndActions();

            var pChildren = _container.Children;

            if (pChildren != null && pChildren.count > 0)
            {
                for (int i = 0; i < pChildren.count; i++)
                {
                    pChildren.Elements[i].PauseSchedulerAndActions();
                }
            }
        }

        /**
     * Provided to make scroll view compatible with SWLayer's resume method
     */

        public void Resume(object sender)
        {
            var pChildren = _container.Children;

            if (pChildren != null && pChildren.count > 0)
            {
                for (int i = 0; i < pChildren.count; i++)
                {
                    pChildren.Elements[i].ResumeSchedulerAndActions();
                }
            }

            _container.ResumeSchedulerAndActions();
        }


        public bool IsDragging
        {
            get { return _dragging; }
        }

        public bool IsTouchMoved
        {
            get { return _touchMoved; }
        }

        /** override functions */
        // optional
        public override bool TouchBegan(CCTouch pTouch)
        {
            if (!Visible)
            {
                return false;
            }

            var frame = GetViewRect();

            //dispatcher does not know about clipping. reject touches outside visible bounds.
            if (_touches.Count > 2 ||
                _touchMoved ||
                !frame.ContainsPoint(_container.ConvertToWorldSpace(_container.ConvertTouchToNodeSpace(pTouch))))
            {
                return false;
            }

            if (!_touches.Contains(pTouch))
            {
                _touches.Add(pTouch);
            }

            if (_touches.Count == 1)
            {
                // scrolling
                _touchPoint = ConvertTouchToNodeSpace(pTouch);
                _touchMoved = false;
                _dragging = true; //dragging started
                _scrollDistance = CCPoint.Zero;
                _touchLength = 0.0f;
            }
            else if (_touches.Count == 2)
            {
                _touchPoint = CCPoint.Midpoint(ConvertTouchToNodeSpace(_touches[0]),
                                                             ConvertTouchToNodeSpace(_touches[1]));
                _touchLength = CCPoint.Distance(_container.ConvertTouchToNodeSpace(_touches[0]),
                                                              _container.ConvertTouchToNodeSpace(_touches[1]));
                _dragging = false;
            }
            return true;
        }

        public override void TouchMoved(CCTouch touch)
        {
            if (!Visible)
            {
                return;
            }

            if (_touches.Contains(touch))
            {
                if (_touches.Count == 1 && _dragging)
                {// scrolling
                    CCPoint moveDistance, newPoint; //, maxInset, minInset;
                    float newX, newY;

                    var frame = GetViewRect();

                    newPoint = ConvertTouchToNodeSpace(_touches[0]);
                    moveDistance = newPoint - _touchPoint;

                    float dis = 0.0f;
                    if (_direction == CCScrollViewDirection.Vertical)
                    {
                        dis = moveDistance.Y;
                    }
                    else if (_direction == CCScrollViewDirection.Horizontal)
                    {
                        dis = moveDistance.X;
                    }
                    else
                    {
                        dis = (float)Math.Sqrt(moveDistance.X * moveDistance.X + moveDistance.Y * moveDistance.Y);
                    }

                    if (!_touchMoved && Math.Abs(ConvertDistanceFromPointToInch(dis)) < MOVE_INCH)
                    {
                        //CCLOG("Invalid movement, distance = [%f, %f], disInch = %f", moveDistance.x, moveDistance.y);
                        return;
                    }

                    if (!_touchMoved)
                    {
                        moveDistance = CCPoint.Zero;
                    }

                    _touchPoint = newPoint;
                    _touchMoved = true;

                    if (frame.ContainsPoint(ConvertToWorldSpace(newPoint)))
                    {
                        switch (_direction)
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

                        //maxInset = m_fMaxInset;
                        //minInset = m_fMinInset;

                        newX = _container.Position.X + moveDistance.X;
                        newY = _container.Position.Y + moveDistance.Y;

                        _scrollDistance = moveDistance;
                        SetContentOffset(new CCPoint(newX, newY));
                    }
                }
                else if (_touches.Count == 2 && !_dragging)
                {
                    float len = CCPoint.Distance(_container.ConvertTouchToNodeSpace(_touches[0]),
                                                             _container.ConvertTouchToNodeSpace(_touches[1]));
                    ZoomScale = ZoomScale * len / _touchLength;
                }
            }
        }

        public override void TouchEnded(CCTouch touch)
        {
            if (!Visible)
            {
                return;
            }

            if (_touches.Contains(touch))
            {
                if (_touches.Count == 1 && _touchMoved)
                {
                    Schedule(DeaccelerateScrolling);
                }
                _touches.Remove(touch);
            }

            if (_touches.Count == 0)
            {
                _dragging = false;
                _touchMoved = false;
            }
        }

        public override void TouchCancelled(CCTouch touch)
        {
            if (!Visible)
            {
                return;
            }
            _touches.Remove(touch);
            if (_touches.Count == 0)
            {
                _dragging = false;
                _touchMoved = false;
            }
        }

        public override void AddChild(CCNode child, int zOrder, int tag)
        {
            child.IgnoreAnchorPointForPosition = false;
            // child.AnchorPoint = CCPoint.Zero;
            if (_container != child)
            {
                _container.AddChild(child, zOrder, tag);
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

        private void RelocateContainer(bool animated)
        {
            CCPoint min = MinContainerOffset;
            CCPoint max = MaxContainerOffset;

            CCPoint oldPoint = _container.Position;

            float newX = oldPoint.X;
            float newY = oldPoint.Y;
            if (_direction == CCScrollViewDirection.Both || _direction == CCScrollViewDirection.Horizontal)
            {
                newX = Math.Min(newX, max.X);
                newX = Math.Max(newX, min.X);
            }

            if (_direction == CCScrollViewDirection.Both || _direction == CCScrollViewDirection.Vertical)
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

        private void DeaccelerateScrolling(float dt)
        {
            if (_dragging)
            {
                Unschedule(DeaccelerateScrolling);
                return;
            }

            CCPoint maxInset, minInset;

            _container.Position = _container.Position + _scrollDistance;

            if (_bounceable)
            {
                maxInset = _maxInset;
                minInset = _minInset;
            }
            else
            {
                maxInset = MaxContainerOffset;
                minInset = MinContainerOffset;
            }

            //check to see if offset lies within the inset bounds
            float newX = Math.Min(_container.Position.X, maxInset.X);
            newX = Math.Max(newX, minInset.X);
            float newY = Math.Min(_container.Position.Y, maxInset.Y);
            newY = Math.Max(newY, minInset.Y);

            //newX = _container.Position.X;
            //newY = _container.Position.Y;

            _scrollDistance = _scrollDistance - new CCPoint(newX - _container.Position.X, newY - _container.Position.Y);
            _scrollDistance = _scrollDistance * SCROLL_DEACCEL_RATE;
            SetContentOffset(new CCPoint(newX, newY), false);

            if ((Math.Abs(_scrollDistance.X) <= SCROLL_DEACCEL_DIST &&
                 Math.Abs(_scrollDistance.Y) <= SCROLL_DEACCEL_DIST) ||
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

        private void PerformedAnimatedScroll(float dt)
        {
            if (_dragging)
            {
                Unschedule(PerformedAnimatedScroll);
                return;
            }

            if (_delegate != null)
            {
                _delegate.ScrollViewDidScroll(this);
            }
        }

        /**
     * Expire animated scroll delegate calls
     */

        private void StoppedAnimatedScroll(CCNode node)
        {
            Unschedule(PerformedAnimatedScroll);
            // After the animation stopped, "scrollViewDidScroll" should be invoked, this could fix the bug of lack of tableview cells.
            if (_delegate != null)
            {
                _delegate.ScrollViewDidScroll(this);
            }
        }

        public void UpdateInset()
        {
            if (Container != null)
            {
                _maxInset = MaxContainerOffset;
                _maxInset = new CCPoint(_maxInset.X + _viewSize.Width * INSET_RATIO,
                                          _maxInset.Y + _viewSize.Height * INSET_RATIO);
                _minInset = MinContainerOffset;
                _minInset = new CCPoint(_minInset.X - _viewSize.Width * INSET_RATIO,
                                          _minInset.Y - _viewSize.Height * INSET_RATIO);
            }
        }

        private CCRect GetViewRect()
        {
            var rect = new CCRect(0, 0, _viewSize.Width, _viewSize.Height);
            return CCAffineTransform.Transform(rect, NodeToWorldTransform());
        }

        private static float ConvertDistanceFromPointToInch(float pointDis)
        {
            float factor = (CCDrawManager.ScaleX + CCDrawManager.ScaleY) / 2;
            return pointDis * factor / CCDevice.GetDPI();
        }
    }
}