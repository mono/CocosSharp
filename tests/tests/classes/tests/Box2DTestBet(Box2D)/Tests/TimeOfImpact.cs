using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Box2D.Collision;
using Box2D.Collision.Shapes;
using Box2D.Common;

namespace Box2D.TestBed.Tests
{
    public class TimeOfImpact : Test
    {
        public TimeOfImpact()
        {
            m_shapeA.SetAsBox(25.0f, 5.0f);
            m_shapeB.SetAsBox(2.5f, 2.5f);
        }

        protected override void Draw(Settings settings)
        {
            base.Draw(settings);

            b2Sweep sweepA = new b2Sweep();
            sweepA.alpha0 = 0;
            sweepA.c0.Set(24.0f, -60.0f);
            sweepA.a0 = 2.95f;
            sweepA.c = sweepA.c0;
            sweepA.a = sweepA.a0;
            sweepA.localCenter.SetZero();

            b2Sweep sweepB = new b2Sweep();
            sweepB.alpha0 = 0;
            sweepB.c0.Set(53.474274f, -50.252514f);
            sweepB.a0 = 513.36676f; // - 162.0f * b2_pi;
            sweepB.c.Set(54.595478f, -51.083473f);
            sweepB.a = 513.62781f; //  - 162.0f * b2_pi;
            sweepB.localCenter.SetZero();

            //sweepB.a0 -= 300.0f * b2_pi;
            //sweepB.a -= 300.0f * b2_pi;

            b2TOIInput input = b2TOIInput.Create();
            input.proxyA.Set(m_shapeA, 0);
            input.proxyB.Set(m_shapeB, 0);
            input.sweepA = sweepA;
            input.sweepB = sweepB;
            input.tMax = 1.0f;

            b2TOIOutput output;
            b2TimeOfImpact.Compute(out output, ref input);

            m_debugDraw.DrawString(5, m_textLine, "toi = {0}", output.t);
            m_textLine += 15;

            m_debugDraw.DrawString(5, m_textLine, "max toi iters = {0}, max root iters = {1}",
                                   b2TimeOfImpact.b2_toiMaxIters, b2TimeOfImpact.b2_toiMaxRootIters);
            m_textLine += 15;

            b2Vec2[] vertices = new b2Vec2[b2Settings.b2_maxPolygonVertices];

            b2Transform transformA;
            sweepA.GetTransform(out transformA, 0.0f);
            for (int i = 0; i < m_shapeA.VertexCount; ++i)
            {
                vertices[i] = b2Math.b2Mul(transformA, m_shapeA.Vertices[i]);
            }
            m_debugDraw.DrawPolygon(vertices, m_shapeA.VertexCount, new b2Color(0.9f, 0.9f, 0.9f));

            b2Transform transformB;
            sweepB.GetTransform(out transformB, 0.0f);

            b2Vec2 localPoint = new b2Vec2(2.0f, -0.1f);

            for (int i = 0; i < m_shapeB.VertexCount; ++i)
            {
                vertices[i] = b2Math.b2Mul(transformB, m_shapeB.Vertices[i]);
            }
            m_debugDraw.DrawPolygon(vertices, m_shapeB.VertexCount, new b2Color(0.5f, 0.9f, 0.5f));

            sweepB.GetTransform(out transformB, output.t);
            for (int i = 0; i < m_shapeB.VertexCount; ++i)
            {
                vertices[i] = b2Math.b2Mul(transformB, m_shapeB.Vertices[i]);
            }
            m_debugDraw.DrawPolygon(vertices, m_shapeB.VertexCount, new b2Color(0.5f, 0.7f, 0.9f));

            sweepB.GetTransform(out transformB, 1.0f);
            for (int i = 0; i < m_shapeB.VertexCount; ++i)
            {
                vertices[i] = b2Math.b2Mul(transformB, m_shapeB.Vertices[i]);
            }
            m_debugDraw.DrawPolygon(vertices, m_shapeB.VertexCount, new b2Color(0.9f, 0.5f, 0.5f));

#if false
        for (float t = 0.0f; t < 1.0f; t += 0.1f)
        {
            sweepB.GetTransform(ref transformB, t);
            for (int i = 0; i < m_shapeB.VertexCount; ++i)
            {
                vertices[i] = b2Math.b2Mul(transformB, m_shapeB.Vertices[i]);
            }
            m_debugDraw.DrawPolygon(vertices, m_shapeB.VertexCount, new b2Color(0.9f, 0.5f, 0.5f));
        }
#endif
        }

        public b2PolygonShape m_shapeA = new b2PolygonShape();
        public b2PolygonShape m_shapeB = new b2PolygonShape();
    }
}
