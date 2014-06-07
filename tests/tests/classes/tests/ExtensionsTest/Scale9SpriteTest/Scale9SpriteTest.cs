using CocosSharp;

namespace tests.Extensions
{
    public class S9SpriteTestDemo : CCControlScene
    {
        public override void OnEnter()
        {
            base.OnEnter();
            CCSpriteFrameCache.Instance.AddSpriteFrames("Images/blocks9ss.plist");
            getSceneTitleLabel().Text = Title();
            getSceneSubtitleLabel().Text = Subtitle();
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
            CCApplication.SharedApplication.MainWindowDirector.ReplaceScene(
                Scale9SpriteSceneManager.sharedSprite9SceneManager().previousControlScene());
        }

        public override void restartCallback(object sender)
        {
            CCApplication.SharedApplication.MainWindowDirector.ReplaceScene(
                Scale9SpriteSceneManager.sharedSprite9SceneManager().currentControlScene());
        }

        public override void nextCallback(object sender)
        {
            CCApplication.SharedApplication.MainWindowDirector.ReplaceScene(
                Scale9SpriteSceneManager.sharedSprite9SceneManager().nextControlScene());
        }
    }

    // S9BatchNodeBasic
    public class S9BatchNodeBasic : S9SpriteTestDemo
    {
        public override void OnEnter()
        {
            base.OnEnter();
            CCSize winSize = CCApplication.SharedApplication.MainWindowDirector.WinSize;
            float x = winSize.Width / 2;
            float y = 0 + (winSize.Height / 2);

            CCLog.Log("S9BatchNodeBasic ...");

            var batchNode = new CCSpriteBatchNode("Images/blocks9.png");
            //batchNode.AnchorPoint = new CCPoint(0.5f, 0.5f);
            CCLog.Log("batchNode created with : Images/blocks9.png");

            var blocks = new CCScale9Sprite();
            CCLog.Log("... created");

            blocks.UpdateWithBatchNode(batchNode, new CCRect(0, 0, 96, 96), false, new CCRect(0, 0, 96, 96));
            CCLog.Log("... updateWithBatchNode");

            blocks.Position = new CCPoint(x, y);
            CCLog.Log("... setPosition");

            AddChild(blocks);
            CCLog.Log("AddChild");

            CCLog.Log("... S9BatchNodeBasic done.");
        }

        public override string Title()
        {
            return "Scale9Sprite created empty and updated from SpriteBatchNode";
        }

        public override string Subtitle()
        {
            return "updateWithBatchNode(); capInsets=full size";
        }
    }

    // S9FrameNameSpriteSheet
    public class S9FrameNameSpriteSheet : S9SpriteTestDemo
    {
        public override void OnEnter()
        {
            base.OnEnter();
            CCSize winSize = CCApplication.SharedApplication.MainWindowDirector.WinSize;
            float x = winSize.Width / 2;
            float y = 0 + (winSize.Height / 2);

            CCLog.Log("S9FrameNameSpriteSheet ...");

            var blocks = CCScale9Sprite.SpriteWithFrameName("blocks9.png");
            CCLog.Log("... created");

            blocks.Position = new CCPoint(x, y);
            CCLog.Log("... setPosition");

            AddChild(blocks);
            CCLog.Log("AddChild");

            CCLog.Log("... S9FrameNameSpriteSheet done.");
        }

        public override string Title()
        {
            return "Scale9Sprite from sprite sheet";
        }

        public override string Subtitle()
        {
            return "createWithSpriteFrameName(); default cap insets";
        }
    }

    // S9FrameNameSpriteSheetRotated
    public class S9FrameNameSpriteSheetRotated : S9SpriteTestDemo
    {
        public override void OnEnter()
        {
            base.OnEnter();
            CCSize winSize = CCApplication.SharedApplication.MainWindowDirector.WinSize;
            float x = winSize.Width / 2;
            float y = 0 + (winSize.Height / 2);

            CCLog.Log("S9FrameNameSpriteSheetRotated ...");

            var blocks = CCScale9Sprite.SpriteWithFrameName("blocks9r.png");

            CCLog.Log("... created");

            blocks.Position = new CCPoint(x, y);
            CCLog.Log("... setPosition");

            AddChild(blocks);
            CCLog.Log("AddChild");

            CCLog.Log("... S9FrameNameSpriteSheetRotated done.");
        }

