using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cocos2D;

namespace tests
{
    public class MotionStreakTest1 : MotionStreakTest
    {
        string s_streak = "Images/streak";
        string s_pPathB1 = "Images/b1";
        string s_pPathB2 = "Images/b2";
        string s_pPathR1 = "Images/r1";
        string s_pPathR2 = "Images/r2";
        string s_pPathF1 = "Images/f1";
        string s_pPathF2 = "Images/f2";

        protected CCNode m_root;
        protected CCNode m_target;

        public override void OnEnter()
        {
            base.OnEnter();

            CCSize s = CCDirector.SharedDirector.WinSize;

            // the root object just rotates around
            m_root = new CCSprite(s_pPathR1);
            AddChild(m_root, 1);
            m_root.Position = new CCPoint(s.Width / 2, s.Height / 2);

            // the target object is offset from root, and the streak is moved to follow it
            m_target = new CCSprite(s_pPathR1);
            m_root.AddChild(m_target);
            m_target.Position = new CCPoint(s.Width / 4, 0);

            // create the streak object and add it to the scene
            streak = CCMotionStreak.Create(2, 3, 32, CCTypes.CCGreen, s_streak);
            streak.FastMode = true;
            AddChild(streak);
            // schedule an update on each frame so we can syncronize the streak with the target
            Schedule(onUpdate);

            var a1 = new CCRotateBy (2, 360);

            var action1 = new CCRepeatForever (a1);
            var motion = new CCMoveBy (2, new CCPoint(100, 0));
            m_root.RunAction(new CCRepeatForever ((CCActionInterval)new CCSequence(motion, motion.Reverse())));
            m_root.RunAction(action1);

            var colorAction = new CCRepeatForever ((CCActionInterval)
                new CCSequence(
                    new CCTintTo (0.2f, 255, 0, 0),
                    new CCTintTo (0.2f, 0, 255, 0),
                    new CCTintTo (0.2f, 0, 0, 255),
                    new CCTintTo (0.2f, 0, 255, 255),
                    new CCTintTo (0.2f, 255, 255, 0),
                    new CCTintTo (0.2f, 255, 0, 255),
                    new CCTintTo (0.2f, 255, 255, 255)
                    )
                );

            streak.RunAction(colorAction);
        }

        public void onUpdate(float delta)
        {
            streak.Position = m_target.ConvertToWorldSpace(CCPoint.Zero);
        }

        public override string title()
        {
            return "MotionStreak test 1";
        }
    }
}
