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
	
	public enum SeparationType
	{
		e_points,
		e_faceA,
		e_faceB
	};
	
	public struct b2SeparationFunction
	{
		
		// TODO_ERIN might not need to return the separation
		
		public float Initialize(ref b2SimplexCache cache,
		                        ref b2DistanceProxy proxyA, ref b2Sweep sweepA,
		                        ref b2DistanceProxy proxyB, ref b2Sweep sweepB,
		                        float t1)
		{
			m_proxyA = proxyA;
			m_proxyB = proxyB;
			int count = cache.count;
			Debug.Assert(0 < count && count < 3);
			
			m_sweepA = sweepA;
			m_sweepB = sweepB;
			
			b2Transform xfA = b2Transform.Identity, xfB = b2Transform.Identity;
			m_sweepA.GetTransform(ref xfA, t1);
			m_sweepB.GetTransform(ref xfB, t1);
			
			if (count == 1)
			{
				m_type = SeparationType.e_points;
				b2Vec2 localPointA = m_proxyA.GetVertex((int)cache.indexA[0]);
				b2Vec2 localPointB = m_proxyB.GetVertex((int)cache.indexB[0]);
				b2Vec2 pointA = b2Math.b2Mul(xfA, localPointA);
				b2Vec2 pointB = b2Math.b2Mul(xfB, localPointB);
				m_axis = pointB - pointA;
				float s = m_axis.Normalize();
				return s;
			}
			else if (cache.indexA[0] == cache.indexA[1])
			{
				// Two points on B and one on A.
				m_type = SeparationType.e_faceB;
				b2Vec2 localPointB1 = proxyB.GetVertex((int)cache.indexB[0]);
				b2Vec2 localPointB2 = proxyB.GetVertex((int)cache.indexB[1]);

                float b21x = localPointB2.x - localPointB1.x;
                float b21y = localPointB2.y - localPointB1.y;
                m_axis.x = -b21y;
                m_axis.y = b21x;

                // m_axis = b2Math.b2Cross(localPointB2 - localPointB1, 1.0f);
				m_axis.Normalize();
				b2Vec2 normal = b2Math.b2Mul(xfB.q, m_axis);
				
				m_localPoint = 0.5f * (localPointB1 + localPointB2);
				b2Vec2 pointB = b2Math.b2Mul(xfB, m_localPoint);
				
				b2Vec2 localPointA = proxyA.GetVertex((int)cache.indexA[0]);
				b2Vec2 pointA = b2Math.b2Mul(xfA, localPointA);
				
                b2Vec2 aminusb = pointA - pointB;
				float s = b2Math.b2Dot(ref aminusb, ref normal);
				if (s < 0.0f)
				{
					m_axis = -m_axis;
					s = -s;
				}
				return s;
			}
			else
			{
				// Two points on A and one or two points on B.
				m_type = SeparationType.e_faceA;
				b2Vec2 localPointA1 = m_proxyA.GetVertex(cache.indexA[0]);
				b2Vec2 localPointA2 = m_proxyA.GetVertex(cache.indexA[1]);
                b2Vec2 a2minusa1 = localPointA2 - localPointA1;
                m_axis = a2minusa1.UnitCross();// b2Math.b2Cross(localPointA2 - localPointA1, 1.0f);
				m_axis.Normalize();
				b2Vec2 normal = b2Math.b2Mul(xfA.q, m_axis);
				
				m_localPoint = 0.5f * (localPointA1 + localPointA2);
				b2Vec2 pointA = b2Math.b2Mul(xfA, m_localPoint);
				
				b2Vec2 localPointB = m_proxyB.GetVertex(cache.indexB[0]);
				b2Vec2 pointB = b2Math.b2Mul(xfB, localPointB);
                b2Vec2 bminusa = pointB - pointA;
				float s = b2Math.b2Dot(ref bminusa, ref normal);
				if (s < 0.0f)
				{
					m_axis = -m_axis;
					s = -s;
				}
				return s;
			}
		}
		
		public float FindMinSeparation(ref int indexA, ref int indexB, float t)
		{
			b2Transform xfA = b2Transform.Identity, xfB = b2Transform.Identity;
			m_sweepA.GetTransform(ref xfA,t);
			m_sweepB.GetTransform(ref xfB,t);
			
			switch (m_type)
			{
			case SeparationType.e_points:
			{
				b2Vec2 axisA = b2Math.b2MulT(xfA.q, m_axis);
				b2Vec2 axisB = b2Math.b2MulT(xfB.q, -m_axis);
				
				indexA = m_proxyA.GetSupport(axisA);
				indexB = m_proxyB.GetSupport(axisB);
				
				b2Vec2 localPointA = m_proxyA.GetVertex(indexA);
				b2Vec2 localPointB = m_proxyB.GetVertex(indexB);
				
				b2Vec2 pointA = b2Math.b2Mul(xfA, localPointA);
				b2Vec2 pointB = b2Math.b2Mul(xfB, localPointB);
				
				float separation = b2Math.b2Dot(pointB - pointA, m_axis);
				return separation;
			}
				
			case SeparationType.e_faceA:
			{
				b2Vec2 normal = b2Math.b2Mul(xfA.q, m_axis);
				b2Vec2 pointA = b2Math.b2Mul(xfA, m_localPoint);
				
				b2Vec2 axisB = b2Math.b2MulT(xfB.q, -normal);
				
				indexA = -1;
				indexB = m_proxyB.GetSupport(axisB);
				
				b2Vec2 localPointB = m_proxyB.GetVertex(indexB);
				b2Vec2 pointB = b2Math.b2Mul(xfB, localPointB);
				
				float separation = b2Math.b2Dot(pointB - pointA, normal);
				return separation;
			}
				
			case SeparationType.e_faceB:
			{
				b2Vec2 normal = b2Math.b2Mul(xfB.q, m_axis);
				b2Vec2 pointB = b2Math.b2Mul(xfB, m_localPoint);
				
				b2Vec2 axisA = b2Math.b2MulT(xfA.q, -normal);
				
				indexB = -1;
				indexA = m_proxyA.GetSupport(axisA);
				
				b2Vec2 localPointA = m_proxyA.GetVertex(indexA);
				b2Vec2 pointA = b2Math.b2Mul(xfA, localPointA);
				
				float separation = b2Math.b2Dot(pointA - pointB, normal);
				return separation;
			}
				
			default:
				Debug.Assert(false);
				indexA = -1;
				indexB = -1;
				return 0.0f;
			}
		}
		
		public float Evaluate(int indexA, int indexB, float t)
		{
			b2Transform xfA = b2Transform.Identity, xfB = b2Transform.Identity;
			m_sweepA.GetTransform(ref xfA, t);
			m_sweepB.GetTransform(ref xfB, t);
			
			switch (m_type)
			{
			case SeparationType.e_points:
			{
				b2Vec2 axisA = b2Math.b2MulT(xfA.q, m_axis);
				b2Vec2 axisB = b2Math.b2MulT(xfB.q, -m_axis);
				
				b2Vec2 localPointA = m_proxyA.GetVertex(indexA);
				b2Vec2 localPointB = m_proxyB.GetVertex(indexB);
				
				b2Vec2 pointA = b2Math.b2Mul(xfA, localPointA);
				b2Vec2 pointB = b2Math.b2Mul(xfB, localPointB);
				float separation = b2Math.b2Dot(pointB - pointA, m_axis);
				
				return separation;
			}
				
			case SeparationType.e_faceA:
			{
				b2Vec2 normal = b2Math.b2Mul(xfA.q, m_axis);
				b2Vec2 pointA = b2Math.b2Mul(xfA, m_localPoint);
				
				b2Vec2 axisB = b2Math.b2MulT(xfB.q, -normal);
				
				b2Vec2 localPointB = m_proxyB.GetVertex(indexB);
				b2Vec2 pointB = b2Math.b2Mul(xfB, localPointB);
				
				float separation = b2Math.b2Dot(pointB - pointA, normal);
				return separation;
			}
				
			case SeparationType.e_faceB:
			{
				b2Vec2 normal = b2Math.b2Mul(xfB.q, m_axis);
				b2Vec2 pointB = b2Math.b2Mul(xfB, m_localPoint);
				
				b2Vec2 axisA = b2Math.b2MulT(xfA.q, -normal);
				
				b2Vec2 localPointA = m_proxyA.GetVertex(indexA);
				b2Vec2 pointA = b2Math.b2Mul(xfA, localPointA);
				
				float separation = b2Math.b2Dot(pointA - pointB, normal);
				return separation;
			}
				
			default:
				Debug.Assert(false);
				return 0.0f;
			}
		}
		
		private b2DistanceProxy m_proxyA;
		private b2DistanceProxy m_proxyB;
		private b2Sweep m_sweepA, m_sweepB;
		private SeparationType m_type;
		private b2Vec2 m_localPoint;
		private b2Vec2 m_axis;
	}
	
	
	/// Input parameters for b2TimeOfImpact
	public struct b2TOIInput
	{
		public static b2TOIInput Zero = b2TOIInput.Create();

		public static b2TOIInput Create()
		{
			var toi = new b2TOIInput();
			toi.proxyA = b2DistanceProxy.Create();
            toi.proxyB = b2DistanceProxy.Create();
			toi.sweepA = new b2Sweep();
			toi.sweepB = new b2Sweep();
			return toi;
		}

		public b2DistanceProxy proxyA;
		public b2DistanceProxy proxyB;
		public b2Sweep sweepA;
		public b2Sweep sweepB;
		public float tMax;        // defines sweep interval [0, tMax]
	}
	
	public enum b2ImpactState
	{
		e_unknown,
		e_failed,
		e_overlapped,
		e_touching,
		e_separated
	};
	
	// Output parameters for b2TimeOfImpact.
	public struct b2TOIOutput
	{
		public b2ImpactState state;
		public float t;
	};
	
	
	public class b2TimeOfImpact
	{
		
		public static int b2_toiCalls, b2_toiIters, b2_toiMaxIters;
		public static int b2_toiRootIters, b2_toiMaxRootIters;
		
		
		
		// CCD via the local separating axis method. This seeks progression
		// by computing the largest time at which separation is maintained.
		public static b2TOIOutput Compute(b2TOIInput input)
		{
			b2TOIOutput output = new b2TOIOutput();
			++b2_toiCalls;
			
			output.state = b2ImpactState.e_unknown;
			output.t = input.tMax;
			
			b2DistanceProxy proxyA = input.proxyA.Copy();
			b2DistanceProxy proxyB = input.proxyB.Copy();
			
			b2Sweep sweepA = input.sweepA;
			b2Sweep sweepB = input.sweepB;
			
			// Large rotations can make the root finder fail, so we normalize the
			// sweep angles.
			sweepA.Normalize();
			sweepB.Normalize();
			
			float tMax = input.tMax;
			
			float totalRadius = proxyA.Radius + proxyB.Radius;
			float target = Math.Max(b2Settings.b2_linearSlop, totalRadius - 3.0f * b2Settings.b2_linearSlop);
			float tolerance = 0.25f * b2Settings.b2_linearSlop;
			Debug.Assert(target > tolerance);
			
			float t1 = 0.0f;
			int k_maxIterations = 20;    // TODO_ERIN b2Settings
			int iter = 0;
			
			// Prepare input for distance query.
			b2SimplexCache cache = b2SimplexCache.Create();
			b2DistanceInput distanceInput = new b2DistanceInput();
			distanceInput.proxyA = input.proxyA;
			distanceInput.proxyB = input.proxyB;
			distanceInput.useRadii = false;
			
			// The outer loop progressively attempts to compute new separating axes.
			// This loop terminates when an axis is repeated (no progress is made).
			while (true)
			{
				b2Transform xfA = b2Transform.Identity, xfB = b2Transform.Identity;
				sweepA.GetTransform(ref xfA, t1);
				sweepB.GetTransform(ref xfB, t1);
				
				// Get the distance between shapes. We can also use the results
				// to get a separating axis.
				distanceInput.transformA = xfA;
				distanceInput.transformB = xfB;
				b2DistanceOutput distanceOutput = new b2DistanceOutput();
				b2Simplex.b2Distance(ref distanceOutput, ref cache, ref distanceInput);
				
				// If the shapes are overlapped, we give up on continuous collision.
				if (distanceOutput.distance <= 0.0f)
				{
					// Failure!
					output.state = b2ImpactState.e_overlapped;
					output.t = 0.0f;
					break;
				}
				
				if (distanceOutput.distance < target + tolerance)
				{
					// Victory!
					output.state = b2ImpactState.e_touching;
					output.t = t1;
					break;
				}
				
				// Initialize the separating axis.
				b2SeparationFunction fcn = new b2SeparationFunction();
				fcn.Initialize(ref cache, ref proxyA, ref sweepA, ref proxyB, ref sweepB, t1);
				#if false
				// Dump the curve seen by the root finder
				{
					int N = 100;
					float dx = 1.0f / N;
					float xs[N+1];
					float fs[N+1];
					
					float x = 0.0f;
					
					for (int i = 0; i <= N; ++i)
					{
						sweepA.GetTransform(&xfA, x);
						sweepB.GetTransform(&xfB, x);
						float f = fcn.Evaluate(xfA, xfB) - target;
						
						printf("%g %g\n", x, f);
						
						xs[i] = x;
						fs[i] = f;
						
						x += dx;
					}
				}
				#endif
				
				// Compute the TOI on the separating axis. We do this by successively
				// resolving the deepest point. This loop is bounded by the number of vertices.
				bool done = false;
				float t2 = tMax;
				int pushBackIter = 0;
				while(true)
				{
					// Find the deepest point at t2. Store the witness point indices.
					int indexA = 0, indexB = 0;
					float s2 = fcn.FindMinSeparation(ref indexA, ref indexB, t2);
					
					// Is the final configuration separated?
					if (s2 > target + tolerance)
					{
						// Victory!
						output.state = b2ImpactState.e_separated;
						output.t = tMax;
						done = true;
						break;
					}
					
					// Has the separation reached tolerance?
					if (s2 > target - tolerance)
					{
						// Advance the sweeps
						t1 = t2;
						break;
					}
					
					// Compute the initial separation of the witness points.
					float s1 = fcn.Evaluate(indexA, indexB, t1);
					
					// Check for initial overlap. This might happen if the root finder
					// runs out of iterations.
					if (s1 < target - tolerance)
					{
						output.state = b2ImpactState.e_failed;
						output.t = t1;
						done = true;
						break;
					}
					
					// Check for touching
					if (s1 <= target + tolerance)
					{
						// Victory! t1 should hold the TOI (could be 0.0).
						output.state = b2ImpactState.e_touching;
						output.t = t1;
						done = true;
						break;
					}
					
					// Compute 1D root of: f(x) - target = 0
					int rootIterCount = 0;
					float a1 = t1, a2 = t2;
					while (true)
					{
						// Use a mix of the secant rule and bisection.
						float t;
						if (rootIterCount % 1 == 1) // even/odd
						{
							// Secant rule to improve convergence.
							t = a1 + (target - s1) * (a2 - a1) / (s2 - s1);
						}
						else
						{
							// Bisection to guarantee progress.
							t = 0.5f * (a1 + a2);
						}
						
						float s = fcn.Evaluate(indexA, indexB, t);
						
						if (b2Math.b2Abs(s - target) < tolerance)
						{
							// t2 holds a tentative value for t1
							t2 = t;
							break;
						}
						
						// Ensure we continue to bracket the root.
						if (s > target)
						{
							a1 = t;
							s1 = s;
						}
						else
						{
							a2 = t;
							s2 = s;
						}
						
						++rootIterCount;
						++b2_toiRootIters;
						
						if (rootIterCount == 50)
						{
							break;
						}
					}
					
					b2_toiMaxRootIters = Math.Max(b2_toiMaxRootIters, rootIterCount);
					
					++pushBackIter;
					
					if (pushBackIter == b2Settings.b2_maxPolygonVertices)
					{
						break;
					}
				}
				
				++iter;
				++b2_toiIters;
				
				if (done)
				{
					break;
				}
				
				if (iter == k_maxIterations)
				{
					// Root finder got stuck. Semi-victory.
					output.state = b2ImpactState.e_failed;
					output.t = t1;
					break;
				}
			}
			
			b2_toiMaxIters = Math.Max(b2_toiMaxIters, iter);
			return (output);
		}
	}
}
