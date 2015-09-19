using System;
using CocosSharp;

namespace CocosSharp
{
    public static class NativeConverters
    {
        public static CCViewResolutionPolicy NativePolicy(this CocosSharpView.ViewResolutionPolicy resPolicy)
        {
            CCViewResolutionPolicy nativePolicy = CCViewResolutionPolicy.ShowAll;

            switch (resPolicy) 
            {
            case CocosSharpView.ViewResolutionPolicy.ExactFit:
                nativePolicy = CCViewResolutionPolicy.ExactFit;
                break;
            case CocosSharpView.ViewResolutionPolicy.FixedHeight:
                nativePolicy = CCViewResolutionPolicy.FixedHeight;
                break;
            case CocosSharpView.ViewResolutionPolicy.FixedWidth:
                nativePolicy = CCViewResolutionPolicy.FixedWidth;
                break;
            case CocosSharpView.ViewResolutionPolicy.NoBorder:
                nativePolicy = CCViewResolutionPolicy.NoBorder;
                break;
            default:
                nativePolicy = CCViewResolutionPolicy.ShowAll;
                break;
            }

            return nativePolicy;
        }

        public static CCSizeI NativeSize(this Xamarin.Forms.Size size)
        {
            return new CCSizeI((int)size.Width, (int)size.Height);
        }
    }  
        
    public partial class CocosSharpViewRenderer
    {            
        public CocosSharpViewRenderer ()
        {
        }

        protected override void OnElementPropertyChanged (object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {

            base.OnElementPropertyChanged (sender, e);
            if (this.Element == null || this.Control == null)
                return;

            else if (e.PropertyName == CocosSharpView.ViewCreatedProperty.PropertyName)
                Control.ViewCreated += Element.ViewCreated;
            else if (e.PropertyName == CocosSharpView.DesignResolutionProperty.PropertyName)
                Control.DesignResolution = Element.DesignResolution.NativeSize ();
            else if (e.PropertyName == CocosSharpView.ResolutionPolicyProperty.PropertyName)
                Control.ResolutionPolicy = Element.ResolutionPolicy.NativePolicy();
        }

        void CommonOnElementChanged(CCGameView nativeView)
        {
            if (Element.ViewCreated != null)
                nativeView.ViewCreated += Element.ViewCreated;

            nativeView.DesignResolution = Element.DesignResolution.NativeSize();
            nativeView.ResolutionPolicy = Element.ResolutionPolicy.NativePolicy();

            SetNativeControl(nativeView);
        }
    }
}

