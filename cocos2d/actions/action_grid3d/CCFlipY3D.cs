using System;

namespace cocos2d
{
    public class CCFlipY3D : CCFlipX3D
    {
        public override void Update(float time)
        {
            float angle = (float) Math.PI * time; // 180 degrees
            var mz = (float) Math.Sin(angle);
            angle = angle / 2.0f; // x calculates degrees from 0 to 90
            var my = (float) Math.Cos(angle);

            CCVertex3F v0, v1, v;
            var diff = new CCVertex3F();

            v0 = OriginalVertex(new CCGridSize(1, 1));
            v1 = OriginalVertex(new CCGridSize(0, 0));

            float y0 = v0.Y;
            float y1 = v1.Y;
            float y;
            CCGridSize a, b, c, d;

            if (y0 > y1)
            {
                // Normal Grid
                a = new CCGridSize(0, 0);
                b = new CCGridSize(0, 1);
                c = new CCGridSize(1, 0);
                d = new CCGridSize(1, 1);
                y = y0;
            }
            else
            {
                // Reversed Grid
                b = new CCGridSize(0, 0);
                a = new CCGridSize(0, 1);
                d = new CCGridSize(1, 0);
                c = new CCGridSize(1, 1);
                y = y1;
            }

            diff.Y = y - y * my;
            diff.Z = Math.Abs((float) Math.Floor((y * mz) / 4.0f));

            // bottom-left
            v = OriginalVertex(a);
            v.Y = diff.Y;
            v.Z += diff.Z;
            SetVertex(a, ref v);

            // upper-left
            v = OriginalVertex(b);
            v.Y -= diff.Y;
            v.Z -= diff.Z;
            SetVertex(b, ref v);

            // bottom-right
            v = OriginalVertex(c);
            v.Y = diff.Y;
            v.Z += diff.Z;
            SetVertex(c, ref v);

            // upper-right
            v = OriginalVertex(d);
            v.Y -= diff.Y;
            v.Z -= diff.Z;
            SetVertex(d, ref v);
        }

        public override object Copy(ICopyable pZone)
        {
            CCFlipY3D pCopy;
            if (pZone != null)
            {
                //in case of being called at sub class
                pCopy = (CCFlipY3D) (pZone);
            }
            else
            {
                pCopy = new CCFlipY3D();
                pZone = pCopy;
            }

            base.Copy(pZone);

            pCopy.InitWithSize(m_sGridSize, m_fDuration);

            return pCopy;
        }

        /** creates the action with duration */

        public new static CCFlipY3D Create(float duration)
        {
            var pAction = new CCFlipY3D();
            pAction.InitWithSize(new CCGridSize(1, 1), duration);
            return pAction;
        }
    }
}