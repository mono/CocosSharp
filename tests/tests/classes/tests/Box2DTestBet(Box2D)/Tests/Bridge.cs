using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Box2D.Collision.Shapes;
using Box2D.Common;
using Box2D.Dynamics;
using Box2D.Dynamics.Joints;

namespace Box2D.TestBed.Tests
{
    public class Bridge : Test
    {
        public int e_count = 30;

        public Bridge()
        {
            b2Body ground = null;
            {
                b2BodyDef bd  = b2BodyDef.Create();
                ground = m_world.CreateBody(bd);

                b2EdgeShape shape = new b2EdgeShape();
                shape.Set(new b2Vec2(-40.0f, 0.0f), new b2Vec2(40.0f, 0.0f));
                ground.CreateFixture(shape, 0.0f);
            }

            {
                b2PolygonShape shape = new b2PolygonShape();
                shape.SetAsBox(0.5f, 0.125f);

                b2FixtureDef fd = new b2FixtureDef();
                fd.shape = shape;
                fd.density = 20.0f;
                fd.friction = 0.2f;

                b2RevoluteJointDef jd = new b2RevoluteJointDef();

                b2Body prevBody = ground;
                for (int i = 0; i < e_count; ++i)
                {
                    b2BodyDef bd  = b2BodyDef.Create();
                    bd.type = b2BodyType.b2_dynamicBody;
                    bd.position = new b2Vec2(-14.5f + 1.0f * i, 5.0f);
                    b2Body body = m_world.CreateBody(bd);
                    body.CreateFixture(fd);

                    b2Vec2 anchor = new b2Vec2(-15.0f + 1.0f * i, 5.0f);
                    jd.Initialize(prevBody, body, anchor);
                    m_world.CreateJoint(jd);

                    if (i == (e_count >> 1))
                    {
                        m_middle = body;
                    }
                    prevBody = body;
                }

                b2Vec2 anchor1 = new b2Vec2(-15.0f + 1.0f * e_count, 5.0f);
                jd.Initialize(prevBody, ground, anchor1);
                m_world.CreateJoint(jd);
            }

            for (int i = 0; i < 2; ++i)
            {
                b2Vec2[] vertices = new b2Vec2[3];
                vertices[0].Set(-0.5f, 0.0f);
                vertices[1].Set(0.5f, 0.0f);
                vertices[2].Set(0.0f, 1.5f);

                b2PolygonShape shape = new b2PolygonShape();
                shape.Set(vertices, 3);

                b2FixtureDef fd = new b2FixtureDef();
                fd.shape = shape;
                fd.density = 1.0f;

                b2BodyDef bd  = b2BodyDef.Create();
                bd.type = b2BodyType.b2_dynamicBody;
                bd.position = new b2Vec2(-8.0f + 8.0f * i, 12.0f);
                b2Body body = m_world.CreateBody(bd);
                body.CreateFixture(fd);
            }

            for (int i = 0; i < 3; ++i)
            {
                b2CircleShape shape = new b2CircleShape();
                shape.Radius = 0.5f;

                b2FixtureDef fd = new b2FixtureDef();
                fd.shape = shape;
                fd.density = 1.0f;

                b2BodyDef bd  = b2BodyDef.Create();
                bd.type = b2BodyType.b2_dynamicBody;
                bd.position.Set(-6.0f + 6.0f * i, 10.0f);
                b2Body body = m_world.CreateBody(bd);
                body.CreateFixture(fd);
            }
        }

        public b2Body m_middle;
    }
}
