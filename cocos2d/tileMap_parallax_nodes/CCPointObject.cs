

namespace CocosSharp
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

        private void InitWithCcPoint(CCPoint ratio, CCPoint offset)
        {
            Ratio = ratio;
            Offset = offset;
            Child = null;
        }
    }
}