using System;
using System.Collections.Generic;
using CocosSharp;

namespace tests.Clipping
{

    public class ClippingNodeTestScene : TestScene
    {
        private static int sceneIdx = -1;
        private static int MAX_LAYER = 8;

        public override void runThisTest()
        {
            CCLayer pLayer = nextTestAction();
            AddChild(pLayer);
            Director.ReplaceScene(this);
        }

        public static CCLayer createTestLayer(int nIndex)
        {
            switch (nIndex)
            {
                case 0:
                    return new ScrollViewDemo();
                case 1:
                    return new HoleDemo();
                case 2:
                    return new ShapeTest();
                case 3:
                    return new ShapeInvertedTest();
                case 4:
                    return new SpriteTest();
                case 5:
                    return new SpriteNoAlphaTest();
                case 6:
                    return new SpriteInvertedTest();
                case 7:
                    return new NestedTest();
            }
            return null;
        }

        protected override void NextTestCase()
        {
            nextTestAction();
        }

        protected override void PreviousTestCase()
        {
            backTestAction();
        }

        protected override void RestTestCase()
        {
            restartTestAction();
        }

        public static CCLayer nextTestAction()
        {
            sceneIdx++;
            sceneIdx = sceneIdx % MAX_LAYER;
            CCLayer pLayer = createTestLayer(sceneIdx);
            return pLayer;
        }

        public static CCLayer backTestAction()
        {
            sceneIdx--;
            int total = MAX_LAYER;
            if (sceneIdx < 0)
                sceneIdx += total;
            CCLayer pLayer = createTestLayer(sceneIdx);
            return pLayer;
        }

        public static CCLayer restartTestAction()
        {
            CCLayer pLayer = createTestLayer(sceneIdx);
            return pLayer;
        }
    }

    public class BaseClippingNodeTest : TestNavigationLayer
    {
        protected const int kTagStencilNode = 1;
        protected const int kTagClipperNode = 2;
        protected const int kTagContentNode = 3;

		CCSprite background;

        public virtual void Setup()
        {
        }

        public override string Title
        {
            get
            {
                return "Clipping Demo";
            }
        }

        public override string Subtitle
        {
            get
            {
                return base.Subtitle;
            }
        }

        public BaseClippingNodeTest() : base()
        {
			background = new CCSprite(TestResource.s_back3);
			background.AnchorPoint = new CCPoint(0.5f, 0.5f);
			AddChild(background, -1);

        }

		public override void OnEnter()
		{
            Setup();

            base.OnEnter(); 
            var windowSize = Layer.VisibleBoundsWorldspace.Size;
			var s = windowSize;

			background.Position = s.Center;
		}

        public override void RestartCallback(object sender)
        {
            base.RestartCallback(sender);

            CCScene s = new ClippingNodeTestScene();
            s.AddChild(ClippingNodeTestScene.restartTestAction());
            Director.ReplaceScene(s);
        }

        public override void NextCallback(object sender)
        {
            base.BackCallback(sender);

            CCScene s = new ClippingNodeTestScene();
            s.AddChild(ClippingNodeTestScene.nextTestAction());
            Director.ReplaceScene(s);
        }

        public override void BackCallback(object sender)
        {
            base.BackCallback(sender);

            CCScene s = new ClippingNodeTestScene();
            s.AddChild(ClippingNodeTestScene.backTestAction());
            Director.ReplaceScene(s);
        }

        protected static void SetEnableRecursiveCascading(CCNode node, bool enable)
        {
			var rgba = node;

            if (rgba != null)
            {
                rgba.IsColorCascaded = true;
                rgba.IsOpacityCascaded = enable;
            }

            if (node.ChildrenCount > 0)
            {
                foreach (CCNode children in node.Children)
                {
                    SetEnableRecursiveCascading(children, enable);
                }
            }
        }
    }

    public class BasicTest : BaseClippingNodeTest
    {
        CCNode stencil;
        CCClippingNode clipper;
        CCNode content;

        protected virtual CCAction ActionRotate()
        {
            return new CCRepeatForever(new CCRotateBy(1.0f, 90.0f));
        }

        protected virtual CCAction ActionScale()
        {
            var scale = new CCScaleBy(1.33f, 1.5f);
            return new CCRepeatForever(new CCSequence(scale, scale.Reverse()));
        }

