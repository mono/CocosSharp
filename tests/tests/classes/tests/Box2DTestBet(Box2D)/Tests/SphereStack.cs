using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Box2D.Collision.Shapes;
using Box2D.Common;
using Box2D.Dynamics;

namespace Box2D.TestBed.Tests
{
    public class SphereStack : Test
    {
        public const int e_count = 10;

        public SphereStack()
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

                for (int i = 0; i < e_count; ++i)
                {
                    b2BodyDef bd  = b2BodyDef.Create();
                    bd.type = b2BodyType.b2_dynamicBody;
                    bd.position.Set(0.0f, 4.0f + 3.0f * i);

                    m_bodies[i] = m_world.CreateBody(bd);

                    m_bodies[i].CreateFixture(shape, 1.0f);

                    m_bodies[i].LinearVelocity = new b2Vec2(0.0f, -50.0f);
                }
            }
        }

        public override void Step(Settings settings)
        {
            base.Step(settings);

            //for (int32 i = 0; i < e_count; ++i)
            //{
            //    printf("%g ", m_bodies[i]->GetWorldCenter().y);
            //}

            //for (int32 i = 0; i < e_count; ++i)
            //{
            //    printf("%g ", m_bodies[i]->GetLinearVelocity().y);
            //}

            //printf("\n");
        }

        public b2Body[] m_bodies = new b2Body[e_count];
    }
}
