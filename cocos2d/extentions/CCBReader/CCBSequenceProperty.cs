using System.Collections.Generic;

namespace Cocos2D
{
    public class CCBSequenceProperty 
    {
        public string Name;
        public CCBPropType Type { get; set; }
        public readonly List<CCBKeyframe> Keyframes = new List<CCBKeyframe>();
    }
}