        protected virtual CCDrawNode Shape()
        {
            var shape = new CCDrawNode();
            var triangle = new CCPoint[3];
            triangle[0] = new CCPoint(-100, -100);
            triangle[1] = new CCPoint(100, -100);
            triangle[2] = new CCPoint(0, 100);

            shape.DrawPolygon(triangle, 3, new CCColor4F(0, 1, 0, 1), 0, new CCColor4F(0, 1, 0, 1));

            return shape;
        }

        protected virtual CCSprite Grossini()
        {
            var grossini = new CCSprite(TestResource.s_pPathGrossini);
            grossini.Scale = 1.5f;
            return grossini;
        }

        protected virtual CCNode Stencil()
        {
            return null;
        }

        protected virtual CCClippingNode Clipper()
        {
			return new CCClippingNode() { Tag = kTagClipperNode };
        }

        protected virtual CCNode Content()
        {
            return null;
        }

        public override string Title
        {
            get 
            {
                return "Basic Test";
            }
        }

        public override void Setup()
        {

            stencil = Stencil();
            stencil.Tag = kTagStencilNode;
            stencil.Position = new CCPoint(50, 50);

            CCClippingNode clipper = Clipper();
            clipper.AnchorPoint = new CCPoint(0.5f, 0.5f);
            clipper.Stencil = stencil;
            AddChild(clipper);

			content = Content();
			content.Tag = kTagContentNode;
            content.Position = new CCPoint(50, 50);
            clipper.AddChild(content);
        }

		public override void OnEnter()
		{
			base.OnEnter(); CCSize windowSize = Layer.VisibleBoundsWorldspace.Size;

            stencil.RunAction(this.ActionRotate());
            content.RunAction (this.ActionScale());

			var s = windowSize;
			this[kTagClipperNode].Position = new CCPoint(s.Width / 2 - 50, s.Height / 2 - 50);

		}
    }

    internal class ShapeTest : BasicTest
    {
        public override string Title
        {
            get
            {
                return "Shape Basic Test";
            }
        }

        public override string Subtitle
        {
            get
            {
                return "A DrawNode as stencil and Sprite as content";
            }
        }

        protected override CCNode Stencil()
        {
            CCNode node = Shape();
            return node;
        }

        protected override CCNode Content()
        {
            CCNode node = Grossini();
            return node;
        }
    }

    internal class ShapeInvertedTest : ShapeTest
    {
        public override string Title
        {
            get
            {
                return "Shape Inverted Basic Test";
            }
        }

        public override string Subtitle
        {
            get
            {
                return "A DrawNode as stencil and Sprite as content, inverted";
            }
        }

        protected override CCClippingNode Clipper()
        {
            CCClippingNode clipper = base.Clipper();
            clipper.Inverted = true;
            return clipper;
        }
    }

    internal class SpriteTest : ShapeTest
    {
        public override string Title
        {
            get
            {
                return "Sprite Basic Test";
            }
        }

        public override string Subtitle
        {
            get
            {
                return "A Sprite as stencil and DrawNode as content";
            }
        }

        protected override CCNode Stencil()
        {
            CCNode node = Grossini();
            node.RunAction(ActionRotate());
            return node;
        }

        protected override CCClippingNode Clipper()
        {
            CCClippingNode clipper = base.Clipper();
            clipper.AlphaThreshold = 0.05f;
            return clipper;
        }

        protected override CCNode Content()
        {
            CCNode node = Shape();
            node.RunAction(ActionScale());
            return node;
        }
    }

    internal class SpriteNoAlphaTest : SpriteTest
    {
        public override string Title
        {
            get
            {
                return "Sprite No Alpha Basic Test";
            }
        }

        public override string Subtitle
        {
            get
            {
                return "A Sprite as stencil and DrawNode as content, no alpha";
            }
        }

        protected override CCClippingNode Clipper()
        {
            CCClippingNode clipper = base.Clipper();
            clipper.AlphaThreshold = 1;
            return clipper;
        }
    }

    internal class SpriteInvertedTest : SpriteTest
    {
        public override string Title
        {
            get
            {
                return "Sprite Inverted Basic Test";
            }
        }

        public override string Subtitle
        {
            get
            {
                return "A Sprite as stencil and DrawNode as content, inverted";
            }
        }

        protected override CCClippingNode Clipper()
        {
            CCClippingNode clipper = base.Clipper();
            clipper.AlphaThreshold = 0.05f;
            clipper.Inverted = true;
            return clipper;
        }
    }

    internal class NestedTest : BaseClippingNodeTest
    {
        public override string Title
        {
            get
            {
                return "Nested Test";
            }
        }

