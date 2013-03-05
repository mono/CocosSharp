using System;
using System.Collections.Generic;

namespace cocos2d
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

        public CCScrollView()
        {
            m_eDirection = CCScrollViewDirection.Both;
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
                if (m_pContainer == null)
                {
                    InitWithViewSize(value, null);
                }
                else
                {
                    m_pContainer.ContentSize = value;
                }
                UpdateInset();
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

                    CCPoint offset = CCPointExtension.Subtract(center, newCenter);
                    if (m_pDelegate != null)
                    {
                        m_pDelegate.ScrollViewDidZoom(this);
                    }
                    SetContentOffset(CCPointExtension.Add(m_pContainer.Position, offset), false);
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

                m_fMaxInset = MaxContainerOffset;
                m_fMaxInset = new CCPoint(m_fMaxInset.x + m_tViewSize.Width * INSET_RATIO,
                                          m_fMaxInset.y + m_tViewSize.Height * INSET_RATIO);
                m_fMinInset = MinContainerOffset;
                m_fMinInset = new CCPoint(m_fMinInset.x - m_tViewSize.Width * INSET_RATIO,
                                          m_fMinInset.y - m_tViewSize.Height * INSET_RATIO);
            }
        }

        public CCNode Container
        {
            set
            {
                RemoveAllChildrenWithCleanup(true);

                if (value == null)
                {
                    return;
                }

                m_pContainer = value;

                m_pContainer.IgnoreAnchorPointForPosition = false;
                m_pContainer.AnchorPoint = new CCPoint(0.0f, 0.0f);

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
            CCDirector.SharedDirector.TouchDispatcher.AddTargetedDelegate(this, 0, false);
        }

        /**
     * Returns an autoreleased scroll view object.
     *
     * @param size view size
     * @param container parent object
     * @return autoreleased scroll view object
     */

        public static CCScrollView Create(CCSize size, CCNode container)
        {
            var pRet = new CCScrollView();
            pRet.InitWithViewSize(size, container);
            return pRet;
        }

        /**
     * Returns an autoreleased scroll view object.
     *
     * @param size view size
     * @param container parent object
     * @return autoreleased scroll view object
     */

        public new static CCScrollView Create()
        {
            var pRet = new CCScrollView();
            pRet.Init();
            return pRet;
        }

        /**
     * Returns a scroll view object
     *
     * @param size view size
     * @param container parent object
     * @return scroll view object
     */

        public bool InitWithViewSize(CCSize size, CCNode container)
        {
            m_pContainer = container ?? CCLayer.Create();

            if (base.Init())
            {
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

                    offset.x = Math.Max(minOffset.x, Math.Min(maxOffset.x, offset.x));
                    offset.y = Math.Max(minOffset.y, Math.Min(maxOffset.y, offset.y));
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
            CCMoveTo scroll = CCMoveTo.Create(dt, offset);
            CCCallFuncN expire = CCCallFuncN.Create(StoppedAnimatedScroll);
            m_pContainer.RunAction(CCSequence.Create(scroll, expire));
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
                    CCActionTween scaleAction = CCActionTween.Create(dt, "zoomScale", m_pContainer.Scale, s);
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

            var viewRect = new CCRect(-offset.x / scale, -offset.y / scale, size.Width / scale, size.Height / scale);

            return viewRect.IntersectsRect(node.BoundingBox);
        }

        /**
     * Provided to make scroll view compatible with SWLayer's pause method
     */

        public void Pause(CCObject sender)
        {
            m_pContainer.PauseSchedulerAndActions();

            RawList<CCNode> pChildren = m_pContainer.Children;

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

        public void Resume(CCObject sender)
        {
            RawList<CCNode> pChildren = m_pContainer.Children;

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

            CCPoint frameOriginal = Parent.ConvertToWorldSpace(Position);
            var frame = new CCRect(frameOriginal.x, frameOriginal.y, m_tViewSize.Width, m_tViewSize.Height);

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
                m_tScrollDistance = new CCPoint(0.0f, 0.0f);
                m_fTouchLength = 0.0f;
            }
            else if (m_pTouches.Count == 2)
            {
                m_tTouchPoint = CCPointExtension.Midpoint(ConvertTouchToNodeSpace(m_pTouches[0]),
                                                             ConvertTouchToNodeSpace(m_pTouches[1]));
                m_fTouchLength = CCPointExtension.Distance(m_pContainer.ConvertTouchToNodeSpace(m_pTouches[0]),
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
                {
                    // scrolling

                    m_bTouchMoved = true;
                    CCPoint frameOriginal = Parent.ConvertToWorldSpace(Position);
                    var frame = new CCRect(frameOriginal.x, frameOriginal.y, m_tViewSize.Width, m_tViewSize.Height);
                    CCPoint newPoint = ConvertTouchToNodeSpace(m_pTouches[0]);
                    CCPoint moveDistance = CCPointExtension.Subtract(newPoint, m_tTouchPoint);
                    m_tTouchPoint = newPoint;

                    if (frame.ContainsPoint(ConvertToWorldSpace(newPoint)))
                    {
                        switch (m_eDirection)
                        {
                            case CCScrollViewDirection.Vertical:
                                moveDistance = new CCPoint(0.0f, moveDistance.y);
                                break;
                            case CCScrollViewDirection.Horizontal:
                                moveDistance = new CCPoint(moveDistance.x, 0.0f);
                                break;
                        }

                        m_pContainer.Position = CCPointExtension.Add(m_pContainer.Position, moveDistance);

                        CCPoint maxInset = m_fMaxInset;
                        CCPoint minInset = m_fMinInset;


                        //check to see if offset lies within the inset bounds
                        float newX = Math.Min(m_pContainer.Position.x, maxInset.x);
                        newX = Math.Max(newX, minInset.x);
                        float newY = Math.Min(m_pContainer.Position.y, maxInset.y);
                        newY = Math.Max(newY, minInset.y);

                        m_tScrollDistance = CCPointExtension.Subtract(moveDistance,
                                                                    new CCPoint(newX - m_pContainer.Position.x, newY - m_pContainer.Position.y));
                        SetContentOffset(new CCPoint(newX, newY), false);
                    }
                }
                else if (m_pTouches.Count == 2 && !m_bDragging)
                {
                    float len = CCPointExtension.Distance(m_pContainer.ConvertTouchToNodeSpace(m_pTouches[0]),
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
                    if (m_uPagesCount > 0)
                    {
                        if (Direction == CCScrollViewDirection.Horizontal)
                        {
                            var curPage = (int) Math.Round(-Container.Position.x / m_tViewSize.Width);
                            curPage += (m_tScrollDistance.x > 10) ? -1 : 0 + ((m_tScrollDistance.x < -10) ? 1 : 0);
                            curPage = (curPage < 0) ? 0 : curPage;
                            curPage = (curPage > m_uPagesCount - 1) ? m_uPagesCount - 1 : curPage;

                            SetContentOffset(new CCPoint(-curPage * m_tViewSize.Width, 0), true);
                        }

                        else if (Direction == CCScrollViewDirection.Vertical)
                        {
                            var curPage = (int) Math.Round(-Container.Position.y / m_tViewSize.Height);
                            curPage += (m_tScrollDistance.y > 10) ? -1 : 0 + ((m_tScrollDistance.y < -10) ? 1 : 0);
                            curPage = (curPage < 0) ? 0 : curPage;
                            curPage = (curPage > m_uPagesCount - 1) ? m_uPagesCount - 1 : curPage;

                            SetContentOffset(new CCPoint(0, -curPage * m_tViewSize.Height), true);
                        }
                    }
                    else
                    {
                        Schedule(DeaccelerateScrolling);
                    }
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

            DrawManager.PushMatrix();
            //kmGLPushMatrix();

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

            DrawManager.PopMatrix();
            //kmGLPopMatrix();
/*
			// draw bounding box
			CCRect box = m_pContainer.boundingBox;
			var v = new[]
                {
                    new CCPoint(box.origin.x, box.origin.y),
                    new CCPoint(box.origin.x + box.size.width, box.origin.y),
                    new CCPoint(box.origin.x + box.size.width, box.origin.y + box.size.height),
                    new CCPoint(box.origin.x, box.origin.y + box.size.height),
                };
			CCDrawingPrimitives.ccDrawPoly(v, 4, true, new ccColor4F(255, 0, 0, 255));
*/
        }

        public override void AddChild(CCNode child, int zOrder, int tag)
        {
            //child.ignoreAnchorPointForPosition = false;
            //child.anchorPoint = new CCPoint(0.0f, 0.0f);
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

            float newX = oldPoint.x;
            float newY = oldPoint.y;
            if (m_eDirection == CCScrollViewDirection.Both || m_eDirection == CCScrollViewDirection.Horizontal)
            {
                newX = Math.Min(newX, max.x);
                newX = Math.Max(newX, min.x);
            }

            if (m_eDirection == CCScrollViewDirection.Both || m_eDirection == CCScrollViewDirection.Vertical)
            {
                newY = Math.Min(newY, max.y);
                newY = Math.Max(newY, min.y);
            }

            if (newY != oldPoint.y || newX != oldPoint.x)
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

            m_pContainer.Position = CCPointExtension.Add(m_pContainer.Position, m_tScrollDistance);

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
            float newX = Math.Min(m_pContainer.Position.x, maxInset.x);
            newX = Math.Max(newX, minInset.x);
            float newY = Math.Min(m_pContainer.Position.y, maxInset.y);
            newY = Math.Max(newY, minInset.y);

            m_tScrollDistance = CCPointExtension.Subtract(m_tScrollDistance, new CCPoint(newX - m_pContainer.Position.x, newY - m_pContainer.Position.y));
            m_tScrollDistance = CCPointExtension.Multiply(m_tScrollDistance, SCROLL_DEACCEL_RATE);
            SetContentOffset(new CCPoint(newX, newY), false);

            if ((Math.Abs(m_tScrollDistance.x) <= SCROLL_DEACCEL_DIST &&
                 Math.Abs(m_tScrollDistance.y) <= SCROLL_DEACCEL_DIST) ||
                newX == maxInset.x || newX == minInset.x ||
                newY == maxInset.y || newY == minInset.y)
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
        }

        /**
     * clip this view so that outside of the visible bounds can be hidden.
     */

        private void BeforeDraw()
        {
            if (m_bClippingToBounds)
            {
                CCPoint screenPos = Parent.ConvertToWorldSpace(Position);

                float s = Scale;

                CCDirector director = CCDirector.SharedDirector;
                s *= director.ContentScaleFactor;

                CCSize winSize = CCDirector.SharedDirector.WinSize;

                DrawManager.ScissorRectEnabled = true;
                DrawManager.SetScissorInPoints(screenPos.x, winSize.Height - (screenPos.y + m_tViewSize.Height * s), m_tViewSize.Width * s,
                                               m_tViewSize.Height * s);
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
                DrawManager.ScissorRectEnabled = false;
            }
        }

        public void UpdateInset()
        {
            if (m_pContainer != null)
            {
                m_fMaxInset = MaxContainerOffset;
                m_fMaxInset = new CCPoint(m_fMaxInset.x + m_tViewSize.Width * INSET_RATIO,
                                          m_fMaxInset.y + m_tViewSize.Height * INSET_RATIO);
                m_fMinInset = MinContainerOffset;
                m_fMinInset = new CCPoint(m_fMinInset.x - m_tViewSize.Width * INSET_RATIO,
                                          m_fMinInset.y - m_tViewSize.Height * INSET_RATIO);
            }
        }
    }
}