        public override string Title()
        {
            return "Scale9Sprite from sprite sheet (stored rotated)";
        }

        public override string Subtitle()
        {
            return "createWithSpriteFrameName(); default cap insets";
        }
    }

    // S9BatchNodeScaledNoInsets
    public class S9BatchNodeScaledNoInsets : S9SpriteTestDemo
    {
        public override void OnEnter()
        {
            base.OnEnter();
            CCSize winSize = CCApplication.SharedApplication.MainWindowDirector.WinSize;
            float x = winSize.Width / 2;
            float y = 0 + (winSize.Height / 2);

            CCLog.Log("S9BatchNodeScaledNoInsets ...");

            // scaled without insets
            var batchNode_scaled = new CCSpriteBatchNode("Images/blocks9.png");
            CCLog.Log("batchNode_scaled created with : Images/blocks9.png");

            var blocks_scaled = new CCScale9Sprite();
            CCLog.Log("... created");
            blocks_scaled.UpdateWithBatchNode(batchNode_scaled, new CCRect(0, 0, 96, 96), false,
                                              new CCRect(0, 0, 96, 96));
            CCLog.Log("... updateWithBatchNode");

            blocks_scaled.Position = new CCPoint(x, y);
            CCLog.Log("... setPosition");

            blocks_scaled.ContentSize = new CCSize(96 * 4, 96 * 2);
            CCLog.Log("... setContentSize");

            AddChild(blocks_scaled);
            CCLog.Log("AddChild");

            CCLog.Log("... S9BtchNodeScaledNoInsets done.");
        }

        public override string Title()
        {
            return "Scale9Sprite created empty and updated from SpriteBatchNode";
        }

        public override string Subtitle()
        {
            return "updateWithBatchNode(); capInsets=full size; rendered 4 X Width, 2 X Height";
        }
    }

    // S9FrameNameSpriteSheetScaledNoInsets
    public class S9FrameNameSpriteSheetScaledNoInsets : S9SpriteTestDemo
    {
        public override void OnEnter()
        {
            base.OnEnter();
            CCSize winSize = CCApplication.SharedApplication.MainWindowDirector.WinSize;
            float x = winSize.Width / 2;
            float y = 0 + (winSize.Height / 2);

            CCLog.Log("S9FrameNameSpriteSheetScaledNoInsets ...");

            var blocks_scaled = CCScale9Sprite.SpriteWithFrameName("blocks9.png");

            CCLog.Log("... created");

            blocks_scaled.Position = new CCPoint(x, y);
            CCLog.Log("... setPosition");

            blocks_scaled.ContentSize = new CCSize(96 * 4, 96 * 2);
            CCLog.Log("... setContentSize");

            AddChild(blocks_scaled);
            CCLog.Log("AddChild");

            CCLog.Log("... S9FrameNameSpriteSheetScaledNoInsets done.");
        }

        public override string Title()
        {
            return "Scale9Sprite from sprite sheet";
        }

        public override string Subtitle()
        {
            return "createWithSpriteFrameName(); default cap insets; rendered 4 X Width, 2 X Height";
        }
    }

    // S9FrameNameSpriteSheetRotatedScaledNoInsets
    public class S9FrameNameSpriteSheetRotatedScaledNoInsets : S9SpriteTestDemo
    {
        public override void OnEnter()
        {
            base.OnEnter();
            CCSize winSize = CCApplication.SharedApplication.MainWindowDirector.WinSize;
            float x = winSize.Width / 2;
            float y = 0 + (winSize.Height / 2);

            CCLog.Log("S9FrameNameSpriteSheetRotatedScaledNoInsets ...");

            var blocks_scaled = CCScale9Sprite.SpriteWithFrameName("blocks9r.png");
            CCLog.Log("... created");

            blocks_scaled.Position = new CCPoint(x, y);
            CCLog.Log("... setPosition");

            blocks_scaled.ContentSize = new CCSize(96 * 4, 96 * 2);
            CCLog.Log("... setContentSize");

            AddChild(blocks_scaled);
            CCLog.Log("AddChild");

            CCLog.Log("... S9FrameNameSpriteSheetRotatedScaledNoInsets done.");
        }

