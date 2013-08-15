using System;
using System.Collections.Generic;
using System.Linq;

namespace Cocos2D
{
    public enum TestCases
    {
        TEST_ACCELEROMRTER = 0,
        TEST_ACTION_MANAGER,
        TEST_EASE_ACTIONS,
        TEST_PROGRESS_ACTIONS,
        TEST_ACTIONS,
        TEST_BOX2D,
        TEST_BOX2DBED,
        TEST_BOX2DBED2,
        //TEST_BUGS,
        //TEST_CHIPMUNK,
        TEST_CLICK_AND_MOVE,
        TEST_CLIPPINGNODE,
        TEST_COCOSDENSHION,
        //TEST_CURL,
        //TEST_CURRENT_LANGUAGE,
        TEST_DIRECTOR,
        TEST_DRAW_PRIMITIVES,
        TEST_EFFECT_ADVANCE,
        TEST_EFFECTS,
        TEST_EXTENSIONS,
        TEST_FONTS,
        TEST_INTERVAL,
        //TEST_KEYPAD,
        TEST_LABEL,
        TEST_LAYER,
        TEST_MENU,
        TEST_MOTION_STREAK,
        //TEST_MUTITOUCH,
        TEST_COCOSNODE,
        TEST_ORIENTATION,
        TEST_PARALLAX,
        TEST_PARTICLE,
        TEST_PERFORMANCE,
        TEST_RENDERTEXTURE,
        TEST_ROTATE_WORLD,
        TEST_SCENE,
        TEST_SCHEDULER,
        //TEST_SHADER,
        TEST_SPRITE,
        TEST_TEXT_INPUT,
        TEST_TEXTURE2D,
        //TEST_TEXTURECACHE,
        TEST_TILE_MAP,
        TEST_TOUCHES,
        TEST_TRANSITIONS,
        //TEST_USERDEFAULT,
        TEST_ZWOPTEX,
#if IOS || MONOMAC || WINDOWSGL || WINDOWS || ANDROID || NETFX_CORE
        TEST_SYSTEM_FONTS,
#endif
        TESTS_COUNT,
    };


    public class Tests
    {
        public static string[] g_aTestNames = {
            "Accelerometer",
            "ActionManagerTest",
            "ActionsEaseTest",
            "ActionsProgressTest",
            "ActionsTest",
            "Box2dTest",
            "Box2dTestBed(Farseer)",
            "Box2dTestBed(Box2D)",
            //"BugsTest",
            //"ChipmunkTest",
            "ClickAndMoveTest",
            "ClippingNodeTest",
            "CocosDenshionTest",
            //"CurlTest",
            //"CurrentLanguageTest"
            "DirectorTest",
            "DrawPrimitivesTest",
            "EffectAdvancedTest",
            "EffectsTest",
            "ExtensionsTest",
            "FontTest",
            "IntervalTest",
            //"KeypadTest",
            "LabelTest",
            "LayerTest",
            "MenuTest",
            "MotionStreakTest",
            //"MutiTouchTest"
            "NodeTest",
            "OrientationTest",
            "ParallaxTest",
            "ParticleTest",
            "PerformanceTest",
            "RenderTextureTest",
            "RotateWorldTest",
            "SceneTest",
            "SchdulerTest",
            //"ShaderTest",
            "SpriteTest",
            "TextInputTest",
            "Texture2DTest",
            //"TextureCacheTest",
            "TileMapTest",
            "TouchesTest",
            "TransitionsTest",
            //"UserDefaultTest",
            "ZwoptexTest",
#if IOS || MONOMAC || WINDOWSGL || WINDOWS || (ANDROID && !OUYA) || NETFX_CORE
			"SystemFontTest",
#endif
        };
    }

}