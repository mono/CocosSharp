using CocosSharp;

namespace tests.Extensions
{
    public class S9SpriteTestDemo : CCControlScene
    {
        public override void OnEnter()
        {
            base.OnEnter();
            CCSpriteFrameCache.SharedSpriteFrameCache.AddSpriteFrames("Images/blocks9ss.plist");
            SceneTitleLabel.Text = Title();
            SceneSubtitleLabel.Text = Subtitle();
        }

        public virtual string Title()
        {
            return "";
        }

        public virtual string Subtitle()
        {
            return "";
        }

        public override void previousCallback(object sender)
        {
            Scene.Director.ReplaceScene(
                Scale9SpriteSceneManager.SharedSprite9SceneManager.PreviousControlScene);
        }

        public override void restartCallback(object sender)
        {
            Scene.Director.ReplaceScene(
                Scale9SpriteSceneManager.SharedSprite9SceneManager.CurrentControlScene);
        }

        public override void nextCallback(object sender)
        {
            Scene.Director.ReplaceScene(
                Scale9SpriteSceneManager.SharedSprite9SceneManager.NextControlScene);
        }
    }

    // S9BatchNodeBasic
    public class S9Scale9SpriteTest : S9SpriteTestDemo
    {
        public override void OnEnter()
        {
            base.OnEnter();

            CCSize winSize = VisibleBoundsWorldspace.Size;
            float x = winSize.Center.X;
            float y = winSize.Center.Y;

            //var widgetSize = _widget->getContentSize();

            var moveTo = new CCMoveBy(1.0f, new CCPoint(30, 0));
            var moveBack = moveTo.Reverse();
            var rotateBy = new CCRotateBy(1.0f, 180);
            var scaleBy = new CCScaleTo(1.0f, -2.0f);
            var action = new CCSequence(moveTo, moveBack, rotateBy, scaleBy);


            var normalSprite1 = new CCSprite("Images/animationbuttonnormal.png");
            normalSprite1.Position = winSize.Center;
            normalSprite1.PositionX -= 100;
            normalSprite1.PositionY += 100;
            normalSprite1.FlipY = true;


            AddChild(normalSprite1);
            normalSprite1.RunAction(action);

            var normalSprite2 = new CCScale9Sprite("Images/animationbuttonnormal.png");
            normalSprite2.Position = winSize.Center;
            normalSprite2.PositionX -= 80;
            normalSprite2.PositionY += 100;
            normalSprite2.IsScale9Enabled = false;
            normalSprite2.Opacity = 100;
            AddChild(normalSprite2);
            normalSprite2.Color = CCColor3B.Green;
            normalSprite2.RunAction(action);

            
            var sp1 = new CCScale9Sprite("Images/animationbuttonnormal.png");
            sp1.Position = winSize.Center;
            sp1.PositionX -= 100;
            sp1.PositionY -= 50;
            sp1.Scale = 1.2f;
            sp1.ContentSize = new CCSize(100, 100);
            sp1.Color = CCColor3B.Green;
            AddChild(sp1);
            sp1.RunAction(action);

            var sp2 = new CCScale9Sprite("Images/animationbuttonnormal.png");
            sp2.Position = winSize.Center;
            sp2.PositionX += 100;
            sp2.PositionY -= 50;
            sp2.PreferredSize = sp1.ContentSize * 1.2f;
            sp2.ContentSize = new CCSize(100, 100);
            sp2.Color = CCColor3B.Green;
            AddChild(sp2);
            sp2.RunAction(action);

        }

        public override string Title()
        {
            return "Scale9Sprite actions";
        }

        public override string Subtitle()
        {
            return "";
        }
    }


    // S9BatchNodeBasic
    public class S9BatchNodeBasic : S9SpriteTestDemo
    {
        public override void OnEnter()
        {
            base.OnEnter();
            CCSize winSize = Layer.VisibleBoundsWorldspace.Size;
            float x = winSize.Width / 2;
            float y = 0 + (winSize.Height / 2);

            var blocksSprite = new CCSprite("Images/blocks9.png");

            var blocks = new CCScale9Sprite();

            blocks.UpdateWithSprite(blocksSprite, new CCRect(0, 0, 96, 96), false, new CCRect(0, 0, 96, 96));

            blocks.Position = new CCPoint(x, y);

            AddChild(blocks);
        }

