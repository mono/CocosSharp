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
    public class CollisionFiltering : Test
    {
        public const int k_smallGroup = 1;
        public const int k_largeGroup = -1;

        public const ushort k_defaultCategory = 0x0001;
        public const ushort k_triangleCategory = 0x0002;
        public const ushort k_boxCategory = 0x0004;
        public const ushort k_circleCategory = 0x0008;

        public const ushort k_triangleMask = 0xFFFF;
        public const ushort k_boxMask = 0xFFFF ^ k_triangleCategory;
        public const ushort k_circleMask = 0xFFFF;

        public CollisionFiltering()
        {
            // Ground body
            {
                b2EdgeShape shape = new b2EdgeShape();
                shape.Set(new b2Vec2(-40.0f, 0.0f), new b2Vec2(40.0f, 0.0f));

                b2FixtureDef sd = new b2FixtureDef();
                sd.shape = shape;
                sd.friction = 0.3f;

                b2BodyDef bd  = b2BodyDef.Create();
                b2Body ground = m_world.CreateBody(bd);
                ground.CreateFixture(sd);
            }

            // Small triangle
            b2Vec2[] vertices = new b2Vec2[3];
            vertices[0].Set(-1.0f, 0.0f);
            vertices[1].Set(1.0f, 0.0f);
            vertices[2].Set(0.0f, 2.0f);
            b2PolygonShape polygon = new b2PolygonShape();
            polygon.Set(vertices, 3);

            b2FixtureDef triangleShapeDef = new b2FixtureDef();
            triangleShapeDef.shape = polygon;
            triangleShapeDef.density = 1.0f;

            triangleShapeDef.filter.groupIndex = k_smallGroup;
            triangleShapeDef.filter.categoryBits = k_triangleCategory;
            triangleShapeDef.filter.maskBits = k_triangleMask;

            b2BodyDef triangleBodyDef  = b2BodyDef.Create();
            triangleBodyDef.type = b2BodyType.b2_dynamicBody;
            triangleBodyDef.position.Set(-5.0f, 2.0f);

            b2Body body1 = m_world.CreateBody(triangleBodyDef);
            body1.CreateFixture(triangleShapeDef);

            // Large triangle (recycle definitions)
            vertices[0] *= 2.0f;
            vertices[1] *= 2.0f;
            vertices[2] *= 2.0f;
            polygon.Set(vertices, 3);
            triangleShapeDef.filter.groupIndex = k_largeGroup;
            triangleBodyDef.position.Set(-5.0f, 6.0f);
            triangleBodyDef.fixedRotation = true; // look at me!

            b2Body body2 = m_world.CreateBody(triangleBodyDef);
            body2.CreateFixture(triangleShapeDef);

            {
                b2BodyDef bd  = b2BodyDef.Create();
                bd.type = b2BodyType.b2_dynamicBody;
                bd.position.Set(-5.0f, 10.0f);
                b2Body body = m_world.CreateBody(bd);

                b2PolygonShape p = new b2PolygonShape();
                p.SetAsBox(0.5f, 1.0f);
                body.CreateFixture(p, 1.0f);

                b2PrismaticJointDef jd = new b2PrismaticJointDef();
                jd.BodyA = body2;
                jd.BodyB = body;
                jd.enableLimit = true;
                jd.localAnchorA.Set(0.0f, 4.0f);
                jd.localAnchorB.SetZero();
                jd.localAxisA.Set(0.0f, 1.0f);
                jd.lowerTranslation = -1.0f;
                jd.upperTranslation = 1.0f;

                m_world.CreateJoint(jd);
            }

            // Small box
            polygon.SetAsBox(1.0f, 0.5f);
            b2FixtureDef boxShapeDef = new b2FixtureDef();
            boxShapeDef.shape = polygon;
            boxShapeDef.density = 1.0f;
            boxShapeDef.restitution = 0.1f;

            boxShapeDef.filter.groupIndex = k_smallGroup;
            boxShapeDef.filter.categoryBits = k_boxCategory;
            boxShapeDef.filter.maskBits = k_boxMask;

            b2BodyDef boxBodyDef  = b2BodyDef.Create();
            boxBodyDef.type = b2BodyType.b2_dynamicBody;
            boxBodyDef.position.Set(0.0f, 2.0f);

            b2Body body3 = m_world.CreateBody(boxBodyDef);
            body3.CreateFixture(boxShapeDef);

            // Large box (recycle definitions)
            polygon.SetAsBox(2.0f, 1.0f);
            boxShapeDef.filter.groupIndex = k_largeGroup;
            boxBodyDef.position.Set(0.0f, 6.0f);

            b2Body body4 = m_world.CreateBody(boxBodyDef);
            body4.CreateFixture(boxShapeDef);

            // Small circle
            b2CircleShape circle = new b2CircleShape();
            circle.Radius = 1.0f;

            b2FixtureDef circleShapeDef = new b2FixtureDef();
            circleShapeDef.shape = circle;
            circleShapeDef.density = 1.0f;

            circleShapeDef.filter.groupIndex = k_smallGroup;
            circleShapeDef.filter.categoryBits = k_circleCategory;
            circleShapeDef.filter.maskBits = k_circleMask;

            b2BodyDef circleBodyDef  = b2BodyDef.Create();
            circleBodyDef.type = b2BodyType.b2_dynamicBody;
            circleBodyDef.position.Set(5.0f, 2.0f);

            b2Body body5 = m_world.CreateBody(circleBodyDef);
            body5.CreateFixture(circleShapeDef);

            // Large circle
            circle.Radius *= 2.0f;
            circleShapeDef.filter.groupIndex = k_largeGroup;
            circleBodyDef.position.Set(5.0f, 6.0f);

            b2Body body6 = m_world.CreateBody(circleBodyDef);
            body6.CreateFixture(circleShapeDef);
        }
    }
}
