using System;
using CocosSharp;
using Random = CocosSharp.CCRandom;

namespace tests.Extensions
{
    class CCControlButtonTest_HelloVariableSize : CCControlScene
	{
        public CCControlButtonTest_HelloVariableSize()
		{

		}

        public override void OnEnter()
        {
            base.OnEnter();

            CCSize screenSize = Layer.VisibleBoundsWorldspace.Size;

            // Defines an array of title to create buttons dynamically
            var stringArray = new[] {
                "Hello",
                "Variable",
                "Size",
                "!"
            };

            CCNode layer = new CCNode ();
            AddChild(layer, 1);

            float total_width = 0, height = 0;

            // For each title in the array
            int i = 0;
            foreach(var title in stringArray)
            {
                // Creates a button with this string as title
                var button = standardButtonWithTitle(title);
                if (i == 0)
                {
                    button.Opacity = 50;
                    button.Color = new CCColor3B(0, 255, 0);
                }
                else if (i == 1)
                {
                    button.Opacity = 200;
                    button.Color = new CCColor3B(0, 255, 0);
                }
                else if (i == 2)
                {
                    button.Opacity = 100;
                    button.Color = new CCColor3B(0, 0, 255);
                }

                button.Position = new CCPoint (total_width + button.ContentSize.Width / 2, button.ContentSize.Height / 2);
                layer.AddChild(button);

                // Compute the size of the layer
                height = button.ContentSize.Height;
                total_width += button.ContentSize.Width;
                i++;
            }

            layer.AnchorPoint = new CCPoint(0.5f, 0.5f);
            layer.ContentSize = new CCSize(total_width, height);
            layer.Position = new CCPoint(screenSize.Width / 2.0f, screenSize.Height / 2.0f);

            // Add the black background
            var background = new CCScale9SpriteFile("extensions/buttonBackground");
            background.ContentSize = new CCSize(total_width + 14, height + 14);
            background.Position = new CCPoint(screenSize.Width / 2.0f, screenSize.Height / 2.0f);
            AddChild(background);
        }
		
		/** Creates and return a button with a default background and title color. */
		public CCControlButton standardButtonWithTitle(string title)
		{
			/** Creates and return a button with a default background and title color. */
			var backgroundButton = new CCScale9SpriteFile("extensions/button");
			var backgroundHighlightedButton = new CCScale9SpriteFile("extensions/buttonHighlighted");
    
			var titleButton = new CCLabel(title, "Arial", 30, CCLabelFormat.SpriteFont);

			titleButton.Color = new CCColor3B(159, 168, 176);

            var button = new CCControlButton(titleButton, backgroundButton);
			button.SetBackgroundSpriteForState(backgroundHighlightedButton, CCControlState.Highlighted);
			button.SetTitleColorForState(CCColor3B.White, CCControlState.Highlighted);
    
			return button;
		}


        public static CCScene SceneWithTitle(string title)
		{
            var pScene = new CCScene (AppDelegate.SharedWindow);
			var controlLayer = new CCControlButtonTest_HelloVariableSize();
    		controlLayer.SceneTitleLabel.Text = (title);
			pScene.AddChild(controlLayer);
			return pScene;
		}
	}

    class CCControlButtonTest_Inset : CCControlScene
    {
        public CCControlButtonTest_Inset()
        {
        }

        public override void OnEnter()
        {
            base.OnEnter();

            CCSize screenSize = Layer.VisibleBoundsWorldspace.Size;

            // Defines an array of title to create buttons dynamically
            var stringArray = new[] {
                "Inset",
                "Inset",
                "Inset"
            };

            CCNode layer = new CCNode ();
            AddChild(layer, 1);

            float total_width = 0, height = 0;

            var insetRect = new CCRect(5, 5, 5, 5);
            // For each title in the array
            foreach (var title in stringArray)
            {
                // Creates a button with this string as title
                CCControlButton button = insetButtonWithTitle(title, insetRect);
                button.Position = new CCPoint(total_width + button.ContentSize.Width / 2, button.ContentSize.Height / 2);
                layer.AddChild(button);

                // Compute the size of the layer
                height = button.ContentSize.Height;
                total_width += button.ContentSize.Width;
            }

            layer.AnchorPoint = CCPoint.AnchorMiddle;
            layer.ContentSize = new CCSize(total_width, height);
            layer.Position = screenSize.Center;

            // Add the black background
            var background = new CCScale9SpriteFile("extensions/buttonBackground");
            background.ContentSize = new CCSize(total_width + 14, height + 14);
            background.Position = screenSize.Center;
            AddChild(background);

        }

