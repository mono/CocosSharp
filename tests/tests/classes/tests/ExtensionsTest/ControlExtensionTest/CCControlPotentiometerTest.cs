using System;
using CocosSharp;

namespace tests.Extensions
{
    internal class CCControlPotentiometerTest : CCControlScene
    {
        private CCLabel DisplayValueLabel { get; set; }

        public CCControlPotentiometerTest()
        {
        }

        public override void OnEnter()
        {
            base.OnEnter();

            var screenSize = Layer.VisibleBoundsWorldspace.Size;

            var layer = new CCNode();
            layer.Position = screenSize.Center;
            AddChild(layer, 1);

            double layer_width = 0;

            // Add the black background for the text
            CCScale9Sprite background = new CCScale9SpriteFile("extensions/buttonBackground.png");
            background.ContentSize = new CCSize(80, 50);
            background.Position = new CCPoint((float) layer_width + background.ContentSize.Width / 2.0f, 0);
            layer.AddChild(background);

            layer_width += background.ContentSize.Width;

            DisplayValueLabel = new CCLabel("", "Arial", 30);

            DisplayValueLabel.Position = background.Position;
            layer.AddChild(DisplayValueLabel);

            // Add the slider
            var potentiometer = new CCControlPotentiometer("extensions/potentiometerTrack.png"
                ,
                "extensions/potentiometerProgress.png"
                , "extensions/potentiometerButton.png");
            potentiometer.Position = new CCPoint((float) layer_width + 10 + potentiometer.ContentSize.Width / 2, 0);

            // When the value of the slider will change, the given selector will be call
            potentiometer.AddTargetWithActionForControlEvents(this, ValueChanged, CCControlEvent.ValueChanged);

            layer.AddChild(potentiometer);

            layer_width += potentiometer.ContentSize.Width;

            // Set the layer size
            layer.ContentSize = new CCSize((float) layer_width, 0);
            layer.AnchorPoint = CCPoint.AnchorMiddle;

            // Update the value label
            ValueChanged(potentiometer, CCControlEvent.ValueChanged);

        }
        public void ValueChanged(Object sender, CCControlEvent controlEvent)
        {
            var pControl = (CCControlPotentiometer) sender;
            // Change value of label.
            DisplayValueLabel.Text = string.Format("{0:0.00}", pControl.Value);
        }

        public static CCScene sceneWithTitle(string title)
        {
            var pScene = new CCScene (AppDelegate.SharedWindow);
            var controlLayer = new CCControlPotentiometerTest();
            if (controlLayer != null)
            {
                controlLayer.SceneTitleLabel.Text = (title);
                pScene.AddChild(controlLayer);
            }
            return pScene;
        }
    }
}