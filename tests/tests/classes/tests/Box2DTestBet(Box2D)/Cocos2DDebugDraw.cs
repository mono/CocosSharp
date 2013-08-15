using System;
using System.Collections.Generic;
using Box2D.Collision;
using Box2D.Common;
using Cocos2D;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Box2D.TestBed
{
    public static class b2VecHelper
    {
        public static Vector2 ToVector2(this b2Vec2 vec)
        {
            return new Vector2(vec.x, vec.y);
        }

        public static Color ToColor(this b2Color color)
        {
            return new Color(color.r, color.g, color.b);
        }
    }

    public class Cocos2DDebugDraw : b2Draw
    {
#if XBOX || WINDOWS_PHONE || OUYA
        public const int CircleSegments = 16;
#else
        public const int CircleSegments = 32;
#endif

        private CCPrimitiveBatch _primitiveBatch;

        public Color TextColor = Color.White;
       
        private SpriteFont _spriteFont;


        public Cocos2DDebugDraw()
        {
            _primitiveBatch = new CCPrimitiveBatch(CCDrawManager.GraphicsDevice);
            _spriteFont = CCApplication.SharedApplication.Content.Load<SpriteFont>("fonts/arial-12");
        }

        public override void DrawPolygon(b2Vec2[] vertices, int vertexCount, b2Color color)
        {
            if (!_primitiveBatch.IsReady())
            {
                throw new InvalidOperationException("BeginCustomDraw must be called before drawing anything.");
            }
            for (int i = 0; i < vertexCount - 1; i++)
            {
                _primitiveBatch.AddVertex(vertices[i].ToVector2(), color.ToColor(), PrimitiveType.LineList);
                _primitiveBatch.AddVertex(vertices[i + 1].ToVector2(), color.ToColor(), PrimitiveType.LineList);
            }

            _primitiveBatch.AddVertex(vertices[vertexCount - 1].ToVector2(), color.ToColor(), PrimitiveType.LineList);
            _primitiveBatch.AddVertex(vertices[0].ToVector2(), color.ToColor(), PrimitiveType.LineList);
        }

        public override void DrawSolidPolygon(b2Vec2[] vertices, int vertexCount, b2Color color)
        {
            if (!_primitiveBatch.IsReady())
            {
                throw new InvalidOperationException("BeginCustomDraw must be called before drawing anything.");
            }
            if (vertexCount == 2)
            {
                DrawPolygon(vertices, vertexCount, color);
                return;
            }

            var colorFill = color.ToColor() * 0.5f;
 
            for (int i = 1; i < vertexCount - 1; i++)
            {
                _primitiveBatch.AddVertex(vertices[0].ToVector2(), colorFill, PrimitiveType.TriangleList);
                _primitiveBatch.AddVertex(vertices[i].ToVector2(), colorFill, PrimitiveType.TriangleList);
                _primitiveBatch.AddVertex(vertices[i + 1].ToVector2(), colorFill, PrimitiveType.TriangleList);
            }
                DrawPolygon(vertices, vertexCount, color);
        }

        public override void DrawCircle(b2Vec2 center, float radius, b2Color color)
        {
            if (!_primitiveBatch.IsReady())
            {
                throw new InvalidOperationException("BeginCustomDraw must be called before drawing anything.");
            }
            const double increment = Math.PI * 2.0 / CircleSegments;
            double theta = 0.0;

            for (int i = 0, count = CircleSegments; i < count; i++)
            {
                b2Vec2 v1 = center + radius * new b2Vec2((float)Math.Cos(theta), (float)Math.Sin(theta));
                b2Vec2 v2 = center +
                             radius *
                             new b2Vec2((float)Math.Cos(theta + increment), (float)Math.Sin(theta + increment));

                _primitiveBatch.AddVertex(v1.ToVector2(), color.ToColor(), PrimitiveType.LineList);
                _primitiveBatch.AddVertex(v2.ToVector2(), color.ToColor(), PrimitiveType.LineList);

                theta += increment;
            }
        }

        public override void DrawSolidCircle(b2Vec2 center, float radius, b2Vec2 axis, b2Color color)
        {
            if (!_primitiveBatch.IsReady())
            {
                throw new InvalidOperationException("BeginCustomDraw must be called before drawing anything.");
            }
            const double increment = Math.PI * 2.0 / CircleSegments;
            double theta = 0.0;

            var colorFill = color.ToColor() * 0.5f;

            b2Vec2 v0 = center + radius * new b2Vec2((float)Math.Cos(theta), (float)Math.Sin(theta));
            theta += increment;

            for (int i = 1; i < CircleSegments - 1; i++)
            {
                b2Vec2 v1 = center + radius * new b2Vec2((float)Math.Cos(theta), (float)Math.Sin(theta));
                b2Vec2 v2 = center +
                             radius *
                             new b2Vec2((float)Math.Cos(theta + increment), (float)Math.Sin(theta + increment));

                _primitiveBatch.AddVertex(v0.ToVector2(), colorFill, PrimitiveType.TriangleList);
                _primitiveBatch.AddVertex(v1.ToVector2(), colorFill, PrimitiveType.TriangleList);
                _primitiveBatch.AddVertex(v2.ToVector2(), colorFill, PrimitiveType.TriangleList);

                theta += increment;
            }
            DrawCircle(center, radius, color);

            DrawSegment(center, center + axis * radius, color);
        }

        public override void DrawSegment(b2Vec2 p1, b2Vec2 p2, b2Color color)
        {
            if (!_primitiveBatch.IsReady())
            {
                throw new InvalidOperationException("BeginCustomDraw must be called before drawing anything.");
            }
            _primitiveBatch.AddVertex(p1.ToVector2(), color.ToColor(), PrimitiveType.LineList);
            _primitiveBatch.AddVertex(p2.ToVector2(), color.ToColor(), PrimitiveType.LineList);
        }

        public override void DrawTransform(b2Transform xf)
        {
            const float axisScale = 0.4f;
            b2Vec2 p1 = xf.p;

            b2Vec2 p2 = p1 + axisScale * xf.q.GetXAxis();
            DrawSegment(p1, p2, new b2Color(1, 0, 0));

            p2 = p1 + axisScale * xf.q.GetYAxis();
            DrawSegment(p1, p2, new b2Color(0, 1, 0));
        }

        public void DrawString(int x, int y, string format, params object[] objects)
        {
            CCDrawManager.spriteBatch.Begin();
            CCDrawManager.spriteBatch.DrawString(_spriteFont, String.Format(format, objects), new Vector2(x, y), TextColor);
            CCDrawManager.spriteBatch.End();
        }

        public void DrawPoint(b2Vec2 p, float size, b2Color color)
        {
            b2Vec2[] verts = new b2Vec2[4];
            float hs = size / 2.0f;
            verts[0] = p + new b2Vec2(-hs, -hs);
            verts[1] = p + new b2Vec2(hs, -hs);
            verts[2] = p + new b2Vec2(hs, hs);
            verts[3] = p + new b2Vec2(-hs, hs);

            DrawSolidPolygon(verts, 4, color);
        }

        public void DrawAABB(b2AABB aabb, b2Color p1)
        {
            b2Vec2[] verts = new b2Vec2[4];
            verts[0] = new b2Vec2(aabb.LowerBound.x, aabb.LowerBound.y);
            verts[1] = new b2Vec2(aabb.UpperBound.x, aabb.LowerBound.y);
            verts[2] = new b2Vec2(aabb.UpperBound.x, aabb.UpperBound.y);
            verts[3] = new b2Vec2(aabb.LowerBound.x, aabb.UpperBound.y);

            DrawPolygon(verts, 4, p1);
        }

        public void Begin()
        {
           _primitiveBatch.Begin();
        }

        public void End()
        {
            _primitiveBatch.End();
        }
    }

}