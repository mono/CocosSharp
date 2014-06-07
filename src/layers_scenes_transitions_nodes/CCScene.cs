using System;
using System.IO;

namespace CocosSharp
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
        #region Properties

        public virtual bool IsTransition
        {
            get { return false; }
        }

        #endregion Properties


        #region Constructors

        public CCScene() : base()
        {
        }

        #endregion Constructors


        #region Setup content

        protected override void RunningOnNewWindow(CCSize windowSize)
        {
            base.RunningOnNewWindow(windowSize);

            if (Director != null) 
            {
                ContentSize = Director.WindowSizeInPoints;
                IgnoreAnchorPointForPosition = true;
                AnchorPoint = new CCPoint(0.5f, 0.5f);
            }
        }

        #endregion Setup content
    }
}