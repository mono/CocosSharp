using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
    public class AtlasTestSceneNew : TestScene
    {
        static int sceneIdx = -1;
        static int MAX_LAYER = 0;

        public AtlasTestSceneNew () : base()
        {
            MAX_LAYER = labelCreateFunctions.Length;
        }

        protected override void NextTestCase()
        {
            nextAtlasAction();
        }
        protected override void PreviousTestCase()
        {
            backAtlasAction();
        }
        protected override void RestTestCase()
        {
            restartAtlasAction();
        }
        public static CCLayer nextAtlasAction()
        {
            sceneIdx++;
            sceneIdx = sceneIdx % MAX_LAYER;

            CCLayer pLayer = createAtlasLayer(sceneIdx);
            return pLayer;

        }

        public static CCLayer backAtlasAction()
        {

            sceneIdx--;
            int total = MAX_LAYER;
            if (sceneIdx < 0)
                sceneIdx += total;

            CCLayer pLayer = createAtlasLayer(sceneIdx);
            return pLayer;
        }

        public static CCLayer restartAtlasAction()
        {
            CCLayer pLayer = createAtlasLayer(sceneIdx);

            return pLayer;
        }

        static Func<CCLayer>[] labelCreateFunctions =
        {
                () => new LabelFNTColorAndOpacity(),
                () => new LabelSystemFontColorAndOpacity(),
                () => new LabelFNTSpriteActions(),
                () => new LabelFNTPadding(),
                () => new LabelFNTOffset(),
                () => new LabelFNTColor(),
                () => new LabelFNTHundredLabels(),
                () => new LabelFNTMultiLine(),
                () => new LabelFNTandSFEmpty (),
                () => new LabelFNTRetina(),
                () => new LabelFNTGlyphDesigner(),
                () => new LabelTTFUnicodeChinese (),
                () => new LabelSystemFontUnicodeJapanese (),
                () => new LabelFNTMultiLineAlignment(),
                () => new LabelFNTUNICODELanguages(),
                () => new LabelTTFAlignmentNew(),
                () => new LabelFNTBounds(),
                () => new LabelSFLongLineWrapping(),
                () => new LabelSFColor(),
                () => new LabelSystemFontColor(),
                () => new LabelTTFFontsTestNew(),
                () => new LabelSFAlignmentTest(),
                () => new LabelSystemFontAlignmentTest(),
                () => new LabelSFOldNew(),
                () => new LabelFNTOldNew(),
                () => new LabelFNTFlippedY(),
                () => new LabelFNTLineHeightTest(),
                () => new LabelFNTAdditionalKerningTest(),
                () => new LabelFNTContentSizeRatios(),
                () => new LabelSFContentSizeRatios(),
                () => new LabelSystemFont168(),
                () => new LabelSystemFontRenderTexture(),

#if !WINDOWS_PHONE8
                () => new LabelFNTFromHTTP(),
#endif
                () => new LabelSFTextureView(),
        };

        public static CCLayer createAtlasLayer(int index)
        {
            return labelCreateFunctions[index]();
        }

        public override void runThisTest()
        {
            CCLayer pLayer = nextAtlasAction();
            AddChild(pLayer);

            Scene.Director.ReplaceScene(this);
        }
    }
}
