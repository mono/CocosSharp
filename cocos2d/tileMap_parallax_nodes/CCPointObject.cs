

namespace Cocos2D
{
    public class CCPointObject
    {
        public CCNode Child;
        public CCPoint Offset;
        public CCPoint Ratio;

        public CCPointObject (CCPoint ratio, CCPoint offset)
        {
            InitWithCcPoint(ratio, offset);
        }

        public bool InitWithCcPoint(CCPoint ratio, CCPoint offset)
        {
            Ratio = ratio;
            Offset = offset;
            Child = null;
            return true;
        }
    }
}