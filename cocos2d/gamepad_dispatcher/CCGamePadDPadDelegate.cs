using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace cocos2d
{
    /// <summary>
    /// Called with an update on the current D-Pad status. 
    /// </summary>
    /// <param name="leftButton">The left d-pad button status</param>
    /// <param name="upButton">The up d-pad button status</param>
    /// <param name="rightButton">The right d-pad button status</param>
    /// <param name="downButton">The down d-pad button status</param>
    public delegate void CCGamePadDPadDelegate(CCGamePadButtonStatus leftButton,
    CCGamePadButtonStatus upButton,
    CCGamePadButtonStatus rightButton,
    CCGamePadButtonStatus downButton,
    PlayerIndex player);

}