using System.Collections.Generic;

namespace Cocos2D
{
    public class CCTMXObjectGroup 
    {
        protected List<Dictionary<string, string>> m_pObjects;
        protected Dictionary<string, string> m_pProperties;

        /// <summary>
        /// name of the group
        /// </summary>
        protected string m_sGroupName;

        protected CCPoint m_tPositionOffset;

        public CCTMXObjectGroup()
        {
            m_pObjects = new List<Dictionary<string, string>>();
            m_pProperties = new Dictionary<string, string>();
        }

        /// <summary>
        /// offset position of child objects
        /// </summary>
        public CCPoint PositionOffset
        {
            get { return m_tPositionOffset; }
            set { m_tPositionOffset = value; }
        }

        /// <summary>
        /// list of properties stored in a dictionary
        /// </summary>
        public Dictionary<string, string> Properties
        {
            get { return m_pProperties; }
            set { m_pProperties = value; }
        }

        /// <summary>
        /// array of the objects
        /// </summary>
        public List<Dictionary<string, string>> Objects
        {
            get { return m_pObjects; }
            set { m_pObjects = value; }
        }

        public string GroupName
        {
            get { return m_sGroupName; }
            set { m_sGroupName = value; }
        }

        /// <summary>
        ///  return the value for the specific property name
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public string PropertyNamed(string propertyName)
        {
            return m_pProperties[propertyName];
        }

        /// <summary>
        ///  return the dictionary for the specific object name.
        ///  It will return the 1st object found on the array for the given name.
        /// </summary>
        /// <param name="objectName"></param>
        /// <returns></returns>
        public Dictionary<string, string> ObjectNamed(string objectName)
        {
            if (m_pObjects != null && m_pObjects.Count > 0)
            {
                for (int i = 0; i < m_pObjects.Count; i++)
                {
                    string name = m_pObjects[i]["name"];
                    if (name != null && name == objectName)
                    {
                        return m_pObjects[i];
                    }
                }
            }
            // object not found
            return null;
        }
    }
}