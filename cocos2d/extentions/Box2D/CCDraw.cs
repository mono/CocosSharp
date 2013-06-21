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
using System.Runtime;

namespace Cocos2D
{
	public class CCDraw : b2Draw
	{
        private b2Vec2 _Center = new b2Vec2(0f, 0f);

		public CCDraw(int ptm) 
            : base(ptm)
		{
		}

        public CCDraw(b2Vec2 drawCenter)
        {
            _Center = drawCenter;
        }

        public CCDraw(b2Vec2 drawCenter, int ptm)
            : base(ptm)
        {
            _Center = drawCenter;
        }

        public override void DrawPolygon(b2Vec2[] vertices, int vertexCount, b2Color color)
        {
            b2Vec2[] alt = new b2Vec2[vertexCount];
            for (int i = 0; i < vertexCount; i++)
            {
                alt[i] = vertices[i] * PTMRatio + _Center;
            }
            CCDrawingPrimitives.DrawPoly(alt, vertexCount, true, color);
        }

        public override void DrawSolidPolygon(b2Vec2[] vertices, int vertexCount, b2Color color)
        {
            b2Vec2[] alt = new b2Vec2[vertexCount];
            for (int i = 0; i < vertexCount; i++)
            {
                alt[i] = vertices[i] * PTMRatio + _Center;
            }
            CCDrawingPrimitives.DrawSolidPoly(alt, vertexCount, color);
        }

        public override void DrawCircle(b2Vec2 center, float radius, b2Color color)
        {
            CCDrawingPrimitives.DrawCircle(center * PTMRatio + _Center, radius * PTMRatio, color);
        }

        public override void DrawSolidCircle(b2Vec2 center, float radius, b2Vec2 axis, b2Color color)
        {
            DrawCircle(center, radius, color); 
        }

        public override void DrawSegment(b2Vec2 p1, b2Vec2 p2, b2Color color)
        {
            CCDrawingPrimitives.DrawLine(p1 * PTMRatio + _Center, p2 * PTMRatio + _Center, color);
        }

        public override void DrawTransform(b2Transform xf)
        {
            float axisScale = 0.4f;
            b2Vec2 p1 = xf.p;

            b2Vec2 col1 = new b2Vec2(xf.q.c, xf.q.s);
            b2Vec2 col2 = new b2Vec2(-xf.q.s, xf.q.c);

            b2Vec2 p2 = p1 + axisScale * col1;
            DrawSegment(p1, p2, new b2Color(1f, 0f, 0f));

            p2 = p1 + axisScale * col2; 
            DrawSegment(p1, p2, new b2Color(0f, 0f, 1f));
        }
    }
}
