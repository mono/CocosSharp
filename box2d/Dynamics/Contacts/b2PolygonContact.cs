/*
* Copyright (c) 2006-2009 Erin Catto http://www.box2d.org
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
using System;
using System.Diagnostics;
using Box2D.Common;
using Box2D.Collision.Shapes;
using Box2D.Collision;

namespace Box2D.Dynamics.Contacts
{
    /*
    public class b2PolygonContact : b2Contact
    {
		public b2PolygonContact(b2Fixture fixtureA, int indexA, b2Fixture fixtureB, int indexB)
			: this(fixtureA, fixtureB)
		{	}
        public b2PolygonContact(b2Fixture fixtureA, b2Fixture fixtureB)
            : base(fixtureA, 0, fixtureB, 0)
        {
            Debug.Assert(m_fixtureA.ShapeType == b2ShapeType.e_polygon);
            Debug.Assert(m_fixtureB.ShapeType == b2ShapeType.e_polygon);
        }

        public override void Evaluate(ref b2Manifold manifold, ref b2Transform xfA, ref b2Transform xfB)
        {
            b2Collision.b2CollidePolygons(ref manifold,
                                (b2PolygonShape)m_fixtureA.Shape, ref xfA,
                                (b2PolygonShape)m_fixtureB.Shape, ref xfB);
        }
    }
    */
}