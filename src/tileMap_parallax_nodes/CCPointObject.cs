

namespace CocosSharp
{
    public class CCPointObject
    {
        public CCNode Child { get; set; }
        public CCPoint Offset { get; set; }
        public CCPoint Ratio { get; set; }


        #region Constructors

        public CCPointObject(CCPoint ratio, CCPoint offset)
        {
            Ratio = ratio;
            Offset = offset;
            Child = null;
        }

        #endregion Constructors
    }
}