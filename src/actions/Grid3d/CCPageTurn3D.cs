using System;
using Microsoft.Xna.Framework;

namespace CocosSharp
{
    public class CCPageTurn3D : CCGrid3DAction
    {
        #region Constructors

        public CCPageTurn3D (float duration, CCGridSize gridSize) : base (duration, gridSize)
        {
        }

        #endregion Constructors


        protected internal override CCActionState StartAction(CCNode target)
        {
            return new CCPageTurn3DState (this, GridNode(target));
        }
    }


    #region Action state

    public class CCPageTurn3DState : CCGrid3DActionState
    {
        public CCPageTurn3DState (CCPageTurn3D action, CCNodeGrid target) : base (action, target)
        {
        }

        public override void Update (float time)
        {

            if (Target == null)
                return;
            
            float tt = Math.Max (0, time - 0.25f);
            float deltaAy = (tt * tt * 500);
            float ay = -100 - deltaAy;

            float deltaTheta = -MathHelper.PiOver2 * (float)Math.Sqrt (time);
            float theta = MathHelper.PiOver2 + deltaTheta;

            var sinTheta = (float)Math.Sin (theta);
            var cosTheta = (float)Math.Cos (theta);

            for (int i = 0; i <= GridSize.X; ++i)
            {
                for (int j = 0; j <= GridSize.Y; ++j)
                {
                    // Get original vertex
                    CCVertex3F p = OriginalVertex (i, j);

                    var R = (float)Math.Sqrt ((p.X * p.X) + ((p.Y - ay) * (p.Y - ay)));
                    float r = R * sinTheta;
                    var alpha = (float)Math.Asin (p.X / R);
                    float beta = alpha / sinTheta;
                    var cosBeta = (float)Math.Cos (beta);

                    // If beta > PI then we've wrapped around the cone
                    // Reduce the radius to stop these points interfering with others
                    if (beta <= MathHelper.Pi)
                    {
                        p.X = (r * (float)Math.Sin (beta));
                    }
                    else
                    {
                        // Force X = 0 to stop wrapped
                        // points
                        p.X = 0;
                    }

                    p.Y = (R + ay - (r * (1 - cosBeta) * sinTheta));

                    // We scale z here to avoid the animation being
                    // too much bigger than the screen due to perspective transform
                    p.Z = (r * (1 - cosBeta) * cosTheta) / 7; // "100" didn't work for

                    //    Stop z coord from dropping beneath underlying page in a transition
                    // issue #751
                    if (p.Z < 0.5f)
                    {
                        p.Z = 0.5f;
                    }

                    // Set new coords
                    SetVertex (i, j, ref p);
                }
            }
        }

    }

    #endregion Action state
}