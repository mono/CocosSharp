using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cocos2D;

namespace tests
{
    public class LayerTest1 : LayerTest
    {
        const int kTagLayer = 1;
        const int kCCMenuTouchPriority = -128;

        public override void OnEnter()
        {
            base.OnEnter();

            this.TouchEnabled = true;

            CCSize s = CCDirector.SharedDirector.WinSize;
            CCLayerColor layer = new CCLayerColor(new CCColor4B(0xFF, 0x00, 0x00, 0x80), 200, 200);

            layer.IgnoreAnchorPointForPosition = false;
            layer.Position = (new CCPoint(s.Width / 2, s.Height / 2));
            AddChild(layer, 1, kTagLayer);
        }

        public override string title()
        {
            return "ColorLayer resize (tap & move)";
        }

        public override void RegisterWithTouchDispatcher()
        {
            CCDirector.SharedDirector.TouchDispatcher.AddTargetedDelegate(this, kCCMenuTouchPriority + 1, true);
        }

        public void updateSize(CCPoint touchLocation)
        {
            CCSize s = CCDirector.SharedDirector.WinSize;
            CCSize newSize = new CCSize(Math.Abs(touchLocation.X - s.Width / 2) * 2, Math.Abs(touchLocation.Y - s.Height / 2) * 2);
            CCLayerColor l = (CCLayerColor)GetChildByTag(kTagLayer);
            l.ContentSize = newSize;
        }

        public override bool TouchBegan(CCTouch touche)
        {
            updateSize(touche.Location);
            return true;
        }

        public override void TouchMoved(CCTouch touche)
        {
            updateSize(touche.Location);
        }

        public override void TouchEnded(CCTouch touche)
        {
            updateSize(touche.Location);
        }
    }

    
    public class LayerTestCascadingOpacityA : LayerTest
    {
        const int kTagLayer = 1;

        public override void OnEnter()
        {
            base.OnEnter();

            var s = CCDirector.SharedDirector.WinSize;
            var layer1 = new CCLayerRGBA();

            var sister1 = new CCSprite("Images/grossinis_sister1.png");
            var sister2 = new CCSprite("Images/grossinis_sister2.png");
            var label = new CCLabelBMFont("Test", "fonts/bitmapFontTest.fnt");
    
            layer1.AddChild(sister1);
            layer1.AddChild(sister2);
            layer1.AddChild(label);
            this.AddChild( layer1, 0, kTagLayer);
    
            sister1.Position= new CCPoint( s.Width*1/3, s.Height/2);
            sister2.Position = new CCPoint( s.Width*2/3, s.Height/2);
            label.Position = new CCPoint(s.Width / 2, s.Height / 2);

            layer1.RunAction(
                new CCRepeatForever(
                    CCSequence.FromActions(
                        new CCFadeTo(4, 0),
                        new CCFadeTo(4, 255),
                        new CCDelayTime(1))
                    )
                );

            sister1.RunAction(
                new CCRepeatForever(
                    CCSequence.FromActions(
                        new CCFadeTo(2, 0),
                        new CCFadeTo(2, 255),
                        new CCFadeTo(2, 0),
                        new CCFadeTo(2, 255),
                        new CCDelayTime(1))
                    )
                );
    
    
            // Enable cascading in scene
            SetEnableRecursiveCascading(this, true);
        }

        public override string title()
        {
            return "LayerRGBA: cascading opacity";
        }
    }

    public class LayerTestCascadingOpacityB : LayerTest
    {
        const int kTagLayer = 1;

        public override void OnEnter()
        {
            base.OnEnter();

            var s = CCDirector.SharedDirector.WinSize;
            var layer1 = new CCLayerColor(new CCColor4B(192, 0, 0, 255), s.Width, s.Height / 2);
            layer1.CascadeColorEnabled = false;

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

            layer1.RunAction(
                new CCRepeatForever(
                    CCSequence.FromActions(
                        new CCFadeTo(4, 0),
                        new CCFadeTo(4, 255),
                        new CCDelayTime(1))
                    )
                );

            sister1.RunAction(
                new CCRepeatForever(
                    CCSequence.FromActions(
                        new CCFadeTo(2, 0),
                        new CCFadeTo(2, 255),
                        new CCFadeTo(2, 0),
                        new CCFadeTo(2, 255),
                        new CCDelayTime(1))
                    )
                );


            // Enable cascading in scene
            SetEnableRecursiveCascading(this, true);
        }

        public override string title()
        {
            return "CCLayerColor: cascading opacity";
        }
    }

    public class LayerTestCascadingOpacityC : LayerTest
    {
        const int kTagLayer = 1;

