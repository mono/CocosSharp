using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace Cocos2D
{
    /// <summary>
    /// Passes the left and right trigger depression value (strength) for the given player.
    /// </summary>
    /// <param name="leftTriggerStrength">Left trigger value</param>
    /// <param name="rightTriggerStrength">Right trigger value</param>
    /// <param name="player">The player to which it pertains</param>
    public delegate void CCGamePadTriggerDelegate(float leftTriggerStrength, float rightTriggerStrength, PlayerIndex player);
}
