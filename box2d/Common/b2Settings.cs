using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Box2D.Common
{
    public class b2Settings
    {
        public static float b2_maxFloat = float.MaxValue;
        public static float b2_epsilon = float.Epsilon;
        public static float b2_alphaEpsilon = 1.0f - 10.0f * b2Settings.b2_epsilon;
        public static float b2_pi = (float)Math.PI;
        public static float b2_epsilonSqrd = b2_epsilon * b2_epsilon;

        /// Global tuning constants based on meters-kilograms-seconds (MKS) units.
        ///

        // Collision

        /// The maximum number of contact points between two convex shapes. Do
        /// not change this value.
        public static int b2_maxManifoldPoints = 2;

        /// The maximum number of vertices on a convex polygon. You cannot increase
        /// this too much because b2BlockAllocator has a maximum object size.
        public static int b2_maxPolygonVertices = 8;

        /// This is used to fatten AABBs in the dynamic tree. This allows proxies
        /// to move by a small amount without triggering a tree adjustment.
        /// This is in meters.
        public static float b2_aabbExtension = 0.1f;

        /// This is used to fatten AABBs in the dynamic tree. This is used to predict
        /// the future position based on the current displacement.
        /// This is a dimensionless multiplier.
        public static float b2_aabbMultiplier = 2.0f;

        /// A small length used as a collision and constraint tolerance. Usually it is
        /// chosen to be numerically significant, but visually insignificant.
        public static float b2_linearSlop = 0.005f;

        /// A small angle used as a collision and constraint tolerance. Usually it is
        /// chosen to be numerically significant, but visually insignificant.
        public static float b2_angularSlop = (2.0f / 180.0f * b2_pi);

        /// The radius of the polygon/edge shape skin. This should not be modified. Making
        /// this smaller means polygons will have an insufficient buffer for continuous collision.
        /// Making it larger may create artifacts for vertex collision.
        public static float b2_polygonRadius = (2.0f * b2_linearSlop);

        /// Maximum number of sub-steps per contact in continuous physics simulation.
        public static int b2_maxSubSteps = 8;

        // Dynamics

        /// Maximum number of contacts to be handled to solve a TOI impact.
        public static readonly int b2_maxTOIContacts = 32;

        /// A velocity threshold for elastic collisions. Any collision with a relative linear
        /// velocity below this threshold will be treated as inelastic.
        public static readonly float b2_velocityThreshold = 1.0f;

        /// The maximum linear position correction used when solving constraints. This helps to
        /// prevent overshoot.
        public static readonly float b2_maxLinearCorrection = 0.2f;

        /// The maximum angular position correction used when solving constraints. This helps to
        /// prevent overshoot.
        public static readonly float b2_maxAngularCorrection = (8.0f / 180.0f * b2_pi);

        /// The maximum linear velocity of a body. This limit is very large and is used
        /// to prevent numerical problems. You shouldn't need to adjust this.
        public static readonly float b2_maxTranslation = 2.0f;
        public static readonly float b2_maxTranslationSquared = (b2_maxTranslation * b2_maxTranslation);

        /// The maximum angular velocity of a body. This limit is very large and is used
        /// to prevent numerical problems. You shouldn't need to adjust this.
        public static readonly float b2_maxRotation = (0.5f * b2_pi);
        public static readonly float b2_maxRotationSquared = (b2_maxRotation * b2_maxRotation);

        /// This scale factor controls how fast overlap is resolved. Ideally this would be 1 so
        /// that overlap is removed in one time step. However using values close to 1 often lead
        /// to overshoot.
        public static readonly float b2_baumgarte = 0.2f;
        public static readonly float b2_toiBaugarte = 0.75f;


        // Sleep

        /// The time that a body must be still before it will go to sleep.
        public static readonly float b2_timeToSleep = 0.5f;

        /// A body cannot sleep if its linear velocity is above this tolerance.
        public static readonly float b2_linearSleepTolerance = 0.01f;

        /// A body cannot sleep if its angular velocity is above this tolerance.
        public static readonly float b2_angularSleepTolerance = (2.0f / 180.0f * b2_pi);


        /// Logging function.
        public static void b2Log(string s, params object[] p)
        {
            System.Diagnostics.Debug.WriteLine(s, p);
        }

        /// Current version.
        public static b2Version b2_version = new b2Version(2, 2, 1);

        // Heavily used vector object
        public static b2Vec2 b2_aabbExtensionVec = new b2Vec2(b2Settings.b2_aabbExtension, b2Settings.b2_aabbExtension);
    }

    /// Version numbering scheme.
    public struct b2Version
    {
        public int major;        ///< significant changes
        public int minor;        ///< incremental changes
        public int revision;        ///< bug fixes
        public b2Version(int m, int i, int r)
        {
            major = m;
            minor = i;
            revision = r;
        }
    }
}
