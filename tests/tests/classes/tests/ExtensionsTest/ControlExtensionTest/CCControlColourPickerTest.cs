using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests.Extensions
{
    class CCControlColourPickerTest : CCControlScene
    {
        private CCLabel ColorLabel { get; set; }

        public CCControlColourPickerTest()
        {
        }

        public override void OnEnter()
        {
            base.OnEnter();

            var screenSize = Layer.VisibleBoundsWorldspace.Size;

            CCNode layer = new CCNode();
            layer.Position = screenSize.Center;
            AddChild(layer, 1);

            float layer_width = 0;

            // Create the colour picker
            CCControlColourPicker colourPicker = new CCControlColourPicker();
            colourPicker.Color = new CCColor3B(37, 46, 252);
            colourPicker.Position = new CCPoint(colourPicker.ContentSize.Width / 2, 0);

            // Add it to the layer
            layer.AddChild(colourPicker);

            // Add the target-action pair
            colourPicker.ValueChanged += ColourValueChanged; 

            layer_width += colourPicker.ContentSize.Width;

            // Add the black background for the text
            CCScale9Sprite background = new CCScale9SpriteFile("extensions/buttonBackground.png");
            background.ContentSize = new CCSize(150, 50);
            background.Position = new CCPoint(layer_width + background.ContentSize.Width / 2.0f, 0);
            layer.AddChild(background);

            layer_width += background.ContentSize.Width;

            ColorLabel = new CCLabel("#color", "Arial", 26, CCLabelFormat.SpriteFont);

            ColorLabel.Position = background.Position;
            layer.AddChild(ColorLabel);

            // Set the layer size
            layer.ContentSize = new CCSize(layer_width, 0);
            layer.AnchorPoint = new CCPoint(0.5f, 0.5f);

            // Update the color text
            ColourValueChanged(colourPicker, CCControlEvent.ValueChanged);

        }

        private void ColourValueChanged(object sender, CCControl.CCControlEventArgs e)
        {
            ColourValueChanged(sender, e.ControlEvent);
        }


        /** Callback for the change value. */

        public void ColourValueChanged(Object sender, CCControlEvent controlEvent)
        {
            CCControlColourPicker pPicker = (CCControlColourPicker)sender;
            ColorLabel.Text = string.Format("#{0:X00}{1:X00}{2:X00}", pPicker.Color.R, pPicker.Color.G, pPicker.Color.B);
        }

        public static CCScene SceneWithTitle(string title)
        {
            var pScene = new CCScene(AppDelegate.SharedWindow);
            var controlLayer = new CCControlColourPickerTest();
            if (controlLayer != null)
            {
                controlLayer.SceneTitleLabel.Text = title;
                pScene.AddChild(controlLayer);
            }
            return pScene;
        }
    }
}
