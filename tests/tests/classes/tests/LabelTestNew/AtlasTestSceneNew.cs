using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
    public class AtlasTestScene : TestScene
    {
        static int sceneIdx = -1;
        static readonly int MAX_LAYER = 26;

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
        public static CCLayer createAtlasLayer(int nIndex)
        {
            switch (nIndex)
            {
			case 0: return new LabelAtlasTest();
			case 1: return new LabelAtlasColorTest();
			case 2: return new Atlas3();
			case 3: return new Atlas4();
			case 4: return new Atlas5();
			case 5: return new Atlas6();
			case 6: return new AtlasBitmapColor();
			case 7: return new AtlasFastBitmap();
			case 8: return new BitmapFontMultiLine();
			case 9: return new LabelsEmpty();
			case 10: return new LabelBMFontHD();
			case 11: return new LabelAtlasHD();
			case 12: return new LabelGlyphDesigner();
			case 13: return new LabelTTFTest();
			case 14: return new LabelTTFMultiline();
			case 15: return new LabelTTFChinese();
			case 16: return new LabelBMFontChinese();
			case 17: return new BitmapFontMultiLineAlignment();
			case 18: return new LabelTTFA8Test();
			case 19: return new BMFontOneAtlas();
			case 20: return new BMFontUnicode();
			case 21: return new BMFontInit();
			case 22: return new TTFFontInit();
			case 23: return new Issue1343();
			case 24: return new GitHubIssue5();
			// Not a label test. Should be moved to Atlas test
			case 25: return new Atlas1();

                default:
                    break;
            }

            return null;

        }

        public override void runThisTest()
        {
            CCLayer pLayer = nextAtlasAction();
            AddChild(pLayer);

            Scene.Director.ReplaceScene(this);
        }
    }
}