        public override string Subtitle
        {
            get
            {
                return "Nest 9 Clipping Nodes, max is usually 8";
            }
        }

        public override void Setup()
        {
            const int depth = 9;

            CCNode parent = this;

            for (int i = 0; i < depth; i++)
            {

                int size = 225 - i * (225 / (depth * 2));

                CCClippingNode clipper = new CCClippingNode();
                clipper.ContentSize = new CCSize(size, size);
                clipper.AnchorPoint = new CCPoint(0.5f, 0.5f);
				clipper.Position = parent.ContentSize.Center;
                clipper.AlphaThreshold = 0.05f;
                clipper.RunAction(new CCRepeatForever(new CCRotateBy(i % 3 != 0 ? 1.33f : 1.66f, i % 2 != 0 ? 90 : -90)));
                parent.AddChild(clipper);

                CCNode stencil = new CCSprite(TestResource.s_pPathGrossini);
                stencil.Scale = 2.5f - (i * (2.5f / depth));
                stencil.AnchorPoint = new CCPoint(0.5f, 0.5f);
				stencil.Position = clipper.ContentSize.Center;
                stencil.Visible = false;
                stencil.RunAction(new CCSequence(new CCDelayTime(i), new CCShow()));
                clipper.Stencil = stencil;

                clipper.AddChild(stencil);

                parent = clipper;
            }
        }

		public override void OnEnter()
		{
			base.OnEnter(); 

            CCSize windowSize = Layer.VisibleBoundsWorldspace.Size;

			foreach (var clipper in Children)
			{
				if (clipper is CCClippingNode)
				{
					PositionInWindow((CCClippingNode)clipper);
				}

			}
		}

		void PositionInWindow (CCClippingNode clipper)
		{
			clipper.Position = clipper.Parent.ContentSize.Center;

			foreach (var child in clipper.Children)
			{
				if (child is CCClippingNode)
				{
					var clipr = (CCClippingNode)child;
					clipr.Stencil.Position = clipr.ContentSize.Center;

					PositionInWindow(clipr);

				}

			}
		}
    }

    internal class HoleDemo : BaseClippingNodeTest
    {
        private CCClippingNode m_pOuterClipper;
        private CCNode m_pHoles;
        private CCNode m_pHolesStencil;

        public override void Setup()
        {
            CCSprite target = new CCSprite(TestResource.s_pPathBlock);
            target.AnchorPoint = CCPoint.Zero;
            target.Scale = 3;

			m_pOuterClipper = new CCClippingNode();

            CCAffineTransform tranform = CCAffineTransform.Identity;
			tranform = CCAffineTransform.ScaleCopy(tranform, target.ScaleX, target.ScaleY);

            m_pOuterClipper.ContentSize = CCAffineTransform.Transform(target.ContentSize, tranform);
            m_pOuterClipper.AnchorPoint = new CCPoint(0.5f, 0.5f);
            m_pOuterClipper.RunAction(new CCRepeatForever(new CCRotateBy(1, 45)));

            m_pOuterClipper.Stencil = target;

            CCClippingNode holesClipper = new CCClippingNode();
            holesClipper.Inverted = true;
            holesClipper.AlphaThreshold = 0.05f;

            holesClipper.AddChild(target);

            m_pHoles = new CCNode();

            holesClipper.AddChild(m_pHoles);

            m_pHolesStencil = new CCNode();

            holesClipper.Stencil = m_pHolesStencil;

            m_pOuterClipper.AddChild(holesClipper);

            this.AddChild(m_pOuterClipper);

			var listener = new CCEventListenerTouchAllAtOnce();
			listener.OnTouchesBegan = onTouchesBegan;

            AddEventListener(listener);    
        }

		public override void OnEnter()
		{
			base.OnEnter();

			m_pOuterClipper.Position = ContentSize.Center;

		}
        public override string Title
        {
            get
            {
                return "Hole Demo";
            }
        }

        public override string Subtitle
        {
            get
            {
                return "Touch/click to poke holes";
            }
        }

