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
                () => new Sprite1(),
                () => new SpriteBatchNode1(),
                () => new SpriteFrameTest(),
                () => new SpriteFrameAliasNameTest(),
                () => new SpriteAnchorPoint(),
                () => new SpriteBatchNodeAnchorPoint(),
                () => new SpriteOffsetAnchorRotation(),
                () => new SpriteBatchNodeOffsetAnchorRotation(),
                () => new SpriteOffsetAnchorScale(),
                () => new SpriteBatchNodeOffsetAnchorScale(),
                () => new SpriteOffsetAnchorSkew(),
                () => new SpriteBatchNodeOffsetAnchorSkew(),
                () => new SpriteOffsetAnchorSkewScale(),
                () => new SpriteBatchNodeOffsetAnchorSkewScale(),
                () => new SpriteAnimationSplit(),
                () => new SpriteColorOpacity(),
                () => new SpriteBatchNodeColorOpacity(),
                () => new SpriteZOrder(),
                () => new SpriteBatchNodeZOrder(),
                () => new SpriteBatchNodeReorder(),
                () => new SpriteBatchNodeReorderIssue744(),
                () => new SpriteBatchNodeReorderIssue766(),
                () => new SpriteBatchNodeReorderIssue767(),
                () => new SpriteZVertex(),
                () => new SpriteBatchNodeZVertex(),
                () => new Sprite6(),
                () => new SpriteFlip(),
                () => new SpriteBatchNodeFlip(),
                () => new SpriteAliased(),
                () => new SpriteBatchNodeAliased(),
                () => new SpriteNewTexture(),
                () => new SpriteBatchNodeNewTexture(),
                () => new SpriteHybrid(),
                () => new SpriteBatchNodeChildren(),
                () => new SpriteBatchNodeChildrenZ(),
                () => new SpriteChildrenVisibility(),
                () => new SpriteChildrenVisibilityIssue665(),
                () => new SpriteChildrenAnchorPoint(),
                () => new SpriteBatchNodeChildrenAnchorPoint(),
                () => new SpriteBatchNodeChildrenScale(),
                () => new SpriteChildrenChildren(),
                () => new SpriteBatchNodeChildrenChildren(),
                () => new SpriteNilTexture(),
                () => new SpriteSubclass(),
                () => new AnimationCache(),
                () => new SpriteOffsetAnchorFlip(),
                () => new SpriteBatchNodeOffsetAnchorFlip(),
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
