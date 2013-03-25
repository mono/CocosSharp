using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Box2D.Dynamics
{
    /// <summary>
    /// Callback class for AABB queries.
    /// See b2World::Query
    /// </summary>
    public abstract class b2QueryCallback
    {
        /// <summary>
        /// Called for each fixture found in the query AABB.
        /// @return false to terminate the query.
        /// </summary>
        public abstract bool ReportFixture(b2Fixture fixture);
    }
}
