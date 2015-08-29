using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
    public class GitHubIssue5 : AtlasDemo
    {
        private CCLabel _TestLabel;
        private int _Index = 0;

        public GitHubIssue5()
        {
            _TestLabel = new CCLabel("", "Arial", 10, CCLabelFormat.SpriteFont);
            AddChild(_TestLabel);
        }

        protected override void AddedToScene()
        {
            base.AddedToScene();

            CCSize s = Layer.VisibleBoundsWorldspace.Size;
			_TestLabel.Position = new CCPoint(s.Width / 2, s.Height / 4 * 2);

		}

        public override void OnEnter()
        {
            base.OnEnter();
            _Index++;
            // Start the ticker
            RunAction(new CCRepeatForever (
                new CCSequence(
                new CCDelayTime (1f),
                new CCCallFunc(UpdateLabel))
                ));
        }

        private void UpdateLabel()
        {
            switch (_Index % 3)
            {
                case 0:
                    _TestLabel.Text = ("******* DEBUG MODE - please uncomment #define Debug *******");
                    break;
                case 1:
                      _TestLabel.Text = ("******* [" + String.Format("{0:0.##}", 50) + "] p " + _Index + " z X *******");
                    break;
                case 2:
                    _TestLabel.Text = ("");
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
