using System.Collections.Generic;

namespace CocosSharp
{
    public class CCTMXObjectGroup 
    {
        #region Properties

        public string GroupName { get; set; }

        public CCPoint PositionOffset { get; set; }             // offset position of child objects

        public Dictionary<string, string> Properties { get; set; }
        public List<Dictionary<string, string>> Objects { get; set; }

        #endregion Properties


        #region Constructors

        public CCTMXObjectGroup()
        {
            Objects = new List<Dictionary<string, string>>();
            Properties = new Dictionary<string, string>();
        }

        #endregion Constructors


        public string PropertyNamed(string propertyName)
        {
            return Properties[propertyName];
        }

        public Dictionary<string, string> ObjectNamed(string objectName)
        {
            if (Objects != null && Objects.Count > 0)
            {
                foreach (Dictionary<string, string> tmxObjDict in Objects)
                {
                    string name = tmxObjDict["name"];
                    if (name == objectName)
                    {
                        return tmxObjDict;
                    }
                }
            }

            return null;
        }
    }
}