using System.Diagnostics;
using Microsoft.Xna.Framework;

namespace Cocos2D
{
    public class CCFollow : CCAction
    {
        protected bool m_bBoundaryFullyCovered;

        protected bool m_bBoundarySet;
        protected float m_fBottomBoundary;
        protected float m_fLeftBoundary;
        protected float m_fRightBoundary;
        protected float m_fTopBoundary;
        protected CCPoint m_obFullScreenSize;
        protected CCPoint m_obHalfScreenSize;
        protected CCNode m_pobFollowedNode;

        public CCFollow(CCNode followedNode, CCRect rect)
        {
            InitWithTarget(followedNode, rect);
        }

        protected CCFollow(CCFollow follow) : base(follow)
        {
            m_nTag = follow.m_nTag;
        }

        /// <summary>
        /// whether camera should be limited to certain area
        /// </summary>
        public bool BoundarySet
        {
            get { return m_bBoundarySet; }
            set { m_bBoundarySet = value; }
        }

        private bool InitWithTarget(CCNode pFollowedNode, CCRect rect)
        {
            Debug.Assert(pFollowedNode != null);

            m_pobFollowedNode = pFollowedNode;
            if (rect.Equals(CCRect.Zero))
            {
                m_bBoundarySet = false;
            }
            else
            {
                m_bBoundarySet = true;
            }

            m_bBoundaryFullyCovered = false;

            CCSize winSize = CCDirector.SharedDirector.WinSize;
            m_obFullScreenSize = (CCPoint) winSize;
            m_obHalfScreenSize = m_obFullScreenSize * 0.5f;

            if (m_bBoundarySet)
            {
                m_fLeftBoundary = -((rect.Origin.X + rect.Size.Width) - m_obFullScreenSize.X);
                m_fRightBoundary = -rect.Origin.X;
                m_fTopBoundary = -rect.Origin.Y;
                m_fBottomBoundary = -((rect.Origin.Y + rect.Size.Height) - m_obFullScreenSize.Y);

                if (m_fRightBoundary < m_fLeftBoundary)
                {
                    // screen width is larger than world's boundary width
                    //set both in the middle of the world
                    m_fRightBoundary = m_fLeftBoundary = (m_fLeftBoundary + m_fRightBoundary) / 2;
                }
                if (m_fTopBoundary < m_fBottomBoundary)
                {
                    // screen width is larger than world's boundary width
                    //set both in the middle of the world
                    m_fTopBoundary = m_fBottomBoundary = (m_fTopBoundary + m_fBottomBoundary) / 2;
                }

                if ((m_fTopBoundary == m_fBottomBoundary) && (m_fLeftBoundary == m_fRightBoundary))
                {
                    m_bBoundaryFullyCovered = true;
                }
            }

            return true;
        }

        public override object Copy(ICCCopyable zone)
        {
            if (zone != null)
            {
                var ret = (CCFollow) zone;
                base.Copy(zone);
                ret.m_nTag = m_nTag;
                return ret;
            }
            else
            {
                return new CCFollow(this);
            }
        }

        public override void Step(float dt)
        {
            if (m_bBoundarySet)
            {
                // whole map fits inside a single screen, no need to modify the position - unless map boundaries are increased
                if (m_bBoundaryFullyCovered)
                {
                    return;
                }

                CCPoint tempPos = m_obHalfScreenSize - m_pobFollowedNode.Position;

                m_pTarget.Position = new CCPoint(
                    MathHelper.Clamp(tempPos.X, m_fLeftBoundary, m_fRightBoundary),
                    MathHelper.Clamp(tempPos.Y, m_fBottomBoundary, m_fTopBoundary)
                    );
            }
            else
            {
                m_pTarget.Position = m_obHalfScreenSize - m_pobFollowedNode.Position;
            }
        }

        public override bool IsDone
        {
            get { return !m_pobFollowedNode.IsRunning; }
        }

        public override void Stop()
        {
            m_pTarget = null;
            base.Stop();
        }
    }
}