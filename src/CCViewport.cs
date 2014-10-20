using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CocosSharp
{
    public class CCViewport
    {
        internal event EventHandler OnViewportChanged;

        CCDisplayOrientation displayOrientation;

        internal CCRect exactFitLandscapeRatio;
        internal CCRect exactFitPortraitRatio;
        CCRect viewportInPixels;

        CCSize landscapeScreenSizeInPixels;

        Viewport xnaViewport;


        #region Properties

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
            CCDisplayOrientation displayOrientationIn=CCDisplayOrientation.LandscapeLeft)
        {
            if(exactFitPortraitRatioIn == default(CCRect))
            {
                exactFitPortraitRatioIn = exactFitLandscapeRatioIn;
            }

            displayOrientation = displayOrientationIn;
            exactFitLandscapeRatio = exactFitLandscapeRatioIn;
            exactFitPortraitRatio = exactFitPortraitRatioIn;
        }

        public CCViewport(
            CCRect exactFitLandscapeRatioIn, 
            CCDisplayOrientation displayOrientationIn=CCDisplayOrientation.LandscapeLeft)
            : this (exactFitLandscapeRatioIn, exactFitLandscapeRatioIn, displayOrientationIn)
        {   }

        internal CCViewport(
            CCRect exactFitLandscapeRatioIn,
            CCDisplayOrientation supportedDisplayOrientationIn,
            CCDisplayOrientation currentDisplayOrientationIn)
            : this(exactFitLandscapeRatioIn, currentDisplayOrientationIn)
        {
        }

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
                (int)Math.Ceiling(exactFitRectRatio.Size.Width * screenSize.Width),
                (int)Math.Ceiling(exactFitRectRatio.Size.Height * screenSize.Height)
            );

            Rectangle xnaViewportRect = exactFitRectPixels;

            xnaViewportRect.X = (int)Math.Floor((exactFitRectPixels.Width - xnaViewportRect.Width) / 2.0f) + exactFitRectPixels.X;
            xnaViewportRect.Y = (int)Math.Floor((exactFitRectPixels.Height - xnaViewportRect.Height) / 2.0f) + exactFitRectPixels.Y;

            viewportInPixels = new CCRect(xnaViewportRect);
            xnaViewport = new Viewport(xnaViewportRect);

            if (dispatchChange)
                OnViewportChanged(this, null);
        }
    }
}

