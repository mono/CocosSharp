using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Joints;
using FarseerPhysics.Factories;
using FarseerPhysics.TestBed.Framework;
using Microsoft.Xna.Framework;

namespace FarseerPhysics.TestBed.Tests
{
    public class Tumbler : Test
    {
        private const int Count = 50;

        private int m_Count;

        private Tumbler()
        {
            var ground = BodyFactory.CreateBody(World);

            var body = BodyFactory.CreateBody(World);
            body.BodyType = BodyType.Dynamic;
            body.Position = new Vector2(0.0f, 10.0f);

            PolygonShape shape = new PolygonShape(5);
            shape.SetAsBox(0.5f, 10.0f, new Vector2(10.0f, 0.0f), 0.0f);
            body.CreateFixture(shape);

            shape.SetAsBox(0.5f, 10.0f, new Vector2(-10.0f, 0.0f), 0.0f);
            body.CreateFixture(shape);

            shape.SetAsBox(10.0f, 0.5f, new Vector2(0.0f, 10.0f), 0.0f);
            body.CreateFixture(shape);

            shape.SetAsBox(10.0f, 0.5f, new Vector2(0.0f, -10.0f), 0.0f);
            body.CreateFixture(shape);

            var jd = new RevoluteJoint(ground, body, new Vector2(0.0f, 10.0f), new Vector2(0.0f, 0.0f));
            jd.ReferenceAngle = 0.0f;
            jd.MotorSpeed = 0.05f * MathHelper.Pi;
            jd.MaxMotorTorque = 1e8f;
            jd.MotorEnabled = true;

            World.AddJoint(jd);
        }

        public override void Update(GameSettings settings, GameTime gameTime)
        {
            base.Update(settings, gameTime);

            if (m_Count < Count)
            {
                var body = BodyFactory.CreateBody(World, new Vector2(0.0f, 10.0f));
                body.BodyType = BodyType.Dynamic;
                
                PolygonShape shape = new PolygonShape(5);
                shape.SetAsBox(0.125f, 0.125f);

                body.CreateFixture(shape);

                m_Count++;
            }
        }

        
        internal static Tumbler Create()
        {
            return new Tumbler();
        }
    }
}
