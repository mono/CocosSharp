

namespace cocos2d
{
    public class CCPointObject
    {
        public CCNode Child;
        public CCPoint Offset;
        public CCPoint Ratio;

        public static CCPointObject Create(CCPoint ratio, CCPoint offset)
        {
            var pRet = new CCPointObject();
            pRet.InitWithCcPoint(ratio, offset);
            return pRet;
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