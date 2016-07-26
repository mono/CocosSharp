using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
    public enum kTags : int
    {
        kTagTileMap = 1,
        kTagSpriteBatchNode = 1,
        kTagNode = 2,
        kTagAnimation1 = 1,
        kTagSpriteLeft,
        kTagSpriteRight,
    }

    public enum kTagSprite
    {
        kTagSprite1,
        kTagSprite2,
        kTagSprite3,
        kTagSprite4,
        kTagSprite5,
        kTagSprite6,
        kTagSprite7,
        kTagSprite8,
    }

    public class SpriteTestScene : TestScene
    {

        public SpriteTestScene() : base()
        {
            MAX_LAYER = createSpriteTestLayerFunctions.Length;
        }
        public override void runThisTest()
        {
            CCLayer layer = NextSpriteTestAction();
            AddChild(layer);

            Director.ReplaceScene(this);
        }

        static int sceneIdx = -1;
        static int MAX_LAYER = 0;

        static Func<CCLayer>[] createSpriteTestLayerFunctions =
            {
                () => new SpriteUntrimmedSizeInPixels(),
                () => new Sprite1(),
                () => new SpriteFrameTest(),
                () => new SpriteFrameAliasNameTest(),
                () => new SpriteAnchorPoint(),
                () => new SpriteOffsetAnchorRotation(),
                () => new SpriteOffsetAnchorScale(),
                () => new SpriteOffsetAnchorSkew(),
                () => new SpriteOffsetAnchorSkewScale(),
                () => new SpriteAnimationSplit(),
                () => new SpriteColorOpacity(),
                () => new SpriteZOrder(),
                () => new SpriteZVertex(),
                () => new Sprite6(),
                () => new SpriteFlip(),
                () => new SpriteAliased(),
                () => new SpriteNewTexture(),
                () => new SpriteHybrid(),
                () => new SpriteChildrenVisibility(),
                () => new SpriteChildrenVisibilityIssue665(),
                () => new SpriteChildrenAnchorPoint(),
                () => new SpriteChildrenChildren(),
                () => new SpriteNilTexture(),
                () => new SpriteSubclass(),
                () => new AnimationCache(),
                () => new SpriteOffsetAnchorFlip(),
                () => new SpriteMaskTest(),
            };

        public static CCLayer createSpriteTestLayer(int index)
        {
            return createSpriteTestLayerFunctions[index]();

        }

        protected override void NextTestCase()
        {
            NextSpriteTestAction();
        }

        protected override void PreviousTestCase()
        {
            BackSpriteTestAction();
        }

        protected override void RestTestCase()
        {
            RestartSpriteTestAction();
        }

        public static CCLayer NextSpriteTestAction()
        {
            sceneIdx++;
            sceneIdx = sceneIdx % MAX_LAYER;

            CCLayer layer = createSpriteTestLayer(sceneIdx);
            return layer;
        }

        public static CCLayer BackSpriteTestAction()
        {
            sceneIdx--;
            int total = MAX_LAYER;
            if (sceneIdx < 0)
                sceneIdx += total;

            CCLayer layer = createSpriteTestLayer(sceneIdx);

            return layer;
        }

        public static CCLayer RestartSpriteTestAction()
        {
            CCLayer layer = createSpriteTestLayer(sceneIdx);

            return layer;
        }
    }
}
