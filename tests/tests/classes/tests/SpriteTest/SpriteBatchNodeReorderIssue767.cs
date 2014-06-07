using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
    public class SpriteBatchNodeReorderIssue767 : SpriteTestDemo
    {
        public SpriteBatchNodeReorderIssue767()
        {
            CCSize s = CCApplication.SharedApplication.MainWindowDirector.WindowSizeInPoints;

            CCSpriteFrameCache.Instance.AddSpriteFrames("animations/ghosts.plist", "animations/ghosts");
            CCNode aParent;
            CCSprite l1, l2a, l2b, l3a1, l3a2, l3b1, l3b2;

            //
            // SpriteBatchNode: 3 levels of children
            //
            aParent = new CCSpriteBatchNode("animations/ghosts");
            AddChild(aParent, 0, (int) kTagSprite.kTagSprite1);

            // parent
            l1 = new CCSprite("father.gif");
            l1.Position = (new CCPoint(s.Width / 2, s.Height / 2));
            aParent.AddChild(l1, 0, (int) kTagSprite.kTagSprite2);
            CCSize l1Size = l1.ContentSize;

            // child left
            l2a = new CCSprite("sister1.gif");
            l2a.Position = (new CCPoint(-25 + l1Size.Width / 2, 0 + l1Size.Height / 2));
            l1.AddChild(l2a, -1, (int) kTags.kTagSpriteLeft);
            CCSize l2aSize = l2a.ContentSize;


            // child right
            l2b = new CCSprite("sister2.gif");
            l2b.Position = (new CCPoint(+25 + l1Size.Width / 2, 0 + l1Size.Height / 2));
            l1.AddChild(l2b, 1, (int) kTags.kTagSpriteRight);
            CCSize l2bSize = l2a.ContentSize;


            // child left bottom
            l3a1 = new CCSprite("child1.gif");
            l3a1.Scale = (0.65f);
            l3a1.Position = (new CCPoint(0 + l2aSize.Width / 2, -50 + l2aSize.Height / 2));
            l2a.AddChild(l3a1, -1);

            // child left top
            l3a2 = new CCSprite("child1.gif");
            l3a2.Scale = (0.65f);
            l3a2.Position = (new CCPoint(0 + l2aSize.Width / 2, +50 + l2aSize.Height / 2));
            l2a.AddChild(l3a2, 1);

            // child right bottom
            l3b1 = new CCSprite("child1.gif");
            l3b1.Scale = (0.65f);
            l3b1.Position = (new CCPoint(0 + l2bSize.Width / 2, -50 + l2bSize.Height / 2));
            l2b.AddChild(l3b1, -1);

            // child right top
            l3b2 = new CCSprite("child1.gif");
            l3b2.Scale = (0.65f);
            l3b2.Position = (new CCPoint(0 + l2bSize.Width / 2, +50 + l2bSize.Height / 2));
            l2b.AddChild(l3b2, 1);

            Schedule(reorderSprites, 1);
        }

        public override string title()
        {
            return "SpriteBatchNode: reorder issue #767";
        }

        public override string subtitle()
        {
            return "Should not crash";
        }

        public void reorderSprites(float dt)
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
