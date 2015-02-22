using System;
using System.Collections.Generic;

namespace CocosSharp
{
    public class CCCustomCommand : CCRenderCommand
    {
        public Action Action { get; set; }
        public CCCustomCommand(float globalZOrder, CCAffineTransform modelViewTransform, int flags = 0) 
            : base(globalZOrder, modelViewTransform, flags)
        {
            CommandType = CommandType.CUSTOM_COMMAND;
        }

        public CCCustomCommand(float globalZOrder) 
            : this(globalZOrder, CCAffineTransform.Identity)
        {

        }

        internal override void Execute(CCDrawManager drawManager)
        {
            if (Action != null)
            {
                Action();
            }
        }
    }
}

