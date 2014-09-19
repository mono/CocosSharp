using System.Collections.Generic;
using System.Text;
using Box2D.Collision;
using Box2D.Common;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CocosSharp
{
    internal static class b2VecHelper
    {
        public static CCVector2 ToCCVector2(this b2Vec2 vec)
        {
            return new CCVector2(vec.x, vec.y);
        }

        internal static Color ToColor(this b2Color color)
        {
            return new Color(color.r, color.g, color.b);
        }

        internal static CCColor4B ToCCColor4B(this b2Color color)
        {
            return new CCColor4B (color.r, color.g, color.b, 255);
        }
    }

    public class CCBox2dDraw : b2Draw
    {
        #if WINDOWS_PHONE || OUYA
        public const int CircleSegments = 16;
        #else
        public const int CircleSegments = 32;
        #endif
        internal Color TextColor = Color.White;

        CCPrimitiveBatch primitiveBatch;
        SpriteFont spriteFont;
        List<StringData> stringData;
        StringBuilder stringBuilder;


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

        public CCBox2dDraw(string spriteFontName, int ptmRatio) : base(ptmRatio)
        {
            primitiveBatch = new CCPrimitiveBatch(5000);
            spriteFont = CCContentManager.SharedContentManager.Load<SpriteFont>(spriteFontName);
            stringData = new List<StringData>();
            stringBuilder = new StringBuilder();
        }

        #endregion Constructors


        public override void DrawPolygon(b2Vec2[] vertices, int vertexCount, b2Color color)
        {
            if (!primitiveBatch.IsReady())
            {
                throw new InvalidOperationException("BeginCustomDraw must be called before drawing anything.");
            }

            for (int i = 0; i < vertexCount - 1; i++)
            {
                primitiveBatch.AddVertex(vertices[i].ToCCVector2() * PTMRatio, color.ToCCColor4B(), PrimitiveType.LineList);
                primitiveBatch.AddVertex(vertices[i + 1].ToCCVector2() * PTMRatio, color.ToCCColor4B(), PrimitiveType.LineList);
            }

            primitiveBatch.AddVertex(vertices[vertexCount - 1].ToCCVector2() * PTMRatio, color.ToCCColor4B(), PrimitiveType.LineList);
            primitiveBatch.AddVertex(vertices[0].ToCCVector2() * PTMRatio, color.ToCCColor4B(), PrimitiveType.LineList);
        }

        public override void DrawSolidPolygon(b2Vec2[] vertices, int vertexCount, b2Color color)
        {
            if (!primitiveBatch.IsReady())
            {
                throw new InvalidOperationException("BeginCustomDraw must be called before drawing anything.");
            }

            if (vertexCount == 2)
            {
                DrawPolygon(vertices, vertexCount, color);
                return;
            }

            var colorFill = color.ToCCColor4B() * 0.5f;

            for (int i = 1; i < vertexCount - 1; i++)
            {
                primitiveBatch.AddVertex(vertices[0].ToCCVector2() * PTMRatio, colorFill, PrimitiveType.TriangleList);
                primitiveBatch.AddVertex(vertices[i].ToCCVector2() * PTMRatio, colorFill, PrimitiveType.TriangleList);
                primitiveBatch.AddVertex(vertices[i + 1].ToCCVector2() * PTMRatio, colorFill, PrimitiveType.TriangleList);
            }

            DrawPolygon(vertices, vertexCount, color);
        }

        public override void DrawCircle(b2Vec2 center, float radius, b2Color color)
        {
            if (!primitiveBatch.IsReady())
            {
                throw new InvalidOperationException("BeginCustomDraw must be called before drawing anything.");
            }
            const double increment = Math.PI * 2.0 / CircleSegments;
            double theta = 0.0;

            var col = color.ToCCColor4B();
            CCVector2 centr = center.ToCCVector2();

            for (int i = 0, count = CircleSegments; i < count; i++)
            {
                CCVector2 v1 = (centr + radius * new CCVector2((float) Math.Cos(theta), (float) Math.Sin(theta))) * PTMRatio;
                CCVector2 v2 = (centr +
                    radius *
                    new CCVector2((float) Math.Cos(theta + increment), (float) Math.Sin(theta + increment))) * PTMRatio;

                primitiveBatch.AddVertex(ref v1, col, PrimitiveType.LineList);
                primitiveBatch.AddVertex(ref v2, col, PrimitiveType.LineList);

                theta += increment;
            }
        }

        public override void DrawSolidCircle(b2Vec2 center, float radius, b2Vec2 axis, b2Color color)
        {
            if (!primitiveBatch.IsReady())
            {
                throw new InvalidOperationException("BeginCustomDraw must be called before drawing anything.");
            }
            const double increment = Math.PI * 2.0 / CircleSegments;
            double theta = 0.0;

            var colorFill = color.ToCCColor4B() * 0.5f;
            var centr = center.ToCCVector2();

            CCVector2 v0 = center.ToCCVector2() + radius * new CCVector2((float) Math.Cos(theta), (float) Math.Sin(theta));
            theta += increment;

            v0 *= PTMRatio;

            for (int i = 1; i < CircleSegments - 1; i++)
            {
                var v1 = centr + radius * new CCVector2((float) Math.Cos(theta), (float) Math.Sin(theta));

                v1 *= PTMRatio;

                var v2 = centr +
                    radius * new CCVector2((float) Math.Cos(theta + increment), (float) Math.Sin(theta + increment));

                v2 *= PTMRatio;

                primitiveBatch.AddVertex(ref v0, colorFill, PrimitiveType.TriangleList);
                primitiveBatch.AddVertex(ref v1, colorFill, PrimitiveType.TriangleList);
                primitiveBatch.AddVertex(ref v2, colorFill, PrimitiveType.TriangleList);

                theta += increment;
            }
            DrawCircle(center, radius, color);

            DrawSegment(center, center + axis * radius, color);
        }

        public override void DrawSegment(b2Vec2 p1, b2Vec2 p2, b2Color color)
        {
            if (!primitiveBatch.IsReady())
            {
                throw new InvalidOperationException("BeginCustomDraw must be called before drawing anything.");
            }
            primitiveBatch.AddVertex(p1.ToCCVector2() * PTMRatio , color.ToCCColor4B(), PrimitiveType.LineList);
            primitiveBatch.AddVertex(p2.ToCCVector2() * PTMRatio, color.ToCCColor4B(), PrimitiveType.LineList);
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
            stringData.Add(new StringData(x, y, format, objects, Color.White));
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
            primitiveBatch.Begin();
        }

        public void End()
        {
            primitiveBatch.End();

            //            var _batch = CCDrawManager.SharedDrawManager.SpriteBatch;
            //
            //            _batch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
            //
            //            for (int i = 0; i < stringData.Count; i++)
            //            {
            //                stringBuilder.Length = 0;
            //                stringBuilder.AppendFormat(stringData[i].S, stringData[i].Args);
            //                _batch.DrawString(spriteFont, stringBuilder, new Vector2(stringData[i].X, stringData[i].Y),
            //                    stringData[i].Color);
            //            }
            //
            //            _batch.End();

            stringData.Clear();
        }
    }
}