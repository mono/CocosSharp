using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Box2D.Collision.Shapes;
using Box2D.Common;
using Box2D.Dynamics;
using Box2D.Dynamics.Joints;
using Box2D.TestBed;

namespace Box2D.TestBed.Tests
{
    class CircleStress : Test
    {
        private b2RevoluteJoint joint;
        
        public CircleStress()
        {
            b2Body leftWall = null;
            b2Body rightWall = null;
            {
                // Ground
                b2PolygonShape sd = new b2PolygonShape();
                sd.SetAsBox(50.0f, 10.0f);
                b2BodyDef bd = new b2BodyDef();
                bd.type = b2BodyType.b2_staticBody;
                bd.position = new b2Vec2(0.0f, -10.0f);
                b2Body b = m_world.CreateBody(bd);
                b2FixtureDef fd = new b2FixtureDef();
                fd.shape = sd;
                fd.friction = 1.0f;
                b.CreateFixture(fd);

                // Walls
                sd.SetAsBox(3.0f, 50.0f);
                bd = new b2BodyDef();
                bd.position = new b2Vec2(45.0f, 25.0f);
                rightWall = m_world.CreateBody(bd);
                rightWall.CreateFixture(sd, 0);
                bd.position = new b2Vec2(-45.0f, 25.0f);
                leftWall = m_world.CreateBody(bd);
                leftWall.CreateFixture(sd, 0);

                // Corners
                bd = new b2BodyDef();
                sd.SetAsBox(20.0f, 3.0f);
                bd.angle = (float)(-Math.PI / 4.0);
                bd.position = new b2Vec2(-35f, 8.0f);
                b2Body myBod = m_world.CreateBody(bd);
                myBod.CreateFixture(sd, 0);
                bd.angle = (float)(Math.PI / 4.0);
                bd.position = new b2Vec2(35f, 8.0f);
                myBod = m_world.CreateBody(bd);
                myBod.CreateFixture(sd, 0);

                // top
                sd.SetAsBox(50.0f, 10.0f);
                bd.type = b2BodyType.b2_staticBody;
                bd.angle = 0;
                bd.position = new b2Vec2(0.0f, 75.0f);
                b = m_world.CreateBody(bd);
                fd.shape = sd;
                fd.friction = 1.0f;
                b.CreateFixture(fd);
            }

            {
                b2CircleShape cd;
                b2FixtureDef fd = new b2FixtureDef();

                b2BodyDef bd = new b2BodyDef();
                bd.type = b2BodyType.b2_dynamicBody;
                int numPieces = 5;
                float radius = 6f;
                bd.position = new b2Vec2(0.0f, 10.0f);
                b2Body body = m_world.CreateBody(bd);
                for (int i = 0; i < numPieces; i++)
                {
                    cd = new b2CircleShape();
                    cd.Radius = 1.2f;
                    fd.shape = cd;
                    fd.density = 25;
                    fd.friction = .1f;
                    fd.restitution = .9f;
                    float xPos = radius * (float) Math.Cos(2f * Math.PI * (i / (float) (numPieces)));
                    float yPos = radius * (float) Math.Sin(2f * Math.PI * (i / (float) (numPieces)));
                    cd.Position.Set(xPos, yPos);

                    body.CreateFixture(fd);
                }

                body.SetBullet(false);

                b2RevoluteJointDef rjd = new b2RevoluteJointDef();
                rjd.Initialize(body, m_groundBody, body.Position);
                rjd.motorSpeed = (float) Math.PI;
                rjd.maxMotorTorque = 1000000.0f;
                rjd.enableMotor = true;
                joint = (b2RevoluteJoint) m_world.CreateJoint(rjd);
            }

            {
                int loadSize = 41;

                for (int j = 0; j < 15; j++)
                {
                    for (int i = 0; i < loadSize; i++)
                    {
                        b2CircleShape circ = new b2CircleShape();
                        b2BodyDef bod = new b2BodyDef();
                        bod.type = b2BodyType.b2_dynamicBody;
                        circ.Radius = 1.0f + (i % 2 == 0 ? 1.0f : -1.0f) * .5f * Rand.RandomFloat(.5f, 1f);
                        b2FixtureDef fd2 = new b2FixtureDef();
                        fd2.shape = circ;
                        fd2.density = circ.Radius * 1.5f;
                        fd2.friction = 0.5f;
                        fd2.restitution = 0.7f;
                        float xPos = -39f + 2 * i;
                        float yPos = 50f + j;
                        bod.position = new b2Vec2(xPos, yPos);
                        b2Body myBody = m_world.CreateBody(bod);
                        myBody.CreateFixture(fd2);

                    }
                }

            }

            m_world.Gravity = new b2Vec2(0, -50);
        }

        public override void Keyboard(char key)
        {
            switch (key)
            {
                case 's':
                    joint.SetMotorSpeed(0);
                    break;
                case '1':
                    joint.SetMotorSpeed((float)Math.PI);
                    break;
                case '2':
                    joint.SetMotorSpeed((float)Math.PI * 2);
                    break;
                case '3':
                    joint.SetMotorSpeed((float)Math.PI * 3);
                    break;
                case '4':
                    joint.SetMotorSpeed((float)Math.PI * 6);
                    break;
                case '5':
                    joint.SetMotorSpeed((float)Math.PI * 10);
                    break;
            }
        }
    }

}
