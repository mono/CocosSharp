using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
    public class SpriteBatchNodeChildren : SpriteTestDemo
    {
        CCSprite sprite1;
        CCSprite sprite2;
        CCSprite sprite3;

        CCAnimation animation;
        CCFiniteTimeAction seq2;
        CCFiniteTimeAction action_rot;
        CCFiniteTimeAction action_back;
        CCFiniteTimeAction action_s;
        CCFiniteTimeAction action;
        CCFiniteTimeAction action_s_back;

        #region Properties

        public override string Title
        {
            get { return "SpriteBatchNode Grand Children"; }
        }

        #endregion Properties


        #region Constructors

        public SpriteBatchNodeChildren()
        {
            CCSpriteBatchNode batch = new CCSpriteBatchNode("animations/grossini", 50);
            AddChild(batch, 0, (int)kTags.kTagSpriteBatchNode);

            CCSpriteFrameCache.SharedSpriteFrameCache.AddSpriteFrames("animations/grossini.plist");

            sprite1 = new CCSprite("grossini_dance_01.png");
            sprite2 = new CCSprite("grossini_dance_02.png");
            sprite3 = new CCSprite("grossini_dance_03.png");

            batch.AddChild(sprite1);
            sprite1.AddChild(sprite2);
            sprite1.AddChild(sprite3);

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
                CCSpriteFrame frame = CCSpriteFrameCache.SharedSpriteFrameCache[str];
                animFrames.Add(frame);
            }

            animation = new CCAnimation(animFrames, 0.2f);

            action = new CCMoveBy (2, new CCPoint(200, 0));
            action_back = (CCFiniteTimeAction)action.Reverse();
            action_rot = new CCRotateBy (2, 360);
            action_s = new CCScaleBy(2, 2);
            action_s_back = (CCFiniteTimeAction)action_s.Reverse();

            seq2 = (CCFiniteTimeAction)action_rot.Reverse();
        }

        #endregion Constructors


        #region Setup content

        public override void OnEnter()
        {
            base.OnEnter(); CCSize windowSize = Layer.VisibleBoundsWorldspace.Size;

            sprite1.Position = (new CCPoint(windowSize.Width / 3, windowSize.Height / 2));
            sprite2.Position = (new CCPoint(50, 50));
            sprite3.Position = (new CCPoint(-50, -50));

            sprite1.RunAction(new CCRepeatForever (new CCAnimate(animation)));
            sprite2.RunAction(new CCRepeatForever (seq2));

            sprite1.RunAction((CCAction)(new CCRepeatForever (action_rot)));
            sprite1.RunAction((CCAction)(new CCRepeatForever ((CCFiniteTimeAction)(new CCSequence(action, action_back)))));
            sprite1.RunAction((CCAction)(new CCRepeatForever ((CCFiniteTimeAction)(new CCSequence(action_s, action_s_back)))));
        }

        #endregion Setup content


        public override void OnExit()
        {
            base.OnExit();
            CCSpriteFrameCache.SharedSpriteFrameCache.RemoveUnusedSpriteFrames();
        }

    }
}
