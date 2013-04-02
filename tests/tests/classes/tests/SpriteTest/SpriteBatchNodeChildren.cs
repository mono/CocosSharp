using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class SpriteBatchNodeChildren : SpriteTestDemo
    {
        public SpriteBatchNodeChildren()
        {
            CCSize s = CCDirector.SharedDirector.WinSize;

            // parents
            CCSpriteBatchNode batch = CCSpriteBatchNode.Create("animations/grossini", 50);

            AddChild(batch, 0, (int)kTags.kTagSpriteBatchNode);

            CCSpriteFrameCache.SharedSpriteFrameCache.AddSpriteFramesWithFile("animations/grossini.plist");

            CCSprite sprite1 = CCSprite.Create("grossini_dance_01.png");
            sprite1.Position = (new CCPoint(s.Width / 3, s.Height / 2));

            CCSprite sprite2 = CCSprite.Create("grossini_dance_02.png");
            sprite2.Position = (new CCPoint(50, 50));

            CCSprite sprite3 = CCSprite.Create("grossini_dance_03.png");
            sprite3.Position = (new CCPoint(-50, -50));

            batch.AddChild(sprite1);
            sprite1.AddChild(sprite2);
            sprite1.AddChild(sprite3);

            // BEGIN NEW CODE
            var animFrames = new List<CCSpriteFrame>();
            string str = "";
            for (int i = 1; i < 15; i++)
            {
                string temp = "";
                if (i<10)
                {
                    temp = "0" + i;
                }
                else
                {
                    temp = i.ToString();
                }
                str = string.Format("grossini_dance_{0}.png", temp);
                CCSpriteFrame frame = CCSpriteFrameCache.SharedSpriteFrameCache.SpriteFrameByName(str);
                animFrames.Add(frame);
            }

            CCAnimation animation = CCAnimation.Create(animFrames, 0.2f);
            sprite1.RunAction(CCRepeatForever.Create(new CCAnimate (animation)));
            // END NEW CODE

            CCActionInterval action = new CCMoveBy (2, new CCPoint(200, 0));
            CCActionInterval action_back = (CCActionInterval)action.Reverse();
            CCActionInterval action_rot = new CCRotateBy (2, 360);
            CCActionInterval action_s = CCScaleBy.Create(2, 2);
            CCActionInterval action_s_back = (CCActionInterval)action_s.Reverse();

            CCActionInterval seq2 = (CCActionInterval)action_rot.Reverse();
            sprite2.RunAction(CCRepeatForever.Create(seq2));

            sprite1.RunAction((CCAction)(CCRepeatForever.Create(action_rot)));
            sprite1.RunAction((CCAction)(CCRepeatForever.Create((CCActionInterval)(CCSequence.Create(action, action_back)))));
            sprite1.RunAction((CCAction)(CCRepeatForever.Create((CCActionInterval)(CCSequence.Create(action_s, action_s_back)))));

        }

        public override void OnExit()
        {
            base.OnExit();
            CCSpriteFrameCache.SharedSpriteFrameCache.RemoveUnusedSpriteFrames();
        }

        public override string title()
        {
            return "SpriteBatchNode Grand Children";
        }
    }
}
