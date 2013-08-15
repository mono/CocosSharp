using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Box2D.Collision.Shapes;
using Box2D.Common;
using Box2D.Dynamics;

namespace Box2D.TestBed.Tests
{
    public class EdgeTest : Test
    {
        public EdgeTest()
        {
            {
                b2BodyDef bd  = b2BodyDef.Create();
                b2Body ground = m_world.CreateBody(bd);

                b2Vec2 v1 = new b2Vec2(-10.0f, 0.0f);
                b2Vec2 v2 = new b2Vec2(-7.0f, -2.0f);
                b2Vec2 v3 = new b2Vec2(-4.0f, 0.0f);
                b2Vec2 v4 = new b2Vec2(0.0f, 0.0f);
                b2Vec2 v5 = new b2Vec2(4.0f, 0.0f);
                b2Vec2 v6 = new b2Vec2(7.0f, 2.0f);
                b2Vec2 v7 = new b2Vec2(10.0f, 0.0f);

                b2EdgeShape shape = new b2EdgeShape();

                shape.Set(v1, v2);
                shape.HasVertex3 = true;
                shape.Vertex3 = v3;
                ground.CreateFixture(shape, 0.0f);

                shape.Set(v2, v3);
                shape.HasVertex0 = true;
                shape.HasVertex3 = true;
                shape.Vertex0 = v1;
                shape.Vertex3 = v4;
                ground.CreateFixture(shape, 0.0f);

                shape.Set(v3, v4);
                shape.HasVertex0 = true;
                shape.HasVertex3 = true;
                shape.Vertex0 = v2;
                shape.Vertex3 = v5;
                ground.CreateFixture(shape, 0.0f);

                shape.Set(v4, v5);
                shape.HasVertex0 = true;
                shape.HasVertex3 = true;
                shape.Vertex0 = v3;
                shape.Vertex3 = v6;
                ground.CreateFixture(shape, 0.0f);

                shape.Set(v5, v6);
                shape.HasVertex0 = true;
                shape.HasVertex3 = true;
                shape.Vertex0 = v4;
                shape.Vertex3 = v7;
                ground.CreateFixture(shape, 0.0f);

                shape.Set(v6, v7);
                shape.HasVertex0 = true;
                shape.Vertex0 = v5;
                ground.CreateFixture(shape, 0.0f);
            }

            {
                b2BodyDef bd  = b2BodyDef.Create();
                bd.type = b2BodyType.b2_dynamicBody;
                bd.position.Set(-0.5f, 0.6f);
                bd.allowSleep = false;
                b2Body body = m_world.CreateBody(bd);

                b2CircleShape shape = new b2CircleShape();
                shape.Radius = 0.5f;

                body.CreateFixture(shape, 1.0f);
            }

            {
                b2BodyDef bd  = b2BodyDef.Create();
                bd.type = b2BodyType.b2_dynamicBody;
                bd.position.Set(1.0f, 0.6f);
                bd.allowSleep = false;
                b2Body body = m_world.CreateBody(bd);

                b2PolygonShape shape = new b2PolygonShape();
                shape.SetAsBox(0.5f, 0.5f);

                body.CreateFixture(shape, 1.0f);
            }
        }
    }
}
