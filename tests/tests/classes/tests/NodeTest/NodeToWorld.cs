using CocosSharp;

namespace tests
{
    public class NodeToWorld : TestCocosNodeDemo
    {

		CCMenu menu;
		CCSprite back;


        public NodeToWorld()
        {
            //
            // This code tests that nodeToParent works OK:
            //  - It tests different anchor Points
            //  - It tests different children anchor points

			back = new CCSprite(TestResource.s_back3);
            AddChild(back, -10);
            back.AnchorPoint = (new CCPoint(0, 0));

			var item = new CCMenuItemImage(TestResource.s_PlayNormal, TestResource.s_PlaySelect);
			menu = new CCMenu(item);
            menu.AlignItemsVertically();
            back.AddChild(menu);

			item.RepeatForever(CocosNodeTestStaticLibrary.nodeRotate);

			back.RepeatForever(CocosNodeTestStaticLibrary.nodeMove, CocosNodeTestStaticLibrary.nodeMove.Reverse());
        }


        protected override void AddedToNewScene()
        {
            base.AddedToNewScene();

            CCSize windowSize = Scene.VisibleBoundsWorldspace.Size;

			var backSize = back.ContentSize;
			menu.Position = backSize.Center;
		}

        public override string title()
        {
            return "nodeToParent transform";
        }
    }

	public class NodeToWorld3D : TestCocosNodeDemo
	{

		CCNode parent;
		CCSprite back;
		CCMenu menu;


		public NodeToWorld3D()
		{
			//
			// This code tests that nodeToParent works OK:
			//  - It tests different anchor Points
			//  - It tests different children anchor points

			parent = new CCNode();
			parent.AnchorPoint = new CCPoint(0.5f, 0.5f);
			AddChild(parent);

			back = new CCSprite(TestResource.s_back3);
			parent.AddChild(back, -10);
			back.AnchorPoint = CCPoint.Zero;


			var item = new CCMenuItemImage(TestResource.s_PlayNormal, TestResource.s_PlaySelect);
			menu = new CCMenu(item);
			menu.AlignItemsVertically();

			back.AddChild(menu);

			item.RepeatForever(CocosNodeTestStaticLibrary.nodeRotate);

			back.RepeatForever(CocosNodeTestStaticLibrary.nodeMove, CocosNodeTestStaticLibrary.nodeMove.Reverse());

			parent.RunAction (CocosNodeTestStaticLibrary.nodeOrbit);
		}


        protected override void AddedToNewScene()
        {
            base.AddedToNewScene();

            CCSize windowSize = Scene.VisibleBoundsWorldspace.Size;

			parent.ContentSize = windowSize;
			parent.Position = windowSize.Center;

			var backSize = back.ContentSize;
			menu.Position = backSize.Center;
		}


		public override string title()
		{
			return "nodeToParent transform in 3D";
		}
	}
}