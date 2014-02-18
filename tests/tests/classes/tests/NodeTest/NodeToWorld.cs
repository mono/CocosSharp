using CocosSharp;

namespace tests
{
    public class NodeToWorld : TestCocosNodeDemo
    {
        public NodeToWorld()
        {
            //
            // This code tests that nodeToParent works OK:
            //  - It tests different anchor Points
            //  - It tests different children anchor points

			var back = new CCSprite(TestResource.s_back3);
            AddChild(back, -10);
            back.AnchorPoint = (new CCPoint(0, 0));
			var backSize = back.ContentSize;

			var item = new CCMenuItemImage(TestResource.s_PlayNormal, TestResource.s_PlaySelect);
			var menu = new CCMenu(item);
            menu.AlignItemsVertically();
            menu.Position = new CCPoint(backSize.Width / 2, backSize.Height / 2);
            back.AddChild(menu);

			var rot = new CCRotateBy (5, 360);

			item.RepeatForever(rot);

			var move = new CCMoveBy (3, new CCPoint(200, 0));
            var move_back = move.Reverse();

			back.RepeatForever(move, move_back);
        }

        public override string title()
        {
            return "nodeToParent transform";
        }
    }
}