        // Creates and return a button with a default background and title color. 
        public CCControlButton standardButtonWithTitle(string title)
        {
            // Creates and return a button with a default background and title color. 
            var backgroundButton = new CCScale9SpriteFile("extensions/button");
            var backgroundHighlightedButton = new CCScale9SpriteFile("extensions/buttonHighlighted");

            var titleButton = new CCLabel(title, "Arial", 30, CCLabelFormat.SpriteFont);

            titleButton.Color = new CCColor3B(159, 168, 176);

            var button = new CCControlButton(titleButton, backgroundButton);
            button.SetBackgroundSpriteForState(backgroundHighlightedButton, CCControlState.Highlighted);
            button.SetTitleColorForState(CCColor3B.White, CCControlState.Highlighted);

            return button;
        }

        public CCControlButton insetButtonWithTitle(string title, CCRect inset)
        {
            /** Creates and return a button with a default background and title color. */
            var backgroundButton = new CCScale9SpriteFile("extensions/button");
            var backgroundHighlightedButton = new CCScale9SpriteFile("extensions/buttonHighlighted");
            backgroundButton.CapInsets = inset;
            backgroundHighlightedButton.CapInsets = inset;

            var titleButton = new CCLabel(title, "Arial", 30, CCLabelFormat.SpriteFont);

            titleButton.Color = new CCColor3B(159, 168, 176);

            var button = new CCControlButton(titleButton, backgroundButton);
            button.SetBackgroundSpriteForState(backgroundHighlightedButton, CCControlState.Highlighted);
            button.SetTitleColorForState(CCColor3B.White, CCControlState.Highlighted);

            return button;
        }

        public static CCScene SceneWithTitle(string title)
        {
            var pScene = new CCScene (AppDelegate.SharedWindow);
            var controlLayer = new CCControlButtonTest_Inset();
            controlLayer.SceneTitleLabel.Text = (title);
            pScene.AddChild(controlLayer);
            return pScene;
        }
    }

	class CCControlButtonTest_Event : CCControlScene
	{

        private CCLabel DisplayValueLabel { get; set; }

        public CCControlButtonTest_Event()
		{
		}

        public override void OnEnter()
        {
            base.OnEnter();

            CCSize screenSize = Layer.VisibleBoundsWorldspace.Size;

            // Add a label in which the button events will be displayed
            DisplayValueLabel = new CCLabel("No Event", "Arial", 28, CCLabelFormat.SpriteFont);
            DisplayValueLabel.AnchorPoint = new CCPoint(0.5f, -0.5f);
            DisplayValueLabel.Position = screenSize.Center;
            AddChild(DisplayValueLabel, 1);

            // Add the button
            var backgroundButton = new CCScale9SpriteFile("extensions/button");
            var backgroundHighlightedButton = new CCScale9SpriteFile("extensions/buttonHighlighted");

            var titleButton = new CCLabel("Touch Me!", "Arial", 30, CCLabelFormat.SpriteFont);

            titleButton.Color = new CCColor3B(159, 168, 176);

            var controlButton = new CCControlButton(titleButton, backgroundButton);
            controlButton.SetBackgroundSpriteForState(backgroundHighlightedButton, CCControlState.Highlighted);
            controlButton.SetTitleColorForState(CCColor3B.White, CCControlState.Highlighted);

            controlButton.AnchorPoint = CCPoint.AnchorMiddleTop;
            controlButton.Position = screenSize.Center;
            AddChild(controlButton, 1);

            // Add the black background
            var background = new CCScale9SpriteFile("extensions/buttonBackground");
            background.ContentSize = new CCSize(300, 170);
            background.Position = screenSize.Center;
            AddChild(background);

            // Sets up event handlers
            controlButton.TouchDown += ControlButton_TouchDown;
            controlButton.TouchDragInside += ControlButton_TouchDragInside;
            controlButton.TouchDragOutside += ControlButton_TouchDragOutside;
            controlButton.TouchDragEnter += ControlButton_TouchDragEnter;
            controlButton.TouchDragExit += ControlButton_TouchDragExit;
            //controlButton.TouchUpInside += ControlButton_TouchUpInside;
            controlButton.TouchUpOutside += ControlButton_TouchUpOutside;
            controlButton.TouchCancel += ControlButton_TouchCancel;

            // To see clicked events your will need comment out TouchUpInside events
            controlButton.Clicked += ControlButton_Clicked;
        }

        private void ControlButton_Clicked(object sender, EventArgs e)
        {
            DisplayValueLabel.Text = ("Clicked");
        }

        private void ControlButton_TouchDown(object sender, CCControl.CCControlEventArgs e)
        {
            DisplayValueLabel.Text = ("Touch Down");
        }

