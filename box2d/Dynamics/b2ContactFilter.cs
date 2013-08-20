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
    public class b2ContactFilter
    {
        /// Return true if contact calculations should be performed between these two shapes.
        /// @warning for performance reasons this is only called when the AABBs begin to overlap.
        public virtual bool ShouldCollide(b2Fixture fixtureA, b2Fixture fixtureB)
        {
            b2Filter filterA = fixtureA.Filter;
            b2Filter filterB = fixtureB.Filter;

            if (filterA.groupIndex == filterB.groupIndex && filterA.groupIndex != 0)
            {
                return filterA.groupIndex > 0;
            }

            bool collide = (filterA.maskBits & filterB.categoryBits) != 0 && (filterA.categoryBits & filterB.maskBits) != 0;
            return collide;
        }

        public static b2ContactFilter b2_defaultFilter = new b2ContactFilter();
    }
}
