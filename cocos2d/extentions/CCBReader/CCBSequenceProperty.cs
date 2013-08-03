using System.Collections.Generic;

namespace Cocos2D
{
    public class CCBSequenceProperty 
    {
        public CCBSequenceProperty()
        {
            _name = "";
            Init();
        }

        private bool Init()
        {
            _keyframes = new List<CCBKeyframe>();
            return true;
        }

        private string _name;
        private CCBPropertyType _type;
        private List<CCBKeyframe> _keyframes;

        public CCBPropertyType Type
        {
            get { return _type; }
            set { _type = value; }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public List<CCBKeyframe> Keyframes
        {
            get { return _keyframes; }
        }
    }
}