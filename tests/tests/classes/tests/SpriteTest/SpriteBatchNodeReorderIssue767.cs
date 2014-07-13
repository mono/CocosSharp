using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
    public class SpriteBatchNodeReorderIssue767 : SpriteTestDemo
    {
        CCNode aParent;
        CCSprite l1, l2a, l2b, l3a1, l3a2, l3b1, l3b2;


        #region Properties

        public override string Title
        {
            get { return "SpriteBatchNode: reorder issue #767"; }
        }

        public override string Subtitle
        {
            get { return "Should not crash"; }
        }

        #endregion Properties


        #region Constructors

        public SpriteBatchNodeReorderIssue767()
        {
            CCApplication.SharedApplication.SpriteFrameCache.AddSpriteFrames("animations/ghosts.plist", "animations/ghosts");

            // SpriteBatchNode: 3 levels of children
            aParent = new CCSpriteBatchNode("animations/ghosts");
            AddChild(aParent, 0, (int) kTagSprite.kTagSprite1);

            // parent
            l1 = new CCSprite("father.gif");
            aParent.AddChild(l1, 0, (int) kTagSprite.kTagSprite2);

            // child left
            l2a = new CCSprite("sister1.gif");
            l1.AddChild(l2a, -1, (int) kTags.kTagSpriteLeft);


            // child right
            l2b = new CCSprite("sister2.gif");
            l1.AddChild(l2b, 1, (int) kTags.kTagSpriteRight);

            // child left bottom
            l3a1 = new CCSprite("child1.gif");
            l3a1.Scale = (0.65f);
            l2a.AddChild(l3a1, -1);

            // child left top
            l3a2 = new CCSprite("child1.gif");
            l3a2.Scale = (0.65f);
            l2a.AddChild(l3a2, 1);

            // child right bottom
            l3b1 = new CCSprite("child1.gif");
            l3b1.Scale = (0.65f);
            l2b.AddChild(l3b1, -1);

            // child right top
            l3b2 = new CCSprite("child1.gif");
            l3b2.Scale = (0.65f);
            l2b.AddChild(l3b2, 1);

        }

        #endregion Constructors


        #region Setup content

        public override void OnEnter()
        {
            base.OnEnter(); CCSize windowSize = Scene.VisibleBoundsWorldspace.Size;

            CCSize l1Size = l1.ContentSize;
            CCSize l2aSize = l2a.ContentSize;
            CCSize l2bSize = l2a.ContentSize;

            l1.Position = (new CCPoint(windowSize.Width / 2, windowSize.Height / 2));
            l2a.Position = (new CCPoint(-25 + l1Size.Width / 2, 0 + l1Size.Height / 2));
            l2b.Position = (new CCPoint(+25 + l1Size.Width / 2, 0 + l1Size.Height / 2));
            l3a1.Position = (new CCPoint(0 + l2aSize.Width / 2, -50 + l2aSize.Height / 2));
            l3a2.Position = (new CCPoint(0 + l2aSize.Width / 2, +50 + l2aSize.Height / 2));
            l3b1.Position = (new CCPoint(0 + l2bSize.Width / 2, -50 + l2bSize.Height / 2));
            l3b2.Position = (new CCPoint(0 + l2bSize.Width / 2, +50 + l2bSize.Height / 2));

            Schedule(ReorderSprites, 1);
        }

        #endregion Setup content


        void ReorderSprites(float dt)
        {
            var spritebatch = (CCSpriteBatchNode)GetChildByTag((int)kTagSprite.kTagSprite1);
            var father = (CCSprite)spritebatch.GetChildByTag((int)kTagSprite.kTagSprite2);
            var left = (CCSprite)father.GetChildByTag((int)kTags.kTagSpriteLeft);
            var right = (CCSprite)father.GetChildByTag((int)kTags.kTagSpriteRight);

            int newZLeft = 1;

            if (left.ZOrder == 1)
            {
                newZLeft = -1;
            }

            father.ReorderChild(left, newZLeft);
            father.ReorderChild(right, -newZLeft);
        }
    }
}
