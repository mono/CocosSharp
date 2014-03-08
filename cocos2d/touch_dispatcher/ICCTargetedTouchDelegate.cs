
namespace CocosSharp
{
    public interface ICCTargetedTouchDelegate : ICCTouchDelegate
    {
        bool TouchBegan(CCTouch touch);

        // optional
        void TouchMoved(CCTouch touch);
        void TouchEnded(CCTouch touch);
        void TouchCancelled(CCTouch touch);
    }
}