using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CocosSharp
{
    public partial class CCDrawingPrimitives
    {
		static CCPrimitiveBatch batch;
		public static float PointSize { get; set; }
		public static CCColor4B DrawColor { get; set; }
		public static float LineWidth { get; set; }

        internal static void Initialize(GraphicsDevice graphics)
        {
            batch = new CCPrimitiveBatch(graphics);
			DrawColor = CCColor4B.White;
			LineWidth = 1;
			PointSize = 1.0f;
        }

        public static void Begin()
        {
            batch.Begin();

        }

        public static void End()
        {
            batch.End();

			// Restore original values
			DrawColor = CCColor4B.White;
			LineWidth = 1;
			PointSize = 1.0f;
        }


        public static void DrawPoint(CCPoint point)
        {
            DrawPoint(point, PointSize, DrawColor);
        }

        public static void DrawPoint(CCPoint point, float size)
        {
            DrawPoint(point, size, DrawColor);
        }

        public static void DrawPoint(CCPoint p, float size, CCColor4B color)
        {
            var verts = new CCPoint[4];

            float hs = size / 2.0f;

            verts[0] = p + new CCPoint(-hs, -hs);
            verts[1] = p + new CCPoint(hs, -hs);
            verts[2] = p + new CCPoint(hs, hs);
            verts[3] = p + new CCPoint(-hs, hs);

            DrawPoly(verts, 4, false, true, color);
        }

        public static void DrawPoints(CCPoint[] points, float size, CCColor4B color)
        {
            DrawPoints(points, points.Length, size, color);
        }

		public static void DrawPoints(CCPoint[] points, float size)
		{
			DrawPoints(points, points.Length, size, DrawColor);
		}

		public static void DrawPoints(CCPoint[] points)
		{
			DrawPoints(points, points.Length, PointSize, DrawColor);
		}

		public static void DrawPoints(CCPoint[] points, int numberOfPoints, float size, CCColor4B color)
        {
            for (int i = 0; i < numberOfPoints; i++)
            {
                DrawPoint(points[i], size, color);
            }
        }


        public static void DrawLine(CCPoint origin, CCPoint destination, CCColor4B color)
        {

			var a = origin;
			var b = destination;

			var n = CCPoint.Normalize(CCPoint.Perp(a - b));

			var lww = LineWidth * 0.5f;
			var nw = n * lww;
			var v0 = b - nw;
			var v1 = b + nw;
			var v2 = a - nw;
			var v3 = a + nw;

			// Triangles from beginning to end
			batch.AddVertex(v1, color, PrimitiveType.TriangleList);
			batch.AddVertex(v2, color, PrimitiveType.TriangleList);
			batch.AddVertex(v0, color, PrimitiveType.TriangleList);

			batch.AddVertex(v1, color, PrimitiveType.TriangleList);
			batch.AddVertex(v2, color, PrimitiveType.TriangleList);
			batch.AddVertex(v3, color, PrimitiveType.TriangleList);

        }

		public static void DrawLine(CCPoint origin, CCPoint destination)
		{

			DrawLine(origin, destination, DrawColor);
		}

		public static void DrawRect(CCRect rect, CCColor4B color)
        {
            float x1 = rect.MinX;
            float y1 = rect.MinY;
            float x2 = rect.MaxX;
            float y2 = rect.MaxY;

            DrawLine(new CCPoint(x1, y1), new CCPoint(x2, y1), color);
            DrawLine(new CCPoint(x2, y1), new CCPoint(x2, y2), color);
            DrawLine(new CCPoint(x2, y2), new CCPoint(x1, y2), color);
            DrawLine(new CCPoint(x1, y2), new CCPoint(x1, y1), color);
        }

		/// <summary>
		/// draws a poligon given a pointer to CCPoint coordiantes and the number of vertices measured in points.
		/// The polygon can be closed or open
		/// </summary>
		public static void DrawPoly(CCPoint[] vertices, int numOfVertices, bool closePolygon)
		{
			DrawPoly(vertices, numOfVertices, closePolygon, false, DrawColor);
		}

		/// <summary>
		/// draws a poligon given a pointer to CCPoint coordiantes and the number of vertices measured in points.
		/// The polygon can be closed or open
		/// </summary>
		public static void DrawPoly(CCPoint[] vertices, CCColor4B color, bool closePolygon = false)
		{
			DrawPoly(vertices, vertices.Length, closePolygon, false, color);
		}

		/// <summary>
		/// draws a poligon given a pointer to CCPoint coordiantes and the number of vertices measured in points.
		/// The polygon can be closed or open
		/// </summary>
		public static void DrawPoly(CCPoint[] vertices, bool closePolygon = false)
		{
			DrawPoly(vertices, vertices.Length, closePolygon, false, DrawColor);
		}

        /// <summary>
        /// draws a poligon given a pointer to CCPoint coordiantes and the number of vertices measured in points.
        /// The polygon can be closed or open
        /// </summary>
        public static void DrawPoly(CCPoint[] vertices, int numOfVertices, bool closePolygon, CCColor4B color)
        {
            DrawPoly(vertices, numOfVertices, closePolygon, false, color);
        }


        /// <summary>
        /// draws a polygon given a pointer to CCPoint coordiantes and the number of vertices measured in points.
        /// The polygon can be closed or open and optionally filled with current GL color
        /// </summary>
        public static void DrawPoly(CCPoint[] vertices, int numOfVertices, bool closePolygon, bool fill, CCColor4B color)
        {

            if (fill)
            {
                for (int i = 1; i < numOfVertices - 1; i++)
                {
					batch.AddVertex(new CCVector2(vertices[0].X, vertices[0].Y), color, PrimitiveType.TriangleList);
					batch.AddVertex(new CCVector2(vertices[i].X, vertices[i].Y), color, PrimitiveType.TriangleList);
					batch.AddVertex(new CCVector2(vertices[i + 1].X, vertices[i + 1].Y), color, PrimitiveType.TriangleList);
                }
            }
            else
            {
                for (int i = 0; i < numOfVertices - 1; i++)
                {
					DrawLine(vertices[i], vertices[i + 1]);
                }

                if (closePolygon)
                {
					DrawLine(vertices[numOfVertices - 1], vertices[0]);
                }
            }
        }

		public static void DrawSolidPoly(CCPoint[] vertices, CCColor4B color)
		{
			DrawSolidPoly(vertices, vertices.Length, color, false);
		}

        public static void DrawSolidPoly(CCPoint[] vertices, int count, CCColor4B color)
        {
            DrawSolidPoly(vertices, count, color, false);
        }

        public static void DrawSolidPoly(CCPoint[] vertices, int count, CCColor4B color, bool outline)
        {
            if (count == 2)
            {
                DrawPoly(vertices, count, false, color);
                return;
            }

			var colorFill = color * (outline ? 0.5f : 1.0f);

            for (int i = 1; i < count - 1; i++)
            {
                batch.AddVertex(new CCVector2(vertices[0].X, vertices[0].Y), colorFill, PrimitiveType.TriangleList);
                batch.AddVertex(new CCVector2(vertices[i].X, vertices[i].Y), colorFill, PrimitiveType.TriangleList);
                batch.AddVertex(new CCVector2(vertices[i + 1].X, vertices[i + 1].Y), colorFill, PrimitiveType.TriangleList);
            }

            if (outline)
            {
                DrawPoly(vertices, count, true, color);
            }
        }

        public static void DrawSolidRect(CCPoint origin, CCPoint destination, CCColor4B color)
        {
            CCPoint[] vertices =
                {
                    origin,
                    new CCPoint(destination.X, origin.Y),
                    destination,
                    new CCPoint(origin.X, destination.Y),
                };

            DrawSolidPoly(vertices, 4, color);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="center"></param>
        /// <param name="radius"></param>
        /// <param name="angle">The amount of the circle to draw, in radiians</param>
        /// <param name="segments"></param>
        /// <param name="drawLineToCenter"></param>
        /// <param name="color"></param>
		public static void DrawCircle(CCPoint center, float radius, float angle, int segments, bool drawLineToCenter, CCColor4B color, float scaleX = 1.0f, float scaleY = 1.0f)
        {
            float increment = MathHelper.Pi * 2.0f / segments;
            double theta = angle;

			var vertices = new CCPoint[segments * 2];

			for (int i = 0, s=0; i < segments; i++, s += 2)
			{
				vertices[s] = center + new CCPoint((float) Math.Cos(theta) * scaleX, (float) Math.Sin(theta) * scaleY) * radius;
				vertices[s + 1] = center + new CCPoint((float) Math.Cos(theta + increment) * scaleX, (float) Math.Sin(theta + increment) * scaleY) * radius;

				theta += increment;
			}

			DrawPoly(vertices, color);

            if (drawLineToCenter)
            {
                DrawLine(center, vertices[vertices.Length-1], color);
            }
        }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="center"></param>
		/// <param name="radius"></param>
		/// <param name="angle">The amount of the circle to draw, in radiians</param>
		/// <param name="segments"></param>
		/// <param name="drawLineToCenter"></param>
		/// <param name="color"></param>
		public static void DrawCircle(CCPoint center, float radius, float angle, int segments, bool drawLineToCenter, float scaleX = 1.0f, float scaleY = 1.0f)
		{
			DrawCircle(center, radius, angle, segments, drawLineToCenter, DrawColor, scaleX, scaleY);
		}

		public static void DrawSolidCircle(CCPoint center, float radius, float angle, int segments, float scaleX = 1.0f, float scaleY = 1.0f)
		{
			DrawSolidCircle(center, radius, angle, segments, DrawColor, scaleX, scaleY);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="center"></param>
		/// <param name="radius"></param>
		/// <param name="angle">The amount of the circle to draw, in radiians</param>
		/// <param name="segments"></param>
		/// <param name="color"></param>
		public static void DrawSolidCircle(CCPoint center, float radius, float angle, int segments, CCColor4B color, float scaleX = 1.0f, float scaleY = 1.0f)
		{
			float increment = MathHelper.Pi * 2.0f / segments;
			double theta = angle;

			var vertices = new CCPoint[segments * 2];

			for (int i = 0, s=0; i < segments; i++, s += 2)
			{
				vertices[s] = center + new CCPoint((float) Math.Cos(theta) * scaleX, (float) Math.Sin(theta) * scaleY) * radius;
				vertices[s + 1] = center + new CCPoint((float) Math.Cos(theta + increment) * scaleX, (float) Math.Sin(theta + increment) * scaleY) * radius;

				theta += increment;
			}

			DrawSolidPoly(vertices, color);
		}


		public static void DrawArc (CCRect rect, int startAngle, int sweepAngle)
		{
			DrawEllipticalArc(rect, startAngle, sweepAngle, false, DrawColor);
		}

        public static void DrawArc (CCRect rect, int startAngle, int sweepAngle, CCColor4B color)
        {
            DrawEllipticalArc(rect, startAngle, sweepAngle, false, color);
        }

        public static void DrawArc (int x, int y, int width, int height, int startAngle, int sweepAngle)
        {
            DrawEllipticalArc(x,y,width,height,startAngle,sweepAngle,false, DrawColor);

        }

		public static void DrawArc (int x, int y, int width, int height, int startAngle, int sweepAngle, CCColor4B color)
		{
			DrawEllipticalArc(x,y,width,height,startAngle,sweepAngle,false, color);

		}

		public static void DrawEllipse (CCRect rect)
        {
            DrawEllipticalArc(rect, 0, 360, false, DrawColor);
        }
        
		public static void DrawEllipse (CCRect rect, CCColor4B color)
		{
			DrawEllipticalArc(rect, 0, 360, false, color);
		}

		public static void DrawEllips (int x, int y, int width, int height)
		{
			DrawEllipticalArc(x,y,width,height,0,360,false, DrawColor);

		}

		public static void DrawEllips (int x, int y, int width, int height, CCColor4B color)
        {
            DrawEllipticalArc(x,y,width,height,0,360,false, color);
            
        }

		public static void DrawPie (CCRect rect, int startAngle, int sweepAngle)
		{
			DrawEllipticalArc(rect, startAngle, sweepAngle, true, DrawColor);
		}

		public static void DrawPie (CCRect rect, int startAngle, int sweepAngle, CCColor4B color)
        {
            DrawEllipticalArc(rect, startAngle, sweepAngle, true, color);
        }
        
        public static void DrawPie (int x, int y, int width, int height, int startAngle, int sweepAngle)
        {
            DrawEllipticalArc(x,y,width,height,startAngle,sweepAngle,true, DrawColor);
            
        }

		public static void DrawPie (int x, int y, int width, int height, int startAngle, int sweepAngle, CCColor4B color)
		{
			DrawEllipticalArc(x,y,width,height,startAngle,sweepAngle,true, color);

		}

		public static void DrawQuadBezier(CCPoint origin, CCPoint control, CCPoint destination, int segments)
		{
			DrawQuadBezier(origin, control, destination, segments, DrawColor);
		}

        public static void DrawQuadBezier(CCPoint origin, CCPoint control, CCPoint destination, int segments, CCColor4B color)
        {
            var vertices = new VertexPositionColor[segments + 1];

            float t = 0.0f;
            for (int i = 0; i < segments; i++)
            {
                float x = CCSplineMath.QuadBezier(origin.X, control.X, destination.X, t);
                float y = CCSplineMath.QuadBezier(origin.Y, control.Y, destination.Y, t);

                vertices[i].Position = new Vector3(x, y, 0);
                vertices[i].Color = new Color(color.R, color.G, color.B, color.A);

                t += 1.0f / segments;
            }
            
            vertices[segments] = new VertexPositionColor
                {
                    Position = new Vector3(destination.X, destination.Y, 0),
                    Color = new Color(color.R, color.G, color.B, color.A),
                };

            BasicEffect basicEffect = CCDrawManager.PrimitiveEffect;
            basicEffect.Projection = CCDrawManager.ProjectionMatrix;
            basicEffect.View = CCDrawManager.ViewMatrix;
            basicEffect.World = CCDrawManager.WorldMatrix;

            foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                basicEffect.GraphicsDevice.DrawUserPrimitives(PrimitiveType.LineStrip, vertices, 0, segments);
            }
        }

		public static void DrawCubicBezier(CCPoint origin, CCPoint control1, CCPoint control2, CCPoint destination, int segments)
		{
			DrawCubicBezier(origin, control1, control2, destination, segments, DrawColor);
		}

        /// <summary>
        /// draws a cubic bezier path
        /// @since v0.8
        /// </summary>
        public static void DrawCubicBezier(CCPoint origin, CCPoint control1, CCPoint control2, CCPoint destination, int segments, CCColor4B color)
        {
            var vertices = new VertexPositionColor[segments + 1];

            float t = 0;
            for (int i = 0; i < segments; ++i)
            {
                float x = CCSplineMath.CubicBezier(origin.X, control1.X, control2.X, destination.X, t);
                float y = CCSplineMath.CubicBezier(origin.Y, control1.Y, control2.Y, destination.Y, t);

                vertices[i] = new VertexPositionColor();
                vertices[i].Position = new Vector3(x, y, 0);
                vertices[i].Color = new Color(color.R, color.G, color.B, color.A);
                t += 1.0f / segments;
            }
            vertices[segments] = new VertexPositionColor
                {
                    Color = new Color(color.R, color.G, color.B, color.A),
                    Position = new Vector3(destination.X, destination.Y, 0)
                };

            BasicEffect basicEffect = CCDrawManager.PrimitiveEffect;
            basicEffect.Projection = CCDrawManager.ProjectionMatrix;
            basicEffect.View = CCDrawManager.ViewMatrix;
            basicEffect.World = CCDrawManager.WorldMatrix;

            foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes)
            {
                pass.Apply();

                basicEffect.GraphicsDevice.DrawUserPrimitives(PrimitiveType.LineStrip, vertices, 0, segments);
            }
        }

        public static void DrawCatmullRom(List<CCPoint> points, int segments)
        {
            DrawCardinalSpline(points, 0.5f, segments);
        }

        public static void DrawCardinalSpline(List<CCPoint> config, float tension, int segments)
        {
            var vertices = new VertexPositionColor[segments + 1];

            int p;
            float lt;
            float deltaT = 1.0f / config.Count;

            for (int i = 0; i < segments + 1; i++)
            {
                float dt = (float) i / segments;

                // border
                if (dt == 1)
                {
                    p = config.Count - 1;
                    lt = 1;
                }
                else
                {
                    p = (int) (dt / deltaT);
                    lt = (dt - deltaT * p) / deltaT;
                }

                // Interpolate
                // Interpolate    
                int c = config.Count - 1;
                CCPoint pp0 = config[Math.Min(c, Math.Max(p - 1, 0))];
                CCPoint pp1 = config[Math.Min(c, Math.Max(p + 0, 0))];
                CCPoint pp2 = config[Math.Min(c, Math.Max(p + 1, 0))];
                CCPoint pp3 = config[Math.Min(c, Math.Max(p + 2, 0))];

                CCPoint newPos = CCSplineMath.CCCardinalSplineAt(pp0, pp1, pp2, pp3, tension, lt);

                vertices[i].Position.X = newPos.X;
                vertices[i].Position.Y = newPos.Y;
                vertices[i].Color = Color.White;
            }

            BasicEffect basicEffect = CCDrawManager.PrimitiveEffect;
            basicEffect.Projection = CCDrawManager.ProjectionMatrix;
            basicEffect.View = CCDrawManager.ViewMatrix;
            basicEffect.World = CCDrawManager.WorldMatrix;
            foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                basicEffect.GraphicsDevice.DrawUserPrimitives(PrimitiveType.LineStrip, vertices, 0, segments);
            }
        }

    }

}