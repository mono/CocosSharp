using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cocos2D
{
    public interface ICCIMEDelegate
    {

        bool AttachWithIME();
        bool DetachWithIME();

        //friend class CCIMEDispatcher;

        /**
        @brief	Decide the delegate instance is ready for receive ime message or not.

        Called by CCIMEDispatcher.
        */
        bool CanAttachWithIME();
        /**
        @brief	When the delegate detach with IME, this method call by CCIMEDispatcher.
        */
        bool DidAttachWithIME();

        /**
        @brief	Decide the delegate instance can stop receive ime message or not.
        */
        bool CanDetachWithIME();

        /**
        @brief	When the delegate detach with IME, this method call by CCIMEDispatcher.
        */
        bool DidDetachWithIME();

        /**
        @brief	Called by CCIMEDispatcher when some text input from IME.
        */
        void InsertText(string text, int len);

        /**
        @brief	Called by CCIMEDispatcher when user clicked the backward key.
        */
        void DeleteBackward();

        /**
        @brief	Called by CCIMEDispatcher for get text which delegate already has.
        */
        string GetContentText();

        //////////////////////////////////////////////////////////////////////////
        // keyboard show/hide notification
        //////////////////////////////////////////////////////////////////////////
        void KeyboardWillShow(CCIMEKeyboardNotificationInfo info);

        void KeyboardDidShow(CCIMEKeyboardNotificationInfo info);

        void KeyboardWillHide(CCIMEKeyboardNotificationInfo info);

        void KeyboardDidHide(CCIMEKeyboardNotificationInfo info);
    }
}
