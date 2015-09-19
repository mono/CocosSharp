using System;
using Xamarin.Forms.Platform.iOS;
using Xamarin.Forms;
using CocosSharp;

[assembly:ExportRenderer(typeof(CocosSharpView), typeof(CocosSharpViewRenderer))]
namespace CocosSharp
{
    public partial class CocosSharpViewRenderer : ViewRenderer<CocosSharpView, CCGameView>
    {
        protected override void OnElementChanged (ElementChangedEventArgs<CocosSharpView> e)
        {
            base.OnElementChanged (e);

            if (e.OldElement != null || this.Element == null)
                return;

            var nativeView = new CCGameView (CoreGraphics.CGRect.Empty);

            CommonOnElementChanged(nativeView);
        }
    }
}

