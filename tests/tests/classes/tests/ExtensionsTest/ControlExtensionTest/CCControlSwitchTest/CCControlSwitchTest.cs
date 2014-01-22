using CocosSharp;

namespace tests.Extensions
{
    public class CCControlSwitchTest : CCControlScene
    {
        private CCLabelTTF m_pDisplayValueLabel;

        public CCControlSwitchTest()
        {
            CCSize screenSize = CCDirector.SharedDirector.WinSize;

            CCNode layer = new CCNode ();
            layer.Position = new CCPoint(screenSize.Width / 2, screenSize.Height / 2);
            AddChild(layer, 1);

            float layer_width = 0.0f;

            // Add the black background for the text
            CCScale9Sprite background = new CCScale9SpriteFile("extensions/buttonBackground");
            background.ContentSize = new CCSize(80, 50);
            background.Position = new CCPoint(layer_width + background.ContentSize.Width / 2.0f, 0);
            layer.AddChild(background);

            layer_width += background.ContentSize.Width;

            m_pDisplayValueLabel = new CCLabelTTF("#color", "Arial", 30);

            m_pDisplayValueLabel.Position = background.Position;
            layer.AddChild(m_pDisplayValueLabel);

            // Create the switch
            CCControlSwitch switchControl = new CCControlSwitch
                (
                    new CCSprite("extensions/switch-mask"),
                    new CCSprite("extensions/switch-on"),
                    new CCSprite("extensions/switch-off"),
                    new CCSprite("extensions/switch-thumb"),
                    new CCLabelTTF("On", "Arial", 16),
                    new CCLabelTTF("Off", "Arial", 16)
                );
            switchControl.Position = new CCPoint(layer_width + 10 + switchControl.ContentSize.Width / 2, 0);
            layer.AddChild(switchControl);

            switchControl.AddTargetWithActionForControlEvents(this, valueChanged, CCControlEvent.ValueChanged);

            // Set the layer size
            layer.ContentSize = new CCSize(layer_width, 0);
            layer.AnchorPoint = new CCPoint(0.5f, 0.5f);

            // Update the value label
            valueChanged(switchControl, CCControlEvent.ValueChanged);
        }

        /* Callback for the change value. */

        public void valueChanged(object sender, CCControlEvent controlEvent)
        {
            var pSwitch = (CCControlSwitch) sender;
            if (pSwitch.IsOn())
            {
                m_pDisplayValueLabel.Text = ("On");
            }
            else
            {
                m_pDisplayValueLabel.Text = ("Off");
            }
        }


        public new static CCScene sceneWithTitle(string title)
        {
            CCScene pScene = new CCScene();
            var controlLayer = new CCControlSwitchTest();
            if (controlLayer != null)
            {
                controlLayer.getSceneTitleLabel().Text = (title);
                pScene.AddChild(controlLayer);
            }
            return pScene;
        }
    }
}