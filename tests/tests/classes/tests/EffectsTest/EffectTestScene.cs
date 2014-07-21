using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
    public class EffectTestScene : TestScene
    {
        public override void runThisTest()
        {
            CCLayer layer = new TextLayer ();
            layer.Camera = AppDelegate.SharedCamera;

            AddChild(layer);
            Director.ReplaceScene(this);
        }

        protected override void NextTestCase()
        {
        }
        protected override void PreviousTestCase()
        {
        }
        protected override void RestTestCase()
        {
        }

        public static int kTagTextLayer = 1;
        public static int kTagBackground = 1;
		public static int kTagKathia = 3;
		public static int kTagTamara = 4;
        public static int kTagLabel = 2;
        public static int actionIdx = 0;

        public static string[] effectsList = new string[] { 
            "Shaky3D",
            "Waves3D",
            "FlipX3D",
            "FlipY3D",
            "Lens3D",
            "Ripple3D",
            "Liquid",
            "Waves",
            "Twirl",
            "ShakyTiles3D",
            "ShatteredTiles3D",
            "ShuffleTiles",
            "FadeOutTRTiles",
            "FadeOutBLTiles",
            "FadeOutUpTiles",
            "FadeOutDownTiles",
            "TurnOffTiles",
            "WavesTiles3D",
            "JumpTiles3D",
            "SplitRows",
            "SplitCols",
            "PageTurn3D",
        };
    }
}
