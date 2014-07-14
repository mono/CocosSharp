using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
    public class ResumeTest : ActionManagerTest
    {
        string s_pPathGrossini = "Images/grossini";
        
        public override string title() 
        {
            return "Resume Test";    
        }

        public override void OnEnter()
        {
            base.OnEnter();

            CCSize s = Scene.VisibleBoundsWorldspace.Size;

            CCLabelTtf l = new CCLabelTtf("Grossini only rotate/scale in 3 seconds", "arial", 16);
            AddChild(l);
            l.Position = (new CCPoint(s.Width / 2, 245));

            CCSprite pGrossini = new CCSprite(s_pPathGrossini);
            AddChild(pGrossini, 0, (int)KTag.kTagGrossini);
            pGrossini.Position = new CCPoint(s.Width / 2, s.Height / 2);

            pGrossini.RunAction(new CCScaleBy(2, 2));

            Application.ActionManager.PauseTarget(pGrossini);
            pGrossini.RunAction(new CCRotateBy (2, 360));

            this.Schedule(resumeGrossini, 3.0f);
        }

        public void resumeGrossini(float time)
        {
            this.Unschedule(resumeGrossini);

            CCNode pGrossini = GetChildByTag((int)KTag.kTagGrossini);
            Application.ActionManager.ResumeTarget(pGrossini);
        }
    }
}
