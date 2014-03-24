using System;
using CocosSharp;

namespace tests.Extensions
{
    public class HelloCocosBuilderLayer : BaseLayer
    {
        public CCLabelTtf mTestTitleLabelTTF;
        public CCSprite mBurstSprite;

        public int mCustomPropertyInt;
        public float mCustomPropertyFloat;
        public bool mCustomPropertyBoolean;
        public string mCustomPropertyString;

        public override void OnNodeLoaded(CCNode node, CCNodeLoader nodeLoader)
        {
            CCRotateBy ccRotateBy = new CCRotateBy (20.0f, 360);
            CCRepeatForever ccRepeatForever = new CCRepeatForever (ccRotateBy);
            mBurstSprite.RunAction(ccRepeatForever);
        }

        public void openTest(string pCCBFileName, string pCCNodeName, CCNodeLoader pCCNodeLoader)
        {
            /* Create an autorelease CCNodeLoaderLibrary. */
            CCNodeLoaderLibrary ccNodeLoaderLibrary = new CCNodeLoaderLibrary();

            ccNodeLoaderLibrary.RegisterCCNodeLoader("TestHeaderLayer", new Loader<TestHeaderLayer>());
            if (pCCNodeName != null && pCCNodeLoader != null)
            {
                ccNodeLoaderLibrary.RegisterCCNodeLoader(pCCNodeName, pCCNodeLoader);
            }

            /* Create an autorelease CCBReader. */
            var ccbReader = new CCBReader(ccNodeLoaderLibrary);

            /* Read a ccbi file. */
            // Load the scene from the ccbi-file, setting this class as
            // the owner will cause lblTestTitle to be set by the CCBReader.
            // lblTestTitle is in the TestHeader.ccbi, which is referenced
            // from each of the test scenes.
            CCNode node = ccbReader.ReadNodeGraphFromFile(pCCBFileName, this);

            mTestTitleLabelTTF.Text = (pCCBFileName);

            CCScene scene = new CCScene();
            scene.AddChild(node);

            /* Push the new scene with a fancy transition. */
            CCColor3B transitionColor = new CCColor3B();
            transitionColor.R = 0;
            transitionColor.G = 0;
            transitionColor.B = 0;

            CCDirector.SharedDirector.PushScene(new CCTransitionFade(0.5f, scene, transitionColor));
        }

        public void onMenuTestClicked(object pSender, CCControlEvent pCCControlEvent)
        {
            openTest("ccb/ccb/TestMenus.ccbi", "TestMenusLayer", new Loader<MenuTestLayer>());
        }

        public void onSpriteTestClicked(object pSender, CCControlEvent pCCControlEvent)
        {
            openTest("ccb/ccb/TestSprites.ccbi", "TestSpritesLayer", new Loader<SpriteTestLayer>());
        }

        public void onButtonTestClicked(object pSender, CCControlEvent pCCControlEvent)
        {
            openTest("ccb/ccb/TestButtons.ccbi", "TestButtonsLayer", new Loader<ButtonTestLayer>());
        }

        public void onAnimationsTestClicked(object pSender, CCControlEvent pCCControlEvent)
        {
            // Load node graph (TestAnimations is a sub class of CCLayer) and retrieve the ccb action manager
            CCBAnimationManager actionManager = null;

            /* Create an autorelease CCNodeLoaderLibrary. */
            CCNodeLoaderLibrary ccNodeLoaderLibrary = new CCNodeLoaderLibrary();

            ccNodeLoaderLibrary.RegisterCCNodeLoader("TestHeaderLayer", new Loader<TestHeaderLayer>());
            ccNodeLoaderLibrary.RegisterCCNodeLoader("TestAnimationsLayer", new Loader<AnimationsTestLayer>());


            /* Create an autorelease CCBReader. */
            var ccbReader = new CCBReader(ccNodeLoaderLibrary);

            /* Read a ccbi file. */
            // Load the scene from the ccbi-file, setting this class as
            // the owner will cause lblTestTitle to be set by the CCBReader.
            // lblTestTitle is in the TestHeader.ccbi, which is referenced
            // from each of the test scenes.
            CCNode animationsTest = ccbReader.ReadNodeGraphFromFile("ccb/ccb/TestAnimations.ccbi", this);
            ((AnimationsTestLayer) animationsTest).setAnimationManager(ccbReader.AnimationManager);

            mTestTitleLabelTTF.Text = ("TestAnimations.ccbi");

            CCScene scene = new CCScene();
            scene.AddChild(animationsTest);

            /* Push the new scene with a fancy transition. */
            CCColor3B transitionColor = new CCColor3B();
            transitionColor.R = 0;
            transitionColor.G = 0;
            transitionColor.B = 0;

            CCDirector.SharedDirector.PushScene(new CCTransitionFade(0.5f, scene, transitionColor));
        }

        public void onParticleSystemTestClicked(object pSender, CCControlEvent pCCControlEvent)
        {
            openTest("ccb/ccb/TestParticleSystems.ccbi", "TestParticleSystemsLayer", new Loader<ParticleSystemTestLayer>());
        }

        public void onScrollViewTestClicked(object pSender, CCControlEvent pCCControlEvent)
        {
            openTest("ccb/ccb/TestScrollViews.ccbi", "TestScrollViewsLayer", new Loader<ScrollViewTestLayer>());
        }

        public void onTimelineCallbackSoundClicked(object pSender, CCControlEvent pCCControlEvent)
        {
            openTest("ccb/ccb/TestTimelineCallback.ccbi", "TimelineCallbackTestLayer", new Loader<TimelineCallbackTestLayer>());
        }
    }
}