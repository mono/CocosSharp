using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cocos2D
{
    public interface ICCTextFieldDelegate
    {
        /**
        @brief	If the sender doesn't want to attach with IME, return true;
        */
        bool onTextFieldAttachWithIME(CCTextFieldTTF sender);

        /**
        @brief	If the sender doesn't want to detach with IME, return true;
        */
        bool onTextFieldDetachWithIME(CCTextFieldTTF sender);

        /**
        @brief	If the sender doesn't want to insert the text, return true;
        */
        bool onTextFieldInsertText(CCTextFieldTTF sender, string text, int nLen);

        /**
        @brief	If the sender doesn't want to delete the delText, return true;
        */
        bool onTextFieldDeleteBackward(CCTextFieldTTF sender, string delText, int nLen);
        /**
        @brief	If doesn't want draw sender as default, return true.
        */
        bool onDraw(CCTextFieldTTF sender);
    }
}
