using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace cocos2d
{
    public class CCCardinalSplineTo : CCActionInterval
    {
        protected float m_fDeltaT;
        protected float m_fTension;
        protected List<CCPoint> m_pPoints;

        public List<CCPoint> Points
        {
            get { return m_pPoints; }
            set { m_pPoints = value; }
        }

        public static CCCardinalSplineTo Create(float duration, List<CCPoint> points, float tension)
        {
            var ret = new CCCardinalSplineTo();
            ret.InitWithDuration(duration, points, tension);
            return ret;
        }

        public bool InitWithDuration(float duration, List<CCPoint> points, float tension)
        {
            Debug.Assert(points.Count > 0, "Invalid configuration. It must at least have one control point");

            if (base.InitWithDuration(duration))
            {
                Points = points;
                m_fTension = tension;

                return true;
            }

            return false;
        }

        public override CCObject CopyWithZone(CCZone pZone)
        {
            CCCardinalSplineTo pRet;
            if (pZone != null && pZone.m_pCopyObject != null) //in case of being called at sub class
            {
                pRet = (CCCardinalSplineTo) (pZone.m_pCopyObject);
            }
            else
            {
                pRet = new CCCardinalSplineTo();
                pZone = new CCZone(pRet);
            }

            base.CopyWithZone(pZone);

            pRet.InitWithDuration(Duration, m_pPoints, m_fTension);

            return pRet;
        }

        public override void StartWithTarget(CCNode target)
        {
            base.StartWithTarget(target);

            m_fDeltaT = 1f / m_pPoints.Count;
        }

        public override void Update(float time)
        {
            int p;
            float lt;

            // border
            if (time == 1)
            {
                p = m_pPoints.Count - 1;
                lt = 1;
            }
            else
            {
                p = (int) (time / m_fDeltaT);
                lt = (time - m_fDeltaT * p) / m_fDeltaT;
            }

            // Interpolate    
            var c = m_pPoints.Count - 1;
            CCPoint pp0 = m_pPoints[Math.Min(c, Math.Max(p - 1, 0))];
            CCPoint pp1 = m_pPoints[Math.Min(c, Math.Max(p + 0, 0))];
            CCPoint pp2 = m_pPoints[Math.Min(c, Math.Max(p + 1, 0))];
            CCPoint pp3 = m_pPoints[Math.Min(c, Math.Max(p + 2, 0))];

            CCPoint newPos = Spline.CCCardinalSplineAt(pp0, pp1, pp2, pp3, m_fTension, lt);

            UpdatePosition(newPos);
        }

        public override CCFiniteTimeAction Reverse()
        {
            List<CCPoint> pReverse = m_pPoints.ToList();
            pReverse.Reverse();

            return Create(m_fDuration, pReverse, m_fTension);
        }

        public virtual void UpdatePosition(CCPoint newPos)
        {
            m_pTarget.Position = newPos;
        }
    }

    public class CCCardinalSplineBy : CCCardinalSplineTo
    {
        protected CCPoint m_startPosition;

        public new static CCCardinalSplineBy Create(float duration, List<CCPoint> points, float tension)
        {
            var ret = new CCCardinalSplineBy();
            ret.InitWithDuration(duration, points, tension);
            return ret;
        }

        public override void StartWithTarget(CCNode target)
        {
            base.StartWithTarget(target);
            m_startPosition = target.Position;
        }

        public override CCFiniteTimeAction Reverse()
        {
            List<CCPoint> copyConfig = m_pPoints.ToList();

            //
            // convert "absolutes" to "diffs"
            //
            CCPoint p = copyConfig[0];

            for (int i = 1; i < copyConfig.Count; ++i)
            {
                CCPoint current = copyConfig[i];
                CCPoint diff = CCPointExtension.ccpSub(current, p);
                copyConfig[i] = diff;

                p = current;
            }

            // convert to "diffs" to "reverse absolute"
            copyConfig.Reverse();

            // 1st element (which should be 0,0) should be here too

            p = copyConfig[copyConfig.Count - 1];
            copyConfig.RemoveAt(copyConfig.Count - 1);

            p = CCPointExtension.ccpNeg(p);
            copyConfig.Insert(0, p);

            for (int i = 1; i < copyConfig.Count; ++i)
            {
                CCPoint current = copyConfig[i];
                current = CCPointExtension.ccpNeg(current);
                CCPoint abs = CCPointExtension.ccpAdd(current, p);
                copyConfig[i] = abs;

                p = abs;
            }

            return Create(m_fDuration, copyConfig, m_fTension);
        }

        public override void UpdatePosition(CCPoint newPos)
        {
            m_pTarget.Position = CCPointExtension.ccpAdd(newPos, m_startPosition);
        }
    }

    public class CCCatmullRomTo : CCCardinalSplineTo
    {
        public static CCCatmullRomTo Create(float dt, List<CCPoint> points)
        {
            var ret = new CCCatmullRomTo();
            ret.InitWithDuration(dt, points);
            return ret;
        }

        public bool InitWithDuration(float dt, List<CCPoint> points)
        {
            if (base.InitWithDuration(dt, points, 0.5f))
            {
                return true;
            }
            return false;
        }
    }

    public class CCCatmullRomBy : CCCardinalSplineBy
    {
        public static CCCatmullRomBy Create(float dt, List<CCPoint> points)
        {
            var ret = new CCCatmullRomBy();
            ret.InitWithDuration(dt, points);
            return ret;
        }

        public bool InitWithDuration(float dt, List<CCPoint> points)
        {
            if (base.InitWithDuration(dt, points, 0.5f))
            {
                return true;
            }

            return false;
        }
    }
}