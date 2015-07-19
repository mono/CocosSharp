using System;
using CocosSharp;

namespace tests.Extensions
{
    public class CCControlSliderTest : CCControlScene
    {
        protected CCLabel DisplayValueLabel { get; set; }


        public CCControlSliderTest()
        {
            DisplayValueLabel = new CCLabel("Move the slider thumb!\nThe lower slider is restricted.", "Arial", 28, CCLabelFormat.SpriteFont);
            DisplayValueLabel.AnchorPoint = new CCPoint(0.5f, -0.5f);
        }

        public override void OnEnter()
        {
            base.OnEnter();

            CCSize screenSize = Layer.VisibleBoundsWorldspace.Size;

            // Add a label in which the slider value will be displayed
            DisplayValueLabel.Position = screenSize.Center;
            AddChild(DisplayValueLabel);

            // Add the slider
            var slider = new CCControlSlider("extensions/sliderTrack", "extensions/sliderProgress",
                "extensions/sliderThumb");
            slider.AnchorPoint = CCPoint.AnchorMiddleTop;
            slider.MinimumValue = 0.0f; // Sets the min value of range
            slider.MaximumValue = 5.0f; // Sets the max value of range
            slider.Position = screenSize.Center;
            slider.PositionY += 16;
            slider.Tag = 1;

            // When the value of the slider will change, the given selector will be call
            slider.AddTargetWithActionForControlEvents(this, valueChanged, CCControlEvent.ValueChanged);

            var restrictSlider = new CCControlSlider("extensions/sliderTrack", "extensions/sliderProgress",
                "extensions/sliderThumb");
            restrictSlider.AnchorPoint = CCPoint.AnchorMiddleTop;
            restrictSlider.MinimumValue = 0.0f; // Sets the min value of range
            restrictSlider.MaximumValue = 5.0f; // Sets the max value of range
            restrictSlider.MaximumAllowedValue = 4.0f;
            restrictSlider.MinimumAllowedValue = 1.5f;
            restrictSlider.Value = 3.0f;
            restrictSlider.Position = screenSize.Center;
            restrictSlider.PositionY -= 24;
            restrictSlider.Tag = 2;

            //same with restricted
            restrictSlider.AddTargetWithActionForControlEvents(this, valueChanged, CCControlEvent.ValueChanged);

            AddChild(slider);
            AddChild(restrictSlider);
        }

        public void valueChanged(object sender, CCControlEvent controlEvent)
        {
            var slider = (CCControlSlider) sender;
            // Change value of label.
            if (slider.Tag == 1)
                DisplayValueLabel.Text = (String.Format("Upper slider value = {0:0.00}", slider.Value));
            if (slider.Tag == 2)
                DisplayValueLabel.Text = (String.Format("Lower slider value = {0:0.00}", slider.Value));
        }


        public static CCScene sceneWithTitle(string title)
        {
            var scene = new CCScene (AppDelegate.SharedWindow);
            var controlLayer = new CCControlSliderTest();
            if (controlLayer != null)
            {
                controlLayer.SceneTitleLabel.Text = (title);
                scene.AddChild(controlLayer);
            }
            return scene;
        }
    }
}