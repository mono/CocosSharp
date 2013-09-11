
using Box2D.Collision.Shapes;
using Box2D.Common;
using Box2D.Dynamics;

namespace Box2D.TestBed.Tests
{
    public class CircleBenchmarkTest : Test
    {
        private const int XCount = 30;
        private const int YCount = 15;

        public CircleBenchmarkTest()
        {
            b2BodyDef bd = new b2BodyDef();
            b2Body ground = m_world.CreateBody(bd);

            // Floor
            b2EdgeShape shape = new b2EdgeShape();
            shape.Set(new b2Vec2(-40.0f, 0.0f), new b2Vec2(40.0f, 0.0f));
            ground.CreateFixture(shape, 0.0f);

            // Left wall
            shape = new b2EdgeShape();
            shape.Set(new b2Vec2(-40.0f, 0.0f), new b2Vec2(-40.0f, 45.0f));
            ground.CreateFixture(shape, 0.0f);

            // Right wall
            shape = new b2EdgeShape();
            shape.Set(new b2Vec2(40.0f, 0.0f), new b2Vec2(40.0f, 45.0f));
            ground.CreateFixture(shape, 0.0f);

            // Roof
            shape = new b2EdgeShape();
            shape.Set(new b2Vec2(-40.0f, 45.0f), new b2Vec2(40.0f, 45.0f));
            ground.CreateFixture(shape, 0.0f);

            var sphere = new b2CircleShape();
            sphere.Radius = 1.0f;

            for (int i = 0; i < XCount; i++)
            {
                for (int j = 0; j < YCount; ++j)
                {
                    bd = new b2BodyDef();
                    bd.type = b2BodyType.b2_dynamicBody;
                    bd.position.Set(-38f + 2.1f * i, 2.0f + 2.0f * j);

                    var body = m_world.CreateBody(bd);
                    body.CreateFixture(sphere, 1.0f);
                }
            }
        }
    }
}