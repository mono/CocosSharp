using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Box2D.Common
{
    [Flags]
    public enum b2DrawFlags
    {
        e_shapeBit = 0x1,
        e_jointBit = 0x2,
        e_aabbBit = 0x4,
        e_pairBit = 0x8,
        e_centerOfMassBit = 0x10
    }

    public abstract class b2Draw
    {
        private b2DrawFlags m_drawFlags = 0x0;

        public b2Draw()
        {
        }

        /// Set the drawing flags.
        public void SetFlags(b2DrawFlags flags)
        {
            m_drawFlags = flags;
        }

        /// Get the drawing flags.
        public b2DrawFlags GetFlags()
        {
            return (m_drawFlags);
        }

        /// Append flags to the current flags.
        public void AppendFlags(b2DrawFlags flags)
        {
            m_drawFlags |= flags;
        }

        /// Clear flags from the current flags.
        public void ClearFlags(b2DrawFlags flags)
        {
            m_drawFlags &= ~flags;
        }

        /// Draw a closed polygon provided in CCW order.
        public abstract void DrawPolygon(b2Vec2[] vertices, int vertexCount, b2Color color);

        /// Draw a solid closed polygon provided in CCW order.
        public abstract void DrawSolidPolygon(b2Vec2[] vertices, int vertexCount, b2Color color);

        /// Draw a circle.
        public abstract void DrawCircle(b2Vec2 center, float radius, b2Color color);

        /// Draw a solid circle.
        public abstract void DrawSolidCircle(b2Vec2 center, float radius, b2Vec2 axis, b2Color color);

        /// Draw a line segment.
        public abstract void DrawSegment(b2Vec2 p1, b2Vec2 p2, b2Color color);

        /// Draw a transform. Choose your own length scale.
        /// @param xf a transform.
        public abstract void DrawTransform(b2Transform xf);
    }
}
