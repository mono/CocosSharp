
namespace Cocos2D
{
    public interface ICCTargetedTouchDelegate : ICCTouchDelegate
    {
        bool TouchBegan(CCTouch pTouch, CCEvent pEvent);

        // optional
        void TouchMoved(CCTouch pTouch, CCEvent pEvent);
        void TouchEnded(CCTouch pTouch, CCEvent pEvent);
        void TouchCancelled(CCTouch pTouch, CCEvent pEvent);
    }
}