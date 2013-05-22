using System.Collections.Generic;

namespace cocos2d
{
    public class CCBSequenceProperty 
    {
        public string Name;
        public CCBPropType Type { get; set; }
        public readonly List<CCBKeyframe> Keyframes = new List<CCBKeyframe>();
    }
}