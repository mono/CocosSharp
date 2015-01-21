using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
    public class PauseTest : ActionManagerTest
    {
        string s_pPathGrossini = "Images/grossini";
        int kTagGrossini = 1;

        public override string Title
        {
            get
            {
                return "Pause Test";
            }
        }

        public override string Subtitle
        {
            get
            {
                return "After 5 seconds grossini should move";
            }
        }
        public override void OnEnter()
        {
            //
            // This test MUST be done in 'onEnter' and not on 'init'
            // otherwise the paused action will be resumed at 'onEnter' time
            //
            base.OnEnter();

            CCSize s = Layer.VisibleBoundsWorldspace.Size;

            //
            // Also, this test MUST be done, after [super onEnter]
            //
            CCSprite grossini = new CCSprite(s_pPathGrossini);
            AddChild(grossini, 0, kTagGrossini);
            grossini.Position = (new CCPoint(200, 200));

            CCAction action = new CCMoveBy (1, new CCPoint(150, 0));

            grossini.AddAction(action, true);

            Schedule(unpause, 3);
        }

        public void unpause(float dt)
        {
            Unschedule(unpause);
            CCNode node = GetChildByTag(kTagGrossini);
            Application.ActionManager.ResumeTarget(node);
        }
    }
}
