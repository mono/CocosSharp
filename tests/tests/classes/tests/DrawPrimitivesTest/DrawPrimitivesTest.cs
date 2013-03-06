using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class DrawPrimitivesTest : CCLayer
    {
        public override void Draw()
        {
            base.Draw();

            CCApplication app = CCApplication.SharedApplication;
            CCSize s = CCDirector.SharedDirector.WinSize;

            CCDrawingPrimitives.Begin();
            
            // draw a simple line
            CCDrawingPrimitives.DrawLine(new CCPoint(0, 0), new CCPoint(s.Width, s.Height), new CCColor4B(255, 255, 255, 255));

            // line: color, width, aliased
            CCDrawingPrimitives.DrawLine(new CCPoint(0, s.Height), new CCPoint(s.Width, 0), new CCColor4B(255, 0, 0, 255));

            // draw big point in the center
            CCDrawingPrimitives.DrawPoint(new CCPoint(s.Width / 2, s.Height / 2), 64, new CCColor4B(0, 0, 255, 128));

            // draw 4 small points
            CCPoint[] points = { new CCPoint(60, 60), new CCPoint(70, 70), new CCPoint(60, 70), new CCPoint(70, 60) };
            CCDrawingPrimitives.DrawPoints(points, 4, 4, new CCColor4B(0, 255, 255, 255));

            // draw a green circle with 10 segments
            CCDrawingPrimitives.DrawCircle(new CCPoint(s.Width / 2, s.Height / 2), 100, 0, 10, false, new CCColor4B(0, 255, 0, 255));

            // draw a green circle with 50 segments with line to center
            CCDrawingPrimitives.DrawCircle(new CCPoint(s.Width / 2, s.Height / 2), 50, ccMacros.CC_DEGREES_TO_RADIANS(90), 50, true, new CCColor4B(0, 255, 255, 255));

            // open yellow poly
            CCPoint[] vertices = { new CCPoint(0, 0), new CCPoint(50, 50), new CCPoint(100, 50), new CCPoint(100, 100), new CCPoint(50, 100) };
            CCDrawingPrimitives.DrawPoly(vertices, 5, false, new CCColor4B(255, 255, 0, 255));

            // filled poly
            CCPoint[] filledVertices = { new CCPoint(0, 120), new CCPoint(50, 120), new CCPoint(50, 170), new CCPoint(25, 200), new CCPoint(0, 170) };
            CCDrawingPrimitives.DrawSolidPoly(filledVertices, 5, new CCColor4B(128, 128, 255, 255));

            // closed purble poly
            CCPoint[] vertices2 = { new CCPoint(30, 130), new CCPoint(30, 230), new CCPoint(50, 200) };
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
            CCPoint[] vertices3 = { new CCPoint(60, 160), new CCPoint(70, 190), new CCPoint(100, 190), new CCPoint(90, 160) };
            CCDrawingPrimitives.DrawSolidPoly(vertices3, 4, new CCColor4B(255, 255, 0, 255));

            CCDrawingPrimitives.End();

        }
    }
}