        public override string Title()
        {
            return "Scale9Sprite created empty and updated from Sprite";
        }

        public override string Subtitle()
        {
            return "UpdateWithSprite(); capInsets=full size";
        }
    }

    // S9FrameNameSpriteSheet
    public class S9FrameNameSpriteSheet : S9SpriteTestDemo
    {
        public override void OnEnter()
        {
            base.OnEnter();
            CCSize winSize = Layer.VisibleBoundsWorldspace.Size;
            float x = winSize.Width / 2;
            float y = 0 + (winSize.Height / 2);

            var blocks = CCScale9Sprite.SpriteWithFrameName("blocks9.png");

            blocks.Position = new CCPoint(x, y);

            AddChild(blocks);
        }

        public override string Title()
        {
            return "Scale9Sprite from sprite sheet";
        }

        public override string Subtitle()
        {
            return "SpriteWithFrameName(); default cap insets";
        }
    }

    // S9FrameNameSpriteSheetRotated
    public class S9FrameNameSpriteSheetRotated : S9SpriteTestDemo
    {
        public override void OnEnter()
        {
            base.OnEnter();
            CCSize winSize = Layer.VisibleBoundsWorldspace.Size;
            float x = winSize.Width / 2;
            float y = 0 + (winSize.Height / 2);

            var blocks = CCScale9Sprite.SpriteWithFrameName("blocks9r.png");

            blocks.Position = new CCPoint(x, y);

            AddChild(blocks);

        }

        public override string Title()
        {
            return "Scale9Sprite from sprite sheet (stored rotated)";
        }

        public override string Subtitle()
        {
            return "SpriteWithFrameName(); default cap insets";
        }
    }


    // S9FrameNameSpriteSheetCropped
    public class S9FrameNameSpriteSheetCropped : S9SpriteTestDemo
    {
        public override void OnEnter()
        {
            base.OnEnter();
            CCSize winSize = Layer.VisibleBoundsWorldspace.Size;
            var position = winSize.Center;

            CCSpriteFrameCache.SharedSpriteFrameCache.AddSpriteFrames("Images/blocks9ss.plist");

            var blocks = CCScale9Sprite.SpriteWithFrameName("blocks9c.png");

            blocks.Position = position;

            AddChild(blocks);
        }

        public override string Title()
        {
            return "Scale9Sprite from sprite sheet (stored rotated)";
        }

        public override string Subtitle()
        {
            return "SpriteWithFrameName(); default cap insets";
        }
    }

    // S9FrameNameSpriteSheetCroppedRotated
    public class S9FrameNameSpriteSheetCroppedRotated : S9SpriteTestDemo
    {
        public override void OnEnter()
        {
            base.OnEnter();
            CCSize winSize = Layer.VisibleBoundsWorldspace.Size;
            var position = winSize.Center;

            CCSpriteFrameCache.SharedSpriteFrameCache.AddSpriteFrames("Images/blocks9ss.plist");

            var blocks = CCScale9Sprite.SpriteWithFrameName("blocks9cr.png");

            blocks.Position = position;

            AddChild(blocks);
        }

        public override string Title()
        {
            return "Scale9Sprite from sprite sheet (stored rotated)";
        }

        public override string Subtitle()
        {
            return "SpriteWithFrameName(); default cap insets";
        }
    }


    // S9BatchNodeScaledNoInsets
    public class S9BatchNodeScaledNoInsets : S9SpriteTestDemo
    {
        public override void OnEnter()
        {
            base.OnEnter();
            CCSize winSize = Layer.VisibleBoundsWorldspace.Size;
            float x = winSize.Width / 2;
            float y = 0 + (winSize.Height / 2);

            // scaled without insets
            var spriteScaled = new CCSprite("Images/blocks9.png");

            var blocksScaled = new CCScale9Sprite();

            blocksScaled.UpdateWithSprite(spriteScaled, new CCRect(0, 0, 96, 96), false,
                                              new CCRect(0, 0, 96, 96));

            blocksScaled.Position = new CCPoint(x, y);

            blocksScaled.ContentSize = new CCSize(96 * 4, 96 * 2);

            AddChild(blocksScaled);
        }

