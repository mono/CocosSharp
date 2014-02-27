using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
    public class BaseDrawNodeTest : CCLayer
    {

        public BaseDrawNodeTest()
        {
            InitBaseDrawNodeTest();
        }

        public virtual void Setup()
        {
        }

        public virtual string title()
        {
            return "Draw Demo";
        }

        public virtual string subtitle()
        {
            return "";
        }

        private void InitBaseDrawNodeTest ()
        {

            CCSize s = CCDirector.SharedDirector.WinSize;

            var label = new CCLabelTtf(title(), "arial", 32);
            AddChild(label, 1);
            label.Position = (new CCPoint(s.Width / 2, s.Height - 50));

            string subtitle_ = subtitle();
            if (subtitle_.Length > 0)
            {
                var l = new CCLabelTtf(subtitle_, "arial", 16);
                AddChild(l, 1);
                l.Position = (new CCPoint(s.Width / 2, s.Height - 80));
            }

            var item1 = new CCMenuItemImage(TestResource.s_pPathB1, TestResource.s_pPathB2, backCallback);
            var item2 = new CCMenuItemImage(TestResource.s_pPathR1, TestResource.s_pPathR2, restartCallback);
            var item3 = new CCMenuItemImage(TestResource.s_pPathF1, TestResource.s_pPathF2, nextCallback);

            var menu = new CCMenu(item1, item2, item3);

            menu.Position = new CCPoint(0, 0);
            item1.Position = new CCPoint(s.Width / 2 - 100, 30);
            item2.Position = new CCPoint(s.Width / 2, 30);
            item3.Position = new CCPoint(s.Width / 2 + 100, 30);

            AddChild(menu, 1);

        }

        public void restartCallback(object pSender)
        {
            CCScene s = new DrawPrimitivesTestScene();
            s.AddChild(DrawPrimitivesTestScene.restartTestAction());
            CCDirector.SharedDirector.ReplaceScene(s);
        }

        public void nextCallback(object pSender)
        {
            CCScene s = new DrawPrimitivesTestScene();
            s.AddChild(DrawPrimitivesTestScene.nextTestAction());
            CCDirector.SharedDirector.ReplaceScene(s);
        }

        public void backCallback(object pSender)
        {
            CCScene s = new DrawPrimitivesTestScene();
            s.AddChild(DrawPrimitivesTestScene.backTestAction());
            CCDirector.SharedDirector.ReplaceScene(s);
        }
    }

    public class DrawPrimitivesWithRenderTextureTest : BaseDrawNodeTest
    {
        public DrawPrimitivesWithRenderTextureTest()
        {
            CCSize s = CCDirector.SharedDirector.WinSize;
            CCRenderTexture text = new CCRenderTexture((int)s.Width, (int)s.Height);

            CCDrawNode draw = new CCDrawNode();
            text.AddChild(draw, 10);
            text.Begin();
            // Draw polygons
            CCPoint[] points = new CCPoint[]
                {
                    new CCPoint(s.Height / 4, 0),
                    new CCPoint(s.Width, s.Height / 5),
                    new CCPoint(s.Width / 3 * 2, s.Height)
                };
            draw.DrawPolygon(points, points.Length, new CCColor4F(1, 0, 0, 0.5f), 4, new CCColor4F(0, 0, 1, 1));
            text.End();
            AddChild(text, 24);
        }
    }

    public class DrawPrimitivesTest : BaseDrawNodeTest
    {
        protected override void Draw()
        {
            base.Draw();

            CCApplication app = CCApplication.SharedApplication;
            CCSize s = CCDirector.SharedDirector.WinSize;

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
            CCDrawingPrimitives.DrawArc(new CCRect(200, 200, 100, 200), 0, 180,
                                        new CCColor4B(Microsoft.Xna.Framework.Color.AliceBlue));

            // draw an ellipse within rectangular region
            CCDrawingPrimitives.DrawEllipse(new CCRect(500, 200, 100, 200), new CCColor4B(255, 0, 0, 255));

            // draw an arc within rectangular region
            CCDrawingPrimitives.DrawPie(new CCRect(350, 0, 100, 100), 20, 100,
                                        new CCColor4B(Microsoft.Xna.Framework.Color.AliceBlue));

            // draw an arc within rectangular region
            CCDrawingPrimitives.DrawPie(new CCRect(347, -5, 100, 100), 120, 260,
                                        new CCColor4B(Microsoft.Xna.Framework.Color.Aquamarine));

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
        public DrawNodeTest()
        {
            InitDrawNodeTest();
        }

        private bool InitDrawNodeTest ()
        {

            CCSize s = CCDirector.SharedDirector.WinSize;

            CCDrawNode draw = new CCDrawNode();
            AddChild(draw, 10);

            // Draw 10 circles
            for (int i = 0; i < 10; i++)
            {
                draw.DrawDot(new CCPoint(s.Width / 2, s.Height / 2), 10 * (10 - i),
                             new CCColor4F(CCRandom.Float_0_1(), CCRandom.Float_0_1(), CCRandom.Float_0_1(), 1));
            }

            // Draw polygons
            CCPoint[] points = new CCPoint[]
                {
                    new CCPoint(s.Height / 4, 0),
                    new CCPoint(s.Width, s.Height / 5),
                    new CCPoint(s.Width / 3 * 2, s.Height)
                };
            draw.DrawPolygon(points, points.Length, new CCColor4F(1, 0, 0, 0.5f), 4, new CCColor4F(0, 0, 1, 1));

            // star poly (triggers buggs)
            {
                const float o = 80;
                const float w = 20;
                const float h = 50;
                CCPoint[] star = new CCPoint[]
                    {
                        new CCPoint(o + w, o - h), new CCPoint(o + w * 2, o), // lower spike
                        new CCPoint(o + w * 2 + h, o + w), new CCPoint(o + w * 2, o + w * 2), // right spike
                        //				{o +w, o+w*2+h}, {o,o+w*2},					// top spike
                        //				{o -h, o+w}, {o,o},							// left spike
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
                        new CCPoint(o, o), new CCPoint(o + w, o - h), new CCPoint(o + w * 2, o), // lower spike
                        new CCPoint(o + w * 2 + h, o + w), new CCPoint(o + w * 2, o + w * 2), // right spike
                        new CCPoint(o + w, o + w * 2 + h), new CCPoint(o, o + w * 2), // top spike
                        new CCPoint(o - h, o + w), // left spike
                    };

                draw.DrawPolygon(star, star.Length, new CCColor4F(1, 0, 0, 0.5f), 1, new CCColor4F(0, 0, 1, 1));
            }


            // Draw segment
            draw.DrawSegment(new CCPoint(20, s.Height), new CCPoint(20, s.Height / 2), 10, new CCColor4F(0, 1, 0, 1));

            draw.DrawSegment(new CCPoint(10, s.Height / 2), new CCPoint(s.Width / 2, s.Height / 2), 40,
                             new CCColor4F(1, 0, 1, 0.5f));

            return true;
        }
    }
}
