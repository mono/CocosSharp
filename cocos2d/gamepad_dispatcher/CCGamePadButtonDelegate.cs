using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace CocosSharp
{
//    /// <summary>
//    /// How the button was engaged. You will get a Pressed notification unless the director
//    /// is configuerd to consume button presses and create the Tapped button status.
//    /// </summary>
//    public enum CCGamePadButtonStatus
//    {
//        Pressed,
//        Released,
//        /// <summary>
//        /// A pressed and released action was merged
//        /// </summary>
//        Tapped,
//        /// <summary>
//        /// Used when one of the buttons reported does not exist on the game pad
//        /// </summary>
//        NotApplicable
//    }

    /// <summary>
    /// The event delegate to handle game pad button state changes. This delegate handles all discrete button
    /// devices on the gamepad. See the CCGamePadTriggerDelegate and CCGamePadStickDelegate and CCGamePadDPadDelegate for the analog
    /// controls.
    /// </summary>
    /// <param name="backButton">State of the back button</param>
    /// <param name="startButton">State of the start button</param>
    /// <param name="systemButton">State of the system (Xbox, PS3, Ouya) button</param>
    /// <param name="aButton">State of the A (bottom) button</param>
    /// <param name="bButton">State of the B (right) button</param>
    /// <param name="xButton">State of the X (left) button</param>
    /// <param name="yButton">State of the Y (top) button</param>
    /// <param name="leftShoulder">State of the left shoulder button</param>
    /// <param name="rightShoulder">State of the right shoulder button</param>
    public delegate void CCGamePadButtonDelegate(CCGamePadButtonStatus backButton, 
                CCGamePadButtonStatus startButton, 
                CCGamePadButtonStatus systemButton,
                CCGamePadButtonStatus aButton, 
                CCGamePadButtonStatus bButton, 
                CCGamePadButtonStatus xButton, 
                CCGamePadButtonStatus yButton, 
                CCGamePadButtonStatus leftShoulder, 
                CCGamePadButtonStatus rightShoulder,
    PlayerIndex player);

}