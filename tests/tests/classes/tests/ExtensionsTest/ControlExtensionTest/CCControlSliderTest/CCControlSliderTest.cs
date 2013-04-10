using System;
using cocos2d;

namespace tests.Extensions
{
    public class CCControlSliderTest : CCControlScene
	{
		public override bool Init()
		{
			if (base.Init())
			{
				CCSize screenSize = CCDirector.SharedDirector.WinSize;

				// Add a label in which the slider value will be displayed
				m_pDisplayValueLabel = new CCLabelTTF("Move the slider thumb!\nThe lower slider is restricted.", "Marker Felt", 32);
				m_pDisplayValueLabel.AnchorPoint = new CCPoint(0.5f, -1.0f);
				m_pDisplayValueLabel.Position = new CCPoint(screenSize.Width / 1.7f, screenSize.Height / 2.0f);
				AddChild(m_pDisplayValueLabel);

				// Add the slider
                var slider = CCControlSlider.Create("extensions/sliderTrack", "extensions/sliderProgress", "extensions/sliderThumb");
				slider.AnchorPoint = new CCPoint(0.5f, 1.0f);
				slider.MinimumValue = 0.0f; // Sets the min value of range
				slider.MaximumValue = 5.0f; // Sets the max value of range
				slider.Position = new CCPoint(screenSize.Width / 2.0f, screenSize.Height / 2.0f + 16);
				slider.Tag = 1;

				// When the value of the slider will change, the given selector will be call
				slider.AddTargetWithActionForControlEvents(this, valueChanged, CCControlEvent.ValueChanged);

                var restrictSlider = CCControlSlider.Create("extensions/sliderTrack", "extensions/sliderProgress", "extensions/sliderThumb");
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
				return true;
			}
			return false;
		}

		public void valueChanged(object sender, CCControlEvent controlEvent)
		{
			var pSlider = (CCControlSlider)sender;
			// Change value of label.
			if (pSlider.Tag == 1)
				m_pDisplayValueLabel.Label = (String.Format("Upper slider value = {0:2}", pSlider.Value));
			if (pSlider.Tag == 2)
				m_pDisplayValueLabel.Label = (String.Format("Lower slider value = {0:2}", pSlider.Value));
		}



		protected CCLabelTTF m_pDisplayValueLabel;

		public new static CCScene sceneWithTitle(string title)
		{
			var pScene = CCScene.Create();
			var controlLayer = new CCControlSliderTest();
			if (controlLayer != null && controlLayer.Init())
			{
				controlLayer.getSceneTitleLabel().Label = (title);
				pScene.AddChild(controlLayer);
			}
			return pScene;
		}
	}
}