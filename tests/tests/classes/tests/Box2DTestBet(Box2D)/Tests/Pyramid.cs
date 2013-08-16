using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Box2D.Collision.Shapes;
using Box2D.Common;
using Box2D.Dynamics;

namespace Box2D.TestBed.Tests
{
    public class Pyramid : Test
    {
        public const int e_count = 20;

        public Pyramid()
        {
            {
                b2BodyDef bd  = new b2BodyDef();
                b2Body ground = m_world.CreateBody(bd);

                b2EdgeShape shape = new b2EdgeShape();
                shape.Set(new b2Vec2(-40.0f, 0.0f), new b2Vec2(40.0f, 0.0f));
                ground.CreateFixture(shape, 0.0f);
            }

            {
                float a = 0.5f;
                b2PolygonShape shape = new b2PolygonShape();
                shape.SetAsBox(a, a);

                b2Vec2 x = new b2Vec2(-7.0f, 0.75f);
                b2Vec2 y;
                b2Vec2 deltaX = new b2Vec2(0.5625f, 1.25f);
                b2Vec2 deltaY = new b2Vec2(1.125f, 0.0f);

                for (int i = 0; i < e_count; ++i)
                {
                    y = x;

                    for (int j = i; j < e_count; ++j)
                    {
                        b2BodyDef bd  = new b2BodyDef();
                        bd.type = b2BodyType.b2_dynamicBody;
                        bd.position = y;
                        b2Body body = m_world.CreateBody(bd);
                        body.CreateFixture(shape, 5.0f);

                        y += deltaY;
                    }

                    x += deltaX;
                }
            }
        }

        public override void Step(Settings settings)
        {
            base.Step(settings);

            //b2DynamicTree* tree = &m_world->m_contactManager.m_broadPhase.m_tree;

            //if (m_stepCount == 400)
            //{
            //    tree->RebuildBottomUp();
            //}
        }
    }
}
