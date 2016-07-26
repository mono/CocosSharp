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

    public class DrawNodeWithRenderTextureTest : BaseDrawNodeTest
    {
        #region Setup content

        public override string Subtitle
        {
            get
            {
                return "RenderTarget CCDrawNode Visit";
            }
        }

        public override void OnEnter()
        {
            base.OnEnter(); 

            var windowSize = Layer.VisibleBoundsWorldspace.Size;
            CCRenderTexture text = new CCRenderTexture(windowSize,windowSize);
            text.Sprite.Position = windowSize.Center;
            AddChild(text.Sprite, 24);

            CCDrawNode draw = new CCDrawNode();

            // Draw polygons
            CCPoint[] points = new CCPoint[]
                {
                    new CCPoint(windowSize.Height / 4, 0),
                    new CCPoint(windowSize.Width, windowSize.Height / 5),
                    new CCPoint(windowSize.Width / 3 * 2, windowSize.Height)
                };
            draw.DrawPolygon(points, points.Length, new CCColor4F(1, 0, 0, 0.5f), 4, new CCColor4F(0, 0, 1, 1));
            draw.AnchorPoint = CCPoint.AnchorLowerLeft;

            text.Begin();
            draw.Visit();
            text.End();        
        }

        #endregion Setup content
    }

    public class DrawNodeWithRenderTextureTest1 : BaseDrawNodeTest
    {
        #region Setup content

        public override string Subtitle
        {
            get
            {
                return "Test RenderTarget Clipping with CCDrawNode as Child";
            }
        }

        public override void OnEnter()
        {
            base.OnEnter(); 

            CCDrawNode circle = new CCDrawNode();
            circle.DrawSolidCircle(new CCPoint(150.0f, 150.0f), 75.0f, new CCColor4B(255, 255, 255, 255));

            CCRenderTexture rtm = new CCRenderTexture(new CCSize(200.0f, 200.0f), new CCSize(200.0f, 200.0f), CCSurfaceFormat.Color, CCDepthFormat.Depth24Stencil8);

            rtm.BeginWithClear(CCColor4B.Orange);
            circle.Visit();
            rtm.End();

            // Make sure our children nodes get visited

            rtm.Sprite.Position = VisibleBoundsWorldspace.Center;
            rtm.Sprite.AnchorPoint = CCPoint.AnchorMiddle;


            AddChild(rtm.Sprite);
        }

        #endregion Setup content
    }

    public class DrawNodeWithRenderTextureTest2 : BaseDrawNodeTest
    {
        #region Setup content

        public override string Subtitle
        {
            get
            {
                return "Test RenderTarget Clipping with CCDrawNode Visit";
            }
        }


        public override void OnEnter()
        {
            base.OnEnter(); 

            CCDrawNode circle = new CCDrawNode();
            circle.DrawSolidCircle(new CCPoint(150.0f, 150.0f), 75.0f, new CCColor4B(255, 255, 255, 255));

            CCRenderTexture rtm = new CCRenderTexture(new CCSize(200.0f, 200.0f), new CCSize(200.0f, 200.0f), CCSurfaceFormat.Color, CCDepthFormat.Depth24Stencil8);
            rtm.BeginWithClear(CCColor4B.Orange);
            circle.Visit(); // Draw to rendertarget
            rtm.End();

            rtm.Sprite.Position = VisibleBoundsWorldspace.Center;
            rtm.Sprite.AnchorPoint = CCPoint.AnchorMiddle;

            var sprite = rtm.Sprite;
            sprite.AnchorPoint = CCPoint.AnchorMiddle;
            sprite.Position = VisibleBoundsWorldspace.Center;

            AddChild(sprite);
        }

        #endregion Setup content
    }


    public class DrawNodeWithRenderTextureTest3 : BaseDrawNodeTest
    {
        #region Setup content

        public override string Subtitle
        {
            get
            {
                return "Test RenderTarget Clipping with CCDrawNode Visit to Sprite";
            }
        }


        public override void OnEnter()
        {
            base.OnEnter(); 

            CCDrawNode circle = new CCDrawNode();
            circle.DrawSolidCircle(new CCPoint(150.0f, 150.0f), 75.0f, new CCColor4B(255, 255, 255, 255));

            CCRenderTexture rtm = new CCRenderTexture(new CCSize(200.0f, 200.0f), new CCSize(200.0f, 200.0f), CCSurfaceFormat.Color, CCDepthFormat.Depth24Stencil8);
            rtm.BeginWithClear(CCColor4B.Orange);
            circle.Visit(); // Draw to rendertarget
            rtm.End();

            // Create a new sprite from the render target texture
            var sprite = new CCSprite(rtm.Texture);
            sprite.Position = VisibleBoundsWorldspace.Center;


            AddChild(sprite);
        }

        #endregion Setup content
    }


    public class DrawPrimitivesTest : BaseDrawNodeTest
    {
        public DrawPrimitivesTest() : base()
        {
            //RenderDrawPrimTest ();
        }

        public override void OnEnter()
        {
            base.OnEnter();
            RenderDrawPrimTest();
        }
        void RenderDrawPrimTest()
        {
            CCSize size = Layer.VisibleBoundsWorldspace.Size;

            var visibleRect = VisibleBoundsWorldspace;

            CCDrawNode drawNode = new CCDrawNode ();

            // *NOTE* Using the Director.ContentScaleFactor for now until we work something out with the initialization
            // CCDrawPriitives should be able to do this converstion themselves.

            // draw a simple line
            // The default state is:
            // Line Width: 1
            // color: 255,255,255,255 (white, non-transparent)
            // Anti-Aliased
            //  glEnable(GL_LINE_SMOOTH);
            float lineWidth = 1.0f;
            CCColor4B lineColor = CCColor4B.White;
            drawNode.DrawLine(visibleRect.LeftBottom(), visibleRect.RightTop(), lineWidth);

            // line: color, width, aliased
            lineWidth = 5.0f ;
            lineColor = CCColor4B.Red;
            drawNode.DrawLine(visibleRect.LeftTop (), visibleRect.RightBottom (), lineWidth, lineColor);

            // TIP:
            // If you are going to use always thde same color or width, you don't
            // need to call it before every draw
            //

            // draw big point in the center

            drawNode.DrawSolidCircle(visibleRect.Center(), 64.0f, new CCColor4B(0, 0, 255, 128));

            // draw 4 small points
            CCPoint[] points = {new CCPoint(60, 60), new CCPoint(70, 70), new CCPoint(60, 70), new CCPoint(70, 60)};
            foreach (CCPoint point in points)
            {
                drawNode.DrawSolidCircle (point, 8.0f, new CCColor4B (0, 255, 255, 255));
            }

            // draw a green circle with 10 segments
            drawNode.DrawCircle(visibleRect.Center, 100.0f, 10, CCColor4B.Green);

            // draw an arc
            drawNode.DrawSolidArc(visibleRect.Center, 10.0f, 0, 180, CCColor4B.AliceBlue);

            // draw an ellipse within rectangular region
            drawNode.DrawEllipse(new CCRect(100, 100, 100, 200), 10.0f, CCColor4B.Red);


            // open yellow poly
            CCPoint[] vertices =
            {
                new CCPoint(0, 0), new CCPoint(50, 50), new CCPoint(100, 50), new CCPoint(100, 100),
                new CCPoint(50, 100)
            };
                
            // draw quad bezier path
            drawNode.DrawQuadBezier(
                new CCPoint(0, size.Height),
                visibleRect.Center,
                (CCPoint)visibleRect.Size,
                50,
                10.0f,
                new CCColor4B(255, 0, 255, 255));

            // draw cubic bezier path
            drawNode.DrawCubicBezier(
                visibleRect.Center,
                new CCPoint(size.Width / 2 + 30, size.Height / 2 + 50),
                new CCPoint(size.Width / 2 + 60, size.Height / 2 - 50),
                new CCPoint(size.Width, size.Height / 2), 100, 10.0f,
                new CCColor4B(255, 0, 255, 255));

            //draw a solid polygon
            CCPoint[] vertices3 =
            {
                new CCPoint(60, 160), new CCPoint(70, 190), new CCPoint(100, 190),
                new CCPoint(90, 160)
            };

            drawNode.DrawPolygon(vertices3, 4, new CCColor4F(1,1,0,1), 1.0f, new CCColor4F(1,1,0,1));
            
            AddChild(drawNode);
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
        CCGeometryNode geoBatch;

        public GeometryBatchTest1 () : base()
        {
            texture = CCTextureCache.SharedTextureCache.AddImage("Images/CyanSquare.png");
            //texture = CCTextureCache.SharedTextureCache.AddImage("Images/BackGround.png");

        }

        protected override void AddedToScene()
        {
            base.AddedToScene();

            CreateGeom();
        }

        void CreateGeom()
        {
            geoBatch = new CCGeometryNode();
            AddChild(geoBatch);
            var visibleRect = VisibleBoundsWorldspace;

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
        }

        public override string Title
        {
            get
            {
                return "Geometry Node";
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
        CCGeometryNode geoBatch = new CCGeometryNode();

        public GeometryBatchTest2 () : base()
        {
            texture = CCTextureCache.SharedTextureCache.AddImage("Images/CyanSquare.png");
            //texture = CCTextureCache.SharedTextureCache.AddImage("Images/BackGround.png");
            AddChild(geoBatch);

        }

        protected override void AddedToScene()
        {
            base.AddedToScene();

            var visibleRect = VisibleBoundsWorldspace;

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
        }

        public override void OnExit()
        {
            geoBatch.ClearInstances();
            base.OnExit();
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
				draw.DrawSolidCircle(s.Center, 10 * (10 - i),
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
            draw.DrawLine(new CCPoint(20, windowSize.Height), new CCPoint(20, windowSize.Height / 2), 10, new CCColor4F(0, 1, 0, 1), CCLineCap.Round);

            draw.DrawLine(new CCPoint(10, windowSize.Height / 2), new CCPoint(windowSize.Width / 2, windowSize.Height / 2), 40,
                new CCColor4F(1, 0, 1, 0.5f), CCLineCap.Round);

            CCSize size = Layer.VisibleBoundsWorldspace.Size;

            var visibleRect = VisibleBoundsWorldspace;

            // draw quad bezier path
            draw.DrawQuadBezier(new CCPoint(0, size.Height),
                visibleRect.Center,
                (CCPoint)visibleRect.Size,
                50, 3,
                new CCColor4B(255, 0, 255, 255));

            // draw cubic bezier path
            draw.DrawCubicBezier(visibleRect.Center,
                new CCPoint(size.Width / 2 + 30, size.Height / 2 + 50),
                new CCPoint(size.Width / 2 + 60, size.Height / 2 - 50),
                new CCPoint(size.Width, size.Height / 2), 100, 2, CCColor4B.Green);

            // draw an ellipse within rectangular region
            draw.DrawEllipse(new CCRect(100, 300, 100, 200), 8, CCColor4B.AliceBlue);

            var splinePoints = new List<CCPoint>();
            splinePoints.Add(new CCPoint(0, 0));
            splinePoints.Add(new CCPoint(50, 70));
            splinePoints.Add(new CCPoint(0, 140));
            splinePoints.Add(new CCPoint(100, 210));
            splinePoints.Add(new CCPoint(0, 280));
            splinePoints.Add(new CCPoint(150, 350));

            int numberOfSegments = 64;
            float tension = .05f;
            draw.DrawCardinalSpline(splinePoints, tension, numberOfSegments);

            draw.DrawSolidArc(
                pos: new CCPoint(350, windowSize.Height * 0.75f),
                radius: 100,
                startAngle: CCMathHelper.ToRadians(45),
                sweepAngle: CCMathHelper.Pi / 2, // this is in radians, clockwise
                color: CCColor4B.Aquamarine);


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
                draw.DrawSolidCircle(s.Center, 10 * (10 - i),
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
            draw.DrawLine(new CCPoint(20, windowSize.Height), new CCPoint(20, windowSize.Height / 2), 10, new CCColor4F(0, 1, 0, 1), CCLineCap.Round);

            draw.DrawLine(new CCPoint(10, windowSize.Height / 2), new CCPoint(windowSize.Width / 2, windowSize.Height / 2), 40,
                new CCColor4F(1, 0, 1, 0.5f), CCLineCap.Round);
        }

        #endregion Setup content
    }

    public class DrawNodeTestBlend : BaseDrawNodeTest
    {
        #region Setup content
        CCDrawNode triangleList1;
        CCDrawNode triangleList2;

        public DrawNodeTestBlend()
        {
            triangleList1 = new CCDrawNode();
            triangleList1.BlendFunc = CCBlendFunc.NonPremultiplied;

            AddChild(triangleList1, 10);

            triangleList2 = new CCDrawNode();
            // The default BlendFunc is CCBlendFunc.AlphaBlend
            //triangleList2.BlendFunc = CCBlendFunc.AlphaBlend;

            AddChild(triangleList2, 10);

        }

        public override string Subtitle
        {
            get
            {
                return "Using DrawTriangleList With BlendFunc";
            }
        }

        public override void OnEnter()
        {
            base.OnEnter(); 
            CCSize windowSize = Layer.VisibleBoundsWorldspace.Size;


            var line = new CCDrawNode();
            line.DrawLine(new CCPoint(0, windowSize.Height / 2), new CCPoint(windowSize.Width, windowSize.Height / 2), 10);
            AddChild(line, 0);

            byte alpha = 100; // 255 is full opacity

            var green = new CCColor4B(0, 255, 0, alpha);

            CCV3F_C4B[] verts = new CCV3F_C4B[] {
                new CCV3F_C4B( new CCPoint(0,0), green),
                new CCV3F_C4B( new CCPoint(30,60), green),
                new CCV3F_C4B( new CCPoint(60,0), green)
            };

            triangleList1.DrawTriangleList (verts);

            triangleList1.Position = windowSize.Center;
            triangleList1.PositionX -= windowSize.Width / 4;
            triangleList1.PositionY -= triangleList1.ContentSize.Center.Y;

            // Because the default BlendFunc of our DrawNode is AlphaBlend we need
            // to pass the colors as pre multiplied alpha.
            var greenPreMultiplied = green;
            greenPreMultiplied.G = (byte)(greenPreMultiplied.G * alpha / 255.0f);

            verts[0].Colors = greenPreMultiplied;
            verts[1].Colors = greenPreMultiplied;
            verts[2].Colors = greenPreMultiplied;

            triangleList2.DrawTriangleList (verts);

            triangleList2.Position = windowSize.Center;
            triangleList2.PositionX += windowSize.Width / 4;
            triangleList2.PositionY -= triangleList2.BoundingRect.Size.Height / 2;
        }

        #endregion Setup content
    }

    // Forum Post https://forums.xamarin.com/discussion/comment/145878/#Comment_145878
    public class DrawNodeTriangleVertex : BaseDrawNodeTest
    {
        
        CCDrawNode drawTriangles;
        CCDrawNode backGround;

        public DrawNodeTriangleVertex()
        {
            Color = new CCColor3B(127, 127, 127);
            Opacity = 255;
            drawTriangles = new CCDrawNode();
            drawTriangles.BlendFunc = CCBlendFunc.NonPremultiplied;

            AddChild(drawTriangles, 10);

            backGround = new CCDrawNode();

        }

        public override string Subtitle
        {
            get
            {
                return "Using AddTriangleVertex for Custom Geometry";
            }
        }

        #region Setup content
        public override void OnEnter()
        {
            base.OnEnter();

            CCSize windowSize = VisibleBoundsWorldspace.Size;

            var move = new CCMoveBy(4, new CCPoint(windowSize.Width / 2, 0));
            backGround.Position = VisibleBoundsWorldspace.Left();
            backGround.PositionX += windowSize.Width / 4;

            // Run background animation
            backGround.RepeatForever(move, move.Reverse());

            backGround.DrawSolidCircle(CCPoint.Zero, 220, CCColor4B.White);
            AddChild(backGround);


            var color = CCColor4B.Red;
            color.A = (byte)(255 * 0.3f);

            // Draw polygons
            //P1: 380:-160 P2: 200:-240 P3: 160:-420
            CCPoint[] points = new CCPoint[]
            {
                //P1: 380:-160 P2: 200:-240 P3: 160:-420
                new CCPoint(380,-160),
                new CCPoint(200,-240),
                new CCPoint(160,-420),
            };

            //P1: 160:-420 P2: 200:-520 P3: 360:-540
            CCPoint[] pointss = new CCPoint[]
            {
                new CCPoint(160,-420),
                new CCPoint(200,-520),
                new CCPoint(360,-540)
            };
            //P1: 360:-540 P2: 420:-600 P3: 520:-520
            CCPoint[] pointss2 = new CCPoint[]
            {
                new CCPoint(360,-540),
                new CCPoint(420,-600),
                new CCPoint(530,-520)
            };
            //P1: 520:-520 P2: 380:-160 P3: 160:-420
            CCPoint[] pointss3 = new CCPoint[]
            {
                new CCPoint(520,-520),
                new CCPoint(380,-160),
                new CCPoint(160,-420)
            };

            // P1: 160:-420 P2: 360:-540 P3: 520:-520
            CCPoint[] pointss4 = new CCPoint[]
            {
                new CCPoint(160,-420),
                new CCPoint(360,-540),
                new CCPoint(520,-520)
            };

            DrawSolidPolygon(points, color);
            DrawSolidPolygon(pointss, color);
            DrawSolidPolygon(pointss2, color);
            DrawSolidPolygon(pointss3, color);
            DrawSolidPolygon(pointss4, color);

            drawTriangles.Position = windowSize.Center;
            // Offset by the bounds of the polygons to more or less center it
            drawTriangles.PositionX -= 370;
            drawTriangles.PositionY += 440;
        }
        #endregion Setup content

        void DrawSolidPolygon(CCPoint[] points, CCColor4B color)
        {
            for (int i = 0; i < points.Length - 2; i++)
            {
                drawTriangles.AddTriangleVertex(new CCV3F_C4B(points[0], color)); 
                drawTriangles.AddTriangleVertex(new CCV3F_C4B(points[i + 1], color)); 
                drawTriangles.AddTriangleVertex(new CCV3F_C4B(points[i + 2], color)); 
            }

        }

    }

    // Forum Post https://forums.xamarin.com/discussion/comment/145878/#Comment_145878
    // Issue #287 : https://github.com/mono/CocosSharp/issues/287
    public class DrawNodeDrawPolygon : BaseDrawNodeTest
    {

        CCDrawNode drawTriangles;
        CCDrawNode backGround;

        public DrawNodeDrawPolygon()
        {
            Color = new CCColor3B(127, 127, 127);
            Opacity = 255;
            drawTriangles = new CCDrawNode();

            AddChild(drawTriangles, 10);

            backGround = new CCDrawNode();

        }

        public override string Subtitle
        {
            get
            {
                return "Using DrawPolygon - Issue #287";
            }
        }

        #region Setup content
        public override void OnEnter()
        {
            base.OnEnter();

            CCSize windowSize = VisibleBoundsWorldspace.Size;

            var move = new CCMoveBy(4, new CCPoint(windowSize.Width / 2, 0));
            backGround.Position = VisibleBoundsWorldspace.Left();
            backGround.PositionX += windowSize.Width / 4;

            // Run background animation
            backGround.RepeatForever(move, move.Reverse());

            backGround.DrawSolidCircle(CCPoint.Zero, 220, CCColor4B.White);
            AddChild(backGround);


            var color = CCColor4B.Red;
            var alpha = (byte)(255 * 0.3f);
            color.A = alpha;

            // Draw polygons
            //P1: 380:-160 P2: 200:-240 P3: 160:-420
            CCPoint[] points = new CCPoint[]
            {
                //P1: 380:-160 P2: 200:-240 P3: 160:-420
                new CCPoint(380,-160),
                new CCPoint(200,-240),
                new CCPoint(160,-420),

            };

            DrawSolidPolygon(points, color);

            //P1: 160:-420 P2: 200:-520 P3: 360:-540
            points[0] = new CCPoint(160, -420);
            points[1] = new CCPoint(200, -520);
            points[2] = new CCPoint(360, -540);

            DrawSolidPolygon(points, color);

            //P1: 360:-540 P2: 420:-600 P3: 520:-520
            points[0] = new CCPoint(360, -540);
            points[1] = new CCPoint(420, -600);
            points[2] = new CCPoint(530, -520);

            DrawSolidPolygon(points, color);

            //P1: 520:-520 P2: 380:-160 P3: 160:-420
            points[0] = new CCPoint(520, -520);
            points[1] = new CCPoint(380, -160);
            points[2] = new CCPoint(160, -420);

            DrawSolidPolygon(points, color);

            // P1: 160:-420 P2: 360:-540 P3: 520:-520
            points[0] = new CCPoint(160, -420);
            points[1] = new CCPoint(360, -540);
            points[2] = new CCPoint(520, -520);

            DrawSolidPolygon(points, color);

            drawTriangles.Position = windowSize.Center;
            // Offset by the bounds of the polygons to more or less center it
            drawTriangles.PositionX -= 370;
            drawTriangles.PositionY += 440;
        }
        #endregion Setup content

        void DrawSolidPolygon(CCPoint[] points, CCColor4B color)
        {
            drawTriangles.DrawPolygon(points, points.Length, color, 0, CCColor4B.Transparent);
        }

    }

}
