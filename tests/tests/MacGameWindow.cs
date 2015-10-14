
using System;
using System.Collections.Generic;
using System.Linq;
using MonoMac.Foundation;
using MonoMac.AppKit;

using CocosSharp;

namespace tests
{
    public partial class MacGameWindow : MonoMac.AppKit.NSWindow
    {
        #region Constructors

        // Called when created from unmanaged code
        public MacGameWindow(IntPtr handle)
            : base(handle)
        {
            Initialize();
        }
		
        // Called when created directly from a XIB file
        [Export("initWithCoder:")]
        public MacGameWindow(NSCoder coder)
            : base(coder)
        {
            Initialize();
        }
		
        // Shared initialization code
        void Initialize()
        {
        }

        #endregion
       
    }
}