        public override string Title()
        {
            return "Scale9Sprite created empty and updated from Sprite";
        }

        public override string Subtitle()
        {
            return "UpdateWithSprite(); capInsets=full size; rendered 4 X Width, 2 X Height";
        }
    }

    // S9FrameNameSpriteSheetScaledNoInsets
    public class S9FrameNameSpriteSheetScaledNoInsets : S9SpriteTestDemo
    {
        public override void OnEnter()
        {
            base.OnEnter();
            CCSize winSize = Layer.VisibleBoundsWorldspace.Size;
            float x = winSize.Width / 2;
            float y = 0 + (winSize.Height / 2);

            CCSpriteFrameCache.SharedSpriteFrameCache.AddSpriteFrames("Images/blocks9ss.plist");

            var blocksScaled = CCScale9Sprite.SpriteWithFrameName("blocks9.png");

            blocksScaled.Position = new CCPoint(x, y);

            blocksScaled.ContentSize = new CCSize(96 * 4, 96 * 2);

            AddChild(blocksScaled);
        }

        public override string Title()
        {
            return "Scale9Sprite from sprite sheet";
        }

        public override string Subtitle()
        {
            return "SpriteWithFrameName(); default cap insets; rendered 4 X Width, 2 X Height";
        }
    }

    // S9FrameNameSpriteSheetRotatedScaledNoInsets
    public class S9FrameNameSpriteSheetRotatedScaledNoInsets : S9SpriteTestDemo
    {
        public override void OnEnter()
        {
            base.OnEnter();
            CCSize winSize = Layer.VisibleBoundsWorldspace.Size;
            float x = winSize.Width / 2;
            float y = 0 + (winSize.Height / 2);

            var blocksScaled = CCScale9Sprite.SpriteWithFrameName("blocks9r.png");

            blocksScaled.Position = new CCPoint(x, y);

            blocksScaled.ContentSize = new CCSize(96 * 4, 96 * 2);

            AddChild(blocksScaled);
        }

        public override string Title()
        {
            return "Scale9Sprite from sprite sheet (stored rotated)";
        }

        public override string Subtitle()
        {
            return "SpriteWithFrameName(); default cap insets; rendered 4 X Width, 2 X Height";
        }
    }

    // S9BatchNodeScaleWithCapInsets
    public class S9BatchNodeScaleWithCapInsets : S9SpriteTestDemo
    {
        public override void OnEnter()
        {
            base.OnEnter();
            CCSize winSize = Layer.VisibleBoundsWorldspace.Size;
            float x = winSize.Width / 2;
            float y = 0 + (winSize.Height / 2);

            var spriteScaledWithInsets = new CCSprite("Images/blocks9.png");

            var blocksScaledWithInsets = new CCScale9Sprite();

            blocksScaledWithInsets.UpdateWithSprite(spriteScaledWithInsets, new CCRect(0, 0, 96, 96), false,
                                                          new CCRect(32, 32, 32, 32));

            blocksScaledWithInsets.ContentSize = new CCSize(96 * 4.5f, 96 * 2.5f);

            blocksScaledWithInsets.Position = new CCPoint(x, y);

            AddChild(blocksScaledWithInsets);
        }

        public override string Title()
        {
            return "Scale9Sprite created empty and updated from Sprite";
        }

        public override string Subtitle()
        {
            return "UpdateWithSprite(); capInsets=(32, 32, 32, 32)";
        }
    }

    // S9FrameNameSpriteSheetInsets
    public class S9FrameNameSpriteSheetInsets : S9SpriteTestDemo
    {
        public override void OnEnter()
        {
            base.OnEnter();
            CCSize winSize = Layer.VisibleBoundsWorldspace.Size;
            float x = winSize.Width / 2;
            float y = 0 + (winSize.Height / 2);


            var blocksWithInsets = CCScale9Sprite.SpriteWithFrameName("blocks9.png", new CCRect(32, 32, 32, 32));

            blocksWithInsets.Position = new CCPoint(x, y);

            AddChild(blocksWithInsets);
        }

