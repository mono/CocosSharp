using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
    public class RendererTestScene : TestScene
    {
        static int sceneIdx = -1;
        static int MAX_LAYER = 0;

        public RendererTestScene () : base()
        {
            MAX_LAYER = rendererCreateFunctions.Length;
        }

        protected override void NextTestCase()
        {
            nextRendererAction();
        }
        protected override void PreviousTestCase()
        {
            backRendererAction();
        }
        protected override void RestTestCase()
        {
            restartRendererAction();
        }
        public static CCLayer nextRendererAction()
        {
            sceneIdx++;
            sceneIdx = sceneIdx % MAX_LAYER;

            CCLayer pLayer = CreateRendererLayer(sceneIdx);
            return pLayer;

        }

        public static CCLayer backRendererAction()
        {

            sceneIdx--;
            int total = MAX_LAYER;
            if (sceneIdx < 0)
                sceneIdx += total;

            CCLayer pLayer = CreateRendererLayer(sceneIdx);
            return pLayer;
        }

        public static CCLayer restartRendererAction()
        {
            CCLayer pLayer = CreateRendererLayer(sceneIdx);

            return pLayer;
        }

        static Func<CCLayer>[] rendererCreateFunctions =
        {
            () => new RendererSpriteLabelTest(),
                () => new NewSpriteTest(),
                () => new NewDrawNodeTest(),
                () => new RendererBufferOverflowTest(),
                () => new CaptureScreenTest(),
                () => new RendererSpriteLabelTest(),
        };

        public static CCLayer CreateRendererLayer(int index)
        {
            return rendererCreateFunctions[index]();
        }

        public override void runThisTest()
        {
            CCLayer pLayer = nextRendererAction();
            AddChild(pLayer);

            Scene.Director.ReplaceScene(this);
        }
    }

    public class RendererDemo : TestNavigationLayer
    {
        //protected:

        public RendererDemo()
        {

        }

        public override string Title
        {
            get
            {
                return "No title";
            }
        }


        public override string Subtitle
        {
            get
            {
                return string.Empty;
            }
        }


        public override void RestartCallback(object sender)
        {
            base.RestartCallback(sender);
            CCScene s = new RendererTestScene();
            s.AddChild(RendererTestScene.restartRendererAction());

            Director.ReplaceScene(s);
        }

        public override void NextCallback(object sender)
        {
            base.NextCallback(sender);

            CCScene s = new RendererTestScene();

            s.AddChild(RendererTestScene.nextRendererAction());

            Director.ReplaceScene(s);
        }

        public override void BackCallback(object sender)
        {
            base.BackCallback(sender);

            CCScene s = new RendererTestScene();

            s.AddChild(RendererTestScene.backRendererAction());

            Director.ReplaceScene(s);
        }

    }

    public class NewSpriteTest : RendererDemo
    {

        public NewSpriteTest()
        {
        }

        protected override void AddedToScene()
        {
            base.AddedToScene();

            var visibleRect = VisibleBoundsWorldspace;

            createSpriteTest();
            createNewSpriteTest();
        }

        void createSpriteTest()
        {
            var winSize = VisibleBoundsWorldspace.Size;

            var parent = new CCSprite("Images/grossini");
            parent.Position = new CCPoint(winSize.Width / 4, winSize.Height / 2);
            var child1 = new CCSprite("Images/grossinis_sister1.png");
            child1.Position = new CCPoint(0.0f, -20.0f);
            var child2 = new CCSprite("Images/grossinis_sister2.png");
            child2.Position = new CCPoint(20.0f, -20.0f);
            var child3 = new CCSprite("Images/grossinis_sister1.png");
            child3.Position = new CCPoint(40.0f, -20.0f);
            var child4 = new CCSprite("Images/grossinis_sister2.png");
            child4.Position = new CCPoint(60.0f, -20.0f);
            var child5 = new CCSprite("Images/grossinis_sister2.png");
            child5.Position = new CCPoint(80.0f, -20.0f);
            var child6 = new CCSprite("Images/grossinis_sister2.png");
            child6.Position = new CCPoint(100.0f, -20.0f);
            var child7 = new CCSprite("Images/grossinis_sister2.png");
            child7.Position = new CCPoint(120.0f, -20.0f);

            parent.AddChild(child1);
            parent.AddChild(child2);
            parent.AddChild(child3);
            parent.AddChild(child4);
            parent.AddChild(child5);
            parent.AddChild(child6);
            parent.AddChild(child7);
            AddChild(parent);
        }

        void createNewSpriteTest()
        {
            var winSize = VisibleBoundsWorldspace.Size;

            var parent = new CCSprite("Images/grossini.png");
            parent.Position = new CCPoint(winSize.Width * 2 / 3, winSize.Height / 2);
            var child1 = new CCSprite("Images/grossinis_sister1.png");
            child1.Position = new CCPoint(0.0f, -20.0f);
            var child2 = new CCSprite("Images/grossinis_sister2.png");
            child2.Position = new CCPoint(20.0f, -20.0f);
            var child3 = new CCSprite("Images/grossinis_sister1.png");
            child3.Position = new CCPoint(40.0f, -20.0f);
            var child4 = new CCSprite("Images/grossinis_sister2.png");
            child4.Position = new CCPoint(60.0f, -20.0f);
            var child5 = new CCSprite("Images/grossinis_sister2.png");
            child5.Position = new CCPoint(80.0f, -20.0f);
            var child6 = new CCSprite("Images/grossinis_sister2.png");
            child6.Position = new CCPoint(100.0f, -20.0f);
            var child7 = new CCSprite("Images/grossinis_sister2.png");
            child7.Position = new CCPoint(120.0f, -20.0f);

            parent.AddChild(child1);
            parent.AddChild(child2);
            parent.AddChild(child3);
            parent.AddChild(child4);
            parent.AddChild(child5);
            parent.AddChild(child6);
            parent.AddChild(child7);
            AddChild(parent);
        }

        public override string Title
        {
            get
            {
                return "Renderer";
            }
        }

        public override string Subtitle
        {
            get
            {
                return "SpriteTest";
            }
        }

    }

    public class NewDrawNodeTest : RendererDemo
    {

        public NewDrawNodeTest()
        {
        }

        protected override void AddedToScene()
        {
            base.AddedToScene();

            var s = VisibleBoundsWorldspace.Size;

            var parent = new CCNode();
            parent.Position = new CCPoint(s.Width / 2, s.Height / 2);
            AddChild(parent);

            var rectNode = new CCDrawNode();
            CCPoint[] rectangle = new CCPoint [] 
            {
                new CCPoint(-50, -50),
                new CCPoint(50, -50),
                new CCPoint(50, 50),
                new CCPoint(-50, 50)
            };

            var white = new CCColor4F(1, 1, 1, 1);
            rectNode.DrawPolygon(rectangle, 4, white, 1, white);
            parent.AddChild(rectNode);
        }


        public override string Title
        {
            get
            {
                return "Renderer";
            }
        }

        public override string Subtitle
        {
            get
            {
                return "DrawNode";
            }
        }

    }

    public class RendererBufferOverflowTest : RendererDemo
    {

        public RendererBufferOverflowTest()
        {
        }

        protected override void AddedToScene()
        {
            base.AddedToScene();

            var s = VisibleBoundsWorldspace.Size;

            var parent = new CCNode();
            parent.Position = new CCPoint(0, 0);
            AddChild(parent);

            for (int i = 0; i < 10000 / 3.9; ++i)
            {
                var sprite = new CCSprite("Images/grossini_dance_01.png");
                sprite.Scale = 0.1f;
                sprite.Position = new CCPoint(CCMacros.CCRandomBetween0And1() * s.Width, CCMacros.CCRandomBetween0And1() * s.Height);
                parent.AddChild(sprite);
            }
        }


        public override string Title
        {
            get
            {
                return "Renderer";
            }
        }

        public override string Subtitle
        {
            get
            {
                return "Buffer overflow";
            }
        }

    }

    public class CaptureScreenTest : RendererDemo
    {

        public CaptureScreenTest()
        {
        }

        protected override void AddedToScene()
        {
            base.AddedToScene();

            var s = VisibleBoundsWorldspace.Size;

            var left = new CCPoint (s.Width / 4, s.Height / 2);
            var right = new CCPoint (s.Width / 4 * 3, s.Height / 2);

            var sp1 = new CCSprite("Images/grossini.png");
            sp1.Position = new CCPoint(left);
            var move1 = new CCMoveBy(1, new CCPoint(s.Width / 2, 0));
            
            AddChild(sp1);
            sp1.RepeatForever(move1, move1.Reverse());
            
            
            var sp2 = new CCSprite("Images/grossinis_sister1.png");
            sp2.Position = new CCPoint(right);
            var move2 = new CCMoveBy(1, new CCPoint(-s.Width / 2, 0));
            AddChild(sp2);
            sp2.RepeatForever(move2, move2.Reverse());

            var label1 = new CCLabel("capture all", "fonts/arial", 24, CCLabelFormat.SpriteFont);
            var mi1 = new CCMenuItemLabel(label1, OnCapture);
            var menu = new CCMenu(mi1);
            AddChild(menu);
            menu.Position = new CCPoint(s.Width / 2, s.Height / 4);

        }

        CCRenderTexture target;
        int childTag = 1001;
        void OnCapture(object sender)
        {

            RemoveChildByTag(childTag);

            var windowSize = VisibleBoundsWorldspace.Size;

            // create a render texture, this is what we are going to draw into
            target = new CCRenderTexture(windowSize, windowSize,
                CCSurfaceFormat.Color,
                CCDepthFormat.None, CCRenderTargetUsage.PreserveContents);

            target.Sprite.Position = windowSize.Center;

            target.Sprite.AnchorPoint = CCPoint.AnchorMiddle;

            // begin drawing to the render texture
            target.BeginWithClear(CCColor4B.Blue);

            Layer.Visit();

            // finish drawing and return context back to the screen
            target.End();

            AddChild(target.Sprite, 0, childTag);

            target.Sprite.Scale = 0.25f;
        }


        public override string Title
        {
            get
            {
                return "Renderer";
            }
        }

        public override string Subtitle
        {
            get
            {
                return "Capture screen test, press the menu item to capture the screen";
            }
        }

    }

    public class RendererSpriteLabelTest : RendererDemo
    {


        class Button : CCNode
        {

            CCSprite buttonSprite;

            public event TriggeredHandler Triggered;
            // A delegate type for hooking up button triggered events
            public delegate void TriggeredHandler(object sender, EventArgs e);


            private Button()
            {
                AttachListener();
            }

            public Button(CCSprite sprite, CCLabel label)
                : this()
            {
                this.ContentSize = sprite.ScaledContentSize;
                sprite.AnchorPoint = CCPoint.AnchorLowerLeft;
                label.Position = sprite.ContentSize.Center;

                // Create the render texture to draw to.  It will be the size of the button background sprite
                var render = new CCRenderTexture(sprite.ContentSize, sprite.ContentSize);

                // Clear it to any background color you want
                render.BeginWithClear(CCColor4B.Transparent);

                // Render the background sprite to the render texture
                sprite.Visit();

                // Render the label to the render texture
                label.Visit();

                // End the rendering
                render.End();

                // Add the button sprite to this node so it can be rendered
                buttonSprite = render.Sprite;
                buttonSprite.AnchorPoint = CCPoint.AnchorMiddle;
                AddChild(this.buttonSprite);
            }

            void AttachListener()
            {
                // Register Touch Event
                var listener = new CCEventListenerTouchOneByOne();
                listener.IsSwallowTouches = true;

                listener.OnTouchBegan = OnTouchBegan;
                listener.OnTouchEnded = OnTouchEnded;
                listener.OnTouchCancelled = OnTouchCancelled;

                AddEventListener(listener, this);
            }

            bool touchHits(CCTouch touch)
            {
                var location = touch.Location;
                
                var area = buttonSprite.BoundingBox;
                return area.ContainsPoint(buttonSprite.WorldToParentspace(location));
                
            }

            bool OnTouchBegan(CCTouch touch, CCEvent touchEvent)
            {
                bool hits = touchHits(touch);
                if (hits)
                {
                    // undo the rotation that was applied by the action attached.
                    Rotation = 0;
                    scaleButtonTo(0.9f);
                }

                return hits;
            }

            void OnTouchEnded(CCTouch touch, CCEvent touchEvent)
            {
                bool hits = touchHits(touch);
                if (hits && Triggered != null)
                    Triggered(this, EventArgs.Empty);
                scaleButtonTo(1);
            }

            void OnTouchCancelled(CCTouch touch, CCEvent touchEvent)
            {
                scaleButtonTo(1);
            }

            void scaleButtonTo(float scale)
            {
                var action = new CCScaleTo(0.1f, scale);
                action.Tag = 900;
                StopAction(900);
                RunAction(action);
            }
        }

        public RendererSpriteLabelTest()
        {
        }

        protected override void AddedToScene()
        {
            base.AddedToScene();

            var winSize = VisibleBoundsWorldspace.Size;

            var moveTo = new CCMoveBy(1.0f, new CCPoint(30, 0));
            var moveBack = moveTo.Reverse();
            var rotateBy = new CCRotateBy(1.0f, 180);
            var scaleBy = new CCScaleTo(1.0f, -2.0f);
            var action = new CCSequence(moveTo, moveBack, rotateBy, scaleBy);

            var buttonNormal = new CCSprite("Images/animationbuttonnormal.png");
            var button1 = new Button(buttonNormal,
                new CCLabel("Click", "fonts/arial", 20, CCLabelFormat.SpriteFont));
            
            button1.Position = winSize.Center;
            button1.PositionX -= 100;
            button1.PositionY += 100;

            AddChild(button1);
            button1.RunAction(action);

            var button2 = new Button(buttonNormal,
                new CCLabel("Me", "fonts/arial", 20, CCLabelFormat.SpriteFont));
            button2.Position = winSize.Center;
            button2.PositionX += 100;
            button2.PositionY += 100;

            AddChild(button2);
            button2.RunAction(action);

            var button3 = new Button(buttonNormal,
                new CCLabel("Ple", "fonts/arial", 20, CCLabelFormat.SpriteFont));
            button3.Position = winSize.Center;
            button3.PositionX -= 100;
            button3.PositionY -= 100;

            AddChild(button3);
            button3.RunAction(action);

            var button4 = new Button(buttonNormal,
                new CCLabel("ase", "fonts/arial", 20, CCLabelFormat.SpriteFont));
            button4.Position = winSize.Center;
            button4.PositionX += 100;
            button4.PositionY -= 100;

            AddChild(button4);
            button4.RunAction(action);

        }


        public override string Title
        {
            get
            {
                return "Renderer";
            }
        }

        public override string Subtitle
        {
            get
            {
                return "Render Sprite Label Test";
            }
        }

    }


}