        public override string Title()
        {
            return "Scale9Sprite from sprite sheet (stored rotated)";
        }

        public override string Subtitle()
        {
            return "createWithSpriteFrameName(); default cap insets; rendered 4 X Width, 2 X Height";
        }
    }

    // S9BatchNodeScaleWithCapInsets
    public class S9BatchNodeScaleWithCapInsets : S9SpriteTestDemo
    {
        public override void OnEnter()
        {
            base.OnEnter();
            CCSize winSize = CCApplication.SharedApplication.MainWindowDirector.WinSize;
            float x = winSize.Width / 2;
            float y = 0 + (winSize.Height / 2);

            CCLog.Log("S9BatchNodeScaleWithCapInsets ...");

            var batchNode_scaled_with_insets = new CCSpriteBatchNode("Images/blocks9.png");
            CCLog.Log("batchNode_scaled_with_insets created with : Images/blocks9.png");

            var blocks_scaled_with_insets = new CCScale9Sprite();
            CCLog.Log("... created");

            blocks_scaled_with_insets.UpdateWithBatchNode(batchNode_scaled_with_insets, new CCRect(0, 0, 96, 96), false,
                                                          new CCRect(32, 32, 32, 32));
            CCLog.Log("... updateWithBatchNode");

            blocks_scaled_with_insets.ContentSize = new CCSize(96 * 4.5f, 96 * 2.5f);
            CCLog.Log("... setContentSize");

            blocks_scaled_with_insets.Position = new CCPoint(x, y);
            CCLog.Log("... setPosition");

            AddChild(blocks_scaled_with_insets);
            CCLog.Log("AddChild");

            CCLog.Log("... S9BatchNodeScaleWithCapInsets done.");
        }

        public override string Title()
        {
            return "Scale9Sprite created empty and updated from SpriteBatchNode";
        }

        public override string Subtitle()
        {
            return "updateWithBatchNode(); capInsets=(32, 32, 32, 32)";
        }
    }

    // S9FrameNameSpriteSheetInsets
    public class S9FrameNameSpriteSheetInsets : S9SpriteTestDemo
    {
        public override void OnEnter()
        {
            base.OnEnter();
            CCSize winSize = CCApplication.SharedApplication.MainWindowDirector.WinSize;
            float x = winSize.Width / 2;
            float y = 0 + (winSize.Height / 2);

            CCLog.Log("S9FrameNameSpriteSheetInsets ...");

            var blocks_with_insets = CCScale9Sprite.SpriteWithFrameName("blocks9.png", new CCRect(32, 32, 32, 32));
            CCLog.Log("... created");

            blocks_with_insets.Position = new CCPoint(x, y);
            CCLog.Log("... setPosition");

            AddChild(blocks_with_insets);
            CCLog.Log("AddChild");

            CCLog.Log("... S9FrameNameSpriteSheetInsets done.");
        }

        public override string Title()
        {
            return "Scale9Sprite scaled with insets sprite sheet";
        }

        public override string Subtitle()
        {
            return "createWithSpriteFrameName(); cap insets=(32, 32, 32, 32)";
        }
    }

