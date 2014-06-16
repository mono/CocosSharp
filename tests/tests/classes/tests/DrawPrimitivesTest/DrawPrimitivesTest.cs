using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
    public class BaseDrawNodeTest : TestNavigationLayer
    {
        #region Properties

        public override string Title
        {
            get { return "Draw Demo"; }
        }

        #endregion Properties


        #region Constructors

        public BaseDrawNodeTest()
        {
        }

        #endregion Constructors


        #region Setup content

        public virtual void Setup()
        {
        }

        #endregion Setup content


        #region Callbacks

        public override void RestartCallback(object sender)
        {
            CCScene s = new DrawPrimitivesTestScene();
            s.AddChild(DrawPrimitivesTestScene.restartTestAction());
            Director.ReplaceScene(s);
        }

        public override void NextCallback(object sender)
        {
            CCScene s = new DrawPrimitivesTestScene();
            s.AddChild(DrawPrimitivesTestScene.nextTestAction());
            Director.ReplaceScene(s);
        }

        public override void BackCallback(object sender)
        {
            CCScene s = new DrawPrimitivesTestScene();
            s.AddChild(DrawPrimitivesTestScene.backTestAction());
            Director.ReplaceScene(s);
        }

        #endregion Callbacks
    }

    public class DrawPrimitivesWithRenderTextureTest : BaseDrawNodeTest
    {
        #region Setup content

        protected override void RunningOnNewWindow(CCSize windowSize)
        {
            base.RunningOnNewWindow(windowSize);

            CCRenderTexture text = new CCRenderTexture((int)windowSize.Width, (int)windowSize.Height);

            CCDrawNode draw = new CCDrawNode();
            text.AddChild(draw, 10);
            text.Begin();
            // Draw polygons
            CCPoint[] points = new CCPoint[]
            {
                new CCPoint(windowSize.Height / 4, 0),
                new CCPoint(windowSize.Width, windowSize.Height / 5),
                new CCPoint(windowSize.Width / 3 * 2, windowSize.Height)
            };
            draw.DrawPolygon(points, points.Length, new CCColor4F(1, 0, 0, 0.5f), 4, new CCColor4F(0, 0, 1, 1));
            text.End();
            AddChild(text, 24);
        }

        #endregion Setup content
    }

    public class DrawPrimitivesTest : BaseDrawNodeTest
    {
        protected override void Draw()
        {
            base.Draw();

            CCSize s = Director.WindowSizeInPoints;

            CCDrawingPrimitives.Begin();

            // draw a simple line
            CCDrawingPrimitives.DrawLine(new CCPoint(0, 0), new CCPoint(s.Width, s.Height),
                new CCColor4B(255, 255, 255, 255));

            // line: color, width, aliased
            CCDrawingPrimitives.DrawLine(new CCPoint(0, s.Height), new CCPoint(s.Width, 0),
                new CCColor4B(255, 0, 0, 255));

            // draw big point in the center
            CCDrawingPrimitives.DrawPoint(new CCPoint(s.Width / 2, s.Height / 2), 64, new CCColor4B(0, 0, 255, 128));

            // draw 4 small points
            CCPoint[] points = {new CCPoint(60, 60), new CCPoint(70, 70), new CCPoint(60, 70), new CCPoint(70, 60)};
            CCDrawingPrimitives.DrawPoints(points, 4, 4, new CCColor4B(0, 255, 255, 255));

            // draw a green circle with 10 segments
            CCDrawingPrimitives.DrawCircle(new CCPoint(s.Width / 2, s.Height / 2), 100, 0, 10, false,
                new CCColor4B(0, 255, 0, 255));

            // draw a green circle with 50 segments with line to center
            CCDrawingPrimitives.DrawCircle(new CCPoint(s.Width / 2, s.Height / 2), 50, CCMacros.CCDegreesToRadians(90),
                50, true, new CCColor4B(0, 255, 255, 255));


            // draw an arc within rectangular region
            CCDrawingPrimitives.DrawArc(new CCRect(200, 200, 100, 200), 0, 180, CCColor4B.AliceBlue);

            // draw an ellipse within rectangular region
            CCDrawingPrimitives.DrawEllipse(new CCRect(500, 200, 100, 200), new CCColor4B(255, 0, 0, 255));

            // draw an arc within rectangular region
            CCDrawingPrimitives.DrawPie(new CCRect(350, 0, 100, 100), 20, 100, CCColor4B.AliceBlue);

            // draw an arc within rectangular region
            CCDrawingPrimitives.DrawPie(new CCRect(347, -5, 100, 100), 120, 260, CCColor4B.Aquamarine);

            // open yellow poly
            CCPoint[] vertices =
            {
                new CCPoint(0, 0), new CCPoint(50, 50), new CCPoint(100, 50), new CCPoint(100, 100),
                new CCPoint(50, 100)
            };
            CCDrawingPrimitives.DrawPoly(vertices, 5, false, new CCColor4B(255, 255, 0, 255));

            // filled poly
            CCPoint[] filledVertices =
            {
                new CCPoint(0, 120), new CCPoint(50, 120), new CCPoint(50, 170),
                new CCPoint(25, 200), new CCPoint(0, 170)
            };
            CCDrawingPrimitives.DrawSolidPoly(filledVertices, 5, new CCColor4B(128, 128, 255, 255));

            // closed purble poly
            CCPoint[] vertices2 = {new CCPoint(30, 130), new CCPoint(30, 230), new CCPoint(50, 200)};
            CCDrawingPrimitives.DrawPoly(vertices2, 3, true, new CCColor4B(255, 0, 255, 255));

            // draw quad bezier path
            CCDrawingPrimitives.DrawQuadBezier(new CCPoint(0, s.Height),
                new CCPoint(s.Width / 2, s.Height / 2),
                new CCPoint(s.Width, s.Height),
                50,
                new CCColor4B(255, 0, 255, 255));

            // draw cubic bezier path
            CCDrawingPrimitives.DrawCubicBezier(new CCPoint(s.Width / 2, s.Height / 2),
                new CCPoint(s.Width / 2 + 30, s.Height / 2 + 50),
                new CCPoint(s.Width / 2 + 60, s.Height / 2 - 50),
                new CCPoint(s.Width, s.Height / 2), 100,
                new CCColor4B(255, 0, 255, 255));

            //draw a solid polygon
            CCPoint[] vertices3 =
            {
                new CCPoint(60, 160), new CCPoint(70, 190), new CCPoint(100, 190),
                new CCPoint(90, 160)
            };
            CCDrawingPrimitives.DrawSolidPoly(vertices3, 4, new CCColor4B(255, 255, 0, 255));

            CCDrawingPrimitives.End();
        }
    }

    public class DrawNodeTest : BaseDrawNodeTest
    {
        #region Setup content

        protected override void RunningOnNewWindow(CCSize windowSize)
        {
            base.RunningOnNewWindow(windowSize);
            CCDrawNode draw = new CCDrawNode();
            AddChild(draw, 10);

            // Draw 10 circles
            for (int i = 0; i < 10; i++)
            {
                draw.DrawDot(new CCPoint(windowSize.Width / 2, windowSize.Height / 2), 10 * (10 - i),
                    new CCColor4F(CCRandom.Float_0_1(), CCRandom.Float_0_1(), CCRandom.Float_0_1(), 1));
            }

            // Draw polygons
            CCPoint[] points = new CCPoint[]
            {
                new CCPoint(windowSize.Height / 4, 0),
                new CCPoint(windowSize.Width, windowSize.Height / 5),
                new CCPoint(windowSize.Width / 3 * 2, windowSize.Height)
            };
            draw.DrawPolygon(points, points.Length, new CCColor4F(1, 0, 0, 0.5f), 4, new CCColor4F(0, 0, 1, 1));

            // star poly (triggers buggs)
            {
                const float o = 80;
                const float w = 20;
                const float h = 50;
                CCPoint[] star = new CCPoint[]
                {
                    new CCPoint(o + w, o - h), new CCPoint(o + w * 2, o),                           // lower spike
                    new CCPoint(o + w * 2 + h, o + w), new CCPoint(o + w * 2, o + w * 2),           // right spike
                };

                draw.DrawPolygon(star, star.Length, new CCColor4F(1, 0, 0, 0.5f), 1, new CCColor4F(0, 0, 1, 1));
            }

            // star poly (doesn't trigger bug... order is important un tesselation is supported.
            {
                const float o = 180;
                const float w = 20;
                const float h = 50;
                var star = new CCPoint[]
                {
                    new CCPoint(o, o), new CCPoint(o + w, o - h), new CCPoint(o + w * 2, o),        // lower spike
                    new CCPoint(o + w * 2 + h, o + w), new CCPoint(o + w * 2, o + w * 2),           // right spike
                    new CCPoint(o + w, o + w * 2 + h), new CCPoint(o, o + w * 2),                   // top spike
                    new CCPoint(o - h, o + w), // left spike
                };

                draw.DrawPolygon(star, star.Length, new CCColor4F(1, 0, 0, 0.5f), 1, new CCColor4F(0, 0, 1, 1));
            }


            // Draw segment
            draw.DrawSegment(new CCPoint(20, windowSize.Height), new CCPoint(20, windowSize.Height / 2), 10, new CCColor4F(0, 1, 0, 1));

            draw.DrawSegment(new CCPoint(10, windowSize.Height / 2), new CCPoint(windowSize.Width / 2, windowSize.Height / 2), 40,
                new CCColor4F(1, 0, 1, 0.5f));
        }

        #endregion Setup content
    }
}
