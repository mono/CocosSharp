using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Box2D.Collision.Shapes;
using Box2D.Common;
using Box2D.Dynamics;

namespace Box2D.TestBed.Tests
{
    public class VerticalStack : Test
    {
        public const int e_columnCount = 5;
        public const int e_rowCount = 16;
        //public const int e_columnCount = 1;
        //public const int e_rowCount = 1;

        public VerticalStack()
        {
            {
                b2BodyDef bd  = b2BodyDef.Create();
                b2Body ground = m_world.CreateBody(bd);

                b2EdgeShape shape = new b2EdgeShape();
                shape.Set(new b2Vec2(-40.0f, 0.0f), new b2Vec2(40.0f, 0.0f));
                ground.CreateFixture(shape, 0.0f);

                shape.Set(new b2Vec2(20.0f, 0.0f), new b2Vec2(20.0f, 20.0f));
                ground.CreateFixture(shape, 0.0f);
            }

            float[] xs = {0.0f, -10.0f, -5.0f, 5.0f, 10.0f};

            for (int j = 0; j < e_columnCount; ++j)
            {
                b2PolygonShape shape = new b2PolygonShape();
                shape.SetAsBox(0.5f, 0.5f);

                b2FixtureDef fd = new b2FixtureDef();
                fd.shape = shape;
                fd.density = 1.0f;
                fd.friction = 0.3f;

                for (int i = 0; i < e_rowCount; ++i)
                {
                    b2BodyDef bd  = b2BodyDef.Create();
                    bd.type = b2BodyType.b2_dynamicBody;

                    int n = j * e_rowCount + i;
                    Debug.Assert(n < e_rowCount * e_columnCount);
                    m_indices[n] = n;
                    bd.userData = m_indices[n];

                    float x = 0.0f;
                    //float32 x = RandomFloat(-0.02f, 0.02f);
                    //float32 x = i % 2 == 0 ? -0.025f : 0.025f;
                    bd.position.Set(xs[j] + x, 0.752f + 1.54f * i);
                    b2Body body = m_world.CreateBody(bd);

                    m_bodies[n] = body;

                    body.CreateFixture(fd);
                }
            }

            m_bullet = null;
        }

        public override void Keyboard(char key)
        {
            switch (key)
            {
                case ',':
                    if (m_bullet != null)
                    {
                        m_world.DestroyBody(m_bullet);
                        m_bullet = null;
                    }
                    {
                        b2CircleShape shape = new b2CircleShape();
                        shape.Radius = 0.25f;

                        b2FixtureDef fd = new b2FixtureDef();
                        fd.shape = shape;
                        fd.density = 20.0f;
                        fd.restitution = 0.05f;

                        b2BodyDef bd  = b2BodyDef.Create();
                        bd.type = b2BodyType.b2_dynamicBody;
                        bd.bullet = true;
                        bd.position.Set(-31.0f, 5.0f);

                        m_bullet = m_world.CreateBody(bd);
                        m_bullet.CreateFixture(fd);

                        m_bullet.LinearVelocity = new b2Vec2(400.0f, 0.0f);
                    }
                    break;
            }
        }

        public override void Step(Settings settings)
        {
            base.Step(settings);
            m_debugDraw.DrawString(5, m_textLine, "Press: (,) to launch a bullet.");
            m_textLine += 15;

            //if (m_stepCount == 300)
            //{
            //    if (m_bullet != NULL)
            //    {
            //        m_world->DestroyBody(m_bullet);
            //        m_bullet = NULL;
            //    }

            //    {
            //        b2CircleShape shape;
            //        shape.m_radius = 0.25f;

            //        b2FixtureDef fd;
            //        fd.shape = &shape;
            //        fd.density = 20.0f;
            //        fd.restitution = 0.05f;

            //        b2BodyDef bd;
            //        bd.type = b2_dynamicBody;
            //        bd.bullet = true;
            //        bd.position.Set(-31.0f, 5.0f);

            //        m_bullet = m_world->CreateBody(&bd);
            //        m_bullet->CreateFixture(&fd);

            //        m_bullet->SetLinearVelocity(b2Vec2(400.0f, 0.0f));
            //    }
            //}
        }

        public b2Body m_bullet;
        public b2Body[] m_bodies = new b2Body[e_rowCount * e_columnCount];
        public int[] m_indices = new int[e_rowCount * e_columnCount];
    }
}
