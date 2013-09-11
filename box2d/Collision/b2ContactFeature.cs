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

        public void Set(b2ContactFeature cf)
        {
            indexA = cf.indexA;
            indexB = cf.indexB;
            typeA = cf.typeA;
            typeB = cf.typeB;
        }

        public override bool Equals(object o)
        {
            b2ContactFeature bcf = (b2ContactFeature)o;
            return (indexA == bcf.indexA && indexB == bcf.indexB && typeA == bcf.typeA && typeB == bcf.typeB);
        }

        public bool Equals(ref b2ContactFeature bcf)
        {
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
            set
            {
                indexA = (byte) ((value >> 24) & 0xFF);
                indexB = (byte) ((value >> 16) & 0xFF);
                typeA = (((value >> 8) & 0xFF) == 0 ? b2ContactFeatureType.e_vertex : b2ContactFeatureType.e_face);
                typeB = ((value & 0xFF) == 0 ? b2ContactFeatureType.e_vertex : b2ContactFeatureType.e_face);
            }
        }

        public static b2ContactFeature Zero = new b2ContactFeature(0, 0, 0, 0);
    }
}
