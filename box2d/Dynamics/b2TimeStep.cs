using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Box2D.Common;

namespace Box2D.Dynamics
{
#if PROFILING
    /// Profiling data. Times are in milliseconds.
    public struct b2Profile
    {
        public float step;
        public float collide;
        public float solve;
        public float solveInit;
        public float solveVelocity;
        public float solvePosition;
        public float broadphase;
        public float solveTOI;
        public float solveTOIAdvance;
        public float computeTOI;
        public float timeInInit;
        public int jointCount;
        public int bodyCount;
        public int contactCount;
        public int toiSolverIterations;
    }
#endif

    /// This is an internal structure.
    public struct b2TimeStep
    {
        public float dt;            // time step
        public float inv_dt;        // inverse time step (0 if dt == 0).
        public float dtRatio;    // dt * inv_dt0
        public int velocityIterations;
        public int positionIterations;
        public bool warmStarting;
    }

    /// This is an internal structure.
    internal struct b2Position
    {
        public b2Vec2 c;
        public float a;
    }

    /// This is an internal structure.
    internal struct b2Velocity
    {
        public b2Vec2 v;
        public float w;
    }

    /// Solver Data
    public struct b2SolverData
    {
        public b2TimeStep step;
        //public b2Body[] Bodies;
        //public b2Position[] positions;
        //public b2Velocity[] velocities;
    }

}
