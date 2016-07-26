using System;
using System.Diagnostics;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI.Xaml;
using Windows.UI.Core;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace CocosSharp
{

    public partial class CCGameView 
    {

        #region Mouse handling

        PointerEventHandler mouseMovedHandler;

        void PlatformUpdateMouseEnabled()
        {
            if (mouseMovedHandler == null)
                mouseMovedHandler = new PointerEventHandler(MouseMoved);

            if (MouseEnabled)
            {
                AddHandler(UIElement.PointerMovedEvent, mouseMovedHandler, true);
            }
            else
            {
                RemoveHandler(UIElement.PointerMovedEvent, mouseMovedHandler);
            }
        }

        void MouseMoved(object sender, PointerRoutedEventArgs args)
        {
            var pointerPoint = args.GetCurrentPoint(this);
            if (pointerPoint.PointerDevice.PointerDeviceType == Windows.Devices.Input.PointerDeviceType.Mouse)
            {
                
                var pos = new CCPoint((float)pointerPoint.Position.X, (float)pointerPoint.Position.Y);

                UpdateIncomingMoveMouse((int)pointerPoint.PointerId, ref pos);

                args.Handled = true;
            }
        }

        #endregion Mouse handling
    }
}