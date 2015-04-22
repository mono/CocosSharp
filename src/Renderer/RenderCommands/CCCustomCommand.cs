using System;
using System.Collections.Generic;

namespace CocosSharp
{
    internal class CCCustomCommand : CCRenderCommand
    {
        public Action Action { get; set; }
        public CCCustomCommand(float globalZOrder, CCAffineTransform worldTransform) 
            : base(globalZOrder, worldTransform)
        {
            CommandType = CCRenderer.CCCommandType.Custom;
        }

        public CCCustomCommand(float globalZOrder) 
            : this(globalZOrder, CCAffineTransform.Identity)
        {

        }

        internal override void RequestRenderCommand(CCRenderer renderer)
        {
            if (Action != null)
            {
                Action();
            }

        }

    }
}

