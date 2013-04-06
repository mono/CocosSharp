using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Box2D.Collision
{
    public enum b2ContactFeatureType : byte
    {
        e_vertex = 0,
        e_face = 1
    }

    /// The features that intersect to form the contact point
    /// This must be 4 bytes or less.
    public struct b2ContactFeature
    {
        public byte indexA;        ///< Feature index on shapeA
        public byte indexB;        ///< Feature index on shapeB
        public b2ContactFeatureType typeA;        ///< The feature type on shapeA
        public b2ContactFeatureType typeB;        ///< The feature type on shapeB

        public b2ContactFeature(byte iA, byte iB, b2ContactFeatureType tA, b2ContactFeatureType tB)
        {
            indexA = iA;
            indexB = iB;
            typeA = tA;
            typeB = tB;
        }

        public override int GetHashCode()
        {
            return key;
        }

        public override bool Equals(object o)
        {
            b2ContactFeature bcf = (b2ContactFeature)o;
            return (indexA == bcf.indexA && indexB == bcf.indexB && typeA == bcf.typeA && typeB == bcf.typeB);
        }

        /// <summary>
        /// Hack to make the b2ContactID union from Box2D work.
        /// </summary>
        public int key
        {
            get
            {
                return (indexA << 24 | indexB << 16 | ((byte)typeA) << 8 | (byte)typeB);
            }
        }

        public static b2ContactFeature Zero = new b2ContactFeature(0, 0, 0, 0);
    }
}
