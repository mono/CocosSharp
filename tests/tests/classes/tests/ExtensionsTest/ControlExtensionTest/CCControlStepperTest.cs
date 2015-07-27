using System;
using CocosSharp;

namespace tests.Extensions
{
    public class CCControlStepperTest : CCControlScene
    {
        protected CCLabel DisplayValueLabel { get; set; }

        public CCControlStepperTest()
        {
        }

        public override void OnEnter()
        {
            base.OnEnter();

            CCSize screenSize = Layer.VisibleBoundsWorldspace.Size;

            var layer = new CCNode();
            layer.Position = screenSize.Center;
            AddChild(layer, 1);

            float layer_width = 0;

            // Add the black background for the text
            CCScale9Sprite background = new CCScale9SpriteFile("extensions/buttonBackground.png");
            background.ContentSize = new CCSize(100, 50);
            background.Position = new CCPoint(layer_width + background.ContentSize.Width / 2.0f, 0);
            layer.AddChild(background);

            DisplayValueLabel = new CCLabel("0", "Arial", 26, CCLabelFormat.SpriteFont);

            DisplayValueLabel.Position = background.Position;
            layer.AddChild(DisplayValueLabel);

            layer_width += background.ContentSize.Width;

            CCControlStepper stepper = MakeControlStepper();
            stepper.Position = new CCPoint(layer_width + 10 + stepper.ContentSize.Width / 2, 0);
            stepper.ValueChanged += Stepper_ValueChanged;
            layer.AddChild(stepper);

            layer_width += stepper.ContentSize.Width;

            // Set the layer size
            layer.ContentSize = new CCSize(layer_width, 0);
            layer.AnchorPoint = CCPoint.AnchorMiddle;

            // Update the value label
            ValueChanged(stepper, CCControlEvent.ValueChanged);

        }

        private void Stepper_ValueChanged(object sender, CCControl.CCControlEventArgs e)
        {
            ValueChanged(sender, e.ControlEvent);
        }

        /** Creates and returns a new ControlStepper. */

        public CCControlStepper MakeControlStepper()
        {
            var minusSprite = new CCSprite("extensions/stepper-minus.png");
            var plusSprite = new CCSprite("extensions/stepper-plus.png");

            return new CCControlStepper(minusSprite, plusSprite);
        }

        /** Callback for the change value. */

        public void ValueChanged(Object sender, CCControlEvent controlEvent)
        {
            var pControl = (CCControlStepper) sender;
            // Change value of label.
            DisplayValueLabel.Text = String.Format("{0:0.00}", pControl.Value);
        }

        public static CCScene SceneWithTitle(string title)
        {
            var pScene = new CCScene (AppDelegate.SharedWindow);
            var controlLayer = new CCControlStepperTest();
            if (controlLayer != null)
            {
                controlLayer.SceneTitleLabel.Text = (title);
                pScene.AddChild(controlLayer);
            }
            return pScene;
        }
    }
}