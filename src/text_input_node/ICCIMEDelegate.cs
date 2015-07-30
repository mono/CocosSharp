using System;

namespace CocosSharp
{
    public interface ICCIMEDelegate
    {
        /// <summary>
        /// Gets or sets the text field associated with the IME
        /// </summary>
        /// <value>The text field.</value>
        CCTextField TextFieldInFocus { get; set; }

        bool AttachWithIME();
        bool DetachWithIME();

        /// <summary>
        /// Determines whether this instance can attach with IME.
        /// </summary>
        /// <returns><c>true</c> if this instance can attach with IME; otherwise, <c>false</c>.</returns>
        bool CanAttachWithIME();


        /// <summary>
        /// Did instance attach with IME.
        /// </summary>
        /// <returns><c>true</c>, if attach with IME was successful, <c>false</c> otherwise.</returns>
        bool DidAttachWithIME();

        /// <summary>
        /// Determines whether this instance can detach with IME.
        /// </summary>
        /// <returns><c>true</c> if this instance can detach with IME; otherwise, <c>false</c>.</returns>
        bool CanDetachWithIME();

        /// <summary>
        /// Did instance detach with IME.
        /// </summary>
        /// <returns><c>true</c>, if detach with IM was dided, <c>false</c> otherwise.</returns>
        bool DidDetachWithIME();

        /// <summary>
        /// Gets or sets the content text.
        /// </summary>
        /// <value>The content text.</value>
        string ContentText { get; set; }

        /// <summary>
        /// Occurs when the IME interface wants to insert text.
        /// </summary>
        event EventHandler<CCIMEKeybardEventArgs> InsertText;

        /// <summary>
        /// Occurs when IME wants to replace the text for instance those implementations that do not send each keyboard event.
        /// </summary>
        event EventHandler<CCIMEKeybardEventArgs> ReplaceText;

        /// <summary>
        /// Occurs when when user types the back key.
        /// </summary>
        event EventHandler<CCIMEKeybardEventArgs> DeleteBackward;


        #region keyboard show/hide notification

        /// <summary>
        /// Occurs when keyboard will show.
        /// </summary>
        event EventHandler<CCIMEKeyboardNotificationInfo> KeyboardWillShow;
        /// <summary>
        /// Occurs when keyboard did show.
        /// </summary>
        event EventHandler<CCIMEKeyboardNotificationInfo> KeyboardDidShow;
        /// <summary>
        /// Occurs when keyboard will hide.
        /// </summary>
        event EventHandler<CCIMEKeyboardNotificationInfo> KeyboardWillHide;
        /// <summary>
        /// Occurs when keyboard did hide.
        /// </summary>
        event EventHandler<CCIMEKeyboardNotificationInfo> KeyboardDidHide;

        #endregion
    }
}
