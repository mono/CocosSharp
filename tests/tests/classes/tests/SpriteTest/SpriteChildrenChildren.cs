using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class SpriteChildrenChildren : SpriteTestDemo
    {
        public SpriteChildrenChildren()
        {
            CCSize s = CCDirector.SharedDirector.WinSize;

            CCSpriteFrameCache.SharedSpriteFrameCache.AddSpriteFramesWithFile("animations/ghosts.plist");

            CCNode aParent;
            CCSprite l1, l2a, l2b, l3a1, l3a2, l3b1, l3b2;
            CCActionInterval rot = new CCRotateBy (10, 360);
            CCAction seq = CCRepeatForever.Create(rot);

            CCActionInterval rot_back = (CCActionInterval)rot.Reverse();
            CCAction rot_back_fe = CCRepeatForever.Create(rot_back);

            //
            // SpriteBatchNode: 3 levels of children
            //

            aParent = CCNode.Create();
            AddChild(aParent);

            // parent
            l1 = CCSprite.Create("father.gif");
            l1.Position = (new CCPoint(s.Width / 2, s.Height / 2));
            l1.RunAction((CCAction)(seq.Copy()));
            aParent.AddChild(l1);
            CCSize l1Size = l1.ContentSize;

            // child left
            l2a = CCSprite.Create("sister1.gif");
            l2a.Position = (new CCPoint(-50 + l1Size.Width / 2, 0 + l1Size.Height / 2));
            l2a.RunAction((CCAction)(rot_back_fe.Copy()));
            l1.AddChild(l2a);
            CCSize l2aSize = l2a.ContentSize;

            // child right
            l2b = CCSprite.Create("sister2.gif");
            l2b.Position = (new CCPoint(+50 + l1Size.Width / 2, 0 + l1Size.Height / 2));
            l2b.RunAction((CCAction)(rot_back_fe.Copy()));
            l1.AddChild(l2b);
            CCSize l2bSize = l2a.ContentSize;

            // child left bottom
            l3a1 = CCSprite.Create("child1.gif");
            l3a1.Scale = 0.45f;
            l3a1.Position = new CCPoint(0 + l2aSize.Width / 2, -100 + l2aSize.Height / 2);
            l2a.AddChild(l3a1);

            // child left top
            l3a2 = CCSprite.Create("child1.gif");
            l3a2.Scale = 0.45f;
            l3a1.Position = (new CCPoint(0 + l2aSize.Width / 2, +100 + l2aSize.Height / 2));
            l2a.AddChild(l3a2);

            // child right bottom
            l3b1 = CCSprite.Create("child1.gif");
            l3b1.Scale = 0.45f;
            l3b1.FlipY = true;
            l3b1.Position = (new CCPoint(0 + l2bSize.Width / 2, -100 + l2bSize.Height / 2));
            l2b.AddChild(l3b1);

            // child right top
            l3b2 = CCSprite.Create("child1.gif");
            l3b2.Scale = 0.45f;
            l3b2.FlipY = true;
            l3b1.Position = new CCPoint(0 + l2bSize.Width / 2, +100 + l2bSize.Height / 2);
            l2b.AddChild(l3b2);
        }

        public override string title()
        {
            return "Sprite multiple levels of children";
        }
    }
}
