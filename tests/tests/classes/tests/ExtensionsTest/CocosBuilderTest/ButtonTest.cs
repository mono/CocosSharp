using System.Diagnostics;
using Cocos2D;

namespace tests.Extensions
{
    public class ButtonTestLayer : BaseLayer
    {
        public CCLabelBMFont mCCControlEventLabel;

        public void onCCControlButtonClicked(object obj, CCControlEvent pCCControlEvent)
        {
            switch (pCCControlEvent)
            {
                case CCControlEvent.TouchDown:
                    mCCControlEventLabel.Text = ("Touch Down.");
                    break;
                case CCControlEvent.TouchDragInside:
                    mCCControlEventLabel.Text = ("Touch Drag Inside.");
                    break;
                case CCControlEvent.TouchDragOutside:
                    mCCControlEventLabel.Text = ("Touch Drag Outside.");
                    break;
                case CCControlEvent.TouchDragEnter:
                    mCCControlEventLabel.Text = ("Touch Drag Enter.");
                    break;
                case CCControlEvent.TouchDragExit:
                    mCCControlEventLabel.Text = ("Touch Drag Exit.");
                    break;
                case CCControlEvent.TouchUpInside:
                    mCCControlEventLabel.Text = ("Touch Up Inside.");
                    break;
                case CCControlEvent.TouchUpOutside:
                    mCCControlEventLabel.Text = ("Touch Up Outside.");
                    break;
                case CCControlEvent.TouchCancel:
                    mCCControlEventLabel.Text = ("Touch Cancel.");
                    break;
                case CCControlEvent.ValueChanged:
                    mCCControlEventLabel.Text = ("Value Changed.");
                    break;
                default:
                    Debug.Assert(false); // OH SHIT!
                    break;
            }
        }
    }
}