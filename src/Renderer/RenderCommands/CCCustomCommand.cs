using System;
using System.Collections.Generic;

namespace CocosSharp
{
    public class CCCustomCommand : CCRenderCommand
    {
        public Action Action { get; set; }
        public CCCustomCommand(float globalZOrder, CCAffineTransform worldTransform) 
            : base(globalZOrder, worldTransform)
        {
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

