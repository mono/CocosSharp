using System;
using System.Collections.Generic;

namespace CocosSharp
{
    /// <summary>
    /// Interface for nodes that can be focused. 
    /// </summary>
    public interface ICCFocusable
    {
        bool CanReceiveFocus
        {
            get;
        }
        bool HasFocus
        {
            get;
            set;
        }

    }
}
