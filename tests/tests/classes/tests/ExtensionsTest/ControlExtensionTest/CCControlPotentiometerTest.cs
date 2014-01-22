using System;
using CocosSharp;

namespace tests.Extensions
{
    internal class CCControlPotentiometerTest : CCControlScene
    {
        private CCLabelTTF _displayValueLabel;

        public CCLabelTTF DisplayValueLabel
        {
            get { return _displayValueLabel; }
            set { _displayValueLabel = value; }
        }

        public CCControlPotentiometerTest()
        {
            CCSize screenSize = CCDirector.SharedDirector.WinSize;

            var layer = new CCNode();
            layer.Position = new CCPoint(screenSize.Width / 2, screenSize.Height / 2);
            AddChild(layer, 1);

            double layer_width = 0;

            // Add the black background for the text
            CCScale9Sprite background = new CCScale9SpriteFile("extensions/buttonBackground.png");
            background.ContentSize = new CCSize(80, 50);
            background.Position = new CCPoint((float) layer_width + background.ContentSize.Width / 2.0f, 0);
            layer.AddChild(background);

            layer_width += background.ContentSize.Width;

            DisplayValueLabel = new CCLabelTTF("", "Arial", 30);

            _displayValueLabel.Position = background.Position;
            layer.AddChild(_displayValueLabel);

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
            layer.AnchorPoint = new CCPoint(0.5f, 0.5f);

            // Update the value label
            ValueChanged(potentiometer, CCControlEvent.ValueChanged);
        }

        public void ValueChanged(Object sender, CCControlEvent controlEvent)
        {
            var pControl = (CCControlPotentiometer) sender;
            // Change value of label.
            _displayValueLabel.Text = string.Format("{0:0.00}", pControl.Value);
        }

        public new static CCScene sceneWithTitle(string title)
        {
            var pScene = new CCScene();
            var controlLayer = new CCControlPotentiometerTest();
            if (controlLayer != null)
            {
                controlLayer.getSceneTitleLabel().Text = (title);
                pScene.AddChild(controlLayer);
            }
            return pScene;
        }
    }
}