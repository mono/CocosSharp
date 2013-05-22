
using System.Collections.Generic;

namespace Cocos2D
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