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
            CCDirector.SharedDirector.ReplaceScene(this);
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

    public class BaseClippingNodeTest : CCLayer
    {
        protected const int kTagStencilNode = 1;
        protected const int kTagClipperNode = 2;
        protected const int kTagContentNode = 3;

        public virtual void Setup()
        {
        }

        public virtual string title()
        {
            return "Clipping Demo";
        }

        public virtual string subtitle()
        {
            return "";
        }

        public override bool Init()
        {
            base.Init();

            CCSize s = CCDirector.SharedDirector.WinSize;

            CCSprite background = new CCSprite(TestResource.s_back3);
            background.AnchorPoint = new CCPoint(0.5f, 0.5f);
            background.Position = s.Center;
            AddChild(background, -1);

            var label = new CCLabelTTF(title(), "arial", 32);
            AddChild(label, 1);
            label.Position = (new CCPoint(s.Width / 2, s.Height - 50));

            string subtitle_ = subtitle();
            if (subtitle_.Length > 0)
            {
                var l = new CCLabelTTF(subtitle_, "arial", 16);
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

            Setup();

            return true;
        }

        public void restartCallback(object pSender)
        {
            CCScene s = new ClippingNodeTestScene();
            s.AddChild(ClippingNodeTestScene.restartTestAction());
            CCDirector.SharedDirector.ReplaceScene(s);
        }

        public void nextCallback(object pSender)
        {
            CCScene s = new ClippingNodeTestScene();
            s.AddChild(ClippingNodeTestScene.nextTestAction());
            CCDirector.SharedDirector.ReplaceScene(s);
        }

        public void backCallback(object pSender)
        {
            CCScene s = new ClippingNodeTestScene();
            s.AddChild(ClippingNodeTestScene.backTestAction());
            CCDirector.SharedDirector.ReplaceScene(s);
        }

        protected static void SetEnableRecursiveCascading(CCNode node, bool enable)
        {
            var rgba = node as ICCColor;

            if (rgba != null)
            {
                rgba.CascadeColorEnabled = true;
                rgba.CascadeOpacityEnabled = enable;
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
            return new CCClippingNode();
        }

        protected virtual CCNode Content()
        {
            return null;
        }

        public override string title()
        {
            return "Basic Test";
        }

        public override void Setup()
        {
            CCSize s = CCDirector.SharedDirector.WinSize;

            CCNode stencil = Stencil();
            stencil.Tag = kTagStencilNode;
            stencil.Position = new CCPoint(50, 50);

            CCClippingNode clipper = Clipper();
            clipper.Tag = kTagClipperNode;
            clipper.AnchorPoint = new CCPoint(0.5f, 0.5f);
            clipper.Position = new CCPoint(s.Width / 2 - 50, s.Height / 2 - 50);
            clipper.Stencil = stencil;
            AddChild(clipper);

            CCNode content = Content();
            content.Position = new CCPoint(50, 50);
            clipper.AddChild(content);
        }
    }

    internal class ShapeTest : BasicTest
    {
        public override string title()
        {
            return "Shape Basic Test";
        }

        public override string subtitle()
        {
            return "A DrawNode as stencil and Sprite as content";
        }

        protected override CCNode Stencil()
        {
            CCNode node = Shape();
            node.RunAction(this.ActionRotate());
            return node;
        }

        protected override CCNode Content()
        {
            CCNode node = Grossini();
            node.RunAction(ActionScale());
            return node;
        }
    }

    internal class ShapeInvertedTest : ShapeTest
    {
        public override string title()
        {
            return "Shape Inverted Basic Test";
        }

        public override string subtitle()
        {
            return "A DrawNode as stencil and Sprite as content, inverted";
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
        public override string title()
        {
            return "Sprite Basic Test";
        }

        public override string subtitle()
        {
            return "A Sprite as stencil and DrawNode as content";
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
        public override string title()
        {
            return "Sprite No Alpha Basic Test";
        }

        public override string subtitle()
        {
            return "A Sprite as stencil and DrawNode as content, no alpha";
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
        public override string title()
        {
            return "Sprite Inverted Basic Test";
        }

        public override string subtitle()
        {
            return "A Sprite as stencil and DrawNode as content, inverted";
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
        public override string title()
        {
            return "Nested Test";
        }

        public override string subtitle()
        {
            return "Nest 9 Clipping Nodes, max is usually 8";
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
                clipper.Position = new CCPoint(parent.ContentSize.Width / 2, parent.ContentSize.Height / 2);
                clipper.AlphaThreshold = 0.05f;
                clipper.RunAction(new CCRepeatForever(new CCRotateBy(i % 3 != 0 ? 1.33f : 1.66f, i % 2 != 0 ? 90 : -90)));
                parent.AddChild(clipper);

                CCNode stencil = new CCSprite(TestResource.s_pPathGrossini);
                stencil.Scale = 2.5f - (i * (2.5f / depth));
                stencil.AnchorPoint = new CCPoint(0.5f, 0.5f);
                stencil.Position = new CCPoint(clipper.ContentSize.Width / 2, clipper.ContentSize.Height / 2);
                stencil.Visible = false;
                stencil.RunAction(new CCSequence(new CCDelayTime(i), new CCShow()));
                clipper.Stencil = stencil;

                clipper.AddChild(stencil);

                parent = clipper;
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
            tranform = CCAffineTransform.Scale(tranform, target.Scale, target.Scale);

            m_pOuterClipper.ContentSize = CCAffineTransform.Transform(target.ContentSize, tranform);
            m_pOuterClipper.AnchorPoint = new CCPoint(0.5f, 0.5f);
            m_pOuterClipper.Position = ContentSize.Center;
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

            this.TouchEnabled = true;
        }

        public override string title()
        {
            return "Hole Demo";
        }

        public override string subtitle()
        {
            return "Touch/click to poke holes";
        }

        private void pokeHoleAtPoint(CCPoint point)
        {
            float scale = CCRandom.Float_0_1() * 0.2f + 0.9f;
            float rotation = CCRandom.Float_0_1() * 360f;

            CCSprite hole = new CCSprite("Images/hole_effect.png");
            hole.Position = point;
            hole.Rotation = rotation;
            hole.Scale = scale;

            m_pHoles.AddChild(hole);

            CCSprite holeStencil = new CCSprite("Images/hole_stencil.png");
            holeStencil.Position = point;
            holeStencil.Rotation = rotation;
            holeStencil.Scale = scale;

            m_pHolesStencil.AddChild(holeStencil);

            m_pOuterClipper.RunAction(new CCSequence(new CCScaleBy(0.05f, 0.95f), new CCScaleTo(0.125f, 1)));
        }

        public override void TouchesBegan(List<CCTouch> touches)
        {
            CCTouch touch = touches[0];
            CCPoint point =
                m_pOuterClipper.ConvertToNodeSpace(touch.Location);
            CCRect rect = new CCRect(0, 0, m_pOuterClipper.ContentSize.Width,
                                     m_pOuterClipper.ContentSize.Height);
            if (!rect.ContainsPoint(point)) return;
            this.pokeHoleAtPoint(point);
        }
    }

    internal class ScrollViewDemo : BaseClippingNodeTest
    {
        private bool m_bScrolling;
        private CCPoint m_lastPoint;

        public override string title()
        {
            return "Scroll View Demo";
        }

        public override string subtitle()
        {
            return "Move/drag to scroll the content";
        }

        public override void Setup()
        {
            var s = CCDirector.SharedDirector.WinSize;

            CCClippingNode clipper = new CCClippingNode();
            clipper.Tag = kTagClipperNode;
            clipper.ContentSize = new CCSize(200, 200);
            clipper.AnchorPoint = new CCPoint(0.5f, 0.5f);
            clipper.Position = ContentSize.Center;
            clipper.RunAction(new CCRepeatForever(new CCRotateBy(1, 45)));
            AddChild(clipper);

            CCDrawNode stencil = new CCDrawNode();
            CCPoint[] rectangle =
                {
                    new CCPoint(0, 0),
                    new CCPoint(clipper.ContentSize.Width, 0),
                    new CCPoint(clipper.ContentSize.Width, clipper.ContentSize.Height),
                    new CCPoint(0, clipper.ContentSize.Height),
                };

            CCColor4F white = new CCColor4F(1, 1, 1, 1);
            stencil.DrawPolygon(rectangle, 4, white, 0, white);
            clipper.Stencil = stencil;

            CCSprite content = new CCSprite(TestResource.s_back2);
            content.Tag = kTagContentNode;
            content.AnchorPoint = new CCPoint(0.5f, 0.5f);
            content.Position = clipper.ContentSize.Center;
            clipper.AddChild(content);

            m_bScrolling = false;

            TouchEnabled = true;
        }

        public override void TouchesBegan(List<CCTouch> touches)
        {
            CCTouch touch = touches[0];
            CCNode clipper = this.GetChildByTag(kTagClipperNode);
            CCPoint point = clipper.ConvertToNodeSpace(touch.Location);
            CCRect rect = new CCRect(0, 0, clipper.ContentSize.Width, clipper.ContentSize.Height);
            m_bScrolling = rect.ContainsPoint(point);
            m_lastPoint = point;
        }

        public override void TouchesMoved(List<CCTouch> touches)
        {
            if (!m_bScrolling)
            {
                return;
            }
            CCTouch touch = touches[0];
            CCNode clipper = this.GetChildByTag(kTagClipperNode);
            CCPoint point = clipper.ConvertToNodeSpace(touch.Location);
            CCPoint diff = point - m_lastPoint;
            CCNode content = clipper.GetChildByTag(kTagContentNode);
            content.Position = content.Position + diff;
            m_lastPoint = point;
        }

        public override void TouchesEnded(List<CCTouch> touches)
        {
            if (!m_bScrolling)
            {
                return;
            }
            m_bScrolling = false;
        }
    }
}