using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CocosSharp
{
    public class CCDrawNode : CCNode
    {
        CCRawList<VertexPositionColor> vertices;
        bool dirty;

        /** draw a polygon with a fill color and line color */

        struct ExtrudeVerts
        {
            public CCPoint offset;
            public CCPoint n;
        }

        public CCBlendFunc BlendFunc { get; set; }


        #region Constructors

        public CCDrawNode()
        {
            BlendFunc = CCBlendFunc.AlphaBlend;
            vertices = new CCRawList<VertexPositionColor>(512);
        }

        #endregion Constructors


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
				vertices.Add(vert1); // output vertex

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
				vertices.Add(vert1); // output vertex

				vertices.Add(verticeCenter);
			} 

			dirty = true;
		}

        public void DrawSegment(CCPoint from, CCPoint to, float radius, CCColor4F color)
        {
			var cl = color.ToColor();

			var a = from;
			var b = to;


			// Dots are causing a problem right now when being drawn.
			//DrawDot(a, radius, color); // Draw starting line ending
			//DrawDot(b, radius, color); // Draw ending Line ending

			var n = CCPoint.Normalize(CCPoint.PerpendicularCCW(a - b));

			var lww = radius;
			var nw = n * lww;
			var v0 = b - nw;
			var v1 = b + nw;
			var v2 = a - nw;
			var v3 = a + nw;

			// Triangles from beginning to end
			vertices.Add(new VertexPositionColor(v1.ToVector3(), cl));
			vertices.Add(new VertexPositionColor(v2.ToVector3(), cl));
			vertices.Add(new VertexPositionColor(v0.ToVector3(), cl));

			vertices.Add(new VertexPositionColor(v1.ToVector3(), cl));
			vertices.Add(new VertexPositionColor(v2.ToVector3(), cl));
			vertices.Add(new VertexPositionColor(v3.ToVector3(), cl));


            dirty = true;
        }

        public void DrawCircle(CCPoint center, float radius, CCColor4B color)
        {
            DrawCircle(center, radius, CCMacros.CCDegreesToRadians(360f), 360, color);
        }

        public void DrawCircle(CCPoint center, float radius, float angle, int segments, CCColor4B color)
        {
            float increment = MathHelper.Pi * 2.0f / segments;
            double theta = 0.0;

            CCPoint v1;
            CCPoint v2 = CCPoint.Zero;
            List<CCPoint> verts = new List<CCPoint>();

            for (int i = 0; i < segments; i++)
            {
                v1 = center + new CCPoint((float)Math.Cos(theta), (float)Math.Sin(theta)) * radius;
                v2 = center + new CCPoint((float)Math.Cos(theta + increment), (float)Math.Sin(theta + increment)) * radius;
                verts.Add(v1);
                theta += increment;
            }

            CCColor4F cf = new CCColor4F(color.R/255f, color.G/255f, color.B/255f, color.A/255f);
            DrawPolygon(verts.ToArray(), verts.Count, cf, 0, new CCColor4F(0f, 0f, 0f, 0f));
        }

        public void DrawRect(CCRect rect, CCColor4B color)
        {
            float x1 = rect.MinX;
            float y1 = rect.MinY;
            float x2 = rect.MaxX;
            float y2 = rect.MaxY;
            CCPoint[] pt = new CCPoint[] { 
                new CCPoint(x1,y1), new CCPoint(x2,y1), new CCPoint(x2,y2), new CCPoint(x1,y2)
            };
            CCColor4F cf = new CCColor4F(color.R/255f, color.G/255f, color.B/255f, color.A/255f);
            DrawPolygon(pt, 4, cf, 0, new CCColor4F(0f, 0f, 0f, 0f));
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

            bool outline = (fillColor.A > 0.0f && borderWidth > 0.0f);

            float inset = (!outline ? 0.5f : 0.0f);
            
            for (int i = 0; i < count - 2; i++)
            {
                var v0 = verts[0] - (extrude[0].offset * inset);
                var v1 = verts[i + 1] - (extrude[i + 1].offset * inset);
                var v2 = verts[i + 2] - (extrude[i + 2].offset * inset);

                vertices.Add(new VertexPositionColor(v0.ToVector3(), fillColor.ToColor())); //__t(v2fzero)
                vertices.Add(new VertexPositionColor(v1.ToVector3(), fillColor.ToColor())); //__t(v2fzero)
                vertices.Add(new VertexPositionColor(v2.ToVector3(), fillColor.ToColor())); //__t(v2fzero)
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

                    vertices.Add(new VertexPositionColor(inner0.ToVector3(), borderColor.ToColor())); //__t(v2fneg(n0))
                    vertices.Add(new VertexPositionColor(inner1.ToVector3(), borderColor.ToColor())); //__t(v2fneg(n0))
                    vertices.Add(new VertexPositionColor(outer1.ToVector3(), borderColor.ToColor())); //__t(n0)

                    vertices.Add(new VertexPositionColor(inner0.ToVector3(), borderColor.ToColor())); //__t(v2fneg(n0))
                    vertices.Add(new VertexPositionColor(outer0.ToVector3(), borderColor.ToColor())); //__t(n0)
                    vertices.Add(new VertexPositionColor(outer1.ToVector3(), borderColor.ToColor())); //__t(n0)
                }
                else
                {
                    var inner0 = (v0 - (offset0 * 0.5f));
                    var inner1 = (v1 - (offset1 * 0.5f));
                    var outer0 = (v0 + (offset0 * 0.5f));
                    var outer1 = (v1 + (offset1 * 0.5f));

                    vertices.Add(new VertexPositionColor(inner0.ToVector3(), fillColor.ToColor())); //__t(v2fzero)
                    vertices.Add(new VertexPositionColor(inner1.ToVector3(), fillColor.ToColor())); //__t(v2fzero)
                    vertices.Add(new VertexPositionColor(outer1.ToVector3(), fillColor.ToColor())); //__t(n0)

                    vertices.Add(new VertexPositionColor(inner0.ToVector3(), fillColor.ToColor())); //__t(v2fzero)
                    vertices.Add(new VertexPositionColor(outer0.ToVector3(), fillColor.ToColor())); //__t(n0)
                    vertices.Add(new VertexPositionColor(outer1.ToVector3(), fillColor.ToColor())); //__t(n0)
                }
            }

            dirty = true;
        }

        public void Clear()
        {
            vertices.Clear();
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
            Window.DrawManager.DrawPrimitives(PrimitiveType.TriangleList, vertices.Elements, 0, vertices.Count / 3);
        }
    }
}
