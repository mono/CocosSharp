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
        CCRawList<VertexPositionColor> triangleVertices;
        CCRawList<VertexPositionColor> lineVertices;
        SpriteFont spriteFont;
        List<StringData> stringData;
        StringBuilder stringBuilder;
        string spriteFontName;

        bool dirty;

        /** draw a polygon with a fill color and line color */

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
            BlendFunc = CCBlendFunc.AlphaBlend;
            triangleVertices = new CCRawList<VertexPositionColor>(DefaultBufferSize);
            lineVertices = new CCRawList<VertexPositionColor>(DefaultBufferSize);
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


		// See http://slabode.exofire.net/circle_draw.shtml
		// An Efficient Way to Draw Approximate Circles in OpenGL
		// Try to keep from calculating Cos and Sin of values everytime and just use
		// add and subtract where possible to calculate the values.
		public void DrawDot(CCPoint pos, float radius, CCColor4F color)
		{
			var cl = color.ToColor();

			var segments = 10 * (float)Math.Sqrt(radius);  //<- Let's try to guess at # segments for a reasonable smoothness

			float theta = MathHelper.Pi * 2.0f / segments; 
			float tangetial_factor = (float)Math.Tan(theta);   //calculate the tangential factor 

			float radial_factor = (float)Math.Cos(theta);   //calculate the radial factor 

			float x = radius;  //we start at angle = 0 
			float y = 0; 

			var verticeCenter = new VertexPositionColor(new Vector3(pos.X, pos.Y, 0), cl);
			var vert1 = new VertexPositionColor(Vector3.Zero, cl);
			float tx = 0; 
			float ty = 0; 

			for (int i = 0; i < segments; i++)
			{
			
				vert1.Position.X = x + pos.X;
				vert1.Position.Y = y + pos.Y;
				triangleVertices.Add(vert1); // output vertex

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

				vert1.Position.X = x + pos.X;
				vert1.Position.Y = y + pos.Y;
				triangleVertices.Add(vert1); // output vertex

				triangleVertices.Add(verticeCenter);
			} 

			dirty = true;
		}

        // See http://slabode.exofire.net/circle_draw.shtml
        // An Efficient Way to Draw Approximate Circles in OpenGL
        // Try to keep from calculating Cos and Sin of values everytime and just use
        // add and subtract where possible to calculate the values.
        public void DrawCircle(CCPoint pos, float radius, CCColor4B color)
        {
            var cl = color.ToColor();

            int segments = (int)(10 * (float)Math.Sqrt(radius));  //<- Let's try to guess at # segments for a reasonable smoothness

            float theta = MathHelper.Pi * 2.0f / segments;
            float tangetial_factor = (float)Math.Tan(theta);   //calculate the tangential factor 

            float radial_factor = (float)Math.Cos(theta);   //calculate the radial factor 

            float x = radius;  //we start at angle = 0 
            float y = 0;

            var verticeCenter = new VertexPositionColor(new Vector3(pos.X, pos.Y, 0), cl);
            var vert1 = new VertexPositionColor(Vector3.Zero, cl);
            float tx = 0;
            float ty = 0;

            for (int i = 0; i < segments; i++)
            {

                vert1.Position.X = x + pos.X;
                vert1.Position.Y = y + pos.Y;
                lineVertices.Add(vert1); // output vertex

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

                vert1.Position.X = x + pos.X;
                vert1.Position.Y = y + pos.Y;
                lineVertices.Add(vert1); // output vertex

            }

            dirty = true;
        }

        // See http://slabode.exofire.net/circle_draw.shtml
        // An Efficient Way to Draw Approximate Circles in OpenGL
        // Try to keep from calculating Cos and Sin of values everytime and just use
        // add and subtract where possible to calculate the values.
        public void DrawSolidCircle(CCPoint pos, float radius, CCColor4B color)
        {
            var cl = color.ToColor();

            int segments = (int)(10 * (float)Math.Sqrt(radius));  //<- Let's try to guess at # segments for a reasonable smoothness

            float theta = MathHelper.Pi * 2.0f / segments;
            float tangetial_factor = (float)Math.Tan(theta);   //calculate the tangential factor 

            float radial_factor = (float)Math.Cos(theta);   //calculate the radial factor 

            float x = radius;  //we start at angle = 0 
            float y = 0;

            var verticeCenter = new VertexPositionColor(new Vector3(pos.X, pos.Y, 0), cl);
            var vert1 = new VertexPositionColor(Vector3.Zero, cl);
            float tx = 0;
            float ty = 0;

            for (int i = 0; i < segments; i++)
            {
                triangleVertices.Add(verticeCenter);

                vert1.Position.X = x + pos.X;
                vert1.Position.Y = y + pos.Y;
                triangleVertices.Add(vert1); // output vertex

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

                vert1.Position.X = x + pos.X;
                vert1.Position.Y = y + pos.Y;
                triangleVertices.Add(vert1); // output vertex
            }

            dirty = true;
        }


        // Used for drawing line caps
        public void DrawSolidArc(CCPoint pos, float radius, float startAngle, float sweepAngle, CCColor4B color)
        {
            var cl = color.ToColor();

            int segments = (int)(10 * (float)Math.Sqrt(radius));  //<- Let's try to guess at # segments for a reasonable smoothness

            float theta = sweepAngle / (segments - 1);// MathHelper.Pi * 2.0f / segments;
            float tangetial_factor = (float)Math.Tan(theta);   //calculate the tangential factor 

            float radial_factor = (float)Math.Cos(theta);   //calculate the radial factor 

            float x = radius * (float)Math.Cos(startAngle);   //we now start at the start angle
            float y = radius * (float)Math.Sin(startAngle); 

            var verticeCenter = new VertexPositionColor(new Vector3(pos.X, pos.Y, 0), cl);
            var vert1 = new VertexPositionColor(Vector3.Zero, cl);
            float tx = 0;
            float ty = 0;

            for (int i = 0; i < segments - 1; i++)
            {
                triangleVertices.Add(verticeCenter);

                vert1.Position.X = x + pos.X;
                vert1.Position.Y = y + pos.Y;
                triangleVertices.Add(vert1); // output vertex

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

                vert1.Position.X = x + pos.X;
                vert1.Position.Y = y + pos.Y;
                triangleVertices.Add(vert1); // output vertex
            }

            dirty = true;
        }

        public void DrawSegment(CCPoint from, CCPoint to, float radius, CCColor4F color)
        {
			var cl = color.ToColor();

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
            triangleVertices.Add(new VertexPositionColor(v1.ToVector3(), cl));
            triangleVertices.Add(new VertexPositionColor(v2.ToVector3(), cl));
            triangleVertices.Add(new VertexPositionColor(v0.ToVector3(), cl));

            triangleVertices.Add(new VertexPositionColor(v1.ToVector3(), cl));
            triangleVertices.Add(new VertexPositionColor(v2.ToVector3(), cl));
            triangleVertices.Add(new VertexPositionColor(v3.ToVector3(), cl));

            var mb = (float)Math.Atan2(v1.Y - b.Y, v1.X - b.X);
            var ma = (float)Math.Atan2(v2.Y - a.Y, v2.X - a.X);

            // Draw rounded line caps
            DrawSolidArc(a, radius, ma, MathHelper.Pi, color);
            DrawSolidArc(b, radius, mb, MathHelper.Pi, color);

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

        public void DrawPolygon(CCPoint[] verts, int count, CCColor4B fillColor, float borderWidth,
                        CCColor4B borderColor)
        {
            DrawPolygon(verts, count, new CCColor4F(fillColor), borderWidth, new CCColor4F(borderColor));
        }

        public void DrawPolygon(CCPoint[] verts, int count, CCColor4F fillColor, float borderWidth,
                                CCColor4F borderColor)
        {
            var extrude = new ExtrudeVerts[count];

            for (int i = 0; i < count; i++)
            {
                var v0 = verts[(i - 1 + count) % count];
                var v1 = verts[i];
                var v2 = verts[(i + 1) % count];

                var n1 = CCPoint.Normalize(CCPoint.PerpendicularCCW(v1 - v0));
                var n2 = CCPoint.Normalize(CCPoint.PerpendicularCCW(v2 - v1));

                var offset = (n1 + n2) * (1.0f / (CCPoint.Dot(n1, n2) + 1.0f));
                extrude[i] = new ExtrudeVerts() {offset = offset, n = n2};
            }

            bool outline = (borderColor.A > 0.0f && borderWidth > 0.0f);

            var colorFill = fillColor.ToColor();
            var borderFill = borderColor.ToColor();
            
            float inset = (!outline ? 0.5f : 0.0f);
            
            for (int i = 0; i < count - 2; i++)
            {
                var v0 = verts[0] - (extrude[0].offset * inset);
                var v1 = verts[i + 1] - (extrude[i + 1].offset * inset);
                var v2 = verts[i + 2] - (extrude[i + 2].offset * inset);

                triangleVertices.Add(new VertexPositionColor(v0.ToVector3(), colorFill)); //__t(v2fzero)
                triangleVertices.Add(new VertexPositionColor(v1.ToVector3(), colorFill)); //__t(v2fzero)
                triangleVertices.Add(new VertexPositionColor(v2.ToVector3(), colorFill)); //__t(v2fzero)
            }

            for (int i = 0; i < count; i++)
            {
                int j = (i + 1) % count;
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

                    triangleVertices.Add(new VertexPositionColor(inner0.ToVector3(), borderFill)); //__t(v2fneg(n0))
                    triangleVertices.Add(new VertexPositionColor(inner1.ToVector3(), borderFill)); //__t(v2fneg(n0))
                    triangleVertices.Add(new VertexPositionColor(outer1.ToVector3(), borderFill)); //__t(n0)

                    triangleVertices.Add(new VertexPositionColor(inner0.ToVector3(), borderFill)); //__t(v2fneg(n0))
                    triangleVertices.Add(new VertexPositionColor(outer0.ToVector3(), borderFill)); //__t(n0)
                    triangleVertices.Add(new VertexPositionColor(outer1.ToVector3(), borderFill)); //__t(n0)
                }
                else
                {
                    var inner0 = (v0 - (offset0 * 0.5f));
                    var inner1 = (v1 - (offset1 * 0.5f));
                    var outer0 = (v0 + (offset0 * 0.5f));
                    var outer1 = (v1 + (offset1 * 0.5f));

                    triangleVertices.Add(new VertexPositionColor(inner0.ToVector3(), colorFill)); //__t(v2fzero)
                    triangleVertices.Add(new VertexPositionColor(inner1.ToVector3(), colorFill)); //__t(v2fzero)
                    triangleVertices.Add(new VertexPositionColor(outer1.ToVector3(), colorFill)); //__t(n0)

                    triangleVertices.Add(new VertexPositionColor(inner0.ToVector3(), colorFill)); //__t(v2fzero)
                    triangleVertices.Add(new VertexPositionColor(outer0.ToVector3(), colorFill)); //__t(n0)
                    triangleVertices.Add(new VertexPositionColor(outer1.ToVector3(), colorFill)); //__t(n0)
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

        protected override void Draw()
        {
            if (dirty)
            {
                //TODO: Set vertices to buffer
                dirty = false;
            }

            Window.DrawManager.TextureEnabled = false;
            Window.DrawManager.BlendFunc(BlendFunc);
            FlushTriangles();
            FlushLines();
            DrawStrings();
        }

        void FlushTriangles()
        {
            var triangleVertsCount = triangleVertices.Count;
            if (triangleVertsCount >= 3)
            {
                int primitiveCount = triangleVertsCount / 3;
                // submit the draw call to the graphics card
                Window.DrawManager.DrawPrimitives(PrimitiveType.TriangleList, triangleVertices.Elements, 0, primitiveCount);
                //DrawManager.DrawCount++;
            }
        }

        void FlushLines()
        {
            var lineVertsCount = lineVertices.Count;
            if (lineVertsCount >= 2)
            {
                int primitiveCount = lineVertsCount / 2;
                // submit the draw call to the graphics card
                 Window.DrawManager.DrawPrimitives(PrimitiveType.LineList, lineVertices.Elements, 0, primitiveCount);
                //DrawManager.DrawCount++;
            }
        }

        void DrawStrings()
        {

            if (spriteFont == null || stringData == null || stringData.Count == 0)
                return; 

            var _batch = CCDrawManager.SharedDrawManager.SpriteBatch;

            _batch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);

            for (int i = 0; i < stringData.Count; i++)
            {
                stringBuilder.Length = 0;
                stringBuilder.AppendFormat(stringData[i].S, stringData[i].Args);

                _batch.DrawString(spriteFont,
                    stringBuilder,
                    new Vector2(stringData[i].X, stringData[i].Y),
                    stringData[i].Color);
            }

            _batch.End();

        }

    }
}
