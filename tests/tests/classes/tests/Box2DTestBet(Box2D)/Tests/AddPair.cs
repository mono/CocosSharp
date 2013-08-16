using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Box2D;
using Box2D.Collision.Shapes;
using Box2D.Common;
using Box2D.Dynamics;

namespace Box2D.TestBed.Tests
{
    public class AddPair : Test
    {

        public AddPair()
        {
            m_world.Gravity = new b2Vec2(0.0f, 0.0f);
            {
                b2CircleShape shape = new b2CircleShape();
                shape.Position = b2Vec2.Zero;
                shape.Radius = 0.1f;

                float minX = -6.0f;
                float maxX = 0.0f;
                float minY = 4.0f;
                float maxY = 6.0f;

                for (int i = 0; i < 400; ++i)
                {
                    b2BodyDef bd  = new b2BodyDef();
                    bd.type = b2BodyType.b2_dynamicBody;
                    bd.position = new b2Vec2(Rand.RandomFloat(minX, maxX), Rand.RandomFloat(minY, maxY));
                    b2Body body = m_world.CreateBody(bd);
                    body.CreateFixture(shape, 0.01f);
                }
            }

            {
                b2PolygonShape shape = new b2PolygonShape();
                shape.SetAsBox(1.5f, 1.5f);
                b2BodyDef bd  = new b2BodyDef();
                bd.type = b2BodyType.b2_dynamicBody;
                bd.position = new b2Vec2(-40.0f, 5.0f);
                bd.bullet = true;
                b2Body body = m_world.CreateBody(bd);
                body.CreateFixture(shape, 1.0f);
                body.LinearVelocity = new b2Vec2(150.0f, 0.0f);
            }
        }
    }
}
