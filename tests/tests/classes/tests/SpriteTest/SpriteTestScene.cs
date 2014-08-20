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
        public override void runThisTest()
        {
            CCLayer layer = NextSpriteTestAction();
            AddChild(layer);

            Director.ReplaceScene(this);
        }

        static int sceneIdx = -1;
        static int MAX_LAYER = 48;

        public static CCLayer createSpriteTestLayer(int index)
        {
            switch (index)
            {
            case 0: return new Sprite1();
            case 1: return new SpriteBatchNode1();
            case 2: return new SpriteFrameTest();
            case 3: return new SpriteFrameAliasNameTest();
            case 4: return new SpriteAnchorPoint();
            case 5: return new SpriteBatchNodeAnchorPoint();
            case 6: return new SpriteOffsetAnchorRotation();
            case 7: return new SpriteBatchNodeOffsetAnchorRotation();
            case 8: return new SpriteOffsetAnchorScale();
            case 9: return new SpriteBatchNodeOffsetAnchorScale();
            case 10: return new SpriteOffsetAnchorSkew();
            case 11: return new SpriteBatchNodeOffsetAnchorSkew();
            case 12: return new SpriteOffsetAnchorSkewScale();
            case 13: return new SpriteBatchNodeOffsetAnchorSkewScale();
            case 14: return new SpriteAnimationSplit();
            case 15: return new SpriteColorOpacity();
            case 16: return new SpriteBatchNodeColorOpacity();
            case 17: return new SpriteZOrder();
            case 18: return new SpriteBatchNodeZOrder();
            case 19: return new SpriteBatchNodeReorder();
            case 20: return new SpriteBatchNodeReorderIssue744();
            case 21: return new SpriteBatchNodeReorderIssue766();
            case 22: return new SpriteBatchNodeReorderIssue767();
            case 23: return new SpriteZVertex();
            case 24: return new SpriteBatchNodeZVertex();
            case 25: return new Sprite6();
            case 26: return new SpriteFlip();
            case 27: return new SpriteBatchNodeFlip();
            case 28: return new SpriteAliased();
            case 29: return new SpriteBatchNodeAliased();
            case 30: return new SpriteNewTexture();
            case 31: return new SpriteBatchNodeNewTexture();
            case 32: return new SpriteHybrid();
            case 33: return new SpriteBatchNodeChildren();
            case 34: return new SpriteBatchNodeChildrenZ();
            case 35: return new SpriteChildrenVisibility();
            case 36: return new SpriteChildrenVisibilityIssue665();
            case 37: return new SpriteChildrenAnchorPoint();
            case 38: return new SpriteBatchNodeChildrenAnchorPoint();
            case 39: return new SpriteBatchNodeChildrenScale();
            case 40: return new SpriteChildrenChildren();
            case 41: return new SpriteBatchNodeChildrenChildren();
            case 42: return new SpriteNilTexture();
            case 43: return new SpriteSubclass();
            case 44: return new AnimationCache();
            case 45: return new SpriteOffsetAnchorFlip();
            case 46: return new SpriteBatchNodeOffsetAnchorFlip();
            case 47: return new SpriteMaskTest();

                //case 47: return new SpriteBatchNodeReorderSameIndex();
                //case 48: return new SpriteBatchNodeReorderOneChild();
                //case 49: return new NodeSort();
                //case 50: return new SpriteSkewNegativeScaleChildren();
                //case 51: return new SpriteBatchNodeSkewNegativeScaleChildren();
                //case 52: return new SpriteDoubleResolution();
                //case 53: return new SpriteBatchBug1217();
                //case 54: return new AnimationCacheFile();
            }

            return null;
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
