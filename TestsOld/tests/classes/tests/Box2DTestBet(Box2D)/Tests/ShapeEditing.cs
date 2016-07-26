using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Box2D.Collision.Shapes;
using Box2D.Common;
using Box2D.Dynamics;

namespace Box2D.TestBed.Tests
{
    public class ShapeEditing : Test
    {

        public ShapeEditing()
        {
            {
                b2BodyDef bd1  = new b2BodyDef();
                b2Body ground = m_world.CreateBody(bd1);

                b2EdgeShape shape1 = new b2EdgeShape();
                shape1.Set(new b2Vec2(-40.0f, 0.0f), new b2Vec2(40.0f, 0.0f));
                ground.CreateFixture(shape1, 0.0f);
            }

            b2BodyDef bd  = new b2BodyDef();
            bd.type = b2BodyType.b2_dynamicBody;
            bd.position.Set(0.0f, 10.0f);
            m_body = m_world.CreateBody(bd);

            b2PolygonShape shape = new b2PolygonShape();
            shape.SetAsBox(4.0f, 4.0f, new b2Vec2(0.0f, 0.0f), 0.0f);
            m_fixture1 = m_body.CreateFixture(shape, 10.0f);

            m_fixture2 = null;

            m_sensor = false;
        }

        public override void Keyboard(char key)
        {
            switch (key)
            {
                case 'c':
                    if (m_fixture2 == null)
                    {
                        b2CircleShape shape = new b2CircleShape();
                        shape.Radius = 3.0f;
                        shape.Position = new b2Vec2(0.5f, -4.0f);
                        m_fixture2 = m_body.CreateFixture(shape, 10.0f);
                        m_body.SetAwake(true);
                    }
                    break;

                case 'd':
                    if (m_fixture2 != null)
                    {
                        m_body.DestroyFixture(m_fixture2);
                        m_fixture2 = null;
                        m_body.SetAwake(true);
                    }
                    break;

                case 's':
                    if (m_fixture2 != null)
                    {
                        m_sensor = !m_sensor;
                        m_fixture2.IsSensor = m_sensor;
                    }
                    break;
            }
        }

        protected override void Draw(Settings settings)
        {
            base.Draw(settings);
            m_debugDraw.DrawString(5, m_textLine, "Press: (c) create a shape, (d) destroy a shape.");
            m_textLine += 15;
            m_debugDraw.DrawString(5, m_textLine, "sensor = {0}", m_sensor);
            m_textLine += 15;
        }

        public b2Body m_body;
        public b2Fixture m_fixture1 = new b2Fixture();
        public b2Fixture m_fixture2 = new b2Fixture();
        public bool m_sensor;
    }
}
