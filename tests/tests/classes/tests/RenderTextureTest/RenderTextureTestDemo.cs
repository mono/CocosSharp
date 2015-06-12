using System;
using System.Collections.Generic;
using CocosSharp;

namespace tests
{
    public class RenderTextureTestDemo : TestNavigationLayer
    {
        #region Properties

        public override string Title
        {
            get { return "Render Texture Test"; }
        }

        #endregion Properties


        #region Callbacks

        public override void RestartCallback(object sender)
        {
            CCScene s = new RenderTextureScene();
            s.AddChild(RenderTextureScene.restartTestCase());
            Director.ReplaceScene(s);
        }

        public override void NextCallback(object sender)
        {
            CCScene s = new RenderTextureScene();
            s.AddChild(RenderTextureScene.nextTestCase());
            Director.ReplaceScene(s);
        }

        public override void BackCallback(object sender)
        {
            CCScene s = new RenderTextureScene();
            s.AddChild(RenderTextureScene.backTestCase());
            Director.ReplaceScene(s);
        }

        #endregion Callbacks
    }


    public class RenderTextureDrawNode : RenderTextureTestDemo
    {

        CCDrawNode canvasNode;
        CCPoint lastPoint;
        CCMenu saveImageMenu;

        #region Properties

        public override string Title
        {
            get { return "Touch and drag on screen"; }
        }

        public override string Subtitle
        {
            get { return "CCDrawNode BoundingRect test\nPress 'Save Image' to create a snapshot of the render texture"; }
        }

        #endregion Properties


        #region Constructors

        public RenderTextureDrawNode()
        {
            canvasNode = new CCDrawNode();
            AddChild(canvasNode);

            // Save image menu
            CCMenuItemFont.FontSize = 16;
            CCMenuItemFont.FontName = "arial";
            CCMenuItem item1 = new CCMenuItemFont("Save Image", SaveImage);
            CCMenuItem item2 = new CCMenuItemFont("Clear", ClearImage);

            saveImageMenu = new CCMenu(item1, item2);
            AddChild(saveImageMenu);

            // Register for touch events
            var touchListener = new CCEventListenerTouchAllAtOnce();
            touchListener.OnTouchesEnded = OnTouchesEnded;
            touchListener.OnTouchesBegan = OnTouchesBegan;
            touchListener.OnTouchesMoved = OnTouchesMoved;
            AddEventListener(touchListener, this);
        }

        #endregion Constructors


        #region Setup content

        public override void OnEnter()
        {
            base.OnEnter();

            CCSize windowSize = Layer.VisibleBoundsWorldspace.Size;
            saveImageMenu.AlignItemsVertically();
            saveImageMenu.Position = new CCPoint(windowSize.Width - 80, windowSize.Height - 30);
            ClearImage(null);
        }

        #endregion Setup content

        void OnTouchesBegan(List<CCTouch> touches, CCEvent touchEvent)
        {
            var touch = touches[0];
            lastPoint = touch.Location;

        }

        void OnTouchesMoved(List<CCTouch> touches, CCEvent touchEvent)
        {
            var touch = touches[0];

            // Let's make sure that the two line points are not the same or we will have errors later on
            // with NaN on some platforms when calculating DrawNode's BoundingRect.
            if(lastPoint != touch.Location)
                canvasNode.DrawSegment(lastPoint, touch.Location, 1, new CCColor4F(CCColor4B.White));

            lastPoint = touch.Location;
        }

        void OnTouchesEnded(List<CCTouch> touches, CCEvent touchEvent)
        {
            if (touches.Count > 0)
            {

            }
        }

        void ClearImage(object sender)
        {
            canvasNode.Clear();
            RemoveChildByTag(spriteTag);

            Color = new CCColor3B((byte)(CCMacros.CCRandomBetween0And1() * 255), (byte)(CCMacros.CCRandomBetween0And1() * 255),
                (byte)(CCMacros.CCRandomBetween0And1() * 255));

            while (Opacity < 127)
                Opacity = (byte)(CCMacros.CCRandomBetween0And1() * 255);

        }

