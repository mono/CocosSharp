using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Box2D.Collision.Shapes;
using Box2D.Common;
using Box2D.Dynamics;

namespace Box2D.TestBed.Tests
{
    public class VaryingFriction : Test
    {
        public VaryingFriction()
        {
            {
                b2BodyDef bd  = b2BodyDef.Create();
                b2Body ground = m_world.CreateBody(bd);

                b2EdgeShape shape = new b2EdgeShape();
                shape.Set(new b2Vec2(-40.0f, 0.0f), new b2Vec2(40.0f, 0.0f));
                ground.CreateFixture(shape, 0.0f);
            }

            {
                b2PolygonShape shape = new b2PolygonShape();
                shape.SetAsBox(13.0f, 0.25f);

                b2BodyDef bd  = b2BodyDef.Create();
                bd.position.Set(-4.0f, 22.0f);
                bd.angle = -0.25f;

                b2Body ground = m_world.CreateBody(bd);
                ground.CreateFixture(shape, 0.0f);
            }

            {
                b2PolygonShape shape = new b2PolygonShape();
                shape.SetAsBox(0.25f, 1.0f);

                b2BodyDef bd  = b2BodyDef.Create();
                bd.position.Set(10.5f, 19.0f);

                b2Body ground = m_world.CreateBody(bd);
                ground.CreateFixture(shape, 0.0f);
            }

            {
                b2PolygonShape shape = new b2PolygonShape();
                shape.SetAsBox(13.0f, 0.25f);

                b2BodyDef bd  = b2BodyDef.Create();
                bd.position.Set(4.0f, 14.0f);
                bd.angle = 0.25f;

                b2Body ground = m_world.CreateBody(bd);
                ground.CreateFixture(shape, 0.0f);
            }

            {
                b2PolygonShape shape = new b2PolygonShape();
                shape.SetAsBox(0.25f, 1.0f);

                b2BodyDef bd  = b2BodyDef.Create();
                bd.position.Set(-10.5f, 11.0f);

                b2Body ground = m_world.CreateBody(bd);
                ground.CreateFixture(shape, 0.0f);
            }

            {
                b2PolygonShape shape = new b2PolygonShape();
                shape.SetAsBox(13.0f, 0.25f);

                b2BodyDef bd  = b2BodyDef.Create();
                bd.position.Set(-4.0f, 6.0f);
                bd.angle = -0.25f;

                b2Body ground = m_world.CreateBody(bd);
                ground.CreateFixture(shape, 0.0f);
            }

            {
                b2PolygonShape shape = new b2PolygonShape();
                shape.SetAsBox(0.5f, 0.5f);

                b2FixtureDef fd = new b2FixtureDef();
                fd.shape = shape;
                fd.density = 25.0f;

                float[] friction = {0.75f, 0.5f, 0.35f, 0.1f, 0.0f};

                for (int i = 0; i < 5; ++i)
                {
                    b2BodyDef bd  = b2BodyDef.Create();
                    bd.type = b2BodyType.b2_dynamicBody;
                    bd.position.Set(-15.0f + 4.0f * i, 28.0f);
                    b2Body body = m_world.CreateBody(bd);

                    fd.friction = friction[i];
                    body.CreateFixture(fd);
                }
            }
        }
    }
}
