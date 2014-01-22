using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests.Extensions
{
    class CCControlColourPickerTest : CCControlScene
    {
        public CCLabelTTF ColorLabel
        {
            get { return _colorLabel; }
            set { _colorLabel = value; }
        }

        public CCControlColourPickerTest()
        {
            CCSize screenSize = CCDirector.SharedDirector.WinSize;

            CCNode layer = new CCNode();
            layer.Position = new CCPoint(screenSize.Width / 2, screenSize.Height / 2);
            AddChild(layer, 1);

            float layer_width = 0;

            // Create the colour picker
            CCControlColourPicker colourPicker = new CCControlColourPicker();
            colourPicker.Color = new CCColor3B(37, 46, 252);
            colourPicker.Position = new CCPoint(colourPicker.ContentSize.Width / 2, 0);

            // Add it to the layer
            layer.AddChild(colourPicker);

            // Add the target-action pair
            colourPicker.AddTargetWithActionForControlEvents(this, ColourValueChanged,
                                                              CCControlEvent.ValueChanged);


            layer_width += colourPicker.ContentSize.Width;

            // Add the black background for the text
            CCScale9Sprite background = new CCScale9SpriteFile("extensions/buttonBackground.png");
            background.ContentSize = new CCSize(150, 50);
            background.Position = new CCPoint(layer_width + background.ContentSize.Width / 2.0f, 0);
            layer.AddChild(background);

            layer_width += background.ContentSize.Width;

            _colorLabel = new CCLabelTTF("#color", "Arial", 26);

            _colorLabel.Position = background.Position;
            layer.AddChild(_colorLabel);

            // Set the layer size
            layer.ContentSize = new CCSize(layer_width, 0);
            layer.AnchorPoint = new CCPoint(0.5f, 0.5f);

            // Update the color text
            ColourValueChanged(colourPicker, CCControlEvent.ValueChanged);
        }

        /** Callback for the change value. */

        public void ColourValueChanged(Object sender, CCControlEvent controlEvent)
        {
            CCControlColourPicker pPicker = (CCControlColourPicker)sender;
            _colorLabel.Text = string.Format("#{0:X00}{1:X00}{2:X00}", pPicker.Color.R, pPicker.Color.G, pPicker.Color.B);
        }

        private CCLabelTTF _colorLabel;

        public new static CCScene sceneWithTitle(string title)
        {
            var pScene = new CCScene();
            var controlLayer = new CCControlColourPickerTest();
            if (controlLayer != null)
            {
                controlLayer.getSceneTitleLabel().Text = (title);
                pScene.AddChild(controlLayer);
            }
            return pScene;
        }
    }
}
