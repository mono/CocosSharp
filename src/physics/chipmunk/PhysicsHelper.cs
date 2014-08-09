/****************************************************************************
 Copyright (c) 2013 Chukong Technologies Inc. ported by Jose Medrano (@netonjm)
 
 http://www.cocos2d-x.org
 
 Permission is hereby granted, free of charge, to any person obtaining a copy
 of this software and associated documentation files (the "Software"), to deal
 in the Software without restriction, including without limitation the rights
 to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 copies of the Software, and to permit persons to whom the Software is
 furnished to do so, subject to the following conditions:
 
 The above copyright notice and this permission notice shall be included in
 all copies or substantial portions of the Software.
 
 THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 THE SOFTWARE.
 ****************************************************************************/

using ChipmunkSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CocosSharp
{
	public class PhysicsHelper
	{

		//TODO: This conversion methods don't be necesary, Cocosharp works with cpVect
		//Because there are 2 physics engine Vec2 is a intermedial class to use cpVect and b2Vect or any other engine

		//public static cpVect cpv2point(cpVect vec) { return new cpVect(vec.x, vec.y); }
		// public static cpVect point2cpv(cpVect point) { return new cpVect(point.x, point.y); }
		public static CCSize cpv2size(cpVect vec) { return new CCSize((float)vec.x, (float)vec.y); }
		public static cpVect size2cpv(CCSize size) { return new cpVect(size.Width, size.Height); }
		//public static float cpfloat2float(float f) { return f; }
		//public static float float2cpfloat(float f) { return f; }
		public static cpBB rect2cpbb(CCRect rect) { return new cpBB(rect.Origin.X, rect.Origin.Y, rect.Origin.X + rect.Size.Width, rect.Origin.Y + rect.Size.Height); }
		public static CCRect cpbb2rect(cpBB bb) { return new CCRect((float)bb.l, (float)bb.b, (float)(bb.r - bb.l), (float)(bb.t - bb.b)); }

		//public static List<cpVect> cpvs2points(List<cpVect> cpvs, List<cpVect> output, int count)
		//{
		//    for (int i = 0; i < count; ++i)
		//    {
		//        output[i] = cpv2point(cpvs[i]);
		//    }

		//    return output;
		//}

		//public static List<cpVect> points2cpvs(List<cpVect> points, List<cpVect> output, int count)
		//{
		//    for (int i = 0; i < count; ++i)
		//    {
		//        output[i] = point2cpv(points[i]);
		//    }

		//    return output;
		//}

		/*
				internal static float float2cpfloat(float p)
				{
					throw new NotImplementedException();
				}

				internal static object points2cpvs(Chipmunk.cpVect[] points, Chipmunk.cpVect[] cpvs, int count)
				{
					throw new NotImplementedException();
				}

				internal static void cpvs2points(Chipmunk.cpVect[] cpvs, Chipmunk.cpVect[] points, int count)
				{
					throw new NotImplementedException();
				}

				internal static Chipmunk.cpVect cpv2point(Chipmunk.cpVect center)
				{
					throw new NotImplementedException();
				}

				internal static Chipmunk.cpVect point2cpv(Chipmunk.cpVect offset)
				{
					throw new NotImplementedException();
				}

				internal static float cpfloat2float(float p)
				{
					throw new NotImplementedException();
				}

				internal static Chipmunk.cpVect size2cpv(CCSize size)
				{
					throw new NotImplementedException();
				}

				internal static void cpvs2points(List<Chipmunk.cpVect> list, Chipmunk.cpVect points, int p)
				{
					throw new NotImplementedException();
				}

				internal static CCSize cpv2size(Chipmunk.cpVect cpVect)
				{
					throw new NotImplementedException();
				}
		 * */
	}
}