        public override string Title()
        {
            return "Scale9Sprite scaled with insets sprite sheet";
        }

        public override string Subtitle()
        {
            return "SpriteWithFrameName(); cap insets=(32, 32, 32, 32)";
        }
    }

    // S9FrameNameSpriteSheetInsetsScaled
    public class S9FrameNameSpriteSheetInsetsScaled : S9SpriteTestDemo
    {
        public override void OnEnter()
        {
            base.OnEnter();
            CCSize winSize = Layer.VisibleBoundsWorldspace.Size;
            float x = winSize.Width / 2;
            float y = 0 + (winSize.Height / 2);

            var blocksScaledWithInsets = CCScale9Sprite.SpriteWithFrameName("blocks9.png", new CCRect(32, 32, 32, 32));

            blocksScaledWithInsets.ContentSize = new CCSize(96 * 4.5f, 96 * 2.5f);

            blocksScaledWithInsets.Position = new CCPoint(x, y);

            AddChild(blocksScaledWithInsets);
        }

        public override string Title()
        {
            return "Scale9Sprite scaled with insets sprite sheet";
        }

        public override string Subtitle()
        {
            return "SpriteWithFrameName(); default cap insets; rendered scaled 4.5 X Width, 2.5 X Height";
        }
    }

    // S9FrameNameSpriteSheetRotatedInsets
    public class S9FrameNameSpriteSheetRotatedInsets : S9SpriteTestDemo
    {
        public override void OnEnter()
        {
            base.OnEnter();
            CCSize winSize = Layer.VisibleBoundsWorldspace.Size;
            float x = winSize.Width / 2;
            float y = 0 + (winSize.Height / 2);

            var blocksWithInsets = CCScale9Sprite.SpriteWithFrameName("blocks9r.png", new CCRect(32, 32, 32, 32));

            blocksWithInsets.Position = new CCPoint(x, y);

            AddChild(blocksWithInsets);
        }

        public override string Title()
        {
            return "Scale9Sprite scaled with insets sprite sheet (stored rotated)";
        }

        public override string Subtitle()
        {
            return "SpriteWithFrameName(); cap insets=(32, 32, 32, 32)";
        }
    }

    // S9_TexturePacker
    public class S9_TexturePacker : S9SpriteTestDemo
    {
        public override void OnEnter()
        {
            base.OnEnter();
            CCSize winSize = Layer.VisibleBoundsWorldspace.Size;
            CCSpriteFrameCache.SharedSpriteFrameCache.AddSpriteFrames("Images/ui.plist");

            float x = winSize.Width / 4;
            float y = 0 + (winSize.Height / 2);

            var buttonNormal = CCScale9Sprite.SpriteWithFrameName("button_normal.png");

            buttonNormal.Position = new CCPoint(x, y);

            buttonNormal.ContentSize = new CCSize(14 * 16, 10 * 16);

            AddChild(buttonNormal);

            x = winSize.Width * 3 / 4;

            var buttonActivated = CCScale9Sprite.SpriteWithFrameName("button_actived.png");

            buttonActivated.Position = new CCPoint(x, y);

            buttonActivated.ContentSize = new CCSize(14 * 16, 10 * 16);

            AddChild(buttonActivated);

        }

        public override string Title()
        {
            return "Scale9Sprite from a spritesheet created with TexturePacker";
        }

        public override string Subtitle()
        {
            return "SpriteWithFrameName('button_normal.png'); SpriteWithFrameName('button_actived.png');";
        }
    }

    // S9FrameNameSpriteSheetRotatedInsetsScaled
    public class S9FrameNameSpriteSheetRotatedInsetsScaled : S9SpriteTestDemo
    {
        public override void OnEnter()
        {
            base.OnEnter();
            CCSize winSize = Layer.VisibleBoundsWorldspace.Size;
            float x = winSize.Width / 2;
            float y = 0 + (winSize.Height / 2);

            var blocksScaledWithInsets = CCScale9Sprite.SpriteWithFrameName("blocks9.png", new CCRect(32, 32, 32, 32));

            blocksScaledWithInsets.ContentSize = new CCSize(96 * 4.5f, 96 * 2.5f);

            blocksScaledWithInsets.Position = new CCPoint(x, y);

            AddChild(blocksScaledWithInsets);
        }

