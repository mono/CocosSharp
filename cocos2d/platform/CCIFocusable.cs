using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cocos2d
{
    /// <summary>
    /// Interface for nodes that can be focused. 
    /// </summary>
    public interface CCIFocusable
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
