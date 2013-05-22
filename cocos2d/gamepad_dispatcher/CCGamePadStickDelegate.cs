using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace Cocos2D
{
    /// <summary>
    /// Mapped from the gamepad game stick status, will tell yoyu when the game
    /// stick is down or up and the direction and magnitude of the stick movement
    /// in [u,v] coordinates.
    /// </summary>
    public struct CCGameStickStatus
    {
        /// <summary>
        /// When true, the stick is down, otherwise it is up.
        /// </summary>
        public bool IsDown;
        /// <summary>
        /// The direction of the stick movement as a unit vector.
        /// </summary>
        public CCPoint Direction;
        /// <summary>
        /// The magnitude of the stick movement, used to control soft or hard movements using
        /// the stick.
        /// </summary>
        public float Magnitude;
    }

    /// <summary>
    /// each time the game pad status is queried, this method will get triggered.
    /// </summary>
    /// <param name="leftStick">The status of the left stick</param>
    /// <param name="rightStick">The status of the right stick</param>
    /// <param name="player">The player to which this pertains</param>
    public delegate void CCGamePadStickUpdateDelegate(CCGameStickStatus leftStick, CCGameStickStatus rightStick, PlayerIndex player);
}
