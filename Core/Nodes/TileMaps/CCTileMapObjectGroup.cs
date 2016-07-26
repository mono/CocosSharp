using System;
using System.Collections.Generic;

namespace CocosSharp
{
    public class CCTileMapObjectGroup 
    {
        const string ObjectNameKey = "name";


        #region Properties

        public string GroupName { get; set; }
        public CCPoint PositionOffset { get; set; }
        public Dictionary<string, string> Properties { get; set; }
        public List<Dictionary<string, string>> Objects { get; set; }

        #endregion Properties


        #region Constructors

        public CCTileMapObjectGroup()
        {
            Objects = new List<Dictionary<string, string>>();
            Properties = new Dictionary<string, string>();
        }

        #endregion Constructors


        #region Fetching object group data

        public string PropertyNamed(string propertyName)
        {
            return Properties[propertyName];
        }

        public Dictionary<string, string> ObjectNamed(string objectName)
        {
            if (Objects != null)
            {
                foreach (Dictionary<string, string> tmxObjDict in Objects)
                {
                    string name = tmxObjDict[ObjectNameKey];
                    if (name == objectName)
                    {
                        return tmxObjDict;
                    }
                }
            }

            return null;
        }

        #endregion Fetching object group data
    }
}

