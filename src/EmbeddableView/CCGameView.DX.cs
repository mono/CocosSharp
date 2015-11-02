using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace CocosSharp
{
    public class GameDesktop 
    {
        protected virtual void Dispose(bool disposing)
        {
        }
    }
    public partial class CCGameView : GameDesktop
    {
        #region Constructors

        public CCGameView()
        {
            Initialise();
        }

        #endregion Constructors


        #region Initialisation

        void PlatformInitialise()
        {
        }

        void PlatformInitialiseGraphicsDevice(ref PresentationParameters presParams)
        {
        }

        void PlatformStartGame()
        {
            viewportDirty = true;
        }

        void InitialiseInputHandling()
        {

        }
        #endregion Initialisation


        #region Cleaning up

        void PlatformDispose(bool disposing)
        {
            
        }

        bool PlatformCanDisposeGraphicsDevice()
        {
            return true;
        }

        #endregion Cleaning up


        #region Rendering

        #endregion Rendering


        #region Run loop

        void PlatformUpdatePaused()
        {

            //MobilePlatformUpdatePaused();
        }

        void OnUpdateFrame(object sender, object e)
        {
            Tick();

            

            Draw();

            PlatformPresent();
        }

        void PlatformPresent()
        {
            if (graphicsDevice != null)
                graphicsDevice.Present();
        }

        void ProcessInput ()
        {

        }

        #endregion Run loop


        #region Touch handling

        //void PlatformUpdateTouchEnabled()
        //{
        //    if (TouchEnabled)
        //    {
        //    }
        //    else
        //    {
        //    }
        //}
        #endregion Touch handling

        #region Mouse Handling
        void PlatformUpdateMouseEnabled ()
        {

        }

        void PlatformUpdateMouseVisible()
        {

        }
        #endregion Mouse Handling

    }
}