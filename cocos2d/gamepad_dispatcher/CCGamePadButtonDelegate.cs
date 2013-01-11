using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace cocos2d
{
    /// <summary>
    /// Dispatches button-pressed events, which are discrete press events used primarily for
    /// menu driven applications. if you want to react to button down and up continuity, use the
    /// GamePad.IsButtonPressed and GamePad.IsButtonReleased methods.
    /// </summary>
    public interface CCGamePadButtonDelegate
    {
        // The back button pressed
        void BackButtonPressed(PlayerIndex player);
        void StartButtonPressed(PlayerIndex player);
        void SystemButtonPressed(PlayerIndex player); // XBox button, ouya button
        void AButtonPressed(PlayerIndex player);
        void XButtonPressed(PlayerIndex player);
        void YButtonPressed(PlayerIndex player);
        void BButtonPressed(PlayerIndex player);
    }

    public class CCGamePadButtonHandler : CCObject
    {
        protected CCGamePadButtonDelegate m_pDelegate;

        public CCGamePadButtonDelegate Delegate
        {
            get { return m_pDelegate; }
            set { m_pDelegate = value; }
        }

        /** initializes a CCGamePadHandler with a delegate */

        public virtual bool InitWithDelegate(CCGamePadButtonDelegate pDelegate)
        {
            m_pDelegate = pDelegate;
            return true;
        }

        /** allocates a CCGamePadHandler with a delegate */

        public static CCGamePadButtonHandler HandlerWithDelegate(CCGamePadButtonDelegate pDelegate)
        {
            var pHandler = new CCGamePadButtonHandler();
            pHandler.InitWithDelegate(pDelegate);
            return pHandler;
        }
    }
}