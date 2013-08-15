/*
* Farseer Physics Engine based on Box2D.XNA port:
* Copyright (c) 2010 Ian Qvist
* 
* Box2D.XNA port of Box2D:
* Copyright (c) 2009 Brandon Furtwangler, Nathan Furtwangler
*
* Original source Box2D:
* Copyright (c) 2006-2009 Erin Catto http://www.gphysics.com 
* 
* This software is provided 'as-is', without any express or implied 
* warranty.  In no event will the authors be held liable for any damages 
* arising from the use of this software. 
* Permission is granted to anyone to use this software for any purpose, 
* including commercial applications, and to alter it and redistribute it 
* freely, subject to the following restrictions: 
* 1. The origin of this software must not be misrepresented; you must not 
* claim that you wrote the original software. If you use this software 
* in a product, an acknowledgment in the product documentation would be 
* appreciated but is not required. 
* 2. Altered source versions must be plainly marked as such, and must not be 
* misrepresented as being the original software. 
* 3. This notice may not be removed or altered from any source distribution. 
*/


using Box2D.TestBed.Tests;

namespace Box2D.TestBed
{
    public static class TestEntries
    {
        public static TestEntry[] TestList =
            {
                //Original tests
                new TestEntry {Name = "Tumbler", CreateFcn = () => new Tumbler()},
                new TestEntry {Name = "Tiles", CreateFcn = () => new Tiles()},
                new TestEntry {Name = "Dump Shell", CreateFcn = () => new DumpShell()},
                new TestEntry {Name = "Gears", CreateFcn = () => new Gears()},
                new TestEntry {Name = "Cantilever", CreateFcn = () => new Cantilever()},
                new TestEntry {Name = "Varying Restitution", CreateFcn = () => new VaryingRestitution()},
                new TestEntry {Name = "Character Collision", CreateFcn = () => new CharacterCollision()},
                new TestEntry {Name = "Edge Test", CreateFcn = () => new EdgeTest()},
                new TestEntry {Name = "Body Types", CreateFcn = () => new BodyTypes()},
                new TestEntry {Name = "Shape Editing", CreateFcn = () => new ShapeEditing()},
                new TestEntry {Name = "Car", CreateFcn = () => new Car()},
                new TestEntry {Name = "Apply Force", CreateFcn = () => new ApplyForce()},
                new TestEntry {Name = "Prismatic", CreateFcn = () => new Prismatic()},
                new TestEntry {Name = "Vertical Stack", CreateFcn = () => new VerticalStack()},
                new TestEntry {Name = "Sphere Stack", CreateFcn = () => new SphereStack()},
                new TestEntry {Name = "Revolute", CreateFcn = () => new Revolute()},
                new TestEntry {Name = "Pulleys", CreateFcn = () => new Pulleys()},
                //new TestEntry {Name = "Poly Shapes", CreateFcn = () => new PolyShapes()},
                //new TestEntry {Name = "Rope", CreateFcn = () => new Tests.Rope()},
                new TestEntry {Name = "Web", CreateFcn = () => new Web()},
                new TestEntry {Name = "Rope Joint", CreateFcn = () => new RopeJoint()},
                new TestEntry {Name = "OneSided Platform", CreateFcn = () => new OneSidedPlatform()},
                new TestEntry {Name = "Pinball", CreateFcn = () => new Pinball()},
                new TestEntry {Name = "Bullet Test", CreateFcn = () => new BulletTest()},
                new TestEntry {Name = "Continuous Test", CreateFcn = () => new ContinuousTest()},
                //new TestEntry {Name = "Time Of Impact", CreateFcn = () => new TimeOfImpact()},
                //new TestEntry {Name = "Ray Cast", CreateFcn = () => new RayCast()},
                new TestEntry {Name = "Confined", CreateFcn = () => new Confined()},
                new TestEntry {Name = "Pyramid", CreateFcn = () => new Pyramid()},
                new TestEntry {Name = "Theo Jansen's Walker", CreateFcn = () => new TheoJansen()},
                //new TestEntry {Name = "Edge Shapes", CreateFcn = () => new EdgeShapes()},
                new TestEntry {Name = "Poly Collision", CreateFcn = () => new PolyCollision()},
                new TestEntry {Name = "Bridge", CreateFcn = () => new Bridge()},
                new TestEntry {Name = "Breakable", CreateFcn = () => new Breakable()},
                new TestEntry {Name = "Chain", CreateFcn = () => new Chain()},
                new TestEntry {Name = "Collision Filtering", CreateFcn = () => new CollisionFiltering()},
                new TestEntry {Name = "Collision Processing", CreateFcn = () => new CollisionProcessing()},
                new TestEntry {Name = "Compound Shapes", CreateFcn = () => new CompoundShapes()},
                //new TestEntry {Name = "Distance Test", CreateFcn = () => new DistanceTest()},
                new TestEntry {Name = "Dominos", CreateFcn = () => new Dominos()},
                //new TestEntry {Name = "DynamicTree Test", CreateFcn = () => new DynamicTreeTest()},
                new TestEntry {Name = "Sensor Test", CreateFcn = () => new SensorTest()},
                new TestEntry {Name = "Slider Crank", CreateFcn = () => new SliderCrank()},
                new TestEntry {Name = "Varying Friction", CreateFcn = () => new VaryingFriction()},
                new TestEntry {Name = "AddPair", CreateFcn = () => new AddPair()},
                new TestEntry {Name = null, CreateFcn = null}
            };
    }
}