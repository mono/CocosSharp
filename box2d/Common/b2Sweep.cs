using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Box2D.Common
{
    /// This describes the motion of a body/shape for TOI computation.
    /// Shapes are defined with respect to the body origin, which may
    /// no coincide with the center of mass. However, to support dynamics
    /// we must interpolate the center of mass position.
    public struct b2Sweep
    {
        /*public b2Sweep()
        {
            localCenter = new b2Vec2();
            c0 = new b2Vec2();
            c = new b2Vec2();
            a0 = 0f;
            a = 0f;
            alpha0 = 0f;
        }*/
        /// Get the interpolated transform at a specific time.
        /// @param beta is a factor in [0,1], where 0 indicates alpha0.
        public b2Transform GetTransform(b2Transform xfb, float beta)
        {
            xfb.p = (1.0f - beta) * c0 + beta * c;
            float angle = (1.0f - beta) * a0 + beta * a;
            xfb.q.Set(angle);

            // Shift to origin
            xfb.p -= b2Math.b2Mul(xfb.q, localCenter);
            return (xfb);
        }

        /// Advance the sweep forward, yielding a new initial state.
        /// @param alpha the new initial time.
        public void Advance(float alpha)
        {
            Debug.Assert(alpha0 < 1.0f);
            float beta = (alpha - alpha0) / (1.0f - alpha0);
            c0 = (1.0f - beta) * c0 + beta * c;
            a0 = (1.0f - beta) * a0 + beta * a;
            alpha0 = alpha;
        }

        /// Normalize the angles.
        public void Normalize()
        {
            float twoPi = 2.0f * b2Settings.b2_pi;
            float d = twoPi * (float)Math.Floor(a0 / twoPi);
            a0 -= d;
            a -= d;
        }

        public b2Vec2 localCenter;    ///< local center of mass position
        public b2Vec2 c0, c;        ///< center world positions
        public float a0, a;        ///< world angles

        /// Fraction of the current time step in the range [0,1]
        /// c0 and a0 are the positions at alpha0.
        public float alpha0;
    }
}