        public override void OnEnter()
        {
            base.OnEnter();

            var s = CCDirector.SharedDirector.WinSize;
            var layer1 = new CCLayerColor(new CCColor4B(192, 0, 0, 255), s.Width, s.Height / 2);
            layer1.CascadeColorEnabled = false;

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

            layer1.RunAction(
                new CCRepeatForever(
                    CCSequence.FromActions(
                        new CCFadeTo(4, 0),
                        new CCFadeTo(4, 255),
                        new CCDelayTime(1))
                    )
                );

            sister1.RunAction(
                new CCRepeatForever(
                    CCSequence.FromActions(
                        new CCFadeTo(2, 0),
                        new CCFadeTo(2, 255),
                        new CCFadeTo(2, 0),
                        new CCFadeTo(2, 255),
                        new CCDelayTime(1))
                    )
                );
        }

        public override string title()
        {
            return "CCLayerColor: non-cascading opacity";
        }
    }

    public class LayerTestCascadingColorA : LayerTest
    {
        const int kTagLayer = 1;

        public override void OnEnter()
        {
            base.OnEnter();

            var s = CCDirector.SharedDirector.WinSize;
            var layer1 = new CCLayerRGBA();

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

            layer1.RunAction(
                new CCRepeatForever(
                    CCSequence.FromActions(
                        new CCTintTo(6, 255, 0, 255),
                        new CCTintTo(6, 255, 255, 255),
                        new CCDelayTime(1))
                    )
                );

            sister1.RunAction(
                new CCRepeatForever(
                    CCSequence.FromActions(
                        new CCTintTo(2, 255, 255, 0),
                        new CCTintTo(2, 255, 255, 255),
                        new CCTintTo(2, 0, 255, 255),
                        new CCTintTo(2, 255, 255, 255),
                        new CCTintTo(2, 255, 0, 255),
                        new CCTintTo(2, 255, 255, 255),
                        new CCDelayTime(1))
                    )
                );

            // Enable cascading in scene
            SetEnableRecursiveCascading(this, true);
        }

        public override string title()
        {
            return "LayerRGBA: cascading color";
        }
    }

    public class LayerTestCascadingColorB : LayerTest
    {
        private const int kTagLayer = 1;

        public override void OnEnter()
        {
            base.OnEnter();

            var s = CCDirector.SharedDirector.WinSize;
            var layer1 = new CCLayerColor(new CCColor4B(192, 0, 0, 255), s.Width, s.Height / 2);
            layer1.CascadeColorEnabled = false;

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

            layer1.RunAction(
                new CCRepeatForever(
                    CCSequence.FromActions(
                        new CCTintTo(6, 255, 0, 255),
                        new CCTintTo(6, 255, 255, 255),
                        new CCDelayTime(1))
                    )
                );

            sister1.RunAction(
                new CCRepeatForever(
                    CCSequence.FromActions(
                        new CCTintTo(2, 255, 255, 0),
                        new CCTintTo(2, 255, 255, 255),
                        new CCTintTo(2, 0, 255, 255),
                        new CCTintTo(2, 255, 255, 255),
                        new CCTintTo(2, 255, 0, 255),
                        new CCTintTo(2, 255, 255, 255),
                        new CCDelayTime(1))
                    )
                );

            // Enable cascading in scene
            SetEnableRecursiveCascading(this, true);
        }

        public override string title()
        {
            return "CCLayerColor: cascading color";
        }
    }

    public class LayerTestCascadingColorC : LayerTest
    {
        const int kTagLayer = 1;

        public override void OnEnter()
        {
            base.OnEnter();

            var s = CCDirector.SharedDirector.WinSize;
            var layer1 = new CCLayerColor(new CCColor4B(192, 0, 0, 255), s.Width, s.Height / 2);
            layer1.CascadeColorEnabled = false;

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

            layer1.RunAction(
                new CCRepeatForever(
                    CCSequence.FromActions(
                        new CCTintTo(6, 255, 0, 255),
                        new CCTintTo(6, 255, 255, 255),
                        new CCDelayTime(1))
                    )
                );

            sister1.RunAction(
                new CCRepeatForever(
                    CCSequence.FromActions(
                        new CCTintTo(2, 255, 255, 0),
                        new CCTintTo(2, 255, 255, 255),
                        new CCTintTo(2, 0, 255, 255),
                        new CCTintTo(2, 255, 255, 255),
                        new CCTintTo(2, 255, 0, 255),
                        new CCTintTo(2, 255, 255, 255),
                        new CCDelayTime(1))
                    )
                );
        }

        public override string title()
        {
            return "CCLayerColor: non-cascading color";
        }
    }
}
