using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests.Extensions
{
    internal class TimelineCallbackTestLayer : BaseLayer
    {
        public CCLabel helloLabel;

        public void onCallback1(CCNode sender)
        {
            // Rotate the label when the button is pressed
            helloLabel.RunAction(new CCRotateBy(1, 360));
            helloLabel.Text = "Callback 1";
        }

        public void onCallback2(CCNode sender)
        {
            // Rotate the label when the button is pressed
            helloLabel.RunAction(new CCRotateBy(1, -360));
            helloLabel.Text = "Callback 2";
        }

    }
}
