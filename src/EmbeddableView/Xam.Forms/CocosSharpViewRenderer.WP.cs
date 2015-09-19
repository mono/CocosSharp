using System;
using Xamarin.Forms.Platform.WinRT;
using Xamarin.Forms;
using CocosSharp;

[assembly: ExportRenderer(typeof(CocosSharpView), typeof(CocosSharpViewRenderer))]
namespace CocosSharp
{
    public partial class CocosSharpViewRenderer : ViewRenderer<CocosSharpView, CCGameView>
    {
        protected override void OnElementChanged(ElementChangedEventArgs<CocosSharpView> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null || this.Element == null)
                return;

            var nativeView = new CCGameView();

            CommonOnElementChanged(nativeView);
        }
    }
}
