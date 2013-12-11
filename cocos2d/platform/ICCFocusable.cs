using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
