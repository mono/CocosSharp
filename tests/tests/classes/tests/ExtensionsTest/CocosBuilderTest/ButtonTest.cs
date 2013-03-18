using System.Diagnostics;
using cocos2d;

namespace tests.Extensions
{
    public class ButtonTestLayer : BaseLayer
    {
        public CCLabelBMFont mCCControlEventLabel;

        public void onCCControlButtonClicked(CCObject obj, CCControlEvent pCCControlEvent)
        {
            switch (pCCControlEvent)
            {
                case CCControlEvent.TouchDown:
                    mCCControlEventLabel.String = ("Touch Down.");
                    break;
                case CCControlEvent.TouchDragInside:
                    mCCControlEventLabel.String = ("Touch Drag Inside.");
                    break;
                case CCControlEvent.TouchDragOutside:
                    mCCControlEventLabel.String = ("Touch Drag Outside.");
                    break;
                case CCControlEvent.TouchDragEnter:
                    mCCControlEventLabel.String = ("Touch Drag Enter.");
                    break;
                case CCControlEvent.TouchDragExit:
                    mCCControlEventLabel.String = ("Touch Drag Exit.");
                    break;
                case CCControlEvent.TouchUpInside:
                    mCCControlEventLabel.String = ("Touch Up Inside.");
                    break;
                case CCControlEvent.TouchUpOutside:
                    mCCControlEventLabel.String = ("Touch Up Outside.");
                    break;
                case CCControlEvent.TouchCancel:
                    mCCControlEventLabel.String = ("Touch Cancel.");
                    break;
                case CCControlEvent.ValueChanged:
                    mCCControlEventLabel.String = ("Value Changed.");
                    break;
                default:
                    Debug.Assert(false); // OH SHIT!
                    break;
            }
        }
    }
}