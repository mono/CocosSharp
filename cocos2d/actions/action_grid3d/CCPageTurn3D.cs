using System;
using Microsoft.Xna.Framework;

namespace cocos2d
{
    public class CCPageTurn3D : CCGrid3DAction
    {
        public override void Update(float time)
        {
            float tt = Math.Max(0, time - 0.25f);
            float deltaAy = (tt * tt * 500);
            float ay = -100 - deltaAy;

            float deltaTheta = -MathHelper.PiOver2 * (float) Math.Sqrt(time);
            float theta = /*0.01f */ +MathHelper.PiOver2 + deltaTheta;

            var sinTheta = (float) Math.Sin(theta);
            var cosTheta = (float) Math.Cos(theta);

            for (int i = 0; i <= m_sGridSize.x; ++i)
            {
                for (int j = 0; j <= m_sGridSize.y; ++j)
                {
                    // Get original vertex
                    var gs = new ccGridSize(i, j);
                    ccVertex3F p = OriginalVertex(gs);

                    var R = (float) Math.Sqrt((p.x * p.x) + ((p.y - ay) * (p.y - ay)));
                    float r = R * sinTheta;
                    var alpha = (float) Math.Asin(p.x / R);
                    float beta = alpha / sinTheta;
                    var cosBeta = (float) Math.Cos(beta);

                    // If beta > PI then we've wrapped around the cone
                    // Reduce the radius to stop these points interfering with others
                    if (beta <= MathHelper.Pi)
                    {
                        p.x = (r * (float) Math.Sin(beta));
                    }
                    else
                    {
                        // Force X = 0 to stop wrapped
                        // points
                        p.x = 0;
                    }

                    p.y = (R + ay - (r * (1 - cosBeta) * sinTheta));

                    // We scale z here to avoid the animation being
                    // too much bigger than the screen due to perspective transform
                    p.z = (r * (1 - cosBeta) * cosTheta) / 7; // "100" didn't work for

                    //    Stop z coord from dropping beneath underlying page in a transition
                    // issue #751
                    if (p.z < 0.5f)
                    {
                        p.z = 0.5f;
                    }

                    // Set new coords
                    SetVertex(gs, ref p);
                }
            }
        }

        public new static CCPageTurn3D Create(ccGridSize gridSize, float time)
        {
            var pAction = new CCPageTurn3D();
            pAction.InitWithSize(gridSize, time);
            return pAction;
        }
    }
}