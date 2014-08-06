using System;
using System.Diagnostics;

namespace CocosSharp
{
    public class CCFlipX3D : CCGrid3DAction
    {
        #region Constructors

        public CCFlipX3D (float duration, CCGridSize gridSize) : base (duration, gridSize)
        {
            if (gridSize.X != 1 || gridSize.Y != 1)
            {
                // Grid size must be (1,1)
                Debug.Assert (false);
            }
        }

        public CCFlipX3D (float duration) : this (duration, CCGridSize.One)
        { 
        }

        #endregion Constructors


        protected internal override CCActionState StartAction(CCNode target)
        {
            return new CCFlipX3DState (this, target);
        }
    }


    #region Action state

    public class CCFlipX3DState : CCGrid3DActionState
    {
        public CCFlipX3DState (CCFlipX3D action, CCNode target) : base (action, target)
        {
        }

        public override void Update (float time)
        {
            float angle = (float)Math.PI * time; // 180 degrees
            var mz = (float)Math.Sin (angle);
            angle = angle / 2.0f; // x calculates degrees from 0 to 90
            var mx = (float)Math.Cos (angle);

            CCVertex3F v0, v1, v;
            var diff = new CCVertex3F ();

            v0 = OriginalVertex (1, 1);
            v1 = OriginalVertex (0, 0);

            float x0 = v0.X;
            float x1 = v1.X;
            float x;
            CCGridSize a, b, c, d;

            if (x0 > x1)
            {
                // Normal Grid
                a = new CCGridSize (0, 0);
                b = new CCGridSize (0, 1);
                c = new CCGridSize (1, 0);
                d = new CCGridSize (1, 1);
                x = x0;
            }
            else
            {
                // Reversed Grid
                c = new CCGridSize (0, 0);
                d = new CCGridSize (0, 1);
                a = new CCGridSize (1, 0);
                b = new CCGridSize (1, 1);
                x = x1;
            }

            diff.X = (x - x * mx);
            diff.Z = Math.Abs ((float)Math.Floor ((x * mz) / 4.0f));

            // bottom-left
            v = OriginalVertex (a);
            v.X = diff.X;
            v.Z += diff.Z;
            SetVertex (a, ref v);

            // upper-left
            v = OriginalVertex (b);
            v.X = diff.X;
            v.Z += diff.Z;
            SetVertex (b, ref v);

            // bottom-right
            v = OriginalVertex (c);
            v.X -= diff.X;
            v.Z -= diff.Z;
            SetVertex (c, ref v);

            // upper-right
            v = OriginalVertex (d);
            v.X -= diff.X;
            v.Z -= diff.Z;
            SetVertex (d, ref v);
        }
    }

    #endregion Action state
}