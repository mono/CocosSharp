using System;
using System.Collections.Generic;
using CocosSharp;

namespace tests.Extensions
{
    public class Scale9SpriteSceneManager
    {
        public static int Scale9SpriteTestMax = 0;

        static List<Func<S9SpriteTestDemo>> actionTestFunctions = new List<Func<S9SpriteTestDemo>>()
            {
                () => new S9Scale9SpriteTest(),
                () => new S9BatchNodeBasic(),
                () => new S9FrameNameSpriteSheet(),
                () => new S9FrameNameSpriteSheetRotated(),
                () => new S9FrameNameSpriteSheetCropped(),
                () => new S9FrameNameSpriteSheetCroppedRotated(),
                () => new S9BatchNodeScaledNoInsets(),
                () => new S9FrameNameSpriteSheetScaledNoInsets(),
                () => new S9FrameNameSpriteSheetRotatedScaledNoInsets(),
                () => new S9BatchNodeScaleWithCapInsets(),
                () => new S9FrameNameSpriteSheetInsets(),
                () => new S9FrameNameSpriteSheetInsetsScaled(),
                () => new S9FrameNameSpriteSheetRotatedInsets(),
                () => new S9_TexturePacker(),
                () => new S9FrameNameSpriteSheetRotatedInsetsScaled(),
                () => new S9FrameNameSpriteSheetRotatedSetCapInsetLater(),
                () => new S9CascadeOpacityAndColor(),
                () => new S9Flip(),
            };

        private static Scale9SpriteSceneManager sharedInstance;
        protected int currentControlSceneId;

        public Scale9SpriteSceneManager()
        {
            Scale9SpriteTestMax = actionTestFunctions.Count;
            currentControlSceneId = 0;
        }


        /** Returns the singleton of the control scene manager. */

        public static Scale9SpriteSceneManager SharedSprite9SceneManager
        {
            get
            {
                if (sharedInstance == null)
                {
                    sharedInstance = new Scale9SpriteSceneManager();
                }
                return sharedInstance;
            }
        }


        /** Returns the next control scene. */

        public CCScene NextControlScene
        {
            get
            {
                currentControlSceneId = (currentControlSceneId + 1) % Scale9SpriteTestMax;

                return CurrentControlScene;
            }
        }

        /** Returns the previous control scene. */

        public CCScene PreviousControlScene
        {
            get
            {
                currentControlSceneId = currentControlSceneId - 1;
                if (currentControlSceneId < 0)
                {
                    currentControlSceneId = Scale9SpriteTestMax - 1;
                }

                return CurrentControlScene;
            }
        }


        /** Returns the current control scene. */

        public CCScene CurrentControlScene
        {
            get
            {
                var scene = new CCScene(AppDelegate.SharedWindow);
                scene.AddChild(actionTestFunctions[currentControlSceneId]());
                return scene;
            }
        }

        /** Control scene id. */

        public virtual int getCurrentControlSceneId()
        {
            return currentControlSceneId;
        }

        public virtual void setCurrentControlSceneId(int var)
        {
            currentControlSceneId = var;
        }
    }
}