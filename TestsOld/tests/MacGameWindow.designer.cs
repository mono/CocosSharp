// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoMac.Foundation;
using System.CodeDom.Compiler;

namespace tests
{
	[Register ("MacGameController")]
	partial class MacGameController
	{
		[Outlet]
		CocosSharp.CCGameView gameView { get; set; }

		void ReleaseDesignerOutlets ()
		{
			if (gameView != null) {
				gameView.Dispose ();
				gameView = null;
			}
		}
	}

	[Register ("MacGameWindow")]
	partial class MacGameWindow
	{
		
		void ReleaseDesignerOutlets ()
		{
		}
	}
}
