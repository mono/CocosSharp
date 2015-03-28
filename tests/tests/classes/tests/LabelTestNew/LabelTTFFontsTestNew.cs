using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
    public class LabelTTFFontsTestNew : AtlasDemoNew
    {

        private static readonly string[] fontList =
            {
                "fonts/A Damn Mess.ttf",
                "fonts/Abberancy.ttf",
                "fonts/Abduction.ttf",
                "fonts/American Typewriter.ttf",
                "fonts/Paint Boy.ttf",
                "fonts/Schwarzwald.ttf",
                "fonts/Scissor Cuts.ttf",

            };


        public LabelTTFFontsTestNew()
        {
        }

        public override void OnEnter()
        {
            base.OnEnter();

            var size = Layer.VisibleBoundsWorldspace.Size;

            var labelFormatCenter = CCLabelFormat.SystemFont;
            labelFormatCenter.Alignment = CCTextAlignment.Center;

            for (var i=0;i < fontList.Length; ++i) 
            {
                var label = new CCLabel(fontList[i], fontList[i], 20, labelFormatCenter);
                if( label != null) 
                {            
                    label.Position = size.Center;
                    label.PositionY = ((size.Height * 0.6f) / fontList.Length * i) + (size.Height/5f);
                    AddChild(label);
                } 
                else 
                {
                    CCLog.Log ("ERROR: Cannot load: %s", fontList[i]);
                }
            }


        }

        public override string Title
		{
            get {
                return "New Label + TTF";
            }
        }

        public override string Subtitle
        {
            get {
                return string.Empty;
            }
        }
    }
}