        const int spriteTag = 501;
        void SaveImage(object sender)
        {
            RemoveChildByTag(spriteTag);
            var renderSize = canvasNode.BoundingRect.Size;
            var rtm = new CCRenderTexture(renderSize, renderSize);

            rtm.BeginWithClear(CCColor4B.Green);
            canvasNode.Position -= canvasNode.BoundingRect.Origin;
            canvasNode.Visit();
            rtm.End();
            canvasNode.Position = CCPoint.Zero;

            rtm.Texture.IsAntialiased = true;
            rtm.Sprite.AnchorPoint = CCPoint.AnchorLowerLeft;
            rtm.Sprite.Opacity = 127;
            AddChild(rtm.Sprite, 50, spriteTag); ;
        }

    }


    public class RenderTextureDrawNodeVisit : RenderTextureTestDemo
    {

        CCDrawNode canvasNode;
        CCPoint lastPoint;
        CCMenu saveImageMenu;

        #region Properties

        public override string Title
        {
            get { return "Touch and drag on screen"; }
        }

        public override string Subtitle
        {
            get { return "Overriding Visit\nPress 'Save Image' to create a snapshot of the render texture"; }
        }

        #endregion Properties


        #region Constructors

        public RenderTextureDrawNodeVisit()
        {
            canvasNode = new CCDrawNode();
            AddChild(canvasNode);

            // Save image menu
            CCMenuItemFont.FontSize = 16;
            CCMenuItemFont.FontName = "arial";
            CCMenuItem item1 = new CCMenuItemFont("Save Image", SaveImage);
            CCMenuItem item2 = new CCMenuItemFont("Clear", ClearImage);

            saveImageMenu = new CCMenu(item1, item2);
            AddChild(saveImageMenu);

            // Register for touch events
            var touchListener = new CCEventListenerTouchAllAtOnce();
            touchListener.OnTouchesEnded = OnTouchesEnded;
            touchListener.OnTouchesBegan = OnTouchesBegan;
            touchListener.OnTouchesMoved = OnTouchesMoved;
            AddEventListener(touchListener, this);
        }

        #endregion Constructors


        #region Setup content

        public override void OnEnter()
        {
            base.OnEnter();

            CCSize windowSize = Layer.VisibleBoundsWorldspace.Size;
            saveImageMenu.AlignItemsVertically();
            saveImageMenu.Position = new CCPoint(windowSize.Width - 80, windowSize.Height - 30);
            ClearImage(null);
        }

        #endregion Setup content

        void OnTouchesBegan(List<CCTouch> touches, CCEvent touchEvent)
        {
            var touch = touches[0];
            lastPoint = touch.Location;

        }

        void OnTouchesMoved(List<CCTouch> touches, CCEvent touchEvent)
        {
            var touch = touches[0];

            // Let's make sure that the two line points are not the same or we will have errors later on
            // with NaN on some platforms when calculating DrawNode's BoundingRect.
            if (lastPoint != touch.Location)
                canvasNode.DrawSegment(lastPoint, touch.Location, 1, new CCColor4F(CCColor4B.White));

            lastPoint = touch.Location;
        }

        void OnTouchesEnded(List<CCTouch> touches, CCEvent touchEvent)
        {
            if (touches.Count > 0)
            {

            }
        }

        void ClearImage(object sender)
        {
            canvasNode.Clear();
            RemoveChildByTag(spriteTag);

            Color = new CCColor3B((byte)(CCMacros.CCRandomBetween0And1() * 255), (byte)(CCMacros.CCRandomBetween0And1() * 255),
                (byte)(CCMacros.CCRandomBetween0And1() * 255));

            while (Opacity < 127)
                Opacity = (byte)(CCMacros.CCRandomBetween0And1() * 255);

        }

        const int spriteTag = 501;
        void SaveImage(object sender)
        {
            RemoveChildByTag(spriteTag);
            var renderSize = canvasNode.BoundingRect.Size;
            var renderOrigin = canvasNode.BoundingRect.Origin;

            // here we calculate the translation of the rendering.  It should be the offset of BoundingRect Origin
            // this will then be passed to the Visit(ref CCAffineTransform) method.
            var translate = CCAffineTransform.Translate(canvasNode.AffineWorldTransform, -renderOrigin.X, -renderOrigin.Y);

            var rtm = new CCRenderTexture(renderSize, renderSize);

            rtm.BeginWithClear(CCColor4B.Green);
            canvasNode.Visit(ref translate);
            rtm.End();

            rtm.Texture.IsAntialiased = true;
            rtm.Sprite.AnchorPoint = CCPoint.AnchorLowerLeft;
            rtm.Sprite.Opacity = 127;
            AddChild(rtm.Sprite, 50, spriteTag); ;
        }

    }


}