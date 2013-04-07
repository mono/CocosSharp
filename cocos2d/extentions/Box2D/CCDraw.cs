/* Copyright (c) 2012 Scott Lembcke and Howling Moon Software
 * Copyright (c) 2012 cocos2d-x.org
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 */


// implementation of CCDrawNode
using Box2D.Common;
using System;

namespace cocos2d
{
	public class CCDraw : b2Draw
	{
		public CCDraw() 
		{
		}

        public override void DrawPolygon(b2Vec2[] vertices, int vertexCount, b2Color color)
        {
            CCDrawingPrimitives.DrawPoly(vertices, vertexCount, true, color);
        }

        public override void DrawSolidPolygon(b2Vec2[] vertices, int vertexCount, b2Color color)
        {
            CCDrawingPrimitives.DrawSolidPoly(vertices, vertexCount, color);
        }

        public override void DrawCircle(b2Vec2 center, float radius, b2Color color)
        {
            CCDrawingPrimitives.DrawCircle(center, radius, color);
        }

        public override void DrawSolidCircle(b2Vec2 center, float radius, b2Vec2 axis, b2Color color)
        {
            throw new NotImplementedException();
        }

        public override void DrawSegment(b2Vec2 p1, b2Vec2 p2, b2Color color)
        {
            throw new NotImplementedException();
        }

        public override void DrawTransform(b2Transform xf)
        {
            throw new NotImplementedException();
        }
    }
}
