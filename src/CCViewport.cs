using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CocosSharp
{
    #region Enums

    public enum CCViewportResolutionPolicy
    {
        // The entire application is visible in the specified area without trying to preserve the original aspect ratio. 
        // Distortion can occur, and the application may appear stretched or compressed.
        ExactFit,

        // The entire application fills the specified area, without distortion but possibly with some cropping, 
        // while maintaining the original aspect ratio of the application.
        AspectFillLandscape,

        AspectFillPortrait,

        // The entire application is visible in the specified area without distortion while maintaining the original 
        // aspect ratio of the application. Borders can appear on two sides of the application.
        AspectFitLandscape,

        AspectFitPortrait
    }

    #endregion Enums


    public class CCViewport
    {
        internal event EventHandler OnViewportChanged;

        CCViewportResolutionPolicy resolutionPolicy;
        CCDisplayOrientation displayOrientation;

        internal CCRect exactFitLandscapeRatio;
        internal CCRect exactFitPortraitRatio;
        CCRect viewportInPixels;

        CCSize landscapeScreenSizeInPixels;

        Viewport xnaViewport;


        #region Properties

        public CCViewportResolutionPolicy ResolutionPolicy 
        { 
            get { return resolutionPolicy; } 
            set 
            {
                if (resolutionPolicy != value) 
                {
                    resolutionPolicy = value;
                    UpdateViewport();
                }
            }
        }

        public CCRect ExactFitLandscapeRatio
        {
            get { return exactFitLandscapeRatio; }
            set 
            {
                if(exactFitLandscapeRatio != value) 
                {
                    // Ratio can only be between 0.0 and 1.0
                    value.Origin.X = CCMathHelper.Clamp(value.Origin.X, 0.0f, 1.0f);
                    value.Origin.Y = CCMathHelper.Clamp(value.Origin.Y, 0.0f, 1.0f);
                    value.Size.Width = CCMathHelper.Clamp(value.Size.Width, 0.0f, 1.0f);
                    value.Size.Height = CCMathHelper.Clamp(value.Size.Height, 0.0f, 1.0f);

                    exactFitLandscapeRatio = value;
                    UpdateViewport();
                }
            }
        }

        public CCRect ExactFitPortraitRatio
        {
            get { return exactFitPortraitRatio; }
            set 
            {
                if(exactFitPortraitRatio != value) 
                {
                    // Ratio can only be between 0.0 and 1.0
                    value.Origin.X = CCMathHelper.Clamp(value.Origin.X, 0.0f, 1.0f);
                    value.Origin.Y = CCMathHelper.Clamp(value.Origin.Y, 0.0f, 1.0f);
                    value.Size.Width = CCMathHelper.Clamp(value.Size.Width, 0.0f, 1.0f);
                    value.Size.Height = CCMathHelper.Clamp(value.Size.Height, 0.0f, 1.0f);

                    exactFitPortraitRatio = value;
                    UpdateViewport();
                }
            }
        }

        public float AspectRatio
        {
            get { return viewportInPixels.Size.Width / viewportInPixels.Size.Height; }
        }

        internal CCRect ViewportInPixels
        {
            get { return viewportInPixels; }
        }

        internal CCDisplayOrientation DisplayOrientation
        {
            get { return displayOrientation; }
            set 
            {
                if(displayOrientation != value) 
                {
                    displayOrientation = value;
                    UpdateViewport();
                }
            }
        }

        internal CCSize LandscapeScreenSizeInPixels
        {
            set 
            {
                if(landscapeScreenSizeInPixels != value) 
                {
                    landscapeScreenSizeInPixels = value;
                    UpdateViewport();
                }
            }
        }

        internal Viewport XnaViewport
        {
            get { return xnaViewport; }
        }

        #endregion Properties


        #region Constructors

        public CCViewport(
			CCRect exactFitLandscapeRatioIn, CCRect exactFitPortraitRatioIn, 
            CCViewportResolutionPolicy resolutionPolicyIn=CCViewportResolutionPolicy.AspectFitLandscape, 
            CCDisplayOrientation displayOrientationIn=CCDisplayOrientation.LandscapeLeft)
        {
            if(exactFitPortraitRatioIn == default(CCRect))
            {
                exactFitPortraitRatioIn = exactFitLandscapeRatioIn;
            }

            resolutionPolicy = resolutionPolicyIn;
            displayOrientation = displayOrientationIn;
            exactFitLandscapeRatio = exactFitLandscapeRatioIn;
            exactFitPortraitRatio = exactFitPortraitRatioIn;
        }

		public CCViewport(
			CCRect exactFitLandscapeRatioIn, 
            CCViewportResolutionPolicy resolutionPolicyIn=CCViewportResolutionPolicy.AspectFitLandscape, 
			CCDisplayOrientation displayOrientationIn=CCDisplayOrientation.LandscapeLeft)
			: this (exactFitLandscapeRatioIn, exactFitLandscapeRatioIn, resolutionPolicyIn, displayOrientationIn)
		{	}

        #endregion Constructors


        internal void UpdateViewport(bool dispatchChange = true)
        {
            if(landscapeScreenSizeInPixels == CCSize.Zero)
                return;

            bool isPortrat = DisplayOrientation.IsPortrait();

            CCRect exactFitRectRatio = isPortrat ? ExactFitPortraitRatio : ExactFitLandscapeRatio;
            CCSize portraitScreenSize = landscapeScreenSizeInPixels.Inverted;
            CCSize screenSize = isPortrat ? portraitScreenSize : landscapeScreenSizeInPixels;

            Rectangle exactFitRectPixels = new Rectangle (
                (int)(exactFitRectRatio.Origin.X * screenSize.Width), 
                (int)(exactFitRectRatio.Origin.Y * screenSize.Height),
                (int)(exactFitRectRatio.Size.Width * screenSize.Width),
                (int)(exactFitRectRatio.Size.Height * screenSize.Height)
            );

            Rectangle xnaViewportRect = exactFitRectPixels;

            float exactFitAspectRatio = exactFitRectPixels.Width / exactFitRectPixels.Height;
            float screenAspectRatioLandscape = landscapeScreenSizeInPixels.Width / landscapeScreenSizeInPixels.Height;
            float screenAspectRatioPortrait = portraitScreenSize.Width / portraitScreenSize.Height;

            switch(ResolutionPolicy) 
            {
                case CCViewportResolutionPolicy.AspectFitLandscape:
                    // The screen width > screen height    
                    if (screenAspectRatioLandscape > 1.0f) 
                    {
                        xnaViewportRect.Height = (int) (xnaViewportRect.Width / screenAspectRatioLandscape);
                    } 
                    else 
                    {
                        xnaViewportRect.Width = (int) (xnaViewportRect.Height * screenAspectRatioLandscape);
                    }
                    break;
                case CCViewportResolutionPolicy.AspectFitPortrait:
                    // The screen width > screen height    
                    if (screenAspectRatioPortrait > 1.0f) 
                    {
                        xnaViewportRect.Height = (int) (xnaViewportRect.Width / screenAspectRatioPortrait);
                    } 
                    else 
                    {
                        xnaViewportRect.Width = (int) (xnaViewportRect.Height * screenAspectRatioPortrait);
                    }
                    break;
                case CCViewportResolutionPolicy.AspectFillLandscape:
                    // The exact fit viewport width > exact fit viewport height   
                    if (exactFitAspectRatio > 1.0f) 
                    {
                        xnaViewportRect.Height = (int) (xnaViewportRect.Width / screenAspectRatioLandscape);
                    } 
                    else 
                    {
                        xnaViewportRect.Width = (int) (xnaViewportRect.Height * screenAspectRatioLandscape);
                    }    
                    break;
                case CCViewportResolutionPolicy.AspectFillPortrait:
                    // The exact fit viewport width > exact fit viewport height   
                    if (exactFitAspectRatio > 1.0f) 
                    {
                        xnaViewportRect.Height = (int) (xnaViewportRect.Width / screenAspectRatioPortrait);
                    } 
                    else 
                    {
                        xnaViewportRect.Width = (int) (xnaViewportRect.Height * screenAspectRatioPortrait);
                    }    
                    break;
                    // Defaults to exact fit
                default:
                    break;
            }

            xnaViewportRect.X = (int)Math.Floor((exactFitRectPixels.Width - xnaViewportRect.Width) / 2.0f) + exactFitRectPixels.X;
            xnaViewportRect.Y = (int)Math.Floor((exactFitRectPixels.Height - xnaViewportRect.Height) / 2.0f) + exactFitRectPixels.Y;

            viewportInPixels = new CCRect(xnaViewportRect);
            xnaViewport = new Viewport(xnaViewportRect);

            if (dispatchChange)
                OnViewportChanged(this, null);
        }
    }
}

