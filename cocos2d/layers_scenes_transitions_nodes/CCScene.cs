using System;
using System.IO;

namespace cocos2d
{
    /// <summary>
    /// brief CCScene is a subclass of CCNode that is used only as an abstract concept.
    /// CCScene an CCNode are almost identical with the difference that CCScene has it's
    /// anchor point (by default) at the center of the screen.
    /// For the moment CCScene has no other logic than that, but in future releases it might have
    /// additional logic.
    ///  It is a good practice to use and CCScene as the parent of all your nodes.
    /// </summary>
    public class CCScene : CCNode
    {
        public CCScene()
        {
            m_bIgnoreAnchorPointForPosition = true;
            AnchorPoint = new CCPoint(0.5f, 0.5f);
        }

        #region Game State Management
        /// <summary>
        /// Gets whether or not this scene is serializable. If this is true,
        /// the screen will be recorded into the director's state and
        /// its Serialize and Deserialize methods will be called as appropriate.
        /// If this is false, the screen will be ignored during serialization.
        /// By default, all screens are assumed to be serializable.
        /// </summary>
        public virtual bool IsSerializable
        {
            get { return m_isSerializable; }
            protected set { m_isSerializable = value; }
        }

        private bool m_isSerializable = true;

        /// <summary>
        /// Tells the screen to serialize its state into the given stream.
        /// </summary>
        public virtual void Serialize(Stream stream) { }

        /// <summary>
        /// Tells the screen to deserialize its state from the given stream.
        /// </summary>
        public virtual void Deserialize(Stream stream) { }


        #endregion
        public bool Init()
        {
            bool bRet = false;
            do
            {
                CCDirector director = CCDirector.SharedDirector;
                if (director == null)
                {
                    break;
                }

                ContentSize = director.WinSize;
                // success
                bRet = true;
            } while (false);
            return bRet;
        }

        public new static CCScene Create()
        {
            var pRet = new CCScene();
            pRet.Init();
            return pRet;
        }
    }
}