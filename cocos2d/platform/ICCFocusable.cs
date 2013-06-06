using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cocos2D
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
