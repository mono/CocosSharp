using System;
using CocosSharp;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

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

            var nativeView = new CCGameView (Context);

            CommonOnElementChanged(nativeView);
        }
    }
}

