using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
    public class RenderTextureScene : TestScene
    {
        public static int sceneIdx = -1;
        public static int MAX_LAYER = 0;

        static Func<CCLayer>[] rendertextureCreateFunctions =
        {
                () => new RenderTextureSave(),
                () => new RenderTextureDrawNode(),
                () => new RenderTextureDrawNodeVisit(),
                () => new RenderTextureIssue937(),
                () => new RenderTextureZbuffer(),
                () => new RenderTextureTestDepthStencil(),
                () => new RenderTextureCompositeTest(),

        };

        public RenderTextureScene ()
        {
            MAX_LAYER = rendertextureCreateFunctions.Length; 
        }


        public static CCLayer CreateTestCase(int index)
        {
            return rendertextureCreateFunctions[index]();
        }

        protected override void NextTestCase()
        {
            nextTestCase();
        }

        protected override void PreviousTestCase()
        {
            backTestCase();
        }

        protected override void RestTestCase()
        {
            restartTestCase();
        }

        public static CCLayer nextTestCase()
        {
            sceneIdx++;
            sceneIdx = sceneIdx % MAX_LAYER;

            CCLayer pLayer = CreateTestCase(sceneIdx);

            return pLayer;
        }

        public static CCLayer backTestCase()
        {
            sceneIdx--;
            int total = MAX_LAYER;
            if (sceneIdx < 0)
                sceneIdx += total;

            CCLayer pLayer = CreateTestCase(sceneIdx);

            return pLayer;
        }

        public static CCLayer restartTestCase()
        {
            CCLayer pLayer = CreateTestCase(sceneIdx);

            return pLayer;
        }

        public override void runThisTest()
        {
            CCLayer layer = nextTestCase();
            AddChild(layer);

            Director.ReplaceScene(this);
        }
    }
}
