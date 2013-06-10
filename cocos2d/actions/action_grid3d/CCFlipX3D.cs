using System;
using System.Diagnostics;

namespace Cocos2D
{
    public class CCFlipX3D : CCGrid3DAction
    {
        public CCFlipX3D()
        {
        }

        public CCFlipX3D(float duration)
        {
            InitWithDuration(duration, new CCGridSize(1, 1));
        }

        public CCFlipX3D(CCFlipX3D flipX3D)
        {
            InitWithDuration(flipX3D.m_fDuration, flipX3D.m_sGridSize);
        }

        /// <summary>
        /// initializes the action with duration
        /// </summary>
        public new bool InitWithDuration(float duration)
        {
            return InitWithDuration(duration, new CCGridSize(1, 1));
        }

        protected override bool InitWithDuration(float duration, CCGridSize gridSize)
        {
            if (gridSize.X != 1 || gridSize.Y != 1)
            {
                // Grid size must be (1,1)
                Debug.Assert(false);
                return false;
            }

            return base.InitWithDuration(duration, gridSize);
        }

        public override object Copy(ICCCopyable pZone)
        {
            if (pZone != null)
            {
                //in case of being called at sub class
                var pCopy = (CCFlipX3D) (pZone);
                base.Copy(pZone);

                pCopy.InitWithDuration(m_fDuration, m_sGridSize);

                return pCopy;
            }
            else
            {
                return new CCFlipX3D(this);
            }
        }

        public override void Update(float time)
        {
            float angle = (float) Math.PI * time; // 180 degrees
            var mz = (float) Math.Sin(angle);
            angle = angle / 2.0f; // x calculates degrees from 0 to 90
            var mx = (float) Math.Cos(angle);

            CCVertex3F v0, v1, v;
            var diff = new CCVertex3F();

            v0 = OriginalVertex(new CCGridSize(1, 1));
            v1 = OriginalVertex(new CCGridSize(0, 0));

            float x0 = v0.X;
            float x1 = v1.X;
            float x;
            CCGridSize a, b, c, d;

            if (x0 > x1)
            {
                // Normal Grid
                a = new CCGridSize(0, 0);
                b = new CCGridSize(0, 1);
                c = new CCGridSize(1, 0);
                d = new CCGridSize(1, 1);
                x = x0;
            }
            else
            {
                // Reversed Grid
                c = new CCGridSize(0, 0);
                d = new CCGridSize(0, 1);
                a = new CCGridSize(1, 0);
                b = new CCGridSize(1, 1);
                x = x1;
            }

            diff.X = (x - x * mx);
            diff.Z = Math.Abs((float) Math.Floor((x * mz) / 4.0f));

            // bottom-left
            v = OriginalVertex(a);
            v.X = diff.X;
            v.Z += diff.Z;
            SetVertex(a, ref v);

            // upper-left
            v = OriginalVertex(b);
            v.X = diff.X;
            v.Z += diff.Z;
            SetVertex(b, ref v);

            // bottom-right
            v = OriginalVertex(c);
            v.X -= diff.X;
            v.Z -= diff.Z;
            SetVertex(c, ref v);

            // upper-right
            v = OriginalVertex(d);
            v.X -= diff.X;
            v.Z -= diff.Z;
            SetVertex(d, ref v);
        }
    }
}