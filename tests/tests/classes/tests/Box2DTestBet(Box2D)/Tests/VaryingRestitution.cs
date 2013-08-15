using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Box2D.Collision.Shapes;
using Box2D.Common;
using Box2D.Dynamics;

namespace Box2D.TestBed.Tests
{
    public class VaryingRestitution : Test
    {
        public VaryingRestitution()
        {
            {
                b2BodyDef bd  = b2BodyDef.Create();
                b2Body ground = m_world.CreateBody(bd);

                b2EdgeShape shape = new b2EdgeShape();
                shape.Set(new b2Vec2(-40.0f, 0.0f), new b2Vec2(40.0f, 0.0f));
                ground.CreateFixture(shape, 0.0f);
            }

            {
                b2CircleShape shape = new b2CircleShape();
                shape.Radius = 1.0f;

                b2FixtureDef fd = new b2FixtureDef();
                fd.shape = shape;
                fd.density = 1.0f;

                float[] restitution = {0.0f, 0.1f, 0.3f, 0.5f, 0.75f, 0.9f, 1.0f};

                for (int i = 0; i < 7; ++i)
                {
                    b2BodyDef bd  = b2BodyDef.Create();
                    bd.type = b2BodyType.b2_dynamicBody;
                    bd.position.Set(-10.0f + 3.0f * i, 20.0f);

                    b2Body body = m_world.CreateBody(bd);

                    fd.restitution = restitution[i];
                    body.CreateFixture(fd);
                }
            }
        }
    }
}
