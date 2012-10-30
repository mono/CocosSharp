using System.Collections.Generic;

namespace cocos2d
{
    public class CCBSequenceProperty : CCObject
    {
        public string Name;
        public kCCBPropType Type { get; set; }
        public readonly List<CCBKeyframe> Keyframes = new List<CCBKeyframe>();
    }
}