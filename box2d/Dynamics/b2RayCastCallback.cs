using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Box2D.Common;

namespace Box2D.Dynamics
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class b2RayCastCallback
    {
        /// <summary>
        /// Called for each fixture found in the query. You control how the ray cast
        /// proceeds by returning a float:
        /// return -1: ignore this fixture and continue
        /// return 0: terminate the ray cast
        /// return fraction: clip the ray to this point
        /// return 1: don't clip the ray and continue
        /// </summary>
        /// <param name="fixture">the fixture hit by the ray</param>
        /// <param name="point">the point of initial intersection</param>
        /// <param name="normal">the normal vector at the point of intersection</param>
        /// <param name="fraction"></param>
        /// <returns>-1 to filter, 0 to terminate, fraction to clip the ray for
        /// closest hit, 1 to continue</returns>
        public abstract float ReportFixture(b2Fixture fixture, b2Vec2 point,
                                    b2Vec2 normal, float fraction);
    }
}
