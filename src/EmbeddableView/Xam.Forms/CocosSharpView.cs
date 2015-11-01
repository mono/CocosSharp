using System;
using Xamarin.Forms;

namespace CocosSharp
{
    public class CocosSharpView : View
    {
        public enum ViewResolutionPolicy
        {
            ExactFit,       // Fit to entire view. Distortion may occur
            NoBorder,       // Maintain design resolution aspect ratio, but scene may appear cropped
            ShowAll,        // Maintain design resolution aspect ratio, ensuring entire scene is visible
            FixedHeight,    // Use width of design resolution and scale height to aspect ratio of view
            FixedWidth      // Use height of design resolution and scale width to aspect ratio of view 
        }


        public static readonly BindableProperty ViewCreatedProperty =
            BindableProperty.Create<CocosSharpView, EventHandler<EventArgs>> (p => p.ViewCreated, null);

        public static readonly BindableProperty DesignResolutionProperty = 
            BindableProperty.Create<CocosSharpView, Size> (p=> p.DesignResolution, new Size(640, 480));

        public static readonly BindableProperty ResolutionPolicyProperty = 
            BindableProperty.Create<CocosSharpView, ViewResolutionPolicy> (p=> p.ResolutionPolicy, ViewResolutionPolicy.ShowAll);

        public static readonly BindableProperty PausedProperty =
            BindableProperty.Create<CocosSharpView, bool> (p=> p.Paused, false);


        #region Bindable properties

        public EventHandler<EventArgs> ViewCreated 
        {
            get { return (EventHandler<EventArgs>) GetValue (ViewCreatedProperty); }
            set { SetValue (ViewCreatedProperty, value); }
        }

        public ViewResolutionPolicy ResolutionPolicy
        {
            get { return (ViewResolutionPolicy) GetValue (ResolutionPolicyProperty); }
            set { SetValue (ResolutionPolicyProperty, value); }
        }

        public Size DesignResolution
        {
            get { return (Size) GetValue (DesignResolutionProperty); }
            set { SetValue (DesignResolutionProperty, value); }
        }

        public bool Paused
        {
            get { return (bool) GetValue (PausedProperty); }
            set { SetValue (PausedProperty, value); }
        }

        #endregion Bindable properties


        #region Constructors

        public CocosSharpView ()
        {
        }

        #endregion Constructors
    }
}


