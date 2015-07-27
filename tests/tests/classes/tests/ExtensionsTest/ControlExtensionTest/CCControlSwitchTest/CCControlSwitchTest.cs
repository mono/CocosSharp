using CocosSharp;

namespace tests.Extensions
{
    public class CCControlSwitchTest : CCControlScene
    {
        private CCLabel DisplayValueLabel { get; set; }

        public CCControlSwitchTest()
        {

        }

        public override void OnEnter()
        {
            base.OnEnter();

            var screenSize = Layer.VisibleBoundsWorldspace.Size;

            var layer = new CCNode ();
            layer.Position = screenSize.Center;
            AddChild(layer, 1);

            var layerWidth = 0.0f;

            // Add the black background for the text
            var background = new CCScale9SpriteFile("extensions/buttonBackground");
            background.ContentSize = new CCSize(80, 50);
            background.Position = new CCPoint(layerWidth + background.ContentSize.Width / 2.0f, 0);
            layer.AddChild(background);

            layerWidth += background.ContentSize.Width;

            DisplayValueLabel = new CCLabel("#color", "Arial", 30, CCLabelFormat.SpriteFont);

            DisplayValueLabel.Position = background.Position;
            layer.AddChild(DisplayValueLabel);

            // Create the switch
            CCControlSwitch switchControl = new CCControlSwitch
                (
                    new CCSprite("extensions/switch-mask"),
                    new CCSprite("extensions/switch-on"),
                    new CCSprite("extensions/switch-off"),
                    new CCSprite("extensions/switch-thumb"),
                    new CCLabel("On", "Arial", 16, CCLabelFormat.SpriteFont),
                    new CCLabel("Off", "Arial", 16, CCLabelFormat.SpriteFont)
                );
            switchControl.Position = new CCPoint(layerWidth + 10 + switchControl.ContentSize.Width / 2, 0);
            layer.AddChild(switchControl);

            // Subscribe to the switches StateChanged event
            switchControl.StateChanged += SwitchControl_StateChanged;

            // --------------- OR ---------------------
            // we can subscribe to the ValueChanged event.
            //switchControl.ValueChanged += SwitchControl_ValueChanged;



            // Set the layer size
            layer.ContentSize = new CCSize(layerWidth, 0);
            layer.AnchorPoint = CCPoint.AnchorMiddle;

            // Update the value label
            ValueChanged(switchControl, CCControlEvent.ValueChanged);
        }

        private void SwitchControl_StateChanged(object sender, CCControlSwitch.CCSwitchStateEventArgs e)
        {
            if (e.State)
            {
                DisplayValueLabel.Text = ("On");
            }
            else
            {
                DisplayValueLabel.Text = ("Off");
            }

        }

        private void SwitchControl_ValueChanged(object sender, CCControl.CCControlEventArgs e)
        {
            ValueChanged(sender, e.ControlEvent);
        }

        /* Callback for the change value. */

        public void ValueChanged(object sender, CCControlEvent controlEvent)
        {
            var controlSwitch = (CCControlSwitch) sender;
			if (controlSwitch.On)
            {
                DisplayValueLabel.Text = ("On");
            }
            else
            {
                DisplayValueLabel.Text = ("Off");
            }
        }


        public static CCScene SceneWithTitle(string title)
        {
            var pScene = new CCScene (AppDelegate.SharedWindow);
            var controlLayer = new CCControlSwitchTest();
            if (controlLayer != null)
            {
                controlLayer.SceneTitleLabel.Text = (title);
                pScene.AddChild(controlLayer);
            }
            return pScene;
        }
    }
}