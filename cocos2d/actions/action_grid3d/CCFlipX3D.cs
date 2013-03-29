using System;
using System.Diagnostics;

namespace cocos2d
{
    public class CCFlipX3D : CCGrid3DAction
    {
        /// <summary>
        /// initializes the action with duration
        /// </summary>
        public new bool InitWithDuration(float duration)
        {
            return InitWithSize(new CCGridSize(1, 1), duration);
        }

        public override bool InitWithSize(CCGridSize gridSize, float duration)
        {
            if (gridSize.X != 1 || gridSize.Y != 1)
            {
                // Grid size must be (1,1)
                Debug.Assert(false);
                return false;
            }

            return base.InitWithSize(gridSize, duration);
        }

        public override object Copy(ICopyable pZone)
        {
            CCFlipX3D pCopy;
            if (pZone != null)
            {
                //in case of being called at sub class
                pCopy = (CCFlipX3D) (pZone);
            }
            else
            {
                pCopy = new CCFlipX3D();
                pZone = pCopy;
            }

            base.Copy(pZone);

            pCopy.InitWithSize(m_sGridSize, m_fDuration);

            return pCopy;
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

        public new static CCFlipX3D Create(float duration)
        {
            var pAction = new CCFlipX3D();
            pAction.InitWithSize(new CCGridSize(1, 1), duration);
            return pAction;
        }
    }
}