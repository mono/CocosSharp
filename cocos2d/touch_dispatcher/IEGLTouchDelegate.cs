
using System.Collections.Generic;

namespace Cocos2D
{
    public interface IEGLTouchDelegate
    {
        void TouchesBegan(List<CCTouch> touches, CCEvent pEvent);
        void TouchesMoved(List<CCTouch> touches, CCEvent pEvent);
        void TouchesEnded(List<CCTouch> touches, CCEvent pEvent);
        void TouchesCancelled(List<CCTouch> touches, CCEvent pEvent);
    }
}