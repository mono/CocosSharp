using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;
using tests;
using System.Diagnostics;

namespace CocosSharp
{

    public class AtlasDemoNew : TestNavigationLayer
    {
        //protected:

        public AtlasDemoNew()
        {

        }

        public enum LabelTestConstant
        {
            IDC_NEXT = 100,
            IDC_BACK,
            IDC_RESTART
        }


		public override string Title
		{
			get
			{
				return title();
			}
		}

        public virtual string title()
        {
            return "No title";
        }

		public override string Subtitle
		{
			get
			{
				return subtitle();
			}
		}

        public virtual string subtitle()
        {
            return "";
        }

		public override void RestartCallback(object sender)
		{
			base.RestartCallback(sender);
            CCScene s = new AtlasTestSceneNew();
            s.AddChild(AtlasTestSceneNew.restartAtlasAction());

            Director.ReplaceScene(s);
        }

		public override void NextCallback(object sender)
		{
			base.NextCallback(sender);

            CCScene s = new AtlasTestSceneNew();

            s.AddChild(AtlasTestSceneNew.nextAtlasAction());

            Director.ReplaceScene(s);
        }

		public override void BackCallback(object sender)
		{
			base.BackCallback(sender);

            CCScene s = new AtlasTestSceneNew();

            s.AddChild(AtlasTestSceneNew.backAtlasAction());

            Director.ReplaceScene(s);
        }

    }

}
