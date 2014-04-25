

namespace CocosSharp
{
    public class CCAnimationFrame
    {
		public CCSpriteFrame SpriteFrame { get; private set; }
		public float DelayUnits { get; private set; }
		public PlistDictionary UserInfo { get; private set; }

        #region Constructors

        public CCAnimationFrame(CCSpriteFrame spriteFrame, float delayUnits, PlistDictionary userInfo)
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