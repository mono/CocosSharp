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

        public override void OnEnter()
        {
            base.OnEnter(); 

            var windowSize = Layer.VisibleBoundsWorldspace.Size;

            CCRenderTexture text = new CCRenderTexture(windowSize,windowSize);

            text.Begin();
            // Draw polygons
            CCPoint[] points = new CCPoint[]
            {
                new CCPoint(windowSize.Height / 4, 0),
                new CCPoint(windowSize.Width, windowSize.Height / 5),
                new CCPoint(windowSize.Width / 3 * 2, windowSize.Height)
            };

            CCDrawingPrimitives.Begin();
            CCDrawingPrimitives.DrawPoly (points, points.Length, true, true, new CCColor4F (1, 0, 0, 0.5f));
            CCDrawingPrimitives.End ();
            text.End();

            CCSprite polySprite = new CCSprite(text.Texture);
            polySprite.Position = windowSize.Center;
            AddChild (polySprite);
        }

        #endregion Setup content
    }

    public class DrawPrimitivesTest : BaseDrawNodeTest
    {
        protected override void Draw()
        {
            base.Draw();

            CCSize size = Layer.VisibleBoundsWorldspace.Size;

            var visibleRect = VisibleBoundsWorldspace;

            CCDrawingPrimitives.Begin();


			// *NOTE* Using the Director.ContentScaleFactor for now until we work something out with the initialization
			// CCDrawPriitives should be able to do this converstion themselves.

			// draw a simple line
			// The default state is:
			// Line Width: 1
			// color: 255,255,255,255 (white, non-transparent)
			// Anti-Aliased
			//  glEnable(GL_LINE_SMOOTH);
			CCDrawingPrimitives.LineWidth = 1 ;
            CCDrawingPrimitives.DrawLine(visibleRect.LeftBottom(), visibleRect.RightTop());

			// line: color, width, aliased
			CCDrawingPrimitives.LineWidth = 5 ;
			CCDrawingPrimitives.DrawColor = CCColor4B.Red;
            CCDrawingPrimitives.DrawLine(visibleRect.LeftTop(), visibleRect.RightBottom());

			// TIP:
			// If you are going to use always thde same color or width, you don't
			// need to call it before every draw
			//

			// draw big point in the center
			CCDrawingPrimitives.PointSize = 64 ;
			CCDrawingPrimitives.DrawColor = new CCColor4B(0, 0, 255, 128);
            CCDrawingPrimitives.DrawPoint(visibleRect.Center());

            // draw 4 small points
			CCPoint[] points = {new CCPoint(60, 60), new CCPoint(70, 70), new CCPoint(60, 70), new CCPoint(70, 60)};

			CCDrawingPrimitives.PointSize = 8 ;
			CCDrawingPrimitives.DrawColor = new CCColor4B(0, 255, 255, 255);
            CCDrawingPrimitives.DrawPoints(points);

            // draw a green circle with 10 segments
			CCDrawingPrimitives.LineWidth = 16 ;
			CCDrawingPrimitives.DrawColor = CCColor4B.Green;
            CCDrawingPrimitives.DrawCircle(visibleRect.Center, 100, 0, 10, false);


            // draw a green circle with 50 segments with line to center
			CCDrawingPrimitives.LineWidth = 2 ;
			CCDrawingPrimitives.DrawColor = new CCColor4B(0, 255, 255, 255);
            CCDrawingPrimitives.DrawCircle(visibleRect.Center, 50, CCMacros.CCDegreesToRadians(45), 50, true);


			// draw a pink solid circle with 50 segments
			CCDrawingPrimitives.LineWidth = 2 ;
			CCDrawingPrimitives.DrawColor = new CCColor4B(255, 0, 255, 255);
            CCDrawingPrimitives.DrawSolidCircle(visibleRect.Center + new CCPoint(140, 0), 40, CCMacros.CCDegreesToRadians(90), 50);


            // draw an arc within rectangular region
			CCDrawingPrimitives.LineWidth = 5 ;
            CCDrawingPrimitives.DrawArc(new CCRect(200, 100, 100, 200), 0, 180, CCColor4B.AliceBlue);

            // draw an ellipse within rectangular region
			CCDrawingPrimitives.DrawEllipse(new CCRect(100, 100, 100, 200), CCColor4B.Red);

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
			CCDrawingPrimitives.LineWidth = 10 ;
			CCDrawingPrimitives.DrawColor = CCColor4B.Yellow;
            CCDrawingPrimitives.DrawPoly(vertices, 5, false, new CCColor4B(255, 255, 0, 255));


			// filled poly
			CCDrawingPrimitives.LineWidth = 1 ;
			CCPoint[] filledVertices =
            {
                new CCPoint(0, 120), new CCPoint(50, 120), new CCPoint(50, 170),
                new CCPoint(25, 200), new CCPoint(0, 170)
            };
			CCDrawingPrimitives.DrawSolidPoly(filledVertices, new CCColor4F(0.5f, 0.5f, 1, 1 ));

			// closed purple poly
			CCDrawingPrimitives.LineWidth = 2 ;
			CCDrawingPrimitives.DrawColor = new CCColor4B(255, 0, 255, 255);
			CCPoint[] vertices2 = {new CCPoint(30, 130), new CCPoint(30, 230), new CCPoint(50, 200)};
			CCDrawingPrimitives.DrawPoly(vertices2, true);

            // draw quad bezier path
            CCDrawingPrimitives.DrawQuadBezier(new CCPoint(0, size.Height),
                visibleRect.Center,
                (CCPoint)visibleRect.Size,
                50,
                new CCColor4B(255, 0, 255, 255));

            // draw cubic bezier path
            CCDrawingPrimitives.DrawCubicBezier(visibleRect.Center,
                new CCPoint(size.Width / 2 + 30, size.Height / 2 + 50),
                new CCPoint(size.Width / 2 + 60, size.Height / 2 - 50),
                new CCPoint(size.Width, size.Height / 2), 100);

            //draw a solid polygon
            CCPoint[] vertices3 =
            {
                new CCPoint(60, 160), new CCPoint(70, 190), new CCPoint(100, 190),
                new CCPoint(90, 160)
            };
			CCDrawingPrimitives.DrawSolidPoly(vertices3, 4, new CCColor4F(1,1,0,1));

            CCDrawingPrimitives.End();
        }

		public override string Title
		{
			get
			{
				return "Draw Primitives";
			}
		}

		public override string Subtitle
		{
			get
			{
				return "Drawing Primitives. Use DrawNode instead";
			}
		}
    }

    public class GeometryBatchTest1 : BaseDrawNodeTest
    {

        CCTexture2D texture;

        public GeometryBatchTest1 () : base()
        {
            texture = CCTextureCache.SharedTextureCache.AddImage("Images/CyanSquare.png");
            //texture = CCTextureCache.SharedTextureCache.AddImage("Images/BackGround.png");

        }

        protected override void AddedToScene()
        {
            base.AddedToScene();
        }

        protected override void Draw()
        {
            base.Draw();

            var visibleRect = VisibleBoundsWorldspace;

            var geoBatch = new CCGeometryBatch();

            geoBatch.Begin();

            var item = geoBatch.CreateGeometryInstance(3, 3);

            var vertices = item.GeometryPacket.Vertices;

            var windowSize = VisibleBoundsWorldspace.Size;
            var packet = item.GeometryPacket;

            packet.Texture = texture;
            item.GeometryPacket = packet;
            // Draw polygons

            vertices[0].Colors = CCColor4B.White;
            vertices[1].Colors = CCColor4B.White;
            vertices[2].Colors = CCColor4B.White;

            // Texture coordinates use a normalzed value 0 to 1
            vertices[0].TexCoords.U = 0;
            vertices[0].TexCoords.V = 0;

            vertices[1].TexCoords.U = 1;
            vertices[1].TexCoords.V = 0;

            vertices[2].TexCoords.U = 1;
            vertices[2].TexCoords.V = 1;

            vertices[0].Vertices.X = 0;
            vertices[0].Vertices.Y = 0;

            vertices[1].Vertices.X = texture.PixelsWide;
            vertices[1].Vertices.Y = 0;

            vertices[2].Vertices.X = texture.PixelsWide;
            vertices[2].Vertices.Y = texture.PixelsHigh;

            item.GeometryPacket.Indicies = new int[] { 2, 1, 0 };

            var rotation = CCAffineTransform.Identity;
            rotation.Rotation = (float)Math.PI / 4.0f;
            rotation.Tx = windowSize.Center.X - texture.PixelsWide / 2;
            rotation.Ty = windowSize.Center.Y - texture.PixelsHigh / 2;

            item.InstanceAttributes.AdditionalTransform = rotation;

            geoBatch.End();
        }

        public override string Title
        {
            get
            {
                return "Geometry Batch";
            }
        }

        public override string Subtitle
        {
            get
            {
                return "Auto Clear of instances and transform";
            }
        }
    }

    public class GeometryBatchTest2 : BaseDrawNodeTest
    {

        CCTexture2D texture;
        CCGeometryBatch geoBatch = new CCGeometryBatch();

        public GeometryBatchTest2 () : base()
        {
            texture = CCTextureCache.SharedTextureCache.AddImage("Images/CyanSquare.png");
            //texture = CCTextureCache.SharedTextureCache.AddImage("Images/BackGround.png");

        }

        protected override void AddedToScene()
        {
            base.AddedToScene();

            var visibleRect = VisibleBoundsWorldspace;

            // We will not clear the primitives after creating them
            // This will allow us to keep drawing the same over and over.
            geoBatch.AutoClearInstances = false;

            geoBatch.Begin();

            var item = geoBatch.CreateGeometryInstance(3, 3);

            var vertices = item.GeometryPacket.Vertices;

            var windowSize = VisibleBoundsWorldspace.Size;
            var packet = item.GeometryPacket;

            packet.Texture = texture;
            item.GeometryPacket = packet;
            // Draw polygons

            vertices[0].Colors = CCColor4B.White;
            vertices[1].Colors = CCColor4B.White;
            vertices[2].Colors = CCColor4B.White;

            // Texture coordinates use a normalzed value 0 to 1
            vertices[0].TexCoords.U = 0;
            vertices[0].TexCoords.V = 0;

            vertices[1].TexCoords.U = 1;
            vertices[1].TexCoords.V = 0;

            vertices[2].TexCoords.U = 1;
            vertices[2].TexCoords.V = 1;

            vertices[0].Vertices.X = 0;
            vertices[0].Vertices.Y = 0;

            vertices[1].Vertices.X = texture.PixelsWide;
            vertices[1].Vertices.Y = 0;

            vertices[2].Vertices.X = texture.PixelsWide;
            vertices[2].Vertices.Y = texture.PixelsHigh;

            item.GeometryPacket.Indicies = new int[] { 2, 1, 0 };

            var rotation = CCAffineTransform.Identity;
            rotation.Rotation = (float)Math.PI / 4.0f;
            rotation.Tx = windowSize.Center.X - texture.PixelsWide / 2;
            rotation.Ty = windowSize.Center.Y - texture.PixelsHigh / 2;

            item.InstanceAttributes.AdditionalTransform = rotation;

            geoBatch.End();
        }

        public override void OnExit()
        {
            // We will clean the batch up here.
            geoBatch.ClearInstances();
        }

        protected override void Draw()
        {
            base.Draw();

            geoBatch.Draw();
        }

        public override string Title
        {
            get
            {
                return "Geometry Batch";
            }
        }

        public override string Subtitle
        {
            get
            {
                return "No Auto Clear of instances";
            }
        }
    }

    public class DrawNodeTest : BaseDrawNodeTest
    {
        #region Setup content
		CCDrawNode draw;

		public DrawNodeTest()
		{
			draw = new CCDrawNode();
			draw.BlendFunc = CCBlendFunc.NonPremultiplied;

			AddChild(draw, 10);

		}


        public override void OnEnter()
        {
            base.OnEnter(); 
            CCSize windowSize = Layer.VisibleBoundsWorldspace.Size;
            CCDrawNode draw = new CCDrawNode();
            AddChild(draw, 10);

			var s = windowSize;

            // Draw 10 circles
            for (int i = 0; i < 10; i++)
            {
				draw.DrawDot(s.Center, 10 * (10 - i),
                    new CCColor4F(CCRandom.Float_0_1(), CCRandom.Float_0_1(), CCRandom.Float_0_1(), 1));
            }

            // Draw polygons
            CCPoint[] points = new CCPoint[]
            {
                new CCPoint(windowSize.Height / 4, 0),
                new CCPoint(windowSize.Width, windowSize.Height / 5),
                new CCPoint(windowSize.Width / 3 * 2, windowSize.Height)
            };
            draw.DrawPolygon(points, points.Length, new CCColor4F(1.0f, 0, 0, 0.5f), 4, new CCColor4F(0, 0, 1, 1));
			
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


    public class DrawNodeTest1 : BaseDrawNodeTest
    {
        #region Setup content
        CCDrawNode draw;

        public DrawNodeTest1()
        {
            draw = new CCDrawNode();
            draw.BlendFunc = CCBlendFunc.NonPremultiplied;

            AddChild(draw, 10);

        }

        public override string Subtitle
        {
            get
            {
                return "Using DrawTriangleList user defined geometry";
            }
        }

        public override void OnEnter()
        {
            base.OnEnter(); 
            CCSize windowSize = Layer.VisibleBoundsWorldspace.Size;
            CCDrawNode draw = new CCDrawNode();
            AddChild(draw, 10);

            var s = windowSize;

            // Draw 10 circles
            for (int i = 0; i < 10; i++)
            {
                draw.DrawDot(s.Center, 10 * (10 - i),
                    new CCColor4F(CCRandom.Float_0_1(), CCRandom.Float_0_1(), CCRandom.Float_0_1(), 1));
            }

            // Draw polygons
            CCV3F_C4B[] points = new CCV3F_C4B[3];

            points[0].Colors = CCColor4B.Red;
            points[0].Colors.A = 127;

            points[1].Colors = CCColor4B.Green;
            points[1].Colors.A = 127;

            points[2].Colors = CCColor4B.Blue;
            points[2].Colors.A = 127;

            points[0].Vertices.X = windowSize.Height / 4;
            points[0].Vertices.Y = 0;

            points[1].Vertices.X = windowSize.Width;
            points[1].Vertices.Y = windowSize.Height / 5;

            points[2].Vertices.X = windowSize.Width / 3 * 2;
            points[2].Vertices.Y = windowSize.Height;

            draw.DrawTriangleList(points);

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