    // S9FrameNameSpriteSheetInsetsScaled
    public class S9FrameNameSpriteSheetInsetsScaled : S9SpriteTestDemo
    {
        public override void OnEnter()
        {
            base.OnEnter();
            CCSize winSize = CCApplication.SharedApplication.MainWindowDirector.WinSize;
            float x = winSize.Width / 2;
            float y = 0 + (winSize.Height / 2);

            CCLog.Log("S9FrameNameSpriteSheetInsetsScaled ...");

            var blocks_scaled_with_insets = CCScale9Sprite.SpriteWithFrameName("blocks9.png", new CCRect(32, 32, 32, 32));
            CCLog.Log("... created");

            blocks_scaled_with_insets.ContentSize = new CCSize(96 * 4.5f, 96 * 2.5f);
            CCLog.Log("... setContentSize");

            blocks_scaled_with_insets.Position = new CCPoint(x, y);
            CCLog.Log("... setPosition");

            AddChild(blocks_scaled_with_insets);
            CCLog.Log("AddChild");

            CCLog.Log("... S9FrameNameSpriteSheetInsetsScaled done.");
        }

        public override string Title()
        {
            return "Scale9Sprite scaled with insets sprite sheet";
        }

        public override string Subtitle()
        {
            return "createWithSpriteFrameName(); default cap insets; rendered scaled 4.5 X Width, 2.5 X Height";
        }
    }

    // S9FrameNameSpriteSheetRotatedInsets
    public class S9FrameNameSpriteSheetRotatedInsets : S9SpriteTestDemo
    {
        public override void OnEnter()
        {
            base.OnEnter();
            CCSize winSize = CCApplication.SharedApplication.MainWindowDirector.WinSize;
            float x = winSize.Width / 2;
            float y = 0 + (winSize.Height / 2);

            CCLog.Log("S9FrameNameSpriteSheetRotatedInsets ...");

            var blocks_with_insets = CCScale9Sprite.SpriteWithFrameName("blocks9r.png", new CCRect(32, 32, 32, 32));
            CCLog.Log("... created");

            blocks_with_insets.Position = new CCPoint(x, y);
            CCLog.Log("... setPosition");

            AddChild(blocks_with_insets);
            CCLog.Log("AddChild");

            CCLog.Log("... S9FrameNameSpriteSheetRotatedInsets done.");
        }

        public override string Title()
        {
            return "Scale9Sprite scaled with insets sprite sheet (stored rotated)";
        }

        public override string Subtitle()
        {
            return "createWithSpriteFrameName(); cap insets=(32, 32, 32, 32)";
        }
    }

    // S9_TexturePacker
    public class S9_TexturePacker : S9SpriteTestDemo
    {
        public override void OnEnter()
        {
            base.OnEnter();
            CCSize winSize = CCApplication.SharedApplication.MainWindowDirector.WinSize;
            CCSpriteFrameCache.Instance.AddSpriteFrames("Images/ui.plist");

            float x = winSize.Width / 4;
            float y = 0 + (winSize.Height / 2);

            CCLog.Log("S9_TexturePacker ...");

            var s = CCScale9Sprite.SpriteWithFrameName("button_normal.png");
            CCLog.Log("... created");

            s.Position = new CCPoint(x, y);
            CCLog.Log("... setPosition");

            s.ContentSize = new CCSize(14 * 16, 10 * 16);
            CCLog.Log("... setContentSize");

            AddChild(s);
            CCLog.Log("AddChild");

            x = winSize.Width * 3 / 4;

            var s2 = CCScale9Sprite.SpriteWithFrameName("button_actived.png");
            CCLog.Log("... created");

            s2.Position = new CCPoint(x, y);
            CCLog.Log("... setPosition");

            s2.ContentSize = new CCSize(14 * 16, 10 * 16);
            CCLog.Log("... setContentSize");

            AddChild(s2);
            CCLog.Log("AddChild");

            CCLog.Log("... S9_TexturePacker done.");
        }

        public override string Title()
        {
            return "Scale9Sprite from a spritesheet created with TexturePacker";
        }

        public override string Subtitle()
        {
            return "createWithSpriteFrameName('button_normal.png');createWithSpriteFrameName('button_actived.png');";
        }
    }

