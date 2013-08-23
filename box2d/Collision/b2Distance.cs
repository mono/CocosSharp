/*
* Copyright (c) 2007-2009 Erin Catto http://www.box2d.org
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
using Box2D.Collision.Shapes;
using Box2D.Common;

namespace Box2D.Collision
{
	
	/// Used to warm start b2Math.b2Distance.
	/// Set count to zero on first call.
	public struct b2SimplexCache
	{
        public void Defaults()
        {
            metric = 0f;
            count = 0;
            indexA = new int[3];
            indexB = new int[3];
        }

        public static b2SimplexCache Create()
        {
            b2SimplexCache obj = new b2SimplexCache();
            obj.Defaults();
            return (obj);
        }

		public float metric;        ///< length or area
		public int count;
		public int[] indexA;    ///< vertices on shape A
		public int[] indexB;    ///< vertices on shape B
	}
	/// Input for b2Math.b2Distance.
	/// You have to option to use the shape radii
	/// in the computation. Even 
	public struct b2DistanceInput
	{
        public static b2DistanceInput Create()
        {
            b2DistanceInput result = new b2DistanceInput();
            result.proxyA = b2DistanceProxy.Create();
            result.proxyB = b2DistanceProxy.Create();
            result.transformA = b2Transform.Identity;
            result.transformB = b2Transform.Identity;
            result.useRadii = false;
            return (result);
        }

		public b2DistanceProxy proxyA;
		public b2DistanceProxy proxyB;
		public b2Transform transformA;
		public b2Transform transformB;
		public bool useRadii;
	};
	
	/// Output for b2Math.b2Distance.
	public struct b2DistanceOutput
	{
		public b2Vec2 pointA;        ///< closest point on shapeA
		public b2Vec2 pointB;        ///< closest point on shapeB
		public float distance;
		public int iterations;    ///< number of GJK iterations used
	};
	
	
    public struct b2Simplex
	{
        
        public class b2SimplexVertex
        {
            public b2Vec2 wA;        // support point in proxyA
            public b2Vec2 wB;        // support point in proxyB
            public b2Vec2 w;        // wB - wA
            public float a;        // barycentric coordinate for closest point
            public int indexA;    // wA index
            public int indexB;    // wB index

            public void CopyFrom(b2SimplexVertex other)
            {
                wA = other.wA;
                wB = other.wB;
                w = other.w;
                a = other.a;
                indexA = other.indexA;
                indexB = other.indexB;
            }
        };


        private static b2SimplexVertex[] m_vertices;
        private static b2SimplexVertex m_vertices_0;
        private static b2SimplexVertex m_vertices_1;
        private static b2SimplexVertex m_vertices_2;
        
        private int m_count;

        static b2Simplex()
        {
            m_vertices = new b2SimplexVertex[3];
            m_vertices[0] = new b2SimplexVertex();
            m_vertices[1] = new b2SimplexVertex();
            m_vertices[2] = new b2SimplexVertex();

            m_vertices_0 = m_vertices[0];
            m_vertices_1 = m_vertices[1];
            m_vertices_2 = m_vertices[2];
        }

		private void ReadCache(ref b2SimplexCache cache,
		                      b2DistanceProxy proxyA, ref b2Transform transformA,
		                      b2DistanceProxy proxyB, ref b2Transform transformB)
		{
			Debug.Assert(cache.count <= 3);

			// Copy data from cache.
			m_count = (int)cache.count;
			for (int i = 0; i < m_count; ++i)
			{
				b2SimplexVertex v = m_vertices[i];

                v.indexA = (int)cache.indexA[i];
				v.indexB = (int)cache.indexB[i];
				
                b2Vec2 wALocal = proxyA.m_vertices[v.indexA];
                b2Vec2 wBLocal = proxyB.m_vertices[v.indexB];

                v.wA.x = (transformA.q.c * wALocal.x - transformA.q.s * wALocal.y) + transformA.p.x;
                v.wA.y = (transformA.q.s * wALocal.x + transformA.q.c * wALocal.y) + transformA.p.y;

                v.wB.x = (transformB.q.c * wBLocal.x - transformB.q.s * wBLocal.y) + transformB.p.x;
                v.wB.y = (transformB.q.s * wBLocal.x + transformB.q.c * wBLocal.y) + transformB.p.y;

			    v.w.x = v.wB.x - v.wA.x;
                v.w.y = v.wB.y - v.wA.y;
                
                v.a = 0.0f;
			}
			
			// Compute the new simplex metric, if it is substantially different than
			// old metric then flush the simplex.
			if (m_count > 1)
			{
				float metric1 = cache.metric;
				float metric2 = GetMetric();
				if (metric2 < 0.5f * metric1 || 2.0f * metric1 < metric2 || metric2 < b2Settings.b2_epsilon)
				{
					// Reset the simplex.
					m_count = 0;
				}
			}
			
			// If the cache is empty or invalid ...
			if (m_count == 0)
			{
				b2SimplexVertex v = m_vertices_0;
				
                v.indexA = 0;
				v.indexB = 0;
				
                b2Vec2 wALocal = proxyA.m_vertices[0];
				b2Vec2 wBLocal = proxyB.m_vertices[0];

                v.wA.x = (transformA.q.c * wALocal.x - transformA.q.s * wALocal.y) + transformA.p.x;
                v.wA.y = (transformA.q.s * wALocal.x + transformA.q.c * wALocal.y) + transformA.p.y;

                v.wB.x = (transformB.q.c * wBLocal.x - transformB.q.s * wBLocal.y) + transformB.p.x;
                v.wB.y = (transformB.q.s * wBLocal.x + transformB.q.c * wBLocal.y) + transformB.p.y;

			    v.w.x = v.wB.x - v.wA.x;
                v.w.y = v.wB.y - v.wA.y;
                v.a = 0.0f;
				
                m_count = 1;
			}
		}
		
		public void WriteCache(ref b2SimplexCache cache)
		{
			cache.metric = GetMetric();
			
            cache.count = m_count;

            for (int i = 0; i < m_count; ++i)
			{
				cache.indexA[i] = m_vertices[i].indexA;
				cache.indexB[i] = m_vertices[i].indexB;
			}
		}

        public void GetSearchDirection(out b2Vec2 dir)
        {
            switch (m_count)
            {
                case 1:
                    dir.x = -m_vertices_0.w.x;
                    dir.y = -m_vertices_0.w.y;
                    return;

                case 2:
                {
                    float e12x = m_vertices_1.w.x - m_vertices_0.w.x;
                    float e12y = m_vertices_1.w.y - m_vertices_0.w.y;

                    float sgn = e12x * -m_vertices_0.w.y - e12y * -m_vertices_0.w.x;

                    if (sgn > 0.0f)
                    {
                        // Origin is left of e12.
                        dir.x = -e12y;
                        dir.y = e12x;
                    }
                    else
                    {
                        // Origin is right of e12.
                        dir.x = e12y;
                        dir.y = -e12x;
                    }
                    return;
                }

                default:
                    Debug.Assert(false);
                    dir = b2Vec2.Zero;
                    return;
            }
        }

        public void GetClosestPoint(out b2Vec2 point)
        {
            switch (m_count)
            {
                case 0:
                    Debug.Assert(false);
                    point.x = 0;
                    point.y = 0;
                    return;

                case 1:
                    point = m_vertices_0.w;
                    return;

                case 2:
                    point.x = m_vertices_0.a * m_vertices_0.w.x + m_vertices_1.a * m_vertices_1.w.x;
                    point.y = m_vertices_0.a * m_vertices_0.w.y + m_vertices_1.a * m_vertices_1.w.y;
                    return;

                case 3:
                    point.x = 0;
                    point.y = 0;
                    return;

                default:
                    Debug.Assert(false);
                    point.x = 0;
                    point.y = 0;
                    return;
            }
        }

        public void GetWitnessPoints(out b2Vec2 pA, out b2Vec2 pB)
		{
			switch (m_count)
			{
			case 0:
    			Debug.Assert(false);
			    pA = b2Vec2.Zero;
			    pB = b2Vec2.Zero;
				break;
				
			case 1:
                pA = m_vertices_0.wA;
                pB = m_vertices_0.wB;
				break;
				
			case 2:
                pA.x = m_vertices_0.a * m_vertices_0.wA.x + m_vertices_1.a * m_vertices_1.wA.x;
                pA.y = m_vertices_0.a * m_vertices_0.wA.y + m_vertices_1.a * m_vertices_1.wA.y;
                pB.x = m_vertices_0.a * m_vertices_0.wB.x + m_vertices_1.a * m_vertices_1.wB.x;
                pB.y = m_vertices_0.a * m_vertices_0.wB.y + m_vertices_1.a * m_vertices_1.wB.y;
				break;
				
			case 3:
                pA.x = m_vertices_0.a * m_vertices_0.wA.x + m_vertices_1.a * m_vertices_1.wA.x + m_vertices_2.a * m_vertices_2.wA.x;
                pA.y = m_vertices_0.a * m_vertices_0.wA.y + m_vertices_1.a * m_vertices_1.wA.y + m_vertices_2.a * m_vertices_2.wA.y;
				pB = pA;
				break;
				
			default:
				Debug.Assert(false);
			    pA = b2Vec2.Zero;
			    pB = b2Vec2.Zero;
				break;
			}
		}

        public float GetMetric()
        {
            switch (m_count)
            {
                case 0:
                    Debug.Assert(false);
                    return 0.0f;

                case 1:
                    return 0.0f;

                case 2:
                    float x = m_vertices_0.w.x - m_vertices_1.w.x;
                    float y = m_vertices_0.w.y - m_vertices_1.w.y;
                    return (float) Math.Sqrt(x * x + y * y);

                case 3:
                    float ax = m_vertices_1.w.x - m_vertices_0.w.x;
                    float ay = m_vertices_1.w.y - m_vertices_0.w.y;
                    float bx = m_vertices_2.w.x - m_vertices_0.w.x;
                    float by = m_vertices_2.w.y - m_vertices_0.w.y;
                    return ax * by - ay * bx;

                default:
                    Debug.Assert(false);
                    return 0.0f;
            }
        }


        // Solve a line segment using barycentric coordinates.
		//
		// p = a1 * w1 + a2 * w2
		// a1 + a2 = 1
		//
		// The vector from the origin to the closest point on the line is
		// perpendicular to the line.
		// e12 = w2 - w1
		// dot(p, e) = 0
		// a1 * dot(w1, e) + a2 * dot(w2, e) = 0
		//
		// 2-by-2 linear system
		// [1      1     ][a1] = [1]
		// [w1.e12 w2.e12][a2] = [0]
		//
		// Define
		// d12_1 =  dot(w2, e12)
		// d12_2 = -dot(w1, e12)
		// d12 = d12_1 + d12_2
		//
		// Solution
		// a1 = d12_1 / d12
		// a2 = d12_2 / d12
		public void Solve2()
		{
            float w1x = m_vertices_0.w.x;
            float w1y = m_vertices_0.w.y;
            float w2x = m_vertices_1.w.x;
            float w2y = m_vertices_1.w.y;

            float e12x = w2x - w1x;
            float e12y = w2y - w1y;
			
			// w1 region
            float d12_2 = -(w1x * e12x + w1y * e12y);
			
            if (d12_2 <= 0.0f)
			{
				// a2 <= 0, so we clamp it to 0
				m_vertices_0.a = 1.0f;
				m_count = 1;
				return;
			}
			
			// w2 region
            float d12_1 = w2x * e12x + w2y * e12y;
			if (d12_1 <= 0.0f)
			{
				// a1 <= 0, so we clamp it to 0
                m_vertices_1.a = 1.0f;
				m_count = 1;

			    m_vertices_0.wA = m_vertices_1.wA;
			    m_vertices_0.wB = m_vertices_1.wB;
			    m_vertices_0.w = m_vertices_1.w;
			    m_vertices_0.a = m_vertices_1.a;
			    m_vertices_0.indexA = m_vertices_1.indexA;
			    m_vertices_0.indexB = m_vertices_1.indexB;

			    return;
			}
			
			// Must be in e12 region.
			float inv_d12 = 1.0f / (d12_1 + d12_2);
            m_vertices_0.a = d12_1 * inv_d12;
            m_vertices_1.a = d12_2 * inv_d12;
			m_count = 2;
		}
		
		// Possible regions:
		// - points[2]
		// - edge points[0]-points[2]
		// - edge points[1]-points[2]
		// - inside the triangle
		public void Solve3()
		{
            float w1x = m_vertices_0.w.x;
            float w1y = m_vertices_0.w.y;
            float w2x = m_vertices_1.w.x;
            float w2y = m_vertices_1.w.y;
            float w3x = m_vertices_2.w.x;
            float w3y = m_vertices_2.w.y;
			
			// Edge12
			// [1      1     ][a1] = [1]
			// [w1.e12 w2.e12][a2] = [0]
			// a3 = 0
            float e12x = w2x - w1x;
            float e12y = w2y - w1y;
            
            float w1e12 = w1x * e12x + w1y * e12y;
            float w2e12 = w2x * e12x + w2y * e12y;
			float d12_1 = w2e12;
			float d12_2 = -w1e12;
			
			// Edge13
			// [1      1     ][a1] = [1]
			// [w1.e13 w3.e13][a3] = [0]
			// a2 = 0
            float e13x = w3x - w1x;
            float e13y = w3y - w1y;

            float w1e13 = w1x * e13x + w1y * e13y;
            float w3e13 = w3x * e13x + w3y * e13y;
			float d13_1 = w3e13;
			float d13_2 = -w1e13;
			
			// Edge23
			// [1      1     ][a2] = [1]
			// [w2.e23 w3.e23][a3] = [0]
			// a1 = 0
            float e23x = w3x - w2x;
            float e23y = w3y - w2y;

            float w2e23 = w2x * e23x + w2y * e23y;
            float w3e23 = w3x * e23x + w3y * e23y;
			float d23_1 = w3e23;
			float d23_2 = -w2e23;
			
			// Triangle123
            float n123 = e12x * e13y - e12y * e13x;

            float d123_1 = n123 * (w2x * w3y - w2y * w3x);
            float d123_2 = n123 * (w3x * w1y - w3y * w1x);
            float d123_3 = n123 * (w1x * w2y - w1y * w2x);
			
			// w1 region
			if (d12_2 <= 0.0f && d13_2 <= 0.0f)
			{
                m_vertices_0.a = 1.0f;
				m_count = 1;
				return;
			}
			
			// e12
			if (d12_1 > 0.0f && d12_2 > 0.0f && d123_3 <= 0.0f)
			{
				float inv_d12 = 1.0f / (d12_1 + d12_2);
                m_vertices_0.a = d12_1 * inv_d12;
                m_vertices_1.a = d12_2 * inv_d12;
				m_count = 2;
				return;
			}
			
			// e13
			if (d13_1 > 0.0f && d13_2 > 0.0f && d123_2 <= 0.0f)
			{
				float inv_d13 = 1.0f / (d13_1 + d13_2);
                m_vertices_0.a = d13_1 * inv_d13;
                m_vertices_2.a = d13_2 * inv_d13;
				m_count = 2;

			    m_vertices_1.wA = m_vertices_2.wA;
			    m_vertices_1.wB = m_vertices_2.wB;
			    m_vertices_1.w = m_vertices_2.w;
			    m_vertices_1.a = m_vertices_2.a;
			    m_vertices_1.indexA = m_vertices_2.indexA;
			    m_vertices_1.indexB = m_vertices_2.indexB;

			    return;
			}
			
			// w2 region
			if (d12_1 <= 0.0f && d23_2 <= 0.0f)
			{
                m_vertices_1.a = 1.0f;
				m_count = 1;

			    m_vertices_0.wA = m_vertices_1.wA;
			    m_vertices_0.wB = m_vertices_1.wB;
			    m_vertices_0.w = m_vertices_1.w;
			    m_vertices_0.a = m_vertices_1.a;
			    m_vertices_0.indexA = m_vertices_1.indexA;
			    m_vertices_0.indexB = m_vertices_1.indexB;

			    return;
			}
			
			// w3 region
			if (d13_1 <= 0.0f && d23_1 <= 0.0f)
			{
                m_vertices_2.a = 1.0f;
				m_count = 1;

			    m_vertices_0.wA = m_vertices_2.wA;
			    m_vertices_0.wB = m_vertices_2.wB;
			    m_vertices_0.w = m_vertices_2.w;
			    m_vertices_0.a = m_vertices_2.a;
			    m_vertices_0.indexA = m_vertices_2.indexA;
			    m_vertices_0.indexB = m_vertices_2.indexB;

			    return;
			}
			
			// e23
			if (d23_1 > 0.0f && d23_2 > 0.0f && d123_1 <= 0.0f)
			{
				float inv_d23 = 1.0f / (d23_1 + d23_2);
				m_vertices_1.a = d23_1 * inv_d23;
				m_vertices_2.a = d23_2 * inv_d23;
				m_count = 2;

			    m_vertices_0.wA = m_vertices_2.wA;
			    m_vertices_0.wB = m_vertices_2.wB;
			    m_vertices_0.w = m_vertices_2.w;
			    m_vertices_0.a = m_vertices_2.a;
			    m_vertices_0.indexA = m_vertices_2.indexA;
			    m_vertices_0.indexB = m_vertices_2.indexB;

			    return;
			}
			
			// Must be in triangle123
			float inv_d123 = 1.0f / (d123_1 + d123_2 + d123_3);
			m_vertices_0.a = d123_1 * inv_d123;
			m_vertices_1.a = d123_2 * inv_d123;
			m_vertices_2.a = d123_3 * inv_d123;
			m_count = 3;
		}

        private static int[] _saveA = new int[3];
        private static int[] _saveB = new int[3];

		public static void b2Distance(out b2DistanceOutput output, ref b2SimplexCache cache, ref b2DistanceInput input)
		{
			++b2DistanceProxy.b2_gjkCalls;

			b2DistanceProxy proxyA = input.proxyA;
			b2DistanceProxy proxyB = input.proxyB;
			
			// Initialize the simplex.
			b2Simplex simplex = new b2Simplex();
            simplex.ReadCache(ref cache, proxyA, ref input.transformA, proxyB, ref input.transformB);
			
			// Get simplex vertices as an array.
            b2SimplexVertex[] vertices = b2Simplex.m_vertices;
			int k_maxIters = 20;
			
			// These store the vertices of the last simplex so that we
			// can check for duplicates and prevent cycling.
            int[] saveA = _saveA;
            int[] saveB = _saveB;
			int saveCount = 0;

		    b2Vec2 closestPoint;
            simplex.GetClosestPoint(out closestPoint);
			float distanceSqr1 = closestPoint.LengthSquared;
			float distanceSqr2 = distanceSqr1;
			
//            Console.WriteLine("Closest Point={0},{1}, distance={2}", closestPoint.x, closestPoint.y, distanceSqr1);


			// Main iteration loop.
            #region Main Iteration Loop
			int iter = 0;
			while (iter < k_maxIters)
			{
				// Copy simplex so we can identify duplicates.
				saveCount = simplex.m_count;
				for (int i = 0; i < saveCount; ++i)
				{
					saveA[i] = vertices[i].indexA;
					saveB[i] = vertices[i].indexB;
				}
				
				switch (simplex.m_count)
				{
				case 1:
					break;
					
				case 2:
					simplex.Solve2();
					break;
					
				case 3:
					simplex.Solve3();
					break;
					
				default:
					Debug.Assert(false);
					break;
				}
				
				// If we have 3 points, then the origin is in the corresponding triangle.
				if (simplex.m_count == 3)
				{
					break;
				}
				
				// Compute closest point.
			    b2Vec2 p;
                simplex.GetClosestPoint(out p);
			    distanceSqr2 = p.x * p.x + p.y * p.y;

			    // Ensure progress
				if (distanceSqr2 >= distanceSqr1)
				{
					//break;
				}
				distanceSqr1 = distanceSqr2;
				
				// Get search direction.
			    b2Vec2 d;
                simplex.GetSearchDirection(out d);
				
				// Ensure the search direction is numerically fit.
				if ((d.x * d.x + d.y * d.y) < b2Settings.b2_epsilon * b2Settings.b2_epsilon)
				{
					// The origin is probably contained by a line segment
					// or triangle. Thus the shapes are overlapped.
					
					// We can't return zero here even though there may be overlap.
					// In case the simplex is a point, segment, or triangle it is difficult
					// to determine if the origin is contained in the CSO or very close to it.
					break;
				}
				
				// Compute a tentative new simplex vertex using support points.
				b2SimplexVertex vertex = vertices[simplex.m_count];

			    var q = input.transformA.q;

                b2Vec2 b;
                b.x = q.c * -d.x + q.s * -d.y;
                b.y = -q.s * -d.x + q.c * -d.y;

                vertex.indexA = proxyA.GetSupport(ref b);

			    var vA = proxyA.m_vertices[vertex.indexA];

                vertex.wA.x = (q.c * vA.x - q.s * vA.y) + input.transformA.p.x;
                vertex.wA.y = (q.s * vA.x + q.c * vA.y) + input.transformA.p.y;

			    //                b2Vec2 wBLocal = new b2Vec2();
			    q = input.transformB.q;
                b.x = q.c * d.x + q.s * d.y;
                b.y = -q.s * d.x + q.c * d.y;

			    vertex.indexB = proxyB.GetSupport(ref b);

			    var vB = proxyB.m_vertices[vertex.indexB];

                vertex.wB.x = (input.transformB.q.c * vB.x - input.transformB.q.s * vB.y) + input.transformB.p.x;
                vertex.wB.y = (input.transformB.q.s * vB.x + input.transformB.q.c * vB.y) + input.transformB.p.y;

                vertex.w.x = vertex.wB.x - vertex.wA.x;
                vertex.w.y = vertex.wB.y - vertex.wA.y;
				
				// Iteration count is equated to the number of support point calls.
				++iter;
				++b2DistanceProxy.b2_gjkIters;
				
				// Check for duplicate support points. This is the main termination criteria.
				bool duplicate = false;
				for (int i = 0; i < saveCount; ++i)
				{
					if (vertex.indexA == saveA[i] && vertex.indexB == saveB[i])
					{
						duplicate = true;
						break;
					}
				}
				
				// If we found a duplicate support point we must exit to avoid cycling.
				if (duplicate)
				{
					break;
				}
				
				// New vertex is ok and needed.
				++simplex.m_count;
			}
            #endregion
			
			b2DistanceProxy.b2_gjkMaxIters = Math.Max(b2DistanceProxy.b2_gjkMaxIters, iter);
			
			// Prepare output.
            simplex.GetWitnessPoints(out output.pointA, out output.pointB);
			output.distance = b2Math.b2Distance(ref output.pointA, ref output.pointB);
			output.iterations = iter;
			
			// Cache the simplex.
			simplex.WriteCache(ref cache);
			
			// Apply radii if requested.
			if (input.useRadii)
			{
				float rA = proxyA.Radius;
				float rB = proxyB.Radius;
				
				if (output.distance > rA + rB && output.distance > b2Settings.b2_epsilon)
				{
					// Shapes are still not overlapped.
					// Move the witness points to the outer surface.
					output.distance -= rA + rB;
					b2Vec2 normal;
                    normal.x = output.pointB.x - output.pointA.x;
                    normal.y = output.pointB.y - output.pointA.y;
                    
                    normal.Normalize();

                    output.pointA.x += rA * normal.x;
                    output.pointA += rA * normal;

                    output.pointB.x -= rB * normal.x;
                    output.pointB.y -= rB * normal.y;
                }
				else
				{
					// Shapes are overlapped when radii are considered.
					// Move the witness points to the middle.
					b2Vec2 p;
                    p.x = 0.5f * (output.pointA.x + output.pointB.x);
                    p.y = 0.5f * (output.pointA.y + output.pointB.y);
                    
                    output.pointA = p;
					output.pointB = p;
					output.distance = 0.0f;
				}
			}
		}
	}
}

