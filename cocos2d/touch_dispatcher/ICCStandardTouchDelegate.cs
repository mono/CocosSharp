
using System.Collections.Generic;

namespace cocos2d
{

    public interface ICCStandardTouchDelegate : ICCTouchDelegate
    {
        // optional
        void TouchesBegan(List<CCTouch> pTouches, CCEvent pEvent);
        void TouchesMoved(List<CCTouch> pTouches, CCEvent pEvent);
        void TouchesEnded(List<CCTouch> pTouches, CCEvent pEvent);
        void TouchesCancelled(List<CCTouch> pTouches, CCEvent pEvent);
    }
}