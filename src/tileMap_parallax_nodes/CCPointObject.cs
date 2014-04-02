

namespace CocosSharp
{
    public class CCPointObject
    {
        public CCNode Child;
        public CCPoint Offset;
        public CCPoint Ratio;


        #region Constructors

        public CCPointObject(CCPoint ratio, CCPoint offset)
        {
            InitCCPointObject(ratio, offset);
        }

        private void InitCCPointObject(CCPoint ratio, CCPoint offset)
        {
            Ratio = ratio;
            Offset = offset;
            Child = null;
        }

        #endregion Constructors
    }
}