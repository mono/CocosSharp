
using System;

namespace CocosSharp
{
    /// <summary>
    /// @brief CCMenuItem base class
    /// Subclass CCMenuItem (or any subclass) to create your custom CCMenuItem objects.
    /// </summary>
    public class CCMenuItem : CCNode
    {
        #region Properties

        public virtual bool Enabled { get; set; }
        public virtual bool Selected { get; set; }
        public Action<object> Target { get; set; }

        internal CCActionState ZoomActionState { get; set; }

        #endregion Properties


        #region Constructors

        public CCMenuItem() : this(null)
        {
        }
            
        public CCMenuItem(Action<object> target)
        {
            Target = target;
            Enabled = true;
            AnchorPoint = CCPoint.AnchorMiddle;
        }

        #endregion Constructors

        public virtual void Activate()
        {
            if (Enabled && Target !=null)
            {
                Target(this);
            }
        }
            
        /// <summary>
        /// Register a script function, the function is called in activete
        /// If pszFunctionName is NULL, then unregister it.
        /// </summary>
        /// <param name="pszFunctionName"></param>
        public virtual void RegisterScriptHandler(string functionName)
        {
            throw new NotImplementedException("CCMenuItem.RegisterScriptHandler is not supported in this version of Cocos2d-XNA");
        }
    }
}