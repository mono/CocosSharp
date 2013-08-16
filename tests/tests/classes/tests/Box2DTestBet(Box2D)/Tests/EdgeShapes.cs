using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Box2D.Collision.Shapes;
using Box2D.Common;
using Box2D.Dynamics;

namespace Box2D.TestBed.Tests
{
    public class EdgeShapes : Test
    {
        public const int e_maxBodies = 256;

        public EdgeShapes()
        {
            // Ground body
            {
                b2BodyDef bd  = new b2BodyDef();
                b2Body ground = m_world.CreateBody(bd);

                float x1 = -20.0f;
                float y1 = 2.0f * (float) Math.Cos(x1 / 10.0f * b2Settings.b2_pi);
                for (int i = 0; i < 80; ++i)
                {
                    float x2 = x1 + 0.5f;
                    float y2 = 2.0f * (float) Math.Cos(x2 / 10.0f * b2Settings.b2_pi);

                    b2EdgeShape shape = new b2EdgeShape();
                    shape.Set(new b2Vec2(x1, y1), new b2Vec2(x2, y2));
                    ground.CreateFixture(shape, 0.0f);

                    x1 = x2;
                    y1 = y2;
                }
            }

            {
                b2Vec2[] vertices = new b2Vec2[3];
                vertices[0].Set(-0.5f, 0.0f);
                vertices[1].Set(0.5f, 0.0f);
                vertices[2].Set(0.0f, 1.5f);
                m_polygons[0] = new b2PolygonShape();
                m_polygons[0].Set(vertices, 3);
            }

            {
                b2Vec2[] vertices = new b2Vec2[3];
                vertices[0].Set(-0.1f, 0.0f);
                vertices[1].Set(0.1f, 0.0f);
                vertices[2].Set(0.0f, 1.5f);
                m_polygons[1] = new b2PolygonShape();
                m_polygons[1].Set(vertices, 3);
            }

            {
                float w = 1.0f;
                float b = w / (2.0f + b2Math.b2Sqrt(2.0f));
                float s = b2Math.b2Sqrt(2.0f) * b;

                b2Vec2[] vertices = new b2Vec2[8];
                vertices[0].Set(0.5f * s, 0.0f);
                vertices[1].Set(0.5f * w, b);
                vertices[2].Set(0.5f * w, b + s);
                vertices[3].Set(0.5f * s, w);
                vertices[4].Set(-0.5f * s, w);
                vertices[5].Set(-0.5f * w, b + s);
                vertices[6].Set(-0.5f * w, b);
                vertices[7].Set(-0.5f * s, 0.0f);

                m_polygons[2] = new b2PolygonShape();
                m_polygons[2].Set(vertices, 8);
            }

            {
                m_polygons[3] = new b2PolygonShape();
                m_polygons[3].SetAsBox(0.5f, 0.5f);
            }

            {
                m_circle.Radius = 0.5f;
            }

            m_bodyIndex = 0;

            m_angle = 0.0f;
        }

        private void Create(int index)
        {
            if (m_bodies[m_bodyIndex] != null)
            {
                m_world.DestroyBody(m_bodies[m_bodyIndex]);
                m_bodies[m_bodyIndex] = null;
            }

            b2BodyDef bd  = new b2BodyDef();

            float x = Rand.RandomFloat(-10.0f, 10.0f);
            float y = Rand.RandomFloat(10.0f, 20.0f);
            bd.position.Set(x, y);
            bd.angle = Rand.RandomFloat(-b2Settings.b2_pi, b2Settings.b2_pi);
            bd.type = b2BodyType.b2_dynamicBody;

            if (index == 4)
            {
                bd.angularDamping = 0.02f;
            }

            m_bodies[m_bodyIndex] = m_world.CreateBody(bd);

            if (index < 4)
            {
                b2FixtureDef fd = new b2FixtureDef();
                fd.shape = m_polygons[index];
                fd.friction = 0.3f;
                fd.density = 20.0f;
                m_bodies[m_bodyIndex].CreateFixture(fd);
            }
            else
            {
                b2FixtureDef fd = new b2FixtureDef();
                fd.shape = m_circle;
                fd.friction = 0.3f;
                fd.density = 20.0f;
                m_bodies[m_bodyIndex].CreateFixture(fd);
            }

            m_bodyIndex = (m_bodyIndex + 1) % e_maxBodies;
        }

        public void DestroyBody()
        {
            for (int i = 0; i < e_maxBodies; ++i)
            {
                if (m_bodies[i] != null)
                {
                    m_world.DestroyBody(m_bodies[i]);
                    m_bodies[i] = null;
                    return;
                }
            }
        }

        public override void Keyboard(char key)
        {
            switch (key)
            {
                case '1':
                case '2':
                case '3':
                case '4':
                case '5':
                    Create(key - '1');
                    break;

                case 'd':
                    DestroyBody();
                    break;
            }
        }

        protected override void Draw(Settings settings)
        {
            base.Draw(settings);

            m_debugDraw.DrawString(5, m_textLine, "Press 1-5 to drop stuff");
            m_textLine += 15;

            float L = 25.0f;
            b2Vec2 point1 = new b2Vec2(0.0f, 10.0f);
            b2Vec2 d = new b2Vec2(L * (float)Math.Cos(m_angle), -L * b2Math.b2Abs((float)Math.Sin(m_angle)));
            b2Vec2 point2 = point1 + d;

            EdgeShapesCallback callback = new EdgeShapesCallback();

            m_world.RayCast(callback, point1, point2);

            if (callback.m_fixture != null)
            {
                m_debugDraw.DrawPoint(callback.m_point, 5.0f, new b2Color(0.4f, 0.9f, 0.4f));

                m_debugDraw.DrawSegment(point1, callback.m_point, new b2Color(0.8f, 0.8f, 0.8f));

                b2Vec2 head = callback.m_point + 0.5f * callback.m_normal;
                m_debugDraw.DrawSegment(callback.m_point, head, new b2Color(0.9f, 0.9f, 0.4f));
            }
            else
            {
                m_debugDraw.DrawSegment(point1, point2, new b2Color(0.8f, 0.8f, 0.8f));
            }
        }

        public override void Step(Settings settings)
        {
            bool advanceRay = settings.pause || settings.singleStep;

            base.Step(settings);

            if (advanceRay)
            {
                m_angle += 0.25f * b2Settings.b2_pi / 180.0f;
            }
        }

        public int m_bodyIndex;
        public b2Body[] m_bodies = new b2Body[e_maxBodies];
        public b2PolygonShape[] m_polygons = new b2PolygonShape[4];
        public b2CircleShape m_circle = new b2CircleShape();

        public float m_angle;
    }

    public class EdgeShapesCallback : b2RayCastCallback
    {
        public EdgeShapesCallback()
        {
            m_fixture = null;
        }

        public override float ReportFixture(b2Fixture fixture, b2Vec2 point,
                                   b2Vec2 normal, float fraction)
        {
            m_fixture = fixture;
            m_point = point;
            m_normal = normal;

            return fraction;
        }

        public b2Fixture m_fixture = new b2Fixture();
        public b2Vec2 m_point = new b2Vec2();
        public b2Vec2 m_normal = new b2Vec2();
    }
}
