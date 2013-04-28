using cocos2d;

namespace tests.Extensions
{
    public class CCControlSwitchTest : CCControlScene
    {
        private CCLabelTTF m_pDisplayValueLabel;

        public override bool Init()
        {
            if (base.Init())
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

                m_pDisplayValueLabel = new CCLabelTTF("#color", "Marker Felt", 30);

                m_pDisplayValueLabel.Position = background.Position;
                layer.AddChild(m_pDisplayValueLabel);

                // Create the switch
                CCControlSwitch switchControl = new CCControlSwitch
                    (
                        new CCSprite("extensions/switch-mask"),
                        new CCSprite("extensions/switch-on"),
                        new CCSprite("extensions/switch-off"),
                        new CCSprite("extensions/switch-thumb"),
                        new CCLabelTTF("On", "Arial-BoldMT", 16),
                        new CCLabelTTF("Off", "Arial-BoldMT", 16)
                    );
                switchControl.Position = new CCPoint(layer_width + 10 + switchControl.ContentSize.Width / 2, 0);
                layer.AddChild(switchControl);

                switchControl.AddTargetWithActionForControlEvents(this, valueChanged, CCControlEvent.ValueChanged);

                // Set the layer size
                layer.ContentSize = new CCSize(layer_width, 0);
                layer.AnchorPoint = new CCPoint(0.5f, 0.5f);

                // Update the value label
                valueChanged(switchControl, CCControlEvent.ValueChanged);
                return true;
            }
            return false;
        }

        /* Callback for the change value. */

        public void valueChanged(object sender, CCControlEvent controlEvent)
        {
            var pSwitch = (CCControlSwitch) sender;
            if (pSwitch.IsOn())
            {
                m_pDisplayValueLabel.Label = ("On");
            }
            else
            {
                m_pDisplayValueLabel.Label = ("Off");
            }
        }


        public new static CCScene sceneWithTitle(string title)
        {
            CCScene pScene = new CCScene();
            var controlLayer = new CCControlSwitchTest();
            if (controlLayer != null && controlLayer.Init())
            {
                controlLayer.getSceneTitleLabel().Label = (title);
                pScene.AddChild(controlLayer);
            }
            return pScene;
        }
    }
}