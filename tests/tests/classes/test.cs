using System;
using System.Collections.Generic;
using System.Linq;

namespace Cocos2D
{
    public enum TestCases
    {
        TEST_ACTIONS = 0,
        TEST_TRANSITIONS,
        TEST_PROGRESS_ACTIONS,
        TEST_EFFECTS,
        TEST_CLICK_AND_MOVE,
        TEST_ROTATE_WORLD,
        TEST_PARTICLE,
        TEST_EASE_ACTIONS,
        TEST_MOTION_STREAK,
        TEST_DRAW_PRIMITIVES,
        TEST_COCOSNODE,
        TEST_TOUCHES,
        TEST_MENU,
        TEST_ACTION_MANAGER,
        TEST_LAYER,
        TEST_SCENE,
        TEST_PARALLAX,
        TEST_TILE_MAP,
        TEST_INTERVAL,
        //TEST_CHIPMUNK,
        TEST_LABEL,
        TEST_TEXT_INPUT,
        TEST_SPRITE,
        TEST_SCHEDULER,
        TEST_RENDERTEXTURE,
        TEST_TEXTURE2D,
        TEST_BOX2D,
        TEST_BOX2DBED,
        TEST_BOX2DBED2,
        TEST_EFFECT_ADVANCE,
        TEST_ACCELEROMRTER,
        //TEST_KEYPAD,
        TEST_COCOSDENSHION,
        TEST_PERFORMANCE,
        TEST_ZWOPTEX,
        //TEST_CURL,
        //TEST_USERDEFAULT,
        TEST_DIRECTOR,
        //TEST_BUGS,
        TEST_FONTS,
#if IOS || MONOMAC || WINDOWSGL || WINDOWS || ANDROID || NETFX_CORE
        TEST_SYSTEM_FONTS,
#endif
        //TEST_CURRENT_LANGUAGE,
        //TEST_TEXTURECACHE,
        TEST_EXTENSIONS,
        //TEST_SHADER,
        //TEST_MUTITOUCH,
        TEST_CLIPPINGNODE,
        TEST_ORIENTATION,
        TESTS_COUNT,
    };


    public class Tests
    {
        public static string[] g_aTestNames = {
            "ActionsTest",
            "TransitionsTest",
            "ProgressActionsTest",
            "EffectsTest",
            "ClickAndMoveTest",
            "RotateWorldTest",
            "ParticleTest",
            "EaseActionsTest",
            "MotionStreakTest",
            "DrawPrimitivesTest",
            "NodeTest",
            "TouchesTest",
            "MenuTest",
            "ActionManagerTest",
            "LayerTest",
            "SceneTest",
            "ParallaxTest",
            "TileMapTest",
            "IntervalTest",
            //"ChipmunkTest",
            "LabelTest",
            "TextInputTest",
            "SpriteTest",
            "SchdulerTest",
            "RenderTextureTest",
            "Texture2DTest",
            "Box2dTest",
            "Box2dTestBed(Farseer)",
            "Box2dTestBed(Box2D)",
            "EffectAdvancedTest",
            "Accelerometer",
            //"KeypadTest",
            "CocosDenshionTest",
            "PerformanceTest",
            "ZwoptexTest",
            //"CurlTest",
            //"UserDefaultTest",
            "DirectorTest",
            //"BugsTest",
            "FontTest",
#if IOS || MONOMAC || WINDOWSGL || WINDOWS || (ANDROID && !OUYA) || NETFX_CORE
			"SystemFontTest",
#endif
            //"CurrentLanguageTest"
            //"TextureCacheTest",
            "ExtensionsTest",
            //"ShaderTest",
            //"MutiTouchTest"
            "ClippingNodeTest",
            "OrientationTest"
        };
    }

}