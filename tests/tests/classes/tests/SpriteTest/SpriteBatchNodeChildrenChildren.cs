using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
    public class SpriteBatchNodeChildrenChildren : SpriteTestDemo
    {
        CCSpriteBatchNode aParent;
        CCSprite l1, l2a, l2b, l3a1, l3a2, l3b1, l3b2;

        CCRepeatForever seq;
        CCRepeatForever rot_back_fe;


        #region Properties

        public override string Title
        {
            get { return "SpriteBatchNode multiple levels of children"; }
        }

        #endregion Properties


        #region Constructors

        public SpriteBatchNodeChildrenChildren()
        {
            CCApplication.SharedApplication.SpriteFrameCache.AddSpriteFrames("animations/ghosts.plist");

            var rot = new CCRotateBy (10, 360);
            seq = new CCRepeatForever (rot);

            var rot_back = rot.Reverse();
            rot_back_fe = new CCRepeatForever (rot_back);

            // SpriteBatchNode: 3 levels of children

            aParent = new CCSpriteBatchNode("animations/ghosts");
            AddChild(aParent);

            // parent
            l1 = new CCSprite("father.gif");
            aParent.AddChild(l1);


            // child left
            l2a = new CCSprite("sister1.gif");

            l1.AddChild(l2a);

            // child right
            l2b = new CCSprite("sister2.gif");

            l1.AddChild(l2b);


            // child left bottom
            l3a1 = new CCSprite("child1.gif");
            l3a1.Scale = 0.45f;
            l2a.AddChild(l3a1);

            // child left top
            l3a2 = new CCSprite("child1.gif");
            l3a2.Scale = 0.45f;
            l2a.AddChild(l3a2);

            // child right bottom
            l3b1 = new CCSprite("child1.gif");
            l3b1.Scale = 0.45f;
            l3b1.FlipY = true;
            l2b.AddChild(l3b1);

            // child right top
            l3b2 = new CCSprite("child1.gif");
            l3b2.Scale = 0.45f;
            l3b2.FlipY = true;
            l2b.AddChild(l3b2);
        }

        #endregion Constructors


        #region Setup content

        public void OnEnter()
        {
base.OnEnter(); CCSize windowSize = Scene.VisibleBoundsWorldspace.Size;

            var l1Size = l1.ContentSize;
            var l2aSize = l2a.ContentSize;
            var l2bSize = l2a.ContentSize;

            l1.Position = new CCPoint(windowSize.Width / 2, windowSize.Height / 2);
            l1.RunAction(seq);

            l2a.Position = (new CCPoint(-50 + l1Size.Width / 2, 0 + l1Size.Height / 2));
            l2a.RunAction(rot_back_fe);

            l2b.Position = (new CCPoint(+50 + l1Size.Width / 2, 0 + l1Size.Height / 2));
            l2b.RunAction(rot_back_fe);

            l3a1.Position = (new CCPoint(0 + l2aSize.Width / 2, -100 + l2aSize.Height / 2));
            l3b1.Position = new CCPoint(0 + l2bSize.Width / 2, -100 + l2bSize.Height / 2);
            l3b1.Position = new CCPoint(0 + l2bSize.Width / 2, +100 + l2bSize.Height / 2);
        }

        #endregion Setup content
    }
}
