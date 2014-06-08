using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
    public class SpriteBatchNodeChildren : SpriteTestDemo
    {
        public SpriteBatchNodeChildren()
        {
            CCSize s = CCApplication.SharedApplication.MainWindowDirector.WindowSizeInPoints;

            // parents
            CCSpriteBatchNode batch = new CCSpriteBatchNode("animations/grossini", 50);

            AddChild(batch, 0, (int)kTags.kTagSpriteBatchNode);

            CCApplication.SharedApplication.SpriteFrameCache.AddSpriteFrames("animations/grossini.plist");

            CCSprite sprite1 = new CCSprite("grossini_dance_01.png");
            sprite1.Position = (new CCPoint(s.Width / 3, s.Height / 2));

            CCSprite sprite2 = new CCSprite("grossini_dance_02.png");
            sprite2.Position = (new CCPoint(50, 50));

            CCSprite sprite3 = new CCSprite("grossini_dance_03.png");
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
                CCSpriteFrame frame = CCApplication.SharedApplication.SpriteFrameCache[str];
                animFrames.Add(frame);
            }

            CCAnimation animation = new CCAnimation(animFrames, 0.2f);
            sprite1.RunAction(new CCRepeatForever (new CCAnimate (animation)));
            // END NEW CODE

            CCActionInterval action = new CCMoveBy (2, new CCPoint(200, 0));
            CCActionInterval action_back = (CCActionInterval)action.Reverse();
            CCActionInterval action_rot = new CCRotateBy (2, 360);
            CCActionInterval action_s = new CCScaleBy(2, 2);
            CCActionInterval action_s_back = (CCActionInterval)action_s.Reverse();

            CCActionInterval seq2 = (CCActionInterval)action_rot.Reverse();
            sprite2.RunAction(new CCRepeatForever (seq2));

            sprite1.RunAction((CCAction)(new CCRepeatForever (action_rot)));
            sprite1.RunAction((CCAction)(new CCRepeatForever ((CCActionInterval)(new CCSequence(action, action_back)))));
            sprite1.RunAction((CCAction)(new CCRepeatForever ((CCActionInterval)(new CCSequence(action_s, action_s_back)))));

        }

        public override void OnExit()
        {
            base.OnExit();
            CCApplication.SharedApplication.SpriteFrameCache.RemoveUnusedSpriteFrames();
        }

        public override string title()
        {
            return "SpriteBatchNode Grand Children";
        }
    }
}
