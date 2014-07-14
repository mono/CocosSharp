using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
    public class LayerTest1 : LayerTest
    {
        const int kTagLayer = 1;
        const int kCCMenuTouchPriority = -128;

        public override void OnEnter()
        {
            base.OnEnter();

			var listener = new CCEventListenerTouchAllAtOnce();
			listener.OnTouchesBegan = onTouchesBegan;
			listener.OnTouchesMoved = onTouchesMoved;
			listener.OnTouchesEnded = onTouchesEnded;

			AddEventListener(listener);

            CCSize s = Scene.VisibleBoundsWorldspace.Size;

            CCLayerColor layer = new CCLayerColor(new CCColor4B(0xFF, 0x00, 0x00, 0x80), 200, 200);

            layer.IgnoreAnchorPointForPosition = false;
			layer.Position = s.Center;
            AddChild(layer, 1, kTagLayer);
        }

		public override string Title
		{
			get
			{
				return "ColorLayer resize (tap & move)";
			}
		}

        public void updateSize(CCPoint touchLocation)
        {
            CCSize s = Scene.VisibleBoundsWorldspace.Size;

            CCSize newSize = new CCSize(Math.Abs(touchLocation.X - s.Width / 2) * 2, Math.Abs(touchLocation.Y - s.Height / 2) * 2);
            CCLayerColor l = (CCLayerColor)GetChildByTag(kTagLayer);
            l.ContentSize = newSize;
        }

		void onTouchesBegan (List<CCTouch> touches, CCEvent touchEvent)
		{
			onTouchesMoved (touches, touchEvent);
		}
		void onTouchesMoved (List<CCTouch> touches, CCEvent touchEvent)
		{
			var touchLocation = touches[0].LocationOnScreen;
			updateSize (touchLocation);
		}
		void onTouchesEnded (List<CCTouch> touches, CCEvent touchEvent)
		{
			onTouchesMoved (touches, touchEvent);
		}
    }

    
    public class LayerTestCascadingOpacityA : LayerTest
    {
        const int kTagLayer = 1;

        public override void OnEnter()
        {
            base.OnEnter();

            var s = Scene.VisibleBoundsWorldspace.Size;
            var layer1 = new CCLayer();


			var sister1 = new CCSprite("Images/grossinis_sister1.png");
			var sister2 = new CCSprite("Images/grossinis_sister2.png");
            var label = new CCLabelBMFont("Test", "fonts/bitmapFontTest.fnt");
			// by default a CCLabelBMFont has IsColorModifiedByOpacity on by default if the 
			// texture atlas is PreMultipliedAlpha.  Label as used by Cocos2d-x by default has
			// this set to false.  Maybe this is a bug in Cocos2d-x?
			label.IsColorModifiedByOpacity = false;
    
            layer1.AddChild(sister1);
            layer1.AddChild(sister2);
            layer1.AddChild(label);
            this.AddChild( layer1, 0, kTagLayer);
    
            sister1.Position= new CCPoint( s.Width*1/3, s.Height/2);
            sister2.Position = new CCPoint( s.Width*2/3, s.Height/2);
            label.Position = new CCPoint(s.Width / 2, s.Height / 2);

			// Define our delay time
			var delay = new CCDelayTime (1);

			layer1.RepeatForever(
                        new CCFadeTo(4, 0),
                        new CCFadeTo(4, 255),
						delay
                );

			// We only have to define them once.
			var fadeTo11 = new CCFadeTo (2, 0);
			var fadeTo12 = new CCFadeTo (2, 255);

			sister1.RepeatForever(
						fadeTo11,
						fadeTo12,
						fadeTo11,
						fadeTo12,
						delay
                );
    
    
            // Enable cascading in scene
            SetEnableRecursiveCascading(this, true);
        }

		public override string Subtitle
		{
			get
			{
				return "Layer: cascading opacity";
			}
		}
    }

    public class LayerTestCascadingOpacityB : LayerTest
    {
        const int kTagLayer = 1;

        public override void OnEnter()
        {
            base.OnEnter();

            var s = Scene.VisibleBoundsWorldspace.Size;

            var layer1 = new CCLayerColor(new CCColor4B(192, 0, 0, 255), s.Width, s.Height / 2);
            layer1.IsColorCascaded = false;

            layer1.Position = new CCPoint(0, s.Height / 2);

			var sister1 = new CCSprite("Images/grossinis_sister1.png");
			var sister2 = new CCSprite("Images/grossinis_sister2.png");
            var label = new CCLabelBMFont("Test", "fonts/bitmapFontTest.fnt");
			// by default a CCLabelBMFont has IsColorModifiedByOpacity on by default if the 
			// texture atlas is PreMultipliedAlpha.  Label as used by Cocos2d-x by default has
			// this set to false.  Maybe this is a bug in Cocos2d-x?
			label.IsColorModifiedByOpacity = false;

            layer1.AddChild(sister1);
            layer1.AddChild(sister2);
            layer1.AddChild(label);
            this.AddChild(layer1, 0, kTagLayer);

            sister1.Position = new CCPoint(s.Width * 1 / 3, 0);
            sister2.Position = new CCPoint(s.Width * 2 / 3, 0);
            label.Position = new CCPoint(s.Width / 2, 0);

			// Define our delay time
			var delay = new CCDelayTime (1);

			layer1.RepeatForever(
				new CCFadeTo(4, 0),
				new CCFadeTo(4, 255),
				delay
			);

			// We only have to define them once.
			var fadeTo11 = new CCFadeTo (2, 0);
			var fadeTo12 = new CCFadeTo (2, 255);

			sister1.RepeatForever(
				fadeTo11,
				fadeTo12,
				fadeTo11,
				fadeTo12,
				delay
			);


            // Enable cascading in scene
            SetEnableRecursiveCascading(this, true);
        }

		public override string Subtitle
		{
			get
			{
				return "CCLayerColor: cascading opacity";
			}
		}
    }

    public class LayerTestCascadingOpacityC : LayerTest
    {
        const int kTagLayer = 1;

        public override void OnEnter()
        {
            base.OnEnter();

            var s = Scene.VisibleBoundsWorldspace.Size;

            var layer1 = new CCLayerColor(new CCColor4B(192, 0, 0, 255), s.Width, s.Height / 2);
            layer1.IsColorCascaded = false;

            layer1.Position = new CCPoint(0, s.Height / 2);

            var sister1 = new CCSprite("Images/grossinis_sister1.png");
            var sister2 = new CCSprite("Images/grossinis_sister2.png");
            var label = new CCLabelBMFont("Test", "fonts/bitmapFontTest.fnt");

            layer1.AddChild(sister1);
            layer1.AddChild(sister2);
            layer1.AddChild(label);
            this.AddChild(layer1, 0, kTagLayer);

            sister1.Position = new CCPoint(s.Width * 1 / 3, 0);
            sister2.Position = new CCPoint(s.Width * 2 / 3, 0);
            label.Position = new CCPoint(s.Width / 2, 0);

			// Define our delay time
			var delay = new CCDelayTime (1);

			layer1.RepeatForever(
				new CCFadeTo(4, 0),
				new CCFadeTo(4, 255),
				delay
			);

			// We only have to define them once.
			var fadeTo11 = new CCFadeTo (2, 0);
			var fadeTo12 = new CCFadeTo (2, 255);

			sister1.RepeatForever(
				fadeTo11,
				fadeTo12,
				fadeTo11,
				fadeTo12,
				delay
			);
        }

		public override string Subtitle
		{
			get
			{
				return "CCLayerColor: non-cascading opacity";
			}
		}
    }

    public class LayerTestCascadingColorA : LayerTest
    {
        const int kTagLayer = 1;

        public override void OnEnter()
        {
            base.OnEnter();

            var s = Scene.VisibleBoundsWorldspace.Size;
            var layer1 = new CCLayer();

            var sister1 = new CCSprite("Images/grossinis_sister1.png");
            var sister2 = new CCSprite("Images/grossinis_sister2.png");
            var label = new CCLabelBMFont("Test", "fonts/bitmapFontTest.fnt");

            layer1.AddChild(sister1);
            layer1.AddChild(sister2);
            layer1.AddChild(label);
            this.AddChild(layer1, 0, kTagLayer);

            sister1.Position = new CCPoint(s.Width * 1 / 3, s.Height / 2);
            sister2.Position = new CCPoint(s.Width * 2 / 3, s.Height / 2);
            label.Position = new CCPoint(s.Width / 2, s.Height / 2);

			// Define our delay time
			var delay = new CCDelayTime (1);

			layer1.RepeatForever (
                new CCTintTo(6, 255, 0, 255),
                new CCTintTo(6, 255, 255, 255),
				delay
                );

			sister1.RepeatForever (
                new CCTintTo(2, 255, 255, 0),
                new CCTintTo(2, 255, 255, 255),
                new CCTintTo(2, 0, 255, 255),
                new CCTintTo(2, 255, 255, 255),
                new CCTintTo(2, 255, 0, 255),
                new CCTintTo(2, 255, 255, 255),
				delay
                );

            // Enable cascading in scene
            SetEnableRecursiveCascading(this, true);
        }

		public override string Subtitle
		{
			get
			{
				return "Layer: cascading color";
			}
		}
    }

    public class LayerTestCascadingColorB : LayerTest
    {
        private const int kTagLayer = 1;

        public override void OnEnter()
        {
            base.OnEnter();

            var s = Scene.VisibleBoundsWorldspace.Size;

            var layer1 = new CCLayerColor(new CCColor4B(192, 0, 0, 255), s.Width, s.Height / 2);
            layer1.IsColorCascaded = false;

            layer1.Position = new CCPoint(0, s.Height / 2);

            var sister1 = new CCSprite("Images/grossinis_sister1.png");
            var sister2 = new CCSprite("Images/grossinis_sister2.png");
            var label = new CCLabelBMFont("Test", "fonts/bitmapFontTest.fnt");

            layer1.AddChild(sister1);
            layer1.AddChild(sister2);
            layer1.AddChild(label);
            this.AddChild(layer1, 0, kTagLayer);

            sister1.Position = new CCPoint(s.Width * 1 / 3, 0);
            sister2.Position = new CCPoint(s.Width * 2 / 3, 0);
            label.Position = new CCPoint(s.Width / 2, 0);

			// Define our delay time
			var delay = new CCDelayTime (1);

			layer1.RepeatForever (
                new CCTintTo(6, 255, 0, 255),
                new CCTintTo(6, 255, 255, 255),
				delay
                );

			sister1.RepeatForever (
                new CCTintTo(2, 255, 255, 0),
                new CCTintTo(2, 255, 255, 255),
                new CCTintTo(2, 0, 255, 255),
                new CCTintTo(2, 255, 255, 255),
                new CCTintTo(2, 255, 0, 255),
                new CCTintTo(2, 255, 255, 255),
				delay
                );

            // Enable cascading in scene
            SetEnableRecursiveCascading(this, true);
        }

		public override string Subtitle
		{
			get
			{
				return "CCLayerColor: cascading color";
			}
		}
    }

    public class LayerTestCascadingColorC : LayerTest
    {
        const int kTagLayer = 1;

        public override void OnEnter()
        {
            base.OnEnter();

            var s = Scene.VisibleBoundsWorldspace.Size;

            var layer1 = new CCLayerColor(new CCColor4B(192, 0, 0, 255), s.Width, s.Height / 2);
            layer1.IsColorCascaded = false;

            layer1.Position = new CCPoint(0, s.Height / 2);

            var sister1 = new CCSprite("Images/grossinis_sister1.png");
            var sister2 = new CCSprite("Images/grossinis_sister2.png");
            var label = new CCLabelBMFont("Test", "fonts/bitmapFontTest.fnt");

            layer1.AddChild(sister1);
            layer1.AddChild(sister2);
            layer1.AddChild(label);
            this.AddChild(layer1, 0, kTagLayer);

            sister1.Position = new CCPoint(s.Width * 1 / 3, 0);
            sister2.Position = new CCPoint(s.Width * 2 / 3, 0);
            label.Position = new CCPoint(s.Width / 2, 0);

			// Define our delay time
			var delay = new CCDelayTime (1);

			layer1.RepeatForever (
                new CCTintTo(6, 255, 0, 255),
                new CCTintTo(6, 255, 255, 255),
				delay
                );

			sister1.RepeatForever (
                new CCTintTo(2, 255, 255, 0),
                new CCTintTo(2, 255, 255, 255),
                new CCTintTo(2, 0, 255, 255),
                new CCTintTo(2, 255, 255, 255),
                new CCTintTo(2, 255, 0, 255),
                new CCTintTo(2, 255, 255, 255),
				delay
                );
        }

		public override string Title
		{
			get
			{
				return "CCLayerColor: non-cascading color";
			}
		}
    }
}
