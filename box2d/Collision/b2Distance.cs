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
        public static b2SimplexCache Default = b2SimplexCache.Create();

        public void Defaults()
        {
            metric = 0f;
            count = 0;
            indexA = new uint[3];
            indexB = new uint[3];
        }

        public static b2SimplexCache Create()
        {
            b2SimplexCache obj = new b2SimplexCache();
            obj.Defaults();
            return (obj);
        }

		public float metric;        ///< length or area
		public int count;
		public uint[] indexA;    ///< vertices on shape A
		public uint[] indexB;    ///< vertices on shape B
	}
	/// Input for b2Math.b2Distance.
	/// You have to option to use the shape radii
	/// in the computation. Even 
	public struct b2DistanceInput
	{
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
	
	
	public struct b2SimplexVertex
	{
		public b2Vec2 wA;        // support point in proxyA
		public b2Vec2 wB;        // support point in proxyB
		public b2Vec2 w;        // wB - wA
		public float a;        // barycentric coordinate for closest point
		public int indexA;    // wA index
		public int indexB;    // wB index
	};
	
	
	
	
	
	public class b2Simplex
	{
		private b2Vec2 b2Vec2_zero = new b2Vec2(0f, 0f);
		
		public void ReadCache(b2SimplexCache cache,
		                      b2DistanceProxy proxyA, b2Transform transformA,
		                      b2DistanceProxy proxyB, b2Transform transformB)
		{
			Debug.Assert(cache.count <= 3);
			
			// Copy data from cache.
			m_count = (int)cache.count;
			b2SimplexVertex[] vertices = new b2SimplexVertex[] { m_v1, m_v2, m_v3 };
			for (int i = 0; i < m_count; ++i)
			{
				b2SimplexVertex v = vertices[i];
				v.indexA = (int)cache.indexA[i];
				v.indexB = (int)cache.indexB[i];
				b2Vec2 wALocal = proxyA.GetVertex(v.indexA);
				b2Vec2 wBLocal = proxyB.GetVertex(v.indexB);
				v.wA = b2Math.b2Mul(transformA, wALocal);
				v.wB = b2Math.b2Mul(transformB, wBLocal);
				v.w = v.wB - v.wA;
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
				b2SimplexVertex v = vertices[0];
				v.indexA = 0;
				v.indexB = 0;
				b2Vec2 wALocal = proxyA.GetVertex(0);
				b2Vec2 wBLocal = proxyB.GetVertex(0);
				v.wA = b2Math.b2Mul(transformA, wALocal);
				v.wB = b2Math.b2Mul(transformB, wBLocal);
				v.w = v.wB - v.wA;
				m_count = 1;
			}
		}
		
		public void WriteCache(b2SimplexCache cache)
		{
			cache.metric = GetMetric();
			cache.count = m_count;
			b2SimplexVertex[] vertices = new b2SimplexVertex[] { m_v1, m_v2, m_v3 };
			for (int i = 0; i < m_count; ++i)
			{
				cache.indexA[i] = (byte)(vertices[i].indexA);
				cache.indexB[i] = (byte)(vertices[i].indexB);
			}
		}
		
		public b2Vec2 GetSearchDirection()
		{
			switch (m_count)
			{
			case 1:
				return -m_v1.w;
				
			case 2:
			{
				b2Vec2 e12 = m_v2.w - m_v1.w;
				float sgn = b2Math.b2Cross(e12, -m_v1.w);
				if (sgn > 0.0f)
				{
					// Origin is left of e12.
					return b2Math.b2Cross(1.0f, e12);
				}
				else
				{
					// Origin is right of e12.
					return b2Math.b2Cross(e12, 1.0f);
				}
			}
				
			default:
				Debug.Assert(false);
				return b2Vec2_zero;
			}
		}
		
		public b2Vec2 GetClosestPoint()
		{
			switch (m_count)
			{
			case 0:
				Debug.Assert(false);
				return b2Vec2_zero;
				
			case 1:
				return m_v1.w;
				
			case 2:
				return m_v1.a * m_v1.w + m_v2.a * m_v2.w;
				
			case 3:
				return b2Vec2_zero;
				
			default:
				Debug.Assert(false);
				return b2Vec2_zero;
			}
		}
		
		public void GetWitnessPoints(ref b2Vec2 pA, ref b2Vec2 pB)
		{
			switch (m_count)
			{
			case 0:
				Debug.Assert(false);
				break;
				
			case 1:
				pA = m_v1.wA;
				pB = m_v1.wB;
				break;
				
			case 2:
				pA = m_v1.a * m_v1.wA + m_v2.a * m_v2.wA;
				pB = m_v1.a * m_v1.wB + m_v2.a * m_v2.wB;
				break;
				
			case 3:
				pA = m_v1.a * m_v1.wA + m_v2.a * m_v2.wA + m_v3.a * m_v3.wA;
				pB = pA;
				break;
				
			default:
				Debug.Assert(false);
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
				return b2Math.b2Distance(m_v1.w, m_v2.w);
				
			case 3:
				return b2Math.b2Cross(m_v2.w - m_v1.w, m_v3.w - m_v1.w);
				
			default:
				Debug.Assert(false);
				return 0.0f;
			}
		}
		
		
		private b2SimplexVertex m_v1 = new b2SimplexVertex();
		private b2SimplexVertex m_v2 = new b2SimplexVertex();
		private b2SimplexVertex m_v3 = new b2SimplexVertex();
		private int m_count;
		
		
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
			b2Vec2 w1 = m_v1.w;
			b2Vec2 w2 = m_v2.w;
			b2Vec2 e12 = w2 - w1;
			
			// w1 region
			float d12_2 = -b2Math.b2Dot(w1, e12);
			if (d12_2 <= 0.0f)
			{
				// a2 <= 0, so we clamp it to 0
				m_v1.a = 1.0f;
				m_count = 1;
				return;
			}
			
			// w2 region
			float d12_1 = b2Math.b2Dot(w2, e12);
			if (d12_1 <= 0.0f)
			{
				// a1 <= 0, so we clamp it to 0
				m_v2.a = 1.0f;
				m_count = 1;
				m_v1 = m_v2;
				return;
			}
			
			// Must be in e12 region.
			float inv_d12 = 1.0f / (d12_1 + d12_2);
			m_v1.a = d12_1 * inv_d12;
			m_v2.a = d12_2 * inv_d12;
			m_count = 2;
		}
		
		// Possible regions:
		// - points[2]
		// - edge points[0]-points[2]
		// - edge points[1]-points[2]
		// - inside the triangle
		public void Solve3()
		{
			b2Vec2 w1 = m_v1.w;
			b2Vec2 w2 = m_v2.w;
			b2Vec2 w3 = m_v3.w;
			
			// Edge12
			// [1      1     ][a1] = [1]
			// [w1.e12 w2.e12][a2] = [0]
			// a3 = 0
			b2Vec2 e12 = w2 - w1;
			float w1e12 = b2Math.b2Dot(w1, e12);
			float w2e12 = b2Math.b2Dot(w2, e12);
			float d12_1 = w2e12;
			float d12_2 = -w1e12;
			
			// Edge13
			// [1      1     ][a1] = [1]
			// [w1.e13 w3.e13][a3] = [0]
			// a2 = 0
			b2Vec2 e13 = w3 - w1;
			float w1e13 = b2Math.b2Dot(w1, e13);
			float w3e13 = b2Math.b2Dot(w3, e13);
			float d13_1 = w3e13;
			float d13_2 = -w1e13;
			
			// Edge23
			// [1      1     ][a2] = [1]
			// [w2.e23 w3.e23][a3] = [0]
			// a1 = 0
			b2Vec2 e23 = w3 - w2;
			float w2e23 = b2Math.b2Dot(w2, e23);
			float w3e23 = b2Math.b2Dot(w3, e23);
			float d23_1 = w3e23;
			float d23_2 = -w2e23;
			
			// Triangle123
			float n123 = b2Math.b2Cross(e12, e13);
			
			float d123_1 = n123 * b2Math.b2Cross(w2, w3);
			float d123_2 = n123 * b2Math.b2Cross(w3, w1);
			float d123_3 = n123 * b2Math.b2Cross(w1, w2);
			
			// w1 region
			if (d12_2 <= 0.0f && d13_2 <= 0.0f)
			{
				m_v1.a = 1.0f;
				m_count = 1;
				return;
			}
			
			// e12
			if (d12_1 > 0.0f && d12_2 > 0.0f && d123_3 <= 0.0f)
			{
				float inv_d12 = 1.0f / (d12_1 + d12_2);
				m_v1.a = d12_1 * inv_d12;
				m_v2.a = d12_2 * inv_d12;
				m_count = 2;
				return;
			}
			
			// e13
			if (d13_1 > 0.0f && d13_2 > 0.0f && d123_2 <= 0.0f)
			{
				float inv_d13 = 1.0f / (d13_1 + d13_2);
				m_v1.a = d13_1 * inv_d13;
				m_v3.a = d13_2 * inv_d13;
				m_count = 2;
				m_v2 = m_v3;
				return;
			}
			
			// w2 region
			if (d12_1 <= 0.0f && d23_2 <= 0.0f)
			{
				m_v2.a = 1.0f;
				m_count = 1;
				m_v1 = m_v2;
				return;
			}
			
			// w3 region
			if (d13_1 <= 0.0f && d23_1 <= 0.0f)
			{
				m_v3.a = 1.0f;
				m_count = 1;
				m_v1 = m_v3;
				return;
			}
			
			// e23
			if (d23_1 > 0.0f && d23_2 > 0.0f && d123_1 <= 0.0f)
			{
				float inv_d23 = 1.0f / (d23_1 + d23_2);
				m_v2.a = d23_1 * inv_d23;
				m_v3.a = d23_2 * inv_d23;
				m_count = 2;
				m_v1 = m_v3;
				return;
			}
			
			// Must be in triangle123
			float inv_d123 = 1.0f / (d123_1 + d123_2 + d123_3);
			m_v1.a = d123_1 * inv_d123;
			m_v2.a = d123_2 * inv_d123;
			m_v3.a = d123_3 * inv_d123;
			m_count = 3;
		}
		
		public static void b2Distance(ref b2DistanceOutput output, ref b2SimplexCache cache, ref b2DistanceInput input)
		{
			++b2DistanceProxy.b2_gjkCalls;
			
			b2DistanceProxy proxyA = input.proxyA;
			b2DistanceProxy proxyB = input.proxyB;
			
			b2Transform transformA = input.transformA;
			b2Transform transformB = input.transformB;
			
			// Initialize the simplex.
			b2Simplex simplex = new b2Simplex();
			simplex.ReadCache(cache, proxyA, transformA, proxyB, transformB);
			
			// Get simplex vertices as an array.
			b2SimplexVertex[] vertices = new b2SimplexVertex[] { simplex.m_v1, simplex.m_v2, simplex.m_v3 };
			int k_maxIters = 20;
			
			// These store the vertices of the last simplex so that we
			// can check for duplicates and prevent cycling.
			int[] saveA = new int[3];
			int[] saveB = new int[3];
			int saveCount = 0;
			
			b2Vec2 closestPoint = simplex.GetClosestPoint();
			float distanceSqr1 = closestPoint.LengthSquared();
			float distanceSqr2 = distanceSqr1;
			
			// Main iteration loop.
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
				b2Vec2 p = simplex.GetClosestPoint();
				distanceSqr2 = p.LengthSquared();
				
				// Ensure progress
				if (distanceSqr2 >= distanceSqr1)
				{
					//break;
				}
				distanceSqr1 = distanceSqr2;
				
				// Get search direction.
				b2Vec2 d = simplex.GetSearchDirection();
				
				// Ensure the search direction is numerically fit.
				if (d.LengthSquared() < b2Settings.b2_epsilon * b2Settings.b2_epsilon)
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
				vertex.indexA = proxyA.GetSupport(b2Math.b2MulT(transformA.q, -d));
				vertex.wA = b2Math.b2Mul(transformA, proxyA.GetVertex(vertex.indexA));
				//                b2Vec2 wBLocal = new b2Vec2();
				vertex.indexB = proxyB.GetSupport(b2Math.b2MulT(transformB.q, d));
				vertex.wB = b2Math.b2Mul(transformB, proxyB.GetVertex(vertex.indexB));
				vertex.w = vertex.wB - vertex.wA;
				
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
			
			b2DistanceProxy.b2_gjkMaxIters = Math.Max(b2DistanceProxy.b2_gjkMaxIters, iter);
			
			// Prepare output.
			simplex.GetWitnessPoints(ref output.pointA, ref output.pointB);
			output.distance = b2Math.b2Distance(output.pointA, output.pointB);
			output.iterations = iter;
			
			// Cache the simplex.
			simplex.WriteCache(cache);
			
			// Apply radii if requested.
			if (input.useRadii)
			{
				float rA = proxyA.Radius;
				float rB = proxyB.Radius;
				
				if (output.distance > rA + rB && output.distance > b2Settings.b2_epsilon)
				{
					// Shapes are still no overlapped.
					// Move the witness points to the outer surface.
					output.distance -= rA + rB;
					b2Vec2 normal = output.pointB - output.pointA;
					normal.Normalize();
					output.pointA += rA * normal;
					output.pointB -= rB * normal;
				}
				else
				{
					// Shapes are overlapped when radii are considered.
					// Move the witness points to the middle.
					b2Vec2 p = 0.5f * (output.pointA + output.pointB);
					output.pointA = p;
					output.pointB = p;
					output.distance = 0.0f;
				}
			}
		}
	}
}

