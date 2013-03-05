using System.Diagnostics;
using Microsoft.Xna.Framework;

namespace cocos2d
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

        /// <summary>
        /// whether camera should be limited to certain area
        /// </summary>
        public bool BoundarySet
        {
            get { return m_bBoundarySet; }
            set { m_bBoundarySet = value; }
        }

        public bool InitWithTarget(CCNode pFollowedNode, CCRect rect)
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
            m_obFullScreenSize = new CCPoint(winSize.Width, winSize.Height);
            m_obHalfScreenSize = CCPointExtension.Multiply(m_obFullScreenSize, 0.5f);

            if (m_bBoundarySet)
            {
                m_fLeftBoundary = -((rect.origin.x + rect.size.Width) - m_obFullScreenSize.x);
                m_fRightBoundary = -rect.origin.x;
                m_fTopBoundary = -rect.origin.y;
                m_fBottomBoundary = -((rect.origin.y + rect.size.Height) - m_obFullScreenSize.y);

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

        public override CCObject CopyWithZone(CCZone zone)
        {
            CCZone tempZone = zone;
            CCFollow ret;
            if (tempZone != null && tempZone.m_pCopyObject != null)
            {
                ret = (CCFollow) tempZone.m_pCopyObject;
            }
            else
            {
                ret = new CCFollow();
                tempZone = new CCZone(ret);
            }

            base.CopyWithZone(tempZone);
            ret.m_nTag = m_nTag;

            return ret;
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

                CCPoint tempPos = CCPointExtension.Subtract(m_obHalfScreenSize, m_pobFollowedNode.Position);

                m_pTarget.Position = new CCPoint(
                    MathHelper.Clamp(tempPos.x, m_fLeftBoundary, m_fRightBoundary),
                    MathHelper.Clamp(tempPos.y, m_fBottomBoundary, m_fTopBoundary)
                    );
            }
            else
            {
                m_pTarget.Position = CCPointExtension.Subtract(m_obHalfScreenSize, m_pobFollowedNode.Position);
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

        public static CCFollow Create(CCNode followedNode, CCRect rect)
        {
            var ret = new CCFollow();
            ret.InitWithTarget(followedNode, rect);
            return ret;
        }
    }
}