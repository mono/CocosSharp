using System;

namespace CocosSharp
{
    public class CCBezierBy : CCActionInterval
    {
        protected CCBezierConfig m_sConfig;
        protected CCPoint m_startPosition;
        protected CCPoint m_previousPosition;


        #region Constructors

        public CCBezierBy(float t, CCBezierConfig c) : base(t)
        {
            InitCCBezierBy(c);
        }

        // Perform a deep copy of CCBezierBy
        protected CCBezierBy(CCBezierBy bezierBy) : base(bezierBy)
        {
            InitCCBezierBy(bezierBy.m_sConfig);
        }

        private void InitCCBezierBy(CCBezierConfig c)
        {
            m_sConfig = c;
        }

        #endregion Constructors


        public override object Copy(ICCCopyable zone)
        {
            return new CCBezierBy(this);
        }

        protected internal override void StartWithTarget(CCNode target)
        {
            base.StartWithTarget(target);
            m_previousPosition = m_startPosition = target.Position;
        }

        public override void Update(float time)
        {
            if (m_pTarget != null)
            {
                float xa = 0;
                float xb = m_sConfig.ControlPoint1.X;
                float xc = m_sConfig.ControlPoint2.X;
                float xd = m_sConfig.EndPosition.X;

                float ya = 0;
                float yb = m_sConfig.ControlPoint1.Y;
                float yc = m_sConfig.ControlPoint2.Y;
                float yd = m_sConfig.EndPosition.Y;

                float x = CCSplineMath.CubicBezier(xa, xb, xc, xd, time);
                float y = CCSplineMath.CubicBezier(ya, yb, yc, yd, time);

                CCPoint currentPos = m_pTarget.Position;
                CCPoint diff = currentPos - m_previousPosition;
                m_startPosition = m_startPosition + diff;
        
                CCPoint newPos = m_startPosition + new CCPoint(x, y);
                m_pTarget.Position = newPos;

                m_previousPosition = newPos;
            }
        }

        public override CCFiniteTimeAction Reverse()
        {
            CCBezierConfig r;

            r.EndPosition = -m_sConfig.EndPosition;
            r.ControlPoint1 = m_sConfig.ControlPoint2 + -m_sConfig.EndPosition;
            r.ControlPoint2 = m_sConfig.ControlPoint1 + -m_sConfig.EndPosition;

            var action = new CCBezierBy(m_fDuration, r);
            return action;
        }
    }

    public struct CCBezierConfig
    {
        public CCPoint ControlPoint1;
        public CCPoint ControlPoint2;
        public CCPoint EndPosition;
    }
}