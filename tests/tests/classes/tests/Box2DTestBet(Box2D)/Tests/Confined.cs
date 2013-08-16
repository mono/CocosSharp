using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Box2D.Collision.Shapes;
using Box2D.Common;
using Box2D.Dynamics;

namespace Box2D.TestBed.Tests
{
    public class Confined : Test
    {
        public const int e_columnCount = 0;
        public const int e_rowCount = 0;

        public Confined()
        {
            {
                b2BodyDef bd  = new b2BodyDef();
                b2Body ground = m_world.CreateBody(bd);

                b2EdgeShape shape = new b2EdgeShape();

                // Floor
                shape.Set(new b2Vec2(-10.0f, 0.0f), new b2Vec2(10.0f, 0.0f));
                ground.CreateFixture(shape, 0.0f);

                // Left wall
                shape.Set(new b2Vec2(-10.0f, 0.0f), new b2Vec2(-10.0f, 20.0f));
                ground.CreateFixture(shape, 0.0f);

                // Right wall
                shape.Set(new b2Vec2(10.0f, 0.0f), new b2Vec2(10.0f, 20.0f));
                ground.CreateFixture(shape, 0.0f);

                // Roof
                shape.Set(new b2Vec2(-10.0f, 20.0f), new b2Vec2(10.0f, 20.0f));
                ground.CreateFixture(shape, 0.0f);
            }

            float radius = 0.5f;
            b2CircleShape shape1 = new b2CircleShape();
            shape1.Position = b2Vec2.Zero;
            shape1.Radius = radius;

            b2FixtureDef fd = new b2FixtureDef();
            fd.shape = shape1;
            fd.density = 1.0f;
            fd.friction = 0.1f;

            for (int j = 0; j < e_columnCount; ++j)
            {
                for (int i = 0; i < e_rowCount; ++i)
                {
                    b2BodyDef bd  = new b2BodyDef();
                    bd.type = b2BodyType.b2_dynamicBody;
                    bd.position.Set(-10.0f + (2.1f * j + 1.0f + 0.01f * i) * radius, (2.0f * i + 1.0f) * radius);
                    b2Body body = m_world.CreateBody(bd);

                    body.CreateFixture(fd);
                }
            }

            m_world.Gravity = new b2Vec2(0.0f, 0.0f);
        }

        public void CreateCircle()
        {
            float radius = 2.0f;
            b2CircleShape shape = new b2CircleShape();
            shape.Position = b2Vec2.Zero;
            shape.Radius = radius;

            b2FixtureDef fd = new b2FixtureDef();
            fd.shape = shape;
            fd.density = 1.0f;
            fd.friction = 0.0f;

            b2Vec2 p = new b2Vec2(Rand.RandomFloat(), 3.0f + Rand.RandomFloat());
            b2BodyDef bd  = new b2BodyDef();
            bd.type = b2BodyType.b2_dynamicBody;
            bd.position = p;
            //bd.allowSleep = false;
            b2Body body = m_world.CreateBody(bd);

            body.CreateFixture(fd);
        }

        public override void Keyboard(char key)
        {
            switch (key)
            {
                case 'c':
                    CreateCircle();
                    break;
            }
        }

        protected override void Draw(Settings settings)
        {
            base.Draw(settings);

            m_debugDraw.DrawString(5, m_textLine, "Press 'c' to create a circle.");
            m_textLine += 15;
        }

        public override void Step(Settings settings)
        {
            //bool sleeping = true;
            for (b2Body b = m_world.BodyList; b != null; b = b.Next)
            {
                if (b.BodyType != b2BodyType.b2_dynamicBody)
                {
                    continue;
                }

                //if (b->IsAwake())
                //{
                //    sleeping = false;
                //}
            }

            if (m_stepCount == 180)
            {
                m_stepCount += 0;
            }

            //if (sleeping)
            //{
            //    CreateCircle();
            //}

            base.Step(settings);

            for (b2Body b = m_world.BodyList; b != null; b = b.Next)
            {
                if (b.BodyType != b2BodyType.b2_dynamicBody)
                {
                    continue;
                }

                b2Vec2 p = b.Position;
                if (p.x <= -10.0f || 10.0f <= p.x || p.y <= 0.0f || 20.0f <= p.y)
                {
                    p.x += 0.0f;
                }
            }
        }
    }
}
