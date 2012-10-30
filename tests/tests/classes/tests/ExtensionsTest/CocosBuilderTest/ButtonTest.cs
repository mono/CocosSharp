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
                    mCCControlEventLabel.SetString("Touch Down.");
                    break;
                case CCControlEvent.TouchDragInside:
                    mCCControlEventLabel.SetString("Touch Drag Inside.");
                    break;
                case CCControlEvent.TouchDragOutside:
                    mCCControlEventLabel.SetString("Touch Drag Outside.");
                    break;
                case CCControlEvent.TouchDragEnter:
                    mCCControlEventLabel.SetString("Touch Drag Enter.");
                    break;
                case CCControlEvent.TouchDragExit:
                    mCCControlEventLabel.SetString("Touch Drag Exit.");
                    break;
                case CCControlEvent.TouchUpInside:
                    mCCControlEventLabel.SetString("Touch Up Inside.");
                    break;
                case CCControlEvent.TouchUpOutside:
                    mCCControlEventLabel.SetString("Touch Up Outside.");
                    break;
                case CCControlEvent.TouchCancel:
                    mCCControlEventLabel.SetString("Touch Cancel.");
                    break;
                case CCControlEvent.ValueChanged:
                    mCCControlEventLabel.SetString("Value Changed.");
                    break;
                default:
                    Debug.Assert(false); // OH SHIT!
                    break;
            }
        }
    }
}