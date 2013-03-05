using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class GitHubIssue5 : AtlasDemo
    {
        private CCLabelTTF _TestLabel;
        private int _Index = 0;

        public GitHubIssue5()
        {
            CCSize s = CCDirector.SharedDirector.WinSize;
            _TestLabel = CCLabelTTF.Create("", "Arial", 10);
            AddChild(_TestLabel);
            _TestLabel.Position = new CCPoint(s.Width / 2, s.Height / 4 * 2);
        }

        public override void OnEnter()
        {
            base.OnEnter();
            _Index++;
            // Start the ticker
            RunAction(CCRepeatForever.Create(
                CCSequence.Create(
                CCDelayTime.Create(1f),
                CCCallFunc.Create(new SEL_CallFunc(UpdateLabel)))
                ));
        }

        private void UpdateLabel()
        {
            switch (_Index % 3)
            {
                case 0:
                    _TestLabel.SetString("******* DEBUG MODE - please uncomment #define Debug *******");
                    break;
                case 1:
                      _TestLabel.SetString("******* [" + String.Format("{0:0.##}", 50) + "] p " + _Index + " z X *******");
                    break;
                case 2:
                    _TestLabel.SetString("");
                    break;
            }
            _Index++;
        }

        public override string title()
        {
            return "Testing CCLabelTTF";
        }

        public override string subtitle()
        {
            return "Github Issue 5 - Set label to empty string";
        }
    }
}
