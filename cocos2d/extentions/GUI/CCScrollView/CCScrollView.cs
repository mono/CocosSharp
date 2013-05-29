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

    public interface CCScrollViewDelegate
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

        protected bool m_bBounceable;

        protected bool m_bClippingToBounds;
        protected bool m_bDragging;
        protected bool m_bTouchMoved;
        protected CCScrollViewDirection m_eDirection;
        protected CCPoint m_fMaxInset;
        protected float m_fMaxScale;
        protected CCPoint m_fMinInset;
        protected float m_fMinScale;
        protected float m_fTouchLength;
        protected CCNode m_pContainer;
        protected CCScrollViewDelegate m_pDelegate;
        protected List<CCTouch> m_pTouches;
        protected CCPoint m_tContentOffset;
        protected CCPoint m_tScrollDistance;

        //Touch point
        protected CCPoint m_tTouchPoint;
        protected CCSize m_tViewSize;

        protected int m_uPagesCount;

        /**
         * scissor rect for parent, just for restoring GL_SCISSOR_BOX
         */
        CCRect m_tParentScissorRect;
        bool m_bScissorRestored;

        public CCScrollView()
        {
            m_eDirection = CCScrollViewDirection.Both;
            Init();
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

        public int PagesCount
        {
            get { return m_uPagesCount; }
            set { m_uPagesCount = value; }
        }

        public override CCSize ContentSize
        {
            get { return m_pContainer.ContentSize; }
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
                    m_bDragging = false;
                    m_bTouchMoved = false;
                    m_pTouches.Clear();
                }
            }
        }

        public float ZoomScale
        {
            get { return m_pContainer.Scale; }
            set
            {
                if (m_pContainer.Scale != value)
                {
                    CCPoint center;

                    if (m_fTouchLength == 0.0f)
                    {
                        center = new CCPoint(m_tViewSize.Width * 0.5f, m_tViewSize.Height * 0.5f);
                        center = ConvertToWorldSpace(center);
                    }
                    else
                    {
                        center = m_tTouchPoint;
                    }

                    CCPoint oldCenter = m_pContainer.ConvertToNodeSpace(center);
                    m_pContainer.Scale = Math.Max(m_fMinScale, Math.Min(m_fMaxScale, value));
                    CCPoint newCenter = m_pContainer.ConvertToWorldSpace(oldCenter);

                    CCPoint offset = center - newCenter;
                    if (m_pDelegate != null)
                    {
                        m_pDelegate.ScrollViewDidZoom(this);
                    }
                    SetContentOffset(m_pContainer.Position + offset, false);
                }
            }
        }

        public bool Bounceable
        {
            get { return m_bBounceable; }
            set { m_bBounceable = value; }
        }

        /**
     * size to clip. CCNode boundingBox uses contentSize directly.
     * It's semantically different what it actually means to common scroll views.
     * Hence, this scroll view will use a separate size property.
     */

        public CCSize ViewSize
        {
            get { return m_tViewSize; }
            set
            {
                m_tViewSize = value;
                base.ContentSize = value;
            }
        }

        public CCNode Container
        {
            set
            {
                if (value == null)
                {
                    return;
                }

                RemoveAllChildrenWithCleanup(true);
                m_pContainer = value;

                m_pContainer.IgnoreAnchorPointForPosition = false;
                m_pContainer.AnchorPoint = CCPoint.Zero;

                AddChild(m_pContainer);

                ViewSize = m_tViewSize;
            }
            get { return m_pContainer; }
        }

        /**
     * direction allowed to scroll. CCScrollViewDirectionBoth by default.
     */

        public CCScrollViewDirection Direction
        {
            get { return m_eDirection; }
            set { m_eDirection = value; }
        }

        public CCScrollViewDelegate Delegate
        {
            get { return m_pDelegate; }
            set { m_pDelegate = value; }
        }

        public bool ClippingToBounds
        {
            get { return m_bClippingToBounds; }
            set { m_bClippingToBounds = value; }
        }

        public override bool Init()
        {
            return InitWithViewSize(new CCSize(200, 200), null);
        }

        public override void RegisterWithTouchDispatcher()
        {
            //TODO: use CCLayer::getTouchPriority()
            CCDirector.SharedDirector.TouchDispatcher.AddTargetedDelegate(this, 0, false);
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
                m_pContainer = container;

                if (m_pContainer == null)
                {
                    m_pContainer = new CCLayer();
                    m_pContainer.IgnoreAnchorPointForPosition = false;
                    m_pContainer.AnchorPoint = CCPoint.Zero;
                }

                ViewSize = size;

                TouchEnabled = true;
                m_pTouches = new List<CCTouch>();
                m_pDelegate = null;
                m_bBounceable = true;
                m_bClippingToBounds = true;
                //m_pContainer->setContentSize(CCSizeZero);
                m_eDirection = CCScrollViewDirection.Both;
                m_pContainer.Position = new CCPoint(0.0f, 0.0f);
                m_fTouchLength = 0.0f;

                AddChild(m_pContainer);
                m_fMinScale = m_fMaxScale = 1.0f;

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
                if (!m_bBounceable)
                {
                    CCPoint minOffset = MinContainerOffset;
                    CCPoint maxOffset = MaxContainerOffset;

                    offset.X = Math.Max(minOffset.X, Math.Min(maxOffset.X, offset.X));
                    offset.Y = Math.Max(minOffset.Y, Math.Min(maxOffset.Y, offset.Y));
                }

                m_pContainer.Position = offset;

                if (m_pDelegate != null)
                {
                    m_pDelegate.ScrollViewDidScroll(this);
                }
            }
        }

        public CCPoint GetContentOffset()
        {
            return m_pContainer.Position;
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
            m_pContainer.RunAction(CCSequence.FromActions(scroll, expire));
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
                if (m_pContainer.Scale != s)
                {
                    CCActionTween scaleAction = new CCActionTween (dt, "zoomScale", m_pContainer.Scale, s);
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
                return new CCPoint(m_tViewSize.Width - m_pContainer.ContentSize.Width * m_pContainer.ScaleX,
                                   m_tViewSize.Height - m_pContainer.ContentSize.Height * m_pContainer.ScaleY);
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
            m_pContainer.PauseSchedulerAndActions();

            var pChildren = m_pContainer.Children;

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
            var pChildren = m_pContainer.Children;

            if (pChildren != null && pChildren.count > 0)
            {
                for (int i = 0; i < pChildren.count; i++)
                {
                    pChildren.Elements[i].ResumeSchedulerAndActions();
                }
            }

            m_pContainer.ResumeSchedulerAndActions();
        }


        public bool IsDragging
        {
            get { return m_bDragging; }
        }

        public bool IsTouchMoved
        {
            get { return m_bTouchMoved; }
        }

        /** override functions */
        // optional
        public override bool TouchBegan(CCTouch pTouch, CCEvent pEvent)
        {
            if (!Visible)
            {
                return false;
            }

            CCRect frame = GetViewRect();

            //dispatcher does not know about clipping. reject touches outside visible bounds.
            if (m_pTouches.Count > 2 ||
                m_bTouchMoved ||
                !frame.ContainsPoint(m_pContainer.ConvertToWorldSpace(m_pContainer.ConvertTouchToNodeSpace(pTouch))))
            {
                return false;
            }

            if (!m_pTouches.Contains(pTouch))
            {
                m_pTouches.Add(pTouch);
            }

            if (m_pTouches.Count == 1)
            {
                // scrolling
                m_tTouchPoint = ConvertTouchToNodeSpace(pTouch);
                m_bTouchMoved = false;
                m_bDragging = true; //dragging started
                m_tScrollDistance = CCPoint.Zero;
                m_fTouchLength = 0.0f;
            }
            else if (m_pTouches.Count == 2)
            {
                m_tTouchPoint = CCPoint.Midpoint(ConvertTouchToNodeSpace(m_pTouches[0]),
                                                             ConvertTouchToNodeSpace(m_pTouches[1]));
                m_fTouchLength = CCPoint.Distance(m_pContainer.ConvertTouchToNodeSpace(m_pTouches[0]),
                                                              m_pContainer.ConvertTouchToNodeSpace(m_pTouches[1]));
                m_bDragging = false;
            }
            return true;
        }

        public override void TouchMoved(CCTouch touch, CCEvent pEvent)
        {
            if (!Visible)
            {
                return;
            }

            if (m_pTouches.Contains(touch))
            {
                if (m_pTouches.Count == 1 && m_bDragging)
                {// scrolling
                    CCPoint moveDistance, newPoint; //, maxInset, minInset;
                    CCRect frame;
                    float newX, newY;

                    frame = GetViewRect();

                    newPoint = ConvertTouchToNodeSpace(m_pTouches[0]);
                    moveDistance = newPoint - m_tTouchPoint;

                    float dis = 0.0f;
                    if (m_eDirection == CCScrollViewDirection.Vertical)
                    {
                        dis = moveDistance.Y;
                    }
                    else if (m_eDirection == CCScrollViewDirection.Horizontal)
                    {
                        dis = moveDistance.X;
                    }
                    else
                    {
                        dis = (float)Math.Sqrt(moveDistance.X * moveDistance.X + moveDistance.Y * moveDistance.Y);
                    }

                    if (!m_bTouchMoved && Math.Abs(ConvertDistanceFromPointToInch(dis)) < MOVE_INCH)
                    {
                        //CCLOG("Invalid movement, distance = [%f, %f], disInch = %f", moveDistance.x, moveDistance.y);
                        return;
                    }

                    if (!m_bTouchMoved)
                    {
                        moveDistance = CCPoint.Zero;
                    }

                    m_tTouchPoint = newPoint;
                    m_bTouchMoved = true;

                    if (frame.ContainsPoint(ConvertToWorldSpace(newPoint)))
                    {
                        switch (m_eDirection)
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

                        newX = m_pContainer.Position.X + moveDistance.X;
                        newY = m_pContainer.Position.Y + moveDistance.Y;

                        m_tScrollDistance = moveDistance;
                        SetContentOffset(new CCPoint(newX, newY));
                    }
                }
                else if (m_pTouches.Count == 2 && !m_bDragging)
                {
                    float len = CCPoint.Distance(m_pContainer.ConvertTouchToNodeSpace(m_pTouches[0]),
                                                             m_pContainer.ConvertTouchToNodeSpace(m_pTouches[1]));
                    ZoomScale = ZoomScale * len / m_fTouchLength;
                }
            }
        }

        public override void TouchEnded(CCTouch touch, CCEvent pEvent)
        {
            if (!Visible)
            {
                return;
            }

            if (m_pTouches.Contains(touch))
            {
                if (m_pTouches.Count == 1 && m_bTouchMoved)
                {
                    Schedule(DeaccelerateScrolling);
                }
                m_pTouches.Remove(touch);
            }

            if (m_pTouches.Count == 0)
            {
                m_bDragging = false;
                m_bTouchMoved = false;
            }
        }

        public override void TouchCancelled(CCTouch touch, CCEvent pEvent)
        {
            if (!Visible)
            {
                return;
            }
            m_pTouches.Remove(touch);
            if (m_pTouches.Count == 0)
            {
                m_bDragging = false;
                m_bTouchMoved = false;
            }
        }

        /**
     * Determines whether it clips its children or not.
     */

        public override void Visit()
        {
            // quick return if not visible
            if (!Visible)
            {
                return;
            }

            CCDrawManager.PushMatrix();

            if (m_pGrid != null && m_pGrid.Active)
            {
                m_pGrid.BeforeDraw();
                TransformAncestors();
            }

            Transform();
            BeforeDraw();

            if (m_pChildren != null)
            {
                CCNode[] arrayData = m_pChildren.Elements;
                int count = m_pChildren.count;
                int i = 0;

                // draw children zOrder < 0
                for (; i < count; i++)
                {
                    CCNode child = arrayData[i];
                    if (child.m_nZOrder < 0)
                    {
                        child.Visit();
                    }
                    else
                    {
                        break;
                    }
                }

                // this draw
                Draw();

                // draw children zOrder >= 0
                for (; i < count; i++)
                {
                    arrayData[i].Visit();
                }
            }
            else
            {
                Draw();
            }

            AfterDraw();
            if (m_pGrid != null && m_pGrid.Active)
            {
                m_pGrid.AfterDraw(this);
            }

            CCDrawManager.PopMatrix();
        }

        public override void AddChild(CCNode child, int zOrder, int tag)
        {
            child.IgnoreAnchorPointForPosition = false;
            child.AnchorPoint = CCPoint.Zero;
            if (m_pContainer != child)
            {
                m_pContainer.AddChild(child, zOrder, tag);
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

            CCPoint oldPoint = m_pContainer.Position;

            float newX = oldPoint.X;
            float newY = oldPoint.Y;
            if (m_eDirection == CCScrollViewDirection.Both || m_eDirection == CCScrollViewDirection.Horizontal)
            {
                newX = Math.Min(newX, max.X);
                newX = Math.Max(newX, min.X);
            }

            if (m_eDirection == CCScrollViewDirection.Both || m_eDirection == CCScrollViewDirection.Vertical)
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
            if (m_bDragging)
            {
                Unschedule(DeaccelerateScrolling);
                return;
            }

            CCPoint maxInset, minInset;

            m_pContainer.Position = m_pContainer.Position + m_tScrollDistance;

            if (m_bBounceable)
            {
                maxInset = m_fMaxInset;
                minInset = m_fMinInset;
            }
            else
            {
                maxInset = MaxContainerOffset;
                minInset = MinContainerOffset;
            }

            //check to see if offset lies within the inset bounds
            float newX = Math.Min(m_pContainer.Position.X, maxInset.X);
            newX = Math.Max(newX, minInset.X);
            float newY = Math.Min(m_pContainer.Position.Y, maxInset.Y);
            newY = Math.Max(newY, minInset.Y);

            newX = m_pContainer.Position.X;
            newY = m_pContainer.Position.Y;

            m_tScrollDistance = m_tScrollDistance - new CCPoint(newX - m_pContainer.Position.X, newY - m_pContainer.Position.Y);
            m_tScrollDistance = m_tScrollDistance * SCROLL_DEACCEL_RATE;
            SetContentOffset(new CCPoint(newX, newY), false);

            if ((Math.Abs(m_tScrollDistance.X) <= SCROLL_DEACCEL_DIST &&
                 Math.Abs(m_tScrollDistance.Y) <= SCROLL_DEACCEL_DIST) ||
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
            if (m_bDragging)
            {
                Unschedule(PerformedAnimatedScroll);
                return;
            }

            if (m_pDelegate != null)
            {
                m_pDelegate.ScrollViewDidScroll(this);
            }
        }

        /**
     * Expire animated scroll delegate calls
     */

        private void StoppedAnimatedScroll(CCNode node)
        {
            Unschedule(PerformedAnimatedScroll);
            // After the animation stopped, "scrollViewDidScroll" should be invoked, this could fix the bug of lack of tableview cells.
            if (m_pDelegate != null)
            {
                m_pDelegate.ScrollViewDidScroll(this);
            }
        }

        /**
     * clip this view so that outside of the visible bounds can be hidden.
     */

        private void BeforeDraw()
        {
            if (m_bClippingToBounds)
            {
                m_bScissorRestored = false;
                CCRect frame = GetViewRect();
                if (CCDrawManager.ScissorRectEnabled)
                {
                    m_bScissorRestored = true;
                    m_tParentScissorRect = CCDrawManager.ScissorRect;
                    //set the intersection of m_tParentScissorRect and frame as the new scissor rect
                    if (frame.IntersectsRect(m_tParentScissorRect))
                    {
                        float x = Math.Max(frame.Origin.X, m_tParentScissorRect.Origin.X);
                        float y = Math.Max(frame.Origin.Y, m_tParentScissorRect.Origin.Y);
                        float xx = Math.Min(frame.Origin.X + frame.Size.Width, m_tParentScissorRect.Origin.X + m_tParentScissorRect.Size.Width);
                        float yy = Math.Min(frame.Origin.Y + frame.Size.Height, m_tParentScissorRect.Origin.Y + m_tParentScissorRect.Size.Height);
                        CCDrawManager.SetScissorInPoints(x, y, xx - x, yy - y);
                    }
                }
                else
                {
                    CCDrawManager.ScissorRectEnabled = true;
                    CCDrawManager.SetScissorInPoints(frame.Origin.X, frame.Origin.Y, frame.Size.Width, frame.Size.Height);
                }
            }
        }

        /**
     * retract what's done in beforeDraw so that there's no side effect to
     * other nodes.
     */

        private void AfterDraw()
        {
            if (m_bClippingToBounds)
            {
                if (m_bScissorRestored)
                {
                    CCDrawManager.SetScissorInPoints(m_tParentScissorRect.Origin.X, m_tParentScissorRect.Origin.Y, m_tParentScissorRect.Size.Width, m_tParentScissorRect.Size.Height);
                }
                else
                {
                    CCDrawManager.ScissorRectEnabled = false;
                }
            }
        }

        public void UpdateInset()
        {
            if (Container != null)
            {
                m_fMaxInset = MaxContainerOffset;
                m_fMaxInset = new CCPoint(m_fMaxInset.X + m_tViewSize.Width * INSET_RATIO,
                                          m_fMaxInset.Y + m_tViewSize.Height * INSET_RATIO);
                m_fMinInset = MinContainerOffset;
                m_fMinInset = new CCPoint(m_fMinInset.X - m_tViewSize.Width * INSET_RATIO,
                                          m_fMinInset.Y - m_tViewSize.Height * INSET_RATIO);
            }
        }

        private CCRect GetViewRect()
        {
            CCPoint screenPos = ConvertToWorldSpace(CCPoint.Zero);

            float scaleX = ScaleX;
            float scaleY = ScaleY;

            for (CCNode p = m_pParent; p != null; p = p.Parent)
            {
                scaleX *= p.ScaleX;
                scaleY *= p.ScaleY;
            }

            // Support negative scaling. Not doing so causes intersectsRect calls
            // (eg: to check if the touch was within the bounds) to return false.
            // Note, CCNode::getScale will assert if X and Y scales are different.
            if (scaleX < 0f)
            {
                screenPos.X += m_tViewSize.Width * scaleX;
                scaleX = -scaleX;
            }
            if (scaleY < 0f)
            {
                screenPos.Y += m_tViewSize.Height * scaleY;
                scaleY = -scaleY;
            }

            return new CCRect(screenPos.X, screenPos.Y, m_tViewSize.Width * scaleX, m_tViewSize.Height * scaleY);
        }

        private static float ConvertDistanceFromPointToInch(float pointDis)
        {
            float factor = (CCDrawManager.ScaleX + CCDrawManager.ScaleY) / 2;
            return pointDis * factor / CCDevice.GetDPI();
        }
    }
}