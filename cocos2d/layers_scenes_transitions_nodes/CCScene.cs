using System;
using System.IO;

namespace cocos2d
{
    /// <summary>
    /// brief CCScene is a subclass of CCNode that is used only as an abstract concept.
    /// CCScene and CCNode are almost identical with the difference that CCScene has it's
    /// anchor point (by default) at the center of the screen. Scenes have state management
    /// where they can serialize their state and have it reconstructed upon resurrection.
    ///  It is a good practice to use and CCScene as the parent of all your nodes.
    /// </summary>
    public class CCScene : CCNode
    {
        /// <summary>
        /// Sets the anchor point to the middle of the scene but ignores the anchor for positioning.
        /// </summary>
        public CCScene()
        {
            m_bIgnoreAnchorPointForPosition = true;
            AnchorPoint = new CCPoint(0.5f, 0.5f);
        }

        /// <summary>
        /// Returns false always unless this is a transition scene.
        /// </summary>
        public virtual bool IsTransition
        {
            get
            {
                return (false);
            }
        }
        /// <summary>
        /// Initialize this scene
        /// </summary>
        /// <returns></returns>
        public virtual bool Init()
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