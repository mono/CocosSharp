
using System;

namespace CocosSharp
{
    /// <summary>
    /// @brief CCMenuItem base class
    /// Subclass CCMenuItem (or any subclass) to create your custom CCMenuItem objects.
    /// </summary>
    public class CCMenuItem : CCNodeRGBA
    {
        #region Properties

        public virtual bool Enabled { get; set; }
        public virtual bool Selected { get; set; }
        public Action<object> Target { get; set; }

        /// <summary>
        /// Returns the outside box
        /// </summary>
        /// <returns></returns>
        public CCRect Rectangle
        {
            get 
            {
                return new CCRect (Position.X - ContentSize.Width * AnchorPoint.X,
                    Position.Y - ContentSize.Height * AnchorPoint.Y,
                    ContentSize.Width,
                    ContentSize.Height);
            }
        }

        protected CCActionState ZoomActionState { get; set; }

        #endregion Properties


        #region Constructors

        public CCMenuItem() : this(null)
        {
        }
            
        public CCMenuItem(Action<object> target)
        {
            AnchorPoint = new CCPoint(0.5f, 0.5f);
            Target = target;
            Enabled = true;
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
        public virtual void RegisterScriptHandler(string pszFunctionName)
        {
            throw new NotImplementedException("CCMenuItem.RegisterScriptHandler is not supported in this version of Cocos2d-XNA");
        }
    }
}