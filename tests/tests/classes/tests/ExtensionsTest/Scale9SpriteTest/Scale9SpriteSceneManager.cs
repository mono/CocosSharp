using CocosSharp;

namespace tests.Extensions
{
    public class Scale9SpriteSceneManager
    {
        public const int Scale9SpriteTestMax = 14;

        public const int kS9BatchNodeBasic = 0;
        public const int kS9FrameNameSpriteSheet = 1;
        public const int kS9FrameNameSpriteSheetRotated = 2;
        public const int kS9BatchNodeScaledNoInsets = 3;
        public const int kS9FrameNameSpriteSheetScaledNoInsets = 4;
        public const int kS9FrameNameSpriteSheetRotatedScaledNoInsets = 5;
        public const int kS9BatchNodeScaleWithCapInsets = 6;
        public const int kS9FrameNameSpriteSheetInsets = 7;
        public const int kS9FrameNameSpriteSheetInsetsScaled = 8;
        public const int kS9FrameNameSpriteSheetRotatedInsets = 9;
        public const int kS9_TexturePacker = 10;
        public const int kS9FrameNameSpriteSheetRotatedInsetsScaled = 11;
        public const int kS9FrameNameSpriteSheetRotatedSetCapInsetLater = 12;
        public const int kS9CascadeOpacityAndColor = 13;


        private static string[] s_testArray =
            {
                "S9BatchNodeBasicTest",
                "S9FrameNameSpriteSheetTest",
                "S9FrameNameSpriteSheetRotatedTest",
                "S9BatchNodeScaledNoInsetsTest",
                "S9FrameNameSpriteSheetScaledNoInsetsTest",
                "S9FrameNameSpriteSheetRotatedScaledNoInsetsTest",
                "S9BatchNodeScaleWithCapInsetsTest",
                "S9FrameNameSpriteSheetInsetsTest",
                "S9FrameNameSpriteSheetInsetsScaledTest",
                "S9FrameNameSpriteSheetRotatedInsetsTest",
                "S9_TexturePackerTest",
                "S9FrameNameSpriteSheetRotatedInsetsScaledTest",
                "S9FrameNameSpriteSheetRotatedSetCapInsetLaterTest",
                "S9CascadeOpacityAndColorTest"
            };

        private static Scale9SpriteSceneManager sharedInstance;
        protected int m_nCurrentControlSceneId;

        public Scale9SpriteSceneManager()
        {
            m_nCurrentControlSceneId = kS9BatchNodeBasic;
        }


        /** Returns the singleton of the control scene manager. */

        public static Scale9SpriteSceneManager sharedSprite9SceneManager()
        {
            if (sharedInstance == null)
            {
                sharedInstance = new Scale9SpriteSceneManager();
            }
            return sharedInstance;
        }


        /** Returns the next control scene. */

        public CCScene nextControlScene()
        {
            m_nCurrentControlSceneId = (m_nCurrentControlSceneId + 1) % Scale9SpriteTestMax;

            return currentControlScene();
        }

        /** Returns the previous control scene. */

        public CCScene previousControlScene()
        {
            m_nCurrentControlSceneId = m_nCurrentControlSceneId - 1;
            if (m_nCurrentControlSceneId < 0)
            {
                m_nCurrentControlSceneId = Scale9SpriteTestMax - 1;
            }

            return currentControlScene();
        }


        /** Returns the current control scene. */

        public CCScene currentControlScene()
        {
            var scene = new CCScene(AppDelegate.SharedWindow, AppDelegate.SharedViewport);
            switch (m_nCurrentControlSceneId)
            {
                case kS9BatchNodeBasic:
                    scene.AddChild(new S9BatchNodeBasic());
                    break;
                case kS9FrameNameSpriteSheet:
                    scene.AddChild(new S9FrameNameSpriteSheet());
                    break;
                case kS9FrameNameSpriteSheetRotated:
                    scene.AddChild(new S9FrameNameSpriteSheetRotated());
                    break;
                case kS9BatchNodeScaledNoInsets:
                    scene.AddChild(new S9BatchNodeScaledNoInsets());
                    break;
                case kS9FrameNameSpriteSheetScaledNoInsets:
                    scene.AddChild(new S9FrameNameSpriteSheetScaledNoInsets());
                    break;
                case kS9FrameNameSpriteSheetRotatedScaledNoInsets:
                    scene.AddChild(new S9FrameNameSpriteSheetRotatedScaledNoInsets());
                    break;
                case kS9BatchNodeScaleWithCapInsets:
                    scene.AddChild(new S9BatchNodeScaleWithCapInsets());
                    break;
                case kS9FrameNameSpriteSheetInsets:
                    scene.AddChild(new S9FrameNameSpriteSheetInsets());
                    break;
                case kS9FrameNameSpriteSheetInsetsScaled:
                    scene.AddChild(new S9FrameNameSpriteSheetInsetsScaled());
                    break;
                case kS9FrameNameSpriteSheetRotatedInsets:
                    scene.AddChild(new S9FrameNameSpriteSheetRotatedInsets());
                    break;
                case kS9_TexturePacker:
                    scene.AddChild(new S9_TexturePacker());
                    break;
                case kS9FrameNameSpriteSheetRotatedInsetsScaled:
                    scene.AddChild(new S9FrameNameSpriteSheetRotatedInsetsScaled());
                    break;
                case kS9FrameNameSpriteSheetRotatedSetCapInsetLater:
                    scene.AddChild(new S9FrameNameSpriteSheetRotatedSetCapInsetLater());
                    break;
                case kS9CascadeOpacityAndColor:
                    scene.AddChild(new S9CascadeOpacityAndColor());
                    break;
            }
            return scene;
        }

        /** Control scene id. */

        public virtual int getCurrentControlSceneId()
        {
            return m_nCurrentControlSceneId;
        }

        public virtual void setCurrentControlSceneId(int var)
        {
            m_nCurrentControlSceneId = var;
        }
    }
}