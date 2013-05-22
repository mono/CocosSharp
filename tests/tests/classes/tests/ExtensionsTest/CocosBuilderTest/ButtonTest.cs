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
                    mCCControlEventLabel.Label = ("Touch Down.");
                    break;
                case CCControlEvent.TouchDragInside:
                    mCCControlEventLabel.Label = ("Touch Drag Inside.");
                    break;
                case CCControlEvent.TouchDragOutside:
                    mCCControlEventLabel.Label = ("Touch Drag Outside.");
                    break;
                case CCControlEvent.TouchDragEnter:
                    mCCControlEventLabel.Label = ("Touch Drag Enter.");
                    break;
                case CCControlEvent.TouchDragExit:
                    mCCControlEventLabel.Label = ("Touch Drag Exit.");
                    break;
                case CCControlEvent.TouchUpInside:
                    mCCControlEventLabel.Label = ("Touch Up Inside.");
                    break;
                case CCControlEvent.TouchUpOutside:
                    mCCControlEventLabel.Label = ("Touch Up Outside.");
                    break;
                case CCControlEvent.TouchCancel:
                    mCCControlEventLabel.Label = ("Touch Cancel.");
                    break;
                case CCControlEvent.ValueChanged:
                    mCCControlEventLabel.Label = ("Value Changed.");
                    break;
                default:
                    Debug.Assert(false); // OH SHIT!
                    break;
            }
        }
    }
}