        private void pokeHoleAtPoint(CCPoint point)
        {
            float scale = CCRandom.Float_0_1() * 0.2f + 0.9f;
            float rotation = CCRandom.Float_0_1() * 360f;

            CCSprite hole = new CCSprite("Images/hole_effect.png");
            hole.Position = point - m_pHoles.BoundingBoxTransformedToWorld.Origin;
            hole.Rotation = rotation;
            hole.Scale = scale;

            m_pHoles.AddChild(hole);

            CCSprite holeStencil = new CCSprite("Images/hole_stencil.png");
            holeStencil.Position = point - m_pHoles.BoundingBoxTransformedToWorld.Origin;
            holeStencil.Rotation = rotation;
            holeStencil.Scale = scale;

            m_pHolesStencil.AddChild(holeStencil);

            m_pOuterClipper.RunAction(new CCSequence(new CCScaleBy(0.05f, 0.95f), new CCScaleTo(0.125f, 1)));
        }

		void onTouchesBegan(List<CCTouch> touches, CCEvent touchEvent)
        {
            CCTouch touch = touches[0];
            CCPoint point =
                m_pOuterClipper.Layer.ScreenToWorldspace(touch.LocationOnScreen);
            CCRect rect = m_pOuterClipper.BoundingBoxTransformedToWorld;
            if (!rect.ContainsPoint(point)) 
                return;

            this.pokeHoleAtPoint(point);
        }
    }

    internal class ScrollViewDemo : BaseClippingNodeTest
    {
        private bool m_bScrolling;
        private CCPoint m_lastPoint;

        public override string Title
        {
            get
            {
                return "Scroll View Demo";
            }
        }

        public override string Subtitle
        {
            get
            {
                return "Move/drag to scroll the content";
            }
        }

        public override void Setup()
        {
			CCClippingNode clipper = new CCClippingNode() { Tag = kTagClipperNode };
            clipper.ContentSize = new CCSize(200, 200);
            clipper.AnchorPoint = new CCPoint(0.5f, 0.5f);
            AddChild(clipper);

            CCDrawNode stencil = new CCDrawNode();
            CCPoint[] rectangle =
                {
                    new CCPoint(0, 0),
                    new CCPoint(clipper.ContentSize.Width, 0),
                    new CCPoint(clipper.ContentSize.Width, clipper.ContentSize.Height),
                    new CCPoint(0, clipper.ContentSize.Height),
                };

            CCColor4F white = new CCColor4F(0, 0, 0, 1);
            stencil.DrawPolygon(rectangle, 4, white, 0, white);
            clipper.Stencil = stencil;

            CCSprite content = new CCSprite(TestResource.s_back2);
            content.Tag = kTagContentNode;
            content.AnchorPoint = new CCPoint(0.5f, 0.5f);
            clipper.AddChild(content);

            content.RunAction(new CCRepeatForever(new CCRotateBy(1, 45)));

            m_bScrolling = false;

			// Register Touch Event
			var touchListener = new CCEventListenerTouchAllAtOnce();

			touchListener.OnTouchesBegan = onTouchesBegan;
			touchListener.OnTouchesMoved = onTouchesMoved;
			touchListener.OnTouchesEnded = onTouchesEnded;

            AddEventListener(touchListener);        
		}

		public override void OnEnter()
		{
			base.OnEnter(); 

			var clipper = this[kTagClipperNode];
			clipper.Position = ContentSize.Center;
			clipper[kTagContentNode].Position = clipper.ContentSize.Center;
		}

		void onTouchesBegan(List<CCTouch> touches, CCEvent touchEvent)
        {
            CCTouch touch = touches[0];
            CCNode clipper = this.GetChildByTag(kTagClipperNode);
            CCPoint point = clipper.Layer.ScreenToWorldspace(touch.LocationOnScreen);

            m_bScrolling = clipper.BoundingBoxTransformedToWorld.ContainsPoint(point);
            Console.WriteLine("Touched: " + point + "clipper.BB: " + clipper.BoundingBoxTransformedToWorld + " scrolling: " + m_bScrolling );
            m_lastPoint = point;
        }

		void onTouchesMoved(List<CCTouch> touches, CCEvent touchEvent)
        {
            if (!m_bScrolling)
            {
                return;
            }
            CCTouch touch = touches[0];
            CCNode clipper = this[kTagClipperNode];
            CCPoint point = clipper.Layer.ScreenToWorldspace(touch.LocationOnScreen);

            CCPoint diff = point - m_lastPoint;
            CCNode content = clipper[kTagContentNode];
            clipper.Position = clipper.Position + diff;
            m_lastPoint = point;
        }

		void onTouchesEnded(List<CCTouch> touches, CCEvent touchEvent)
        {
            if (!m_bScrolling)
            {
                return;
            }
            m_bScrolling = false;
        }
    }
}