        public override string Title()
        {
            return "Scale9Sprite scaled with insets sprite sheet (stored rotated)";
        }

        public override string Subtitle()
        {
            return "SpriteWithFrameName(); default cap insets; rendered scaled 4.5 X Width, 2.5 X Height";
        }
    }

    // S9FrameNameSpriteSheetRotatedInsetsScaled
    public class S9FrameNameSpriteSheetRotatedSetCapInsetLater : S9SpriteTestDemo
    {
        public override void OnEnter()
        {
            base.OnEnter();
            CCSize winSize = Layer.VisibleBoundsWorldspace.Size;
            float x = winSize.Width / 2;
            float y = 0 + (winSize.Height / 2);

            var blocksScaledWithInsets = CCScale9Sprite.SpriteWithFrameName("blocks9r.png");

            blocksScaledWithInsets.InsetLeft = 32;
            blocksScaledWithInsets.InsetRight = 32;

            blocksScaledWithInsets.PreferredSize = new CCSize(32 * 5.5f, 32 * 4);
            blocksScaledWithInsets.Position = new CCPoint(x, y);

            AddChild(blocksScaledWithInsets);
        }

        public override string Title()
        {
            return "Scale9Sprite from sprite sheet (stored rotated), with setting CapInset later";
        }

        public override string Subtitle()
        {
            return "SpriteWithFrameName(); setInsetLeft(32); setInsetRight(32);";
        }
    }

    // S9CascadeOpacityAndColor
    public class S9CascadeOpacityAndColor : S9SpriteTestDemo
    {
        public override void OnEnter()
        {
            base.OnEnter();
            CCSize winSize = Layer.VisibleBoundsWorldspace.Size;
            float x = winSize.Width / 2;
            float y = 0 + (winSize.Height / 2);
            var rgba = new CCLayer();
            rgba.IsColorCascaded = true;
            rgba.IsOpacityCascaded = true;
            AddChild(rgba);

            var blocksScaledWithInsets = CCScale9Sprite.SpriteWithFrameName("blocks9r.png");

            blocksScaledWithInsets.Position = new CCPoint(x, y);

            rgba.AddChild(blocksScaledWithInsets);
            var actions = new CCSequence(new CCFadeIn(1),
                                         new CCTintTo(1, 0, 255, 0),
                                         new CCTintTo(1, 255, 255, 255),
                                         new CCFadeOut(1));
            rgba.RepeatForever(actions);

        }

        public override string Title()
        {
            return
                "Scale9Sprite and a LayerRGBA parent with setCascadeOpacityEnable(true) and setCascadeColorEnable(true)";
        }

        public override string Subtitle()
        {
            return "when parent change color/opacity, Scale9Sprite should also change";
        }
    }

