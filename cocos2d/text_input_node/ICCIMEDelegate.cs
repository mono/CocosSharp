using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cocos2d
{
    public interface ICCIMEDelegate
    {

        bool attachWithIME();
        bool detachWithIME();

        //friend class CCIMEDispatcher;

        /**
        @brief	Decide the delegate instance is ready for receive ime message or not.

        Called by CCIMEDispatcher.
        */
        bool canAttachWithIME();
        /**
        @brief	When the delegate detach with IME, this method call by CCIMEDispatcher.
        */
        void didAttachWithIME();

        /**
        @brief	Decide the delegate instance can stop receive ime message or not.
        */
        bool canDetachWithIME();

        /**
        @brief	When the delegate detach with IME, this method call by CCIMEDispatcher.
        */
        void didDetachWithIME();

        /**
        @brief	Called by CCIMEDispatcher when some text input from IME.
        */
        void insertText(string text, int len);

        /**
        @brief	Called by CCIMEDispatcher when user clicked the backward key.
        */
        void deleteBackward();

        /**
        @brief	Called by CCIMEDispatcher for get text which delegate already has.
        */
        string getContentText();

        //////////////////////////////////////////////////////////////////////////
        // keyboard show/hide notification
        //////////////////////////////////////////////////////////////////////////
        void keyboardWillShow(CCIMEKeyboardNotificationInfo info);

        void keyboardDidShow(CCIMEKeyboardNotificationInfo info);

        void keyboardWillHide(CCIMEKeyboardNotificationInfo info);

        void keyboardDidHide(CCIMEKeyboardNotificationInfo info);
    }
}