    // S9FrameNameSpriteSheetRotatedInsetsScaled
    public class S9FrameNameSpriteSheetRotatedInsetsScaled : S9SpriteTestDemo
    {
        public override void OnEnter()
        {
            base.OnEnter();
            CCSize winSize = CCApplication.SharedApplication.MainWindowDirector.WinSize;
            float x = winSize.Width / 2;
            float y = 0 + (winSize.Height / 2);

            CCLog.Log("S9FrameNameSpriteSheetRotatedInsetsScaled ...");

            var blocks_scaled_with_insets = CCScale9Sprite.SpriteWithFrameName("blocks9.png", new CCRect(32, 32, 32, 32));
            CCLog.Log("... created");

            blocks_scaled_with_insets.ContentSize = new CCSize(96 * 4.5f, 96 * 2.5f);
            CCLog.Log("... setContentSize");

            blocks_scaled_with_insets.Position = new CCPoint(x, y);
            CCLog.Log("... setPosition");

            AddChild(blocks_scaled_with_insets);
            CCLog.Log("AddChild");

            CCLog.Log("... S9FrameNameSpriteSheetRotatedInsetsScaled done.");
        }

        public override string Title()
        {
            return "Scale9Sprite scaled with insets sprite sheet (stored rotated)";
        }

        public override string Subtitle()
        {
            return "createWithSpriteFrameName(); default cap insets; rendered scaled 4.5 X Width, 2.5 X Height";
        }
    }

    // S9FrameNameSpriteSheetRotatedInsetsScaled
    public class S9FrameNameSpriteSheetRotatedSetCapInsetLater : S9SpriteTestDemo
    {
        public override void OnEnter()
        {
            base.OnEnter();
            CCSize winSize = CCApplication.SharedApplication.MainWindowDirector.WinSize;
            float x = winSize.Width / 2;
            float y = 0 + (winSize.Height / 2);

            CCLog.Log("Scale9FrameNameSpriteSheetRotatedSetCapInsetLater ...");

            var blocks_scaled_with_insets = CCScale9Sprite.SpriteWithFrameName("blocks9r.png");
            CCLog.Log("... created");

            blocks_scaled_with_insets.InsetLeft = 32;
            blocks_scaled_with_insets.InsetRight = 32;

            blocks_scaled_with_insets.PreferredSize = new CCSize(32 * 5.5f, 32 * 4);
            blocks_scaled_with_insets.Position = new CCPoint(x, y);
            CCLog.Log("... setPosition");

            AddChild(blocks_scaled_with_insets);
            CCLog.Log("AddChild");

            CCLog.Log("... Scale9FrameNameSpriteSheetRotatedSetCapInsetLater done.");
        }

        public override string Title()
        {
            return "Scale9Sprite from sprite sheet (stored rotated), with setting CapInset later";
        }

        public override string Subtitle()
        {
            return "createWithSpriteFrameName(); setInsetLeft(32); setInsetRight(32);";
        }
    }

    // S9CascadeOpacityAndColor
    public class S9CascadeOpacityAndColor : S9SpriteTestDemo
    {
        public override void OnEnter()
        {
            base.OnEnter();
            CCSize winSize = CCApplication.SharedApplication.MainWindowDirector.WinSize;
            float x = winSize.Width / 2;
            float y = 0 + (winSize.Height / 2);
            var rgba = new CCLayerRGBA();
            rgba.IsColorCascaded = true;
            rgba.IsOpacityCascaded = true;
            AddChild(rgba);

            CCLog.Log("S9CascadeOpacityAndColor ...");

            var blocks_scaled_with_insets = CCScale9Sprite.SpriteWithFrameName("blocks9r.png");
            CCLog.Log("... created");

            blocks_scaled_with_insets.Position = new CCPoint(x, y);
            CCLog.Log("... setPosition");

            rgba.AddChild(blocks_scaled_with_insets);
            var actions = new CCSequence(new CCFadeIn(1),
                                         new CCTintTo(1, 0, 255, 0),
                                         new CCTintTo(1, 255, 255, 255),
                                         new CCFadeOut(1));
            var repeat = new CCRepeatForever(actions);
            rgba.RunAction(repeat);
            CCLog.Log("AddChild");

            CCLog.Log("... S9CascadeOpacityAndColor done.");
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
}