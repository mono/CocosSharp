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

            CCSprite back = new CCSprite(TestResource.s_back3);
            AddChild(back, -10);
            back.AnchorPoint = (new CCPoint(0, 0));
            CCSize backSize = back.ContentSize;

            CCMenuItem item = new CCMenuItemImage(TestResource.s_PlayNormal, TestResource.s_PlaySelect);
            CCMenu menu = new CCMenu(item);
            menu.AlignItemsVertically();
            menu.Position = (new CCPoint(backSize.Width / 2, backSize.Height / 2));
            back.AddChild(menu);

            CCActionInterval rot = new CCRotateBy (5, 360);
            CCAction fe = new CCRepeatForever (rot);
            item.RunAction(fe);

            CCActionInterval move = new CCMoveBy (3, new CCPoint(200, 0));
            var move_back = (CCActionInterval) move.Reverse();
            CCFiniteTimeAction seq = new CCSequence(move, move_back);
            CCAction fe2 = new CCRepeatForever ((CCActionInterval) seq);
            back.RunAction(fe2);
        }

        public override string title()
        {
            return "nodeToParent transform";
        }
    }
}