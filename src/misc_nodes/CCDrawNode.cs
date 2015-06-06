using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CocosSharp
{
    public class CCDrawNode : CCNode
    {

        const int DefaultBufferSize = 512;
        CCRawList<CCV3F_C4B> triangleVertices;
        CCRawList<CCV3F_C4B> lineVertices;
        SpriteFont spriteFont;
        List<StringData> stringData;
        StringBuilder stringBuilder;
        string spriteFontName;

        bool dirty;

        CCCustomCommand renderTriangles;
        CCCustomCommand renderLines;
        CCCustomCommand renderStrings;


        struct ExtrudeVerts
        {
            public CCPoint offset;
            public CCPoint n;
        }

        public CCBlendFunc BlendFunc { get; set; }

        #region Structs

        struct StringData
        {
            public object[] Args;
            public Color Color;
            public string S;
            public int X, Y;

            public StringData(int x, int y, string s, object[] args, Color color)
            {
                X = x;
                Y = y;
                S = s;
                Args = args;
                Color = color;
            }
        }

        #endregion Structs

        #region Constructors

        public CCDrawNode()
        {
            renderTriangles = new CCCustomCommand(FlushTriangles);
            renderLines = new CCCustomCommand(FlushLines);
            renderStrings = new CCCustomCommand(DrawStrings);

            BlendFunc = CCBlendFunc.AlphaBlend;
            triangleVertices = new CCRawList<CCV3F_C4B>(DefaultBufferSize);
            lineVertices = new CCRawList<CCV3F_C4B>(DefaultBufferSize);
        }

        #endregion Constructors

        public void SelectFont (string fontName, float fontSize)
        {
            if (spriteFontName != fontName)
            {
                spriteFontName = fontName;
                float loadedSize;

                spriteFont = CCSpriteFontCache.SharedInstance.TryLoadFont(fontName, fontSize, out loadedSize);

                if (spriteFont == null)
                {
                    CCLog.Log("Failed to load default font. No font supported.");
                    return;
                }

                stringData = new List<StringData>();
                stringBuilder = new StringBuilder();
            }
        }

        public void DrawString(int x, int y, string format, params object[] objects)
        {
            System.Diagnostics.Debug.Assert(spriteFont != null, "CocosSharp: A font must be set before");

            if (spriteFont != null)
            {
                var color = new CCColor4B(Color.R, Color.G, Color.B, Opacity);
                stringData.Add(new StringData(x, y, format, objects, color));
            }
        }

        public override CCSize ContentSize
        {
            get
            {
                UpdateContextSize();
                return base.ContentSize;
            }
            set
            {
                base.ContentSize = value;
            }
        }


        // TODO: Look into adding dynamic update of ContentSize on each Vertex addition.
        // Calculating on each Vertex addtion may be faster than this.
        private void UpdateContextSize()
        {
            var numTVerts = triangleVertices.Count;
            var numLVerts = lineVertices.Count;

            if (numTVerts == 0 && numLVerts == 0)
                return;

            var size = base.ContentSize;
            var minX = float.MaxValue;
            var minY = float.MaxValue;
            var maxX = float.MinValue;
            var maxY = float.MinValue;

            CCV3F_C4B vert;

            var x = 0.0f;
            var y = 0.0f;

            for (int v = 0; v < numTVerts; v++)
            {
                vert = triangleVertices[v];
                x = vert.Vertices.X;
                y = vert.Vertices.Y;
                minX = Math.Min (x, minX);
                minY = Math.Min (y, minY);
                maxX = Math.Max(x, maxX);
                maxY = Math.Max(x, maxY);
            }

            for (int v = 0; v < numLVerts; v++)
            {
                vert = lineVertices[v];
                x = vert.Vertices.X;
                y = vert.Vertices.Y;
                minX = Math.Min (x, minX);
                minY = Math.Min (y, minY);
                maxX = Math.Max(x, maxX);
                maxY = Math.Max(x, maxY);
            }

            base.ContentSize = new CCSize(maxX - minX, maxY - minY);
        }

        public void AddTriangleVertex (CCV3F_C4B triangleVertex)
        {
            triangleVertices.Add (triangleVertex);
        }

        public void AddLineVertex (CCV3F_C4B lineVertex)
        {
            lineVertices.Add(lineVertex);
        }

		// See http://slabode.exofire.net/circle_draw.shtml
		// An Efficient Way to Draw Approximate Circles in OpenGL
		// Try to keep from calculating Cos and Sin of values everytime and just use
		// add and subtract where possible to calculate the values.
		public void DrawDot(CCPoint pos, float radius, CCColor4B color)
		{
			var cl = color;

			var segments = 10 * (float)Math.Sqrt(radius);  //<- Let's try to guess at # segments for a reasonable smoothness

			float theta = MathHelper.Pi * 2.0f / segments; 
			float tangetial_factor = (float)Math.Tan(theta);   //calculate the tangential factor 

			float radial_factor = (float)Math.Cos(theta);   //calculate the radial factor 

			float x = radius;  //we start at angle = 0 
			float y = 0; 

            var verticeCenter = new CCV3F_C4B(pos, cl);
            var vert1 = new CCV3F_C4B(CCVertex3F.Zero, cl);
			float tx = 0; 
			float ty = 0; 

			for (int i = 0; i < segments; i++)
			{
			
				vert1.Vertices.X = x + pos.X;
				vert1.Vertices.Y = y + pos.Y;
				AddTriangleVertex(vert1); // output vertex

				//calculate the tangential vector 
				//remember, the radial vector is (x, y) 
				//to get the tangential vector we flip those coordinates and negate one of them 
				tx = -y; 
				ty = x; 

				//add the tangential vector 
				x += tx * tangetial_factor; 
				y += ty * tangetial_factor; 

				//correct using the radial factor 
				x *= radial_factor; 
				y *= radial_factor; 

				vert1.Vertices.X = x + pos.X;
				vert1.Vertices.Y = y + pos.Y;
				AddTriangleVertex(vert1); // output vertex

				AddTriangleVertex(verticeCenter);
			} 

			dirty = true;
		}

        // See http://slabode.exofire.net/circle_draw.shtml
        // An Efficient Way to Draw Approximate Circles in OpenGL
        // Try to keep from calculating Cos and Sin of values everytime and just use
        // add and subtract where possible to calculate the values.
        public void DrawCircle(CCPoint pos, float radius, CCColor4B color)
        {
            var cl = color;

            int segments = (int)(10 * (float)Math.Sqrt(radius));  //<- Let's try to guess at # segments for a reasonable smoothness

            float theta = MathHelper.Pi * 2.0f / segments;
            float tangetial_factor = (float)Math.Tan(theta);   //calculate the tangential factor 

            float radial_factor = (float)Math.Cos(theta);   //calculate the radial factor 

            float x = radius;  //we start at angle = 0 
            float y = 0;

            var verticeCenter = new CCV3F_C4B(pos, cl);
            var vert1 = new CCV3F_C4B(CCVertex3F.Zero, cl);
            float tx = 0;
            float ty = 0;

            for (int i = 0; i < segments; i++)
            {

                vert1.Vertices.X = x + pos.X;
                vert1.Vertices.Y = y + pos.Y;
                AddLineVertex(vert1); // output vertex

                //calculate the tangential vector 
                //remember, the radial vector is (x, y) 
                //to get the tangential vector we flip those coordinates and negate one of them 
                tx = -y;
                ty = x;

                //add the tangential vector 
                x += tx * tangetial_factor;
                y += ty * tangetial_factor;

                //correct using the radial factor 
                x *= radial_factor;
                y *= radial_factor;

                vert1.Vertices.X = x + pos.X;
                vert1.Vertices.Y = y + pos.Y;
                AddLineVertex(vert1); // output vertex

            }

            dirty = true;
        }

        // See http://slabode.exofire.net/circle_draw.shtml
        // An Efficient Way to Draw Approximate Circles in OpenGL
        // Try to keep from calculating Cos and Sin of values everytime and just use
        // add and subtract where possible to calculate the values.
        public void DrawSolidCircle(CCPoint pos, float radius, CCColor4B color)
        {
            var cl = color;

            int segments = (int)(10 * (float)Math.Sqrt(radius));  //<- Let's try to guess at # segments for a reasonable smoothness

            float theta = MathHelper.Pi * 2.0f / segments;
            float tangetial_factor = (float)Math.Tan(theta);   //calculate the tangential factor 

            float radial_factor = (float)Math.Cos(theta);   //calculate the radial factor 

            float x = radius;  //we start at angle = 0 
            float y = 0;

            var verticeCenter = new CCV3F_C4B(pos, cl);
            var vert1 = new CCV3F_C4B(CCVertex3F.Zero, cl);
            float tx = 0;
            float ty = 0;

            for (int i = 0; i < segments; i++)
            {
                AddTriangleVertex(verticeCenter);

                vert1.Vertices.X = x + pos.X;
                vert1.Vertices.Y = y + pos.Y;
                AddTriangleVertex(vert1); // output vertex

                //calculate the tangential vector 
                //remember, the radial vector is (x, y) 
                //to get the tangential vector we flip those coordinates and negate one of them 
                tx = -y;
                ty = x;

                //add the tangential vector 
                x += tx * tangetial_factor;
                y += ty * tangetial_factor;

                //correct using the radial factor 
                x *= radial_factor;
                y *= radial_factor;

                vert1.Vertices.X = x + pos.X;
                vert1.Vertices.Y = y + pos.Y;
                AddTriangleVertex(vert1); // output vertex
            }

            dirty = true;
        }


        // Used for drawing line caps
        public void DrawSolidArc(CCPoint pos, float radius, float startAngle, float sweepAngle, CCColor4B color)
        {
            var cl = color;

            int segments = (int)(10 * (float)Math.Sqrt(radius));  //<- Let's try to guess at # segments for a reasonable smoothness

            float theta = sweepAngle / (segments - 1);// MathHelper.Pi * 2.0f / segments;
            float tangetial_factor = (float)Math.Tan(theta);   //calculate the tangential factor 

            float radial_factor = (float)Math.Cos(theta);   //calculate the radial factor 

            float x = radius * (float)Math.Cos(startAngle);   //we now start at the start angle
            float y = radius * (float)Math.Sin(startAngle); 

            var verticeCenter = new CCV3F_C4B(pos, cl);
            var vert1 = new CCV3F_C4B(CCVertex3F.Zero, cl);
            float tx = 0;
            float ty = 0;

            for (int i = 0; i < segments - 1; i++)
            {
                AddTriangleVertex(verticeCenter);

                vert1.Vertices.X = x + pos.X;
                vert1.Vertices.Y = y + pos.Y;
                AddTriangleVertex(vert1); // output vertex

                //calculate the tangential vector 
                //remember, the radial vector is (x, y) 
                //to get the tangential vector we flip those coordinates and negate one of them 
                tx = -y;
                ty = x;

                //add the tangential vector 
                x += tx * tangetial_factor;
                y += ty * tangetial_factor;

                //correct using the radial factor 
                x *= radial_factor;
                y *= radial_factor;

                vert1.Vertices.X = x + pos.X;
                vert1.Vertices.Y = y + pos.Y;
                AddTriangleVertex(vert1); // output vertex
            }

            dirty = true;
        }

        public void DrawSegment(CCPoint from, CCPoint to, float radius, CCColor4F color)
        {
			var cl = color;

			var a = from;
			var b = to;

			var n = CCPoint.Normalize(CCPoint.PerpendicularCCW(a - b));

			var lww = radius;
			var nw = n * lww;
			var v0 = b - nw;
			var v1 = b + nw;
			var v2 = a - nw;
			var v3 = a + nw;

			// Triangles from beginning to end
            AddTriangleVertex(new CCV3F_C4B(v1, cl));
            AddTriangleVertex(new CCV3F_C4B(v2, cl));
            AddTriangleVertex(new CCV3F_C4B(v0, cl));

            AddTriangleVertex(new CCV3F_C4B(v1, cl));
            AddTriangleVertex(new CCV3F_C4B(v2, cl));
            AddTriangleVertex(new CCV3F_C4B(v3, cl));

            var mb = (float)Math.Atan2(v1.Y - b.Y, v1.X - b.X);
            var ma = (float)Math.Atan2(v2.Y - a.Y, v2.X - a.X);

            // Draw rounded line caps
            DrawSolidArc(a, radius, ma, MathHelper.Pi, color);
            DrawSolidArc(b, radius, mb, MathHelper.Pi, color);

            dirty = true;
        }

        public void DrawLine(CCPoint from, CCPoint to, float lineWidth = 1)
        {
            DrawLine(from, to, lineWidth, new CCColor4B(Color.R, Color.G, Color.B, Opacity));
        }

        public void DrawLine(CCPoint from, CCPoint to, CCColor4B color)
        {
            DrawLine(from, to, 1, color);
        }

        public void DrawLine(CCPoint from, CCPoint to, float lineWidth, CCColor4B color)
        {
            var cl = color;

            var a = from;
            var b = to;

            var n = CCPoint.Normalize(CCPoint.PerpendicularCCW(a - b));

            var lww = lineWidth;
            var nw = n * lww;
            var v0 = b - nw;
            var v1 = b + nw;
            var v2 = a - nw;
            var v3 = a + nw;

            // Triangles from beginning to end
            AddTriangleVertex(new CCV3F_C4B(v1, cl));
            AddTriangleVertex(new CCV3F_C4B(v2, cl));
            AddTriangleVertex(new CCV3F_C4B(v0, cl));

            AddTriangleVertex(new CCV3F_C4B(v1, cl));
            AddTriangleVertex(new CCV3F_C4B(v2, cl));
            AddTriangleVertex(new CCV3F_C4B(v3, cl));

            dirty = true;
        }


        public void DrawCircle(CCPoint center, float radius, float angle, int segments, CCColor4B color)
        {
            float increment = MathHelper.Pi * 2.0f / segments;
            double theta = 0.0;

            CCPoint v1;
            List<CCPoint> verts = new List<CCPoint>();

            for (int i = 0; i < segments; i++)
            {
                v1 = center + new CCPoint((float)Math.Cos(theta), (float)Math.Sin(theta)) * radius;
                verts.Add(v1);
                theta += increment;
            }

            CCColor4F cf = new CCColor4F(color.R/255f, color.G/255f, color.B/255f, color.A/255f);
            DrawPolygon(verts.ToArray(), verts.Count, cf, 0, new CCColor4F(0f, 0f, 0f, 0f));
        }

        public void DrawRect(CCPoint p, float size)
        {

            DrawRect (p,size, new CCColor4B(Color.R, Color.G, Color.B, Opacity));
        }

        public void DrawRect(CCPoint p, float size, CCColor4B color)
        {

            var rect = CCRect.Zero;

            float hs = size / 2.0f;

            rect.Origin = p + new CCPoint(-hs, -hs);
            rect.Size = new CCSize (size, size);

            DrawRect (rect, color);
        }

        public void DrawRect(CCRect rect)
        {
            DrawRect(rect, new CCColor4B(Color.R, Color.G, Color.B, Opacity));
        }

        public void DrawRect(CCRect rect, CCColor4B fillColor)
        {
            DrawRect(rect, fillColor, 0, CCColor4B.Transparent);
        }

        public void DrawRect(CCRect rect, CCColor4B fillColor, float borderWidth,
            CCColor4B borderColor)
        {
            float x1 = rect.MinX;
            float y1 = rect.MinY;
            float x2 = rect.MaxX;
            float y2 = rect.MaxY;
            CCPoint[] pt = new CCPoint[] { 
                new CCPoint(x1,y1), new CCPoint(x2,y1), new CCPoint(x2,y2), new CCPoint(x1,y2)
            };
            CCColor4F cf = new CCColor4F(fillColor.R/255f, fillColor.G/255f, fillColor.B/255f, fillColor.A/255f);
            CCColor4F bc = new CCColor4F(borderColor.R/255f, borderColor.G/255f, borderColor.B/255f, borderColor.A/255f);
            DrawPolygon(pt, 4, cf, borderWidth, bc);
        }

        public void DrawEllipse (CCRect rect, float lineWidth, CCColor4B color)
        {
            DrawEllipticalArc(rect, 0, 360, false, lineWidth, color);
        }

        public void DrawEllipse (int x, int y, int width, int height, float lineWidth, CCColor4B color)
        {
            DrawEllipticalArc(x,y,width,height,0,360,false, lineWidth, color);

        }

        internal void DrawEllipticalArc(CCRect arcRect, double lambda1, double lambda2,
            bool isPieSlice, float lineWidth, CCColor4B color)
        {
            make_arcs( 
                arcRect.Origin.X, arcRect.Origin.Y, arcRect.Size.Width, arcRect.Size.Height, 
                (float)lambda1, (float)lambda2, 
                false, true, isPieSlice, lineWidth, color);
        }

        internal void DrawEllipticalArc(float x, float y, float width, float height, double lambda1, double lambda2,
            bool isPieSlice, float lineWidth, CCColor4B color)
        {
            make_arcs( 
                x, y, width, height, 
                (float)lambda1, (float)lambda2, 
                false, true, isPieSlice, lineWidth, color);
        }       

        public void DrawCatmullRom(List<CCPoint> points, int segments)
        {
            DrawCardinalSpline(points, 0.5f, segments);
        }

        public void DrawCatmullRom(List<CCPoint> points, int segments, CCColor4B color)
        {
            DrawCardinalSpline(points, 0.5f, segments, color);
        }

        public void DrawCardinalSpline(List<CCPoint> config, float tension, int segments)
        {
            DrawCardinalSpline(config, tension, segments, new CCColor4B (Color.R, Color.G, Color.B, Opacity));
        }

        public void DrawCardinalSpline(List<CCPoint> config, float tension, int segments, CCColor4B color)
        {

            int p;
            float lt;
            float deltaT = 1.0f / config.Count;

            int count = config.Count;

            var vertices = new CCPoint[segments + 1];

            for (int i = 0; i < segments + 1; i++)
            {
                float dt = (float) i / segments;

                // border
                if (dt == 1)
                {
                    p = count - 1;
                    lt = 1;
                }
                else
                {
                    p = (int) (dt / deltaT);
                    lt = (dt - deltaT * p) / deltaT;
                }

                // Interpolate    
                int c = config.Count - 1;
                CCPoint pp0 = config[Math.Min(c, Math.Max(p - 1, 0))];
                CCPoint pp1 = config[Math.Min(c, Math.Max(p + 0, 0))];
                CCPoint pp2 = config[Math.Min(c, Math.Max(p + 1, 0))];
                CCPoint pp3 = config[Math.Min(c, Math.Max(p + 2, 0))];

                vertices[i] = CCSplineMath.CCCardinalSplineAt(pp0, pp1, pp2, pp3, tension, lt);
            }

            DrawPolygon(vertices, vertices.Length, CCColor4B.Transparent, 1, color, false);
        }



        /// <summary>
        /// draws a cubic bezier path
        /// @since v0.8
        /// </summary>
        public void DrawCubicBezier(CCPoint origin, CCPoint control1, CCPoint control2, CCPoint destination, int segments, float lineWidth, CCColor4B color)
        {

            float t = 0;
            float increment = 1.0f / segments;

            var vertices = new CCPoint[segments];

            vertices[0] = origin;

            for (int i = 1; i < segments; ++i, t+= increment)
            {
                vertices[i].X = CCSplineMath.CubicBezier(origin.X, control1.X, control2.X, destination.X, t);
                vertices[i].Y = CCSplineMath.CubicBezier(origin.Y, control1.Y, control2.Y, destination.Y, t);
            }

            vertices[segments - 1] = destination;

            for (int i = 0; i < vertices.Length - 1; i++)
            {
                DrawLine(vertices[i], vertices[i + 1], lineWidth, color);
            }

            //DrawPolygon(vertices, vertices.Length, color, lineWidth, color);
        }

        public void DrawQuadBezier(CCPoint origin, CCPoint control, CCPoint destination, int segments, float lineWidth, CCColor4B color)
        {
            float t = 0;
            float increment = 1.0f / segments;

            var vertices = new CCPoint[segments];

            vertices[0] = origin;

            for (int i = 1; i < segments; ++i, t+= increment)
            {
                vertices[i].X = CCSplineMath.QuadBezier(origin.X, control.X, destination.X, t);
                vertices[i].Y = CCSplineMath.QuadBezier(origin.Y, control.Y, destination.Y, t);
            }

            vertices[segments-1] = destination;

            DrawPolygon(vertices, vertices.Length, CCColor4B.Transparent, lineWidth, color, false);
        }



        public void DrawPolygon(CCPoint[] verts, int count, CCColor4B fillColor, float borderWidth,
                        CCColor4B borderColor, bool closePolygon = true)
        {
            DrawPolygon(verts, count, new CCColor4F(fillColor), borderWidth, new CCColor4F(borderColor), closePolygon);
        }

        public void DrawTriangleList (CCV3F_C4B[] verts)
        {
           
            for (int x = 0; x < verts.Length; x++)
            {
                AddTriangleVertex(verts[x]);
            }
        }

        public void DrawLineList (CCV3F_C4B[] verts)
        {

            for (int x = 0; x < verts.Length; x++)
            {
                AddLineVertex(verts[x]);

            }
        }

        public void DrawPolygon(CCPoint[] verts, int count, CCColor4F fillColor, float borderWidth,
                                CCColor4F borderColor, bool closePolygon = true)
        {
            var polycount = count;

            var extrude = new ExtrudeVerts[polycount];

            for (int i = 0; i < polycount; i++)
            {
                var v0 = verts[(i - 1 + polycount) % polycount];
                var v1 = verts[i];
                var v2 = verts[(i + 1) % polycount];

                var n1 = CCPoint.Normalize(CCPoint.PerpendicularCCW(v1 - v0));
                var n2 = CCPoint.Normalize(CCPoint.PerpendicularCCW(v2 - v1));

                var offset = (n1 + n2) * (1.0f / (CCPoint.Dot(n1, n2) + 1.0f));
                extrude[i] = new ExtrudeVerts() {offset = offset, n = n2};
            }

            bool outline = (borderColor.A > 0.0f && borderWidth > 0.0f);

            var colorFill = new CCColor4B(fillColor);
            var borderFill = new CCColor4B(borderColor);
            
            float inset = (!outline ? 0.5f : 0.0f);
            
            for (int i = 0; i < polycount - 2; i++)
            {
                var v0 = verts[0] - (extrude[0].offset * inset);
                var v1 = verts[i + 1] - (extrude[i + 1].offset * inset);
                var v2 = verts[i + 2] - (extrude[i + 2].offset * inset);

                AddTriangleVertex(new CCV3F_C4B(v0, colorFill)); //__t(v2fzero)
                AddTriangleVertex(new CCV3F_C4B(v1, colorFill)); //__t(v2fzero)
                AddTriangleVertex(new CCV3F_C4B(v2, colorFill)); //__t(v2fzero)
            }

            //if (closePolygon)
            //{
            //    var i = polycount - 3;
            //    var v0 = verts[0] - (extrude[0].offset * inset);
            //    var v1 = verts[i + 1] - (extrude[i + 1].offset * inset);
            //    var v2 = verts[i + 2] - (extrude[i + 2].offset * inset);

            //    AddTriangleVertex(new CCV3F_C4B(v0, colorFill)); //__t(v2fzero)
            //    AddTriangleVertex(new CCV3F_C4B(v1, colorFill)); //__t(v2fzero)
            //    AddTriangleVertex(new CCV3F_C4B(v2, colorFill)); //__t(v2fzero)
            //}

            for (int i = 0; i < polycount - 1; i++)
            {
                int j = (i + 1) % polycount;
                var v0 = verts[i];
                var v1 = verts[j];

                var n0 = extrude[i].n;

                var offset0 = extrude[i].offset;
                var offset1 = extrude[j].offset;

                if (outline)
                {
                    var inner0 = (v0 - (offset0 * borderWidth));
                    var inner1 = (v1 - (offset1 * borderWidth));
                    var outer0 = (v0 + (offset0 * borderWidth));
                    var outer1 = (v1 + (offset1 * borderWidth));

                    AddTriangleVertex(new CCV3F_C4B(inner0, borderFill)); //__t(v2fneg(n0))
                    AddTriangleVertex(new CCV3F_C4B(inner1, borderFill)); //__t(v2fneg(n0))
                    AddTriangleVertex(new CCV3F_C4B(outer1, borderFill)); //__t(n0)

                    AddTriangleVertex(new CCV3F_C4B(inner0, borderFill)); //__t(v2fneg(n0))
                    AddTriangleVertex(new CCV3F_C4B(outer0, borderFill)); //__t(n0)
                    AddTriangleVertex(new CCV3F_C4B(outer1, borderFill)); //__t(n0)
                }
                else
                {
                    var inner0 = (v0 - (offset0 * 0.5f));
                    var inner1 = (v1 - (offset1 * 0.5f));
                    var outer0 = (v0 + (offset0 * 0.5f));
                    var outer1 = (v1 + (offset1 * 0.5f));

                    AddTriangleVertex(new CCV3F_C4B(inner0, colorFill)); //__t(v2fzero)
                    AddTriangleVertex(new CCV3F_C4B(inner1, colorFill)); //__t(v2fzero)
                    AddTriangleVertex(new CCV3F_C4B(outer1, colorFill)); //__t(n0)

                    AddTriangleVertex(new CCV3F_C4B(inner0, colorFill)); //__t(v2fzero)
                    AddTriangleVertex(new CCV3F_C4B(outer0, colorFill)); //__t(n0)
                    AddTriangleVertex(new CCV3F_C4B(outer1, colorFill)); //__t(n0)
                }
            }

            if (closePolygon)
            {
                for (int i = polycount - 1; i < polycount; i++)
                {
                    int j = (i + 1) % polycount;
                    var v0 = verts[i];
                    var v1 = verts[j];

                    var n0 = extrude[i].n;

                    var offset0 = extrude[i].offset;
                    var offset1 = extrude[j].offset;

                    if (outline)
                    {
                        var inner0 = (v0 - (offset0 * borderWidth));
                        var inner1 = (v1 - (offset1 * borderWidth));
                        var outer0 = (v0 + (offset0 * borderWidth));
                        var outer1 = (v1 + (offset1 * borderWidth));

                        AddTriangleVertex(new CCV3F_C4B(inner0, borderFill)); //__t(v2fneg(n0))
                        AddTriangleVertex(new CCV3F_C4B(inner1, borderFill)); //__t(v2fneg(n0))
                        AddTriangleVertex(new CCV3F_C4B(outer1, borderFill)); //__t(n0)

                        AddTriangleVertex(new CCV3F_C4B(inner0, borderFill)); //__t(v2fneg(n0))
                        AddTriangleVertex(new CCV3F_C4B(outer0, borderFill)); //__t(n0)
                        AddTriangleVertex(new CCV3F_C4B(outer1, borderFill)); //__t(n0)
                    }
                    else
                    {
                        var inner0 = (v0 - (offset0 * 0.5f));
                        var inner1 = (v1 - (offset1 * 0.5f));
                        var outer0 = (v0 + (offset0 * 0.5f));
                        var outer1 = (v1 + (offset1 * 0.5f));

                        AddTriangleVertex(new CCV3F_C4B(inner0, colorFill)); //__t(v2fzero)
                        AddTriangleVertex(new CCV3F_C4B(inner1, colorFill)); //__t(v2fzero)
                        AddTriangleVertex(new CCV3F_C4B(outer1, colorFill)); //__t(n0)

                        AddTriangleVertex(new CCV3F_C4B(inner0, colorFill)); //__t(v2fzero)
                        AddTriangleVertex(new CCV3F_C4B(outer0, colorFill)); //__t(n0)
                        AddTriangleVertex(new CCV3F_C4B(outer1, colorFill)); //__t(n0)
                    }
                }
            }
            dirty = true;
        }

        public void Clear()
        {
            triangleVertices.Clear();
            lineVertices.Clear();
            if (stringData != null)
                stringData.Clear();
        }

        void AddCustomCommandOnDraw(CCCustomCommand customCommandOnDraw)
        {
            var renderer = (Renderer != null) ? Renderer : DrawManager.Renderer;
            if (renderer != null)
            {
                renderer.AddCommand(customCommandOnDraw);
            }
        }

        protected override void VisitRenderer(ref CCAffineTransform worldTransform)
        {
            if (dirty)
            {
                //TODO: Set vertices to buffer
                dirty = false;
            }

            if (triangleVertices.Count > 0
                || lineVertices.Count > 0 
                || (spriteFont != null && stringData != null && stringData.Count > 0))
            {

                if (triangleVertices.Count > 0)
                {
                    renderTriangles.GlobalDepth = worldTransform.Tz;
                    renderTriangles.WorldTransform = worldTransform;
                    AddCustomCommandOnDraw(renderTriangles);
                }

                if (lineVertices.Count > 0)
                {
                    renderLines.GlobalDepth = worldTransform.Tz;
                    renderLines.WorldTransform = worldTransform;
                    AddCustomCommandOnDraw(renderLines);
                }

                if (spriteFont != null && stringData != null && stringData.Count > 0)
                {
                    renderStrings.GlobalDepth = worldTransform.Tz;
                    renderStrings.WorldTransform = worldTransform;
                    AddCustomCommandOnDraw(renderStrings);
                }

            }

        }

        void FlushTriangles()
        {
            var triangleVertsCount = triangleVertices.Count;
            if (triangleVertsCount >= 3)
            {
                var drawManager = DrawManager;

                drawManager.TextureEnabled = false;
                drawManager.BlendFunc(BlendFunc);

                int primitiveCount = triangleVertsCount / 3;
                // submit the draw call to the graphics card
                DrawManager.DrawPrimitives(PrimitiveType.TriangleList, triangleVertices.Elements, 0, primitiveCount);
            }
        }

        void FlushLines()
        {
            var lineVertsCount = lineVertices.Count;
            if (lineVertsCount >= 2)
            {
                var drawManager = DrawManager;

                drawManager.TextureEnabled = false;
                drawManager.BlendFunc(BlendFunc);

                int primitiveCount = lineVertsCount / 2;
                // submit the draw call to the graphics card
                 DrawManager.DrawPrimitives(PrimitiveType.LineList, lineVertices.Elements, 0, primitiveCount);
            }
        }

        void DrawStrings()
        {

            if (spriteFont == null || stringData == null || stringData.Count == 0)
                return; 

            var drawManager = DrawManager;

            drawManager.TextureEnabled = false;
            drawManager.BlendFunc(BlendFunc);

            var batch = CCDrawManager.SharedDrawManager.SpriteBatch;

            batch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);

            for (int i = 0; i < stringData.Count; i++)
            {
                stringBuilder.Length = 0;
                stringBuilder.AppendFormat(stringData[i].S, stringData[i].Args);

                batch.DrawString(spriteFont,
                    stringBuilder,
                    new Vector2(stringData[i].X, stringData[i].Y),
                    stringData[i].Color);
            }

            batch.End();

        }

        static CCPoint startPoint = CCPoint.Zero;
        static CCPoint destinationPoint = CCPoint.Zero;
        static CCPoint controlPoint1 = CCPoint.Zero;
        static CCPoint controlPoint2 = CCPoint.Zero;

        const int SEGMENTS = 50;

        /*
         * Based on the algorithm described in
         *      http://www.stillhq.com/ctpfaq/2002/03/c1088.html#AEN1212
         */
        void
        make_arc(bool start, float x, float y, float width,
            float height, float startAngle, float endAngle, bool antialiasing, bool isPieSlice, float lineWidth, CCColor4B color)
        {
            float delta, bcp;
            double sin_alpha, sin_beta, cos_alpha, cos_beta;
            float PI = (float)Math.PI;

            float rx = width / 2;
            float ry = height / 2;

            /* center */
            float cx = x + rx;
            float cy = y + ry;

            /* angles in radians */
            float alpha = startAngle * PI / 180;
            float beta = endAngle * PI / 180;

            /* adjust angles for ellipses */
            alpha = (float)Math.Atan2(rx * Math.Sin(alpha), ry * Math.Cos(alpha));
            beta = (float)Math.Atan2(rx * Math.Sin(beta), ry * Math.Cos(beta));

            if (Math.Abs(beta - alpha) > PI)
            {
                if (beta > alpha)
                    beta -= 2 * PI;
                else
                    alpha -= 2 * PI;
            }

            delta = beta - alpha;
            bcp = (float)(4.0 / 3.0 * (1 - Math.Cos(delta / 2)) / Math.Sin(delta / 2));

            sin_alpha = Math.Sin(alpha);
            sin_beta = Math.Sin(beta);
            cos_alpha = Math.Cos(alpha);
            cos_beta = Math.Cos(beta);

            /* don't move to starting point if we're continuing an existing curve */
            if (start)
            {
                /* starting point */
                double sx = cx + rx * cos_alpha;
                double sy = cy + ry * sin_alpha;
                if (isPieSlice) 
                {
                    destinationPoint.X = (float)sx;
                    destinationPoint.Y = (float)sy;

                    DrawPolygon(new CCPoint[] {startPoint,destinationPoint}, 2, CCColor4B.Transparent, lineWidth, color);
                }

                startPoint.X = (float)sx;
                startPoint.Y = (float)sy;
            }

            destinationPoint.X = cx + rx * (float)cos_beta;
            destinationPoint.Y = cy + ry * (float)sin_beta;

            controlPoint1.X = cx + rx * (float)(cos_alpha - bcp * sin_alpha);
            controlPoint1.Y = cy + ry * (float)(sin_alpha + bcp * cos_alpha);

            controlPoint2.X = cx + rx * (float)(cos_beta + bcp * sin_beta);
            controlPoint2.Y = cy + ry * (float)(sin_beta - bcp * cos_beta);


            DrawCubicBezier(startPoint, controlPoint1, controlPoint2, destinationPoint, SEGMENTS, lineWidth, color); 

            startPoint.X = destinationPoint.X;
            startPoint.Y = destinationPoint.Y;
        }


        void
        make_arcs(float x, float y, float width, float height, float startAngle, float sweepAngle,
            bool convert_units, bool antialiasing, bool isPieSlice, float lineWidth, CCColor4B color)
        {
            int i;
            float drawn = 0;
            float endAngle;
            bool enough = false;

            endAngle = startAngle + sweepAngle;
            /* if we end before the start then reverse positions (to keep increment positive) */
            if (endAngle < startAngle)
            {
                var temp = endAngle;
                endAngle = startAngle;
                startAngle = temp;
            }

            if (isPieSlice) {
                startPoint.X = x + (width / 2);
                startPoint.Y = y + (height / 2);
            }

            /* i is the number of sub-arcs drawn, each sub-arc can be at most 90 degrees.*/
            /* there can be no more then 4 subarcs, ie. 90 + 90 + 90 + (something less than 90) */
            for (i = 0; i < 4; i++)
            {
                float current = startAngle + drawn;
                float additional;

                if (enough) 
                {
                    if (isPieSlice) 
                    {
                        startPoint.X = x + (width / 2);
                        startPoint.Y = y + (height / 2);
                        DrawPolygon(new CCPoint[] {destinationPoint, startPoint}, 2, CCColor4B.Transparent, lineWidth, color);
                    }
                    return;
                }

                additional = endAngle - current; /* otherwise, add the remainder */
                if (additional > 90)
                {
                    additional = 90.0f;
                }
                else
                {
                    /* a near zero value will introduce bad artefact in the drawing (#78999) */
                    if (( additional >= -0.0001f) && (additional <= 0.0001f))
                        return;
                    enough = true;
                }

                make_arc((i == 0),    /* only move to the starting pt in the 1st iteration */
                    x, y, width, height,   /* bounding rectangle */
                    current, current + additional, antialiasing, isPieSlice, lineWidth, color);

                drawn += additional;

            }

            if (isPieSlice) {
                startPoint.X = x + (width / 2);
                startPoint.Y = y + (height / 2);
                DrawPolygon (new CCPoint[] { destinationPoint, startPoint}, 2, CCColor4B.Transparent, lineWidth, color);
            }

        }

    }
}
