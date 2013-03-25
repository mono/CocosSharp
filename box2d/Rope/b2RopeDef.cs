using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Box2D.Common;

namespace Box2D.Rope
{
    public struct b2RopeDef
    {
        public b2Vec2[] vertices;
        public int count;
        public float[] masses;
        public b2Vec2 gravity;
        public float damping;
        public float k2;
        /// Bending stiffness. Values above 0.5 can make the simulation blow up.
        public float k3;
    }
}
