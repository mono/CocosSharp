using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Box2D.Dynamics
{
    /// <summary>
    /// Implement this class to provide collision filtering. In other words, you can implement
    /// this class if you want finer control over contact creation.
    /// </summary>
    public abstract class b2ContactFilter
    {
        /// Return true if contact calculations should be performed between these two shapes.
        /// @warning for performance reasons this is only called when the AABBs begin to overlap.
        public abstract bool ShouldCollide(b2Fixture fixtureA, b2Fixture fixtureB);

        public static b2ContactFilter b2_defaultFilter = null;
    }
}
