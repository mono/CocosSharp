

namespace CocosSharp
{
    public class CCAnimationFrame
    {
        public CCSpriteFrame SpriteFrame { get; private set; }
        public float DelayUnits { get; private set; }
        public object UserInfo { get; private set; }

        #region Constructors

        public CCAnimationFrame(CCSpriteFrame spriteFrame, float delayUnits, object userInfo)
        {
            SpriteFrame = spriteFrame;
            DelayUnits = delayUnits;
            UserInfo = userInfo;
        }

        protected CCAnimationFrame(CCAnimationFrame animFrame)
        {
            SpriteFrame = animFrame.SpriteFrame;
            DelayUnits = animFrame.DelayUnits;
            UserInfo = animFrame.UserInfo;
        }
        #endregion Constructors

        public CCAnimationFrame Copy()
        {
            return new CCAnimationFrame(this);
        }
    }
}