        public void touchDownAction(object sender, CCControlEvent controlEvent)
		{
			DisplayValueLabel.Text = ("Touch Down");
		}

        private void ControlButton_TouchDragInside(object sender, CCControl.CCControlEventArgs e)
        {
            DisplayValueLabel.Text = ("Drag Inside");
        }

        private void ControlButton_TouchDragOutside(object sender, CCControl.CCControlEventArgs e)
        {
            DisplayValueLabel.Text = ("Drag Outside");
        }

        private void ControlButton_TouchDragEnter(object sender, CCControl.CCControlEventArgs e)
        {
            DisplayValueLabel.Text = ("Drag Enter");
        }

        private void ControlButton_TouchDragExit(object sender, CCControl.CCControlEventArgs e)
        {
            DisplayValueLabel.Text = ("Drag Exit");
        }

        private void ControlButton_TouchUpInside(object sender, CCControl.CCControlEventArgs e)
        {
            DisplayValueLabel.Text = ("Touch Up Inside.");
        }


        private void ControlButton_TouchUpOutside(object sender, CCControl.CCControlEventArgs e)
        {
            DisplayValueLabel.Text = ("Touch Up Outside.");
        }

        private void ControlButton_TouchCancel(object sender, CCControl.CCControlEventArgs e)
        {
            DisplayValueLabel.Text = ("Touch Cancel");
        }

        public static CCScene SceneWithTitle(string title)
		{
            var pScene = new CCScene (AppDelegate.SharedWindow);
			var controlLayer = new CCControlButtonTest_Event();
			if (controlLayer != null)
			{
				controlLayer.SceneTitleLabel.Text = (title);
				pScene.AddChild(controlLayer);
			}
			return pScene;
		}
	}

	class CCControlButtonTest_Styling : CCControlScene
	{
        public CCControlButtonTest_Styling()
		{
		}

        public override void OnEnter()
        {
            base.OnEnter();

            CCSize screenSize = Layer.VisibleBoundsWorldspace.Size;

            var layer = new CCNode ();
            AddChild(layer, 1);

            int space = 10; // px

            float max_w = 0, max_h = 0;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    // Add the buttons
                    var button = standardButtonWithTitle(CCRandom.Next(30).ToString());
                    button.IsAdjustBackgroundImage = false;  // Tells the button that the background image must not be adjust
                    // It'll use the prefered size of the background image
                    button.Position = new CCPoint(button.ContentSize.Width / 2 + (button.ContentSize.Width + space) * i,
                        button.ContentSize.Height / 2 + (button.ContentSize.Height + space) * j);
                    layer.AddChild(button);

                    max_w = Math.Max(button.ContentSize.Width * (i + 1) + space * i, max_w);
                    max_h = Math.Max(button.ContentSize.Height * (j + 1) + space * j, max_h);
                }
            }

            layer.AnchorPoint = CCPoint.AnchorMiddle;
            layer.ContentSize = new CCSize(max_w, max_h);
            layer.Position = screenSize.Center;

            // Add the black background
            var backgroundButton = new CCScale9SpriteFile("extensions/buttonBackground");
            backgroundButton.ContentSize = new CCSize(max_w + 14, max_h + 14);
            backgroundButton.Position = screenSize.Center;
            AddChild(backgroundButton);

        }

		public CCControlButton standardButtonWithTitle(string title)
		{
			/** Creates and return a button with a default background and title color. */
			var backgroundButton = new CCScale9SpriteFile("extensions/button");
			backgroundButton.PreferredSize = new CCSize(55, 55);  // Set the prefered size
			var backgroundHighlightedButton = new CCScale9SpriteFile("extensions/buttonHighlighted");
			backgroundHighlightedButton.PreferredSize = new CCSize(55, 55);  // Set the prefered size
    
			var titleButton = new CCLabel(title, "Arial", 30, CCLabelFormat.SpriteFont);

			titleButton.Color = new CCColor3B(159, 168, 176);

            var button = new CCControlButton(titleButton, backgroundButton);
			button.SetBackgroundSpriteForState(backgroundHighlightedButton, CCControlState.Highlighted);
			button.SetTitleColorForState(CCColor3B.White, CCControlState.Highlighted);
    
			return button;
		}

        public static CCScene SceneWithTitle(string title)
		{
            var pScene = new CCScene (AppDelegate.SharedWindow);
			var controlLayer = new CCControlButtonTest_Styling();
			if (controlLayer != null)
			{
				controlLayer.SceneTitleLabel.Text = (title);
				pScene.AddChild(controlLayer);
			}
			return pScene;
		}
	}

}