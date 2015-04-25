using System;
using System.Collections.Generic;

namespace CocosSharp
{
    internal class CCCustomCommand : CCRenderCommand
    {
        public Action Action { get; set; }

        #region Constructors

        public CCCustomCommand(float globalZOrder, CCAffineTransform worldTransform) 
            : base(globalZOrder, worldTransform)
        {
        }

        public CCCustomCommand(float globalZOrder) 
            : this(globalZOrder, CCAffineTransform.Identity)
        {
        }

        #endregion Constructors


        internal override void RequestRenderCommand(CCRenderer renderer)
        {
            if(Action != null)
                renderer.ProcessCustomRenderCommand(this);
        }

        internal void RenderCustomCommand()
        {
            Action();
        }
    }
}

