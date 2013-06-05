
namespace Cocos2D
{
    public interface ICCTargetedTouchDelegate : ICCTouchDelegate
    {
        bool TouchBegan(CCTouch pTouch);

        // optional
        void TouchMoved(CCTouch pTouch);
        void TouchEnded(CCTouch pTouch);
        void TouchCancelled(CCTouch pTouch);
    }
}