    // S9Flip
    public class S9Flip : S9SpriteTestDemo
    {
        public override void OnEnter()
        {
            base.OnEnter();
            CCSize winSize = Layer.VisibleBoundsWorldspace.Size;
            float x = winSize.Width / 2;
            float y = 0 + (winSize.Height / 2);

            var statusLabel = new CCLabel("Scale9Enabled", "arial", 10, CCLabelFormat.SpriteFont);
            statusLabel.PositionX = x;
            statusLabel.PositionY = winSize.Height - statusLabel.ContentSize.Height - 100;
            AddChild(statusLabel);

            var normalSprite = CCScale9Sprite.SpriteWithFrameName("blocks9r.png");

            normalSprite.PositionX = x;
            normalSprite.PositionY = y;
            AddChild(normalSprite);


            var normalLabel = new CCLabel("Normal Sprite", "Arial", 10, CCLabelFormat.SpriteFont);
            normalLabel.Position += normalSprite.Position +
                new CCPoint(0, normalSprite.ScaledContentSize.Height / 2 + 10);
            AddChild(normalLabel);



            var flipXSprite = CCScale9Sprite.SpriteWithFrameName("blocks9r.png");

            flipXSprite.PositionX = x - 150;
            flipXSprite.PositionY = y;
            flipXSprite.Scale = 1.2f;
            AddChild(flipXSprite);
            flipXSprite.IsFlippedX = false;

            var flipXLabel = new CCLabel("sprite is not flipped!", "Arial", 10, CCLabelFormat.SpriteFont);
            flipXLabel.Position = flipXSprite.Position +
                new CCPoint(0, flipXSprite.ScaledContentSize.Height / 2 + 10);
            AddChild(flipXLabel);


            var flipYSprite = CCScale9Sprite.SpriteWithFrameName("blocks9r.png");

            flipYSprite.PositionX = x + 150;
            flipYSprite.PositionY = y;
            flipYSprite.Scale = 0.8f;
            AddChild(flipYSprite);
            flipYSprite.IsFlippedY = true;

            var flipYLabel = new CCLabel("sprite is flipped!", "Arial", 10, CCLabelFormat.SpriteFont);
            flipYLabel.Position = flipYSprite.Position + 
                new CCPoint(0, flipYSprite.ScaledContentSize.Height / 2 + 10);
            AddChild(flipYLabel);


            var toggleFlipXButton = new CCControlButton("Toggle FlipX", "arial", 12);
            
            toggleFlipXButton.Position = flipXSprite.Position +
                new CCPoint(0, -20 - flipXSprite.ScaledContentSize.Height / 2);
            toggleFlipXButton.AddTargetWithActionForControlEvents(this, (obj, cevent) =>
            {
                flipXSprite.IsFlippedX = !flipXSprite.IsFlippedX;
                if (flipXSprite.IsFlippedX)
                    flipXLabel.Text = "sprite is flipped";
                else
                    flipXLabel.Text = "sprite is not flipped";
            }

            , CCControlEvent.TouchUpInside | CCControlEvent.TouchUpOutside);
            AddChild(toggleFlipXButton);

            var toggleFlipYButton = new CCControlButton("Toggle FlipY", "arial", 12);
            toggleFlipYButton.Position = flipYSprite.Position + 
                new CCPoint(0, -20 - flipYSprite.ScaledContentSize.Height / 2);

            toggleFlipYButton.AddTargetWithActionForControlEvents(this, (obj, cevent) =>
            {
                flipYSprite.IsFlippedY = !flipYSprite.IsFlippedY;
                if (flipYSprite.IsFlippedY)
                    flipYLabel.Text = "sprite is flipped";
                else
                    flipYLabel.Text = "sprite is not flipped";
            }

            , CCControlEvent.TouchUpInside | CCControlEvent.TouchUpOutside);

            AddChild(toggleFlipYButton);

            var toggleScale9Button = new CCControlButton("Toggle Scale9", "arial", 12);
            toggleScale9Button.Position = normalSprite.Position + 
                new CCPoint(0, -20 - normalSprite.ContentSize.Height / 2);

            toggleScale9Button.AddTargetWithActionForControlEvents(this, (obj, cevent) =>
            {
                flipXSprite.IsScale9Enabled = !flipXSprite.IsScale9Enabled;
                flipYSprite.IsScale9Enabled = !flipYSprite.IsScale9Enabled;

                if (flipXSprite.IsScale9Enabled)
                    statusLabel.Text = "Scale9Enabled";
                else
                    statusLabel.Text = "Scale9Disabled";

                if (flipXSprite.IsFlippedX)
                    flipXLabel.Text = "sprite is flipped";
                else
                    flipXLabel.Text = "sprite is not flipped";

                if (flipYSprite.IsFlippedY)
                    flipYLabel.Text = "sprite is flipped";
                else
                    flipYLabel.Text = "sprite is not flipped";
            }

            , CCControlEvent.TouchUpInside | CCControlEvent.TouchUpOutside);

            AddChild(toggleScale9Button);
        }

        public override string Title()
        {
            return
                "Scale9Sprite Flip";
        }

        public override string Subtitle()
        {
            return "FlipX, FlipY and Enable/Disable Scale9Sprite";
        }
    }

}