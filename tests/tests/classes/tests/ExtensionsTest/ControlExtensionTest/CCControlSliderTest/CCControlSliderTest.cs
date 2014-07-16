using System;
using CocosSharp;

namespace tests.Extensions
{
    public class CCControlSliderTest : CCControlScene
    {
        protected CCLabelTtf m_pDisplayValueLabel;


        public CCControlSliderTest()
        {
            CCSize screenSize = Layer.VisibleBoundsWorldspace.Size;

            // Add a label in which the slider value will be displayed
            m_pDisplayValueLabel = new CCLabelTtf("Move the slider thumb!\nThe lower slider is restricted.", "Arial", 32);
            m_pDisplayValueLabel.AnchorPoint = new CCPoint(0.5f, -1.0f);
            m_pDisplayValueLabel.Position = new CCPoint(screenSize.Width / 2, screenSize.Height / 2.0f);
            AddChild(m_pDisplayValueLabel);

            // Add the slider
            var slider = new CCControlSlider("extensions/sliderTrack", "extensions/sliderProgress",
                                             "extensions/sliderThumb");
            slider.AnchorPoint = new CCPoint(0.5f, 1.0f);
            slider.MinimumValue = 0.0f; // Sets the min value of range
            slider.MaximumValue = 5.0f; // Sets the max value of range
            slider.Position = new CCPoint(screenSize.Width / 2.0f, screenSize.Height / 2.0f + 16);
            slider.Tag = 1;

            // When the value of the slider will change, the given selector will be call
            slider.AddTargetWithActionForControlEvents(this, valueChanged, CCControlEvent.ValueChanged);

            var restrictSlider = new CCControlSlider("extensions/sliderTrack", "extensions/sliderProgress",
                                                     "extensions/sliderThumb");
            restrictSlider.AnchorPoint = new CCPoint(0.5f, 1.0f);
            restrictSlider.MinimumValue = 0.0f; // Sets the min value of range
            restrictSlider.MaximumValue = 5.0f; // Sets the max value of range
            restrictSlider.MaximumAllowedValue = 4.0f;
            restrictSlider.MinimumAllowedValue = 1.5f;
            restrictSlider.Value = 3.0f;
            restrictSlider.Position = new CCPoint(screenSize.Width / 2.0f, screenSize.Height / 2.0f - 24);
            restrictSlider.Tag = 2;

            //same with restricted
            restrictSlider.AddTargetWithActionForControlEvents(this, valueChanged, CCControlEvent.ValueChanged);

            AddChild(slider);
            AddChild(restrictSlider);
        }

        public void valueChanged(object sender, CCControlEvent controlEvent)
        {
            var pSlider = (CCControlSlider) sender;
            // Change value of label.
            if (pSlider.Tag == 1)
                m_pDisplayValueLabel.Text = (String.Format("Upper slider value = {0:0.00}", pSlider.Value));
            if (pSlider.Tag == 2)
                m_pDisplayValueLabel.Text = (String.Format("Lower slider value = {0:0.00}", pSlider.Value));
        }


        public new static CCScene sceneWithTitle(string title)
        {
            var pScene = new CCScene (AppDelegate.SharedWindow, AppDelegate.SharedViewport, AppDelegate.SharedDirector);
            var controlLayer = new CCControlSliderTest();
            if (controlLayer != null)
            {
                controlLayer.getSceneTitleLabel().Text = (title);
                pScene.AddChild(controlLayer);
            }
            return pScene;
        }
    }
}